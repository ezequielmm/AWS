// <copyright file="VMTextBoxAppStateTest.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

namespace Mitutoyo.MiCAT.ReportModule.GUI.Test.VisualElements.ReportElements.TextBox.ViewModels
{
   using System.Diagnostics.CodeAnalysis;
   using System.Linq;
   using Mitutoyo.MiCAT.ReportModule.App.AppState;
   using Mitutoyo.MiCAT.ReportModule.Domain.Components;
   using Mitutoyo.MiCAT.ReportModule.Domain.Components.ReportSections;
   using Mitutoyo.MiCAT.ReportModule.GUI.Test.VisualElements.ReportElements.TextBox.Views;
   using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements;
   using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements.TextBox.ViewModels;
   using Mitutoyo.MiCAT.ReportModuleApp.Controllers;
   using Moq;
   using NUnit.Framework;

   [TestFixture]
   [ExcludeFromCodeCoverage]
   public class VMTextBoxAppStateTest
   {
      private VMTextBox _viewModel;
      private VMAppStateTestManager _vmAppStateTestManager;
      private Mock<ITextBoxController> textBoxControllerMock;
      private Mock<IDeleteComponentController> _deleteComponentControllerMock;

      [SetUp]
      public void Setup()
      {
         textBoxControllerMock = new Mock<ITextBoxController>();
         _deleteComponentControllerMock = new Mock<IDeleteComponentController>();

         _vmAppStateTestManager = new VMAppStateTestManager();
      }

      [TearDown]
      public void TearDown()
      {
         _viewModel?.Dispose();
      }

      [Test]
      public void VMTextBox_ShouldInitializeAndUpdateInputText()
      {
         //Arrange
         string previousContent = XAMLConverter.ToXAML("TextBoxContent");
         string newContent = XAMLConverter.ToXAML("NewTextBoxContent");
         var reportBody = new ReportBody();
         var placement = new ReportComponentPlacement(reportBody.Id, 0, 0, 10, 100);
         var reportTextbox = new ReportTextBox(placement, previousContent);
         var vmPlacement = Mock.Of<IVMReportComponentPlacement>();

         _vmAppStateTestManager.UpdateSnapshot(reportBody);
         _vmAppStateTestManager.UpdateSnapshot(reportTextbox);

         _viewModel = new VMTextBox(
            _vmAppStateTestManager.History,
            reportTextbox.Id,
            vmPlacement,
            _deleteComponentControllerMock.Object,
            textBoxControllerMock.Object);

         // Assert
         Assert.AreEqual(previousContent, _viewModel.InputText);

         //Act
         _vmAppStateTestManager.UpdateSnapshot(reportTextbox.WithText(newContent) as ReportTextBox);

         //Assert
         Assert.AreEqual(newContent, _viewModel.InputText);
      }

      [Test]
      public void VMTextBox_ShouldUpdateWatermarkVisibility()
      {
         //Arrange
         var vmPlacement = Mock.Of<IVMReportComponentPlacement>();
         var reportBody = new ReportBody();
         var placement = new ReportComponentPlacement(reportBody.Id, 0, 0, 10, 100);
         var reportTextbox = new ReportTextBox(placement);

         _vmAppStateTestManager.UpdateSnapshot(reportBody);
         _vmAppStateTestManager.UpdateSnapshot(reportTextbox);

         _viewModel = new VMTextBox(
            _vmAppStateTestManager.History,
            reportTextbox.Id,
            vmPlacement,
            _deleteComponentControllerMock.Object,
            textBoxControllerMock.Object);

         var reportModeState = _vmAppStateTestManager.CurrentSnapShot.GetItems<ReportModeState>().Single();
         _viewModel.ShowWatermark = true;
         //Act
         _vmAppStateTestManager.UpdateSnapshot(reportModeState.With(false));
         //Assert
         Assert.AreEqual(false, _viewModel.ShowWatermark);
      }
   }
}
