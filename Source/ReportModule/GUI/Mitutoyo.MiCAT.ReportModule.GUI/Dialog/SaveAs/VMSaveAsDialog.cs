// <copyright file="VMSaveAsDialog.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.ComponentModel.DataAnnotations;
using System.Windows.Input;
using Mitutoyo.MiCAT.Common.GUI.Commands;
using Mitutoyo.MiCAT.ReportModule.GUI.Common;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Dialog.SaveAs
{
   public class VMSaveAsDialog : VMBase
   {
      private string _name;
      private bool _canSave;
      private DialogResults _dialogResult;

      public VMSaveAsDialog()
      {
         CanSave = false;
         DialogResult = DialogResults.None;
         CancelCommand = new RelayCommand(OnCancel);
         SaveCommand = new RelayCommand(OnSave);
      }

      public ICommand CancelCommand { get; set; }
      public ICommand SaveCommand { get; set; }

      [StringLength(250)]
      [Required()]
      public string Name
      {
         get => _name;
         set
         {
            ValidateName(value);
            _name = value;
            RaisePropertyChanged();
         }
      }

      public bool CanSave
      {
         get => _canSave;
         set
         {
            _canSave = value;
            RaisePropertyChanged();
         }
      }

      public DialogResults DialogResult
      {
         get => _dialogResult;
         set
         {
            if (_dialogResult != value)
            {
               _dialogResult = value;
               RaisePropertyChanged();
            }
         }
      }

      private void OnCancel(object obj = null)
      {
         DialogResult = DialogResults.Canceled;
      }

      private void OnSave(object obj = null)
      {
         if (CanSave)
         {
            DialogResult = DialogResults.Ok;
         }
      }

      private void ValidateName(string newName)
      {
         var context = new ValidationContext(this) { MemberName = nameof(Name) };

         CanSave = Validator.TryValidateProperty(newName, context, null);
         Validator.ValidateProperty(newName, context);
      }
   }
}