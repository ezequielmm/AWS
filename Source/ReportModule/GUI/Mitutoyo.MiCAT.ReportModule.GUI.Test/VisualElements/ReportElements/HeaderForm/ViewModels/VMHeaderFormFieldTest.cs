// <copyright file="VMHeaderFormFieldTest.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.ReportModule.App.AppState;
using Mitutoyo.MiCAT.ReportModule.Domain.Components;
using Mitutoyo.MiCAT.ReportModule.Domain.Components.ReportSections;
using Mitutoyo.MiCAT.ReportModule.Domain.Data;
using Mitutoyo.MiCAT.ReportModule.GUI.Common.LayoutCalculator;
using Mitutoyo.MiCAT.ReportModule.GUI.PageLayout;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements.HeaderForm.ViewModels;
using Mitutoyo.MiCAT.ReportModuleApp.AppState;
using Mitutoyo.MiCAT.ReportModuleApp.Controllers;
using Moq;
using NUnit.Framework;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Test.VisualElements.ReportElements.HeaderForm.ViewModels
{
   [ExcludeFromCodeCoverage]
   [TestFixture]
   public class VMHeaderFormFieldTest
   {
      private Mock<IHeaderFormFieldController> _headerFormFieldControllerMock;
      private Mock<IAppStateHistory> _history;
      private Mock<ISnapShot> _snapShotMock;
      private Mock<IVMPages> _vmPages;
      private Mock<IRenderedData> _renderedData;
      private Id<ReportHeaderForm> _reportHeaderFormId;
      private Id<ReportHeaderFormRow> _reportHeaderFormRowId;
      private Id<ReportHeaderFormField> _reportHeaderFormFieldId;
      private DynamicPropertyDescriptor _fieldDescriptor;
      private Mock<IVMHeaderForm> _vmHeaderFormMock;
      private VMHeaderFormRow _vmHeaderFormRow;

      [SetUp]
      public void SetUp()
      {
         _fieldDescriptor = new DynamicPropertyDescriptor
         {
            Id = Guid.NewGuid(),
            Name = "Field 1",
         };

         var dinamicPropertiesState = new DynamicPropertiesState(
                                                   new Id<DynamicPropertiesState>(Guid.NewGuid()),
                                                   new[] { _fieldDescriptor, }.ToImmutableList());

         _reportHeaderFormFieldId = new Id<ReportHeaderFormField>(Guid.NewGuid());

         var reportHeaderFormRow = new ReportHeaderFormRow(new[] { _reportHeaderFormFieldId, });
         _reportHeaderFormRowId = reportHeaderFormRow.Id;

         var reportHeaderFormField = new ReportHeaderFormField(_reportHeaderFormFieldId, VMDynamicPropertyItem.EmptyDynamicPropertyItem.Id, null, 0);

         _headerFormFieldControllerMock = new Mock<IHeaderFormFieldController>();

         _snapShotMock = new Mock<ISnapShot>();
         _history = new Mock<IAppStateHistory>();
         _history.Setup(h => h.AddClient(It.IsAny<ISubscriptionClient>(), 0)).Returns(_history.Object);
         _vmPages = new Mock<IVMPages>();
         _renderedData = new Mock<IRenderedData>();
         _renderedData.Setup(r => r.Pages).Returns(_vmPages.Object as VMPages);

         _reportHeaderFormId = new Id<ReportHeaderForm>(Guid.NewGuid());
         var reportHeaderForm = new ReportHeaderForm(new ReportComponentPlacement(new ReportBody().Id, 0, 0, 100, 100), new[] { _reportHeaderFormRowId, });

         var changeList = new List<IItemChange>() { new AddItemChange(_reportHeaderFormId), new AddItemChange(_reportHeaderFormRowId), new AddItemChange(_reportHeaderFormFieldId) };
         _snapShotMock.Setup(h => h.GetChanges()).Returns(new Changes(changeList.ToImmutableList()));
         _snapShotMock.Setup(s => s.GetItem<ReportHeaderForm>(_reportHeaderFormId as IItemId)).Returns(reportHeaderForm);
         _snapShotMock.Setup(s => s.GetItem(_reportHeaderFormRowId)).Returns(reportHeaderFormRow);
         _snapShotMock.Setup(s => s.GetItem(_reportHeaderFormFieldId)).Returns(reportHeaderFormField);
         _snapShotMock.Setup(s => s.GetItems<DynamicPropertiesState>()).Returns(new List<DynamicPropertiesState>() { dinamicPropertiesState });
         _snapShotMock.Setup(s => s.GetItems<ReportModeState>()).Returns(new List<ReportModeState>() { new ReportModeState() });
         _snapShotMock.Setup(s => s.GetItems<RunSelection>()).Returns(new List<RunSelection>() { new RunSelection() });
         _history.Setup(h => h.CurrentSnapShot).Returns(_snapShotMock.Object);

         _vmHeaderFormMock = new Mock<IVMHeaderForm>();
         _vmHeaderFormMock.Setup(vm => vm.DynamicPropertyItems).Returns(new[]
         {
            VMDynamicPropertyItem.EmptyDynamicPropertyItem,
            new VMDynamicPropertyItem()
            {
               Id = _fieldDescriptor.Id,
               Name =_fieldDescriptor.Name,
               EntityType = _fieldDescriptor.EntityType,
               DisplayName = _fieldDescriptor.Name
            }
         });

         _vmHeaderFormRow = new VMHeaderFormRow(_history.Object, reportHeaderFormRow.Id, 0, _vmHeaderFormMock.Object);
      }

      [Test]
      public void VMHeaderFormField_ShouldInitializeValues()
      {
         var vm = new VMHeaderFormField(_history.Object, _headerFormFieldControllerMock.Object, _reportHeaderFormFieldId, 0, _vmHeaderFormRow);

         Assert.AreEqual(VMDynamicPropertyItem.EmptyDynamicPropertyItem.Id, vm.SelectedFieldId);
         Assert.AreEqual(_vmHeaderFormRow.RowIndex, vm.RowIndex);
         Assert.AreEqual(_vmHeaderFormRow, vm.Row);
         Assert.AreEqual(_vmHeaderFormMock.Object, vm.HeaderForm);
      }

      [Test]
      public void VMHeaderFormField_ShouldSubscribeToAppStateChanges()
      {
         _snapShotMock
            .Setup(s => s.GetItem(_reportHeaderFormFieldId))
            .Returns(new ReportHeaderFormField(_reportHeaderFormFieldId, VMDynamicPropertyItem.EmptyDynamicPropertyItem.Id, null, 0));

         _history.Verify(x => x.Subscribe(It.IsAny<Subscription>()), Times.AtLeastOnce);
      }

      [Test]
      public void VMHeaderFormField_DisposeShouldUnsubscribeToAppStateChanges()
      {
         _snapShotMock
            .Setup(s => s.GetItem(_reportHeaderFormFieldId))
            .Returns(new ReportHeaderFormField(_reportHeaderFormFieldId, VMDynamicPropertyItem.EmptyDynamicPropertyItem.Id, null, 0));

         var vm = new VMHeaderFormField(_history.Object, _headerFormFieldControllerMock.Object, _reportHeaderFormFieldId, 0, _vmHeaderFormRow);
         vm.Dispose();

         _history.Verify(x => x.RemoveClient(It.IsAny<ISubscriptionClient>()), Times.Once);
      }

      [Test]
      public void VMHeaderFormField_WithEmptySelection_CanNotBeEditableAndShodNotBeEditable()
      {
         _snapShotMock
            .Setup(s => s.GetItem(_reportHeaderFormFieldId))
            .Returns(new ReportHeaderFormField(_reportHeaderFormFieldId, VMDynamicPropertyItem.EmptyDynamicPropertyItem.Id, null, 0));

         var vm = new VMHeaderFormField(_history.Object, _headerFormFieldControllerMock.Object, _reportHeaderFormFieldId, 0, _vmHeaderFormRow);

         Assert.IsFalse(vm.CanEdit);
         Assert.IsFalse(vm.IsEditable);
         Assert.AreEqual(VMDynamicPropertyItem.EmptyDynamicPropertyItem.DisplayName, vm.FieldLabel);
      }

      [Test]
      public void VMHeaderFormField_WithSelectedItemAndNotCustomLabel_CanBeEditableAndShouldNotBeEditable()
      {
         _snapShotMock
            .Setup(s => s.GetItem(_reportHeaderFormFieldId))
            .Returns(new ReportHeaderFormField(_reportHeaderFormFieldId, _fieldDescriptor.Id, null, 0));

         var vm = new VMHeaderFormField(_history.Object, _headerFormFieldControllerMock.Object, _reportHeaderFormFieldId, 0, _vmHeaderFormRow);

         Assert.IsTrue(vm.CanEdit);
         Assert.IsFalse(vm.IsEditable);
         Assert.AreEqual(_fieldDescriptor.Name, vm.FieldLabel);
      }

      [Test]
      public void VMHeaderFormField_WithSelectedItemAndCustomLabel_CanBeEditableAndShouldBeEditable()
      {
         _snapShotMock
           .Setup(s => s.GetItem(_reportHeaderFormFieldId))
           .Returns(new ReportHeaderFormField(_reportHeaderFormFieldId, _fieldDescriptor.Id, "Custom label", 0));

         var vm = new VMHeaderFormField(_history.Object, _headerFormFieldControllerMock.Object, _reportHeaderFormFieldId, 0, _vmHeaderFormRow);

         Assert.IsTrue(vm.CanEdit);
         Assert.IsTrue(vm.IsEditable);
         Assert.AreNotEqual(_fieldDescriptor.Name, vm.FieldLabel);
      }

      [Test]
      public void VMHeaderFormField_WithSelectedItemAndEmptyCustomLabel_CanBeEditableAndShouldBeEditable()
      {
         _snapShotMock
           .Setup(s => s.GetItem(_reportHeaderFormFieldId))
           .Returns(new ReportHeaderFormField(_reportHeaderFormFieldId, _fieldDescriptor.Id, string.Empty, 0));

         var vm = new VMHeaderFormField(_history.Object, _headerFormFieldControllerMock.Object, _reportHeaderFormFieldId, 0, _vmHeaderFormRow);

         Assert.IsTrue(vm.CanEdit);
         Assert.IsTrue(vm.IsEditable);
         Assert.AreNotEqual(_fieldDescriptor.Name, vm.FieldLabel);
      }

      [Test]
      public void VMHeaderFormField_ClearLabelCommandShouldResetLabel()
      {
         _snapShotMock
           .Setup(s => s.GetItem(_reportHeaderFormFieldId))
           .Returns(new ReportHeaderFormField(_reportHeaderFormFieldId, _fieldDescriptor.Id, "Custom Label", 0));
         _snapShotMock
           .Setup(s => s.GetItems<RunSelection>())
           .Returns(new RunSelection[] { new RunSelection() });

         var vm = new VMHeaderFormField(_history.Object, _headerFormFieldControllerMock.Object, _reportHeaderFormFieldId, 0, _vmHeaderFormRow);

         vm.ClearLabelCommand.Execute(null);

         _headerFormFieldControllerMock
            .Verify(x =>
               x.UpdateSelectedHeaderFormFieldLabel(
                  _reportHeaderFormFieldId,
                  null),
               Times.Once());
      }

      [Test]
      public void VMHeaderFormField_EditLabelShouldSetIsEditableToTrue()
      {
         _snapShotMock
           .Setup(s => s.GetItem(_reportHeaderFormFieldId))
           .Returns(new ReportHeaderFormField(_reportHeaderFormFieldId, _fieldDescriptor.Id, null, 0));
         _snapShotMock
           .Setup(s => s.GetItems<RunSelection>())
           .Returns(new RunSelection[] { new RunSelection() });

         var vm = new VMHeaderFormField(_history.Object, _headerFormFieldControllerMock.Object, _reportHeaderFormFieldId, 0, _vmHeaderFormRow);

         vm.EditLabelCommand.Execute(null);

         Assert.IsTrue(vm.IsEditable);
         Assert.IsTrue(vm.CanEdit);
      }

      [Test]
      public void VMHeaderFormField_UpdateLabelWhenCanNotEditShouldDoNothing()
      {
         _snapShotMock
           .Setup(s => s.GetItem(_reportHeaderFormFieldId))
           .Returns(new ReportHeaderFormField(_reportHeaderFormFieldId, VMDynamicPropertyItem.EmptyDynamicPropertyItem.Id, null, 0));

         var vm = new VMHeaderFormField(_history.Object, _headerFormFieldControllerMock.Object, _reportHeaderFormFieldId, 0, _vmHeaderFormRow);

         vm.UpdateLabelCommand.Execute(null);

         Assert.IsFalse(vm.IsEditable);
         Assert.IsFalse(vm.CanEdit);
      }

      [Test]
      public void VMHeaderFormField_UpdateLabelWithNoEditionShouldResetIsEditable()
      {
         _snapShotMock
           .Setup(s => s.GetItem(_reportHeaderFormFieldId))
           .Returns(new ReportHeaderFormField(_reportHeaderFormFieldId, _fieldDescriptor.Id, null, 0));

         var vm = new VMHeaderFormField(_history.Object, _headerFormFieldControllerMock.Object, _reportHeaderFormFieldId, 0, _vmHeaderFormRow);

         vm.UpdateLabelCommand.Execute(null);

         Assert.IsFalse(vm.IsEditable);
         Assert.IsTrue(vm.CanEdit);
      }

      [Test]
      public void VMHeaderFormField_UpdateLabelWithEditedLabelShouldKeepIsEditable()
      {
         var customLabel = "custom label";
         _snapShotMock
            .Setup(s => s.GetItem(_reportHeaderFormFieldId))
            .Returns(new ReportHeaderFormField(_reportHeaderFormFieldId, _fieldDescriptor.Id, customLabel, 0));

         var vm = new VMHeaderFormField(_history.Object, _headerFormFieldControllerMock.Object, _reportHeaderFormFieldId, 0, _vmHeaderFormRow);

         vm.IsEditable = true;

         vm.UpdateLabelCommand.Execute(customLabel);

         Assert.IsTrue(vm.IsEditable);
         Assert.IsTrue(vm.CanEdit);
      }

      [Test]
      public void VMHeaderFormField_UpdateLabelShouldUpdateLabel()
      {
         _snapShotMock
            .Setup(s => s.GetItem(_reportHeaderFormFieldId))
            .Returns(new ReportHeaderFormField(_reportHeaderFormFieldId, _fieldDescriptor.Id, null, 0));

         var vm = new VMHeaderFormField(_history.Object, _headerFormFieldControllerMock.Object, _reportHeaderFormFieldId, 0, _vmHeaderFormRow);
         vm.IsEditable = true;

         vm.UpdateLabelCommand.Execute("a value");

         _headerFormFieldControllerMock.Verify(x =>
            x.UpdateSelectedHeaderFormFieldLabel(
               It.IsAny<Id<ReportHeaderFormField>>(),
               It.IsAny<string>()),
            Times.Once);
      }

      [Test]
      public void VMHeaderFormField_SelectedFieldChangedShouldUpdateSelectedFieldId()
      {
         _snapShotMock
            .Setup(s => s.GetItem(_reportHeaderFormFieldId))
            .Returns(new ReportHeaderFormField(_reportHeaderFormFieldId, _fieldDescriptor.Id, null, 0));

         _history.Setup(h => h.CurrentSnapShot).Returns(_snapShotMock.Object);

         var vm = new VMHeaderFormField(_history.Object, _headerFormFieldControllerMock.Object, _reportHeaderFormFieldId, 0, _vmHeaderFormRow);
         vm.SelectedFieldId = Guid.NewGuid();

         vm.SelectedFieldChangedCommand.Execute(null);

         _headerFormFieldControllerMock.Verify(x =>
            x.UpdateSelectedHeaderFormField(
               It.IsAny<Id<ReportHeaderFormField>>(),
               It.IsAny<Guid>()),
            Times.Once);
      }

      [Test]
      public void VMHeaderFormField_UpdateLabelWidthPercentageShouldUpdateLabelWidthPercentage()
      {
         _snapShotMock
            .Setup(s => s.GetItem(_reportHeaderFormFieldId))
            .Returns(new ReportHeaderFormField(_reportHeaderFormFieldId, _fieldDescriptor.Id, null, 0));

         var vm = new VMHeaderFormField(_history.Object, _headerFormFieldControllerMock.Object, _reportHeaderFormFieldId, 0, _vmHeaderFormRow);

         vm.UpdateLabelWidthPercentageCommand.Execute(null);

         _headerFormFieldControllerMock.Verify(x =>
            x.UpdateSelectedHeaderFormFieldLabelWidthPercentage(
               It.IsAny<Id<ReportHeaderFormField>>(),
               It.IsAny<double>()),
            Times.Once);
      }

      [Test]
      public void VMHeaderForm_DisposeShouldUnsubscribeToAppStateChanges()
      {
         _snapShotMock
            .Setup(s => s.GetItem(_reportHeaderFormFieldId))
            .Returns(new ReportHeaderFormField(_reportHeaderFormFieldId, _fieldDescriptor.Id, null, 0));

         var vm = new VMHeaderFormField(_history.Object, _headerFormFieldControllerMock.Object, _reportHeaderFormFieldId, 0, _vmHeaderFormRow);
         vm.Dispose();

         _history.Verify(x =>
            x.RemoveClient(
               It.IsAny<ISubscriptionClient>()),
            Times.Once); // Unsuscribe header form
      }
   }
}
