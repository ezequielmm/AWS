// <copyright file="VMParts.cs" company="Mitutoyo Europe GmbH">
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
using Mitutoyo.MiCAT.ReportModule.Domain.Data;
using Mitutoyo.MiCAT.ReportModule.Domain.Visibility;
using Mitutoyo.MiCAT.ReportModule.GUI.Common;
using Mitutoyo.MiCAT.ReportModule.GUI.Dialog;
using Mitutoyo.MiCAT.ReportModule.GUI.Navigation.TreeView;
using Mitutoyo.MiCAT.ReportModule.GUI.Properties;
using Mitutoyo.MiCAT.ReportModule.GUI.Utilities;
using Mitutoyo.MiCAT.ReportModuleApp.AppState.Extensions;
using Mitutoyo.MiCAT.ReportModuleApp.Controllers;
using Mitutoyo.MiCAT.ReportModuleApp.Utilities;
using IReporterBusyIndicator = Mitutoyo.MiCAT.ReportModule.GUI.Common.BusyIndicators.IBusyIndicator;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Navigation
{
   public class VMParts : VMBase, IUpdateClient, IDisposable
   {
      private readonly IPlanController _planController;
      private readonly IDialogService _dialogService;
      private readonly IActionCaller _actionCaller;
      private readonly ITreeViewItemListUpdater _treeViewItemListUpdater;
      private IEnumerable<IVMNodeTreeViewItem> _partList;
      private IAppStateHistory _appStateHistory;

      private string _title;
      private bool _isVisible = true;

      public RelayCommand<object> RefreshCommand { get; set; }

      public VMParts(
         IPlanController planController,
         IAppStateHistory appStateHistory,
         IDialogService dialogService,
         IActionCaller actionCaller,
         IReporterBusyIndicator busyIndicator,
         ITreeViewItemListUpdater treeViewItemListUpdater)
      {
         _planController = planController;
         _appStateHistory = appStateHistory.AddClient(this);
         _dialogService = dialogService;
         _actionCaller = actionCaller;
         BusyIndicator = busyIndicator;

         _treeViewItemListUpdater = treeViewItemListUpdater;
         _partList = new List<IVMNodeTreeViewItem>();
         var subscription = new Subscription(this).With(new TypeFilter(typeof(PlansState)), new TypeFilter(typeof(ViewVisibility)));
         _appStateHistory.Subscribe(subscription);

         TreeViewItems = new ObservableCollection<IVMNodeTreeViewItem>();
         RefreshCommand = new RelayCommand<object>(RefreshList);

         InitializeTitle();
      }

      public IReporterBusyIndicator BusyIndicator { get; }

      public async void RefreshList(object obj)
      {
         await _actionCaller.RunUserActionAsync(_planController.RefreshMessurementDataSourceList);
      }

      Name IHasName.Name => nameof(VMParts);

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

      public ObservableCollection<IVMNodeTreeViewItem> TreeViewItems { get; }

      private IEnumerable<IVMNodeTreeViewItem> FlattedTreeViewItems
      {
         get
         {
            return TreeViewItems.SelectMany(item => item.ChildrenVms.Union(new[] { item }));
         }
      }

      public Task Update(ISnapShot snapShot)
      {
         if (TreeViewItemsHasChanged(snapShot))
         {
            UpdateTreeViewItemsFromSnapShot(snapShot);
            UpdateTitle(TreeViewItems.Count);
         }

         UpdateVisibility(snapShot);

         return Task.CompletedTask;
      }

      private void UpdateVisibility(ISnapShot snapShot)
      {
         IsVisible = snapShot.IsViewVisible(ViewElement.Plans);
      }

      private bool TreeViewItemsHasChanged(ISnapShot snapShot)
      {
         var planIdsFromSnapShot = GetPlanIdsFromSnapShot(snapShot).OrderBy(x => x);
         var partIdsFromSnapShot = GetPartIdsFromSnapShot(snapShot).OrderBy(x => x);
         var planIdsFromTreeViewItems = GetPlanIdsFromTreeViewItems().OrderBy(x => x);
         var partIdsFromTreeViewItems = GetPartIdsFromTreeViewItems().OrderBy(x => x);

         return !planIdsFromSnapShot.SequenceEqual(planIdsFromTreeViewItems) ||
            !partIdsFromSnapShot.SequenceEqual(partIdsFromTreeViewItems);
      }

      private void UpdateTreeViewItemsFromSnapShot(ISnapShot snapShot)
      {
         var newTreeViewItems = GenerateTreeViewListFromSnapShot(snapShot);

         _treeViewItemListUpdater.UpdateTreeViewItems(TreeViewItems, newTreeViewItems);
      }

      private void InitializeTitle()
      {
         UpdateTitle(0);
      }

      private void UpdateTitle(int treeViewItemsCount)
      {
         Title = $"{Resources.Program} ({treeViewItemsCount})";
      }

      private IEnumerable<Guid> GetPlanIdsFromSnapShot(ISnapShot snapShot)
      {
         var state = snapShot.GetItems<PlansState>().Single();

         return state
            .PlanList
            .Select(x => x.Id);
      }

      private IEnumerable<Guid> GetPartIdsFromSnapShot(ISnapShot snapShot)
      {
         var state = snapShot.GetItems<PlansState>().Single();

         return state
            .PlanList
            .Where(x => x.Part.Id != Guid.Empty)
            .Select(x => x.Part.Id);
      }

      private IEnumerable<Guid> GetPlanIdsFromTreeViewItems()
      {
         return FlattedTreeViewItems.OfType<VmPlanTreeViewItem>().Select(x => x.Id);
      }

      private IEnumerable<Guid> GetPartIdsFromTreeViewItems()
      {
         return FlattedTreeViewItems.OfType<VmPartTreeViewItem>().Select(x => x.Id);
      }

      private IEnumerable<IVMNodeTreeViewItem> GenerateTreeViewListFromSnapShot(ISnapShot snapShot)
      {
         var state = snapShot.GetItems<PlansState>().Single();

         var treeViewItems =
            state.PlanList
               .GroupBy(x => new { x.Part.Id, x.Part.Name, })
               .OrderBy(x => x.Key.Id == Guid.Empty ? 1 : 0)
               .SelectMany(partGroup =>
               {
                  var part = partGroup.Key;

                  if (part.Id == Guid.Empty)
                  {
                     return partGroup
                        .Select(plan =>
                           new VmPlanTreeViewItem(_planController, _dialogService, _actionCaller)
                           {
                              Id = plan.Id,
                              Name = plan.Name,
                           });
                  }

                  var partItem = new VmPartTreeViewItem(_planController, _actionCaller)
                  {
                     Id = part.Id,
                     Name = part.Name,
                  };

                  partItem.ChildrenVms.AddRange(partGroup
                           .Select(plan =>
                              new VmPlanTreeViewItem(_planController, _dialogService, _actionCaller)
                              {
                                 Id = plan.Id,
                                 Name = plan.Name,
                              })
                           .ToList<IVMNodeTreeViewItem>());

                  return new[] { partItem }.AsEnumerable<IVMNodeTreeViewItem>();
               });

         return treeViewItems.ToList();
      }

      public void Dispose()
      {
         _appStateHistory.RemoveClient(this);
      }
   }
}
