// <copyright file="VMReportViewWorkspace.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.ApplicationState.Clients;
using Mitutoyo.MiCAT.Common.GUI.Commands;
using Mitutoyo.MiCAT.Core;
using Mitutoyo.MiCAT.Core.StdLib.Extensions;
using Mitutoyo.MiCAT.ReportModule.App.AppState;
using Mitutoyo.MiCAT.ReportModule.Domain.Data;
using Mitutoyo.MiCAT.ReportModule.GUI.Common;
using Mitutoyo.MiCAT.ReportModule.GUI.Common.BusyIndicators;
using Mitutoyo.MiCAT.ReportModule.GUI.MainWindow;
using Mitutoyo.MiCAT.ReportModule.GUI.MainWindow.Zoom;
using Mitutoyo.MiCAT.ReportModule.GUI.Properties;
using Mitutoyo.MiCAT.ReportModule.GUI.Utilities;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements;
using Mitutoyo.MiCAT.ReportModuleApp.AppState.Extensions;
using Mitutoyo.MiCAT.ReportModuleApp.Controllers;
using Mitutoyo.MiCAT.ReportModuleApp.Utilities;
using Telerik.Windows.Controls.Docking;

namespace Mitutoyo.MiCAT.ReportModule.GUI
{
   public class VMReportViewWorkspace : VMBase, IUpdateClient, IDisposable
   {
      private readonly IAppStateHistory _appStateHistory;
      private readonly IViewVisibilityController _viewVisibilityController;
      private readonly IUnselectionArea _unselectionArea;
      private readonly IActionCaller _actionCaller;
      private readonly ISelectedComponentController _selectedComponentController;
      private readonly ISelectedSectionController _selectedSectionController;
      private readonly IUndoRedoController _undoRedoController;

      private bool _editMode;
      private bool _allowDragDrop;
      private bool _allowZoom;

      private readonly IViewToVMReportComponent _viewToVMReportComponent;
      private readonly IDeleteComponentController _deleteComponentController;

      public VMReportViewWorkspace(IViewToVMReportComponent viewToVMReportComponent,
         IPagesRenderer pageLayoutCalculator,
         IDeleteComponentController deleteController,
         ISelectedComponentController selectedComponentController,
         ISelectedSectionController selectedSectionController,
         IUndoRedoController undoRedoController,
         IBusyIndicator busyIndicator,
         IActionCaller actionCaller,
         IAppStateHistory appStateHistory,
         IVMZoomFactor zoomFactor,
         VMReportView vMReportView,
         IReportModeProperty reportModeProperty,
         IViewVisibilityController viewVisibilityController,
         IUnselectionArea unselectionArea)
      {
         DataSourcePanelTitle = CreateDataSourcePanelTitleText();
         _deleteComponentController = deleteController;
         _selectedComponentController = selectedComponentController;
         _selectedSectionController = selectedSectionController;
         _undoRedoController = undoRedoController;
         _appStateHistory = appStateHistory.AddClient(this);
         _viewVisibilityController = viewVisibilityController;
         _unselectionArea = unselectionArea;
         _viewToVMReportComponent = viewToVMReportComponent;

         PageLayoutCalculator = pageLayoutCalculator;
         ZoomFactor = zoomFactor;
         VMReportView = vMReportView;
         ReportModeProperty = reportModeProperty;
         BusyIndicator = busyIndicator;
         _actionCaller = actionCaller;

         var appStateSubscription = new Subscription(this).With(new TypeFilter(typeof(ReportModeState)), new TypeFilter(typeof(ReportSectionSelection)));
         _appStateHistory.Subscribe(appStateSubscription);

         HideViewsCommand = new RelayCommand<StateChangeEventArgs>(OnHideViews);
         DeleteSelectedItemsCommand = new RelayCommand<object>(OnDeleteSelectedItems, CanExecuteCommand);
         SelectPageSectionCommand = new RelayCommand<MouseButtonEventArgs>(OnSelectPageSection, CanExecuteCommand);
         UndoCommand = new RelayCommand(OnUndo, CanExecuteCommand);
         RedoCommand = new RelayCommand(OnRedo, CanExecuteCommand);

         ClearSelection = new RelayCommand<MouseEventArgs>(OnClearSelection);
      }

      public IReportModeProperty ReportModeProperty { get; }
      private void OnSelectPageSection(MouseButtonEventArgs eventArgs)
      {
         if (eventArgs.ChangedButton == MouseButton.Left
               && !ReportModeProperty.IsEditingReportBodySectionMode
               && !ClickedOnAdorners(eventArgs)
            )
               _selectedSectionController.SelectPageBodySection();
      }

      public IVMZoomFactor ZoomFactor { get; }
      public VMReportView VMReportView { get; }

      public ICommand HideViewsCommand { get; set; }
      public ICommand DeleteSelectedItemsCommand { get; set; }
      public ICommand SelectPageSectionCommand { get; set; }

      public ICommand UndoCommand { get; set; }
      public ICommand RedoCommand { get; set; }
      public ICommand ClearSelection { get; }
      public ICommand ClosingCommand { get; private set; }

      Name IHasName.Name => nameof(VMReportViewWorkspace);

      public string DataSourcePanelTitle { get; }

      public IBusyIndicator BusyIndicator { get; }

      public bool EditMode
      {
         get => _editMode;
         internal set
         {
            if (_editMode == value) return;
            _editMode = value;
            RaisePropertyChanged();
         }
      }
      public bool AllowDragDrop
      {
         get => _allowDragDrop;
         internal set
         {
            if (_allowDragDrop == value) return;
            _allowDragDrop = value;
            RaisePropertyChanged();
         }
      }

      public bool AllowZoom
      {
         get => _allowZoom;
         internal set
         {
            if (_allowZoom == value) return;
            _allowZoom = value;
            RaisePropertyChanged();
         }
      }

      public IPagesRenderer PageLayoutCalculator { get; }

      public Task Update(ISnapShot snapShot)
      {
         UpdateFromReportState(snapShot);

         return Task.CompletedTask;
      }

      private void UpdateFromReportState(ISnapShot snapShot)
      {
         EditMode = snapShot.IsReportInEditMode();
         AllowDragDrop = EditMode && snapShot.IsReportBodySectionSelected();
         AllowZoom = true;
      }

      private string CreateDataSourcePanelTitleText()
      {
         return Resources.DataSourcePanelTitle;
      }

      private void OnHideViews(StateChangeEventArgs args)
      {
         var views = args.GetHiddenViews();

         views.ForEach(view => _viewVisibilityController.UpdateVisibility(view, false));
      }

      private void OnDeleteSelectedItems(object obj = null)
      {
         _deleteComponentController.DeleteSelectedComponents();
      }

      private void OnUndo(object obj = null)
      {
         _actionCaller.RunUIThreadAction(async () => await _undoRedoController.Undo());
      }

      private void OnRedo(object obj = null)
      {
         _actionCaller.RunUIThreadAction(async () => await _undoRedoController.Redo());
      }

      private void OnClearSelection(MouseEventArgs eventArgs)
      {
         if (CheckIsUnSelectionArea(eventArgs))
            _selectedComponentController.UnSelectAll();
      }

      private bool CheckIsUnSelectionArea(RoutedEventArgs eventArgs)
      {
         return _unselectionArea.CheckIsSelected(eventArgs.OriginalSource, eventArgs.Source);
      }

      // REFACTOR: Preview double click event handler is not easy because id Direct (not Tunneling neither Bubbling).
      private bool ClickedOnAdorners(MouseButtonEventArgs eventArgs)
      {
         return eventArgs.OriginalSource is System.Windows.Shapes.Ellipse;
      }

      private bool CanExecuteCommand(object obj = null)
      {
         return EditMode && !BusyIndicator.IsBusy;
      }

      public void Dispose()
      {
         AllowDragDrop = false;
         AllowZoom = false;
         _appStateHistory.RemoveClient(this);
      }
   }
}
