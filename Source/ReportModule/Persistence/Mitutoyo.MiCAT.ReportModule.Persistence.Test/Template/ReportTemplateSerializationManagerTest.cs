// <copyright file="ReportTemplateSerializationManagerTest.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using Mitutoyo.MiCAT.ReportModule.Domain.Data;
using Mitutoyo.MiCAT.ReportModule.Domain.DataResult;
using Mitutoyo.MiCAT.ReportModule.Persistence.Components;
using Mitutoyo.MiCAT.ReportModule.Persistence.Template;
using Mitutoyo.MiCAT.ReportModule.Persistence.Template.CurrentVersion;
using Mitutoyo.MiCAT.ReportModule.Persistence.Template.FormatSerializers;
using Mitutoyo.MiCAT.Web.Data;
using Moq;
using NUnit.Framework;

namespace Mitutoyo.MiCAT.ReportModule.Persistence.Test.Template
{
   [ExcludeFromCodeCoverage]
   [TestFixture]
   public class ReportTemplateSerializationManagerTest
   {
      private Mock<IMapper> _mapperMock;
      private Mock<IReportTemplateContentSerializerSelector> _serializerSelectorMock;
      private Mock<IReportTemplateVersionProvider> _currentVersionProviderMock;
      private Mock<IReportTemplateSerializer> _reportTemplateSerializerMock;
      private ReportTemplateSerializationManager _reportTemplateSerializationManager;
      private Mock<IReportTemplateContentSerializer> _reportTemplateContentSerializerMock;

      [SetUp]
      public void Setup()
      {
         _mapperMock = new Mock<IMapper>();
         _serializerSelectorMock = new Mock<IReportTemplateContentSerializerSelector>();
         _currentVersionProviderMock = new Mock<IReportTemplateVersionProvider>();
         _reportTemplateSerializerMock = new Mock<IReportTemplateSerializer>();
         _reportTemplateContentSerializerMock =  new Mock<IReportTemplateContentSerializer>();

         _currentVersionProviderMock.Setup(p => p.GetCurrentVersion()).Returns("1.0");

         _serializerSelectorMock.Setup(s => s.Select(It.IsAny<VersionIdentifierSO>())).Returns(_reportTemplateContentSerializerMock.Object);

         _reportTemplateSerializationManager = new ReportTemplateSerializationManager(_mapperMock.Object, _serializerSelectorMock.Object,
                                                _currentVersionProviderMock.Object, _reportTemplateSerializerMock.Object);
      }
      [Test]
      public void SerializeShouldReturnJsonString()
      {
         //Arrenge
         var reportTemplateSO = new ReportTemplateSO();
         var result = new SuccessResult<string>("<></>");
         var reportTemplateDataResult = new SuccessResult<string>("{'VersionIdentifier':{'Version':'1.0', 'DataType':'ReportTemplate'},'ReportTemplateContent':''}");

         _reportTemplateContentSerializerMock.Setup(s => s.Serialize(reportTemplateSO)).Returns(result);
         _reportTemplateSerializerMock.Setup(s => s.Serialize(It.IsAny<ReportTemplateDataSO>())).Returns(reportTemplateDataResult);

         //Action
         var ret = _reportTemplateSerializationManager.Serialize(reportTemplateSO);

         //Assert
         Assert.IsTrue(ret.Contains("VersionIdentifier"));
         Assert.IsTrue(ret.Contains("Version"));
         Assert.IsTrue(ret.Contains("DataType"));
         Assert.IsTrue(ret.Contains("ReportTemplateContent"));
      }
      [Test]
      public void SerializeWithErrorInContentShouldReturnResultException()
      {
         //Arrenge
         var reportTemplateSO = new ReportTemplateSO();
         var result = new ErrorResult("Fail to Deserialize");

         _reportTemplateContentSerializerMock.Setup(s => s.Serialize(reportTemplateSO)).Returns(result);

         //Action
         var exceptionResult = Assert.Catch<ResultException>(()=>_reportTemplateSerializationManager.Serialize(reportTemplateSO));

         //Assert
         Assert.AreEqual("InvalidTemplateError", exceptionResult.Key);
      }
      [Test]
      public void SerializeWithErrorInTemplateShouldReturnResultException()
      {
         //Arrenge
         var reportTemplateSO = new ReportTemplateSO();
         var result = new SuccessResult<string>("<></>");
         var reportTemplateSerializerResult = new ErrorResult("Report Template Result Error");

         _reportTemplateContentSerializerMock.Setup(s => s.Serialize(reportTemplateSO)).Returns(result);
         _reportTemplateSerializerMock.Setup(s => s.Serialize(It.IsAny<ReportTemplateDataSO>())).Returns(reportTemplateSerializerResult);

         //Action
         var exceptionResult = Assert.Catch<ResultException>(() => _reportTemplateSerializationManager.Serialize(reportTemplateSO));

         //Assert
         Assert.AreEqual("InvalidTemplateError", exceptionResult.Key);
      }
      [Test]
      public void DeSerializeShouldReturnReportTemplate()
      {
         //Arrenge
         var reportTemplateDTO = new ReportTemplateMediumDTO() { Template = "{'VersionIdentifier':{ 'Version':'1.0', 'DataType':'ReportTemplate'},'ReportTemplateContent':''}"};
         var reportTemplateData = new ReportTemplateDataSO()
         {
            VersionIdentifier = new VersionIdentifierSO() { Version = _currentVersionProviderMock.Object.GetCurrentVersion(), DataType = "ReportTemplate" },
            ReportTemplateContent = "<></>"
         };
         var reportTemplateDataResult = new SuccessResult<ReportTemplateDataSO>(reportTemplateData);
         var reportContentResult = new SuccessResult<ReportTemplateSO>(new ReportTemplateSO()
         {
            CadLayouts = new List<CADElements.CADLayoutSO>(),
            ReportComponentDataItems = new List<object>(),
            ReportComponents = new List<ReportComponentSO>(),
            CommonPageLayout = new CommonPageLayoutSO() { CanvasMargin = new MarginSO() { MarginKind = Domain.MarginKind.Narrow} }
         }) ;

         _reportTemplateSerializerMock.Setup(s => s.Deserialize(It.IsAny<string>())).Returns(reportTemplateDataResult);
         _reportTemplateContentSerializerMock.Setup(s => s.Deserialize(reportTemplateData.ReportTemplateContent)).Returns(reportContentResult);

         //Action
         var ret = _reportTemplateSerializationManager.Deserialize(reportTemplateDTO);

         //Assert
         Assert.That(ret is ReportTemplate);
      }
      [Test]
      public void DeSerializeWithErrorInTemplateShouldReturnResultException()
      {
         //Arrenge
         var reportTemplateDTO = new ReportTemplateMediumDTO() { Template = "{'VersionIdentifier':{ 'Version':'1.0', 'DataType':'ReportTemplate'},'ReportTemplateContent':''}" };
         var reportTemplateDataResult = new ErrorResult("Report Template Error");

         _reportTemplateSerializerMock.Setup(s => s.Deserialize(It.IsAny<string>())).Returns(reportTemplateDataResult);

         //Action
         var exceptionResult = Assert.Catch<ResultException>(() => _reportTemplateSerializationManager.Deserialize(reportTemplateDTO));

         //Assert
         Assert.AreEqual("InvalidTemplateError", exceptionResult.Key);
      }
      [Test]
      public void DeSerializeWithErrorInContentShouldReturnResultException()
      {
         //Arrenge
         var reportTemplateDTO = new ReportTemplateMediumDTO() { Template = "{'VersionIdentifier':{ 'Version':'1.0', 'DataType':'ReportTemplate'},'ReportTemplateContent':''}" };
         var reportTemplateData = new ReportTemplateDataSO()
         {
            VersionIdentifier = new VersionIdentifierSO() { Version = _currentVersionProviderMock.Object.GetCurrentVersion(), DataType = "ReportTemplate" },
            ReportTemplateContent = "<></>"
         };
         var reportTemplateDataResult = new SuccessResult<ReportTemplateDataSO>(reportTemplateData);
         var reportContentResult = new ErrorResult("Report Template Data Error");

         _reportTemplateSerializerMock.Setup(s => s.Deserialize(It.IsAny<string>())).Returns(reportTemplateDataResult);
         _reportTemplateContentSerializerMock.Setup(s => s.Deserialize(reportTemplateData.ReportTemplateContent)).Returns(reportContentResult);

         //Action
         var exceptionResult = Assert.Catch<ResultException>(() => _reportTemplateSerializationManager.Deserialize(reportTemplateDTO));

         //Assert
         Assert.AreEqual("InvalidTemplateError", exceptionResult.Key);
      }
   }
}
