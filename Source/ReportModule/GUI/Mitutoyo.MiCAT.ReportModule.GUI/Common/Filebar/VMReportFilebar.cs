// <copyright file="VMReportFilebar.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.ApplicationState.Clients;
using Mitutoyo.MiCAT.Common.Help;
using Mitutoyo.MiCAT.Core;
using Mitutoyo.MiCAT.ReportModule.Domain.DataResult;
using Mitutoyo.MiCAT.ReportModule.Domain.Visibility;
using Mitutoyo.MiCAT.ReportModule.GUI.Common.BusyIndicators;
using Mitutoyo.MiCAT.ReportModule.GUI.Common.Filebar.MarginsSettings;
using Mitutoyo.MiCAT.ReportModule.GUI.Dialog;
using Mitutoyo.MiCAT.ReportModule.GUI.Utilities;
using Mitutoyo.MiCAT.ReportModuleApp.AppState.Extensions;
using Mitutoyo.MiCAT.ReportModuleApp.Controllers;
using Mitutoyo.MiCAT.ReportModuleApp.Utilities;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Common.Filebar
{
   public class VMReportFilebar : VMBase, IUpdateClient, IDisposable
   {
      private readonly IAppStateHistory _appStateHistory;
      private readonly IUndoRedoController _undoRedoController;
      private readonly IReportTemplateController _reportTemplateController;
      private readonly IDialogService _dialogService;
      private readonly IHelpController _helpController;
      private readonly IPdfExportController pdfExportController;
      private readonly IActionCaller _actionCaller;
      private readonly ICloseController _closeController;
      private readonly IViewVisibilityController _viewVisibilityController;
      private bool _canUndo = false;
      private bool _canRedo = false;
      private bool _reportEditMode = true;
      private bool _isPlansViewVisible = true;
      private bool _isRunsViewVisible = true;

      public VMPageSizeSettings PageSizeSettings { get; }
      public VMMarginSizeSettings MarginSizeSettings { get; }

      public IAsyncCommand SaveCommand { get; set; }
      public IAsyncCommand SaveAsCommand { get; set; }
      public ICommand UndoCommand { get; set; }
      public ICommand RedoCommand { get; set; }
      public RelayCommand<object> ExportToPDFCommand { get; set; }
      public ICommand OpenHelpCommand { get; set; }
      public ICommand CloseCommand { get; set; }
      public ICommand TogglePlansViewCommand { get; set; }
      public ICommand ToggleRunsViewCommand { get; set; }

      public bool CanUndo
      {
         get => _canUndo;
         private set
         {
            if (value == _canUndo)
               return;

            _canUndo = value;

            RaisePropertyChanged();
         }
      }

      public bool CanRedo
      {
         get => _canRedo;
         private set
         {
            if (value == _canRedo)
               return;

            _canRedo = value;
            RaisePropertyChanged();
         }
      }

      public bool ReportEdidMode
      {
         get => _reportEditMode;
         private set
         {
            if (value == _reportEditMode)
               return;

            _reportEditMode = value;

            ExportToPDFCommand.RaiseCanExecuteChanged();
            RaisePropertyChanged();
         }
      }

      public bool IsPlansViewVisible
      {
         get => _isPlansViewVisible;
         set
         {
            if (value == _isPlansViewVisible)
               return;

            _isPlansViewVisible = value;
            RaisePropertyChanged();
         }
      }

      public bool IsRunsViewVisible
      {
         get => _isRunsViewVisible;
         set
         {
            if (value == _isRunsViewVisible)
               return;

            _isRunsViewVisible = value;
            RaisePropertyChanged();
         }
      }

      public IBusyIndicator BusyIndicator { get; }

      public VMReportFilebar(
         IAppStateHistory appStateHistory,
         IUndoRedoController undoRedoController,
         IPdfExportController pdfExportControllerController,
         VMPageSizeSettings pageSizeSettings,
         VMMarginSizeSettings marginSizeSettings,
         IReportTemplateController reportTemplateController,
         IDialogService dialogService,
         IHelpController helpController,
         IActionCaller actionCaller,
         IBusyIndicator busyIndicator,
         ICloseController closeController,
         IViewVisibilityController viewVisibilityController)
      {
         _appStateHistory = appStateHistory.AddClient(this);
         _undoRedoController = undoRedoController;
         _helpController = helpController;
         _reportTemplateController = reportTemplateController;
         _dialogService = dialogService;

         pdfExportController = pdfExportControllerController;
         PageSizeSettings = pageSizeSettings;
         MarginSizeSettings = marginSizeSettings;
         _actionCaller = actionCaller;
         BusyIndicator = busyIndicator;
         _closeController = closeController;
         _viewVisibilityController = viewVisibilityController;

         SaveCommand = new AsyncCommand<object>(OnSave);
         SaveAsCommand = new AsyncCommand<object>(OnSaveAs);
         UndoCommand = new RelayCommand<object>(OnUndo);
         RedoCommand = new RelayCommand<object>(OnRedo);
         ExportToPDFCommand = new RelayCommand<object>(OnExportToPDF, (object obj) => { return !ReportEdidMode; });
         OpenHelpCommand = new RelayCommand<object>(OnOpenHelpCommand);
         CloseCommand = new RelayCommand<object>(OnCloseCommand);
         TogglePlansViewCommand = new RelayCommand<object>(OnTogglePlansView);
         ToggleRunsViewCommand = new RelayCommand<object>(OnToggleRunsView);

         var appStateChangesSubscription = new Subscription(this);
         _appStateHistory.Subscribe(appStateChangesSubscription);
      }

      private void OnTogglePlansView(object obj = null)
      {
         _viewVisibilityController.ToggleVisibility(ViewElement.Plans);
      }

      private void OnToggleRunsView(object obj = null)
      {
         _viewVisibilityController.ToggleVisibility(ViewElement.Runs);
      }

      private void OnCloseCommand(object obj = null)
      {
         _closeController.CloseWorkspace();
      }

      private async Task OnSave(object obj = null)
      {
         await _actionCaller.RunUserActionAsync(async () => await TrySave(_reportTemplateController.SaveCurrentTemplate));
      }

      private async Task OnSaveAs(object obj = null)
      {
         await _actionCaller.RunUserActionAsync(async () => await TrySave(_reportTemplateController.SaveAsCurrentTemplate));
      }

      private void OnUndo(object obj = null)
      {
         _actionCaller.RunUIThreadAction(async () => await _undoRedoController.Undo());
      }

      private void OnRedo(object obj = null)
      {
         _actionCaller.RunUIThreadAction(async () => await _undoRedoController.Redo());
      }

      private void OnExportToPDF(object obj)
      {
         _actionCaller.RunUserAction(() => pdfExportController.Export());
      }

      private void OnOpenHelpCommand(object args)
      {
         _helpController.OpenHelp(HelpTopicPath.Home);
      }

      public Task Update(ISnapShot snapShot)
      {
         ReportEdidMode = snapShot.IsReportInEditMode();

         CanUndo = _undoRedoController.CanUndo;
         CanRedo = _undoRedoController.CanRedo;

         UpdateViewsVisibility(snapShot);

         return Task.CompletedTask;
      }

      // Property needed by Common.RadMenuItemStyle in order to avoid binding warnings
      public string HotKeyDisplay => string.Empty;

      public Name Name => GetType().GetTypeName();

      private async Task TrySave(Func<Task> SaveAction)
      {
         try
         {
            await SaveAction();
         }
         catch (ResultException resultException)
         {
            _dialogService.ShowError(resultException);
         }
      }

      private void UpdateViewsVisibility(ISnapShot snapShot)
      {
         IsPlansViewVisible = snapShot.IsViewVisible(ViewElement.Plans);
         IsRunsViewVisible = snapShot.IsViewVisible(ViewElement.Runs);
      }

      public void Dispose()
      {
         _appStateHistory.RemoveClient(this);
      }
   }
}