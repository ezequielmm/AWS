// <copyright file="ReportModeProperty.cs" company="Mitutoyo Europe GmbH">
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
using Mitutoyo.MiCAT.ReportModule.Domain.Data;
using Mitutoyo.MiCAT.ReportModuleApp.AppState.Extensions;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Common
{
   public class ReportModeProperty : VMBase, IReportModeProperty, IUpdateClient, IDisposable
   {
      private readonly IAppStateHistory _appStateHistory;
      public ReportModeProperty(IAppStateHistory appStateHistory)
      {
         _appStateHistory = appStateHistory.AddClient(this);
         var appstateSuscription = new Subscription(this).With(new TypeFilter(typeof(ReportModeState)), new TypeFilter(typeof(ReportSectionSelection)));
         _appStateHistory.Subscribe(appstateSuscription);
      }

      private bool _isEditMode;
      private bool _isEditingReportBodySectionMode;
      private bool _isReportBodyGrayed;

      public bool IsEditMode
      {
         get => _isEditMode;
         private set
         {
            if (_isEditMode == value)
               return;

            _isEditMode = value;
            RaisePropertyChanged();
         }
      }

      public bool IsEditingReportBodySectionMode
      {
         get => _isEditingReportBodySectionMode;
         private set
         {
            if (_isEditingReportBodySectionMode == value)
               return;

            _isEditingReportBodySectionMode = value;
            RaisePropertyChanged();
         }
      }

      public bool IsReportBodyGrayed
      {
         get => _isReportBodyGrayed;
         private set
         {
            if (_isReportBodyGrayed == value)
               return;

            _isReportBodyGrayed = value;
            RaisePropertyChanged();
         }
      }

      public Name Name => nameof(ReportModeProperty);

      public Task Update(ISnapShot snapShot)
      {
         IsEditMode = snapShot.IsReportInEditMode();

         if (IsEditMode)
         {
            IsEditingReportBodySectionMode = snapShot.IsReportBodySectionSelected();
            IsReportBodyGrayed = !IsEditingReportBodySectionMode;
         }
         else
         {
            IsEditingReportBodySectionMode = false;
            IsReportBodyGrayed = false;
         }

         return Task.CompletedTask;
      }

      public void Dispose()
      {
         _appStateHistory.RemoveClient(this);
      }
   }
}
