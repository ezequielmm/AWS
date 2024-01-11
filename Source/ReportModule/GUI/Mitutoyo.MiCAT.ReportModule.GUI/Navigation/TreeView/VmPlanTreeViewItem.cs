// <copyright file="VmPlanTreeViewItem.cs" company="Mitutoyo Europe GmbH">
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
   public class VmPlanTreeViewItem : VmBaseTreeViewItem
   {
      private readonly IPlanController _planController;
      private readonly IDialogService _dialogService;
      private readonly IActionCaller _actionCaller;

      public VmPlanTreeViewItem(
         IPlanController planController,
         IDialogService dialogService,
         IActionCaller actionCaller)
      {
         _planController = planController;
         _dialogService = dialogService;
         _actionCaller = actionCaller;

         CanDeleteItem = true;
      }

      protected override async Task OnDeleteItem()
      {
         await _actionCaller.RunUserActionAsync(async () =>
         {
            if (!ConfirmDeletePlan()) return;
            await _planController.DeletePlan(Id);
         });
      }

      protected override void OnSelectItem()
      {
         _actionCaller.RunUserActionAsync(() => _planController.SelectPlan(Id));
      }

      private bool ConfirmDeletePlan()
      {
         var vm = new VMConfirmationDialog
         {
            Title = Resources.DeletePlanTitle,
            Message = Resources.DeletePlanMessage,
            YesButtonLabel = "DeleteButtonLabel",
            NoButtonLabel = "CancelButtonLabel",
            ConfirmationMessage = "DoYouWantToContinue",
         };
         _dialogService.ShowDialog(vm, DialogType.Confirmation);
         return vm.CanContinue;
      }
   }
}
