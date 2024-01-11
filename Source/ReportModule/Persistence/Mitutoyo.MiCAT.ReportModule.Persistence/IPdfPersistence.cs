﻿// <copyright file="IPdfPersistence.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using Mitutoyo.MiCAT.ReportModule.Domain.DataResult;
using Mitutoyo.MiCAT.ReportModule.Domain.Report;

namespace Mitutoyo.MiCAT.ReportModule.Persistence
{
   public interface IPdfPersistence
   {
      Result SaveReport(ReportPdf reportPdf, string fileName);
   }
}
