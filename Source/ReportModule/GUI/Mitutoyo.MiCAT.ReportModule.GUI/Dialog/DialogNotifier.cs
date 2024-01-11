// <copyright file="DialogNotifier.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using Mitutoyo.MiCAT.ReportModule.Domain.DataResult;
using Mitutoyo.MiCAT.ReportModuleApp.Utilities;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Dialog
{
   public class DialogNotifier: IMessageNotifier
   {
      private readonly IDialogService _dialogService;
      public DialogNotifier(IDialogService dialogService)
      {
         _dialogService = dialogService;
      }
      public void NotifyError(ResultException ex)
      {
         _dialogService.ShowError(ex);
      }
   }
}
