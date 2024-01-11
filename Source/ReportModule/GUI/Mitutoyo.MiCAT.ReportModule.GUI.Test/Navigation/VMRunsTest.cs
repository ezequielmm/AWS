// <copyright file="VMRunsTest.cs" company="Mitutoyo Europe GmbH">
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
using Moq;
using NUnit.Framework;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Test.Navigation
{
   [ExcludeFromCodeCoverage]
   [TestFixture]
   public class VMRunsTest
   {
      private Mock<IAppStateHistory> _historyMock;
      private Mock<IRunController> _runControllerMock;
      private Mock<IRunRequestController> _runRequestControllerMock;
      private Mock<IDialogService> _dialogServiceMock;
      private Mock<IBusyIndicator> _busyIndicatorMock;
      private ITreeViewItemListUpdater _treeViewItemListUpdater;
      private VMAppStateTestManager _vmAppStateTestManager;
      private BusyIndicatorActionCaller _busyIndicatorActionCaller;

      [SetUp]
      public void Setup()
      {
         _historyMock = new Mock<IAppStateHistory>();
         _historyMock.Setup(h => h.AddClient(It.IsAny<ISubscriptionClient>(), 0)).Returns(_historyMock.Object);
         _runControllerMock = new Mock<IRunController>();
         _runRequestControllerMock = new Mock<IRunRequestController>();
         _dialogServiceMock = new Mock<IDialogService>();
         _busyIndicatorMock = new Mock<IBusyIndicator>();
         _treeViewItemListUpdater = new TreeViewItemListUpdater();
         _busyIndicatorMock.Setup(b => b.SetIsBusyTrueUntilUIIsIdle()).Callback(() => { });
         _busyIndicatorActionCaller = new BusyIndicatorActionCaller(_busyIndicatorMock.Object);

         _vmAppStateTestManager = new VMAppStateTestManager();
      }

      [Test]
      public void SelectRunShouldCallController()
      {
         //Arrange
         var run = new RunDescriptor() { Id = Guid.NewGuid() };
         var runNode = new VmRunTreeViewItem(_runControllerMock.Object, _runRequestControllerMock.Object, _dialogServiceMock.Object, _busyIndicatorActionCaller) { Id = run.Id };

         //Act
         runNode.SelectItemCommand.Execute(runNode);

         //Assert
         _runRequestControllerMock.Verify(p => p.RequestRun(runNode.Id), Times.Once);
      }

      [Test]
      public void UpdateRunShoulOverrideRunList()
      {
         //Arrange
         var sut = new VMRuns(_runControllerMock.Object, _runRequestControllerMock.Object, _historyMock.Object, _dialogServiceMock.Object, _busyIndicatorActionCaller, _busyIndicatorMock.Object, _treeViewItemListUpdater);

         var run = new RunDescriptor() { Id = Guid.NewGuid(), TimeStamp = new DateTime() };
         var plansState = new PlansState().WithRunList(ImmutableList.Create<RunDescriptor>(run));
         _vmAppStateTestManager.UpdateSnapshot(plansState);

         bool runsCollectionChanged = false;

         sut.Runs.CollectionChanged += (s, e) => runsCollectionChanged = true;

         //Act
         sut.Update(_vmAppStateTestManager.CurrentSnapShot);

         //Assert
         Assert.IsTrue(runsCollectionChanged);
      }
   }
}
