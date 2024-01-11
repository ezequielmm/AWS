// <copyright file="XMLReportTemplateContentSerializerTest.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using Mitutoyo.MiCAT.ReportModule.Domain.DataResult;
using Mitutoyo.MiCAT.ReportModule.Persistence.Components;
using Mitutoyo.MiCAT.ReportModule.Persistence.Template.FormatSerializers;
using NUnit.Framework;

namespace Mitutoyo.MiCAT.ReportModule.Persistence.Test.Template.FormatSerializers
{
   [ExcludeFromCodeCoverage]
   [TestFixture]
   public class XMLReportTemplateContentSerializerTest
   {
      [Test]
      public void SupportsVersionShouldReturnTrueWhenSameVersion()
      {
         //Arrenge
         var version = "1.0";
         var reportSerializer = new XMLReportTemplateContentSerializer();
         //Action
         var ret = reportSerializer.SupportsVersion(version);
         //Assert
         Assert.IsTrue(ret);
      }
      [Test]
      public void SupportsVersionShouldReturnFalseWhenDiffVersion()
      {
         //Arrenge
         var version = "2.0";
         var reportSerializer = new XMLReportTemplateContentSerializer();
         //Action
         var ret = reportSerializer.SupportsVersion(version);
         //Assert
         Assert.IsFalse(ret);
      }
      [Test]
      public void SerializeShouldReturnSuccess()
      {
         //Arrenge
         var reportSerializer = new XMLReportTemplateContentSerializer();
         var so = new ReportTemplateSO();
         //Action
         var ret = reportSerializer.Serialize(so);
         //Assert
         Assert.IsTrue(ret.IsSuccess);
         Assert.That(((SuccessResult<string>)ret).Result.Contains("ReportTemplate"));
      }
      [Test]
      public void DeserializeShouldReturnSuccess()
      {
         //Arrenge
         var reportSerializer = new XMLReportTemplateContentSerializer();
         var report = "<ReportTemplate></ReportTemplate>";
         //Action
         var ret = reportSerializer.Deserialize(report);
         //Assert
         Assert.IsTrue(ret.IsSuccess);
         Assert.That(((SuccessResult<ReportTemplateSO>)ret).Result is ReportTemplateSO);
      }
      [Test]
      public void DeserializeWithnullShouldReturnErrorResult()
      {
         //Arrenge
         var reportSerializer = new XMLReportTemplateContentSerializer();
         var report = "<></>";
         //Action
         var ret = reportSerializer.Deserialize(report);
         //Assert
         Assert.IsTrue(ret.IsError);
         Assert.That(((ErrorResult)ret).ErrorMessage == "There is an error in XML document (1, 2).");
      }
   }
}
