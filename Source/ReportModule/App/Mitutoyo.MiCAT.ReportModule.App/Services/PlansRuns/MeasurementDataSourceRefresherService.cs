// <copyright file="MeasurementDataSourceRefresherService.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.ReportModule.Domain.Data;

namespace Mitutoyo.MiCAT.ReportModuleApp.Services.PlansRuns
{
   public class MeasurementDataSourceRefresherService : IMeasurementDataSourceRefresherService
   {
      public ISnapShot Refresh(ISnapShot snapShot, IEnumerable<PlanDescriptor> plans)
      {
         return RefreshPlanState(snapShot, plans);
      }

      public ISnapShot Refresh(ISnapShot snapShot, IEnumerable<PlanDescriptor> plans, IEnumerable<RunDescriptor> runs)
      {
         return RefreshPlanState(snapShot, plans, runs);
      }

      public ISnapShot ClearRunSelection(ISnapShot snapShot)
      {
         var actualRunSelection = snapShot.GetItems<RunSelection>().Single();
         snapShot = ClearRunSelectionRequestIfPending(snapShot);
         return snapShot.UpdateItem(actualRunSelection, actualRunSelection.WithNoSelectedRun());
      }

      private ISnapShot RefreshPlanState(ISnapShot snapShot, IEnumerable<PlanDescriptor> plans)
      {
         var plansState = snapShot.GetItems<PlansState>().Single();
         return RefreshPlanState(snapShot, plans, plansState.RunList);
      }

      private ISnapShot RefreshPlanState(ISnapShot snapShot, IEnumerable<PlanDescriptor> plans, IEnumerable<RunDescriptor> runs)
      {
         var plansState = snapShot.GetItems<PlansState>().Single();
         var newPlansState = UpdatePlanState(plansState, plans, runs);

         snapShot = snapShot.UpdateItem(plansState, newPlansState);

         snapShot = UpdateRunSelection(snapShot, newPlansState.RunList);

         return snapShot;
      }

      private PlansState UpdatePlanState(PlansState plansState, IEnumerable<PlanDescriptor> plans, IEnumerable<RunDescriptor> runs)
      {
         IDescriptor newSelectedPlanPart = null;
         IEnumerable<RunDescriptor> runList;

         if (plansState.SelectedPlanDataSource != null)
         {
            newSelectedPlanPart = GetNewSelectedPlanPart(plansState, plans);
            if (newSelectedPlanPart is null)
               runList = Array.Empty<RunDescriptor>();
            else
               runList = runs;
         }
         else
            runList = Array.Empty<RunDescriptor>();

         var newPlansState = plansState.With(plans.ToImmutableList()).With(newSelectedPlanPart, runList.ToImmutableList());

         return newPlansState;
      }

      private IDescriptor GetNewSelectedPlanPart(PlansState plansState, IEnumerable<PlanDescriptor> newPlanList)
      {
         var planParts = newPlanList.SelectMany(p => new IDescriptor[] { p, p.Part });
         return planParts.FirstOrDefault(x => x.Id == plansState.SelectedPlanDataSource.Id);
      }

      private ISnapShot UpdateRunSelection(ISnapShot snapShot, IEnumerable<RunDescriptor> runs)
      {
         var runSelection = snapShot.GetItems<RunSelection>().Single();
         var requestedRunSelection = snapShot.GetItems<RunSelectionRequest>().Single();

         if (RunNotExistOnList(runSelection.SelectedRun, runs))
         {
            snapShot = snapShot.UpdateItem(runSelection, runSelection.WithNoSelectedRun());
         }
         if (requestedRunSelection.Pending && RunNotExistOnList(requestedRunSelection.RunRequestedId, runs))
         {
            snapShot = snapShot.UpdateItem(requestedRunSelection, requestedRunSelection.WithCompletedRequest());
         }

         return snapShot;
      }

      private bool RunNotExistOnList(Guid? selectedRun, IEnumerable<RunDescriptor> runs)
      {
         return selectedRun.HasValue && !runs.Any(r => r.Id == selectedRun.Value);
      }

      private ISnapShot ClearRunSelectionRequestIfPending(ISnapShot snapShot)
      {
         var actualRunSelectionRequest = snapShot.GetItems<RunSelectionRequest>().Single();

         if (actualRunSelectionRequest.Pending)
            snapShot = snapShot.UpdateItem(actualRunSelectionRequest, actualRunSelectionRequest.WithCompletedRequest());

         return snapShot;
      }
   }
}
