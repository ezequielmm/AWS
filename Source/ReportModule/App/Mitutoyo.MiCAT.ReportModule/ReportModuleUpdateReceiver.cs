// <copyright file="ReportModuleUpdateReceiver.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using Mitutoyo.MiCAT.ReportModule.GUI;
using Mitutoyo.MiCAT.ReportModule.GUI.Common.Filebar;
using Mitutoyo.MiCAT.ReportModule.Properties;

namespace Mitutoyo.MiCAT.ReportModule
{
   using System.Linq;
   using System.Threading.Tasks;
   using ApplicationState;
   using ApplicationState.Clients;
   using Core;
   using ShellModule;
   using ShellModule.Regions;
   public class ReportModuleUpdateReceiver : IUpdateClient
   {
      private readonly IReportModuleInfo _reportModuleInfo;
      private readonly IReportModuleNavigation _navigation;

      public Name Name { get; set; }

      public ReportModuleUpdateReceiver(IAppStateHistory appStateHistory, IReportModuleInfo reportModuleInfo, IReportModuleNavigation navigation)
      {
         this.Name = Resources.ReportNavigation;
         _reportModuleInfo = reportModuleInfo;
         _navigation = navigation;
         appStateHistory.Subscribe(this);
      }

      public Task Update(ISnapShot snapShot)
      {
         UpdateDeletedReportModuleInfo(snapShot);
         var nl = snapShot.GetItems<NavigationListToSelection>().Single();
         var selecteditem = snapShot.GetItems<SelectedItem>().OrderBy(i => i.Order).LastOrDefault();

         if (selecteditem != null && selecteditem.SourceId == nl.TargetId && selecteditem.TargetId.Equals(_reportModuleInfo.ModuleId))
         {
            _navigation.DeActivateViews(RegionNames.ToolBarRegion);
            _navigation.Navigate(RegionNames.WorkspaceRegion, nameof(ReportWorkspaceView));
         }

         if (selecteditem != null && selecteditem.SourceId == nl.TargetId && _reportModuleInfo.ChildIds.Contains(selecteditem.TargetId))
         {
            var container = _navigation.GetContainer(selecteditem.TargetId);

            _navigation.Navigate<ReportFilebar>(RegionNames.ToolBarRegion, selecteditem.TargetId.ToString(), container);
            _navigation.Navigate<ReportViewWorkspace>(RegionNames.WorkspaceRegion, selecteditem.TargetId.ToString(), container);
         }

         return Task.CompletedTask;
      }

      private void UpdateDeletedReportModuleInfo(ISnapShot snapShot)
      {
         var changeList = snapShot.GetChanges().ItemChanges.OfType<DeleteItemChange>().Where(x => x.ItemType == typeof(NavigationModuleChildInfo));
         foreach (var child in changeList)
         {
            _navigation.RemoveContainer((Id)child.ItemId);
            _reportModuleInfo.RemoveChildItem((Id)child.ItemId);
         }
      }
   }
}