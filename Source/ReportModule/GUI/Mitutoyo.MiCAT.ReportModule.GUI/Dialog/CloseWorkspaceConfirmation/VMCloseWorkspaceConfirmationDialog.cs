// <copyright file="VMCloseWorkspaceConfirmationDialog.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Windows.Input;
using Mitutoyo.MiCAT.Common.GUI.Commands;
using Mitutoyo.MiCAT.ReportModule.GUI.Common;
using Mitutoyo.MiCAT.ReportModuleApp.Providers.Inputs;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Dialog.CloseWorkspaceConfirmation
{
   public class VMCloseWorkspaceConfirmationDialog : VMBase
   {
      public VMCloseWorkspaceConfirmationDialog()
      {
         DialogResult = CloseWorkspaceConfirmationResult.Cancel;

         CloseCommand = new RelayCommand(OnCloseCommand);
         SaveAndCloseCommand = new RelayCommand(OnSaveAndCloseCommand);
      }

      public CloseWorkspaceConfirmationResult DialogResult { get; private set; }
      public ICommand CloseCommand { get; set; }
      public ICommand SaveAndCloseCommand { get; set; }

      private void OnCloseCommand(object param)
      {
         DialogResult = CloseWorkspaceConfirmationResult.Close;
      }

      private void OnSaveAndCloseCommand(object param)
      {
         DialogResult = CloseWorkspaceConfirmationResult.SaveAndClose;
      }
   }
}
