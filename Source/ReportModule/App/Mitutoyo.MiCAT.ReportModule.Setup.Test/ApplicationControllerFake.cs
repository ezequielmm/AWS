// <copyright file="ApplicationControllerFake.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.ReportModule.GUI.Utilities.Events;
using Mitutoyo.MiCAT.ReportModule.Setup.ReportWorkspace;
using Mitutoyo.MiCAT.Utilities.IoC;

namespace Mitutoyo.MiCAT.ReportModule.Setup.Test
{
   [ExcludeFromCodeCoverage]
   public class ApplicationControllerFake : ApplicationController
   {
      private readonly IReportWorkspaceStartUp _reportView;
      public ApplicationControllerFake(IServiceLocator serviceLocator) : base(serviceLocator)
      {
         _reportView = new ReportWorkspaceStartUp(serviceLocator);
      }

      internal async Task<ISnapShot> ExecuteAddFirstSnapShotToHistory(
         SelectedReportTemplateInfo initOptions,
         IAppStateHistory history)
      {
         await _reportView.Start(initOptions);
         return history.CurrentSnapShot;
      }
   }
}
