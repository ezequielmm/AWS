// <copyright file="VMMainToolbar.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using System.Threading.Tasks;
using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.ApplicationState.Clients;
using Mitutoyo.MiCAT.Core;
using Mitutoyo.MiCAT.ReportModule.App.AppState;
using Mitutoyo.MiCAT.ReportModule.GUI.Common;
using Mitutoyo.MiCAT.ReportModule.GUI.Utilities;
using Mitutoyo.MiCAT.ReportModuleApp.AppState.Extensions;
using Mitutoyo.MiCAT.ReportModuleApp.Controllers;
using Mitutoyo.MiCAT.ReportModuleApp.Utilities;
using IReportBusyIndicator = Mitutoyo.MiCAT.ReportModule.GUI.Common.BusyIndicators.IBusyIndicator;

namespace Mitutoyo.MiCAT.ReportModule.GUI.MainWindow
{
   public class VMMainToolbar : VMBase, IUpdateClient, IDisposable
   {
      private readonly IAppStateHistory _appStateHistory;
      private readonly IActionCaller _actionCaller;
      private IPdfExportController _pdfExportController;
      private bool _reportEditMode = true;
      private bool _isExportEnabled = true;

      public RelayCommand<object> ExportAsPDFCommand { get; set; }
      public RelayCommand<object> ExportAsCSVCommand { get; set; }

      public VMMainToolbar(
         IAppStateHistory appStateHistory,
         IActionCaller actionCaller,
         IReportBusyIndicator busyIndicator,
         IPdfExportController pdfExportController)
      {
         _appStateHistory = appStateHistory.AddClient(this);

         _actionCaller = actionCaller;
         BusyIndicator = busyIndicator;
         _pdfExportController = pdfExportController;

         ExportAsPDFCommand = new RelayCommand<object>(OnExportAsPDF, (object obj) => { return !ReportEditMode; });
         ExportAsCSVCommand = new RelayCommand<object>(OnExportAsCSV, (object obj) => { return false; });

         var appStateChangesSubscription = new Subscription(this).With(typeof(ReportModeState));
         _appStateHistory.Subscribe(appStateChangesSubscription);
      }

      public IReportBusyIndicator BusyIndicator { get; }
      public bool ReportEditMode
      {
         get => _reportEditMode;
         private set
         {
            if (value == _reportEditMode)
               return;

            _reportEditMode = value;

            ExportAsPDFCommand.RaiseCanExecuteChanged();
            RaisePropertyChanged();
         }
      }
      public bool IsExportAsEnabled
      {
         get => _isExportEnabled;
         private set
         {
            _isExportEnabled = value;
            RaisePropertyChanged();
         }
      }
      private bool EnableExportAs()
      {
         return ExportAsPDFCommand.CanExecute(null) || ExportAsCSVCommand.CanExecute(null);
      }

      private void OnExportAsPDF(object obj)
      {
         _actionCaller.RunUserAction(() => _pdfExportController.Export());
      }

      private void OnExportAsCSV(object obj)
      {
         return;
      }

      public Task Update(ISnapShot snapShot)
      {
         ReportEditMode = snapShot.IsReportInEditMode();
         IsExportAsEnabled = EnableExportAs();
         return Task.CompletedTask;
      }

      public void Dispose()
      {
         _appStateHistory.RemoveClient(this);
      }

      public Name Name => GetType().GetTypeName();
   }
}
