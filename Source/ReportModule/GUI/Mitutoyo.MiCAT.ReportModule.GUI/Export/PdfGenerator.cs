// <copyright file="PdfGenerator.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using Mitutoyo.MiCAT.ReportModule.Domain.Report;
using Mitutoyo.MiCAT.ReportModuleApp.Utilities;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Export
{
   public class PdfGenerator : IPdfGenerator
   {
      private IRadFixedDocumentGenerator _radFixedDocumentGenerator;

      public PdfGenerator(IRadFixedDocumentGenerator radFixedDocumentGenerator)
      {
         _radFixedDocumentGenerator = radFixedDocumentGenerator;
      }

      public ReportPdf GetReportPdfAsync()
      {
         var provider = new PdfFormatProvider { ExportSettings = { ImageQuality = ImageQuality.High } };
         var document = _radFixedDocumentGenerator.GenerateDocumentToExportAsync();
         return new ReportPdf(provider.Export(document));
      }
   }
}
