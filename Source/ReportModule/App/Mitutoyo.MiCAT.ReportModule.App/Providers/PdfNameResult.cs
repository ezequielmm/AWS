﻿// <copyright file="PdfNameResult.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.IO;

namespace Mitutoyo.MiCAT.ReportModuleApp.Providers
{
   public class PdfNameResult
   {
      public PdfNameResult(FileInfo fileInfo, bool isCanceled)
      {
         FileInfo = fileInfo;
         IsCanceled = isCanceled;
      }

      public FileInfo FileInfo { get; }
      public bool IsCanceled { get; }
   }
}
