// <copyright file="VMSelectTemplateForCreateTest.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Windows.Input;
using Mitutoyo.MiCAT.ReportModule.Domain.Data;
using Mitutoyo.MiCAT.ReportModule.GUI.Dialog;
using Mitutoyo.MiCAT.ReportModule.GUI.Dialog.Confirmation;
using Mitutoyo.MiCAT.ReportModule.GUI.SelectTemplates;
using Mitutoyo.MiCAT.ReportModuleApp.Controllers;
using Moq;
using NUnit.Framework;
using Telerik.Windows.Controls;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Test.SelectTemplates
{
   [ExcludeFromCodeCoverage]
   [TestFixture]
   public class VMSelectTemplateForCreateTest
   {
      private Mock<IReportTemplateController> _reportTemplateController;
      private VMSelectTemplateForCreate _vmSelectTemplate;
      private Mock<IDialogService> _dialogService;
      private ICommand SelectionDeleteCommand { get; set; }
      private Guid _reportTemplateId;
      private VMConfirmationDialog _vmConfirmationDialog;

      [SetUp]
      public void SetUp()
      {
         _reportTemplateId = Guid.NewGuid();
         _reportTemplateController = new Mock<IReportTemplateController>();
         _dialogService = new Mock<IDialogService>();
         _vmConfirmationDialog = new VMConfirmationDialog();

         _vmSelectTemplate = new VMSelectTemplateForCreate(_reportTemplateController.Object);
         _reportTemplateController.Setup(rt => rt.GetReportTemplateDescriptorsForCreate())
            .ReturnsAsync(new List<ReportTemplateDescriptor>());
      }
      [Test]
      [Apartment(ApartmentState.STA)]
      public void VMSelectTemplateForCreateTest_ShouldNotDeleteReportTemplateIfTemplateIsADefaultBlankTemplate()
      {
         //Arrange
         _reportTemplateController.Setup(rc => rc.DeleteReportTemplate(_reportTemplateId)).ReturnsAsync(false);
         _vmSelectTemplate.SelectedTemplate = new ReportTemplateDescriptor()
         {
            Id = _reportTemplateId,
            LocalizedName = false,
            Name = string.Empty,
            ReadOnly = true
         };
         // Act
         _vmSelectTemplate.DeleteSpecificTemplateCommand.Execute(_vmSelectTemplate.SelectedTemplate);
         // Assert
         _reportTemplateController.Verify(rt => rt.DeleteReportTemplate(_reportTemplateId), Times.Never);
      }

      [Test]
      [Apartment(ApartmentState.STA)]
      public void VMSelectTemplateForCreateTest_ShouldNotDeleteSelectedReportTemplateIfTemplateIsADefaultBlankTemplate()
      {
         //Arrange
         _reportTemplateController.Setup(rc => rc.DeleteReportTemplate(_reportTemplateId)).ReturnsAsync(false);
         _vmSelectTemplate.SelectedTemplate = new ReportTemplateDescriptor()
         {
            Id = _reportTemplateId,
            LocalizedName = false,
            Name = string.Empty,
            ReadOnly = true
         };
         // Act
         _vmSelectTemplate.DeleteSelectedTemplateCommand.Execute(null);
         // Assert
         _reportTemplateController.Verify(rt => rt.DeleteReportTemplate(_reportTemplateId), Times.Never);
      }

      [Test]
      [Apartment(ApartmentState.STA)]
      public void VMSelectTemplateForCreateTest_ShouldCallDeleteReportTemplateAndGetAllReportTemplateList()
      {
         //Arrange
         _reportTemplateController.Setup(rc => rc.DeleteReportTemplate(_reportTemplateId)).ReturnsAsync(true);
         _vmSelectTemplate.SelectedTemplate = new ReportTemplateDescriptor()
         {
            Id = _reportTemplateId,
            LocalizedName = false,
            Name = string.Empty,
            ReadOnly = false
         };
         // Act
         _vmSelectTemplate.DeleteSpecificTemplateCommand.Execute(_vmSelectTemplate.SelectedTemplate);
         // Assert
         _reportTemplateController.Verify(rt => rt.DeleteReportTemplate(_reportTemplateId), Times.Once);
         _reportTemplateController.Verify(rt => rt.GetReportTemplateDescriptorsForCreate(), Times.Once);
      }

      [Test]
      [Apartment(ApartmentState.STA)]
      public void VMSelectTemplateForCreateTest_ShouldCallDeleteSelectedReportTemplateAndGetAllReportTemplateList()
      {
         //Arrange
         _reportTemplateController.Setup(rc => rc.DeleteReportTemplate(_reportTemplateId)).ReturnsAsync(true);
         _vmSelectTemplate.SelectedTemplate = new ReportTemplateDescriptor()
         {
            Id = _reportTemplateId,
            LocalizedName = false,
            Name = string.Empty,
            ReadOnly = false
         };
         // Act
         _vmSelectTemplate.DeleteSelectedTemplateCommand.Execute(null);
         // Assert
         _reportTemplateController.Verify(rt => rt.DeleteReportTemplate(_reportTemplateId), Times.Once);
         _reportTemplateController.Verify(rt => rt.GetReportTemplateDescriptorsForCreate(), Times.Once);
      }

      [Test]
      [Apartment(ApartmentState.STA)]
      public void OnSelectChangedShouldSelectReportTemplateDescriptor()
      {
         var template = new ReportTemplateDescriptor()
         {
            Id = _reportTemplateId,
            LocalizedName = false,
            Name = string.Empty,
            ReadOnly = false
         };
         var rad = new RadTileView() { Items = { template }, SelectedItem = template };
         //Act
         _vmSelectTemplate.SelectionChangedCommand.Execute(rad);
         //Assertion
         Assert.That(_vmSelectTemplate.SelectedTemplate.Id == template.Id);
      }
      [Test]
      [Apartment(ApartmentState.STA)]
      public void IsOneSelectedShouldBeTrueAfterSelectedReportTemplateDescriptor()
      {
         var template = new ReportTemplateDescriptor()
         {
            Id = _reportTemplateId,
            LocalizedName = false,
            Name = string.Empty,
            ReadOnly = false
         };
         var rad = new RadTileView() { Items = { template }, SelectedItem = template };
         //Act
         _vmSelectTemplate.SelectionChangedCommand.Execute(rad);
         //Assertion
         Assert.IsTrue(_vmSelectTemplate.IsOneSelected);
      }
   }
}
