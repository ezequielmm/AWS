// <copyright file="ReportFileBarController.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Linq;
using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.ReportModule.App.AppState;

namespace Mitutoyo.MiCAT.ReportModuleApp.Controllers
{
   public class ReportFileBarController : IReportFileBarController
   {
      private readonly IAppStateHistory _history;

      public ReportFileBarController(IAppStateHistory history)
      {
         _history = history;
      }

      public void ChangeStateToggleButton(bool toggleButton)
      {
         _history.Run(snapShot => ChangeStateToggleButton(snapShot, toggleButton));
      }
      private ISnapShot ChangeStateToggleButton(ISnapShot snapShot, bool toggleButton)
      {
         var reportModeState = snapShot.GetItems<ReportModeState>().Single();
         if (reportModeState.EditMode == toggleButton)
            return snapShot;
         else
            return snapShot.UpdateItem(reportModeState, reportModeState.With(toggleButton));
      }
   }
}
