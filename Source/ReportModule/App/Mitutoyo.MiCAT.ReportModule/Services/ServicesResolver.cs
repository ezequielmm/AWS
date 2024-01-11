// <copyright file="ServicesResolver.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.Common.Help;
using Mitutoyo.MiCAT.ReportModule.ReportModuleManager;
using Mitutoyo.MiCAT.ReportModuleApp.Utilities;
using Mitutoyo.MiCAT.ShellModule;
using Unity;

namespace Mitutoyo.MiCAT.ReportModule.Services
{
   public static class ServicesResolver
   {
      public static IHelpService ResolveHelpServiceFromShellContainer(IUnityContainer shellUnityContainer)
      {
         var shellAppHistory = shellUnityContainer.Resolve<IAppStateHistory>();
         var shellHelpTopicCOntroller = shellUnityContainer.Resolve<IHelpTopicController>();
         var shellHelpService = new ShellHelpService(shellAppHistory, shellHelpTopicCOntroller);

         return shellHelpService;
      }

      public static ICloseService ResolveCloseServiceFromShellContainer(IUnityContainer shellUnityContainer, Id<NavigationModuleChildInfo> containerId)
      {
         var reportModuleCloseManager = shellUnityContainer.Resolve<IReportModuleCloseManager>();
         return new ShellCloseService(reportModuleCloseManager, containerId);
      }
   }
}
