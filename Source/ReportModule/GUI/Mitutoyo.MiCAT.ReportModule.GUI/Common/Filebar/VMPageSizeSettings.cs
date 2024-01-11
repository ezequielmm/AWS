// <copyright file="VMPageSizeSettings.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.ApplicationState.Clients;
using Mitutoyo.MiCAT.Core;
using Mitutoyo.MiCAT.ReportModule.App.AppState;
using Mitutoyo.MiCAT.ReportModule.Domain;
using Mitutoyo.MiCAT.ReportModule.Domain.Data;
using Mitutoyo.MiCAT.ReportModule.GUI.Utilities;
using Mitutoyo.MiCAT.ReportModuleApp.AppState.Extensions;
using Mitutoyo.MiCAT.ReportModuleApp.Controllers;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Common.Filebar
{
   public class VMPageSizeSettings : VMBase, IUpdateClient, IDisposable
   {
      private readonly ICommonPageLayoutController _commonPageLayoutController;
      private readonly IAppStateHistory _appStateHistory;

      public VMPageSizeSettings() { } // For testing purpose

      public VMPageSizeSettings(ICommonPageLayoutController commonPageLayoutController, IPageSizeList pageSizeList, IAppStateHistory appStateHistory)
      {
         _commonPageLayoutController = commonPageLayoutController;
         _appStateHistory = appStateHistory.AddClient(this);
         UpdatePageSizeCommand = new RelayCommand<PageSizeInfo>(OnUpdatePageSizeCommand);

         SubscribeToAppStateChanges();
         PopulatePageSizeItems(pageSizeList.PageSizeInfoList);
      }

      public Name Name => nameof(VMPageSizeSettings);
      public ICommand UpdatePageSizeCommand { get; private set; }
      public ObservableCollection<VMPageSizeSettingsItem> PageSizeItems { get; private set; }

      public Task Update(ISnapShot snapShot)
      {
         if (snapShot.HasBeenAddedOrUpdated<CommonPageLayout>())
            UpdateSelectedPageSizeItemFromSnapShot(snapShot);

         if (snapShot.HasBeenAddedOrUpdated<ReportModeState>())
            UpdateIsEnabledFromSnapShot(snapShot);

         return Task.CompletedTask;
      }

      public void Dispose()
      {
         UnsubscribeToAppStateChanges();
      }

      private void SubscribeToAppStateChanges()
      {
         var appStateSubscription = new Subscription(this)
            .With(typeof(CommonPageLayout), typeof(ReportModeState));
         _appStateHistory.Subscribe(appStateSubscription);
      }

      private void UnsubscribeToAppStateChanges()
      {
         _appStateHistory.RemoveClient(this);
      }

      private void PopulatePageSizeItems(IEnumerable<PageSizeInfo> pageSizes)
      {
         PageSizeItems = new ObservableCollection<VMPageSizeSettingsItem>(pageSizes.Select(x => new VMPageSizeSettingsItem(x)));
      }

      private void UpdateSelectedPageSizeItemFromSnapShot(ISnapShot snapShot)
      {
         var selectedPaperKind = snapShot.GetCommonPageLayout().PageSize.PaperKind;

         PageSizeItems
            .ToList()
            .ForEach(x => x.IsChecked = x.PageSizeInfo.PaperKind == selectedPaperKind);
      }

      private void UpdateIsEnabledFromSnapShot(ISnapShot snapShot)
      {
         var isEnabled = snapShot.IsReportInEditMode();

         PageSizeItems
            .ToList()
            .ForEach(x => x.IsEnabled = isEnabled);
      }

      private void OnUpdatePageSizeCommand(PageSizeInfo selectedPageSizeInfo)
      {
         _commonPageLayoutController.SetPageSize(selectedPageSizeInfo);
      }
   }
}
