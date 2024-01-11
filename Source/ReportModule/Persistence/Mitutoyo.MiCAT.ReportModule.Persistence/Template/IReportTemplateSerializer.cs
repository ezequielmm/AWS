// <copyright file="IReportTemplateSerializer.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using Mitutoyo.MiCAT.ReportModule.Domain.DataResult;

namespace Mitutoyo.MiCAT.ReportModule.Persistence.Template
{
   public interface IReportTemplateSerializer
   {
      Result Serialize(ReportTemplateDataSO reportTemplateDataSO);
      Result Deserialize(string reportTemplateData);
   }
}
