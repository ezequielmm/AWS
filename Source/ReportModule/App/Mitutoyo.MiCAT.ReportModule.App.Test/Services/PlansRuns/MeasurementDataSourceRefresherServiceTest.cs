// <copyright file="MeasurementDataSourceRefresherServiceTest.cs" company="Mitutoyo Europe GmbH">
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
using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.ReportModule.Domain.Data;
using Mitutoyo.MiCAT.ReportModuleApp.Services.PlansRuns;
using Moq;
using NUnit.Framework;

namespace Mitutoyo.MiCAT.ReportModule.App.Test.Services.PlansRuns
{
   [ExcludeFromCodeCoverage]
   [TestFixture]
   public class MeasurementDataSourceRefresherServiceTest
   {
      private Mock<ISnapShot> _snapShotMock;

      [SetUp]
      public void SetUp()
      {
         _snapShotMock = new Mock<ISnapShot>();
         SetupUpdateItem(_snapShotMock);
      }

      [Test]
      public void RefreshShouldUpdateEmptyPlanStateWithNewPlans()
      {
         //Arrange
         var planState = new PlansState();
         var runSelection = new RunSelection();
         var runSelectionRequest = new RunSelectionRequest();

         SetupGetItems(_snapShotMock, planState, runSelection, runSelectionRequest);

         var sut = new MeasurementDataSourceRefresherService();

         var plans = GetPlans();

         //act
         var newSnapShot = sut.Refresh(_snapShotMock.Object, plans);

         //Assert
         AssertNewSnapShot(newSnapShot, false, null, plans, null, Array.Empty<RunDescriptor>());
      }

      [Test]
      public void RefreshShouldUpdatePlanStateWithNewPlansRemovingSelected()
      {
         //Arrange
         var plans = GetPlans();

         var planState = new PlansState(plans.ToImmutableList()).With(plans[0], ImmutableList<RunDescriptor>.Empty);
         var runSelection = new RunSelection();
         var runSelectionRequest = new RunSelectionRequest();

         SetupGetItems(_snapShotMock, planState, runSelection, runSelectionRequest);

         var sut = new MeasurementDataSourceRefresherService();
         var newPlans = plans.Skip(1);

         //act
         var newSnapShot = sut.Refresh(_snapShotMock.Object, newPlans);

         //Assert
         AssertNewSnapShot(newSnapShot, false, null, newPlans, null, Array.Empty<RunDescriptor>());
      }

      [Test]
      public void RefreshShouldUpdatePlanStateWithNewPlansKeepingSelected()
      {
         //Arrange
         var plans = GetPlans();

         var planState = new PlansState(plans.ToImmutableList()).With(plans[1], ImmutableList<RunDescriptor>.Empty);
         var runSelection = new RunSelection();
         var runSelectionRequest = new RunSelectionRequest();

         SetupGetItems(_snapShotMock, planState, runSelection, runSelectionRequest);

         var sut = new MeasurementDataSourceRefresherService();
         var newPlans = plans.Skip(1);

         //act
         var newSnapShot = sut.Refresh(_snapShotMock.Object, newPlans);

         //Assert
         AssertNewSnapShot(newSnapShot, false, null, newPlans, plans[1], Array.Empty<RunDescriptor>());
      }

      [Test]
      public void RefreshShouldUpdatePlanStateWithNewPlansRunsAndSelections()
      {
         //Arrange
         var runs = Enumerable
            .Range(0, 3)
            .Select(x => new RunDescriptor { Id = Guid.NewGuid() })
            .ToImmutableList();

         var plans = GetPlans();

         var planState = new PlansState(plans.ToImmutableList()).With(plans[0], runs);
         var runSelection = new RunSelection(runs[0].Id, null);
         var runSelectionRequest = new RunSelectionRequest().WithNewRequest(runs[1].Id);

         SetupGetItems(_snapShotMock, planState, runSelection, runSelectionRequest);

         var sut = new MeasurementDataSourceRefresherService();

         //act
         var newSnapShot = sut.Refresh(_snapShotMock.Object, plans, runs);

         //Assert
         AssertNewSnapShot(newSnapShot, true, runs[0].Id, plans, plans[0], runs);
      }

      [Test]
      public void RefreshShouldUpdatePlanStateWithNewRunsCompleteRequest()
      {
         //Arrange
         var runs = Enumerable
            .Range(0, 3)
            .Select(x => new RunDescriptor { Id = Guid.NewGuid() })
            .ToImmutableList();

         var plans = GetPlans();

         var planState = new PlansState(plans.ToImmutableList()).With(plans[0], runs);
         var runSelection = new RunSelection();
         var runSelectionRequest = new RunSelectionRequest().WithNewRequest(runs[0].Id);

         SetupGetItems(_snapShotMock, planState, runSelection, runSelectionRequest);

         var sut = new MeasurementDataSourceRefresherService();
         var newRuns = runs.Skip(1);

         //act
         var newSnapShot = sut.Refresh(_snapShotMock.Object, plans, newRuns);

         //Assert
         AssertNewSnapShot(newSnapShot, false, null, plans, plans[0], newRuns);
      }

      [Test]
      public void RefreshShouldUpdatePlanStateWithNewRunsWithoutSelectedOne()
      {
         //Arrange
         var plans = GetPlans();
         var runs = Enumerable
            .Range(0, 3)
            .Select(x => new RunDescriptor { Id = Guid.NewGuid() })
            .ToImmutableList();

         var planState = new PlansState(plans.ToImmutableList()).With(plans[0], runs);
         var runSelection = new RunSelection(runs[0].Id, null);
         var runSelectionRequest = new RunSelectionRequest();

         SetupGetItems(_snapShotMock, planState, runSelection, runSelectionRequest);

         var sut = new MeasurementDataSourceRefresherService();
         var newRuns = runs.Skip(1);

         //act
         var newSnapShot = sut.Refresh(_snapShotMock.Object, plans, newRuns);

         //Assert
         AssertNewSnapShot(newSnapShot, false, null, plans, plans[0], newRuns);
      }

      [Test]
      public void RefreshShouldUpdatePlanStateWithNewPlansAndCleartRunSelection()
      {
         //Arrange
         var plans = GetPlans();
         var runs = ImmutableList.CreateRange(new RunDescriptor[] { new RunDescriptor { Id = Guid.NewGuid() }, new RunDescriptor() });
         var planState = new PlansState(plans.ToImmutableList()).With(null, runs);
         var runSelection = new RunSelection().WithNewSelectedRun(runs[0].Id, null);
         var runSelectionRequest = new RunSelectionRequest();

         SetupGetItems(_snapShotMock, planState, runSelection, runSelectionRequest);

         var sut = new MeasurementDataSourceRefresherService();

         //act
         var newSnapShot = sut.Refresh(_snapShotMock.Object, plans, Array.Empty<RunDescriptor>());

         //Assert
         AssertNewSnapShot(newSnapShot, false, null, plans, null, Array.Empty<RunDescriptor>());
      }

      [Test]
      public void RefreshShouldUpdatePlanStateWithNewPlansCleartRunSelectionAndCompleteRequest()
      {
         //Arrange
         var plans = GetPlans();
         var runs = ImmutableList.CreateRange(new RunDescriptor[] { new RunDescriptor { Id = Guid.NewGuid() }, new RunDescriptor(), });
         var planState = new PlansState(plans.ToImmutableList()).With(null, runs);
         var runSelection = new RunSelection().WithNewSelectedRun(runs[0].Id, null);
         var runSelectionRequest = new RunSelectionRequest().WithNewRequest(runs[1].Id);

         SetupGetItems(_snapShotMock, planState, runSelection, runSelectionRequest);

         var sut = new MeasurementDataSourceRefresherService();

         //act
         var newSnapShot = sut.Refresh(_snapShotMock.Object, plans, Array.Empty<RunDescriptor>());

         //Assert
         AssertNewSnapShot(newSnapShot, false, null, plans, null, Array.Empty<RunDescriptor>());
      }

      [Test]
      public void RefreshShouldUpdatePlanStateWithNewPlansClearRunsSelectedAndRequestedAsSelectedPlanNoLongerExist()
      {
         //Arrange
         var plans = GetPlans();
         var runs = ImmutableList.CreateRange(new RunDescriptor[] { new RunDescriptor { Id = Guid.NewGuid() }, new RunDescriptor(), });
         var planState = new PlansState(plans.ToImmutableList()).With(plans[0], runs);
         var runSelection = new RunSelection().WithNewSelectedRun(runs[0].Id, null);
         var runSelectionRequest = new RunSelectionRequest().WithNewRequest(runs[1].Id);

         SetupGetItems(_snapShotMock, planState, runSelection, runSelectionRequest);

         var sut = new MeasurementDataSourceRefresherService();
         var newPlans = plans.Skip(1);
         //act
         var newSnapShot = sut.Refresh(_snapShotMock.Object, newPlans, Array.Empty<RunDescriptor>());

         //Assert
         AssertNewSnapShot(newSnapShot, false, null, newPlans, null, Array.Empty<RunDescriptor>());
      }

      private void AssertNewSnapShot(ISnapShot newSnapShot, bool expectedRequestPending, Guid? expectedSelectedRun, IEnumerable<PlanDescriptor> expectedPlans, IDescriptor expectedSelectedPlanPart, IEnumerable<RunDescriptor> expectedRuns)
      {
         var newPlanState = newSnapShot.GetItems<PlansState>().Single();
         var newRunSelection = newSnapShot.GetItems<RunSelection>().Single();
         var newRunSelectionRequest = newSnapShot.GetItems<RunSelectionRequest>().Single();

         Assert.AreEqual(expectedRequestPending, newRunSelectionRequest.Pending);
         Assert.AreEqual(expectedSelectedRun, newRunSelection.SelectedRun);
         CollectionAssert.AreEqual(expectedPlans, newPlanState.PlanList);
         Assert.AreEqual(expectedSelectedPlanPart, newPlanState.SelectedPlanDataSource);
         CollectionAssert.AreEqual(expectedRuns, newPlanState.RunList);
      }

      private void SetupGetItems(Mock<ISnapShot> snapShotMock, PlansState plansState, RunSelection runSelection, RunSelectionRequest runSelectionRequest)
      {
         ServicesTestHelper.SetupGetItemsForNewItem(snapShotMock, plansState);
         ServicesTestHelper.SetupGetItemsForNewItem(snapShotMock, runSelection);
         ServicesTestHelper.SetupGetItemsForNewItem(snapShotMock, runSelectionRequest);
      }

      private void SetupUpdateItem(Mock<ISnapShot> snapShotMock)
      {
         ServicesTestHelper.SetupUpdateItem<PlansState>(snapShotMock);
         ServicesTestHelper.SetupUpdateItem<RunSelection>(snapShotMock);
         ServicesTestHelper.SetupUpdateItem<RunSelectionRequest>(snapShotMock);
      }

      private PlanDescriptor[] GetPlans()
      {
         var part1 = new PartDescriptor { Id = Guid.NewGuid(), Name = "Part 1" };
         var part2 = new PartDescriptor { Id = Guid.NewGuid(), Name = "Part 2" };

         var plans = new[]
         {
            new PlanDescriptor { Id = Guid.NewGuid(), Name = "Plan 1", Part = null},
            new PlanDescriptor { Id = Guid.NewGuid(), Name = "Plan 2", Part = part1 },
            new PlanDescriptor { Id = Guid.NewGuid(), Name = "Plan 3", Part = part2 },
            new PlanDescriptor { Id = Guid.NewGuid(), Name = "Plan 4", Part = part2 }
         };

         return plans;
      }
   }
}
