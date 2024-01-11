// <copyright file="ReportExporter.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading.Tasks;
using Mitutoyo.MiCAT.ReportModuleConnector.Common;
using Mitutoyo.MiCAT.ReportModuleConnector.ExportConnector;

namespace Mitutoyo.MiCAT.ReportModule.Connectors
{
   [ExcludeFromCodeCoverage]
   internal class ReportExporter : IReportExporter
   {
      public async Task<ReportOperationResult<ReportExporterSuccessResult, ReportExporterFailureResult>> ExportToPdf(Guid templateId, Guid runId, FileInfo saveFileInfo)
      {
         try
         {
            using var backgroundExport = new BackgroundExporter();
            await backgroundExport.ExportToPDF(templateId, runId, saveFileInfo);
         }
         catch (Exception ex)
         {
            return ReportOperationResult<ReportExporterSuccessResult, ReportExporterFailureResult>.FromFailure(new ReportExporterFailureResult(ex.Message, -1, ex));
         }

         return ReportOperationResult<ReportExporterSuccessResult, ReportExporterFailureResult>.FromSuccess(new ReportExporterSuccessResult());
      }
   }
}
