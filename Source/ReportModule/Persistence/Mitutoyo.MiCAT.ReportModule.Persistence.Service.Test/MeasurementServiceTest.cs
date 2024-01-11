// <copyright file="MeasurementServiceTest.cs" company="Mitutoyo Europe GmbH">
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
using AutoMapper;
using Mitutoyo.MiCAT.DataServiceClient;
using Mitutoyo.MiCAT.ReportModule.Persistence;
using Mitutoyo.MiCAT.ReportModule.Persistence.Service;
using Mitutoyo.MiCAT.ReportModule.Setup.Configurations;
using Mitutoyo.MiCAT.Web.Data;
using Mitutoyo.MiCAT.Web.Data.Common;
using Moq;
using NUnit.Framework;

namespace Mitutoyo.MiCAT.ReportModule.Service.Test
{
   [ExcludeFromCodeCoverage]
   [TestFixture]
   public class MeasurementServiceTest
   {
      private IMapper _mapper;
      private Mock<IDataServiceClient> _dataServiceClient;
      private List<MeasurementResultLowDTO> _measurementResults;

      [SetUp]
      public virtual void SetUp()
      {
         _mapper = AutoMapperConfig.InitializeAutoMapper().CreateMapper();
         _dataServiceClient = new Mock<IDataServiceClient>();

         var measurementResultId1 = Guid.NewGuid();
         var measurementResultId2 = Guid.NewGuid();
         var measurementResultId3 = Guid.NewGuid();
         var planId1 = Guid.NewGuid();
         var planId2 = Guid.NewGuid();
         _measurementResults = new List<MeasurementResultLowDTO>()
         {
            new MeasurementResultLowDTO()
            {
              Id = measurementResultId1,
              PlanId = planId1,
              TimeStamp = new DateTime(2010,1,1)
            },
            new MeasurementResultLowDTO()
            {
               Id = measurementResultId2,
               PlanId = planId1,
               TimeStamp = new DateTime(2010,2,1)
            },
            new MeasurementResultLowDTO()
            {
               Id = measurementResultId3,
               PlanId = planId2,
               TimeStamp = new DateTime(2010,1,15)
            }
         };

         var characteristic1 = new CharacteristicDTO()
         {
            CharacteristicType = Web.Data.CharacteristicType.Circularity,
            Id = Guid.NewGuid(),
            Name = "C1",
            PropertyValues = new List<PropertyValueDTO>()
            {
               new PropertyValueDTO() { Name = "NominalValue", Value = "2.5" },
               new PropertyValueDTO() { Name = "ToleranceUp", Value = "8" },
               new PropertyValueDTO() { Name = "ToleranceLow", Value = "1" },
               new PropertyValueDTO() { Name = "ToleranceZone", Value = "9" },
               new PropertyValueDTO() { Name = "CharacteristicDetail", Value = "Min" }
            }
         };
         var characteristic2 = new CharacteristicDTO()
         {
            CharacteristicType = Web.Data.CharacteristicType.Flatness,
            Id = Guid.NewGuid(),
            Name = "C2",
            PropertyValues = new List<PropertyValueDTO>()
            {
               new PropertyValueDTO() { Name = "ToleranceUp", Value = "4.1" },
               new PropertyValueDTO() { Name = "ToleranceLow", Value = "2" },
               new PropertyValueDTO() { Name = "ToleranceZone", Value = "5" },
               new PropertyValueDTO() { Name = "CharacteristicDetail", Value = "Max" }
            }
         };

         var characteristic3 = new CharacteristicDTO()
         {
            CharacteristicType = Web.Data.CharacteristicType.Cylindricity,
            Id = Guid.NewGuid(),
            Name = "C3",
            PropertyValues = new List<PropertyValueDTO>()
            {
               new PropertyValueDTO() { Name = "ToleranceLow", Value = "1.2" },
               new PropertyValueDTO() { Name = "ToleranceUp", Value = "4" },
               new PropertyValueDTO() { Name = "ToleranceZone", Value = "6" },
               new PropertyValueDTO() { Name = "CharacteristicDetail", Value = "Invalid Value" }
            }
         };

         var characteristicActuals = Enumerable.Empty<CharacteristicActualDTO>().ToList();
         characteristicActuals.Add(new CharacteristicActualDTO()
         {
            CharacteristicId = characteristic1.Id,
            MeasurementResultId = measurementResultId1,
            Status = EvaluationStatus.Fail,
            PropertyValues = new List<PropertyValueDTO>()
            {
               new PropertyValueDTO() { Name = "Deviation", Value = "4" },
               new PropertyValueDTO() { Name = "Measured", Value = "2.8" }
            }
         });

         characteristicActuals.Add(new CharacteristicActualDTO()
         {
            CharacteristicId = characteristic2.Id,
            MeasurementResultId = measurementResultId2,
            Status = EvaluationStatus.Pass,
            PropertyValues = new List<PropertyValueDTO>()
            {
               new PropertyValueDTO() { Name = "Deviation", Value = "2" },
               new PropertyValueDTO() { Name = "Measured", Value = "1.3" }
            }
         });

         characteristicActuals.Add(new CharacteristicActualDTO()
         {
            CharacteristicId = characteristic3.Id,
            MeasurementResultId = measurementResultId3,
            Status = EvaluationStatus.Pass,
            PropertyValues = new List<PropertyValueDTO>()
            {
               new PropertyValueDTO() { Name = "Deviation", Value = "8" },
               new PropertyValueDTO() { Name = "Measured", Value = "4.2" }
            }
         });

         DataServiceResult<ServiceInfoDTO> result =
            new DataServiceResult<ServiceInfoDTO>(
               new HttpResponseMessage(HttpStatusCode.OK),
               new ServiceInfoDTO());

         _dataServiceClient.Setup(x => x.GetServiceInfo()).Returns(Task.FromResult(result));

         var measurementDataResults = new DataServiceResult<IEnumerable<MeasurementResultLowDTO>>(new HttpResponseMessage(HttpStatusCode.OK), _measurementResults);
         _dataServiceClient
            .Setup(x => x.GetAllMeasurementResults<MeasurementResultLowDTO>())
            .Returns(Task.FromResult(measurementDataResults));

         var characteristicActualDataResult = new DataServiceResult<IEnumerable<CharacteristicActualDTO>>(new HttpResponseMessage(HttpStatusCode.OK), characteristicActuals);
         _dataServiceClient.Setup(x => x.GetAllCharacteristicActuals(measurementResultId1))
            .Returns(Task.Run(() => characteristicActualDataResult));

         var allCharacteristicsResult = Enumerable.Empty<CharacteristicDTO>().ToList();
         allCharacteristicsResult.Add(characteristic1);
         allCharacteristicsResult.Add(characteristic2);
         allCharacteristicsResult.Add(characteristic3);

         List<CharacteristicDTO> characteristics = new List<CharacteristicDTO>()
         {
            characteristic1, characteristic2, characteristic3
         };

         var allPlansDataResult = new DataServiceResult<IEnumerable<PlanLowDTO>>(new HttpResponseMessage(HttpStatusCode.OK), new List<PlanLowDTO>() { new PlanLowDTO() { Id = planId1, Name = "Plan1" } });
         var plansByIdResult = new DataServiceResult<IEnumerable<PlanMediumDTO>>(new HttpResponseMessage(HttpStatusCode.OK), new List<PlanMediumDTO>() { new PlanMediumDTO() { Id = planId1, Name = "Plan1", Characteristics = characteristics } });

         _dataServiceClient
            .Setup(x => x.GetAllPlansForPart<PlanLowDTO>(It.IsAny<Guid>(), It.IsAny<VersionFilter>()))
            .Returns(Task.FromResult(allPlansDataResult));
         _dataServiceClient
            .Setup(x => x.GetPlansById<PlanMediumDTO>(It.IsAny<Guid>(), It.IsAny<VersionFilter>()))
            .Returns(Task.FromResult(plansByIdResult));
      }

      [Test]
      public async Task GetMeasurementResultByPartShouldReturnAllMeasurementResultForAnSpecificPart()
      {
         // Arrange
         var service = new PersistenceDataService(_dataServiceClient.Object);

         Mock<IPersistenceServiceLocator> persistenceServiceLocator = new Mock<IPersistenceServiceLocator>();
         persistenceServiceLocator
            .Setup(x => x.GetService<IPersistenceDataService>(Location.DataService, Mechanism.DataService))
            .Returns(service);

         var measurementPersistence = new MeasurementPersistence(persistenceServiceLocator.Object, _mapper);

         // Act
         var id = _measurementResults.FirstOrDefault().PlanId;
         var result = await measurementPersistence.GetRunsByPartId(id);

         // Assert
         Assert.AreEqual(result.Count(), 2);
      }
   }
}
