// <copyright file="VMTextBoxTest.cs" company="Mitutoyo Europe GmbH">
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
using Mitutoyo.MiCAT.ReportModule.GUI.Common.BusyIndicators;
using Mitutoyo.MiCAT.ReportModule.GUI.Test.VisualElements.ReportElements.TextBox.Views;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements.TextBox.ViewModels;
using Mitutoyo.MiCAT.ReportModuleApp.Controllers;
using Moq;
using NUnit.Framework;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Test.VisualElements.ReportElements.TextBox.ViewModels
{
   [ExcludeFromCodeCoverage]
   [TestFixture]
   public class VMTextBoxTest
   {
      private Mock<ITextBoxController> _textBoxControllerMock;
      private Mock<IAppStateHistory> _history;
      private Mock<ISnapShot> _snapShot;
      private ReportTextBox _reportComponent;
      private Mock<IBusyIndicator> _busyIndicatorMock;
      private Mock<IDeleteComponentController> _deleteComponentControllerMock;

      [SetUp]
      public void SetUp()
      {
         _textBoxControllerMock = new Mock<ITextBoxController>();
         _snapShot = new Mock<ISnapShot>();
         _history = new Mock<IAppStateHistory>();
         _history.Setup(h => h.AddClient(It.IsAny<ISubscriptionClient>(), 0)).Returns(_history.Object);

         _busyIndicatorMock = new Mock<IBusyIndicator>();
         _deleteComponentControllerMock = new Mock<IDeleteComponentController>();

         _busyIndicatorMock.Setup(b => b.SetIsBusyTrueUntilUIIsIdle()).Callback(() => { });

         _reportComponent = new ReportTextBox(new ReportComponentPlacement(new ReportBody().Id, 0, 0, 10, 10), XAMLConverter.ToXAML("TextContent"));
         var changeList = new List<AddItemChange>() { new AddItemChange(_reportComponent.Id, _reportComponent) };
         _snapShot.Setup(h => h.GetChanges()).Returns(new Changes(changeList.ToImmutableList<IItemChange>()));
         _snapShot.Setup(h => h.GetItem(_reportComponent.Id as IItemId<IReportComponent>)).Returns(_reportComponent);
         _snapShot.Setup(s => s.GetItems<ReportModeState>()).Returns(new List<ReportModeState>() { new ReportModeState() });
         _history.Setup(h => h.CurrentSnapShot).Returns(_snapShot.Object);
      }

      [Test]
      public void VMTextBox_ShouldStartEditing()
      {
         // Arrange
         var vmPlacement = Mock.Of<IVMReportComponentPlacement>();

         var sut = new VMTextBox(
            _history.Object,
            _reportComponent.Id,
            vmPlacement,
            _deleteComponentControllerMock.Object,
            _textBoxControllerMock.Object);

         //Act
         sut.StartEditCommand.Execute(null);

         // Assert
         Assert.IsTrue(sut.IsEditing);
      }

      [Test]
      public void VMTextBox_ShouldEndEditing()
      {
         // Arrange
         var vmPlacement = Mock.Of<IVMReportComponentPlacement>();

         var sut = new VMTextBox(
            _history.Object,
            _reportComponent.Id,
            vmPlacement,
            _deleteComponentControllerMock.Object,
            _textBoxControllerMock.Object);

         sut.IsEditing = true;

         //Act
         sut.EndEditCommand.Execute(null);

         // Assert
         Assert.IsFalse(sut.IsEditing);
      }

      [Test]
      public void VMTextBox_ShouldSaveNewText()
      {
         // Arrange
         var newtext = XAMLConverter.ToXAML("New Text");
         var vmPlacement = Mock.Of<IVMReportComponentPlacement>();

         var sut = new VMTextBox(
            _history.Object,
            _reportComponent.Id,
            vmPlacement,
            _deleteComponentControllerMock.Object,
            _textBoxControllerMock.Object);

         sut.InputText = newtext;

         //Act
         sut.EndEditCommand.Execute(null);

         // Assert
         _textBoxControllerMock.Verify(c => c.ModifyText(It.Is<Id<ReportTextBox>>(id => id.Equals(sut.Id)), It.Is<string>(t => t == newtext)), Times.Once);
      }

      [Test]
      public void VMTextBox_ShouldToBeEmpty()
      {
         // Arrange
         var newtext = string.Empty;
         var vmPlacement = Mock.Of<IVMReportComponentPlacement>();

         var sut = new VMTextBox(
            _history.Object,
            _reportComponent.Id,
            vmPlacement,
            _deleteComponentControllerMock.Object,
            _textBoxControllerMock.Object);

         sut.InputText = newtext;

         //Act
         sut.EndEditCommand.Execute(null);

         // Assert
         Assert.IsTrue(sut.IsEmpty);
      }
   }
}
