// <copyright file="ReportBoundarySectionUpdateReceiverTest.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.ApplicationState.Clients;
using Mitutoyo.MiCAT.ReportModule.App.AppState;
using Mitutoyo.MiCAT.ReportModule.Domain.Components.ReportSections;
using Mitutoyo.MiCAT.ReportModule.GUI.ReportBoundarySection;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements;
using Mitutoyo.MiCAT.ReportModuleApp.AppState.Extensions;
using Mitutoyo.MiCAT.ReportModuleApp.Controllers;
using Moq;
using NUnit.Framework;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Test.ReportBoundarySection
{
   [ExcludeFromCodeCoverage]
   [TestFixture]
   public class ReportBoundarySectionUpdateReceiverTest
   {
      private VMAppStateTestManager _vmAppStateTestManager;
      private IReportBoundarySectionUpdateReceiver _reportBoundarySectionUpdateReceiver;
      private Mock<IImageController> _imageControllerMock;
      private Mock<ITextBoxController> _textboxControllerMock;
      private Mock<IHeaderFormController> _headerFormControllerMock;
      private Mock<IVMReportBoundarySectionComponentFactory> _viewModelFactoryMock;
      private Mock<ISelectedComponentController> _selectedComponentControllerMock;
      private Mock<ISelectedSectionController> _sectionSelectionControllerMock;

      [SetUp]
      public void Setup()
      {
         _textboxControllerMock = new Mock<ITextBoxController>();
         _headerFormControllerMock = new Mock<IHeaderFormController>();
         _vmAppStateTestManager = new VMAppStateTestManager();
         _viewModelFactoryMock = new Mock<IVMReportBoundarySectionComponentFactory>();
         _selectedComponentControllerMock = new Mock<ISelectedComponentController>();
         _imageControllerMock = new Mock<IImageController>();
         _reportBoundarySectionUpdateReceiver = new ReportBoundarySectionUpdateReceiver(_viewModelFactoryMock.Object, _vmAppStateTestManager.History);
         (_reportBoundarySectionUpdateReceiver as IUpdateClient).Update(_vmAppStateTestManager.CurrentSnapShot);
         _sectionSelectionControllerMock = new Mock<ISelectedSectionController>();
      }

      [Test]
      [TestCase(false, true)]
      [TestCase(true, false)]
      public void ShouldGetReportModeValueProperly(bool oldReportModeValue, bool newReportModeValue)
      {
         // Act
         var reportModeState = _vmAppStateTestManager.CurrentSnapShot.GetItems<ReportModeState>().First();
         _vmAppStateTestManager.UpdateSnapshot(reportModeState.With(oldReportModeValue));
         _vmAppStateTestManager.UpdateSnapshot(reportModeState.With(newReportModeValue));

         // Assert
         Assert.AreEqual(newReportModeValue, _reportBoundarySectionUpdateReceiver.IsInEditMode);
      }

      [Test]
      [TestCase(true, false)]
      [TestCase(false, true)]
      public void ShouldNotifyReportModeValueChangeProperly(bool oldReportModeValue, bool newReportModeValue)
      {
         // Arrange
         var reportModeState = _vmAppStateTestManager.CurrentSnapShot.GetItems<ReportModeState>().First();
         var vmReportHeader = new VMReportBoundarySection(new Id<ReportHeader>(Guid.NewGuid()), 0, 0, _imageControllerMock.Object, _textboxControllerMock.Object, _headerFormControllerMock.Object, _selectedComponentControllerMock.Object, _sectionSelectionControllerMock.Object);
         var vmReportFooter = new VMReportBoundarySection(new Id<ReportHeader>(Guid.NewGuid()), 0, 0, _imageControllerMock.Object, _textboxControllerMock.Object, _headerFormControllerMock.Object, _selectedComponentControllerMock.Object, _sectionSelectionControllerMock.Object);
         var sectionSelection = _vmAppStateTestManager.CurrentSnapShot.GetReportSectionSelection();
         var selectedSections = new List<IItemId>()
         {
            _vmAppStateTestManager.CurrentSnapShot.GetReportHeaderId(),
            _vmAppStateTestManager.CurrentSnapShot.GetReportFooterId(),
         };

         _vmAppStateTestManager.UpdateSnapshot(sectionSelection.WithNewSelectedSectionsList(selectedSections));

         vmReportHeader.SetReportState(oldReportModeValue);
         vmReportFooter.SetReportState(oldReportModeValue);

         _reportBoundarySectionUpdateReceiver.AddHeaderViewModel(vmReportHeader);
         _reportBoundarySectionUpdateReceiver.AddFooterViewModel(vmReportFooter);

         // Act
         _vmAppStateTestManager.UpdateSnapshot(reportModeState.With(newReportModeValue));

         // Assert
         Assert.AreEqual(newReportModeValue, _reportBoundarySectionUpdateReceiver.IsInEditMode);
         Assert.AreEqual(newReportModeValue, vmReportHeader.IsSectionOnEditMode);
         Assert.AreEqual(newReportModeValue, vmReportFooter.IsSectionOnEditMode);
      }

      [Test]
      public void ShouldDisposeComponentsAfterClearHeaderViewModelsList()
      {
         // Arrange
         var vmReportComponent = new Mock<IVMReportComponent>();
         var vmReportHeader = new VMReportBoundarySection(new Id<ReportHeader>(Guid.NewGuid()), 0, 0, _imageControllerMock.Object, _textboxControllerMock.Object, _headerFormControllerMock.Object, _selectedComponentControllerMock.Object, _sectionSelectionControllerMock.Object);
         vmReportHeader.AddViewModel(vmReportComponent.Object);

         _reportBoundarySectionUpdateReceiver.AddHeaderViewModel(vmReportHeader);

         // Act
         _reportBoundarySectionUpdateReceiver.ClearHeaderViewModelsList();

         // Assert
         vmReportComponent.Verify(u => u.Dispose(), Times.Once);
      }

      [Test]
      public void ShouldDisposeComponentsAfterClearFooterViewModelsList()
      {
         // Arrange
         var vmReportComponent = new Mock<IVMReportComponent>();
         var vmReportFooter = new VMReportBoundarySection(new Id<ReportHeader>(Guid.NewGuid()), 0, 0, _imageControllerMock.Object, _textboxControllerMock.Object, _headerFormControllerMock.Object, _selectedComponentControllerMock.Object, _sectionSelectionControllerMock.Object);
         vmReportFooter.AddViewModel(vmReportComponent.Object);

         _reportBoundarySectionUpdateReceiver.AddFooterViewModel(vmReportFooter);

         // Act
         _reportBoundarySectionUpdateReceiver.ClearFooterViewModelsList();

         // Assert
         vmReportComponent.Verify(u => u.Dispose(), Times.Once);
      }
   }
}