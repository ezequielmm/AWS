// <copyright file="ShellCloseService.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.ReportModule.ReportModuleManager;
using Mitutoyo.MiCAT.ReportModuleApp.Utilities;
using Mitutoyo.MiCAT.ShellModule;

namespace Mitutoyo.MiCAT.ReportModule.Services
{
   public class ShellCloseService : ICloseService
   {
      private readonly IReportModuleCloseManager _reportModuleCloseManager;
      private readonly Id<NavigationModuleChildInfo> _containerId;
      public ShellCloseService(IReportModuleCloseManager reportModuleCloseManager, Id<NavigationModuleChildInfo> containerId)
      {
         _reportModuleCloseManager = reportModuleCloseManager;
         _containerId = containerId;
      }
      public void CloseInstance()
      {
         _reportModuleCloseManager.CloseWorkspaceInstanceById(_containerId);
      }
   }
}
