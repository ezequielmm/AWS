// <copyright file="PlanControllerTest.cs" company="Mitutoyo Europe GmbH">
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
   public class PlanControllerTest : BaseAppStateTest
   {
      private Mock<IMeasurementPersistence> _measurementPersistanceMock;
      private Mock<IPartPersistence> _partPersistenceMock;
      private Mock<IPlanPersistence> _planPersistenceMock;
      private Mock<IMessageNotifier> _notifierMock;
      private Mock<IMeasurementDataSourceRefresherService> _measurementDataSourceRefresherService;
      private PlanController _controller;

      public static ISnapShot BuildHelper(ISnapShot snapShot)
      {
         snapShot = snapShot.AddCollection<AllCharacteristicTypes>(AppStateKinds.Undoable);
         snapShot = snapShot.AddCollection<PlansState>(AppStateKinds.Undoable);
         snapShot = snapShot.AddCollection<RunSelection>(AppStateKinds.Undoable);
         snapShot = snapShot.AddCollection<RunSelectionRequest>(AppStateKinds.Undoable);

         return snapShot;
      }
      [SetUp]
      public virtual void Setup()
      {
         _measurementPersistanceMock = new Mock<IMeasurementPersistence>();
         _partPersistenceMock = new Mock<IPartPersistence>();
         _planPersistenceMock = new Mock<IPlanPersistence>();
         _notifierMock = new Mock<IMessageNotifier>();
         _measurementDataSourceRefresherService = new Mock<IMeasurementDataSourceRefresherService>();

         SetUpHelper(BuildHelper);

         _controller = new PlanController(_history, _measurementPersistanceMock.Object,
            _partPersistenceMock.Object, _planPersistenceMock.Object, _notifierMock.Object, _measurementDataSourceRefresherService.Object);
      }

      [Test]
      public async Task WhenSelectADifferentPartAppStateShouldBeNotified()
      {
         //Arrange
         var firstPart = new PartDescriptor() { Id = Guid.NewGuid(), Name = "Part 1" };
         var secondPart = new PartDescriptor() { Id = Guid.NewGuid(), Name = "Part 2" };
         var list = ImmutableList.Create<PlanDescriptor>(
            new PlanDescriptor { Part = firstPart },
            new PlanDescriptor { Part = secondPart });

         var plansState = new PlansState().With(list).With(firstPart, ImmutableList<RunDescriptor>.Empty);
         var allCharacteristicTypes = new AllCharacteristicTypes(ImmutableList<string>.Empty);

         var snapshot = _history.NextSnapShot(ControllerCall.Empty);
         snapshot = snapshot.AddItem(allCharacteristicTypes);
         snapshot = snapshot.AddItem(plansState);

         _history.AddSnapShot(snapshot);

         _measurementDataSourceRefresherService.Setup(r => r.Refresh(It.IsAny<ISnapShot>(), It.IsAny<IEnumerable<PlanDescriptor>>(), It.IsAny<IEnumerable<RunDescriptor>>()))
           .Returns((ISnapShot ss, IEnumerable<PlanDescriptor> p, IEnumerable<RunDescriptor> r) => ss);

         _measurementDataSourceRefresherService.Setup(r => r.Refresh(It.IsAny<ISnapShot>(), It.IsAny<IEnumerable<PlanDescriptor>>()))
           .Returns((ISnapShot ss, IEnumerable<PlanDescriptor> p) => ss);

         _measurementDataSourceRefresherService.Setup(r => r.ClearRunSelection(It.IsAny<ISnapShot>())).Returns((ISnapShot ss) => ss);

         // Act
         await _controller.SelectPart(secondPart.Id);

         // Assert
      }

      [Test]
      public async Task WhenSelectTheSamePartAppStateShouldNotBeNotified()
      {
         var selectPart = new PartDescriptor() { Id = Guid.NewGuid(), Name = "Part 1" };
         var list = ImmutableList.Create<PlanDescriptor>(new PlanDescriptor { Part = selectPart });
         var plansState = new PlansState().With(list).With(selectPart, ImmutableList<RunDescriptor>.Empty);
         var allCharacteristicTypes = new AllCharacteristicTypes(ImmutableList<string>.Empty);
         var plan = new PlanDescriptor() { Id = Guid.NewGuid(), Part = selectPart };

         _partPersistenceMock.Setup(p => p.GetParts()).Returns(Task.FromResult((IEnumerable<PartDescriptor>)new List<PartDescriptor>() { selectPart }));
         _planPersistenceMock.Setup((p => p.GetPlansForPart(selectPart.Id))).Returns(Task.FromResult((IEnumerable<PlanDescriptor>)new List<PlanDescriptor>() { plan }));

         var snapshot = _history.NextSnapShot(ControllerCall.Empty);
         snapshot = snapshot.AddItem(allCharacteristicTypes);
         snapshot = snapshot.AddItem(plansState);

         _history.AddSnapShot(snapshot);

         // Act
         await _controller.SelectPart(selectPart.Id);

         // Assert
      }

      [Test]
      public async Task WhenSelectADifferentPlanAppStateShouldBeNotified()
      {
         //Arrange
         var firstPlan = new PlanDescriptor() { Id = Guid.NewGuid(), Name = "Plan 1" };
         var secondPlan = new PlanDescriptor() { Id = Guid.NewGuid(), Name = "Plan 2" };
         var list = ImmutableList.Create<PlanDescriptor>(firstPlan, secondPlan);
         var plansState = new PlansState().With(list).With(firstPlan, ImmutableList<RunDescriptor>.Empty);
         var allCharacteristicTypes = new AllCharacteristicTypes(ImmutableList<string>.Empty);

         var snapshot = _history.NextSnapShot(ControllerCall.Empty);
         snapshot = snapshot.AddItem(allCharacteristicTypes);
         snapshot = snapshot.AddItem(plansState);

         _history.AddSnapShot(snapshot);

         _measurementDataSourceRefresherService.Setup(r => r.Refresh(It.IsAny<ISnapShot>(), It.IsAny<IEnumerable<PlanDescriptor>>(), It.IsAny<IEnumerable<RunDescriptor>>()))
           .Returns((ISnapShot ss, IEnumerable<PlanDescriptor> p, IEnumerable<RunDescriptor> r) => ss.UpdateItem(plansState, plansState.With(ImmutableList<PlanDescriptor>.Empty)));

         _measurementDataSourceRefresherService.Setup(r => r.ClearRunSelection(It.IsAny<ISnapShot>())).Returns((ISnapShot ss) => ss);

         // Act
         await _controller.SelectPlan(secondPlan.Id);

         // Assert
      }

      [Test]
      public async Task WhenSelectTheSamePlanAppStateShouldNotBeNotified()
      {
         var selectPlan = new PlanDescriptor() { Id = Guid.NewGuid(), Name = "Plan 1", Part = new PartDescriptor() { Id = Guid.Empty } };
         var list = ImmutableList.Create<PlanDescriptor>(selectPlan);
         var plansState = new PlansState().With(list).With(selectPlan, ImmutableList<RunDescriptor>.Empty);
         var allCharacteristicTypes = new AllCharacteristicTypes(ImmutableList<string>.Empty);

         _planPersistenceMock.Setup(p => p.GetPlans())
            .Returns(Task.FromResult((IEnumerable<PlanDescriptor>)new List<PlanDescriptor>() { selectPlan }));

         var snapshot = _history.NextSnapShot(ControllerCall.Empty);
         snapshot = snapshot.AddItem(allCharacteristicTypes);
         snapshot = snapshot.AddItem(plansState);

         _history.AddSnapShot(snapshot);

         // Act
         await _controller.SelectPlan(selectPlan.Id);

         // Assert
      }

      [Test]
      public async Task WhenSelectAPlanAppStateShouldBeUpdated()
      {
         var selectPlan = new PlanDescriptor() { Id = Guid.NewGuid(), Name = "Plan 1", Part = new PartDescriptor() { Id = Guid.Empty } };
         var list = ImmutableList.Create<PlanDescriptor>(selectPlan);
         var plansState = new PlansState().With(list).With(selectPlan, ImmutableList<RunDescriptor>.Empty);
         var allCharacteristicTypes = new AllCharacteristicTypes(ImmutableList<string>.Empty);

         _planPersistenceMock.Setup(p => p.GetPlans()).Returns(Task.FromResult((IEnumerable<PlanDescriptor>)new List<PlanDescriptor>() { selectPlan }));

         var snapshot = _history.NextSnapShot(ControllerCall.Empty);
         snapshot = snapshot.AddItem(allCharacteristicTypes);
         snapshot = snapshot.AddItem(plansState);

         _history.AddSnapShot(snapshot);

         // Act
         await _controller.SelectPlan(selectPlan.Id);

         // Assert
         var newPlansState = _history.CurrentSnapShot.GetItems<PlansState>();
         Assert.That(newPlansState.ToImmutableList().TrueForAll(p => p.SelectedPlanDataSource.Id == selectPlan.Id));
      }

      [Test]
      public async Task WhenSelectAPlanMeasurmentResutlShoulBeReset()
      {
         var previousSelectedPlan = new PlanDescriptor() { Id = Guid.NewGuid(), Name = "Plan 1" };
         var selectPlan = new PlanDescriptor() { Id = Guid.NewGuid(), Name = "Plan 1" };
         var list = ImmutableList.Create(new PlanDescriptor[] { previousSelectedPlan, selectPlan });
         var plansState = new PlansState().With(list).With(previousSelectedPlan, ImmutableList<RunDescriptor>.Empty);
         var runsSelection = new RunSelection(Guid.NewGuid(), null);
         var runsSelectionRequest = new RunSelectionRequest();

         var snapshot = _history.NextSnapShot(ControllerCall.Empty);
         snapshot = snapshot.AddItem(runsSelection);
         snapshot = snapshot.AddItem(runsSelectionRequest);
         snapshot = snapshot.AddItem(plansState);

         _history.AddSnapShot(snapshot);
         _planPersistenceMock.Setup(p => p.GetPlans())
            .Returns(Task.FromResult((IEnumerable<PlanDescriptor>)list));

         _measurementDataSourceRefresherService.Setup(r => r.Refresh(It.IsAny<ISnapShot>(), It.IsAny<IEnumerable<PlanDescriptor>>(), It.IsAny<IEnumerable<RunDescriptor>>()))
          .Returns((ISnapShot ss, IEnumerable<PlanDescriptor> p, IEnumerable<RunDescriptor> r) => ss);

         _measurementDataSourceRefresherService.Setup(r => r.ClearRunSelection(It.IsAny<ISnapShot>())).Returns((ISnapShot ss) => ss.UpdateItem(runsSelection, runsSelection.WithNoSelectedRun()));

         // Act
         await _controller.SelectPlan(selectPlan.Id);

         // Assert
         var newRunsState = _history.CurrentSnapShot.GetItems<RunSelection>().Single();
         Assert.IsFalse(newRunsState.SelectedRun.HasValue);
      }
      [Test]
      public async Task WhenDeletePartAppStateShouldBeUpdated()
      {
         //Arrange
         var plan = new PlanDescriptor() { Id = Guid.NewGuid(), Name = "Plan 1" };
         var list = ImmutableList.Create<PlanDescriptor>(plan);
         var plansState = new PlansState().With(list);

         _partPersistenceMock.Setup(p => p.DeletePart(plan.Id)).Returns(Task.CompletedTask);

         var snapshot = _history.NextSnapShot(ControllerCall.Empty);
         snapshot = snapshot.AddItem(plansState);

         _history.AddSnapShot(snapshot);

         _measurementDataSourceRefresherService.Setup(r => r.Refresh(It.IsAny<ISnapShot>(), It.IsAny<IEnumerable<PlanDescriptor>>(), It.IsAny<IEnumerable<RunDescriptor>>()))
           .Returns((ISnapShot ss, IEnumerable<PlanDescriptor> p, IEnumerable<RunDescriptor> r) => ss.UpdateItem(plansState, plansState.With(ImmutableList<PlanDescriptor>.Empty)));

         _measurementDataSourceRefresherService.Setup(r => r.Refresh(It.IsAny<ISnapShot>(), It.IsAny<IEnumerable<PlanDescriptor>>()))
           .Returns((ISnapShot ss, IEnumerable<PlanDescriptor> p) => ss.UpdateItem(plansState, plansState.With(ImmutableList<PlanDescriptor>.Empty)));

         //Act
         await _controller.DeletePart(plan.Id);

         //Assert
      }
      [Test]
      public async Task WhenDeletePlanAppStateShouldBeUpdated()
      {
         var plan = new PlanDescriptor() { Id = Guid.NewGuid(), Name = "Plan 1" };
         var list = ImmutableList.Create<PlanDescriptor>(plan);
         var plansState = new PlansState().With(list);

         _planPersistenceMock.Setup(p => p.DeletePlan(plan.Id)).Returns(Task.CompletedTask);
         var snapshot = _history.NextSnapShot(ControllerCall.Empty);
         snapshot = snapshot.AddItem(plansState);

         _history.AddSnapShot(snapshot);

         _measurementDataSourceRefresherService.Setup(r => r.Refresh(It.IsAny<ISnapShot>(), It.IsAny<IEnumerable<PlanDescriptor>>(), It.IsAny<IEnumerable<RunDescriptor>>()))
           .Returns((ISnapShot ss, IEnumerable<PlanDescriptor> p, IEnumerable<RunDescriptor> r) => ss.UpdateItem(plansState, plansState.With(ImmutableList<PlanDescriptor>.Empty)));

         _measurementDataSourceRefresherService.Setup(r => r.Refresh(It.IsAny<ISnapShot>(), It.IsAny<IEnumerable<PlanDescriptor>>()))
          .Returns((ISnapShot ss, IEnumerable<PlanDescriptor> p) => ss.UpdateItem(plansState, plansState.With(ImmutableList<PlanDescriptor>.Empty)));

         await _controller.DeletePlan(plan.Id);
      }
      [Test]
      public async Task WhenDeleteNotFoundPlandShouldShowWindow()
      {
         //Arrange
         var plan = new PlanDescriptor() { Id = Guid.NewGuid(), Name = "Plan 1" };
         var list = ImmutableList.Create<PlanDescriptor>(plan);
         var plansState = new PlansState().With(list);

         var exception = new PlanCouldNotBeDeletedException("error");
         _planPersistenceMock.Setup(p => p.DeletePlan(plan.Id)).Throws(exception);

         var snapshot = _history.NextSnapShot(ControllerCall.Empty);
         snapshot = snapshot.AddItem(plansState);
         _history.AddSnapShot(snapshot);

         _measurementDataSourceRefresherService.Setup(r => r.Refresh(It.IsAny<ISnapShot>(), It.IsAny<IEnumerable<PlanDescriptor>>(), It.IsAny<IEnumerable<RunDescriptor>>()))
           .Returns((ISnapShot ss, IEnumerable<PlanDescriptor> p, IEnumerable<RunDescriptor> r) => ss.UpdateItem(plansState, plansState.With(ImmutableList<PlanDescriptor>.Empty)));

         _measurementDataSourceRefresherService.Setup(r => r.Refresh(It.IsAny<ISnapShot>(), It.IsAny<IEnumerable<PlanDescriptor>>()))
          .Returns((ISnapShot ss, IEnumerable<PlanDescriptor> p) => ss.UpdateItem(plansState, plansState.With(ImmutableList<PlanDescriptor>.Empty)));

         //Act
         await _controller.DeletePlan(plan.Id);

         //Assert
         _notifierMock.Verify(n => n.NotifyError(It.IsAny<PlanCouldNotBeDeletedException>()), Times.Once);
      }
      [Test]
      public async Task WhenDeleteErrorOcurrShouldUpdateAppState()
      {
         var plan = new PlanDescriptor() { Id = Guid.NewGuid(), Name = "Plan 1" };
         var list = ImmutableList.Create<PlanDescriptor>(plan);
         var plansState = new PlansState().With(list);
         var exception = new ResultException("error", "key");
         _planPersistenceMock.Setup(p => p.DeletePlan(plan.Id)).Throws(exception);

         var snapshot = _history.NextSnapShot(ControllerCall.Empty);
         snapshot = snapshot.AddItem(plansState);
         _history.AddSnapShot(snapshot);

         _measurementDataSourceRefresherService.Setup(r => r.Refresh(It.IsAny<ISnapShot>(), It.IsAny<IEnumerable<PlanDescriptor>>(), It.IsAny<IEnumerable<RunDescriptor>>()))
            .Returns((ISnapShot ss, IEnumerable<PlanDescriptor> p, IEnumerable<RunDescriptor> r) => ss.UpdateItem(plansState, plansState.With(ImmutableList<PlanDescriptor>.Empty)));

         _measurementDataSourceRefresherService.Setup(r => r.Refresh(It.IsAny<ISnapShot>(), It.IsAny<IEnumerable<PlanDescriptor>>()))
            .Returns((ISnapShot ss, IEnumerable<PlanDescriptor> p) => ss.UpdateItem(plansState, plansState.With(ImmutableList<PlanDescriptor>.Empty)));

         // Act
         await _controller.DeletePlan(plan.Id);

         // Assert
      }

      [Test]
      public async Task WhenUpdatePlanListWithNoListShouldUpdateAppstateWithEmpty()
      {
         //Arrange
         var selectPlan = new PlanDescriptor() { Id = Guid.NewGuid(), Name = "Plan 1" };
         var list = ImmutableList.Create<PlanDescriptor>(selectPlan);
         var plansState = new PlansState().With(list).With(selectPlan, ImmutableList<RunDescriptor>.Empty);
         var allCharacteristicTypes = new AllCharacteristicTypes(ImmutableList<string>.Empty);

         var snapshot = _history.NextSnapShot(ControllerCall.Empty);
         snapshot = snapshot.AddItem(allCharacteristicTypes);
         snapshot = snapshot.AddItem(plansState);

         _history.AddSnapShot(snapshot);

         _planPersistenceMock.Setup(p => p.GetPlans()).ReturnsAsync(new List<PlanDescriptor>());
         _partPersistenceMock.Setup(p => p.GetParts()).ReturnsAsync(new List<PartDescriptor>());

         _measurementDataSourceRefresherService.Setup(r => r.Refresh(It.IsAny<ISnapShot>(), It.IsAny<IEnumerable<PlanDescriptor>>(), It.IsAny<IEnumerable<RunDescriptor>>()))
           .Returns((ISnapShot ss, IEnumerable<PlanDescriptor> p, IEnumerable<RunDescriptor> r) => ss.UpdateItem(plansState, plansState.With(ImmutableList<PlanDescriptor>.Empty)));

         // Act
         await _controller.RefreshMessurementDataSourceList();

         // Assert
         Assert.IsTrue(_history.CurrentSnapShot.GetItems<PlansState>().Single().PlanList.IsEmpty);
      }
      [Test]
      public async Task WhenUpdatePlanListWitSamePlanListShouldUpdateList()
      {
         //Arrange
         var selectedPart = new PartDescriptor() { Id = Guid.NewGuid() };
         var plan = new PlanDescriptor() { Id = Guid.NewGuid(), Name = "Plan 1", Part = selectedPart };
         var list = ImmutableList.Create<PlanDescriptor>(plan);
         var plansState = new PlansState().With(list).With(selectedPart, ImmutableList<RunDescriptor>.Empty);
         var allCharacteristicTypes = new AllCharacteristicTypes(ImmutableList<string>.Empty);

         var snapshot = _history.NextSnapShot(ControllerCall.Empty);
         snapshot = snapshot.AddItem(allCharacteristicTypes);
         snapshot = snapshot.AddItem(plansState);

         _history.AddSnapShot(snapshot);

         _measurementDataSourceRefresherService.Setup(r => r.Refresh(It.IsAny<ISnapShot>(), It.IsAny<IEnumerable<PlanDescriptor>>(), It.IsAny<IEnumerable<RunDescriptor>>()))
           .Returns((ISnapShot ss, IEnumerable<PlanDescriptor> p, IEnumerable<RunDescriptor> r) => ss);

         _measurementDataSourceRefresherService.Setup(r => r.ClearRunSelection(It.IsAny<ISnapShot>())).Returns((ISnapShot ss) => ss);

         _planPersistenceMock.Setup(p => p.GetPlans()).ReturnsAsync(new List<PlanDescriptor>() { plan });
         _partPersistenceMock.Setup(p => p.GetParts()).ReturnsAsync(new List<PartDescriptor>() { selectedPart });

         // Act
         await _controller.RefreshMessurementDataSourceList();

         // Assert
         Assert.IsFalse(_history.CurrentSnapShot.GetItems<PlansState>().Single().PlanList.IsEmpty);
      }
      [Test]
      public async Task WhenUpdatePlanListWitSamePlanListShouldUpdateListAndKeepSelection()
      {
         //Arrange
         var selectedPart = new PartDescriptor() { Id = Guid.NewGuid() };
         var plan = new PlanDescriptor() { Id = Guid.NewGuid(), Name = "Plan 1", Part = selectedPart };
         var list = ImmutableList.Create<PlanDescriptor>(plan);
         var plansState = new PlansState().With(list).With(plan, ImmutableList<RunDescriptor>.Empty);
         var allCharacteristicTypes = new AllCharacteristicTypes(ImmutableList<string>.Empty);

         var snapshot = _history.NextSnapShot(ControllerCall.Empty);
         snapshot = snapshot.AddItem(allCharacteristicTypes);
         snapshot = snapshot.AddItem(plansState);

         _history.AddSnapShot(snapshot);

         _measurementDataSourceRefresherService.Setup(r => r.Refresh(It.IsAny<ISnapShot>(), It.IsAny<IEnumerable<PlanDescriptor>>(), It.IsAny<IEnumerable<RunDescriptor>>()))
           .Returns((ISnapShot ss, IEnumerable<PlanDescriptor> p, IEnumerable<RunDescriptor> r) => ss);

         _measurementDataSourceRefresherService.Setup(r => r.ClearRunSelection(It.IsAny<ISnapShot>())).Returns((ISnapShot ss) => ss);

         _planPersistenceMock.Setup(p => p.GetPlans()).ReturnsAsync(new List<PlanDescriptor>() { plan });
         _partPersistenceMock.Setup(p => p.GetParts()).ReturnsAsync(new List<PartDescriptor>() { selectedPart });

         // Act
         await _controller.RefreshMessurementDataSourceList();

         // Assert
         Assert.IsFalse(_history.CurrentSnapShot.GetItems<PlansState>().Single().PlanList.IsEmpty);
         Assert.IsTrue(_history.CurrentSnapShot.GetItems<PlansState>().Single().SelectedPlanDataSource.Id == plan.Id);
      }

      [Test]
      public async Task WhenSelectAPartWichHaveNoPlanNotifyException()
      {
         //Arrange
         var selectPart = new PartDescriptor() { Id = Guid.NewGuid(), Name = "Part 1" };
         var list = ImmutableList.Create<PlanDescriptor>(new PlanDescriptor { Part = selectPart });
         var plansState = new PlansState().With(list);
         var allCharacteristicTypes = new AllCharacteristicTypes(ImmutableList<string>.Empty);

         _partPersistenceMock.Setup(p => p.GetParts()).Returns(Task.FromResult((IEnumerable<PartDescriptor>)new List<PartDescriptor>() { selectPart }));

         var snapshot = _history.NextSnapShot(ControllerCall.Empty);
         snapshot = snapshot.AddItem(allCharacteristicTypes);
         snapshot = snapshot.AddItem(plansState);

         _history.AddSnapShot(snapshot);

         _measurementDataSourceRefresherService.Setup(r => r.Refresh(It.IsAny<ISnapShot>(), It.IsAny<IEnumerable<PlanDescriptor>>(), It.IsAny<IEnumerable<RunDescriptor>>()))
           .Returns((ISnapShot ss, IEnumerable<PlanDescriptor> p, IEnumerable<RunDescriptor> r) => ss);

         _measurementDataSourceRefresherService.Setup(r => r.Refresh(It.IsAny<ISnapShot>(), It.IsAny<IEnumerable<PlanDescriptor>>()))
           .Returns((ISnapShot ss, IEnumerable<PlanDescriptor> p) => ss);

         _measurementDataSourceRefresherService.Setup(r => r.ClearRunSelection(It.IsAny<ISnapShot>())).Returns((ISnapShot ss) => ss);

         // Act
         await _controller.SelectPart(selectPart.Id);

         // Assert
         _notifierMock.Verify(n => n.NotifyError(It.IsAny<PartNotFoundException>()));
      }
   }
}
