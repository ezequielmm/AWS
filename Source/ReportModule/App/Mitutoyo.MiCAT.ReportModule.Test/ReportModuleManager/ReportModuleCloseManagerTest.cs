// <copyright file="ReportModuleCloseManagerTest.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.ReportModule.ReportModuleManager;
using Mitutoyo.MiCAT.ReportModuleApp.Manager;
using Mitutoyo.MiCAT.ShellModule;
using Moq;
using NUnit.Framework;

namespace Mitutoyo.MiCAT.ReportModule.Test.ReportModuleManager
{
   [ExcludeFromCodeCoverage]
   [TestFixture]
   public class ReportModuleCloseManagerTest
   {
      private IAppStateHistory _history;
      private IWorkspaceSelectionManager _workspaceSelectionManager;
      private Mock<IWorskpaceContainerResolver> _workspaceContainerResolverMock;
      private Mock<IReportModuleInfo> _moduleInfoMock;
      private IReportModuleCloseManager _reportModuleCloseManager;
      private Mock<IWorkspaceCloseManager> _workSpaceCloseManagerMock;
      [SetUp]
      public void Setup()
      {
         _history = AppStateHelper.CreateAndInitializeAppStateHistory();
         _workspaceSelectionManager = new WorkspaceSelectionManager();
         _workspaceContainerResolverMock = new Mock<IWorskpaceContainerResolver>();
         _moduleInfoMock = new Mock<IReportModuleInfo>();

         _reportModuleCloseManager = new ReportModuleCloseManager(_history, _workspaceSelectionManager,
            _workspaceContainerResolverMock.Object, _moduleInfoMock.Object);

         _workSpaceCloseManagerMock = new Mock<IWorkspaceCloseManager>();
      }
      [Test]
      public void AnyUnsavedChangeOnApplicationShouldReturnTrueForUnsavedChanges()
      {
         // Arrange
         var reportModuleCloseManager = new ReportModuleCloseManager(_history, _workspaceSelectionManager,
            _workspaceContainerResolverMock.Object, _moduleInfoMock.Object);
         _workSpaceCloseManagerMock.Setup(w => w.HasUnsavedChanges()).Returns(true);
         var id1 = Id.Empty;
         _moduleInfoMock.Setup(m => m.ChildIds).Returns(ImmutableList.Create((Id)id1));
         _workspaceContainerResolverMock.Setup(w => w.GetWorkspaceController(id1)).Returns(_workSpaceCloseManagerMock.Object);
         //Act
         var ret = reportModuleCloseManager.AnyUnsavedChangeOnApplication();
         //Assert
         Assert.IsTrue(ret);
      }
      [Test]
      public async Task CloseWorkspaceInstanceByIdShouldDeleteItemFromSnapShot()
      {
         // Arrange
         var child = new NavigationModuleChildInfo(_history.UniqueValueFactory, "name");
         _history.Run(snapShot => AddItem(snapShot, child));

         _workspaceContainerResolverMock.Setup(w => w.GetWorkspaceController(child.Id)).Returns(_workSpaceCloseManagerMock.Object);
         _workSpaceCloseManagerMock.Setup(x => x.OnWorkspaceClosing()).Returns(Task.FromResult(WorkspaceClosingResult.ContinueClosing));
         _moduleInfoMock.Setup(m => m.ChildIdExists(It.IsAny<Id>())).Returns(true);

         //Act
         await _reportModuleCloseManager.CloseWorkspaceInstanceById(child.Id);
         //Assert
         var result = _history.CurrentSnapShot.GetItems<NavigationModuleChildInfo>();
         Assert.Zero(result.Count(r=>r.Id == child.Id));
      }

      private ISnapShot AddItem(ISnapShot snapShot,IStateItem item)
      {
         return snapShot.AddItem(item);
      }
      [Test]
      public async Task CloseAllWorkspaceInstancesShouldDeletItemFromSnapShot()
      {
         // Arrange
         var child = new NavigationModuleChildInfo(_history.UniqueValueFactory, "name");
         _history.Run(ss => AddItem(ss, child));

         _moduleInfoMock.Setup(m => m.ChildIds).Returns(ImmutableList.Create((Id)child.Id));

         //Act
         await _reportModuleCloseManager.CloseAllWorkspaceInstances();
         //Assert
         var result = _history.CurrentSnapShot.GetItems<NavigationModuleChildInfo>();
         Assert.Zero(result.Count(r => r.Id == child.Id));
      }
   }
}
