// <copyright file="PersistenceDataServiceTest.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Mitutoyo.MiCAT.DataServiceClient;
using Mitutoyo.MiCAT.ReportModule.Domain.DataResult;
using Mitutoyo.MiCAT.ReportModule.Persistence.Service;
using Mitutoyo.MiCAT.ReportModule.Persistence.Service.Helpers;
using Mitutoyo.MiCAT.Web.Data;
using Mitutoyo.MiCAT.Web.Data.Common;
using Moq;
using NUnit.Framework;

namespace Mitutoyo.MiCAT.ReportModule.Service.Test
{
   [ExcludeFromCodeCoverage]
   [TestFixture]
   public class PersistenceDataServiceTest
   {
      private Mock<IDataServiceClient> _dataServiceClient;
      private Mock<IAssemblyConfigurationHelper> _connectionStringProvider;

      [SetUp]
      public virtual void SetUp()
      {
         _dataServiceClient = new Mock<IDataServiceClient>();
         _connectionStringProvider = new Mock<IAssemblyConfigurationHelper>();

         DataServiceResult<ServiceInfoDTO> result =
            new DataServiceResult<ServiceInfoDTO>(new HttpResponseMessage(HttpStatusCode.InternalServerError),
               new DataServiceError(null));
         _dataServiceClient.Setup(x => x.GetServiceInfo()).Returns(Task.Run(() => result));
      }

      [Test]
      public void GetEvaluatedCharacteristicShouldReturnDomainObjectsAsync()
      {
         // Arrange
         var measurementResultId1 = Guid.NewGuid();

         var characteristic1 = new CharacteristicDTO()
         {
            CharacteristicType = Web.Data.CharacteristicType.Circularity,
            Id = Guid.NewGuid(),
            Name = "C1"
         };

         var characteristicActuals1 = Enumerable.Empty<CharacteristicActualDTO>().ToList();
         characteristicActuals1.Add(new CharacteristicActualDTO()
         {
            CharacteristicId = characteristic1.Id,
            MeasurementResultId = measurementResultId1
         });

         var characteristicDataResults =
            new DataServiceResult<CharacteristicActualDTO>(new HttpResponseMessage(HttpStatusCode.OK),
               characteristicActuals1.FirstOrDefault());
         _dataServiceClient.Setup(x => x.GetCharacteristicActual(measurementResultId1, characteristic1.Id))
            .Returns(Task.Run(() => characteristicDataResults));

         var service = new PersistenceDataService(_dataServiceClient.Object);

         // Act
         var result =
            (SuccessResult<CharacteristicActualDTO>)service
               .GetCharacteristicActual(measurementResultId1, characteristic1.Id).Result;

         // Assert
         Assert.AreEqual(result.Result.CharacteristicId, characteristic1.Id);
         Assert.AreEqual(result.Result.MeasurementResultId, measurementResultId1);
      }

      [Test]
      public void GetAllCharacteristicActualsShouldReturnDomainObjects()
      {
         // Arrange
         var measurementResultId1 = Guid.NewGuid();
         var measurementResultId2 = Guid.NewGuid();

         var characteristic1 = new CharacteristicDTO()
         {
            CharacteristicType = Web.Data.CharacteristicType.Circularity,
            Id = Guid.NewGuid(),
            Name = "C1"
         };

         var characteristic2 = new CharacteristicDTO()
         {
            CharacteristicType = Web.Data.CharacteristicType.Flatness,
            Id = Guid.NewGuid(),
            Name = "C2"
         };

         var characteristicActuals1 = Enumerable.Empty<CharacteristicActualDTO>().ToList();
         characteristicActuals1.Add(new CharacteristicActualDTO()
         {
            CharacteristicId = characteristic1.Id,
            MeasurementResultId = measurementResultId1
         });

         var characteristicActuals2 = Enumerable.Empty<CharacteristicActualDTO>().ToList();
         characteristicActuals2.Add(new CharacteristicActualDTO()
         {
            CharacteristicId = characteristic2.Id,
            MeasurementResultId = measurementResultId2
         });

         var characteristicActualDataResults =
            new DataServiceResult<IEnumerable<CharacteristicActualDTO>>(new HttpResponseMessage(HttpStatusCode.OK),
               characteristicActuals1);
         _dataServiceClient.Setup(x => x.GetAllCharacteristicActuals(measurementResultId1))
            .Returns(Task.Run(() => characteristicActualDataResults));

         var service = new PersistenceDataService(_dataServiceClient.Object);

         // Act
         var result =
            (SuccessResult<IEnumerable<CharacteristicActualDTO>>)service
               .GetAllCharacteristicActuals(measurementResultId1).Result;

         // Assert
         Assert.AreEqual(result.Result.Count(), characteristicActuals1.Count);
         Assert.AreEqual(result.Result.FirstOrDefault().MeasurementResultId, measurementResultId1);
      }

      [Test]
      public void GetAllPlansByPartShouldReturnDomainObjects()
      {
         // Arrange
         var measurementResultId1 = Guid.NewGuid();

         var partId = Guid.NewGuid();
         var planId = Guid.NewGuid();
         var characteristic1 = new CharacteristicDTO()
         {
            CharacteristicType = Web.Data.CharacteristicType.Circularity,
            Id = Guid.NewGuid(),
            Name = "C1"
         };
         var plans = new List<PlanLowDTO>
         {
            new PlanLowDTO
            {
               Id = planId,
               PartId = partId,
               Name = "plan1",
            }
         };

         var characteristics = Enumerable.Empty<CharacteristicDTO>().ToList();
         characteristics.Add(characteristic1);

         var planDataResults =
            new DataServiceResult<IEnumerable<PlanLowDTO>>(new HttpResponseMessage(HttpStatusCode.OK),
               plans);
         _dataServiceClient.Setup(x => x.GetAllPlansForPart<PlanLowDTO>(partId, It.IsAny<VersionFilter>()))
            .Returns(Task.Run(() => planDataResults));

         var service = new PersistenceDataService(_dataServiceClient.Object);

         // Act
         var result =
            (SuccessResult<IEnumerable<PlanLowDTO>>)service
               .GetAllLatestPlansLowByPart(partId).Result;

         // Assert
         Assert.AreEqual(result.Result.Count(), plans.Count);
         Assert.AreEqual(result.Result.FirstOrDefault().Id, planId);
      }

      [Test]
      public void GetAllMeasurementResultsShouldReturnDomainObjects()
      {
         // Arrange
         var measurementResultId1 = Guid.NewGuid();
         var measurementResultId2 = Guid.NewGuid();
         var measurementResults = new List<MeasurementResultLowDTO>()
         {
            new MeasurementResultLowDTO()
            {
               Id = measurementResultId1
            },
            new MeasurementResultLowDTO()
            {
               Id = measurementResultId2
            }
         };

         var measurementDataResults =
            new DataServiceResult<IEnumerable<MeasurementResultLowDTO>>(new HttpResponseMessage(HttpStatusCode.OK),
               measurementResults);
         _dataServiceClient.Setup(x => x.GetAllMeasurementResults<MeasurementResultLowDTO>()).Returns(Task.Run(() => measurementDataResults));

         var service = new PersistenceDataService(_dataServiceClient.Object);

         // Act
         var result =
            (SuccessResult<IEnumerable<MeasurementResultLowDTO>>)service
               .GetAllMeasurementResults().Result;

         // Assert
         Assert.AreEqual(result.Result.Count(), measurementResults.Count);
         Assert.AreEqual(result.Result.FirstOrDefault().Id, measurementResultId1);
      }

      [Test]
      public void GetAllReportTemplatesLowShouldReturnDomainObjects()
      {
         // Arrange
         var reportTemplateGuid = Guid.NewGuid();
         var reportTemplateGuid2 = Guid.NewGuid();
         var reportTemplateResults = new List<ReportTemplateLowDTO>()
         {
            new ReportTemplateLowDTO()
            {
               Id = reportTemplateGuid
            },
            new ReportTemplateLowDTO()
            {
               Id = reportTemplateGuid2
            }
         };

         var reportTemplateDataResults =
            new DataServiceResult<IEnumerable<ReportTemplateLowDTO>>(new HttpResponseMessage(HttpStatusCode.OK),
               reportTemplateResults);
         _dataServiceClient.Setup(x => x.GetAllReportTemplates<ReportTemplateLowDTO>())
            .Returns(Task.Run(() => reportTemplateDataResults));

         var service = new PersistenceDataService(_dataServiceClient.Object);

         // Act
         var result =
            (SuccessResult<IEnumerable<ReportTemplateLowDTO>>)service.GetReportTemplatesLow().Result;
         // Assert
         Assert.AreEqual(result.Result.Count(), reportTemplateResults.Count);
         Assert.AreEqual(result.Result.FirstOrDefault().Id, reportTemplateGuid);
      }

      [Test]
      public void GetReportTemplateMedByIdShouldReturnDomainObject()
      {
         // Arrange
         var reportTemplateGuid = Guid.NewGuid();
         var reportTemplateResults =
            new ReportTemplateMediumDTO()
            {
               Id = reportTemplateGuid
            };

         var reportTemplateDataResults =
            new DataServiceResult<ReportTemplateMediumDTO>(new HttpResponseMessage(HttpStatusCode.OK),
               reportTemplateResults);
         _dataServiceClient.Setup(x => x.GetReportTemplate<ReportTemplateMediumDTO>(reportTemplateGuid))
            .Returns(Task.Run(() => reportTemplateDataResults));

         var service = new PersistenceDataService(_dataServiceClient.Object);

         // Act
         var result =
            (SuccessResult<ReportTemplateMediumDTO>)service.GetReportTemplateMed(reportTemplateGuid).Result;

         // Assert
         Assert.AreEqual(result.Result.Id, reportTemplateGuid);
      }

      [Test]
      public async Task DeleteReportTemplateShouldCallDeleteReportTemplateServiceOnce()
      {
         // Arrange
         var reportTemplateGuid = Guid.NewGuid();

         _dataServiceClient.Setup(x => x.DeleteReportTemplate(reportTemplateGuid))
            .Returns(Task.Run(() =>
               new DataServiceResult(new HttpResponseMessage(HttpStatusCode.OK), new DataServiceError("error"))));

         var service = new PersistenceDataService(_dataServiceClient.Object);

         // Act
         await service.DeleteReportTemplate(reportTemplateGuid);

         // Assert
         _dataServiceClient.Verify(ds => ds.DeleteReportTemplate(reportTemplateGuid), Times.Once);
      }

      [Test]
      public async Task UpdateReportTemplate_ShouldReturnError()
      {
         //Arrange
         const string ERROR_MESSAGE = "Dataservice error";
         var dataServiceResult = new DataServiceResult(new HttpResponseMessage(HttpStatusCode.BadRequest), new DataServiceError(ERROR_MESSAGE));

         _dataServiceClient.Setup(dsc => dsc.UpdateReportTemplate(It.Is<Guid>(g => g == Guid.Empty), null))
            .Returns(Task.FromResult(dataServiceResult));

         var service = new PersistenceDataService(_dataServiceClient.Object);

         //Act
         var result = await service.UpdateReportTemplate(Guid.Empty, null);

         //Assert
         Assert.IsTrue(result.IsError);
         Assert.AreEqual(ERROR_MESSAGE, (result as ErrorResult).ErrorMessage);
      }

      [Test]
      public async Task UpdateReportTemplate_ShouldReturnSuccess()
      {
         //Arrange
         var dataServiceResult = new DataServiceResult(new HttpResponseMessage(HttpStatusCode.OK), null);
         var reportTemplateId = Guid.NewGuid();
         var reportTemplateUpdateDTO = new ReportTemplateUpdateDTO
         {
            Name = "Report Template",
            PlanId = Guid.NewGuid(),
            PlanVersion = 1,
            ReadOnly = true,
            Template = "Template"
         };

         _dataServiceClient.Setup(dsc => dsc.UpdateReportTemplate(reportTemplateId, reportTemplateUpdateDTO))
            .Returns(Task.FromResult(dataServiceResult));

         var service = new PersistenceDataService(_dataServiceClient.Object);

         //Act
         var result = await service.UpdateReportTemplate(reportTemplateId, reportTemplateUpdateDTO) as SuccessResult<ReportTemplateMediumDTO>;

         //Assert
         Assert.IsTrue(result.IsSuccess);
         Assert.AreEqual(reportTemplateId, result.Result.Id);
         Assert.AreEqual(reportTemplateUpdateDTO.Name, result.Result.Name);
         Assert.AreEqual(reportTemplateUpdateDTO.Template, result.Result.Template);
      }

      [Test]
      public async Task GetAllPlansLow_ShouldReturnError()
      {
         //Arrange
         const string ERROR_MESSAGE = "Dataservice error";
         var dataServiceResult = new DataServiceResult<IEnumerable<PlanLowDTO>>(new HttpResponseMessage(HttpStatusCode.BadRequest), new DataServiceError(ERROR_MESSAGE));

         _dataServiceClient.Setup(dsc => dsc.GetAllPlans<PlanLowDTO>(It.Is<VersionFilter>(v => v.FilterVersionType == FilterVersionType.Last), null))
            .Returns(Task.FromResult(dataServiceResult));

         var service = new PersistenceDataService(_dataServiceClient.Object);

         //Act
         var result = await service.GetAllLatestPlansLow();

         //Assert
         Assert.IsTrue(result.IsError);
         Assert.AreEqual(ERROR_MESSAGE, (result as ErrorResult).ErrorMessage);
      }

      [Test]
      public async Task GetAllPlansLow_ShouldReturnTwoPlanLowDtos()
      {
         //Arrange
         var planDtos = new[]
         {
            new PlanLowDTO{ Id = Guid.NewGuid(), Name = "Plan 1", Version = 1 },
            new PlanLowDTO{ Id = Guid.NewGuid(), Name = "Plan 2", Version = 2 },
         };

         var dataServiceResult = new DataServiceResult<IEnumerable<PlanLowDTO>>(new HttpResponseMessage(HttpStatusCode.OK), planDtos);

         _dataServiceClient.Setup(dsc => dsc.GetAllPlans<PlanLowDTO>(It.Is<VersionFilter>(v => v.FilterVersionType == FilterVersionType.Last), null))
            .Returns(Task.FromResult(dataServiceResult));

         var service = new PersistenceDataService(_dataServiceClient.Object);

         //Act
         var result = await service.GetAllLatestPlansLow() as SuccessResult<IEnumerable<PlanLowDTO>>;

         //Assert
         Assert.IsTrue(result.IsSuccess);
         CollectionAssert.AreEquivalent(planDtos, result.Result);
      }

      [Test]
      public async Task GetMeasurementResultShouldReturnMeasurmentResultDto()
      {
         var measurmentResult = new MeasurementResultMediumDTO() { Id = Guid.NewGuid(), PlanId = Guid.NewGuid(), TimeStamp = new DateTime() };
         var dataServiceResult = new DataServiceResult<MeasurementResultMediumDTO>(new HttpResponseMessage(HttpStatusCode.OK), measurmentResult);

         _dataServiceClient.Setup(dsc => dsc.GetMeasurementResult<MeasurementResultMediumDTO>(measurmentResult.Id)).Returns(Task.Run(() => dataServiceResult));

         var service = new PersistenceDataService(_dataServiceClient.Object);

         var result = await service.GetMeasurementResult(measurmentResult.Id) as SuccessResult<MeasurementResultMediumDTO>;
         Assert.AreEqual(result.Result, measurmentResult);
      }

      [Test]
      public async Task DeletePlanShouldCallDeletePlanServiceOnce()
      {
         // Arrange
         var planGuid = Guid.NewGuid();
         var version = VersionFilter.All();

         _dataServiceClient.Setup(x => x.DeletePlan(planGuid, version))
            .Returns(Task.Run(() =>
               new DataServiceResult(new HttpResponseMessage(HttpStatusCode.OK), new DataServiceError("error"))));

         var service = new PersistenceDataService(_dataServiceClient.Object);

         // Act
         await service.DeletePlan(planGuid, version);

         // Assert
         _dataServiceClient.Verify(ds => ds.DeletePlan(planGuid, version), Times.Once);
      }

      [Test]
      public async Task DeletePartShouldCallDeletePlanServiceOnce()
      {
         // Arrange
         var partGuid = Guid.NewGuid();

         _dataServiceClient.Setup(x => x.DeletePart(partGuid))
            .Returns(Task.Run(() =>
               new DataServiceResult(new HttpResponseMessage(HttpStatusCode.OK), new DataServiceError("error"))));

         var service = new PersistenceDataService(_dataServiceClient.Object);

         // Act
         var result = await service.DeletePart(partGuid);

         // Assert
         _dataServiceClient.Verify(ds => ds.DeletePart(partGuid), Times.Once);
      }

      [Test]
      public async Task DeleteMeasurementeResultShouldCallDeletePlanServiceOnce()
      {
         // Arrange
         var mrGuid = Guid.NewGuid();

         _dataServiceClient.Setup(x => x.DeleteMeasurementResult(mrGuid))
            .Returns(Task.Run(() =>
               new DataServiceResult(new HttpResponseMessage(HttpStatusCode.OK), new DataServiceError("error"))));

         var service = new PersistenceDataService(_dataServiceClient.Object);

         // Act
         var result = await service.DeleteMeasurementResult(mrGuid);

         // Assert
         _dataServiceClient.Verify(ds => ds.DeleteMeasurementResult(mrGuid), Times.Once);
      }

      [Test]
      public async Task AddReportTemplateMed_ShouldReturnSuccess()
      {
         // Arrange
         var reportTemplateDTO = new ReportTemplateCreateDTO()
         {
            Name = "Name",
            Template = "Template"
         };

         var reportTemplateMediumDTO = new ReportTemplateMediumDTO()
         {
            Id = Guid.NewGuid(),
            Name = reportTemplateDTO.Name,
            Template = reportTemplateDTO.Template
         };

         var dataServiceResult = new DataServiceResult<ReportTemplateMediumDTO>(new HttpResponseMessage(HttpStatusCode.OK), reportTemplateMediumDTO);

         _dataServiceClient.Setup(x => x.AddReportTemplate(It.Is<ReportTemplateCreateDTO>(r => r.Name == reportTemplateDTO.Name && r.Template == reportTemplateDTO.Template ))).Returns(Task.FromResult(dataServiceResult));

         var service = new PersistenceDataService(_dataServiceClient.Object);

         // Act
         var result = await service.AddReportTemplateMed("Name", "Template") as SuccessResult<ReportTemplateMediumDTO>;

         // Assert
         Assert.IsTrue(result.IsSuccess);
         Assert.AreEqual(reportTemplateMediumDTO.Id, result.Result.Id);
         Assert.AreEqual(reportTemplateMediumDTO.Name, result.Result.Name);
         Assert.AreEqual(reportTemplateMediumDTO.Template, result.Result.Template);
      }

      [Test]
      public async Task AddReportTemplateMed_ShouldReturnError()
      {
         // Arrange
         var reportTemplateDTO = new ReportTemplateCreateDTO()
         {
            Name = "Name",
            Template = "Template"
         };

         var reportTemplateMediumDTO = new ReportTemplateMediumDTO()
         {
            Id = Guid.NewGuid(),
            Name = reportTemplateDTO.Name,
            Template = reportTemplateDTO.Template
         };

         var dataServiceResult = new DataServiceResult<ReportTemplateMediumDTO>(new HttpResponseMessage(HttpStatusCode.BadRequest), new DataServiceError("Data Service Error"));

         _dataServiceClient.Setup(x => x.AddReportTemplate(It.Is<ReportTemplateCreateDTO>(r => r.Name == reportTemplateDTO.Name && r.Template == reportTemplateDTO.Template))).Returns(Task.FromResult(dataServiceResult));

         var service = new PersistenceDataService(_dataServiceClient.Object);

         // Act
         var result = await service.AddReportTemplateMed("Name", "Template") as SuccessResult<ReportTemplateMediumDTO>;

         // Assert
         Assert.IsNull(result);
      }
   }
}