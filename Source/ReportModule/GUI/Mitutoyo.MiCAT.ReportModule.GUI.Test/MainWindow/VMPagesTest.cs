// <copyright file="VMPagesTest.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Drawing.Printing;
using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.ReportModule.Domain;
using Mitutoyo.MiCAT.ReportModule.Domain.Components.ReportSections;
using Mitutoyo.MiCAT.ReportModule.Domain.Data;
using Mitutoyo.MiCAT.ReportModule.GUI.Common;
using Mitutoyo.MiCAT.ReportModule.GUI.Common.DisabledSpace;
using Mitutoyo.MiCAT.ReportModule.GUI.PageLayout;
using Mitutoyo.MiCAT.ReportModule.GUI.ReportBoundarySection;
using Mitutoyo.MiCAT.ReportModuleApp.Controllers;
using Moq;
using NUnit.Framework;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Test.MainWindow
{
   [ExcludeFromCodeCoverage]
   [TestFixture]
   public class VMPagesTest
   {
      private Mock<IReportBoundarySectionUpdateReceiver> _reportBoundarySectionUpdateReceiverMock;
      private VMReportBoundarySectionFactory _vmReportBoundarySectionFactory;
      private IReportModeProperty _reportModeProperty;
      private Mock<IAppStateHistory> _historyMock;
      private Mock<IImageController> _imageControllerMock;
      private Mock<ITextBoxController> _textboxControllerMock;
      private Mock<IHeaderFormController> _headerFormControllerMock;
      private Mock<ISelectedComponentController> _selectedComponentControllerMock;
      private Mock<ISelectedSectionController> _sectionSelectionControllerMock;
      private Mock<ISnapShot> _snapShotMock;

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
         _snapShotMock = new Mock<ISnapShot>();

         _snapShotMock.Setup(s => s.GetItems<ReportHeader>()).Returns(new List<ReportHeader>() { new ReportHeader() });
         _snapShotMock.Setup(s => s.GetItems<ReportFooter>()).Returns(new List<ReportFooter>() { new ReportFooter() });

         _historyMock.Setup(a => a.AddClient(It.IsAny<object>(), It.IsAny<int>())).Returns(_historyMock.Object);

         _reportModeProperty = new ReportModeProperty(_historyMock.Object);

         _vmReportBoundarySectionFactory = new VMReportBoundarySectionFactory(_historyMock.Object, _reportBoundarySectionUpdateReceiverMock.Object, _imageControllerMock.Object, _textboxControllerMock.Object, _headerFormControllerMock.Object, _selectedComponentControllerMock.Object, _sectionSelectionControllerMock.Object);

         _historyMock.Setup(h => h.CurrentSnapShot).Returns(_snapShotMock.Object);

         _historyMock.Setup(h => h.AddClient(It.IsAny<ISubscriptionClient>(), 0)).Returns(_historyMock.Object);
      }
      [Test]
      public void AddPage_ShouldAddTwoPages()
      {
         //Arrange
         var commonPageLayout = new CommonPageLayout();
         var vm = new VMPages(_vmReportBoundarySectionFactory, _reportBoundarySectionUpdateReceiverMock.Object, _reportModeProperty);

         //Act
         var firstPage = vm.NextPage(commonPageLayout);
         var secondPage = vm.NextPage(commonPageLayout);

         //Assert
         Assert.AreEqual(vm.Pages.Count, 2);
         Assert.AreEqual(firstPage, vm.Pages[0]);
         Assert.AreEqual(secondPage, vm.Pages[1]);
      }

      [Test]
      public void AddPage_ShouldSetTotalPagesHeight()
      {
         //Arrange
         var commonPageLayout = new CommonPageLayout(new PageSizeInfo(PaperKind.Custom, 30, 30), new Margin(MarginKind.Narrow, 0, 48, 0, 48), new HeaderData(100), new FooterData(100));
         var vm = new VMPages(_vmReportBoundarySectionFactory, _reportBoundarySectionUpdateReceiverMock.Object, _reportModeProperty);

         //Act
         vm.NextPage(commonPageLayout);
         vm.NextPage(commonPageLayout);
         vm.NextPage(commonPageLayout);

         //Assert
         Assert.AreEqual(vm.TotalPagesHeight, 180);
      }

      [Test]
      public void CreateFirstPage_ShouldSetNewPagePosition()
      {
         //Arrange
         var commonPageLayout = new CommonPageLayout(new PageSizeInfo(PaperKind.A4, 1122, 794), new Margin(MarginKind.Narrow, 0, 48, 0, 48), new HeaderData(0), new FooterData(0));
         var vm = new VMPages(_vmReportBoundarySectionFactory, _reportBoundarySectionUpdateReceiverMock.Object, _reportModeProperty);

         //Act
         var vmFirstPage = vm.NextPage(commonPageLayout);

         //Assert
         Assert.AreEqual(vmFirstPage.StartVisualY, 68);
         Assert.AreEqual(vmFirstPage.EndVisualY, 1093);
         Assert.AreEqual(vmFirstPage.OffsetForComponentsOnThisPage, 68);
         Assert.AreEqual(vmFirstPage.StartDomainY, 0);
         Assert.AreEqual(vmFirstPage.EndDomainY, 1025);
      }

      [Test]
      public void CreatePage_ShouldSetNewPagePosition()
      {
         //Arrange
         var commonPageLayout = new CommonPageLayout(new PageSizeInfo(PaperKind.A4, 1122, 794), new Margin(MarginKind.Narrow, 0, 48, 0, 48), new HeaderData(5), new FooterData(5));
         var vm = new VMPages(_vmReportBoundarySectionFactory, _reportBoundarySectionUpdateReceiverMock.Object, _reportModeProperty);

         //Act
         var vmFirstPage = vm.NextPage(commonPageLayout);
         var newPage = vm.NextPage(vmFirstPage, commonPageLayout);

         //Assert
         Assert.AreEqual(newPage.StartVisualY, 1210);
         Assert.AreEqual(newPage.EndVisualY, 2235);
         Assert.AreEqual(newPage.OffsetForComponentsOnThisPage, 184);
         Assert.AreEqual(newPage.StartDomainY, 1026);
         Assert.AreEqual(newPage.EndDomainY, 2051);
      }

      [Test]
      public void CreatePage_ShouldAddNewPage()
      {
         //Arrange
         var commonPageLayout = new CommonPageLayout();
         var vm = new VMPages(_vmReportBoundarySectionFactory, _reportBoundarySectionUpdateReceiverMock.Object, _reportModeProperty);

         //Act
         var vmFirstPage = vm.NextPage(commonPageLayout);
         var newPage = vm.NextPage(commonPageLayout);

         //Assert
         Assert.AreEqual(vm.Pages.Count, 2);
         Assert.AreEqual(vm.Pages[0], vmFirstPage);
         Assert.AreEqual(vm.Pages[1], newPage);
      }

      [Test]
      public void GetDomainOutsidePages_UpperThanFirstPage()
      {
         //Arrange
         var commonPageLayout = new CommonPageLayout(new PageSizeInfo(PaperKind.Custom, 30, 30), new Margin(MarginKind.Narrow, 0, 48, 0, 48), new HeaderData(100), new FooterData(100));
         var vm = new VMPages(_vmReportBoundarySectionFactory, _reportBoundarySectionUpdateReceiverMock.Object, _reportModeProperty);
         vm.NextPage(commonPageLayout);
         vm.NextPage(commonPageLayout);
         vm.NextPage(commonPageLayout);

         //Act
         var visualY = vm.GetDomainOutsidePages(1);

         //Assert
         Assert.AreEqual(0, visualY);
      }

      [Test]
      public void GetDomainOutsidePages_BetweenPages()
      {
         //Arrange
         var commonPageLayout = new CommonPageLayout(new PageSizeInfo(PaperKind.Custom, 500, 200), new Margin(MarginKind.Narrow, 0, 50, 0, 50));
         var vm = new VMPages(_vmReportBoundarySectionFactory, _reportBoundarySectionUpdateReceiverMock.Object, _reportModeProperty);
         vm.NextPage(commonPageLayout);
         var page2 = vm.NextPage(commonPageLayout);
         vm.NextPage(commonPageLayout);

         //Act
         var visualY = vm.GetDomainOutsidePages(490);

         //Assert
         Assert.AreEqual(page2.StartDomainY, visualY);
      }

      [Test]
      public void GetDomainOutsidePages_AfterLastPage()
      {
         //Arrange
         var commonPageLayout = new CommonPageLayout(new PageSizeInfo(PaperKind.Custom, 500, 200), new Margin(MarginKind.Narrow, 0, 50, 0, 50));
         var vm = new VMPages(_vmReportBoundarySectionFactory, _reportBoundarySectionUpdateReceiverMock.Object, _reportModeProperty);
         vm.NextPage(commonPageLayout);
         vm.NextPage(commonPageLayout);
         var lastPage = vm.NextPage(commonPageLayout);

         //Act
         var visualY = vm.GetDomainOutsidePages(1500);

         //Assert
         Assert.AreEqual(lastPage.EndDomainY + 1, visualY);
      }

      [Test]
      public void GetDomainOutsidePages_ForEmptyPageList()
      {
         //Arrange
         var vm = new VMPages(_vmReportBoundarySectionFactory, _reportBoundarySectionUpdateReceiverMock.Object, _reportModeProperty);

         //Act
         var visualY = vm.GetDomainOutsidePages(1500);

         //Assert
         Assert.AreEqual(1500, visualY);
      }

      [Test]
      public void TestResetDomainInfoAffectedByDisabledSpaceMultiPage()
      {
         //Arrange
         var commonPageLayout = new CommonPageLayout(new PageSizeInfo(PaperKind.A4, 1122, 794), new Margin(MarginKind.Narrow, 0, 48, 0, 48));
         var vmPages = new VMPages(_vmReportBoundarySectionFactory, _reportBoundarySectionUpdateReceiverMock.Object, _reportModeProperty);
         var page1 = vmPages.NextPage(commonPageLayout);
         var page2 = vmPages.NextPage(commonPageLayout);
         var page3 = vmPages.NextPage(commonPageLayout);
         var page4 = vmPages.NextPage(commonPageLayout);
         var page5 = vmPages.NextPage(commonPageLayout);
         var usableSpaceHeightByPage = page1.UsableSpaceHeight();
         var usedSpaceByDisabledSpace = usableSpaceHeightByPage - 200 + usableSpaceHeightByPage + 50;
         var disabledSpace = new DisabledSpaceData(page2.StartDomainY + 200, page2, page2.StartVisualY + 200, page4, page4.StartVisualY + 50, usedSpaceByDisabledSpace);

         //Act
         vmPages.ResetDomainInfoAffectedByDisabledSpace(disabledSpace);

         //Assert
         Assert.AreEqual(0, page1.StartDomainY);
         Assert.AreEqual(1025, page1.EndDomainY);
         Assert.AreEqual(1026, page2.StartDomainY);
         Assert.AreEqual(disabledSpace.StartDomainY, page2.EndDomainY);
         Assert.AreEqual(disabledSpace.StartDomainY, page3.StartDomainY);
         Assert.AreEqual(disabledSpace.StartDomainY, page3.EndDomainY);
         Assert.AreEqual(disabledSpace.StartDomainY, page4.StartDomainY);
         Assert.AreEqual(2201, page4.EndDomainY);
         Assert.AreEqual(2202, page5.StartDomainY);
         Assert.AreEqual(3227, page5.EndDomainY);
      }

      [Test]
      public void TestResetDomainInfoAffectedByDisabledSpaceSinglePage()
      {
         //Arrange
         var commonPageLayout = new CommonPageLayout(new PageSizeInfo(PaperKind.A4, 1122, 794), new Margin(MarginKind.Narrow, 0, 48, 0, 48));
         var vmPages = new VMPages(_vmReportBoundarySectionFactory, _reportBoundarySectionUpdateReceiverMock.Object, _reportModeProperty);
         var page = vmPages.NextPage(commonPageLayout);
         var disabledSpace = new DisabledSpaceData(page.StartDomainY + 200, page, page.StartVisualY + 200, page, page.StartVisualY + 250, 50);

         //Act
         vmPages.ResetDomainInfoAffectedByDisabledSpace(disabledSpace);

         //Assert
         Assert.AreEqual(0, page.StartDomainY);
         Assert.AreEqual(975, page.EndDomainY);
      }
   }
}
