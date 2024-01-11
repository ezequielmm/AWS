// <copyright file="VMSelectTemplateForCreate.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Collections.Generic;
using System.Threading.Tasks;
using Mitutoyo.MiCAT.ReportModule.Domain.Data;
using Mitutoyo.MiCAT.ReportModule.GUI.Utilities.Events;
using Mitutoyo.MiCAT.ReportModuleApp.Controllers;

namespace Mitutoyo.MiCAT.ReportModule.GUI.SelectTemplates
{
   public class VMSelectTemplateForCreate : VMSelectTemplateBase
   {
      public VMSelectTemplateForCreate(IReportTemplateController reportTemplateController) : base(reportTemplateController) { }

      public override string Title => Properties.Resources.SelectTemplateTitle;

      public override string ProceedButtonText => Properties.Resources.CreateTemplateProceedButtonText;

      protected override SelectedReportTemplateInfo BuildSelectedReportTemplateInfo(ReportTemplate reportTemplate)
      {
         return new SelectedReportTemplateInfo(reportTemplate, ReportMode.ViewMode);
      }

      protected override async Task<IEnumerable<ReportTemplateDescriptor>> GetReportTemplatesFromContoller(IReportTemplateController reportTemplateController)
      {
         return await reportTemplateController.GetReportTemplateDescriptorsForCreate();
      }
   }
}