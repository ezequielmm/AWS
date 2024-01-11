// <copyright file="RunSelectionRequestClientTest.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.ReportModule.Domain.Data;
using Mitutoyo.MiCAT.ReportModule.Domain.DataResult;
using Mitutoyo.MiCAT.ReportModule.Persistence;
using Mitutoyo.MiCAT.ReportModuleApp.Clients;
using Mitutoyo.MiCAT.ReportModuleApp.Controllers;
using Mitutoyo.MiCAT.ReportModuleApp.Services.PlansRuns;
using Mitutoyo.MiCAT.ReportModuleApp.Utilities;
using Moq;
using NUnit.Framework;

namespace Mitutoyo.MiCAT.ReportModule.App.Test.Clients
{
   [ExcludeFromCodeCoverage]
   [TestFixture]
   public class RunSelectionRequestClientTest
   {
      private Mock<IMeasurementPersistence> _measurementPersistenceMock;
      private Mock<IAppStateHistory> _appStateHistoryMock;
      private Mock<IMessageNotifier> _notifierMock;
      private Mock<IRunController> _runController;
      private Mock<IPlanPersistence> _planPersistence;
      private Mock<IMeasurementDataSourceRefresherService> _measurementDataSourceRefresherService;

      private RunSelectionRequestClient _runSelectionRequestClientToTest;

      [SetUp]
      public virtual void Setup()
      {
         _measurementPersistenceMock = new Mock<IMeasurementPersistence>();
         _appStateHistoryMock = new Mock<IAppStateHistory>();
         _notifierMock = new Mock<IMessageNotifier>();
         _runController = new Mock<IRunController>();
         _planPersistence = new Mock<IPlanPersistence>();
         _measurementDataSourceRefresherService = new Mock<IMeasurementDataSourceRefresherService>();

         _appStateHistoryMock.Setup(a => a.AddClient(It.IsAny<object>(), It.IsAny<int>())).Returns(_appStateHistoryMock.Object);
         _runSelectionRequestClientToTest = new RunSelectionRequestClient(_measurementPersistenceMock.Object,
            _appStateHistoryMock.Object,
            _notifierMock.Object,
            _runController.Object,
            _measurementDataSourceRefresherService.Object,
            _planPersistence.Object);
      }

      [Test]
      public async Task WhenRequestARunWithErrorShouldRefreshList()
      {
         //Arrange
         var snapshot = new Mock<ISnapShot>();
         var resultException = new ResultException("RunNotFound", "RunNotFound");
         var runRequest = new RunSelectionRequest();
         var planState = new PlansState(ImmutableList<PlanDescriptor>.Empty).With(null, ImmutableList<RunDescriptor>.Empty);

         runRequest = runRequest.WithNewRequest(Guid.NewGuid());

         snapshot.Setup(ss => ss.GetItems<RunSelectionRequest>()).Returns(new RunSelectionRequest[] { runRequest });
         snapshot.Setup(ss => ss.GetItems<PlansState>()).Returns(new[] { planState });

         _measurementPersistenceMock.Setup(m => m.GetRunDetail(runRequest.RunRequestedId.Value)).Throws(resultException);
         _appStateHistoryMock.SetupGet(h => h.CurrentSnapShot).Returns(snapshot.Object);
         // Act
         await _runSelectionRequestClientToTest.Update(snapshot.Object);

         // Assert
         _notifierMock.Verify(n => n.NotifyError(resultException), Times.Once);
         _appStateHistoryMock.Verify(aps => aps.Run(It.IsAny<Expression<Func<ISnapShot, ISnapShot>>>()), Times.Once);
      }

      [Test]
      public async Task WhenAttendCompletedRequestNothingHappen()
      {
         //Arrange
         var snapshot = new Mock<ISnapShot>();
         var runRequest = new RunSelectionRequest();

         runRequest = runRequest.WithCompletedRequest();
         snapshot.Setup(ss => ss.GetItems<RunSelectionRequest>()).Returns(new RunSelectionRequest[] { runRequest });

         // Act
         await _runSelectionRequestClientToTest.Update(snapshot.Object);

         // Assert
         _notifierMock.Verify(n => n.NotifyError(It.IsAny<ResultException>()), Times.Never);
         _measurementDataSourceRefresherService.Verify(r => r.Refresh(It.IsAny<ISnapShot>(), It.IsAny<IEnumerable<PlanDescriptor>>(), It.IsAny<IEnumerable<RunDescriptor>>()), Times.Never);
         _appStateHistoryMock.Verify(aps => aps.Run((System.Linq.Expressions.Expression<Func<ISnapShot, ISnapShot>>)It.IsAny<System.Linq.Expressions.Expression>()), Times.Never);
      }

      [Test]
      public async Task AttendNewRequest()
      {
         //Arrange
         var snapshot = new Mock<ISnapShot>();
         var runRequest = new RunSelectionRequest();
         var runIdRequested = Guid.NewGuid();

         runRequest = runRequest.WithNewRequest(runIdRequested);
         snapshot.Setup(ss => ss.GetItems<RunSelectionRequest>()).Returns(new RunSelectionRequest[] { runRequest });

         // Act
         await _runSelectionRequestClientToTest.Update(snapshot.Object);

         // Assert
         _notifierMock.Verify(n => n.NotifyError(It.IsAny<ResultException>()), Times.Never);
         _measurementDataSourceRefresherService.Verify(r => r.Refresh(It.IsAny<ISnapShot>(), It.IsAny<IEnumerable<PlanDescriptor>>(), It.IsAny<IEnumerable<RunDescriptor>>()), Times.Never);
         _appStateHistoryMock.Verify(aps => aps.Run((System.Linq.Expressions.Expression<Func<ISnapShot, ISnapShot>>)It.IsAny<System.Linq.Expressions.Expression>()), Times.Once);
      }
   }
}
