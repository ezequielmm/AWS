// <copyright file="IReportTemplateSerializationManager.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using Mitutoyo.MiCAT.ReportModule.Domain.Data;
using Mitutoyo.MiCAT.ReportModule.Persistence.Components;
using Mitutoyo.MiCAT.Web.Data;

namespace Mitutoyo.MiCAT.ReportModule.Persistence.Template
{
   public interface IReportTemplateSerializationManager
   {
      ReportTemplate Deserialize(ReportTemplateMediumDTO reportTemplateDTO);

      string Serialize(ReportTemplateSO reportTemplate);
   }
}
