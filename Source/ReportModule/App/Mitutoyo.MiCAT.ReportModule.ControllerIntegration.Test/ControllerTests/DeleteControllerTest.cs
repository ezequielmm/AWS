// <copyright file="DeleteControllerTest.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.ApplicationState.Controllers;
using Mitutoyo.MiCAT.ReportModule.Domain.Components;
using Mitutoyo.MiCAT.ReportModule.Domain.Components.ReportSections;
using Mitutoyo.MiCAT.ReportModule.Domain.Data;
using Mitutoyo.MiCAT.ReportModuleApp.Controllers;
using Nito.AsyncEx;
using NUnit.Framework;

namespace Mitutoyo.MiCAT.ReportModule.ControllerIntegration.Test.ControllerTests
{
   [ExcludeFromCodeCoverage]
   [TestFixture]
   public class DeleteControllerTest : BaseAppStateTest
   {
      private DeleteComponentController _controller;

      public static ISnapShot BuildHelper(ISnapShot snapShot)
      {
         snapShot = snapShot.AddCollection<ReportBody>(AppStateKinds.Undoable);
         snapShot = snapShot.AddCollection<ReportImage>(AppStateKinds.Undoable);
         snapShot = snapShot.AddCollection<ReportTextBox>(AppStateKinds.Undoable);
         snapShot = snapShot.AddCollection<ReportHeaderForm>(AppStateKinds.Undoable);
         snapShot = snapShot.AddCollection<ReportHeaderFormField>(AppStateKinds.Undoable);
         snapShot = snapShot.AddCollection<ReportHeaderFormRow>(AppStateKinds.Undoable);
         snapShot = snapShot.AddCollection<ReportComponentSelection>(AppStateKinds.Undoable);

         return snapShot;
      }

      [SetUp]
      public virtual void Setup()
      {
         SetUpHelper(BuildHelper);

         _controller = new DeleteComponentController(_history);
      }

      [Test]
      public void DeleteComponent_DeleteComponentAndPreserveOthers()
      {
         // Arrange
         var components = ArrangeDeletedAndPreservedComponents();

         // Act
         _controller.DeleteComponent(components.ComponentToDelete.Id as IItemId<IReportComponent>);

         // Assert
         AssertDeletedAndPreservedComponents(components.ComponentToDelete, components.ComponentToPreserve, components.ComponentSelection);
      }

      [Test]
      public void DeleteSelectedComponent_DeleteSelectedComponentAndPreserveOthers()
      {
         // Arrange
         var components = ArrangeDeletedAndPreservedComponents();

         // Act
         AsyncContext.Run(async () => await _controller.DeleteSelectedComponents());

         // Assert
         AssertDeletedAndPreservedComponents(components.ComponentToDelete, components.ComponentToPreserve, components.ComponentSelection);
      }

      [Test]
      public void DeleteSelectedComponent_DeleteHeaderFormAndExtraItems()
      {
         // Arrange
         var info = ArrangeHeaderForm();

         // Act
         AsyncContext.Run(async () => await _controller.DeleteComponent(info.HeaderForm.Id));

         //Assert
         AssertDeletedHeaderForm(info.HeaderForm, info.ComponentSelection, info.Fields);
      }

      [Test]
      public void DeleteSelectedComponent_DeleteSelectedHeaderFormAndExtraItems()
      {
         // Arrange
         var info = ArrangeHeaderForm();

         // Act
         AsyncContext.Run(async () => await _controller.DeleteSelectedComponents());

         //Assert
         AssertDeletedHeaderForm(info.HeaderForm, info.ComponentSelection, info.Fields);
      }

      private (IReportComponent ComponentToDelete, IReportComponent ComponentToPreserve, ReportComponentSelection ComponentSelection)
         ArrangeDeletedAndPreservedComponents()
      {
         var bodySection = new ReportBody();
         var placementToDelete = new ReportComponentPlacement(bodySection.Id, 0, 0, 10, 10);
         var componentToDelete = new ReportImage(placementToDelete);

         var placementToPreserve = new ReportComponentPlacement(bodySection.Id, 50, 50, 20, 20);
         var componentToPreserve = new ReportTextBox(placementToPreserve);

         var componentSelection = new ReportComponentSelection(new[] { componentToDelete.Id });

         var snapshot = _history.NextSnapShot(ControllerCall.Empty);
         snapshot = snapshot.AddItem(bodySection);
         snapshot = snapshot.AddItem(componentToDelete);
         snapshot = snapshot.AddItem(componentToPreserve);
         snapshot = snapshot.AddItem(componentSelection);

         _history.AddSnapShot(snapshot);

         return (componentToDelete, componentToPreserve, componentSelection);
      }

      private (ReportHeaderForm HeaderForm, ReportComponentSelection ComponentSelection, ReportHeaderFormField[] Fields)
         ArrangeHeaderForm()
      {
         var bodySection = new ReportBody();
         var placement = new ReportComponentPlacement(bodySection.Id, 0, 0, 15, 15);
         var fields = new[] { new ReportHeaderFormField(), new ReportHeaderFormField() };
         var rows = new[] { new ReportHeaderFormRow(new[] { fields[0].Id }), new ReportHeaderFormRow(new[] { fields[1].Id }) };
         var headerForm = new ReportHeaderForm(placement, rows.Select(r => r.Id));

         var componentSelection = new ReportComponentSelection(new[] { headerForm.Id });

         var snapshot = _history.NextSnapShot(ControllerCall.Empty);
         snapshot = snapshot.AddItem(bodySection);
         snapshot = snapshot.AddItem(headerForm);
         snapshot = snapshot.AddItem(rows[0]);
         snapshot = snapshot.AddItem(rows[1]);
         snapshot = snapshot.AddItem(fields[0]);
         snapshot = snapshot.AddItem(fields[1]);
         snapshot = snapshot.AddItem(componentSelection);

         _history.AddSnapShot(snapshot);

         return (headerForm, componentSelection, fields);
      }

      private void AssertDeletedAndPreservedComponents(IReportComponent componentToDelete, IReportComponent componentToPreserve, ReportComponentSelection componentSelection)
      {
         Assert.False(_history.CurrentSnapShot.ContainsItem(componentToDelete.Id));
         Assert.True(_history.CurrentSnapShot.ContainsItem(componentToPreserve.Id));

         Assert.False(_history.CurrentSnapShot.ContainsItem(componentToDelete.Id));
         Assert.True(_history.CurrentSnapShot.ContainsItem(componentToPreserve.Id));

         var componentSelectionToTest = _history.CurrentSnapShot.GetItem(componentSelection.Id);
         Assert.False(componentSelectionToTest.SelectedReportComponentIds.Contains(componentToDelete.Id));
      }

      private void AssertDeletedHeaderForm(ReportHeaderForm headerForm, ReportComponentSelection componentSelection, ReportHeaderFormField[] fields)
      {
         Assert.False(_history.CurrentSnapShot.ContainsItem(headerForm.Id));
         Assert.False(_history.CurrentSnapShot.ContainsItem(headerForm.RowIds.ElementAt(0)));
         Assert.False(_history.CurrentSnapShot.ContainsItem(headerForm.RowIds.ElementAt(1)));
         Assert.False(_history.CurrentSnapShot.ContainsItem(fields[0].Id));
         Assert.False(_history.CurrentSnapShot.ContainsItem(fields[1].Id));

         var componentSelectionToTest = _history.CurrentSnapShot.GetItem(componentSelection.Id);
         Assert.False(componentSelectionToTest.SelectedReportComponentIds.Contains(headerForm.Id));
      }
   }
}
