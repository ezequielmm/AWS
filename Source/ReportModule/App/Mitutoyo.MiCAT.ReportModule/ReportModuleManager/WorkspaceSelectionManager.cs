// <copyright file="WorkspaceSelectionManager.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Linq;
using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.ShellModule;

namespace Mitutoyo.MiCAT.ReportModule.ReportModuleManager
{
   public class WorkspaceSelectionManager : IWorkspaceSelectionManager
   {
      public ISnapShot DeleteWorkspacecFromSelection(ISnapShot snapShot, NavigationModuleChildInfo deletedWorkspace)
      {
         if (snapShot.GetItems<SelectedItem>().Any(s=>s.TargetId == deletedWorkspace.Id))
         {
            var nextId = GetNextSelection(snapShot, deletedWorkspace.Id);
            snapShot = UnselectWorkspace(snapShot, snapShot.GetItems<SelectedItem>().Single(s => s.TargetId == deletedWorkspace.Id));
            snapShot = SelectNextWorkspace(snapShot, nextId);
         }
         return snapShot;
      }
      private ISnapShot UnselectWorkspace(ISnapShot snapShot, SelectedItem selectedItem)
      {
         snapShot = snapShot.DeleteItem(selectedItem);
         return snapShot;
      }
      private ISnapShot SelectNextWorkspace(ISnapShot snapShot, Id nextSelectedId)
      {
         var navigationList = snapShot.GetItems<NavigationList>().Single();
         var nts = snapShot.GetItems<NavigationListToSelection>().Single(i => i.SourceId.Equals(navigationList.Id));
         var selection = snapShot.GetItem(nts.TargetId);

         var selectedItem = new SelectedItem(snapShot.UniqueValue, selection.Id, nextSelectedId, 0);

         return snapShot.AddItem(selectedItem);
      }
      private Id GetNextSelection(ISnapShot snapShot, Id currentSelectionId)
      {
         var parentId = GetParent(currentSelectionId, snapShot);

         var childItems = snapShot.GetItems<NavigationModuleToChild>()
            .Where(item => item.SourceId.Equals(parentId))
            .OrderBy(o => o.Order)
            .Select(child => snapShot.GetItem(child.TargetId))
            .ToList();

         var currentSelection = (NavigationModuleChildInfo)snapShot.GetItem(currentSelectionId);

         var selectionId = parentId;
         var index = childItems.IndexOf(currentSelection);

         while (index > 0)
         {
            index = index - 1;
            var item = childItems.ElementAt(index);
            if (!IsItemInPopoutWindow(snapShot, item.Id))
            {
               selectionId = item.Id;
               break;
            }
         }

         return selectionId;
      }
      private bool IsItemInPopoutWindow(ISnapShot snapShot, Id itemId)
      {
         var popupTargets = snapShot.GetItems<PopupSelectionAssociation>().Select(item => item.TargetId).ToList();
         return popupTargets.Contains(itemId);
      }
      private Id GetParent(Id childId, ISnapShot snapShot)
      {
         var parent = snapShot.GetItems<NavigationModuleToChild>().Single(x => x.TargetId == childId);
         return parent.SourceId;
      }
   }
}
