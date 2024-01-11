// <copyright file="XmlPersistenceServiceTest.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using System.Diagnostics.CodeAnalysis;
using System.Drawing.Printing;
using Mitutoyo.MiCAT.ReportModule.Domain.Data;
using Mitutoyo.MiCAT.ReportModule.Persistence.Components;
using Mitutoyo.MiCAT.ReportModule.Persistence.Service;
using NUnit.Framework;

namespace Mitutoyo.MiCAT.ReportModule.Service.Test
{
   [ExcludeFromCodeCoverage]
   [TestFixture]
   public class XmlPersistenceServiceTest
   {
      [Test]
      public void Save_ThrowException_ReportWithoutName()
      {
         var service = new XmlPersistenceService<ReportTemplate>();

         // Act & Assert
         var ex = Assert.Throws<ArgumentNullException>(() => service.Save(null, string.Empty));
         Assert.That(ex.ParamName, Is.EqualTo("File name cannot be null"));
      }

      [Test]
      public void Load_ShouldLoadReportTemplateCreated()
      {
         // Arrange
         string fileName = this.GetFileName();

         var service = new XmlPersistenceService<ReportTemplateSO>();
         var reportTemplate = this.CreateReportTemplate();
         service.Save(reportTemplate, fileName);

         //Act
         var resultReportLayout = service.Load(fileName);

         // Assert
         Assert.AreEqual(resultReportLayout.ResultObject.CommonPageLayout.PageSize.PaperKind, reportTemplate.CommonPageLayout.PageSize.PaperKind);
         Assert.AreEqual(resultReportLayout.ResultObject.CommonPageLayout.PageSize.Width, reportTemplate.CommonPageLayout.PageSize.Width);
         Assert.AreEqual(resultReportLayout.ResultObject.CommonPageLayout.PageSize.Height, reportTemplate.CommonPageLayout.PageSize.Height);
      }

      private ReportTemplateSO CreateReportTemplate()
      {
         var pageSizeInfoSo = new PageSizeInfoSO();
         pageSizeInfoSo.Width = 100;
         pageSizeInfoSo.Height = 101;
         pageSizeInfoSo.PaperKind = PaperKind.A3;

         var reportTemplate = new ReportTemplateSO()
         {
            CommonPageLayout = new CommonPageLayoutSO { PageSize = pageSizeInfoSo }
         };
         return reportTemplate;
      }
      private string GetFileName()
      {
         return AppDomain.CurrentDomain.BaseDirectory + "/" + Guid.NewGuid() + ".xml";
      }
   }
}
