// <copyright file="VMMarginSizeSettings.cs" company="Mitutoyo Europe GmbH">
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

namespace Mitutoyo.MiCAT.ReportModule.GUI.Common.Filebar.MarginsSettings
{
   public class VMMarginSizeSettings : VMBase, IUpdateClient, IDisposable
   {
      private readonly ICommonPageLayoutController _commonPageLayoutController;
      private readonly IAppStateHistory _appStateHistory;

      public VMMarginSizeSettings() { } // For testing purpose
      public VMMarginSizeSettings(ICommonPageLayoutController commonPageLayoutController, IMarginSizeList marginSizeList, IAppStateHistory appStateHistory)
      {
         _commonPageLayoutController = commonPageLayoutController;
         _appStateHistory = appStateHistory.AddClient(this);
         UpdateMarginSizeCommand = new RelayCommand<Margin>(OnUpdateMarginSizeCommand);

         SubscribeToAppStateChanges();
         PopulateMarginSizeItems(marginSizeList.MarginSizeInfoList);
      }

      public Name Name => nameof(VMMarginSizeSettings);
      public ICommand UpdateMarginSizeCommand { get; private set; }
      public ObservableCollection<VMMarginSizeSettingsItem> MarginSizeItems { get; private set; }

      public Task Update(ISnapShot snapShot)
      {
         if (snapShot.HasBeenAddedOrUpdated<CommonPageLayout>())
            UpdateSelectedMarginSizeItemFromSnapShot(snapShot);

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

      private void PopulateMarginSizeItems(IEnumerable<Margin> marginSizes)
      {
         MarginSizeItems = new ObservableCollection<VMMarginSizeSettingsItem>(marginSizes.Select(x => new VMMarginSizeSettingsItem(x)));
      }

      private void UpdateSelectedMarginSizeItemFromSnapShot(ISnapShot snapShot)
      {
         var currentMarginSize = snapShot.GetCommonPageLayout().CanvasMargin.MarginKind;

         MarginSizeItems
            .ToList()
            .ForEach(x => x.IsChecked = x.Margin.MarginKind == currentMarginSize);
      }

      private void UpdateIsEnabledFromSnapShot(ISnapShot snapShot)
      {
         var isEnabled = snapShot.IsReportInEditMode();

         MarginSizeItems
            .ToList()
            .ForEach(x => x.IsEnabled = isEnabled);
      }

      private void OnUpdateMarginSizeCommand(Margin selectedMarginSizeInfo)
      {
         _commonPageLayoutController.SetMarginSize(selectedMarginSizeInfo);
      }
   }
}
