// <copyright file="VMRuns.cs" company="Mitutoyo Europe GmbH">
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
using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.ApplicationState.Clients;
using Mitutoyo.MiCAT.Core;
using Mitutoyo.MiCAT.Core.StdLib.Extensions;
using Mitutoyo.MiCAT.ReportModule.Domain.Data;
using Mitutoyo.MiCAT.ReportModule.Domain.Visibility;
using Mitutoyo.MiCAT.ReportModule.GUI.Common;
using Mitutoyo.MiCAT.ReportModule.GUI.Dialog;
using Mitutoyo.MiCAT.ReportModule.GUI.Navigation.TreeView;
using Mitutoyo.MiCAT.ReportModule.GUI.Properties;
using Mitutoyo.MiCAT.ReportModuleApp.AppState.Extensions;
using Mitutoyo.MiCAT.ReportModuleApp.Controllers;
using Mitutoyo.MiCAT.ReportModuleApp.Utilities;
using IReportBusyIndicator = Mitutoyo.MiCAT.ReportModule.GUI.Common.BusyIndicators.IBusyIndicator;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Navigation
{
   public class VMRuns : VMBase, IUpdateClient, IDisposable
   {
      private readonly IRunController _runController;
      private readonly IRunRequestController _runRequestController;
      private readonly IDialogService _dialogService;
      private readonly IActionCaller _actionCaller;
      private readonly IAppStateHistory _appStateHistory;
      private readonly ITreeViewItemListUpdater _treeViewItemListUpdater;
      private string _title;
      private bool _isVisible = true;

      public VMRuns(
         IRunController runController,
         IRunRequestController runRequestController,
         IAppStateHistory appStateHistory,
         IDialogService dialogService,
         IActionCaller actionCaller,
         IReportBusyIndicator busyIndicator,
         ITreeViewItemListUpdater treeViewItemListUpdater)
      {
         _runController = runController;
         _runRequestController = runRequestController;
         _appStateHistory = appStateHistory.AddClient(this);
         _dialogService = dialogService;
         _actionCaller = actionCaller;
         _treeViewItemListUpdater = treeViewItemListUpdater;
         BusyIndicator = busyIndicator;

         _title = Resources.Runs;

         var appClientSubscription = new Subscription(this).With(new TypeFilter(typeof(PlansState)), new TypeFilter(typeof(RunSelectionRequest)), new TypeFilter(typeof(ViewVisibility)));
         _appStateHistory.Subscribe(appClientSubscription);

         Runs = new ObservableCollection<IVMNodeTreeViewItem>();
      }

      public IReportBusyIndicator BusyIndicator { get; }
      Name IHasName.Name => nameof(VMRuns);

      public ObservableCollection<IVMNodeTreeViewItem> Runs { get; }

      public string Title
      {
         get { return _title; }

         set
         {
            _title = value;
            RaisePropertyChanged();
         }
      }

      public bool IsVisible
      {
         get => _isVisible;
         set
         {
            if (_isVisible == value) return;
            _isVisible = value;

            RaisePropertyChanged();
         }
      }

      public Task Update(ISnapShot snapShot)
      {
         if (TreeViewItemsHasChanged(snapShot))
         {
            UpdateTreeViewItemsFromSnapShot(snapShot);
         }

         UpdateVisibility(snapShot);

         UpdateLoadingRun(snapShot);

         return Task.CompletedTask;
      }

      private void UpdateLoadingRun(ISnapShot snapShot)
      {
         if (snapShot.GetChanges().Any(c => c.ItemType == typeof(RunSelectionRequest)))
         {
            var requestedRun = snapShot.GetItems<RunSelectionRequest>().Single();

            if (requestedRun.RunRequestedId is null)
               return;

            Runs.ForEach(r => r.IsLoading = (requestedRun.Pending && r.Id == requestedRun.RunRequestedId));
         }
      }

      private void UpdateVisibility(ISnapShot snapShot)
      {
         IsVisible = snapShot.IsViewVisible(ViewElement.Runs);
      }

      private void UpdateTreeViewItemsFromSnapShot(ISnapShot snapShot)
      {
         var plansState = snapShot.GetItems<PlansState>().Single();

         var newElements = GenerateTreeViewFromRunsState(plansState);
         _treeViewItemListUpdater.UpdateTreeViewItems(Runs, newElements);
      }

      private bool TreeViewItemsHasChanged(ISnapShot snapShot)
      {
         var runIdsFromSnapShot = GetRunIdsFromSnapShot(snapShot).OrderBy(x => x);
         var runIdsFromTreeViewItems = GetRunIdsFromTreeViewItems().OrderBy(x => x);

         return !runIdsFromSnapShot.SequenceEqual(runIdsFromTreeViewItems);
      }

      private IEnumerable<Guid> GetRunIdsFromSnapShot(ISnapShot snapShot)
      {
         var state = snapShot.GetItems<PlansState>().First();

         return state
            .RunList
            .Select(x => x.Id);
      }

      private IEnumerable<Guid> GetRunIdsFromTreeViewItems()
      {
         return Runs.Select(x => x.Id);
      }

      private IEnumerable<IVMNodeTreeViewItem> GenerateTreeViewFromRunsState(PlansState currentPlansState)
      {
         var treeView = new List<IVMNodeTreeViewItem>();
         foreach (var nodeDescriptor in currentPlansState.RunList.Select(CreateNodeFromRun))
         {
            treeView.Add(nodeDescriptor);
         }
         return treeView;
      }

      private IVMNodeTreeViewItem CreateNodeFromRun(RunDescriptor run)
      {
         return new VmRunTreeViewItem(_runController, _runRequestController, _dialogService, _actionCaller)
         {
            Id = run.Id,
            Name = run.TimeStamp.Value.ToString(),
         };
      }

      public void Dispose()
      {
         _appStateHistory.RemoveClient(this);
      }
   }
}
