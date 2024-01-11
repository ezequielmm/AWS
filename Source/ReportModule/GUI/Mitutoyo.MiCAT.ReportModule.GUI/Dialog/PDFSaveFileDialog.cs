// <copyright file="PDFSaveFileDialog.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.IO;
using Microsoft.Win32;
using Mitutoyo.MiCAT.ReportModule.GUI.Strings;
using Mitutoyo.MiCAT.ReportModuleApp.Providers;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Dialog
{
   public class PDFSaveFileDialog : IPdfNameResolver
   {
      public PdfNameResult QueryFileInfo()
      {
         var _dialog = new SaveFileDialog
         {
            Title = StringFinder.FindLocalizedString("ExportToPDFTitle", " "),
            FileName = StringFinder.FindLocalizedString("ExportToPDFFileName"),
            Filter = StringFinder.FindLocalizedString("ExportToPDFFileTypeDesc") + "|*.pdf",
            DefaultExt = "pdf"
         };
         var dialogSuccess = _dialog.ShowDialog();
         if ((!dialogSuccess.HasValue) || (!dialogSuccess.Value))
            return new PdfNameResult(null, true);
         FileInfo fileInfo = new FileInfo(_dialog.FileName);
         return new PdfNameResult(fileInfo, false);
      }
   }
}
