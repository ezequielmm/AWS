// <copyright file="DialogService.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using System.Collections.Generic;
using Mitutoyo.MiCAT.ReportModule.GUI.Dialog.Confirmation;
using Mitutoyo.MiCAT.ReportModule.GUI.Providers;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Dialog
{
   public class DialogService : IDialogService
   {
      private readonly IDictionary<DialogType, Type> _mappings;

      private readonly IActiveWindowProvider _activeWindowProvider;
      public DialogService(IActiveWindowProvider activeWindowProvider)
      {
         _mappings = new Dictionary<DialogType, Type>()
         {
            { DialogType.Error, typeof(ErrorDialog) },
            { DialogType.Info, typeof(InfoDialog) },
            { DialogType.Confirmation, typeof(ConfirmationDialog)  }
         };

         _activeWindowProvider = activeWindowProvider;
      }

      public void ShowError(Exception ex)
      {
         ShowDialog((VMErrorDialog)ex, DialogType.Error);
      }

      public void ShowInfo(string messageInfo)
      {
         ShowDialog((VMInfoDialog)messageInfo, DialogType.Info);
      }

      public void ShowDialog<TViewModel>(TViewModel vm, DialogType dialogType) where TViewModel : IDialogRequestClose
      {
         Show(vm, dialogType);
      }

      private bool? Show<TViewModel>(TViewModel viewModel, DialogType dialogType) where TViewModel : IDialogRequestClose
      {
         Type viewType = _mappings[dialogType];

         IDialog dialog = (IDialog)Activator.CreateInstance(viewType);

         viewModel.CloseRequested += DialogEventHandler(viewModel, dialog);

         dialog.DataContext = viewModel;
         dialog.Owner = _activeWindowProvider.GetActiveWindow();

         return dialog.ShowDialog();
      }

      private static EventHandler<DialogEventArgs> DialogEventHandler<TViewModel>(TViewModel viewModel, IDialog dialog)
         where TViewModel : IDialogRequestClose
      {
         EventHandler<DialogEventArgs> handler = null;

         handler = (sender, e) =>
         {
            viewModel.CloseRequested -= handler;

            if (e.DialogResult.HasValue)
            {
               dialog.DialogResult = e.DialogResult;
            }
            else
            {
               dialog.Close();
            }
         };
         return handler;
      }
   }

   public enum DialogType
   {
      Error,
      Info,
      Confirmation
   }
}