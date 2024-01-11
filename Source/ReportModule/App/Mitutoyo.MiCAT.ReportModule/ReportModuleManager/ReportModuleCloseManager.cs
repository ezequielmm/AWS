// <copyright file="ReportModuleCloseManager.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Linq;
using System.Threading.Tasks;
using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.Common.Application.Services.ApplicationExit;
using Mitutoyo.MiCAT.ReportModuleApp.AppState.Extensions;
using Mitutoyo.MiCAT.ReportModuleApp.Manager;
using Mitutoyo.MiCAT.ShellModule;

namespace Mitutoyo.MiCAT.ReportModule.ReportModuleManager
{
   public class ReportModuleCloseManager : IReportModuleCloseManager
   {
      private readonly IAppStateHistory _history;
      private readonly IWorkspaceSelectionManager _workspaceSelectionManager;
      private readonly IWorskpaceContainerResolver _workspaceContainerResolver;
      private readonly IReportModuleInfo _moduleInfo;
      public ReportModuleCloseManager(IAppStateHistory history, IWorkspaceSelectionManager workspaceSelectionManager,
         IWorskpaceContainerResolver worskpaceContainerResolver, IReportModuleInfo moduleInfo)
      {
         _history = history;
         _workspaceSelectionManager = workspaceSelectionManager;
         _moduleInfo = moduleInfo;
         _workspaceContainerResolver = worskpaceContainerResolver;
      }
      private bool ExistsApplicationExitRequest =>
         _history.CurrentSnapShot.HasBeenAdded<ApplicationExitRequest>();
      public bool AnyUnsavedChangeOnApplication()
      {
         var anyUnsavedChange = _moduleInfo.ChildIds.Any(id => HasUnsavedChange(id));
         return anyUnsavedChange;
      }
      private bool HasUnsavedChange(Id id)
      {
         var workspaceController = _workspaceContainerResolver.GetWorkspaceController(id);

         return (workspaceController != null && workspaceController.HasUnsavedChanges());
      }
      public async Task CloseAllWorkspaceInstances()
      {
         await _history.RunAsync(snapShot => CloseAllWorkspaceInstances(snapShot));
      }
      private ISnapShot CloseAllWorkspaceInstances(ISnapShot snapShot)
      {
         foreach (var childId in _moduleInfo.ChildIds)
         {
            var deleteItem = snapShot.GetItems<NavigationModuleChildInfo>()
               .Single(x => x.Id.Equals(childId));

            snapShot = snapShot.DeleteItem(deleteItem);
         }
         return snapShot;
      }

      public async Task CloseWorkspaceInstanceById(Id workspaceId)
      {
         if (workspaceId == null)
            return;

         if (!_moduleInfo.ChildIdExists(workspaceId))
            return;

         if (ExistsApplicationExitRequest)
            return;

         var workspaceController = _workspaceContainerResolver.GetWorkspaceController(workspaceId);
         if (await workspaceController.OnWorkspaceClosing() == WorkspaceClosingResult.ContinueClosing)
         {
            await RemoveContainerById(workspaceId);
         }
      }

      private async Task RemoveContainerById(Id workspaceId)
      {
         if (workspaceId == null)
            return;

         await _history.RunAsync((snapshot) => RemoveContainerById(snapshot, workspaceId));
      }
      private ISnapShot RemoveContainerById(ISnapShot snapShot, Id workspaceId)
      {
         var deleteItem = snapShot.GetItems<NavigationModuleChildInfo>()
            .Single(x => x.Id.Equals(workspaceId));
         snapShot = _workspaceSelectionManager.DeleteWorkspacecFromSelection(snapShot, deleteItem);
         return snapShot.DeleteItem(deleteItem);
      }
   }
}
