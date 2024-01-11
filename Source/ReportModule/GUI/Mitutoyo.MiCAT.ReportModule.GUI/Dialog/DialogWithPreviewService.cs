// <copyright file="DialogWithPreviewService.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Linq;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Dialog
{
   public class DialogWithPreviewService : IDialogWithPreviewService
   {
      private readonly IDialogManager _dialogManager;

      public DialogWithPreviewService(IDialogManager dialogManager)
      {
         _dialogManager = dialogManager;
      }

      public string TryGetUserSelectedFile(string title, string fileExtension)
      {
         string[] file = null;
          _dialogManager.ShowOpenFileDialog(title, null, fileExtension, 1, false, ref file);
          if (file.ToList().Any())
            return file[0];
         return string.Empty;
      }
   }
}
