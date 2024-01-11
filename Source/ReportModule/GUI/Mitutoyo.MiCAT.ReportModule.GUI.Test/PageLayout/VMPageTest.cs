// <copyright file="VMPageTest.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using System.Drawing.Printing;
using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.ReportModule.Domain;
using Mitutoyo.MiCAT.ReportModule.Domain.Components.ReportSections;
using Mitutoyo.MiCAT.ReportModule.Domain.Data;
using Mitutoyo.MiCAT.ReportModule.GUI.Common;
using Mitutoyo.MiCAT.ReportModule.GUI.PageLayout;
using Mitutoyo.MiCAT.ReportModule.GUI.ReportBoundarySection;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements;
using Mitutoyo.MiCAT.ReportModuleApp.Controllers;
using Moq;
using NUnit.Framework;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Test.PageLayout
{
   [ExcludeFromCodeCoverage]
   [TestFixture]
   public class VMPageTest
   {
      private CommonPageLayout _commonPageLayout;
      private VMPages _vmPages;
      private Mock<IReportBoundarySectionUpdateReceiver> _reportBoundarySectionUpdateReceiverMock;
      private VMReportBoundarySectionFactory _vmReportBoundarySectionFactory;
      private IReportModeProperty _reportModeProperty;
      private Mock<IAppStateHistory> _historyMock;
      private Mock<IImageController> _imageControllerMock;
      private Mock<ITextBoxController> _textboxControllerMock;
      private Mock<IHeaderFormController> _headerFormControllerMock;
      private Mock<ISelectedComponentController> _selectedComponentControllerMock;
      private Mock<ISelectedSectionController> _sectionSelectionControllerMock;

      [SetUp]
      public void Setup()
      {
         _textboxControllerMock = new Mock<ITextBoxController>();
         _headerFormControllerMock = new Mock<IHeaderFormController>();
         _reportBoundarySectionUpdateReceiverMock = new Mock<IReportBoundarySectionUpdateReceiver>();
         _reportBoundarySectionUpdateReceiverMock.Setup(u => u.IsInEditMode).Returns(true);
         _imageControllerMock = new Mock<IImageController>();
         _selectedComponentControllerMock = new Mock<ISelectedComponentController>();
         _sectionSelectionControllerMock = new Mock<ISelectedSectionController>();
         _historyMock = new Mock<IAppStateHistory>();
         _historyMock.Setup(h => h.AddClient(It.IsAny<ISubscriptionClient>(), 0)).Returns(_historyMock.Object);
         _vmReportBoundarySectionFactory = new VMReportBoundarySectionFactory(_historyMock.Object, _reportBoundarySectionUpdateReceiverMock.Object, _imageControllerMock.Object, _textboxControllerMock.Object, _headerFormControllerMock.Object, _selectedComponentControllerMock.Object, _sectionSelectionControllerMock.Object);
         _commonPageLayout = new CommonPageLayout(new PageSizeInfo(PaperKind.A4, 1122, 794), new Margin(MarginKind.Narrow, 0, 48, 0, 48), new HeaderData(5), new FooterData(5));
         _reportModeProperty = new ReportModeProperty(_historyMock.Object);
         _vmPages = new VMPages(_vmReportBoundarySectionFactory, _reportBoundarySectionUpdateReceiverMock.Object, _reportModeProperty);

         var snapShot = new Mock<ISnapShot>();
         var reportHeader = new ReportHeader();
         var reportFooter = new ReportFooter();

         snapShot.Setup(s => s.GetItems<ReportHeader>()).Returns(new ReportHeader[] { reportHeader });
         snapShot.Setup(s => s.GetItems<ReportFooter>()).Returns(new ReportFooter[] { reportFooter });

         _historyMock.Setup(h => h.CurrentSnapShot).Returns(snapShot.Object);
      }

      [Test]
      public void TestVmPageConstructorWithHeaderFooter()
      {
         // Arrange
         var reportHeader = new ReportHeader();
         var reportFooter = new ReportFooter();
         var pageLayoutForTest = new CommonPageLayout(new PageSizeInfo(PaperKind.Custom, 100, 200), new Margin(MarginKind.Narrow, 10, 10, 10, 10), new HeaderData(15), new FooterData(16));
         var vmReportHeader = new VMReportBoundarySection(reportHeader.Id, 15, 1, _imageControllerMock.Object, _textboxControllerMock.Object, _headerFormControllerMock.Object, _selectedComponentControllerMock.Object, _sectionSelectionControllerMock.Object);
         var vmReportFooter = new VMReportBoundarySection(reportFooter.Id, 16, 1, _imageControllerMock.Object, _textboxControllerMock.Object, _headerFormControllerMock.Object, _selectedComponentControllerMock.Object, _sectionSelectionControllerMock.Object);

         // Act
         var vmPageToTest = new VMPage(pageLayoutForTest, _reportModeProperty, vmReportHeader, vmReportFooter);

         // Assert
         Assert.AreEqual(pageLayoutForTest.PageSize.Height, vmPageToTest.Height);
         Assert.AreEqual(pageLayoutForTest.PageSize.Width, vmPageToTest.Width);
         Assert.AreEqual(pageLayoutForTest.CanvasMargin, vmPageToTest.Margins);
         Assert.AreEqual(pageLayoutForTest.GetUpperSpace(), vmPageToTest.ReservedUpperSpace);
         Assert.AreEqual(pageLayoutForTest.GetBottomSpace(), vmPageToTest.ReservedBottomSpace);

         Assert.AreEqual(vmReportHeader, vmPageToTest.Header);
         Assert.AreEqual(vmReportFooter, vmPageToTest.Footer);
         Assert.AreEqual(_reportModeProperty, vmPageToTest.ReportModeProperty);

         Assert.AreEqual(VMPage.SPACE_BETWEEN_PAGES, vmPageToTest.VisualizationPagesSeparation.Top);

         Assert.AreEqual(pageLayoutForTest.CanvasMargin.Top, vmPageToTest.TopMargin.Position);
         Assert.AreEqual(pageLayoutForTest.PageSize.Height - pageLayoutForTest.CanvasMargin.Bottom, vmPageToTest.BottomMargin.Position);

         Assert.AreEqual(vmPageToTest.StartVisualY + vmPageToTest.UsableSpaceHeight() - 1, vmPageToTest.EndVisualY);
         Assert.AreEqual(vmPageToTest.EndVisualY, vmPageToTest.EndDomainYAsVisual);
      }

      [Test]
      public void TestVmPageConstructorWithoutHeaderFooter()
      {
         // Arrange
         var pageLayoutForTest = new CommonPageLayout(new PageSizeInfo(PaperKind.Custom, 100, 200), new Margin(MarginKind.Narrow, 10, 10, 10, 10));

         // Act
         var vmPageToTest = new VMPage(pageLayoutForTest, _reportModeProperty, null, null);

         // Assert
         Assert.AreEqual(pageLayoutForTest.PageSize.Height, vmPageToTest.Height);
         Assert.AreEqual(pageLayoutForTest.PageSize.Width, vmPageToTest.Width);
         Assert.AreEqual(pageLayoutForTest.CanvasMargin, vmPageToTest.Margins);
         Assert.AreEqual(pageLayoutForTest.GetUpperSpace(), vmPageToTest.ReservedUpperSpace);
         Assert.AreEqual(pageLayoutForTest.GetBottomSpace(), vmPageToTest.ReservedBottomSpace);

         Assert.IsNull(vmPageToTest.Header);
         Assert.IsNull(vmPageToTest.Footer);
         Assert.AreEqual(_reportModeProperty, vmPageToTest.ReportModeProperty);

         Assert.AreEqual(VMPage.SPACE_BETWEEN_PAGES, vmPageToTest.VisualizationPagesSeparation.Top);

         Assert.AreEqual(pageLayoutForTest.CanvasMargin.Top, vmPageToTest.TopMargin.Position);
         Assert.AreEqual(pageLayoutForTest.PageSize.Height - pageLayoutForTest.CanvasMargin.Bottom, vmPageToTest.BottomMargin.Position);

         Assert.AreEqual(vmPageToTest.StartVisualY + vmPageToTest.UsableSpaceHeight() - 1, vmPageToTest.EndVisualY);
         Assert.AreEqual(vmPageToTest.EndVisualY, vmPageToTest.EndDomainYAsVisual);
      }

      [Test]
      public void YVisualPositionIsOnThisPage_ShouldReturnTrueForLowLimitYVisualPosition()
      {
         TestYVisualPositionIsOnThisPage(1000, true);
      }

      [Test]
      public void YVisualPositionIsOnThisPage_ShouldReturnTrueForMediumYVisualPosition()
      {
         TestYVisualPositionIsOnThisPage(95, true);
      }

      [Test]
      public void YVisualPositionIsOnThisPage_ShouldReturnTrueForHighLimitYVisualPosition()
      {
         TestYVisualPositionIsOnThisPage(1000, true);
      }

      [Test]
      public void YVisualPositionIsOnThisPage_ShouldReturnFalseForLowYVisualPosition()
      {
         TestYVisualPositionIsOnThisPage(2, false);
      }

      [Test]
      public void YVisualPositionIsOnThisPage_ShouldReturnFalseForHighYVisualPosition()
      {
         TestYVisualPositionIsOnThisPage(1200, false);
      }

      [Test]
      public void ReportElementWouldFitOnThisPageOnItsPosition_ShouldReturnTrue()
      {
         TestReportElementWouldFitOnThisPageOnItsPosition(70, 20, true);
      }

      [Test]
      public void ReportElementWouldFitOnThisPageOnItsPosition_ShouldReturnFalse()
      {
         TestReportElementWouldFitOnThisPageOnItsPosition(1050, 61, false);
      }

      [Test]
      public void ReportElementWouldFitOnThisPage_ShouldReturnTrue()
      {
         TestReportElementWouldFitOnThisPage(20, true);
      }

      [Test]
      public void ReportElementWouldFitOnThisPage_ShouldReturnFalse()
      {
         TestReportElementWouldFitOnThisPage(1200, false);
      }

      [Test]
      public void SpaceLeftOnThisPage_ShouldReturn80()
      {
         TestSpaceLeftOnThisPage(1013, 80);
      }

      [Test]
      public void SpaceLeftOnThisPage_ShouldReturnZeroForLimitVisualY()
      {
         TestSpaceLeftOnThisPage(1122, 0);
      }

      [Test]
      public void SpaceLeftOnThisPage_ShouldReturnZeroForExceededVisualY()
      {
         TestSpaceLeftOnThisPage(1300, 0);
      }

      [Test]
      public void ExcedentSpaceToFitOnThisPageOnItsPosition_ShouldReturnZeroForLowValues()
      {
         TestExcedentSpaceToFitOnThisPageOnItsPosition(1, 2, 0);
      }

      [Test]
      public void ExcedentSpaceToFitOnThisPageOnItsPosition_ShouldReturnZeroForLimitValues()
      {
         TestExcedentSpaceToFitOnThisPageOnItsPosition(30, 70, 0);
      }

      [Test]
      public void ExcedentSpaceToFitOnThisPageOnItsPosition_ShouldReturn0()
      {
         TestExcedentSpaceToFitOnThisPageOnItsPosition(110, 120, 0);
      }

      [Test]
      [TestCase(79, false)]
      [TestCase(80, true)]
      [TestCase(81, true)]
      [TestCase(98, true)]
      [TestCase(75, false)]
      [TestCase(25, false)]
      public void FakeSpaceTest(int visualYToTest, bool expected)
      {
         //Arrange
         var page = _vmPages.NextPage(_commonPageLayout);
         var component = Mock.Of<IVMReportComponent>();
         Mock.Get(component).Setup(c => c.VMPlacement.VisualY).Returns(80);
         Mock.Get(component).Setup(c => c.VMPlacement.DomainY).Returns(50);
         page.SetPageBreakInfoByComponent(component.VMPlacement);

         //Act
         var result = page.IsFakeSpace(visualYToTest);

         //Assert
         Assert.AreEqual(expected, result);
      }

      [Test]
      public void GetVisuaYFromYRelativeToThisPageTest()
      {
         // Arrange
         int yToConvert = 25;
         var pageLayoutForTest = new CommonPageLayout(new PageSizeInfo(PaperKind.Custom, 100, 200), new Margin(MarginKind.Narrow, 10, 10, 10, 10), new HeaderData(15), new FooterData(16));
         var vmPageToTest = new VMPage(pageLayoutForTest, _reportModeProperty, null, null);
         vmPageToTest.StartVisualY = 100;

         // Act
         var visualY = vmPageToTest.GetVisuaYFromYRelativeToThisPage(yToConvert);

         //Assert
         Assert.AreEqual(vmPageToTest.StartVisualY - vmPageToTest.ReservedUpperSpace + yToConvert, visualY);
      }

      private void TestYVisualPositionIsOnThisPage(int yVisualPosition, bool resultExpected)
      {
         //Arrange
         var vmFirstPage = _vmPages.NextPage(_commonPageLayout);

         //Act
         var isOnthePage = vmFirstPage.YVisualPositionIsOnThisPage(yVisualPosition);

         //Assert
         Assert.AreEqual(resultExpected, isOnthePage);
      }

      private void TestReportElementWouldFitOnThisPageOnItsPosition(int visualY, int height, bool resultExpected)
      {
         //Arrange
         var vmFirstPage = _vmPages.NextPage(_commonPageLayout);
         var vmPlacementMock = new Mock<IVMReportComponentPlacement>();
         vmPlacementMock.Setup(vm => vm.VisualY).Returns(visualY);
         vmPlacementMock.Setup(vm => vm.DomainHeight).Returns(height);

         //Act
         var fitOnthePage = vmFirstPage.PlacementWouldFitOnThisPageOnItsPosition(vmPlacementMock.Object);

         //Assert
         Assert.AreEqual(resultExpected, fitOnthePage);
      }

      private void TestReportElementWouldFitOnThisPage(int height, bool resultExpected)
      {
         //Arrange
         var vmFirstPage = _vmPages.NextPage(_commonPageLayout);

         var vmPlacement = new Mock<IVMReportComponentPlacement>();
         vmPlacement.Setup(vm => vm.DomainHeight).Returns(height);

         var reportElementMock = new Mock<IVMReportComponent>();
         reportElementMock.Setup(el => el.VMPlacement).Returns(vmPlacement.Object);

         //Act
         var fitOnthePage = vmFirstPage.ReportElementWouldFitOnThisPage(reportElementMock.Object);

         //Assert
         Assert.AreEqual(resultExpected, fitOnthePage);
      }

      private void TestSpaceLeftOnThisPage(int visualY, int spaceLeftExpected)
      {
         //Arrange
         var vmFirstPage = _vmPages.NextPage(_commonPageLayout);

         //Act
         var spaceLeft = vmFirstPage.SpaceLeftOnThisPage(visualY);

         //Assert
         Assert.AreEqual(spaceLeftExpected, spaceLeft);
      }

      private void TestExcedentSpaceToFitOnThisPageOnItsPosition(int visualY, int height, int excedentSpaceExpected)
      {
         //Arrange
         var vmFirstPage = _vmPages.NextPage(_commonPageLayout);

         var vmPlacement = new Mock<IVMReportComponentPlacement>();
         vmPlacement.Setup(vm => vm.VisualY).Returns(visualY);
         vmPlacement.Setup(vm => vm.DomainHeight).Returns(height);

         //Act
         var excedentSpace = vmFirstPage.ExcedentSpaceToFitOnThisPageOnItsPosition(vmPlacement.Object);

         //Assert
         Assert.AreEqual(excedentSpaceExpected, excedentSpace);
      }
   }
}
