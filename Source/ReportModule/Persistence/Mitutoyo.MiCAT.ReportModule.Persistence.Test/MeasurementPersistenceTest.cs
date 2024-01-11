// <copyright file="MeasurementPersistenceTest.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
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
using Mitutoyo.MiCAT.ReportModule.Setup.Configurations;
using Mitutoyo.MiCAT.Web.Data;
using Mitutoyo.MiCAT.Web.Data.Common;
using Moq;
using NUnit.Framework;

namespace Mitutoyo.MiCAT.ReportModule.Persistence.Test
{
   [ExcludeFromCodeCoverage]
   [TestFixture]
   public class MeasurementPersistenceTest
   {
      private IMapper _mapper;
      private List<PlanMediumDTO> _planMediumResults;
      private Guid _planId;
      private Guid _partId;
      private Mock<IPersistenceServiceLocator> _persistenceServiceLocator;
      private Mock<IPersistenceDataService> _service;
      private MeasurementPersistence _measurementPersistence;

      [SetUp]
      public virtual void SetUp()
      {
         _mapper = AutoMapperConfig.InitializeAutoMapper().CreateMapper();
         _persistenceServiceLocator = new Mock<IPersistenceServiceLocator>();

         _service = new Mock<IPersistenceDataService>();

         _persistenceServiceLocator
         .Setup(x => x.GetService<IPersistenceDataService>(Location.DataService, Mechanism.DataService))
         .Returns(_service.Object);

         _planId = Guid.NewGuid();
         _partId = Guid.NewGuid();

         _planMediumResults = new List<PlanMediumDTO>()
         {
            new PlanMediumDTO
            {
               Characteristics = new List<CharacteristicDTO>()
               {
                  new CharacteristicDTO()
                     {CharacteristicType = CharacteristicType.Angle, Id = Guid.NewGuid(), Name = "Ch1"},
                  new CharacteristicDTO()
                     {CharacteristicType = CharacteristicType.Angle, Id = Guid.NewGuid(), Name = "Ch2"},
               },
               Id = _planId,
               Name = "Plan1",
               Version = 0,
               PartId = _partId
            }
         };

         _measurementPersistence = new MeasurementPersistence(_persistenceServiceLocator.Object, _mapper);
      }

      [Test]
      public async Task GetRunsForSelectedPlanOrPartShouldReturnsRunsForPlan()
      {
         //Arrange
         var selectedPlan = new PlanDescriptor { Id = Guid.NewGuid() };

         var expectedRuns = Enumerable
            .Range(0, 3)
            .Select(x => new MeasurementResultLowDTO
            {
               Id = Guid.NewGuid(),
               TimeStamp = DateTime.Now
            }).ToArray();

         _service.Setup(s => s.GetAllMeasurementResultsForPlan(It.Is<Guid>(g => g == selectedPlan.Id)))
            .ReturnsAsync(new SuccessResult<IEnumerable<MeasurementResultLowDTO>>(expectedRuns));

         //Act
         var runs = await _measurementPersistence.GetRunsForSelectedPlanOrPart(selectedPlan);

         //Assert
         CollectionAssert.AreEquivalent(expectedRuns.Select(r => r.Id), runs.Select(r => r.Id));
      }

      [Test]
      public async Task GetRunsForSelectedPlanOrPartShouldReturnsEmptyRuns()
      {
         //Act
         var runs = await _measurementPersistence.GetRunsForSelectedPlanOrPart(null);

         //Assert
         CollectionAssert.IsEmpty(runs);
      }

      [Test]
      public async Task GetRunsByPartIdShouldReturnRunsForEachPlanVersion()
      {
         // Arrange
         List<PlanLowDTO> planLowDtos = new List<PlanLowDTO>
         {
            new PlanLowDTO
            {
               Id = _planId,
               Name = "Plan1",
               Version = 0,
               PartId = _partId
            },
            new PlanLowDTO
            {
               Id = _planId,
               Name = "Plan1 v1",
               Version = 1,
               PartId = _partId
            }
         };
         var planResults =
            new DataServiceResult<IEnumerable<PlanLowDTO>>(new HttpResponseMessage(HttpStatusCode.OK), planLowDtos);

         var measurementDtos = new List<MeasurementResultLowDTO>()
         {
            new MeasurementResultLowDTO
            {
               Id = Guid.NewGuid(),
               PlanId = _planId,
               PlanVersion = 0,
               DMEName = "M1",
            },
            new MeasurementResultLowDTO
            {
               Id = Guid.NewGuid(),
               PlanId = _planId,
               PlanVersion = 1,
               DMEName = "M2",
            },
            new MeasurementResultLowDTO
            {
               Id = Guid.NewGuid(),
               PlanId = Guid.NewGuid(),
               PlanVersion = 0,
               DMEName = "M3",
            }
         };

         _service.Setup(s => s.GetAllLatestPlansLowByPart(_partId)).Returns(Task.FromResult(new SuccessResult<IEnumerable<PlanLowDTO>>(planLowDtos) as Result));
         _service.Setup(x => x.GetAllMeasurementResults()).Returns(Task.FromResult(new SuccessResult<IEnumerable<MeasurementResultLowDTO>>(measurementDtos) as Result));

         // Act
         var result = await _measurementPersistence.GetRunsByPartId(_partId);

         // Assert
         Assert.AreEqual(result.Count(), 2);
      }

      [Test]
      public void GetRunsByPlanIdShouldReturnRunsForPlan()
      {
         // Arrange
         var measurementDtos = new List<MeasurementResultLowDTO>()
         {
            new MeasurementResultLowDTO
            {
               Id = Guid.NewGuid(),
               PlanId = _planId
            },
            new MeasurementResultLowDTO
            {
               Id = Guid.NewGuid(),
               PlanId = _planId
            },
         };

         _service.Setup(x => x.GetAllMeasurementResultsForPlan(_planId)).Returns(Task.FromResult(new SuccessResult<IEnumerable<MeasurementResultLowDTO>>(measurementDtos) as Result));

         // Act
         var result = _measurementPersistence.GetRunsByPlanId(_planId).Result;

         // Assert
         Assert.AreEqual(result.Count(), 2);
         _service.Verify(s => s.GetAllMeasurementResultsForPlan(_planId), Times.Once);
      }

      [Test]
      public void GetCharacteristicTypesShouldReturnAllCharacteristicTypeNames()
      {
         // Act
         var result = _measurementPersistence.GetCharacteristicTypes();

         // Assert
         Assert.NotNull(result);
         Assert.IsNotEmpty(result);
         Assert.AreEqual(Enum.GetValues(typeof(CharacteristicType)).Length, result.Count());
      }
      [Test]
      public void DeleteMeasuremetResultShoulThrowExceptionWhenError()
      {
         // Arrange
         var runId = Guid.NewGuid();

         Result errorResult = new ErrorResult("Error", ResultErrorCode.BadRequest);

         // Act
         _service.Setup(x => x.DeleteMeasurementResult(runId)).Returns(Task.FromResult(errorResult));

         // Assert
         Assert.CatchAsync<ResultException>(async () => await _measurementPersistence.DeleteMeasurementResultById(runId));
      }
      [Test]
      public void DeleteNotExistingRunShoulReturnRunCouldNotBeDeletedException()
      {
         // Arrange
         var runId = Guid.NewGuid();
         Result errorResult = new ErrorResult("Error", ResultErrorCode.NotFound);

         // Act
         _service.Setup(x => x.DeleteMeasurementResult(runId)).Returns(Task.FromResult(errorResult));

         // Assert
         Assert.CatchAsync<RunCouldNotBeDeletedException>(async () => await _measurementPersistence.DeleteMeasurementResultById(runId));
      }
      [Test]
      public async Task DeleteErrorShoulReturnDeletingDataErrorKey()
      {
         // Arrange
         var runId = Guid.NewGuid();
         Result errorResult = new ErrorResult("Error", ResultErrorCode.BadRequest);

         // Act
         _service.Setup(x => x.DeleteMeasurementResult(runId)).Returns(Task.FromResult(errorResult));

         // Assert
         try
         {
            await _measurementPersistence.DeleteMeasurementResultById(runId);
         }
         catch (ResultException ex)
         {
            Assert.AreEqual(ex.Key, "DeletingDataError");
         }
      }

      [Test]
      public void GetRunDetailShouldReturnRunDataProperly()
      {
         // Arrange
         const string NOMINAL_VALUE_KEY = "NominalValue";
         const string UPPER_TOLERANCE_VALUE_KEY = "ToleranceUp";
         const string LOWER_TOLERANCE_VALUE_KEY = "ToleranceLow";
         const string TOLERANCE_ZONE_VALUE_KEY = "ToleranceZone";
         const string DEVIATION_VALUE_KEY = "Deviation";
         const string DETAIL_VALUE_KEY = "CharacteristicDetail";
         const string MEASURED_VALUE_KEY = "Measured";

         const string NAME1 = "Name 1";
         const string NAME2 = "Name 2";

         var charId = Guid.NewGuid();

         var measurementResultDTO = new MeasurementResultMediumDTO()
         {
            Id = Guid.NewGuid(),
            PlanId = _planId,
            PropertyValues = new[]
            {
               new PropertyValueDTO { Name = NAME1, Value = "Value 1" },
               new PropertyValueDTO { Name = NAME2, Value = "Value 2" }
            },
            CharacteristicActuals = new CharacteristicActualDTO[]
            {
               new CharacteristicActualDTO
               {
                  CharacteristicId = charId,
                  Id = Guid.NewGuid(),
                  Status = EvaluationStatus.Pass,
                  PropertyValues = new[]
                  {
                     new PropertyValueDTO { Name = DEVIATION_VALUE_KEY, Value = "10.0"},
                     new PropertyValueDTO { Name = MEASURED_VALUE_KEY, Value = "11.0"},
                  }
               }
            }
         };

         var attachments = new List<AttachmentInfo>();

         //1x1 PNG image
         var attachmentData = Convert.FromBase64String("iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAIAAACQd1PeAAAACXBIWXMAAAsTAAALEwEAmpwYAAAADElEQVQImWNIKyoCAAKMAUuJWnujAAAAAElFTkSuQmCC");

         attachments.Add(new AttachmentInfo()
         {
            Id = Guid.NewGuid(),
            Name = "attachment1",
            Data = new MemoryStream(attachmentData),
            Index = 0,
            Type = AttachmentType.Capture3D
         });

         var measurementResultDTOWithAttachment = new WithAttachments<MeasurementResultMediumDTO>()
         {
            Entity = measurementResultDTO,
            Attachments = attachments
         };

         var featureId = Guid.NewGuid();

         var planMediumDto = new PlanMediumDTO
         {
            Characteristics = new[]
            {
                new CharacteristicDTO
                {
                   CharacteristicType = CharacteristicType.Circularity,
                   FeatureId = featureId,
                   Id = charId,
                   Name = "Char1",
                   PropertyValues = new[]
                   {
                      new PropertyValueDTO  { Name = NOMINAL_VALUE_KEY, Value = "1.0" },
                      new PropertyValueDTO  { Name = UPPER_TOLERANCE_VALUE_KEY, Value = "2.0" },
                      new PropertyValueDTO  { Name = LOWER_TOLERANCE_VALUE_KEY, Value = "3.0" },
                      new PropertyValueDTO  { Name = TOLERANCE_ZONE_VALUE_KEY, Value = "4.0" },
                      new PropertyValueDTO  { Name = DETAIL_VALUE_KEY, Value = "Detail" },
                   }
                }
             },

            Features = new[]
             {
                new FeatureDTO { Id = featureId, FeatureType = FeatureType.Circle, Name = "Feature1"  }
             }
         };

         Result successMeasurementResult = new SuccessResult<WithAttachments<MeasurementResultMediumDTO>>(measurementResultDTOWithAttachment);
         Result successPlanVersionResult = new SuccessResult<IEnumerable<PlanMediumDTO>>(new[] { planMediumDto });

         var planVersion = VersionFilter.SpecificVersion(measurementResultDTO.PlanVersion);

         _service.Setup(x => x.GetPlanVersionsMed(It.Is<Guid>(id => id == measurementResultDTO.PlanId), It.Is<VersionFilter>(v => v.Version == planVersion.Version))).Returns(Task.FromResult(successPlanVersionResult));
         _service.Setup(x => x.GetMeasurementResultWithAttachments(measurementResultDTO.Id)).Returns(Task.FromResult(successMeasurementResult));

         // Act
         var result = _measurementPersistence.GetRunDetail(measurementResultDTO.Id).Result;

         // Assert
         Assert.AreEqual(1, result.CharacteristicList.Count);
         Assert.AreEqual(result.CharacteristicList[0].CharacteristicActual.CharacteristicId, result.CharacteristicList[0].Characteristic.Id);
         Assert.AreEqual(featureId, result.CharacteristicList[0].Characteristic.Feature.Id);
         Assert.AreEqual("Feature1", result.CharacteristicList[0].Characteristic.Feature.Name);
         Assert.AreEqual(EvaluationStatus.Pass.ToString(), result.CharacteristicList[0].CharacteristicActual.Status);
         Assert.AreEqual(2, result.DynamicPropertyValues.Count);
         Assert.IsTrue(result.DynamicPropertyValues.Any(d => d.Name == NAME1));
         Assert.IsTrue(result.DynamicPropertyValues.Any(d => d.Name == NAME2));

         Assert.AreEqual(1, result.Captures3D.Count);
         Assert.AreEqual("attachment1", result.Captures3D[0].Name);
         Assert.AreEqual(1, result.Captures3D[0].Image.Width);
         Assert.AreEqual(1, result.Captures3D[0].Image.Height);

         // Dispose
         attachments[0].Data.Dispose();
      }
   }
}
