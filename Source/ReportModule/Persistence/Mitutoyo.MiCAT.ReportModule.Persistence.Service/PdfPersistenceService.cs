// <copyright file="PdfPersistenceService.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using System.IO;
using Mitutoyo.MiCAT.ReportModule.Domain.DataResult;
using Mitutoyo.MiCAT.ReportModule.Domain.Report;

namespace Mitutoyo.MiCAT.ReportModule.Persistence.Service
{
   public class PdfPersistenceService : IFilePersistenceService<ReportPdf>
   {
      public Result<ReportPdf> Load(string fileName)
      {
         throw new NotImplementedException();
      }

      public void Save(ReportPdf data, string fileName)
      {
         Validate(data, fileName);
         File.WriteAllBytes(fileName, data.Report);
      }

      private void Validate(ReportPdf data, string fileName)
      {
         if (data == null)
            throw new ArgumentNullException("Report to save cannot be null");

         if (string.IsNullOrEmpty(fileName))
            throw new ArgumentNullException("File name cannot be null");
      }
   }
}
