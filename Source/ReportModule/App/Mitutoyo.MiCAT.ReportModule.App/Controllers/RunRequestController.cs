// <copyright file="RunRequestController.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using System.Linq;
using System.Threading.Tasks;
using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.ReportModule.Domain.Data;

namespace Mitutoyo.MiCAT.ReportModuleApp.Controllers
{
   public class RunRequestController : IRunRequestController
   {
      private readonly IAppStateHistory _history;

      public RunRequestController(IAppStateHistory history)
      {
         _history = history;
      }

      public void RequestRun(Guid runId)
      {
         _history.Run(ss => RequestRun(ss, runId));
      }

      public Task RequestRunAsync(Guid runId)
      {
         return _history.RunAsync(ss => RequestRun(ss, runId));
      }

      private ISnapShot RequestRun(ISnapShot snapShot, Guid runId)
      {
         var currentSelectionRequest = snapShot.GetItems<RunSelectionRequest>().Single();
         var currentRunSelected = snapShot.GetItems<RunSelection>().Single();

         if (currentRunSelected.SelectedRun != runId && !(currentSelectionRequest.RunRequestedId == runId && currentSelectionRequest.Pending))
         {
            var newSnapShot = snapShot.UpdateItem(currentSelectionRequest, currentSelectionRequest.WithNewRequest(runId));

            return newSnapShot;
         }
         else
            return snapShot;
      }

      public ISnapShot UpdateRequestRunSelectionAsCompleted(ISnapShot snapShot)
      {
         var runSelectionRequest = snapShot.GetItems<RunSelectionRequest>().Single();
         return snapShot.UpdateItem(runSelectionRequest, runSelectionRequest.WithCompletedRequest());
      }
   }
}
