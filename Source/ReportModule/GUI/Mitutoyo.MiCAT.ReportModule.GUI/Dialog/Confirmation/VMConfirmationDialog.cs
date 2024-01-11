// <copyright file="VMConfirmationDialog.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using System.Windows.Input;
using Mitutoyo.MiCAT.Common.GUI.Commands;
using Mitutoyo.MiCAT.ReportModule.GUI.Common;
using Mitutoyo.MiCAT.ReportModule.GUI.Properties;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Dialog.Confirmation
{
   public class VMConfirmationDialog : VMBase, IDialogRequestClose
   {
      private string _message;
      private string _title;

      private string _yesButtonLabel;
      private string _noButtonLabel;
      private string _confirmationMessage;

      public virtual bool CanContinue { get; set; }

      public ICommand ConfirmCommand { get; set; }
      public ICommand CancelCommand { get; set; }
      public event EventHandler<DialogEventArgs> CloseRequested;

      public VMConfirmationDialog()
      {
         ConfirmCommand = new RelayCommand(OnConfirmClick);
         CancelCommand = new RelayCommand(OnCancelClick);
         YesButtonLabel = "Yes";
         NoButtonLabel = "No";
         ConfirmationMessage = "AreYouSureToContinue";
      }

      public string Title
      {
         get => _title;
         set
         {
            _title = value;
            RaisePropertyChanged();
         }
      }
      public string Message
      {
         get => _message;
         set
         {
            _message = value;
            RaisePropertyChanged();
         }
      }
      public string YesButtonLabel
      {
         get => _yesButtonLabel;
         set
         {
            _yesButtonLabel = Resources.ResourceManager.GetString($"{value}") ?? value;
            RaisePropertyChanged();
         }
      }
      public string NoButtonLabel
      {
         get => _noButtonLabel;
         set
         {
            _noButtonLabel = Resources.ResourceManager.GetString(value) ?? value;
            RaisePropertyChanged();
         }
      }
      public string ConfirmationMessage
      {
         get => _confirmationMessage;
         set
         {
            _confirmationMessage = Resources.ResourceManager.GetString(value) ?? value;
            RaisePropertyChanged();
         }
      }
      protected void Close(bool? dialogResult)
      {
         CloseRequested?.Invoke(this, new DialogEventArgs(dialogResult));
      }

      private void OnConfirmClick(object param)
      {
         CanContinue = true;
         Close(CanContinue);
      }

      private void OnCancelClick(object param)
      {
         Close(CanContinue);
      }
   }
}