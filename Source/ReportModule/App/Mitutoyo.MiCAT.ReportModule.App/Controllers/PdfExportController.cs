// <copyright file="PdfExportController.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using Mitutoyo.MiCAT.ReportModule.Domain.DataResult;
using Mitutoyo.MiCAT.ReportModule.Domain.Report;
using Mitutoyo.MiCAT.ReportModule.Persistence;
using Mitutoyo.MiCAT.ReportModuleApp.Providers;
using Mitutoyo.MiCAT.ReportModuleApp.Utilities;

namespace Mitutoyo.MiCAT.ReportModuleApp.Controllers
{
   public class PdfExportController: IPdfExportController
   {
      private readonly IPdfPersistence _pdfPersistence;
      private IPdfNameResolver _pdfNameResolver;
      private IPdfGenerator _pdfGenerator;
      private IMessageNotifier _notifier;
      private IProcessLauncher _processLauncher;

      public PdfExportController(IPdfPersistence pdfPersistence, IPdfNameResolver pdfNameResolver,
         IPdfGenerator pdfGenerator, IMessageNotifier messageNotifier, IProcessLauncher processLauncher)
      {
         _pdfPersistence = pdfPersistence;
         _pdfNameResolver = pdfNameResolver;
         _pdfGenerator = pdfGenerator;
         _notifier = messageNotifier;
         _processLauncher = processLauncher;
      }

      public bool Export()
      {
         try
         {
            var fileInfo = _pdfNameResolver.QueryFileInfo();
            if (fileInfo.IsCanceled)
               return false;
            var reportPdf = _pdfGenerator.GetReportPdfAsync();
            SaveReport(reportPdf, fileInfo.FileInfo.FullName);
            _processLauncher.LaunchProcess(fileInfo.FileInfo.FullName);
            return true;
         }
         catch (ResultException ex)
         {
            _notifier.NotifyError(ex);
            return false;
         }
      }

      private void SaveReport(ReportPdf reportPdf, string fileName)
      {
         ResultHelper.ThrowIfFailure(_pdfPersistence.SaveReport(reportPdf, fileName), "OpenedFile");
      }
   }
}
