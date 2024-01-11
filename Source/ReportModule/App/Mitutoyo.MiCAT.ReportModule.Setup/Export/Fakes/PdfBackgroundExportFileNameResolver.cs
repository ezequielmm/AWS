// <copyright file="PdfBackgroundExportFileNameResolver.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.IO;
using Mitutoyo.MiCAT.ReportModuleApp.Providers;

namespace Mitutoyo.MiCAT.ReportModule.Setup.Export.Fakes
{
   public class PdfBackgroundExportFileNameResolver : IPdfNameResolver
   {
      private readonly FileInfo _fileInfo;

      public PdfBackgroundExportFileNameResolver(FileInfo fileInfo)
      {
         _fileInfo = fileInfo;
      }

      public PdfNameResult QueryFileInfo()
      {
         return new PdfNameResult(_fileInfo, false);
      }
   }
}
