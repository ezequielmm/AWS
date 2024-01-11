// <copyright file="XMLReportTemplateContentSerializer.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using System.IO;
using System.Xml.Serialization;
using Mitutoyo.MiCAT.ReportModule.Domain.DataResult;
using Mitutoyo.MiCAT.ReportModule.Persistence.Components;

namespace Mitutoyo.MiCAT.ReportModule.Persistence.Template.FormatSerializers
{
   public class XMLReportTemplateContentSerializer : IReportTemplateContentSerializer
   {
      public Result Deserialize(string reportTemplate)
      {
         try
         {
            var xmlSerializer = new XmlSerializer(typeof(ReportTemplateSO));
            using (var stringreader = new StringReader(reportTemplate))
            {
               var template = (ReportTemplateSO)xmlSerializer.Deserialize(stringreader);
               return new SuccessResult<ReportTemplateSO>(template);
            }
         }
         catch (Exception ex)
         {
            return new ErrorResult(ex.Message);
         }
      }

      public Result Serialize(ReportTemplateSO reportTemplateSO)
      {
         try
         {
            XmlSerializer serializer = new XmlSerializer(typeof(ReportTemplateSO));
            using (StringWriter textWriter = new StringWriter())
            {
               serializer.Serialize(textWriter, reportTemplateSO);
               return new SuccessResult<string>(textWriter.ToString());
            }
         }
         catch (Exception ex)
         {
            return new ErrorResult(ex.Message);
         }
      }

      public bool SupportsVersion(string version)
      {
         return version == "1.0" || version == "1.1";
      }
   }
}
