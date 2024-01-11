// <copyright file="ReportTemplatePersistenceTest.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Drawing.Printing;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using Mitutoyo.MiCAT.DataServiceClient;
using Mitutoyo.MiCAT.ReportModule.Domain.Data;
using Mitutoyo.MiCAT.ReportModule.Domain.DataResult;
using Mitutoyo.MiCAT.ReportModule.Domain.DataResult.Exceptions;
using Mitutoyo.MiCAT.ReportModule.Persistence.Service;
using Mitutoyo.MiCAT.ReportModule.Persistence.Service.Helpers;
using Mitutoyo.MiCAT.ReportModule.Persistence.Template;
using Mitutoyo.MiCAT.ReportModule.Persistence.Template.CurrentVersion;
using Mitutoyo.MiCAT.ReportModule.Setup.Configurations;
using Mitutoyo.MiCAT.Web.Data;
using Moq;
using NUnit.Framework;

namespace Mitutoyo.MiCAT.ReportModule.Persistence.Test
{
   [ExcludeFromCodeCoverage]
   [TestFixture]
   public class ReportTemplatePersistenceTest
   {
      private IMapper _mapper;
      private Mock<IDataServiceClient> _dataServiceClient;
      private List<ReportTemplateLowDTO> _reportTemplatesResults;
      private Mock<IDataServiceClient> _dataServiceClientMed;
      private List<ReportTemplateMediumDTO> _reportTemplatesMedResults;
      private Guid _id;
      private Guid _idMed;
      private Mock<IAssemblyConfigurationHelper> _connectionStringProvider;
      private IReportTemplateContentSerializerSelector _selector = new ReportTemplateContentSerializerSelector();
      private IReportTemplateVersionProvider _provider = new CurrentReportTemplateVersion();
      private IReportTemplateSerializer _serializer = new ReportTemplateSerializer();
      private IReportTemplateSerializationManager _serializationManager;

      [SetUp]
      public virtual void SetUp()
      {
         _mapper = AutoMapperConfig.InitializeAutoMapper().CreateMapper();
         _dataServiceClient = new Mock<IDataServiceClient>();
         _dataServiceClientMed = new Mock<IDataServiceClient>();
         _connectionStringProvider = new Mock<IAssemblyConfigurationHelper>();
         _serializationManager = new ReportTemplateSerializationManager(_mapper, _selector, _provider, _serializer);

         var template =
            "<ReportTemplate>\n" +
               "<CommonPageLayout>\n" +
                  "<Id>00000001-0000-0000-0000-000000000001</Id>\n" +
                  "<PageSize>\n" +
                     "<PaperKind>Letter</PaperKind>\n" +
                     "<Height>1056</Height>\n" +
                     "<Width>816</Width>\n" +
                  "</PageSize>\n" +
                  "<CanvasMargin>\n" +
                  "   <Left>60</Left> <Top>30</Top> <Right>30</Right> <Bottom>30</Bottom>" +
                  "</CanvasMargin>\n" +
               "</CommonPageLayout>\n" +
               "<PageLayouts>\n" +
                  "<PageLayout>\n" +
                     "<Id>00000000-0000-0000-0000-000000000001</Id>\n" +
                     "<AddedByUser>true</AddedByUser>\n" +
                     "<ReportComponentIds>\n" +
                     "   <Guid>3ec0056c-f5a8-4ea1-81d5-f32d04b6ed46</Guid>\n" +
                     "</ReportComponentIds>\n" +
                  "</PageLayout>\n" +
               "</PageLayouts>\n" +
               "<ReportComponents>\n" +
                  "<Label>\n" +
                     "<Id>3ec0056c-f5a8-4ea1-81d5-f32d04b6ed46</Id>\n" +
                     "<X>10</X>\n" +
                     "<Y>165</Y>\n" +
                     "<DisplayText>Organization:</DisplayText>\n" +
                     "<FontWeight>Bold</FontWeight>\n" +
                     "<Height>20</Height>\n" +
                     "<Width>30</Width>\n" +
                  "</Label>\n" +
               "</ReportComponents>\n" +
            "</ReportTemplate>\n";
         template = "{VersionIdentifier:{Version:'1.0', DataType:'ReportTemplate'}, 'ReportTemplateContent':'" + template + "'}";

         _id = Guid.NewGuid();
         var reportTemplateResultId2 = Guid.NewGuid();
         var reportTemplateResultId3 = Guid.NewGuid();

         _reportTemplatesResults = new List<ReportTemplateLowDTO>()
         {
            new ReportTemplateLowDTO()
            {
               Id = _id,
               Name = "Template 1"
            },
            new ReportTemplateLowDTO()
            {
               Id = reportTemplateResultId2,
               Name = "Template 2"
            },
            new ReportTemplateLowDTO()
            {
               Id = reportTemplateResultId3,
               Name = "Template 3"
            }
         };

         DataServiceResult<ServiceInfoDTO> result =
            new DataServiceResult<ServiceInfoDTO>(new HttpResponseMessage(HttpStatusCode.InternalServerError),
               new DataServiceError(null));
         _dataServiceClient.Setup(x => x.GetServiceInfo()).Returns(Task.Run(() => result));

         var reportTemplateResults =
            new DataServiceResult<IEnumerable<ReportTemplateLowDTO>>(new HttpResponseMessage(HttpStatusCode.OK),
               _reportTemplatesResults);
         _dataServiceClient.Setup(x => x.GetAllReportTemplates<ReportTemplateLowDTO>())
            .Returns(Task.Run(() => reportTemplateResults));

         var reportTemplateResult =
            new DataServiceResult<ReportTemplateLowDTO>(new HttpResponseMessage(HttpStatusCode.OK),
               _reportTemplatesResults.FirstOrDefault());
         _dataServiceClient.Setup(x => x.GetReportTemplate<ReportTemplateLowDTO>(_id))
            .Returns(Task.Run(() => reportTemplateResult));

         _idMed = Guid.NewGuid();
         var reportTemplateMedResultId2 = Guid.NewGuid();
         var reportTemplateMedResultId3 = Guid.NewGuid();

         _reportTemplatesMedResults = new List<ReportTemplateMediumDTO>()
         {
            new ReportTemplateMediumDTO()
            {
               Id = _idMed,
               Name = "Template 1",
               Template = template
            },
            new ReportTemplateMediumDTO()
            {
               Id = reportTemplateMedResultId2,
               Name = "Template 2",
               Template = "{VersionIdentifier:{Version:'1.0', DataType:'ReportTemplate'}, 'ReportTemplateContent':'" + "<Template2></Template2>" + "'}"
            },
            new ReportTemplateMediumDTO()
            {
               Id = reportTemplateMedResultId3,
               Name = "Template 3",
               Template = "{VersionIdentifier:{Version:'1.0', DataType:'ReportTemplate'}, 'ReportTemplateContent':'" + "<Template3></Template3>" + "'}"
            }
         };

         DataServiceResult<ServiceInfoDTO> resultMed =
            new DataServiceResult<ServiceInfoDTO>(new HttpResponseMessage(HttpStatusCode.InternalServerError),
               new DataServiceError(null));
         _dataServiceClientMed.Setup(x => x.GetServiceInfo()).Returns(Task.Run(() => resultMed));

         var reportTemplateMedResults =
            new DataServiceResult<IEnumerable<ReportTemplateMediumDTO>>(new HttpResponseMessage(HttpStatusCode.OK),
               _reportTemplatesMedResults);
         _dataServiceClientMed.Setup(x => x.GetAllReportTemplates<ReportTemplateMediumDTO>())
            .Returns(Task.Run(() => reportTemplateMedResults));

         var reportTemplateMedResult =
            new DataServiceResult<ReportTemplateMediumDTO>(new HttpResponseMessage(HttpStatusCode.OK),
               _reportTemplatesMedResults.FirstOrDefault());
         _dataServiceClientMed.Setup(x => x.GetReportTemplate<ReportTemplateMediumDTO>(_idMed))
            .Returns(Task.Run(() => reportTemplateMedResult));
      }

      [Test]
      public void UpdateTemplate_ShouldUpdateReportTemplateIfItIsNotReadOnly()
      {
         // Arrange
         var persistenceDataService = new Mock<IPersistenceDataService>();

         var persistenceServiceLocator = new Mock<IPersistenceServiceLocator>();
         persistenceServiceLocator
            .Setup(x => x.GetService<IPersistenceDataService>(Location.DataService, Mechanism.DataService))
            .Returns(persistenceDataService.Object);

         var reportTemplatePersistence = new ReportTemplatePersistence(persistenceServiceLocator.Object, _mapper, _serializationManager);
         var reportTemplate = new ReportTemplate
         {
            TemplateDescriptor = new TemplateDescriptor { Name = "Name", ReadOnly = false, TemplateId = Guid.NewGuid() }
         };

         // Act
         var result = reportTemplatePersistence.UpdateTemplate(reportTemplate);

         // Assert
         persistenceDataService.Verify(
            x => x.UpdateReportTemplate(reportTemplate.TemplateDescriptor.TemplateId,
               It.IsAny<ReportTemplateUpdateDTO>()), Times.Once);
      }

      [Test]
      public void AddTemplate_ShouldAddNewReportTemplateIfItIsReadOnly()
      {
         // Arrange
         var service = new Mock<IPersistenceDataService>();

         var persistenceServiceLocator = new Mock<IPersistenceServiceLocator>();
         persistenceServiceLocator
            .Setup(x => x.GetService<IPersistenceDataService>(Location.DataService, Mechanism.DataService))
            .Returns(service.Object);

         var reportTemplatePersistence = new ReportTemplatePersistence(persistenceServiceLocator.Object, _mapper, _serializationManager);
         var reportTemplate = new ReportTemplate
         {
            TemplateDescriptor = new TemplateDescriptor { Name = "Name", ReadOnly = true, TemplateId = Guid.NewGuid() }
         };

         // Act
         var result = reportTemplatePersistence.AddTemplate(reportTemplate);

         // Assert
         service.Verify(x => x.AddReportTemplateMed(It.Is<string>(s => s == "Name"), It.IsAny<string>()), Times.Once);
      }
      [Test]
      public void GetReportTemplatesShouldReturnReportTemplatesDescriptor()
      {
         // Arrange
         var service = new Mock<IPersistenceDataService>(); new PersistenceDataService(_dataServiceClient.Object);
         service.Setup(p => p.GetReportTemplatesLow()).Returns(Task.FromResult((Result)new SuccessResult<IEnumerable<ReportTemplateLowDTO>>(_reportTemplatesResults)));

         var persistenceServiceLocator = new Mock<IPersistenceServiceLocator>();
         persistenceServiceLocator
            .Setup(x => x.GetService<IPersistenceDataService>(Location.DataService, Mechanism.DataService))
            .Returns(service.Object);
         var reportTemplatePersistence = new ReportTemplatePersistence(persistenceServiceLocator.Object, _mapper, _serializationManager);

         // Act
         var result = reportTemplatePersistence.GetReportTemplatesDescriptors().Result;

         // Assert
         Assert.AreEqual(result.Count(), 3);
         Assert.AreEqual(result.FirstOrDefault().Id, _id);
         Assert.AreEqual(result.FirstOrDefault().Name, "Template 1");
      }

      [Test]
      public void GetReportTemplateShouldReturnAReportTemplate()
      {
         // Arrange
         var service = new Mock<IPersistenceDataService>(); new PersistenceDataService(_dataServiceClient.Object);
         service.Setup(p => p.GetReportTemplateMed(It.IsAny<Guid>())).Returns(Task.FromResult((Result)new SuccessResult<ReportTemplateMediumDTO>(_reportTemplatesMedResults.First())));

         Mock<IPersistenceServiceLocator> persistenceServiceLocator = new Mock<IPersistenceServiceLocator>();
         persistenceServiceLocator
            .Setup(x => x.GetService<IPersistenceDataService>(Location.DataService, Mechanism.DataService))
            .Returns(service.Object);

         var reportTemplatePersistence = new ReportTemplatePersistence(persistenceServiceLocator.Object, _mapper, _serializationManager);

         // Act
         var result = reportTemplatePersistence.GetReportTemplate(_idMed).Result;

         // Assert
         Assert.AreEqual(result.CommonPageLayout.PageSize.PaperKind, PaperKind.Letter);
      }

      [Test]
      public void DeleteReportTemplate_ShouldCallDeleteReportTemplate()
      {
         var serviceInfoDTO = new DataServiceResult<ServiceInfoDTO>(new HttpResponseMessage(), new ServiceInfoDTO());
         _dataServiceClient.Setup(dsc => dsc.GetServiceInfo()).Returns(Task.FromResult(serviceInfoDTO));

         // Arrange
         var service =
            new PersistenceDataService(_dataServiceClient.Object) { };

         var persistenceServiceLocator = new Mock<IPersistenceServiceLocator>();
         persistenceServiceLocator
            .Setup(x => x.GetService<IPersistenceDataService>(Location.DataService, Mechanism.DataService))
            .Returns(service);

         var reportTemplatePersistence = new ReportTemplatePersistence(persistenceServiceLocator.Object, _mapper, _serializationManager);
         var reportTemplate = new ReportTemplate
         {
            TemplateDescriptor = new TemplateDescriptor { Name = "Name", ReadOnly = true, TemplateId = Guid.NewGuid() }
         };

         // Act
         var result = reportTemplatePersistence.DeleteReportTemplate(reportTemplate.TemplateDescriptor.TemplateId);

         // Assert
         _dataServiceClient.Verify(x => x.DeleteReportTemplate(reportTemplate.TemplateDescriptor.TemplateId),
            Times.Once);
      }
      [Test]
      public void UpdateDeletedTemplate_ShouldThrowNotFoundException()
      {
         // Arrange
         var persistenceDataService = new Mock<IPersistenceDataService>();
         persistenceDataService.Setup(p => p.UpdateReportTemplate(It.IsAny<Guid>(),It.IsAny<ReportTemplateUpdateDTO>()))
            .Returns(Task.FromResult((Result)new ErrorResult("error", ResultErrorCode.NotFound)));
         var persistenceServiceLocator = new Mock<IPersistenceServiceLocator>();
         persistenceServiceLocator
            .Setup(x => x.GetService<IPersistenceDataService>(Location.DataService, Mechanism.DataService))
            .Returns(persistenceDataService.Object);

         var reportTemplatePersistence = new ReportTemplatePersistence(persistenceServiceLocator.Object, _mapper, _serializationManager);
         var reportTemplate = new ReportTemplate
         {
            TemplateDescriptor = new TemplateDescriptor { Name = "Name", ReadOnly = false, TemplateId = Guid.NewGuid() }
         };

         // Act
         var reportTemplateNotFoundException = Assert.CatchAsync<ReportTemplateNotFoundException>(()=>reportTemplatePersistence.UpdateTemplate(reportTemplate));

         // Assert
         Assert.AreEqual("ReportTemplateNotFound", reportTemplateNotFoundException.Key);
      }
      [Test]
      public void UpdateTemplateWithDBError_ShouldThrowDBConnectionException()
      {
         // Arrange
         var persistenceDataService = new Mock<IPersistenceDataService>();
         persistenceDataService.Setup(p => p.UpdateReportTemplate(It.IsAny<Guid>(), It.IsAny<ReportTemplateUpdateDTO>()))
            .Returns(Task.FromResult((Result)new ErrorResult("error")));
         var persistenceServiceLocator = new Mock<IPersistenceServiceLocator>();
         persistenceServiceLocator
            .Setup(x => x.GetService<IPersistenceDataService>(Location.DataService, Mechanism.DataService))
            .Returns(persistenceDataService.Object);

         var reportTemplatePersistence = new ReportTemplatePersistence(persistenceServiceLocator.Object, _mapper, _serializationManager);
         var reportTemplate = new ReportTemplate
         {
            TemplateDescriptor = new TemplateDescriptor { Name = "Name", ReadOnly = false, TemplateId = Guid.NewGuid() }
         };

         // Act
         var resultException = Assert.CatchAsync<ResultException>(() => reportTemplatePersistence.UpdateTemplate(reportTemplate));

         // Assert
         Assert.AreEqual("DataServiceConnectionError", resultException.Key);
      }
   }
}