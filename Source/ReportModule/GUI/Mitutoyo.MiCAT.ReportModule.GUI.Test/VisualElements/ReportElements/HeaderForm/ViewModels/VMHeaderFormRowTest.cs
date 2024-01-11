// <copyright file="VMHeaderFormRowTest.cs" company="Mitutoyo Europe GmbH">
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
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements.HeaderForm.ViewModels;
using Mitutoyo.MiCAT.ReportModuleApp.AppState;
using Moq;
using NUnit.Framework;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Test.VisualElements.ReportElements.HeaderForm.ViewModels
{
   [ExcludeFromCodeCoverage]
   [TestFixture]
   public class VMHeaderFormRowTest
   {
      private Mock<IAppStateHistory> _history;
      private Mock<ISnapShot> _snapShotMock;

      private ReportHeaderFormRow _reportHeaderFormRow;
      private ReportHeaderFormField _reportHeaderFormField;

      private Mock<IVMHeaderForm> _vmHeaderFormMock;

      [SetUp]
      public void SetUp()
      {
         var fieldDescriptor = new DynamicPropertyDescriptor
         {
            Id = Guid.NewGuid(),
            Name = "Field 1",
            EntityType = EntityType.None
         };

         var dinamicPropertiesState = new DynamicPropertiesState(
                                                   new Id<DynamicPropertiesState>(Guid.NewGuid()),
                                                   new[] { fieldDescriptor, }.ToImmutableList());

         _reportHeaderFormField = new ReportHeaderFormField();
         _reportHeaderFormRow = new ReportHeaderFormRow(new[] { _reportHeaderFormField.Id, });
         var reportHeaderForm = new ReportHeaderForm(new ReportComponentPlacement(new ReportBody().Id, 0, 0, 100, 100), new[] { _reportHeaderFormRow.Id });

         _snapShotMock = new Mock<ISnapShot>();
         _history = new Mock<IAppStateHistory>();
         _history.Setup(h => h.AddClient(It.IsAny<ISubscriptionClient>(), 0)).Returns(_history.Object);

         var changeList = new List<IItemChange>() { new AddItemChange(reportHeaderForm.Id), new AddItemChange(_reportHeaderFormRow.Id), new AddItemChange(_reportHeaderFormField.Id) };
         _snapShotMock.Setup(h => h.GetChanges()).Returns(new Changes(changeList.ToImmutableList()));
         _snapShotMock.Setup(s => s.GetItem<ReportHeaderForm>(reportHeaderForm.Id as IItemId)).Returns(reportHeaderForm);
         _snapShotMock.Setup(s => s.GetItem(_reportHeaderFormRow.Id)).Returns(_reportHeaderFormRow);
         _snapShotMock.Setup(s => s.GetItem(_reportHeaderFormField.Id)).Returns(_reportHeaderFormField);
         _snapShotMock.Setup(s => s.GetItems<DynamicPropertiesState>()).Returns(new List<DynamicPropertiesState>() { dinamicPropertiesState });
         _snapShotMock.Setup(s => s.GetItems<ReportModeState>()).Returns(new List<ReportModeState>() { new ReportModeState() });
         _snapShotMock.Setup(s => s.GetItems<RunSelection>()).Returns(new List<RunSelection>() { new RunSelection() });
         //_snapShotMock.Setup(x => x.GetItems<PlansState>()).Returns(new[] { new PlansState() });
         _history.Setup(h => h.CurrentSnapShot).Returns(_snapShotMock.Object);

         _vmHeaderFormMock = new Mock<IVMHeaderForm>();
         _vmHeaderFormMock.Setup(vm => vm.DynamicPropertyItems).Returns(new[] { new VMDynamicPropertyItem() });
      }

      [Test]
      public void VMHeaderFormRow_ShouldInitializeValues()
      {
         //Act
         var vm = new VMHeaderFormRow(_history.Object, _reportHeaderFormRow.Id, 0, _vmHeaderFormMock.Object);

         //Assert
         Assert.AreEqual(1, vm.Fields.Count);
         Assert.AreEqual(0, vm.RowIndex);
         Assert.AreEqual(_vmHeaderFormMock.Object, vm.HeaderForm);
      }

      [Test]
      public void VMHeaderFormRow_ShouldSubscribeToAppStateChanges()
      {
         //Act
         var vm = new VMHeaderFormRow(_history.Object, _reportHeaderFormRow.Id, 0, _vmHeaderFormMock.Object);

         //Assert
         _history.Verify(x => x.Subscribe(It.IsAny<Subscription>()), Times.Exactly(2));
      }

      [Test]
      public void VMHeaderFormRow_DisposeShouldUnsubscribeToAppStateChangesAndDisposeField()
      {
         //Arrange
         var vm = new VMHeaderFormRow(_history.Object, _reportHeaderFormRow.Id, 0, _vmHeaderFormMock.Object);

         //Act
         vm.Dispose();

         //Assert
         _history.Verify(x =>
            x.RemoveClient(
               It.IsAny<ISubscriptionClient>()),
            Times.Exactly(2)); // Unsuscribe header form row
      }

      [Test]
      public void VMHeaderFormRow_ShouldRemoveFieldsWhenFieldsRemoved()
      {
         var newSnapShotMock = new Mock<ISnapShot>();
         newSnapShotMock
            .Setup(x => x.GetChanges())
            .Returns(new Changes(new IItemChange[] { new UpdateItemChange(_reportHeaderFormRow.Id) }.ToImmutableList()));

         //REFACTOR: ReportHeaderFormRow ctor. doesn't has an Id argument and it doesn't have "With..." method.
         //Posible wrong mocking: the new ReportHeaderFormRow returned has a new different Id
         newSnapShotMock
            .Setup(x => x.GetItem(_reportHeaderFormRow.Id))
            .Returns(new ReportHeaderFormRow(Enumerable.Empty<Id<ReportHeaderFormField>>()));

         var vm = new VMHeaderFormRow(_history.Object, _reportHeaderFormRow.Id, 0, _vmHeaderFormMock.Object);

         vm.Update(newSnapShotMock.Object);

         Assert.AreEqual(0, vm.Fields.Count);
         Assert.AreEqual(0, vm.RowIndex);
         Assert.AreEqual(_vmHeaderFormMock.Object, vm.HeaderForm);
         _history.Verify(x =>
            x.RemoveClient(
               It.IsAny<ISubscriptionClient>()),
            Times.Once); // Unsuscribe header form field
      }

      [Test]
      public void VMHeaderFormRow_ShouldAddFieldsWhenFieldsAdded()
      {
         var vm = new VMHeaderFormRow(_history.Object, _reportHeaderFormRow.Id, 0, _vmHeaderFormMock.Object);

         var newReportHeaderFormFieldId = new Id<ReportHeaderFormField>(Guid.NewGuid());
         var newReportHeaderFormField = new ReportHeaderFormField();

         _snapShotMock
            .Setup(x => x.GetChanges())
            .Returns(new Changes(new IItemChange[] { new UpdateItemChange(_reportHeaderFormRow.Id) }.ToImmutableList()));

         //REFACTOR: ReportHeaderFormRow ctor. doesn't has an Id argument and it doesn't have "With..." method.
         //Posible wrong mocking: the new ReportHeaderFormRow returned has a new different Id
         _snapShotMock
            .Setup(x => x.GetItem(_reportHeaderFormRow.Id))
            .Returns(new ReportHeaderFormRow(new[] { _reportHeaderFormField.Id, newReportHeaderFormFieldId, }));

         _snapShotMock
            .Setup(x => x.GetItem(newReportHeaderFormFieldId))
            .Returns(newReportHeaderFormField);

         vm.Update(_snapShotMock.Object);

         Assert.AreEqual(2, vm.Fields.Count);
         Assert.AreEqual(0, vm.RowIndex);
         Assert.AreEqual(_vmHeaderFormMock.Object, vm.HeaderForm);
         _history.Verify(x =>
            x.Subscribe(
               It.Is<Subscription>(s => s.IdFilters.Any(f => f.Id.Equals(newReportHeaderFormFieldId)))),
            Times.Once); // Subscribe header form field
      }
   }
}
