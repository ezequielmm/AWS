// <copyright file="ApplicationCommandDelegator.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Threading.Tasks;
using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.Common.GUI.Commands;
using Mitutoyo.MiCAT.ReportModule.ReportModuleManager;
using Mitutoyo.MiCAT.ShellModule;

namespace Mitutoyo.MiCAT.ReportModule
{
   public class ApplicationCommandDelegator : IApplicationCommandDelegator
   {
      private readonly IReportModuleCloseManager _reportModuleCloseManager;
      public ApplicationCommandDelegator(
         IGlobalCommands globalCommands,
         IReportModuleCloseManager reportModuleCloseManager)
      {
         _reportModuleCloseManager = reportModuleCloseManager;

         globalCommands.InstanceCloseCommand
            .RegisterCommand(new AsyncCommand<Id>(CloseWorkspaceInstanceById));
         globalCommands.ExitCommand
            .RegisterCommand(new AsyncCommand(CloseAllWorkspaceInstances, o => !AnyUnsavedChangeOnApplication()));
      }

      private Task CloseWorkspaceInstanceById(Id id)
      {
         return _reportModuleCloseManager.CloseWorkspaceInstanceById(id);
      }
      private Task CloseAllWorkspaceInstances(object args)
      {
         return _reportModuleCloseManager.CloseAllWorkspaceInstances();
      }
      private bool AnyUnsavedChangeOnApplication()
      {
         return _reportModuleCloseManager.AnyUnsavedChangeOnApplication();
      }
   }

   public interface IApplicationCommandDelegator
   {
   }
}
