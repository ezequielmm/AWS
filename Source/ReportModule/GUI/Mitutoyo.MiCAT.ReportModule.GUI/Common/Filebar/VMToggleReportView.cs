// <copyright file="VMToggleReportView.cs" company="Mitutoyo Europe GmbH">
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
using Mitutoyo.MiCAT.Core;
using Mitutoyo.MiCAT.ReportModule.App.AppState;
using Mitutoyo.MiCAT.ReportModule.GUI.Utilities;
using Mitutoyo.MiCAT.ReportModuleApp.AppState.Extensions;
using Mitutoyo.MiCAT.ReportModuleApp.Controllers;
using Mitutoyo.MiCAT.ReportModuleApp.Utilities;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Common.Filebar
{
   public class VMToggleReportView : VMBase, IUpdateClient, IDisposable
   {
      private bool _isChecked;
      private IReportFileBarController _reportFileBarController;
      private readonly IActionCaller _actionCaller;
      private readonly IAppStateHistory _history;

      public ICommand CheckedCommand { get; private set; }
      public ICommand UncheckedCommand { get; private set; }

      public bool IsChecked
      {
         get { return _isChecked; }
         set
         {
            if (_isChecked != value)
            {
               _isChecked = value;
               RaisePropertyChanged();
            }
         }
      }

      public VMToggleReportView(IReportFileBarController reportFileBarController, IAppStateHistory appStateHistory, IActionCaller actionCaller)
      {
         _reportFileBarController = reportFileBarController;
         _actionCaller = actionCaller;
         CheckedCommand = new RelayCommand<bool>(OnChecked);
         UncheckedCommand = new RelayCommand<bool>(OnChecked);
         _history = appStateHistory.AddClient(this);
         _history.Subscribe(new Subscription(this).With(typeof(ReportModeState)));
      }

      private void OnChecked(bool isChecked)
      {
         _actionCaller.RunUIThreadAction(() => _reportFileBarController.ChangeStateToggleButton(isChecked));
      }

      public Task Update(ISnapShot snapShot)
      {
         if (snapShot.IsReportInEditMode())
         {
            IsChecked = true;
         }
         else
         {
            IsChecked = false;
         }

         return Task.CompletedTask;
      }

      public void Dispose()
      {
         _history.RemoveClient(this);
      }

      public Name Name => GetType().GetTypeName();
   }
}
