// <copyright file="VMheaderFormTest.cs" company="Mitutoyo Europe GmbH">
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
using Mitutoyo.MiCAT.ReportModule.App.AppState;
using Mitutoyo.MiCAT.ReportModule.Domain.Components;
using Mitutoyo.MiCAT.ReportModule.Domain.Components.ReportSections;
using Mitutoyo.MiCAT.ReportModule.Domain.Data;
using Mitutoyo.MiCAT.ReportModule.GUI.Properties;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements.HeaderForm.ViewModels;
using Mitutoyo.MiCAT.ReportModuleApp.AppState;
using Mitutoyo.MiCAT.ReportModuleApp.Controllers;
using Moq;
using NUnit.Framework;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Test.VisualElements.ReportElements.HeaderForm.ViewModels
{
   [ExcludeFromCodeCoverage]
   [TestFixture]
   public class VMHeaderFormTest
   {
      private Mock<IVMReportComponentPlacement> _vmPlacementMock;
      private Mock<IHeaderFormController> _headerFormControllerMock;
      private Mock<IHeaderFormFieldController> _headerFormFieldControllerMock;
      private Mock<IAppStateHistory> _history;
      private Mock<IDeleteComponentController> _deleteComponentControllerMock;
      private Mock<ISnapShot> _snapShotMock;
      private Id<ReportHeaderFormRow> _reportHeaderFormRowId;
      private ReportHeaderForm _reportHeaderForm;

      [SetUp]
      public void SetUp()
      {
         var fieldDescriptor =
               new DynamicPropertyDescriptor
               {
                  Id = Guid.NewGuid(),
                  Name = "Field 1",
                  EntityType = EntityType.None
               };
         var dinamicPropertiesState = new DynamicPropertiesState(
                                                new Id<DynamicPropertiesState>(Guid.NewGuid()),
                                                new[] { fieldDescriptor, }.ToImmutableList());

         _headerFormControllerMock = new Mock<IHeaderFormController>();
         _headerFormFieldControllerMock = new Mock<IHeaderFormFieldController>();
         _deleteComponentControllerMock = new Mock<IDeleteComponentController>();

         _snapShotMock = new Mock<ISnapShot>();
         _history = new Mock<IAppStateHistory>();
         _history.Setup(h => h.AddClient(It.IsAny<ISubscriptionClient>(), 0)).Returns(_history.Object);
         _vmPlacementMock = new Mock<IVMReportComponentPlacement>();

         _reportHeaderFormRowId = new Id<ReportHeaderFormRow>(Guid.NewGuid());
         var newReportHeaderFormRow = new ReportHeaderFormRow(Enumerable.Empty<Id<ReportHeaderFormField>>());
         _reportHeaderForm = new ReportHeaderForm(new Id<ReportHeaderForm>(Guid.NewGuid()), new ReportComponentPlacement(new ReportBody().Id, 0, 0, 100, 100), new[] { _reportHeaderFormRowId });

         var changeList = new Changes(new List<UpdateItemChange>() { new UpdateItemChange(_reportHeaderForm.Id, null, _reportHeaderForm) });
         _snapShotMock.Setup(h => h.GetChanges()).Returns(changeList);
         _snapShotMock.Setup(s => s.GetItem<ReportHeaderForm>(_reportHeaderForm.Id as IItemId<IReportComponent>)).Returns(_reportHeaderForm);
         _snapShotMock.Setup(s => s.GetItem(_reportHeaderForm.Id as IItemId<IReportComponent>)).Returns(_reportHeaderForm);
         _snapShotMock.Setup(s => s.GetItem(_reportHeaderFormRowId)).Returns(newReportHeaderFormRow);
         _snapShotMock.Setup(s => s.GetItems<DynamicPropertiesState>()).Returns(new List<DynamicPropertiesState>() { dinamicPropertiesState });
         _snapShotMock.Setup(s => s.GetItems<ReportModeState>()).Returns(new List<ReportModeState>() { new ReportModeState() });
         _history.Setup(h => h.CurrentSnapShot).Returns(_snapShotMock.Object);
      }

      [Test]
      public void VMHeaderForm_DynamicPropertyItemsShouldBePopulated()
      {
         //Act
         var viewModel = new VMHeaderForm(_history.Object,
                                          _reportHeaderForm.Id,
                                          _vmPlacementMock.Object,
                                          _deleteComponentControllerMock.Object,
                                          _headerFormControllerMock.Object,
                                          _headerFormFieldControllerMock.Object);

         //Assert
         CollectionAssert.IsNotEmpty(viewModel.DynamicPropertyItems);
      }

      [Test]
      public void VMHeaderForm_DynamicPropertyItemsShouldContainsEmptyItem()
      {
         //Act
         var viewModel = new VMHeaderForm(_history.Object,
                                          _reportHeaderForm.Id,
                                          _vmPlacementMock.Object,
                                          _deleteComponentControllerMock.Object,
                                          _headerFormControllerMock.Object,
                                          _headerFormFieldControllerMock.Object);

         //Assert
         Assert.IsTrue(viewModel.DynamicPropertyItems.Any(x => x.Id == VMDynamicPropertyItem.EmptyDynamicPropertyItem.Id));
      }

      [Test]
      public void VMHeaderForm_DynamicPropertyItemsShouldBeLocalized()
      {
         //Act
         var viewModel = new VMHeaderForm(_history.Object,
                                          _reportHeaderForm.Id,
                                          _vmPlacementMock.Object,
                                          _deleteComponentControllerMock.Object,
                                          _headerFormControllerMock.Object,
                                          _headerFormFieldControllerMock.Object);

         //Assert
         Assert.IsTrue(viewModel.DynamicPropertyItems.Any(x =>
            x.DisplayName == Resources.FieldItem_SelectProperty &&
            x.Id == VMDynamicPropertyItem.EmptyDynamicPropertyItem.Id));
      }

      [Test]
      public void VMHeaderForm_ShouldAddRowsWhenRowsAdded()
      {
         //Arrange
         var viewModel = new VMHeaderForm(_history.Object,
                                          _reportHeaderForm.Id,
                                          _vmPlacementMock.Object,
                                          _deleteComponentControllerMock.Object,
                                          _headerFormControllerMock.Object,
                                          _headerFormFieldControllerMock.Object);

         var newReportHeaderFormRow = new ReportHeaderFormRow(Enumerable.Empty<Id<ReportHeaderFormField>>());
         var newHeaderForm = new ReportHeaderForm(_reportHeaderForm.Id, new ReportComponentPlacement(new ReportBody().Id, 0, 0, 100, 100), new[] { _reportHeaderFormRowId, newReportHeaderFormRow.Id, });

         var changeList = new Changes(new List<UpdateItemChange>() { new UpdateItemChange(_reportHeaderForm.Id, null, newHeaderForm) });
         _snapShotMock.Setup(h => h.GetChanges()).Returns(changeList);

         _snapShotMock
            .Setup(x => x.GetItem(_reportHeaderForm.Id))
            .Returns(newHeaderForm);

         _snapShotMock
            .Setup(x => x.GetItem(newReportHeaderFormRow.Id))
            .Returns(newReportHeaderFormRow);

         //Act
         viewModel.Update(_snapShotMock.Object);

         //Assert
         Assert.AreEqual(2, viewModel.Rows.Count);
         _history.Verify(x =>
            x.Subscribe(
               It.Is<Subscription>(s => s.IdFilters.Any(f => f.Id.Equals(newReportHeaderFormRow.Id)))),
            Times.Once); // Subscribe header form row
      }

      [Test]
      public void VMHeaderForm_ShouldRemoveRowsWhenRowsRemoved()
      {
         //Arrange
         var viewModel = new VMHeaderForm(_history.Object,
                                          _reportHeaderForm.Id,
                                          _vmPlacementMock.Object,
                                          _deleteComponentControllerMock.Object,
                                          _headerFormControllerMock.Object,
                                          _headerFormFieldControllerMock.Object);

         var newHeaderForm = new ReportHeaderForm(_reportHeaderForm.Id, new ReportComponentPlacement(new ReportBody().Id, 0, 0, 100, 100), Enumerable.Empty<Id<ReportHeaderFormRow>>());

         var changeList = new Changes(new List<UpdateItemChange>() { new UpdateItemChange(_reportHeaderForm.Id, null, newHeaderForm) });
         _snapShotMock.Setup(h => h.GetChanges()).Returns(changeList);

         _snapShotMock
           .Setup(x => x.GetItem(_reportHeaderForm.Id))
           .Returns(newHeaderForm);

         //Act
         viewModel.Update(_snapShotMock.Object);

         //Assert
         Assert.AreEqual(0, viewModel.Rows.Count);
         _history.Verify(x =>
            x.RemoveClient(
               It.IsAny<ISubscriptionClient>()),
            Times.Once); // Unsubscribe header form row
      }

      [Test]
      public void VMHeaderForm_DisposeShouldUnsubscribeToAppStateChangesAndDisposeRows()
      {
         //Arrange
         var vm = new VMHeaderForm(_history.Object,
                                 _reportHeaderForm.Id,
                                 _vmPlacementMock.Object,
                                 _deleteComponentControllerMock.Object,
                                 _headerFormControllerMock.Object,
                                 _headerFormFieldControllerMock.Object);

         //Act
         vm.Dispose();

         //Assert
         _history.Verify(x =>
            x.RemoveClient(
               It.IsAny<ISubscriptionClient>()),
            Times.Exactly(2)); // Unsuscribe header form
      }
   }
}
