// <copyright file="PlanPersistenceTest.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
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
   public class PlanPersistenceTest
   {
      private Mock<IPersistenceServiceLocator> _persistenceServiceLocatorMock;
      private IMapper _mapper;
      private Mock<IPersistenceDataService> _service;
      private Mock<IPartPersistence> _partPersistenceMock;
      private PlanPersistence _sut;

      [SetUp]
      public virtual void SetUp()
      {
         _mapper = AutoMapperConfig.InitializeAutoMapper().CreateMapper();
         _persistenceServiceLocatorMock = new Mock<IPersistenceServiceLocator>();
         _partPersistenceMock = new Mock<IPartPersistence>();
         _service = new Mock<IPersistenceDataService>();

         _persistenceServiceLocatorMock
            .Setup(p => p.GetService<IPersistenceDataService>(
               It.Is<Location>(l => l == Location.DataService),
               It.Is<Mechanism>(m => m == Mechanism.DataService)))
            .Returns(_service.Object);

         _sut = new PlanPersistence(_persistenceServiceLocatorMock.Object, _mapper, _partPersistenceMock.Object);
      }

      [Test]
      public void GetPlansShouldThrowAResultException()
      {
         //Arrange
         const string ERROR_RESULT_KEY = "Error DataService Test";

         _service.Setup(s => s.GetAllLatestPlansLow())
            .Returns(Task.FromResult<Result>(new ErrorResult(ERROR_RESULT_KEY)));

         //Act
         var ex = Assert.ThrowsAsync<ResultException>(async () => await _sut.GetPlans());

         //Assert
         Assert.AreEqual("RetrievingDataError", ex.Key);
         Assert.AreEqual(ERROR_RESULT_KEY, ex.Message);
      }

      [Test]
      public async Task GetPlansShouldReturnPlans()
      {
         var planDTOs = new[]
{
            new PlanLowDTO { Name = "Plan 1"},
            new PlanLowDTO { Name = "Plan 2"}
         };

         _service.Setup(s => s.GetAllLatestPlansLow())
            .Returns(Task.FromResult<Result>(new SuccessResult<IEnumerable<PlanLowDTO>>(planDTOs.AsEnumerable())));

         //Act
         var result = await _sut.GetPlans();

         //Assert
         Assert.AreEqual(2, result.Count());
         Assert.AreEqual(planDTOs[0].Name, result.ToList()[0].Name);
         Assert.AreEqual(planDTOs[1].Name, result.ToList()[1].Name);
      }

      [Test]
      public async Task DeletePlanShoulCallServiceOnce()
      {
         var plan = new PlanDescriptor() { Id = System.Guid.NewGuid() };

         var queryResult = new Result(ResultState.Success);
         _service.Setup(s => s.DeletePlan(plan.Id, It.IsAny<VersionFilter>()))
            .Returns(Task.FromResult(queryResult));

         //Act
         await _sut.DeletePlan(plan.Id);

         //Assert
         _service.Verify(s => s.DeletePlan(plan.Id, It.IsAny<VersionFilter>()), Times.Once);
      }
      [Test]
      public void DeleteNotExistingPlanShouldReturnPlanCouldNotBeDeletedException()
      {
         var plan = new PlanDescriptor() { Id = System.Guid.NewGuid() };

         var queryResult = new ErrorResult("Not Found", ResultErrorCode.NotFound);

         _service.Setup(s => s.DeletePlan(plan.Id, It.IsAny<VersionFilter>()))
            .Returns(Task.FromResult((Result)queryResult));

         Assert.ThrowsAsync<PlanCouldNotBeDeletedException>(async () => await _sut.DeletePlan(plan.Id));
      }
      [Test]
      public void DeleteErrorPlanShouldReturnDeletingDataErrorKey()
      {
         var plan = new PlanDescriptor() { Id = System.Guid.NewGuid() };

         var queryResult = new ErrorResult("error", ResultErrorCode.BadRequest);

         _service.Setup(s => s.DeletePlan(plan.Id, It.IsAny<VersionFilter>()))
            .Returns(Task.FromResult((Result)queryResult));

         var ret = Assert.CatchAsync<ResultException>(() => _sut.DeletePlan(plan.Id));
         Assert.That(ret.Key == "DeletingDataError");
      }
   }
}
