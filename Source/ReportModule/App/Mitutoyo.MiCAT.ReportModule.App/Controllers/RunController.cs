// <copyright file="RunController.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.ReportModule.Domain.Data;
using Mitutoyo.MiCAT.ReportModule.Domain.DataResult;
using Mitutoyo.MiCAT.ReportModule.Persistence;
using Mitutoyo.MiCAT.ReportModuleApp.Services.PlansRuns;
using Mitutoyo.MiCAT.ReportModuleApp.Utilities;

namespace Mitutoyo.MiCAT.ReportModuleApp.Controllers
{
   public class RunController : IRunController
   {
      private readonly IMeasurementPersistence _measurementPersistence;
      private readonly IAppStateHistory _history;
      private readonly IMessageNotifier _notifier;
      private readonly IMeasurementDataSourceRefresherService _measurementDataSourceRefresherService;
      private readonly IPlanPersistence _planPersistence;

      public RunController(
         IMeasurementPersistence measurementPersistence,
         IAppStateHistory history,
         IMessageNotifier notifier,
         IMeasurementDataSourceRefresherService measurementDataSourceRefresherService,
         IPlanPersistence planPersistence)
      {
         _measurementPersistence = measurementPersistence;
         _history = history;
         _notifier = notifier;
         _measurementDataSourceRefresherService = measurementDataSourceRefresherService;
         _planPersistence = planPersistence;
      }

      public ISnapShot CompleteRunSelectionOnAppState(ISnapShot snapShot, Guid runIdRequested, RunData runData)
      {
         var currentRunSelectionRequest = snapShot.GetItems<RunSelectionRequest>().Single();

         if (currentRunSelectionRequest.RunRequestedId == runIdRequested && currentRunSelectionRequest.Pending)
         {
            ISnapShot newSnapShot = SelectRun(snapShot, runIdRequested, runData);
            return UpdateRequestRunSelectionAsCompleted(newSnapShot);
         }
         else
            return snapShot;
      }

      public async Task DeleteRun(Guid runId)
      {
         try
         {
            await _measurementPersistence.DeleteMeasurementResultById(runId);
         }
         catch (ResultException ex)
         {
            _notifier.NotifyError(ex);
         }

         var plans = await _planPersistence.GetPlans();
         var runs = await GetRunsForSelectedPlanOrPart();

         _history.Run(snapShot => _measurementDataSourceRefresherService.Refresh(snapShot, plans, runs));
      }

      private Task<IEnumerable<RunDescriptor>> GetRunsForSelectedPlanOrPart()
      {
         var planState = _history.CurrentSnapShot.GetItems<PlansState>().Single();
         return _measurementPersistence.GetRunsForSelectedPlanOrPart(planState.SelectedPlanDataSource);
      }

      private ISnapShot SelectRun(ISnapShot snapShot, Guid runId, RunData newRunData)
      {
         var runSelected = snapShot.GetItems<RunSelection>().Single();
         return snapShot.UpdateItem(runSelected, runSelected.WithNewSelectedRun(runId, newRunData));
      }

      private ISnapShot UpdateRequestRunSelectionAsCompleted(ISnapShot snapShot)
      {
         var runSelectionRequest = snapShot.GetItems<RunSelectionRequest>().Single();
         return snapShot.UpdateItem(runSelectionRequest, runSelectionRequest.WithCompletedRequest());
      }
   }
}
