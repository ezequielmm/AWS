// <copyright file="WorkspaceSelectionManagerTest.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.ApplicationState.Controllers;
using Mitutoyo.MiCAT.ReportModule.ReportModuleManager;
using Mitutoyo.MiCAT.ShellModule;
using NUnit.Framework;

namespace Mitutoyo.MiCAT.ReportModule.Test.ReportModuleManager
{
   [ExcludeFromCodeCoverage]
   [TestFixture]
   public class WorkspaceSelectionManagerTest
   {
      private IAppStateHistory _history;
      private IWorkspaceSelectionManager _workspaceSelectionManager;
      [SetUp]
      public void Setup()
      {
         _history = AppStateHelper.CreateAndInitializeAppStateHistory();
         SetupAppState();
         _workspaceSelectionManager = new WorkspaceSelectionManager();
      }
      [Test]
      public void DeleteWorkspacecFromSelectionShouldReturnIdForNavigationModuleChildInfoLeft()
      {
         //Arrenge
         var snapShot = _history.NextSnapShot(ControllerCall.Empty);

         var nav = snapShot.GetItems<NavigationModuleChildInfo>().First(x => x.Name == "Report 2");
         var navIdFirst = snapShot.GetItems<NavigationModuleChildInfo>().First(x => x.Name == "Report 1").Id;
         //Act
         snapShot = _workspaceSelectionManager.DeleteWorkspacecFromSelection(snapShot, nav);

         //Assert
         Assert.That(snapShot.GetItems<SelectedItem>().Any(s=>s.TargetId == navIdFirst));
      }
      private void SetupAppState()
      {
         var _snapShot = _history.NextSnapShot(ControllerCall.Empty);
         var _module = new NavigationModuleInfo(_snapShot.UniqueValue, "module", "icon", "p", "a", "R");
         var _child1 = new NavigationModuleChildInfo(_snapShot.UniqueValue, "Report 1");
         var _child2 = new NavigationModuleChildInfo(_snapShot.UniqueValue, "Report 2");

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
