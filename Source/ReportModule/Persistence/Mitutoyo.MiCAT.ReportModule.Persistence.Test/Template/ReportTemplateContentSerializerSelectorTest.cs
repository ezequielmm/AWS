// <copyright file="ReportTemplateContentSerializerSelectorTest.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using Mitutoyo.MiCAT.ReportModule.Domain.DataResult;
using Mitutoyo.MiCAT.ReportModule.Persistence.Template;
using Mitutoyo.MiCAT.ReportModule.Persistence.Template.FormatSerializers;
using NUnit.Framework;

namespace Mitutoyo.MiCAT.ReportModule.Persistence.Test.Template
{
   [ExcludeFromCodeCoverage]
   [TestFixture]
   public class ReportTemplateContentSerializerSelectorTest
   {
      [Test]
      public void SelectShouldReturnXMLReportTemplateContentSerializer()
      {
         //Arrenge
         var reportTemplateContentSerializerSelector = new ReportTemplateContentSerializerSelector();

         var report = new VersionIdentifierSO() { Version = "1.0" };
         //Action
         var ret = reportTemplateContentSerializerSelector.Select(report);
         //Assert
         Assert.That(ret is XMLReportTemplateContentSerializer);
      }
      [Test]
      public void SelectWithDifferentVersionShouldThrowException()
      {
         //Arrenge
         var reportTemplateContentSerializerSelector = new ReportTemplateContentSerializerSelector();
         var report = new VersionIdentifierSO() { Version = "2.0" };
         //Action
         var ret = Assert.Catch<ResultException>(() => reportTemplateContentSerializerSelector.Select(report));
         //Assert
         Assert.AreEqual("IncompatibleTemplateError", ret.Key);
      }
   }
}
