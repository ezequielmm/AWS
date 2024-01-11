// <copyright file="IReportTemplateController.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mitutoyo.MiCAT.ReportModule.Domain.Data;

namespace Mitutoyo.MiCAT.ReportModuleApp.Controllers
{
   public interface IReportTemplateController
   {
      Task<IEnumerable<ReportTemplateDescriptor>> GetReportTemplateDescriptorsForCreate();
      Task<IEnumerable<ReportTemplateDescriptor>> GetReportTemplateDescriptorsForEdit();
      Task<ReportTemplate> GetReportTemplateById(Guid guid);
      Task<bool> DeleteReportTemplate(Guid reportTemplateId);
      ReportTemplate GetCurrentReportTemplate();

      Task<SaveAsResult> SaveCurrentTemplate();
      Task<SaveAsResult> SaveAsCurrentTemplate();
   }

   public enum SaveAsResult
   {
      Canceled,
      Saved,
   }
}
