// <copyright file="PdfPersistence.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using Mitutoyo.MiCAT.ReportModule.Domain.DataResult;
using Mitutoyo.MiCAT.ReportModule.Domain.Report;
using Mitutoyo.MiCAT.ReportModule.Persistence.Service;

namespace Mitutoyo.MiCAT.ReportModule.Persistence
{
   public class PdfPersistence : IPdfPersistence
   {
      private readonly IFilePersistenceService<ReportPdf> _service;
      public PdfPersistence(IPersistenceServiceLocator persistenceServiceLocator)
      {
         _service = persistenceServiceLocator.GetService<IFilePersistenceService<ReportPdf>>(Location.File, Mechanism.Pdf);
      }
      public Result SaveReport(ReportPdf reportPdf, string fileName)
      {
         try
         {
            _service.Save(reportPdf, fileName);
            return new SuccessResult();
         }
         catch
         {
            return new ErrorResult("The file is currently in use. Please save with a different file name or in a different folder.");
         }
      }
   }
}
