// <copyright file="UndoRedoControllerTest.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.ApplicationState.Controllers;
using Mitutoyo.MiCAT.ReportModule.App.AppState;
using Mitutoyo.MiCAT.ReportModule.ControllerIntegration.Test.ControllerTests.TestHelper;
using Mitutoyo.MiCAT.ReportModule.Domain.Components;
using Mitutoyo.MiCAT.ReportModuleApp.Controllers;
using NUnit.Framework;

namespace Mitutoyo.MiCAT.ReportModule.ControllerIntegration.Test.ControllerTests
{
   [ExcludeFromCodeCoverage]
   [TestFixture]
   public class UndoRedoControllerTest : BaseAppStateTest
   {
      private UndoRedoController controller;

      public static ISnapShot BuilderHelper(ISnapShot snapShot)
      {
         snapShot = AppStateReportComponentTestHelper.InitializeSnapShot(snapShot);
         snapShot = snapShot.AddCollection<ReportModeState>(AppStateKinds.Undoable);
         snapShot = snapShot.AddCollection<ReportTextBox>(AppStateKinds.Undoable);

         return snapShot;
      }

      private ISnapShot AddComponent(ISnapShot snapShot, IReportComponent component)
      {
         return snapShot.AddItem(component);
      }

      [SetUp]
      public virtual void Setup()
      {
         SetUpHelper(BuilderHelper);

         controller = new UndoRedoController(_history);
      }

      [Test]
      public async Task Undo_IfThereIsNoChangesShouldNotDoUndo()
      {
         var currentSnapShot = _history.CurrentSnapShot;
         // Act
         await controller.Undo();

         // Assert
         Assert.AreEqual(currentSnapShot, _history.CurrentSnapShot);
      }

      [Test]
      public async Task Undo_IfThereAreChangesShouldDoUndoIfIsOnEditMode()
      {
         // Arrange
         var placement1 = new ReportComponentPlacement(_history.CurrentSnapShot.GetReportBodyId(), 1, 2, 3, 4);
         var component1 = new ReportTextBox(placement1);

         var snapshot = _history.NextSnapShot(ControllerCall.Empty);
         snapshot = snapshot.AddItem(new ReportModeState(true));
         _history.AddSnapShot(snapshot);

         _history.RunUndoable(snapShot => AddComponent(snapShot, component1));

         var placement2 = new ReportComponentPlacement(_history.CurrentSnapShot.GetReportBodyId(), 10, 20, 30, 40);
         var component2 = new ReportTextBox(placement2);

         var snapshot2 = _history.NextSnapShot(ControllerCall.Empty);
         _history.AddSnapShot(snapshot2);

         _history.RunUndoable(snapShot => AddComponent(snapShot, component2));

         // Act
         await controller.Undo();

         // Assert
         var result = _history.CurrentSnapShot.GetItems<ReportTextBox>();
         Assert.AreEqual(1, result.Count());
      }

      [Test]
      public async Task Redo_IfThereIsNoChangesShouldNotDoRedo()
      {
         //Arrange
         var currentSnapShot = _history.CurrentSnapShot;
         // Act
         await controller.Redo();

         // Assert
         Assert.AreEqual(currentSnapShot, _history.CurrentSnapShot);
      }

      [Test]
      public async Task Redo_IfThereAreChangesShouldDoRedoIfIsOnEditMode()
      {
         // Arrange
         var placement1 = new ReportComponentPlacement(_history.CurrentSnapShot.GetReportBodyId(), 1, 2, 3, 4);
         var component1 = new ReportTextBox(placement1);

         var snapshot = _history.NextSnapShot(ControllerCall.Empty);
         snapshot = snapshot.AddItem(new ReportModeState(true));
         _history.AddSnapShot(snapshot);

         _history.RunUndoable(snapShot => AddComponent(snapShot, component1));

         var placement2 = new ReportComponentPlacement(_history.CurrentSnapShot.GetReportBodyId(), 10, 20, 30, 40);
         var component2 = new ReportTextBox(placement2);

         var snapshot2 = _history.NextSnapShot(ControllerCall.Empty);
         _history.AddSnapShot(snapshot2);

         _history.RunUndoable(snapShot => AddComponent(snapShot, component2));

         // Act
         await controller.Undo();
         await controller.Redo();

         // Assert
         var result = _history.CurrentSnapShot.GetItems<ReportTextBox>();
         Assert.AreEqual(2, result.Count());
      }
   }
}
