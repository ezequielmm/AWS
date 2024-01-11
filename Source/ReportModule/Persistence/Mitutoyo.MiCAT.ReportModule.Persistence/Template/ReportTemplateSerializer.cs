// <copyright file="ReportTemplateSerializer.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using Mitutoyo.MiCAT.ReportModule.Domain.DataResult;
using Newtonsoft.Json;

namespace Mitutoyo.MiCAT.ReportModule.Persistence.Template
{
   public class ReportTemplateSerializer : IReportTemplateSerializer
   {
      public Result Deserialize(string reportTemplateData)
      {
         try
         {
            var template = JsonConvert.DeserializeObject<ReportTemplateDataSO>(reportTemplateData, new JsonSerializerSettings
            {
               MissingMemberHandling = MissingMemberHandling.Error
            });

            return new SuccessResult<ReportTemplateDataSO>(template);
         }
         catch (Exception ex)
         {
            return new ErrorResult(ex.Message);
         }
      }

      public Result Serialize(ReportTemplateDataSO reportTemplateDataSO)
      {
         try
         {
            var template = JsonConvert.SerializeObject(reportTemplateDataSO);
            return new SuccessResult<string>(template);
         }
         catch (JsonException ex)
         {
            return new ErrorResult(ex.Message);
         }
      }
   }
}
