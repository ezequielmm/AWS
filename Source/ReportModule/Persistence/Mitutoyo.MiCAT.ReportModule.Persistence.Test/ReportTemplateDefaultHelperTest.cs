// <copyright file="ReportTemplateDefaultHelperTest.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Mitutoyo.MiCAT.ReportModule.Domain.DataResult;
using Mitutoyo.MiCAT.ReportModule.Persistence.Components;
using Mitutoyo.MiCAT.ReportModule.Persistence.Template.FormatSerializers;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace Mitutoyo.MiCAT.ReportModule.Persistence.Test
{
   [ExcludeFromCodeCoverage]
   [TestFixture]
   public class ReportTemplateDefaultHelperTest
   {
      [Test]
      public void GetReportTemplateDefaultById_Test()
      {
         // Arrange
         var templateId = new Guid("9241b7ad-68e4-4095-9ee4-cdeed839eba8");

         // Act
         var reportTemplate = ReportTemplateDefaultHelper.GetReportTemplateDefaultById(templateId);

         // Assert
         Assert.AreEqual(reportTemplate.Name, "Blank");
         Assert.IsFalse(string.IsNullOrEmpty(reportTemplate.Template));
         Assert.IsTrue(reportTemplate.ReadOnly);
      }

      [Test]
      public void GetReportTemplateDefaults_Test()
      {
         // Act
         var reportTemplates = ReportTemplateDefaultHelper.GetReportTemplateDefaults();

         // Assert
         Assert.IsTrue(reportTemplates.Count() > 0);
         Assert.IsTrue(reportTemplates.Any(t => t.ReadOnly));
         Assert.IsTrue(reportTemplates.Any(t => !string.IsNullOrEmpty(t.Template)));
         Assert.IsTrue(reportTemplates.Any(t => !string.IsNullOrEmpty(t.Name)));
      }

      [Test]
      public void GetValidBlankTemplate_Test()
      {
         //Arrange
         var reportTemplateMediumDTO = ReportTemplateDefaultHelper.GetBlank();

         // Act
         var template = JObject.Parse(reportTemplateMediumDTO.Template);
         var xml = template["ReportTemplateContent"].ToString();

         var reportSerializer = new XMLReportTemplateContentSerializer();
         var ret = reportSerializer.Deserialize(xml);

         var report = (((SuccessResult<ReportTemplateSO>)ret).Result as ReportTemplateSO);

         // Assert
         Assert.AreEqual(report.CommonPageLayout.PageSize.PaperKind, System.Drawing.Printing.PaperKind.A4);
         Assert.IsEmpty(report.ReportComponents);
      }
      [Test]
      public void GetReportTemplateDescriptorDefaultsGetBlankDescriptor()
      {
         // Act
         var report = ReportTemplateDefaultHelper.GetReportTemplateDescriptorDefaults();

         // Assert
         Assert.IsTrue(report.Any(x => x.Name == "Blank"));
      }
   }
}
