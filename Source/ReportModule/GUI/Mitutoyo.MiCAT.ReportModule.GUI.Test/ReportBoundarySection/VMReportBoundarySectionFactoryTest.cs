// <copyright file="VMReportBoundarySectionFactoryTest.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.ReportModule.Domain.Components.ReportSections;
using Mitutoyo.MiCAT.ReportModule.GUI.ReportBoundarySection;
using Mitutoyo.MiCAT.ReportModuleApp.Controllers;
using Moq;
using NUnit.Framework;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Test.ReportBoundarySection
{
   [ExcludeFromCodeCoverage]
   [TestFixture]
   public class VMReportBoundarySectionFactoryTest
   {
      [Test]
      public void ShouldGetFooterViewModelProperly()
      {
         // Arrange
         var sutInfo = ArrangeSut();

         // Act
         var footerViewModel = sutInfo.Sut.CreateForFooter(50, 5);

         // Assert
         AssertSectionViewModel(footerViewModel, 50, 5);
         sutInfo.UpdateReceiverMock.Verify(u => u.AddFooterViewModel(It.Is<VMReportBoundarySection>(vm => vm == footerViewModel)), Times.Once);
      }

      [Test]
      public void ShouldGetHeaderViewModelProperly()
      {
         // Arrange
         var sutInfo = ArrangeSut();

         // Act
         var headerViewModel = sutInfo.Sut.CreateForHeader(50, 5);

         // Assert
         AssertSectionViewModel(headerViewModel, 50, 5);
         sutInfo.UpdateReceiverMock.Verify(u => u.AddHeaderViewModel(It.Is<VMReportBoundarySection>(vm => vm == headerViewModel)), Times.Once);
      }

      private (VMReportBoundarySectionFactory Sut, Mock<IReportBoundarySectionUpdateReceiver> UpdateReceiverMock) ArrangeSut()
      {
         var reportHeader = new ReportHeader();
         var reportFooter = new ReportFooter();
         var updateReceiverMock = new Mock<IReportBoundarySectionUpdateReceiver>();
         var reportBoundarySectionImageControllerMock = new Mock<IImageController>();
         var reportBoundarySectionTextboxControllerMock = new Mock<ITextBoxController>();
         var reportBoundarySectionHeaderFormControllerMock = new Mock<IHeaderFormController>();
         var selectedComponentControllerMock = new Mock<ISelectedComponentController>();
         var sectionSelectionControllerMock = new Mock<ISelectedSectionController>();
         var historyMock = new Mock<IAppStateHistory>();
         var snapShot = new Mock<ISnapShot>();

         snapShot.Setup(s => s.GetItems<ReportHeader>()).Returns(new ReportHeader[] { reportHeader });
         snapShot.Setup(s => s.GetItems<ReportFooter>()).Returns(new ReportFooter[] { reportFooter });

         historyMock.Setup(h => h.CurrentSnapShot).Returns(snapShot.Object);

         var sut = new VMReportBoundarySectionFactory(historyMock.Object, updateReceiverMock.Object, reportBoundarySectionImageControllerMock.Object, reportBoundarySectionTextboxControllerMock.Object, reportBoundarySectionHeaderFormControllerMock.Object, selectedComponentControllerMock.Object, sectionSelectionControllerMock.Object);
         return (sut, updateReceiverMock);
      }

      private void AssertSectionViewModel(VMReportBoundarySection vmSection, int height, int pageNumber)
      {
         Assert.IsNotNull(vmSection);
         Assert.AreEqual(height, vmSection.Height);
         Assert.AreEqual(pageNumber, vmSection.PageNumber);
      }
   }
}
