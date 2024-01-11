// <copyright file="ReportTemplate.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using System.Collections.Generic;
using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.ReportModule.Domain.CADElements;
using Mitutoyo.MiCAT.ReportModule.Domain.Components;
using Mitutoyo.MiCAT.ReportModule.Domain.Components.ReportSections;

namespace Mitutoyo.MiCAT.ReportModule.Domain.Data
{
   public class ReportTemplate
   {
      public ReportTemplate()
      {
         CommonPageLayout = new CommonPageLayout();
         CadLayouts = Array.Empty<CADLayout>();
         TemplateDescriptor = new TemplateDescriptor();
         ReportComponents = Array.Empty<IReportComponent>();
         ReportComponentDataItems = Array.Empty<IStateItem>();
         ReportHeader = new ReportHeader(false);
         ReportBody = new ReportBody(true);
         ReportFooter = new ReportFooter(false);
      }

      public CommonPageLayout CommonPageLayout { get; set; }
      public IEnumerable<IReportComponent> ReportComponents { get; set; }
      public IEnumerable<IStateItem> ReportComponentDataItems { get; set; }
      public IEnumerable<CADLayout> CadLayouts { get; set; }
      public TemplateDescriptor TemplateDescriptor { get; set; }
      public string Template { get; set; }
      public ReportHeader ReportHeader { get; set; }
      public ReportBody ReportBody { get; set; }
      public ReportFooter ReportFooter { get; set; }
   }
}
