// <copyright file="CloseWorkspaceConfirmationInput.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Windows;
using Mitutoyo.MiCAT.ReportModule.GUI.Dialog.CloseWorkspaceConfirmation;
using Mitutoyo.MiCAT.ReportModule.GUI.Providers;
using Mitutoyo.MiCAT.ReportModuleApp.Providers.Inputs;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Dialog.Confirmation.CloseWorkspace
{
   public class CloseWorkspaceConfirmationInput : ICloseWorkspaceConfirmationInput
   {
      private readonly IActiveWindowProvider _activeWindowProvider;
      public CloseWorkspaceConfirmationInput(IActiveWindowProvider activeWindowProvider)
      {
         _activeWindowProvider = activeWindowProvider;
      }

      public CloseWorkspaceConfirmationResult ConfirmCloseWorkspace()
      {
         var vm = new VMCloseWorkspaceConfirmationDialog();
         var dialog = new CloseWorkspaceConfirmationDialog()
         {
            Owner = _activeWindowProvider.GetActiveWindow() as Window,
            DataContext = vm,
         };

         dialog.ShowDialog();

         return vm.DialogResult;
      }
   }
}
