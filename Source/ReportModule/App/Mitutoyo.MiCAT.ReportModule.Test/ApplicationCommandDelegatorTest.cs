// <copyright file="ApplicationCommandDelegatorTest.cs" company="Mitutoyo Europe GmbH">
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
using Mitutoyo.MiCAT.ApplicationState.Controllers;
using Mitutoyo.MiCAT.Core;
using Mitutoyo.MiCAT.ReportModule.ReportModuleManager;
using Mitutoyo.MiCAT.ReportModuleApp.Manager;
using Mitutoyo.MiCAT.ShellModule;
using Moq;
using NUnit.Framework;
using Unity;

namespace Mitutoyo.MiCAT.ReportModule.Test
{
   [ExcludeFromCodeCoverage]
   [TestFixture]
   public class ApplicationCommandDelegatorTest
   {
      private IAppStateHistory _history;
      private NavigationModuleChildInfo _child1;
      private NavigationModuleChildInfo _child2;
      private IUnityContainer _container1;
      private IUnityContainer _container2;
      private IWorkspaceCloseManager _workspaceController1;
      private IWorkspaceCloseManager _workspaceController2;
      private ISnapShot _snapShot;
      private IReportModuleNavigation _reportModuleNavigation;
      private IGlobalCommands _globalCommands;
      private NavigationModuleInfo _module;
      private IReportModuleInfo _moduleInfo;
      private IReportModuleCloseManager _reportModuleCloseManager;
      private IWorkspaceSelectionManager _reportModuleSelecteManager;
      private IWorskpaceContainerResolver _worskpaceContainerResolver;

      [SetUp]
      public void Init()
      {
         _history = AppStateHelper.CreateAndInitializeAppStateHistory();
         _container1 = Mock.Of<IUnityContainer>();
         _container2 = Mock.Of<IUnityContainer>();
         _globalCommands = new GlobalCommands();
         _reportModuleNavigation = Mock.Of<IReportModuleNavigation>();
         _workspaceController1 = Mock.Of<IWorkspaceCloseManager>();
         _workspaceController2 = Mock.Of<IWorkspaceCloseManager>();
         _moduleInfo = Mock.Of<IReportModuleInfo>();
         _worskpaceContainerResolver = new WorskpaceContainerResolver(_reportModuleNavigation);
         _reportModuleSelecteManager = new WorkspaceSelectionManager();

         _reportModuleCloseManager = new ReportModuleCloseManager(_history, _reportModuleSelecteManager,
            _worskpaceContainerResolver, _moduleInfo);
         new ApplicationCommandDelegator(_globalCommands, _reportModuleCloseManager);

         Mock
            .Get(_container1)
            .Setup(x => x.Resolve(typeof(IWorkspaceCloseManager), null))
            .Returns(_workspaceController1);
         Mock
            .Get(_container2)
            .Setup(x => x.Resolve(typeof(IWorkspaceCloseManager), null))
            .Returns(_workspaceController2);

         SetupAppState();
         var compositeHasTask = new CompositeHasTask(new TimingContext(ContextKey.None, "key"));
         compositeHasTask.Close();
         Mock.Get(_reportModuleNavigation).Setup(m => m.GetContainer(_child1.Id)).Returns(_container1);
         Mock.Get(_reportModuleNavigation).Setup(m => m.GetContainer(_child2.Id)).Returns(_container2);
         Mock.Get(_moduleInfo).Setup(m => m.ChildIds).Returns(ImmutableList.Create(new Id[] {_child1.Id, _child2.Id}));
      }

      [Test]
      [TestCase(true, true, false)]
      [TestCase(true, false, false)]
      [TestCase(false, true, false)]
      [TestCase(false, false, true)]
      public void CanExceuteForExitCommandTest(bool hasChanges1, bool hasChanges2, bool canExecuteExpected)
      {
         //Arrange
         Mock.Get(_workspaceController1)
            .Setup(x => x.HasUnsavedChanges())
            .Returns(hasChanges1);
         Mock.Get(_workspaceController2)
            .Setup(x => x.HasUnsavedChanges())
            .Returns(hasChanges2);

         //Act
         var canExecuteResult = _globalCommands.ExitCommand.CanExecute(null);

         //Assert
         Assert.AreEqual(canExecuteExpected, canExecuteResult);
      }

      [Test]
      public void ExitCommandRemoveAllWorkspaceWheExecutesTest()
      {
         //Arrange
         Mock.Get(_workspaceController1)
            .Setup(x => x.HasUnsavedChanges())
            .Returns(false);
         Mock.Get(_workspaceController2)
            .Setup(x => x.HasUnsavedChanges())
            .Returns(false);

         //Act
         _globalCommands.ExitCommand.Execute(null);

         //Assert
         Assert.IsFalse(_history.CurrentSnapShot.ContainsItem(_child1.Id));
         Assert.IsFalse(_history.CurrentSnapShot.ContainsItem(_child2.Id));
      }

      [Test]
      public void CloseReportTestShouldCloseWhenContinueClosingIsReturnedByWorkspaceController()
      {
         Mock.Get(_workspaceController2)
            .Setup(x => x.OnWorkspaceClosing())
            .Returns(Task.FromResult(WorkspaceClosingResult.ContinueClosing));
         Mock.Get(_moduleInfo).Setup(m => m.ChildIdExists(It.IsAny<Id>())).Returns(true);
         _globalCommands.InstanceCloseCommand.Execute(_child2.Id);

         var snapShot = _history.CurrentSnapShot;
         var childItems = snapShot.GetItems<NavigationModuleToChild>().Select(i => i.TargetId);
         var currentSelection = snapShot.GetItems<SelectedItem>().SingleOrDefault();

         Assert.IsFalse(childItems.Contains(_child2.Id));
         Assert.IsTrue(currentSelection.TargetId.Equals(_child1.Id));
      }
      [Test]
      public void CloseReportTestShouldNotExecuteClientWhenIdNotExist()
      {
         Mock.Get(_workspaceController2)
            .Setup(x => x.OnWorkspaceClosing())
            .Returns(Task.FromResult(WorkspaceClosingResult.ContinueClosing));
         Mock.Get(_moduleInfo).Setup(m => m.ChildIdExists(It.IsAny<Id>())).Returns(false);
         _globalCommands.InstanceCloseCommand.Execute(_child2.Id);
      }

      [Test]
      public void CloseReportTestShouldNotCloseWhenAbortIsReturnedByWorkspaceController()
      {
         Mock.Get(_workspaceController2)
            .Setup(x => x.OnWorkspaceClosing())
            .Returns(Task.FromResult(WorkspaceClosingResult.Abort));

         _globalCommands.InstanceCloseCommand.Execute(_child2.Id);

         var snapShot = _history.CurrentSnapShot;
         var childItems = snapShot.GetItems<NavigationModuleToChild>().Select(i => i.TargetId);
         var currentSelection = snapShot.GetItems<SelectedItem>().SingleOrDefault();

         Assert.IsTrue(childItems.Contains(_child2.Id));
         Assert.IsTrue(currentSelection.TargetId.Equals(_child2.Id));
      }

      [Test]
      public void CloseReport_NextItemSelectionTestWhenContinueClosingIsReturnedByWorkspaceController()
      {
         Mock.Get(_workspaceController2)
            .Setup(x => x.OnWorkspaceClosing())
            .Returns(Task.FromResult(WorkspaceClosingResult.ContinueClosing));
         Mock.Get(_moduleInfo).Setup(m => m.ChildIdExists(It.IsAny<Id>())).Returns(true);
         // Setup Next selectable item is in Popup window
         SetupPopupWindowItem(_child1.Id);

         _globalCommands.InstanceCloseCommand.Execute(_child2.Id);

         var snapShot = _history.CurrentSnapShot;
         var currentSelection = snapShot.GetItems<SelectedItem>().SingleOrDefault();

         Assert.IsTrue(currentSelection.TargetId.Equals(_module.Id));
      }

      [Test]
      public void CloseReport_NextItemSelectionTestWhenAbortIsReturnedByWorkspaceController()
      {
         Mock.Get(_workspaceController1)
            .Setup(x => x.OnWorkspaceClosing())
            .Returns(Task.FromResult(WorkspaceClosingResult.Abort));

         // Setup Next selectable item is in Popup window
         SetupPopupWindowItem(_child1.Id);

         _globalCommands.InstanceCloseCommand.Execute(_child2.Id);

         var snapShot = _history.CurrentSnapShot;
         var currentSelection = snapShot.GetItems<SelectedItem>().SingleOrDefault();

         Assert.IsTrue(currentSelection.TargetId.Equals(_child2.Id));
      }

      private void SetupPopupWindowItem(Id id)
      {
         _snapShot = _history.NextSnapShot(ControllerCall.Empty);

         var windowEntity = _snapShot.GetItems<PopupWindowEntity>().Single();
         var popupItem = new PopupSelectionAssociation(_snapShot.UniqueValue, windowEntity.Id, id, 0);
         _snapShot = _snapShot.AddItem(popupItem);

         _history.AddSnapShot(_snapShot);
      }
      private void SetupAppState()
      {
         _snapShot = _history.NextSnapShot(ControllerCall.Empty);
         _module = new NavigationModuleInfo(_snapShot.UniqueValue, "module", "icon", "p", "a", "R");
         _child1 = new NavigationModuleChildInfo(_snapShot.UniqueValue, "Report 1");
         _child2 = new NavigationModuleChildInfo(_snapShot.UniqueValue, "Report 2");

         var selectionId = _snapShot.GetItems<Selection>().Single().Id;
         var selectedItem = new SelectedItem(_snapShot.UniqueValue, selectionId, _child2.Id, 0);

         var nl = _snapShot.GetItems<NavigationList>().Single();
         var listToModule = new NavigationListToModule(_history.UniqueValueFactory.UniqueValue(), nl.Id, _module.Id, 0);
         var moduleToChild = new NavigationModuleToChild(_history.UniqueValueFactory.UniqueValue(), _module.Id, _child1.Id, 0);
         var moduleToChild2 = new NavigationModuleToChild(_history.UniqueValueFactory.UniqueValue(), _module.Id, _child2.Id, 1);

         _snapShot = _snapShot.AddItem(_module)
            .AddItem(_child1)
            .AddItem(_child2)
            .AddItem(listToModule)
            .AddItem(moduleToChild)
            .AddItem(moduleToChild2);

         _snapShot = _snapShot.AddItem(selectedItem);

         _history.AddSnapShot(_snapShot);
      }
   }
}
