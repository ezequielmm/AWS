// <copyright file="VmRunTreeViewItem.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Threading.Tasks;
using Mitutoyo.MiCAT.ReportModule.GUI.Dialog;
using Mitutoyo.MiCAT.ReportModule.GUI.Dialog.Confirmation;
using Mitutoyo.MiCAT.ReportModule.GUI.Properties;
using Mitutoyo.MiCAT.ReportModuleApp.Controllers;
using Mitutoyo.MiCAT.ReportModuleApp.Utilities;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Navigation.TreeView
{
   public class VmRunTreeViewItem : VmBaseTreeViewItem
   {
      private readonly IRunController _runController;
      private readonly IRunRequestController _runRequestController;
      private readonly IDialogService _dialogService;
      private readonly IActionCaller _actionCaller;

      public VmRunTreeViewItem(
         IRunController runController,
         IRunRequestController runRequestController,
         IDialogService dialogService,
         IActionCaller actionCaller)
      {
         _runController = runController;
         _runRequestController = runRequestController;
         _dialogService = dialogService;
         _actionCaller = actionCaller;

         CanDeleteItem = true;
      }

      protected override void OnSelectItem()
      {
         _runRequestController.RequestRun(Id);
      }

      protected override async Task OnDeleteItem()
      {
         await _actionCaller.RunUserActionAsync(async () =>
         {
            if (!ConfirmDeleteRun()) return;
            await _runController.DeleteRun(Id);
         });
      }

      private bool ConfirmDeleteRun()
      {
         var vm = new VMConfirmationDialog
         {
            Title = Resources.DeleteRunTitle,
            Message = Resources.DeleteRunMessage,
            YesButtonLabel = "DeleteButtonLabel",
            NoButtonLabel = "CancelButtonLabel",
            ConfirmationMessage = "DoYouWantToContinue",
         };

         _dialogService.ShowDialog(vm, DialogType.Confirmation);
         return vm.CanContinue;
      }
   }
}
