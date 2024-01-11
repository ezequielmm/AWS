// <copyright file="PlanController.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.ReportModule.Domain.Data;
using Mitutoyo.MiCAT.ReportModule.Domain.DataResult;
using Mitutoyo.MiCAT.ReportModule.Domain.DataResult.Exceptions;
using Mitutoyo.MiCAT.ReportModule.Persistence;
using Mitutoyo.MiCAT.ReportModuleApp.Services.PlansRuns;
using Mitutoyo.MiCAT.ReportModuleApp.Utilities;

namespace Mitutoyo.MiCAT.ReportModuleApp.Controllers
{
   public class PlanController : IPlanController
   {
      private readonly IAppStateHistory _history;
      private readonly IPartPersistence _partPersistence;
      private readonly IPlanPersistence _planPersistence;
      private readonly IMeasurementPersistence _measurementPersistence;
      private readonly IMessageNotifier _notifier;
      private readonly IMeasurementDataSourceRefresherService _measurementDataSourceRefresherService;

      public PlanController(
         IAppStateHistory history,
         IMeasurementPersistence measurementPersistence,
         IPartPersistence partPersistence,
         IPlanPersistence planPersistence,
         IMessageNotifier notifier,
         IMeasurementDataSourceRefresherService measurementDataSourceRefresherService)
      {
         _history = history;
         _measurementPersistence = measurementPersistence;
         _partPersistence = partPersistence;
         _planPersistence = planPersistence;
         _notifier = notifier;
         _measurementDataSourceRefresherService = measurementDataSourceRefresherService;
      }

      public async Task SelectPlan(Guid planId)
      {
         var planWasSelected = await TryOperation(SelectPlanOperation, planId);
         if (!planWasSelected)
         {
            ClearPlanDataSourceSelection();
            await RefreshPlanList();
         }
      }

      public async Task SelectPart(Guid partId)
      {
         var partWasSelected = await TryOperation(SelectPartOperation, partId);
         if (!partWasSelected)
         {
            ClearPlanDataSourceSelection();
            await RefreshPlanList();
         }
      }

      private async Task RefreshPlanList()
      {
         var plans = await _planPersistence.GetPlans();

         _history.Run(snapShot => _measurementDataSourceRefresherService.Refresh(snapShot, plans));
      }
      public async Task RefreshMessurementDataSourceList()
      {
         var plans = _planPersistence.GetPlans();
         var runs = GetRunsForSelectedPlanOrPart();

         await Task.WhenAll(plans, runs);

         _history.Run(snapShot => _measurementDataSourceRefresherService.Refresh(snapShot, plans.Result, runs.Result));
      }

      public async Task DeletePlan(Guid planId)
      {
         await TryOperation(_planPersistence.DeletePlan, planId);
         await RefreshPlanList();
      }

      public async Task DeletePart(Guid partId)
      {
         await TryOperation(_partPersistence.DeletePart, partId);
         await RefreshPlanList();
      }

      private Task<IEnumerable<RunDescriptor>> GetRunsForSelectedPlanOrPart()
      {
         var planState = _history.CurrentSnapShot.GetItems<PlansState>().Single();
         return _measurementPersistence.GetRunsForSelectedPlanOrPart(planState.SelectedPlanDataSource);
      }

      private void ClearPlanDataSourceSelection()
      {
         var plansState = _history.CurrentSnapShot.GetItems<PlansState>().SingleOrDefault();
         _history.Run(snapShot => snapShot.UpdateItem(plansState, plansState.WithClearedSelection()));
      }

      private async Task SelectPlanOperation(Guid planId)
      {
         var plansState = _history.CurrentSnapShot.GetItems<PlansState>().SingleOrDefault();

         if (plansState.SelectedPlanDataSource == null || planId != plansState.SelectedPlanDataSource.Id)
         {
            var plan = plansState.PlanList.First(x => x.Id == planId);

            await _planPersistence.CheckIfPlanExist(planId);

            var runs = await GetAssociatedRuns(plan);
            _history.Run(snapShot => SelectPlanOperation(snapShot, planId, runs));
         }
      }

      private ISnapShot SelectPlanOperation(ISnapShot snapShot, Guid planId, IEnumerable<RunDescriptor> runs)
      {
         var plansState = snapShot.GetItems<PlansState>().SingleOrDefault();
         var plan = plansState.PlanList.First(x => x.Id == planId);
         snapShot = _measurementDataSourceRefresherService.ClearRunSelection(snapShot);
         return snapShot.UpdateItem(plansState, plansState.WithClearedSelection().With(plan, runs.ToImmutableList()));
      }

      private async Task SelectPartOperation(Guid partId)
      {
         var snapShot = _history.CurrentSnapShot;
         var plansState = snapShot.GetItems<PlansState>().SingleOrDefault();

         if (plansState.SelectedPlanDataSource == null || partId != plansState.SelectedPlanDataSource.Id)
         {
            var part = plansState.PlanList.Select(x => x.Part).First(x => x.Id == partId);

            await CheckIfPartHasPlans(partId);

            var runs = await GetAssociatedRuns(part);
            _history.Run(snapshot => SelectPartOperation(snapshot, partId, runs));
         }
      }

      private async Task CheckIfPartHasPlans(Guid partId)
      {
         var plansForPart = await GetPlansForPart(partId);
         if (plansForPart.Count() == 0)
            throw new PartNotFoundException(string.Empty);
      }

      private ISnapShot SelectPartOperation(ISnapShot snapShot, Guid partId, IEnumerable<RunDescriptor> runs)
      {
         var plansState = snapShot.GetItems<PlansState>().SingleOrDefault();
         var part = plansState.PlanList.Select(x => x.Part).First(x => x.Id == partId);
         snapShot = _measurementDataSourceRefresherService.ClearRunSelection(snapShot);
         return snapShot.UpdateItem(plansState, plansState.WithClearedSelection().With(part, runs.ToImmutableList()));
      }

      private async Task<IEnumerable<RunDescriptor>> GetAssociatedRuns(PlanDescriptor plan)
      {
         return await _measurementPersistence.GetRunsByPlanId(plan.Id);
      }

      private Task<IEnumerable<RunDescriptor>> GetAssociatedRuns(PartDescriptor part)
      {
         return _measurementPersistence.GetRunsByPartId(part.Id);
      }

      private Task<IEnumerable<PlanDescriptor>> GetPlansForPart(Guid partId)
      {
         return _planPersistence.GetPlansForPart(partId);
      }

      private async Task<bool> TryOperation(Func<Guid, Task> operationMethod, Guid id)
      {
         bool successful = false;
         try
         {
            await operationMethod(id);
            successful = true;
         }
         catch (ResultException ex)
         {
            _notifier.NotifyError(ex);
         }

         return successful;
      }
   }
}
