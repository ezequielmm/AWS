// <copyright file="VMPartsTest.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.ReportModule.Domain.Data;
using Mitutoyo.MiCAT.ReportModule.GUI.Common.BusyIndicators;
using Mitutoyo.MiCAT.ReportModule.GUI.Dialog;
using Mitutoyo.MiCAT.ReportModule.GUI.Navigation;
using Mitutoyo.MiCAT.ReportModule.GUI.Navigation.TreeView;
using Mitutoyo.MiCAT.ReportModuleApp.Controllers;
using Mitutoyo.MiCAT.ReportModuleApp.Utilities;
using Moq;
using NUnit.Framework;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Test.Navigation
{
   [ExcludeFromCodeCoverage]
   [TestFixture]
   public class VMPartsTest
   {
      private Mock<IAppStateHistory> _historyMock;
      private Mock<IPlanController> _planControllerMock;
      private Mock<IDialogService> _dialogServiceMock;
      private Mock<IBusyIndicator> _busyIndicatorMock;
      private ITreeViewItemListUpdater _treeViewItemListUpdater;
      private IActionCaller _actionCaller;
      private VMAppStateTestManager _vmAppStateTestManager;

      [SetUp]
      public void Setup()
      {
         _busyIndicatorMock = new Mock<IBusyIndicator>();
         _historyMock = new Mock<IAppStateHistory>();
         _historyMock.Setup(h => h.AddClient(It.IsAny<ISubscriptionClient>(), 0)).Returns(_historyMock.Object);
         _planControllerMock = new Mock<IPlanController>();
         _dialogServiceMock = new Mock<IDialogService>();
         _treeViewItemListUpdater = new TreeViewItemListUpdater();
         _vmAppStateTestManager = new VMAppStateTestManager();

         _actionCaller = new BusyIndicatorActionCaller(_busyIndicatorMock.Object);
      }

      [Test]
      public void SelectPlanShouldCallControllerSelectPlan()
      {
         var planItem = new VmPlanTreeViewItem(_planControllerMock.Object, _dialogServiceMock.Object, _actionCaller) { Id = Guid.NewGuid(), Name = "Test" };

         //Act
         planItem.SelectItemCommand.Execute(null);

         //Assert
         _planControllerMock.Verify(p => p.SelectPlan(planItem.Id), Times.Once);
      }

      [Test]
      public void SelectPartShouldCallControllerSelectPart()
      {
         var partItem = new VmPartTreeViewItem(_planControllerMock.Object, _actionCaller) { Id = Guid.NewGuid(), Name = "Test" };

         //Act
         partItem.SelectItemCommand.Execute(null);

         //Assert
         _planControllerMock.Verify(p => p.SelectPart(partItem.Id), Times.Once);
      }

      [Test]
      public void UpdateFirstTimeShouldPopulatePartList()
      {
         //Arrange
         var sut = new VMParts(_planControllerMock.Object, _historyMock.Object, _dialogServiceMock.Object, _actionCaller, _busyIndicatorMock.Object, _treeViewItemListUpdater);

         var plan = new PlanDescriptor() { Id = Guid.NewGuid(), Name = "Test", Part = new PartDescriptor() { Id = Guid.Empty } };
         var plansState = new PlansState().With(ImmutableList.Create<PlanDescriptor>(plan));

         _vmAppStateTestManager.UpdateSnapshot(plansState);

         //Act
         sut.Update(_vmAppStateTestManager.History.CurrentSnapShot);

         //Assert
         Assert.That(sut.TreeViewItems.ToImmutableList().Count == 1);
      }

      [Test]
      public void UpdateWithSamePartListDontChangeList()
      {
         //Arrange
         var sut = new VMParts(_planControllerMock.Object, _historyMock.Object, _dialogServiceMock.Object, _actionCaller, _busyIndicatorMock.Object, _treeViewItemListUpdater);
         var plan = new PlanDescriptor() { Id = Guid.NewGuid(), Name = "Test plan", Part = new PartDescriptor() { Id = Guid.NewGuid(), Name = "Test part" } };

         var plansState = new PlansState().With(ImmutableList.Create<PlanDescriptor>(plan));

         var partHash = sut.TreeViewItems.GetHashCode();
         _vmAppStateTestManager.UpdateSnapshot(plansState);

         //Act
         sut.Update(_vmAppStateTestManager.History.CurrentSnapShot);

         //Assert
         Assert.AreEqual(partHash, sut.TreeViewItems.GetHashCode());
      }
   }
}
