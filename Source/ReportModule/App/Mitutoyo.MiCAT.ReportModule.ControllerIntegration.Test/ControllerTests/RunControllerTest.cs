// <copyright file="RunControllerTest.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.ApplicationState.Clients;
using Mitutoyo.MiCAT.ApplicationState.Controllers;
using Mitutoyo.MiCAT.ReportModule.Domain.Data;
using Mitutoyo.MiCAT.ReportModule.Domain.DataResult;
using Mitutoyo.MiCAT.ReportModule.Domain.DataResult.Exceptions;
using Mitutoyo.MiCAT.ReportModule.Persistence;
using Mitutoyo.MiCAT.ReportModuleApp.Controllers;
using Mitutoyo.MiCAT.ReportModuleApp.Services.PlansRuns;
using Mitutoyo.MiCAT.ReportModuleApp.Utilities;
using Moq;
using NUnit.Framework;

namespace Mitutoyo.MiCAT.ReportModule.ControllerIntegration.Test.ControllerTests
{
   [ExcludeFromCodeCoverage]
   [TestFixture]
   public class RunControllerTest : BaseAppStateTest
   {
      private Mock<IMeasurementPersistence> _measurementPersistenceMock;
      private Mock<IMessageNotifier> _notifierMock;
      private RunController _controller;
      private Mock<IUpdateClient> _client;
      private Mock<IMeasurementDataSourceRefresherService> _plansRunsRefresherServiceMock;
      private Mock<IPlanPersistence> _planPersistenceMock;

      public static ISnapShot BuildHelper(ISnapShot snapShot)
      {
         snapShot = snapShot.AddCollection<RunSelection>(AppStateKinds.Undoable);
         snapShot = snapShot.AddCollection<RunSelectionRequest>(AppStateKinds.Undoable);
         snapShot = snapShot.AddCollection<PlansState>(AppStateKinds.Undoable);

         return snapShot;
      }
      [SetUp]
      public virtual void Setup()
      {
         _measurementPersistenceMock = new Mock<IMeasurementPersistence>();
         _notifierMock = new Mock<IMessageNotifier>();
         _plansRunsRefresherServiceMock = new Mock<IMeasurementDataSourceRefresherService>();
         _planPersistenceMock = new Mock<IPlanPersistence>();

         SetUpHelper(BuildHelper);

         _controller = new RunController(_measurementPersistenceMock.Object,
            _history,
            _notifierMock.Object,
            _plansRunsRefresherServiceMock.Object,
            _planPersistenceMock.Object);
         _client = new Mock<IUpdateClient>();

         _history.Subscribe(_client.Object);
      }

      [Test]
      public void CompleteRunSelectionOnAppStateTest()
      {
         //Arrange
         var runIdRequested = Guid.NewGuid();
         RunData runData = new RunData(DateTime.Now, Guid.NewGuid(), 1, ImmutableList<EvaluatedCharacteristic>.Empty, ImmutableList<DynamicPropertyValue>.Empty, ImmutableList<Capture3D>.Empty);
         RunData runDataSelected = new RunData(DateTime.Now, Guid.NewGuid(), 1, ImmutableList<EvaluatedCharacteristic>.Empty, ImmutableList<DynamicPropertyValue>.Empty, ImmutableList<Capture3D>.Empty);
         RunSelection originalRunSelection = new RunSelection(Guid.NewGuid(), runDataSelected);
         RunSelectionRequest originalRunSelectionRequest = new RunSelectionRequest().WithNewRequest(runIdRequested);
         var snapShot = _history.NextSnapShot();

         snapShot = snapShot.AddItem(originalRunSelection);
         snapShot = snapShot.AddItem(originalRunSelectionRequest);

         _history.AddSnapShot(snapShot);

         // Act
         var snapShotResult = _controller.CompleteRunSelectionOnAppState(_history.CurrentSnapShot, runIdRequested, runData);
         // Assert
         var runSelectionResult = snapShotResult.GetItems<RunSelection>().Single();
         var runSelectionRequestResult = snapShotResult.GetItems<RunSelectionRequest>().Single();
         Assert.AreEqual(runIdRequested, runSelectionResult.SelectedRun);
         Assert.AreEqual(runData, runSelectionResult.SelectedRunData);
         Assert.IsFalse(runSelectionRequestResult.Pending);
      }
      [Test]
      public void CompleteRunSelectionOnAppStateIgnoreDifferentRunTest()
      {
         //Arrange
         var runIdRequested = Guid.NewGuid();
         var runIdSelected = Guid.NewGuid();
         RunData runData = new RunData(DateTime.Now, Guid.NewGuid(), 1, ImmutableList<EvaluatedCharacteristic>.Empty, ImmutableList<DynamicPropertyValue>.Empty, ImmutableList<Capture3D>.Empty);
         RunData runDataSelected = new RunData(DateTime.Now, Guid.NewGuid(), 1, ImmutableList<EvaluatedCharacteristic>.Empty, ImmutableList<DynamicPropertyValue>.Empty, ImmutableList<Capture3D>.Empty);
         RunSelection originalRunSelection = new RunSelection(runIdSelected, runDataSelected);
         RunSelectionRequest originalRunSelectionRequest = new RunSelectionRequest().WithNewRequest(Guid.NewGuid());
         var snapShot = _history.NextSnapShot();

         snapShot = snapShot.AddItem(originalRunSelection);
         snapShot = snapShot.AddItem(originalRunSelectionRequest);

         _history.AddSnapShot(snapShot);

         // Act
         var snapShotResult = _controller.CompleteRunSelectionOnAppState(_history.CurrentSnapShot, runIdRequested, runData);
         // Assert
         Assert.AreEqual(_history.CurrentSnapShot, snapShotResult);
      }
      [Test]
      public void CompleteRunSelectionOnAppStateIgnoreNotPendingTest()
      {
         //Arrange
         var runIdRequested = Guid.NewGuid();
         var runIdSelected = Guid.NewGuid();
         RunData runData = new RunData(DateTime.Now, Guid.NewGuid(), 1, ImmutableList<EvaluatedCharacteristic>.Empty, ImmutableList<DynamicPropertyValue>.Empty, ImmutableList<Capture3D>.Empty);
         RunData runDataSelected = new RunData(DateTime.Now, Guid.NewGuid(), 1, ImmutableList<EvaluatedCharacteristic>.Empty, ImmutableList<DynamicPropertyValue>.Empty, ImmutableList<Capture3D>.Empty);
         RunSelection originalRunSelection = new RunSelection(runIdSelected, runDataSelected);
         RunSelectionRequest originalRunSelectionRequest = new RunSelectionRequest().WithCompletedRequest();
         var snapShot = _history.NextSnapShot();

         snapShot = snapShot.AddItem(originalRunSelection);
         snapShot = snapShot.AddItem(originalRunSelectionRequest);

         _history.AddSnapShot(snapShot);

         // Act
         var snapShotResult = _controller.CompleteRunSelectionOnAppState(_history.CurrentSnapShot, runIdRequested, runData);
         // Assert
         Assert.AreEqual(_history.CurrentSnapShot, snapShotResult);
      }

      [Test]
      public async Task DeleteRunFromPlanShouldNotifyClient()
      {
         //Arrange
         var runDescriptor = new RunDescriptor() { Id = Guid.NewGuid(), TimeStamp = new DateTime(2019, 01, 01) };

         IEnumerable<RunDescriptor> runList = new List<RunDescriptor>() { runDescriptor };

         _measurementPersistenceMock.Setup(m => m.DeleteMeasurementResultById(runDescriptor.Id)).Returns(Task.CompletedTask);

         var selectPlan = new PlanDescriptor() { Id = Guid.NewGuid(), Name = "Plan 1" };
         var list = ImmutableList.Create<PlanDescriptor>(selectPlan);
         var plansState = new PlansState().With(list).With(selectPlan, runList.ToImmutableList());
         var runSelection = new RunSelection();
         var runSelectionRequest = new RunSelectionRequest();

         _measurementPersistenceMock.Setup(m => m.GetRunsByPlanId(selectPlan.Id)).Returns(Task.FromResult(runList));

         _plansRunsRefresherServiceMock.Setup(r => r.Refresh(It.IsAny<ISnapShot>(), It.IsAny<IEnumerable<PlanDescriptor>>(), It.IsAny<IEnumerable<RunDescriptor>>()))
            .Returns((ISnapShot ss, IEnumerable<PlanDescriptor> p, IEnumerable<RunDescriptor> r) => ss.UpdateItem(plansState, plansState.With(ImmutableList<PlanDescriptor>.Empty)));

         var snapshot = _history.NextSnapShot(ControllerCall.Empty);
         snapshot = snapshot.AddItem(plansState);
         snapshot = snapshot.AddItem(runSelection);
         snapshot = snapshot.AddItem(runSelectionRequest);
         _history.AddSnapShot(snapshot);

         // Act
         await _controller.DeleteRun(runDescriptor.Id);

         // Assert
         _client.Verify(m => m.Update(It.IsAny<ISnapShot>()), Times.Once());
      }
      [Test]
      public async Task DeleteRunFromPartShouldNotifyClient()
      {
         //Arrange
         var runDescriptor = new RunDescriptor() { Id = Guid.NewGuid(), TimeStamp = new DateTime(2019, 01, 01) };

         RunSelection runSelection = new RunSelection();
         RunSelectionRequest runSelectionRequest = new RunSelectionRequest();
         IEnumerable<RunDescriptor> runList = new List<RunDescriptor>() { runDescriptor };

         var selectPart = new PartDescriptor() { Id = Guid.NewGuid(), Name = "Part 1" };
         var list = ImmutableList.Create<PlanDescriptor>();
         var plansState = new PlansState().With(list).With(selectPart, runList.ToImmutableList());

         _measurementPersistenceMock.Setup(m => m.DeleteMeasurementResultById(runDescriptor.Id)).Returns(Task.CompletedTask);
         _measurementPersistenceMock.Setup(m => m.GetRunsByPartId(selectPart.Id)).Returns(Task.FromResult(runList));

         var snapshot = _history.NextSnapShot(ControllerCall.Empty);
         snapshot = snapshot.AddItem(runSelection);
         snapshot = snapshot.AddItem(runSelectionRequest);
         snapshot = snapshot.AddItem(plansState);
         _history.AddSnapShot(snapshot);

         _plansRunsRefresherServiceMock.Setup(r => r.Refresh(It.IsAny<ISnapShot>(), It.IsAny<IEnumerable<PlanDescriptor>>(), It.IsAny<IEnumerable<RunDescriptor>>()))
            .Returns((ISnapShot ss, IEnumerable<PlanDescriptor> p, IEnumerable<RunDescriptor> r) => ss.UpdateItem(plansState, plansState.With(ImmutableList<PlanDescriptor>.Empty)));

         // Act
         await _controller.DeleteRun(runDescriptor.Id);
         // Assert
         _client.Verify(m => m.Update(It.IsAny<ISnapShot>()), Times.Once());
      }
      [Test]
      public async Task WhenDeleteNotFoundRunShouldShowWindow()
      {
         var run = new RunDescriptor() { Id = Guid.NewGuid() };
         var runList = ImmutableList.Create<RunDescriptor>(run);

         var selectPart = new PartDescriptor() { Id = Guid.NewGuid(), Name = "Part 1" };
         var listPlan = ImmutableList.Create<PlanDescriptor>();
         var plansState = new PlansState().With(listPlan).With(selectPart, runList);
         var runSelection = new RunSelection();
         var runSelectionRequest = new RunSelectionRequest();

         var exception = new RunNotFoundException("error");
         _measurementPersistenceMock.Setup(p => p.DeleteMeasurementResultById(run.Id)).Throws(exception);

         _plansRunsRefresherServiceMock.Setup(r => r.Refresh(It.IsAny<ISnapShot>(), It.IsAny<IEnumerable<PlanDescriptor>>(), It.IsAny<IEnumerable<RunDescriptor>>()))
            .Returns((ISnapShot ss, IEnumerable<PlanDescriptor> p, IEnumerable<RunDescriptor> r) => ss.UpdateItem(plansState, plansState.With(ImmutableList<PlanDescriptor>.Empty)));

         var snapshot = _history.NextSnapShot(ControllerCall.Empty);
         snapshot = snapshot.AddItem(plansState);
         snapshot = snapshot.AddItem(runSelection);
         snapshot = snapshot.AddItem(runSelectionRequest);

         _history.AddSnapShot(snapshot);

         await _controller.DeleteRun(run.Id);
         _notifierMock.Verify(n => n.NotifyError(It.IsAny<RunNotFoundException>()), Times.Once);
      }
      [Test]
      public async Task WhenDeleteErrorOccurShouldNotUpdateAppState()
      {
         var runSelection = new RunSelection();
         var runSelectionRequest = new RunSelectionRequest();
         var run = new RunDescriptor() { Id = Guid.NewGuid() };
         var runList = ImmutableList.Create<RunDescriptor>(run);
         var selectPart = new PartDescriptor() { Id = Guid.NewGuid(), Name = "Part 1" };
         var listPlan = ImmutableList.Create<PlanDescriptor>();
         var plansState = new PlansState().With(listPlan).With(selectPart, runList);
         var snapshot = _history.NextSnapShot(ControllerCall.Empty);
         snapshot = snapshot.AddItem(plansState);
         snapshot = snapshot.AddItem(runSelection);
         snapshot = snapshot.AddItem(runSelectionRequest);
         _history.AddSnapShot(snapshot);

         var exception = new ResultException("error", "errorkey");
         _measurementPersistenceMock.Setup(p => p.DeleteMeasurementResultById(run.Id)).Throws(exception);

         _plansRunsRefresherServiceMock.Setup(r => r.Refresh(It.IsAny<ISnapShot>(), It.IsAny<IEnumerable<PlanDescriptor>>(), It.IsAny<IEnumerable<RunDescriptor>>()))
            .Returns((ISnapShot ss, IEnumerable<PlanDescriptor> p, IEnumerable<RunDescriptor> r) => ss.UpdateItem(plansState, plansState.With(ImmutableList<PlanDescriptor>.Empty)));

         await _controller.DeleteRun(run.Id);
         _notifierMock.Verify(n => n.NotifyError(It.IsAny<ResultException>()), Times.Once);
      }

      [Test]
      public async Task TryDeleteRunNotExistsRunShouldReturnRunCouldNotBeDeletedException()
      {
         //Arrange
         var runDescriptor = new RunDescriptor() { Id = Guid.NewGuid(), TimeStamp = new DateTime(2019, 01, 01) };
         var resultException = new RunCouldNotBeDeletedException("RunNotFound");

         _measurementPersistenceMock.Setup(m => m.DeleteMeasurementResultById(runDescriptor.Id))
            .Throws(resultException);

         var snapshot = _history.NextSnapShot(ControllerCall.Empty);

         RunSelection previousRunSelection = new RunSelection(runDescriptor.Id, null);
         RunSelectionRequest runSelectionRequest = new RunSelectionRequest();

         var selectPlan = new PlanDescriptor() { Id = Guid.NewGuid(), Name = "Plan 1" };
         var list = ImmutableList.Create<PlanDescriptor>(selectPlan);
         var plansState = new PlansState().With(list).With(selectPlan, ImmutableList.Create<RunDescriptor>(runDescriptor));

         snapshot = snapshot.AddItem(previousRunSelection);
         snapshot = snapshot.AddItem(runSelectionRequest);
         snapshot = snapshot.AddItem(plansState);
         _history.AddSnapShot(snapshot);

         _plansRunsRefresherServiceMock.Setup(r => r.Refresh(It.IsAny<ISnapShot>(), It.IsAny<IEnumerable<PlanDescriptor>>(), It.IsAny<IEnumerable<RunDescriptor>>()))
            .Returns((ISnapShot ss, IEnumerable<PlanDescriptor> p, IEnumerable<RunDescriptor> r) => ss.UpdateItem(plansState, plansState.With(ImmutableList<PlanDescriptor>.Empty)));

         //Act
         await _controller.DeleteRun(runDescriptor.Id);
         //Assert
         _notifierMock.Verify(n => n.NotifyError(resultException));
      }
   }
}
