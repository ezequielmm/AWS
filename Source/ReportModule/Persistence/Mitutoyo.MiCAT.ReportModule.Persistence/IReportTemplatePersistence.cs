// <copyright file="IReportTemplatePersistence.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mitutoyo.MiCAT.ReportModule.Domain.Data;

namespace Mitutoyo.MiCAT.ReportModule.Persistence
{
   public interface IReportTemplatePersistence
   {
      IEnumerable<ReportTemplateDescriptor> GetDefaultReportTemplateDescriptors();
      Task<IEnumerable<ReportTemplateDescriptor>> GetReportTemplatesDescriptors();
      ReportTemplate GetDefaultReportTemplate(Guid reportTemplateGuid);
      Task<ReportTemplate> GetReportTemplate(Guid reportTemplateGuid);
      Task<ReportTemplate> AddTemplate(ReportTemplate reportTemplate);
      Task<ReportTemplate> UpdateTemplate(ReportTemplate reportTemplate);
      Task DeleteReportTemplate(Guid reportTemplateId);
   }
}
