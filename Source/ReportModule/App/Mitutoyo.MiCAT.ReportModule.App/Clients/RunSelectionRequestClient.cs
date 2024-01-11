// <copyright file="RunSelectionRequestClient.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.ApplicationState.Clients;
using Mitutoyo.MiCAT.Core;
using Mitutoyo.MiCAT.ReportModule.Domain.Data;
using Mitutoyo.MiCAT.ReportModule.Domain.DataResult;
using Mitutoyo.MiCAT.ReportModule.Persistence;
using Mitutoyo.MiCAT.ReportModuleApp.Controllers;
using Mitutoyo.MiCAT.ReportModuleApp.Services.PlansRuns;
using Mitutoyo.MiCAT.ReportModuleApp.Utilities;

namespace Mitutoyo.MiCAT.ReportModuleApp.Clients
{
   public class RunSelectionRequestClient : IUpdateClient
   {
      private readonly IMeasurementPersistence _measurementPersistence;
      private readonly IAppStateHistory _appStateHistory;
      private readonly IMessageNotifier _notifier;
      private readonly IRunController _runController;
      private readonly IMeasurementDataSourceRefresherService _measurementDataSourceRefresherService;
      private readonly IPlanPersistence _planPersistence;

      public RunSelectionRequestClient(IMeasurementPersistence measurementPersistence,
         IAppStateHistory appStateHistory,
         IMessageNotifier notifier,
         IRunController runController,
         IMeasurementDataSourceRefresherService measurementDataSourceRefresherService,
         IPlanPersistence planPersistence)
      {
         _measurementPersistence = measurementPersistence;
         _notifier = notifier;
         _runController = runController;
         _measurementDataSourceRefresherService = measurementDataSourceRefresherService;
         _planPersistence = planPersistence;
         _appStateHistory = appStateHistory.AddClient(this);
         _appStateHistory.Subscribe(new Subscription(this).With(new TypeFilter(typeof(RunSelectionRequest))));
      }

      public Name Name => nameof(RunSelectionRequestClient);

      public async Task Update(ISnapShot snapShot)
      {
         try
         {
            var runRequested = snapShot.GetItems<RunSelectionRequest>().Single();
            if (runRequested.Pending)
            {
               var runDetail = await _measurementPersistence.GetRunDetail(runRequested.RunRequestedId.Value);
               _appStateHistory.Run(ss => _runController.CompleteRunSelectionOnAppState(ss, runRequested.RunRequestedId.Value, runDetail));
            }
         }
         catch (ResultException ex)
         {
            _notifier.NotifyError(ex);

            var plans = await _planPersistence.GetPlans();
            var runs = await GetRunsForSelectedPlanOrPart();
            _appStateHistory.Run(ss => _measurementDataSourceRefresherService.Refresh(ss, plans, runs));
         }
      }

      private Task<IEnumerable<RunDescriptor>> GetRunsForSelectedPlanOrPart()
      {
         var planState = _appStateHistory.CurrentSnapShot.GetItems<PlansState>().Single();
         return _measurementPersistence.GetRunsForSelectedPlanOrPart(planState.SelectedPlanDataSource);
      }
   }
}
