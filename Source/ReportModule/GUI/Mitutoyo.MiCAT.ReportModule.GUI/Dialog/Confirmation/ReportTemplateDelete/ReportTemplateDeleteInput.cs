// <copyright file="ReportTemplateDeleteInput.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using Mitutoyo.MiCAT.ReportModuleApp.Providers.Inputs;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Dialog.Confirmation.ReportTemplateDelete
{
   public class ReportTemplateDeleteInput : IReportTemplateDeleteConfirmationInput
   {
      private readonly IDialogService _dialogService;

      public ReportTemplateDeleteInput(IDialogService dialogService)
      {
         _dialogService = dialogService;
      }
      public bool ConfirmDeleteReportTemplate()
      {
         var vm = new VMReportTemplateDeleteConfirmationDialog();
         _dialogService.ShowDialog(vm, DialogType.Confirmation);

         return vm.CanContinue;
      }
   }
}
