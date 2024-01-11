// <copyright file="ReportComponentLayoutChangedLogger.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Test.PageLayout
{
   [ExcludeFromCodeCoverage]
   public class ReportComponentLayoutChangedLogger
   {
      public List<string> ChangeLog { get; set; }

      public ReportComponentLayoutChangedLogger(IVMReportBodyPlacement vmReportBodyPlacement)
      {
         if (vmReportBodyPlacement == null)
         {
            throw new ArgumentNullException("viewModel");
         }
         this.ChangeLog = new List<string>();
         vmReportBodyPlacement.ReportComponentLayoutChanged += ViewModel_ReportComponentLayoutChanged;
      }

      private void ViewModel_ReportComponentLayoutChanged(object sender, ReportComponentLayoutChangedEventArgs e)
      {
         ChangeLog.Add("ReportComponentLayoutChanged Event Called");
      }
   }
}