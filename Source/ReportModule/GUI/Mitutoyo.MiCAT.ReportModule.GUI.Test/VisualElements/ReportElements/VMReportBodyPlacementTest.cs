// <copyright file="VMReportBodyPlacementTest.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Drawing.Printing;
using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.ReportModule.App.AppState;
using Mitutoyo.MiCAT.ReportModule.Domain.Components;
using Mitutoyo.MiCAT.ReportModule.Domain.Components.ReportSections;
using Mitutoyo.MiCAT.ReportModule.Domain.Data;
using Mitutoyo.MiCAT.ReportModule.GUI.Common;
using Mitutoyo.MiCAT.ReportModule.GUI.Common.BusyIndicators;
using Mitutoyo.MiCAT.ReportModule.GUI.Common.LayoutCalculator;
using Mitutoyo.MiCAT.ReportModule.GUI.PageLayout;
using Mitutoyo.MiCAT.ReportModule.GUI.ReportBoundarySection;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements;
using Mitutoyo.MiCAT.ReportModuleApp.Controllers;
using Moq;
using NUnit.Framework;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Test.VisualElements.ReportElements
{
   [ExcludeFromCodeCoverage]
   [TestFixture]
   public class VMReportBodyPlacementTest
   {
      private Mock<IAppStateHistory> _historyMock;
      private Mock<ISnapShot> _snapShotMock;
      private Mock<IReportComponentPlacementController> _placementControllerMock;
      private Mock<ISelectedComponentController> _selectedComponentControllerMock;
      private Mock<ISelectedSectionController> _sectionSelectionControllerMock;
      private Mock<IImageController> _imageControllerMock;
      private Mock<ITextBoxController> _textboxControllerMock;
      private Mock<IHeaderFormController> _headerFormController;
      private Mock<IVMPages> _vmPages;
      private ReportComponentPlacement _placementEntity;
      private ReportBody _bodySection;
      private ReportFooter _footerSection;
      private ReportHeader _headerSection;
      private int _domainX;
      private int _domainY;
      private Mock<IRenderedData> _renderData;
      private Mock<IBusyIndicator> _busyIndicatorMock;
      private Mock<IReportBoundarySectionUpdateReceiver> _reportBoundarySectionUpdateReceiverMock;
      private VMReportBoundarySectionFactory _reportBoundarySectionFactory;
      private VMReportBoundarySection _VMReportFooter;
      private VMReportBoundarySection _VMReportHeader;
      private IReportModeProperty _reportModeProperty;
      private IItemId<IReportComponent> _reportComponentId;

      [SetUp]
      public void SetUp()
      {
         _domainX = 50;
         _domainY = 75;
         _footerSection = new ReportFooter(true);
         _headerSection = new ReportHeader(true);
         _textboxControllerMock = new Mock<ITextBoxController>();
         _headerFormController = new Mock<IHeaderFormController>();
         _historyMock = new Mock<IAppStateHistory>();
         _historyMock.Setup(h => h.AddClient(It.IsAny<ISubscriptionClient>(), 0)).Returns(_historyMock.Object);
         _snapShotMock = new Mock<ISnapShot>();
         _historyMock.Setup(h => h.CurrentSnapShot).Returns(_snapShotMock.Object);
         _placementControllerMock = new Mock<IReportComponentPlacementController>();
         _sectionSelectionControllerMock = new Mock<ISelectedSectionController>();
         _selectedComponentControllerMock = new Mock<ISelectedComponentController>();
         _imageControllerMock = new Mock<IImageController>();
         _vmPages = new Mock<IVMPages>();
         _renderData = new Mock<IRenderedData>();
         _busyIndicatorMock = new Mock<IBusyIndicator>();
         _busyIndicatorMock.Setup(b => b.SetIsBusyTrueUntilUIIsIdle()).Callback(() => { });
         _reportBoundarySectionUpdateReceiverMock = new Mock<IReportBoundarySectionUpdateReceiver>();
         _reportBoundarySectionUpdateReceiverMock.Setup(u => u.IsInEditMode).Returns(true);
         _reportModeProperty = new ReportModeProperty(_historyMock.Object);

         _renderData.Setup(rd => rd.Pages).Returns(_vmPages.Object);
         _bodySection = new ReportBody();
         _placementEntity = new ReportComponentPlacement(_bodySection.Id, _domainX, _domainY, 100, 50);
         _reportComponentId = new ReportComponentFake(_placementEntity).Id;

         var changeList = new List<UpdateItemChange>() { new UpdateItemChange(_reportComponentId) };
         _snapShotMock.Setup(h => h.GetChanges()).Returns(new Changes(changeList.ToImmutableList<IItemChange>()));
         _snapShotMock.Setup(s => s.GetItems<ReportModeState>()).Returns(new List<ReportModeState>() { new ReportModeState() });
         _snapShotMock.Setup(ss => ss.GetItems<ReportFooter>()).Returns(new List<ReportFooter>() { _footerSection });
         _snapShotMock.Setup(ss => ss.GetItems<ReportHeader>()).Returns(new List<ReportHeader>() { _headerSection });

         _reportBoundarySectionFactory = new VMReportBoundarySectionFactory(_historyMock.Object, _reportBoundarySectionUpdateReceiverMock.Object, _imageControllerMock.Object, _textboxControllerMock.Object, _headerFormController.Object, _selectedComponentControllerMock.Object, _sectionSelectionControllerMock.Object);

         _VMReportFooter = _reportBoundarySectionFactory.CreateForFooter(5, 1);
         _VMReportHeader = _reportBoundarySectionFactory.CreateForHeader(5, 1);
      }

      [Test]
      public void VMReportComponent_ShouldCallSelectReportComponentController()
      {
         //Arrange
         var vmToTest = new VMReportComponentPlacement(_reportComponentId,
            _placementEntity,
            _placementControllerMock.Object,
            _selectedComponentControllerMock.Object);

         //Act
         vmToTest.Select(null);

         //Assert
         _selectedComponentControllerMock.Verify(s => s.SetSelected(_reportComponentId), Times.Once);
      }

      [Test]
      public void VMReportComponent_ShouldSetPositionNotifyToController()
      {
         // Arrange
         var sut = new VMReportBodyPlacement(_reportComponentId,
            _placementEntity,
            _placementControllerMock.Object,
            _selectedComponentControllerMock.Object,
            _renderData.Object);

         //Update placement Position
         _renderData.Setup(x => x.ConvertToDomainY(150)).Returns(140);
         sut.StartDrag();
         sut.PageVerticalOffset = 10;
         sut.Drag(20, 0);

         sut.Drag(0, 150);

         // Act
         sut.CompleteDrag();

         // Assert
         _placementControllerMock
            .Verify(x => x.SetPosition(_reportComponentId, 70, 0), Times.Once);
      }

      [Test]
      public void StartDragging_ShouldSetIsDraggingFlag()
      {
         // Arrange
         var sut = new VMReportBodyPlacement(_reportComponentId,
            _placementEntity,
            _placementControllerMock.Object,
            _selectedComponentControllerMock.Object,
            _renderData.Object);

         // Act
         sut.StartDrag();

         // Assert
         Assert.IsTrue(sut.IsDragging);
      }

      [Test]
      public void Drag_ShouldMoveVisuals_IfComponentIsDragging()
      {
         // Arrange
         var vmPlacement = new VMReportBodyPlacement(_reportComponentId,
            _placementEntity,
            _placementControllerMock.Object,
            _selectedComponentControllerMock.Object,
            _renderData.Object);

         vmPlacement.PageVerticalOffset = 10;

         int horizontalDelta = 24;
         int verticalDelta = 62;
         var previousVisualX = vmPlacement.VisualX;
         var previousVisualY = vmPlacement.VisualY;

         // Act
         vmPlacement.StartDrag();
         vmPlacement.Drag(horizontalDelta, verticalDelta);

         // Assert
         Assert.IsTrue(vmPlacement.IsDragging);
         Assert.AreEqual(_domainX, vmPlacement.DomainX);
         Assert.AreEqual(_domainY, vmPlacement.DomainY);
         Assert.AreEqual(previousVisualX + horizontalDelta, vmPlacement.VisualX);
         Assert.AreEqual(previousVisualY + verticalDelta, vmPlacement.VisualY);
      }

      [Test]
      public void Drag_ShouldNotMoveVisuals_IfComponentIsNotDragging()
      {
         // Arrange
         var vmPlacement = new VMReportBodyPlacement(_reportComponentId,
            _placementEntity,
            _placementControllerMock.Object,
            _selectedComponentControllerMock.Object,
            _renderData.Object);

         vmPlacement.PageVerticalOffset = 10;

         int horizontalDelta = 24;
         int verticalDelta = 62;
         var previousVisualX = vmPlacement.VisualX;
         var previousVisualY = vmPlacement.VisualY;

         // Act
         vmPlacement.Drag(horizontalDelta, verticalDelta);

         // Assert
         Assert.IsFalse(vmPlacement.IsDragging);
         Assert.AreEqual(_domainX, vmPlacement.DomainX);
         Assert.AreEqual(_domainY, vmPlacement.DomainY);
         Assert.AreEqual(previousVisualX, vmPlacement.VisualX);
         Assert.AreEqual(previousVisualY, vmPlacement.VisualY);
      }

      [Test]
      public void StopDragging_InDisabledSpace_ShouldResetVisuals()
      {
         var renderData = Mock.Of<IRenderedData>();
         Mock.Get(renderData)
            .Setup(x => x.IsInDisabledSpace(It.IsAny<int>()))
            .Returns(true);

         var vmPlacement = new VMReportBodyPlacement(_reportComponentId,
            _placementEntity,
            _placementControllerMock.Object,
            _selectedComponentControllerMock.Object,
            renderData);

         vmPlacement.PageVerticalOffset = 70; //This would be done by Render logic, wich is not running on this test

         var previousVisualX = vmPlacement.VisualX;
         var previousVisualY = vmPlacement.VisualY;

         vmPlacement.StartDrag();
         vmPlacement.Drag(10, 10);
         vmPlacement.CompleteDrag();

         Assert.IsFalse(vmPlacement.IsDragging);
         Assert.AreEqual(_domainX, vmPlacement.DomainX);
         Assert.AreEqual(_domainY, vmPlacement.DomainY);
         Assert.AreEqual(previousVisualX, vmPlacement.VisualX);
         Assert.AreEqual(previousVisualY, vmPlacement.VisualY);
         _placementControllerMock
            .Verify(x =>
               x.SetPosition(
                  _reportComponentId,
                  It.IsAny<int>(),
                  It.IsAny<int>()),
               Times.Never);
      }

      [Test]
      public void StopDragging_InFakeSpace_ShouldSetPositionOnFakeSpace_WhenComponentFitOnPage()
      {
         var pageLayoutForTest = new CommonPageLayout(new PageSizeInfo(PaperKind.Custom, 5, 5), new Margin());
         var renderData = Mock.Of<IRenderedData>();
         var pages = Mock.Of<IVMPages>();
         var pageForComponent = new VMPage(pageLayoutForTest, _reportModeProperty, _VMReportHeader, _VMReportFooter);
         pageForComponent.StartVisualY = 495;

         Mock.Get(renderData)
            .Setup(x => x.IsInDisabledSpace(It.IsAny<int>()))
            .Returns(false);

         Mock.Get(renderData)
            .Setup(x => x.Pages)
            .Returns(pages);

         Mock.Get(renderData)
            .Setup(x => x.IsFakeSpace(It.IsAny<int>()))
            .Returns(true);

         Mock.Get(pages)
            .Setup(x => x.GetPageForPosition(It.IsAny<int>()))
            .Returns(pageForComponent);

         var vmPlacement = new VMReportBodyPlacement(_reportComponentId,
            _placementEntity,
            _placementControllerMock.Object,
            _selectedComponentControllerMock.Object,
            renderData);

         vmPlacement.PageVerticalOffset = pageForComponent.OffsetForComponentsOnThisPage;  //This would be done by Render logic, wich is not running on this test

         vmPlacement.StartDrag();
         vmPlacement.Drag(20, 20);
         vmPlacement.CompleteDrag();

         Assert.IsFalse(vmPlacement.IsDragging);
         Assert.AreEqual(_domainX, vmPlacement.DomainX);
         Assert.AreEqual(_domainY, vmPlacement.DomainY);
         _placementControllerMock
            .Verify(x =>
               x.SetPositionOnFakeSpace(
                  _reportComponentId,
                  It.IsAny<int>(),
                  It.IsAny<int>(),
                  It.IsAny<int>(),
                  It.IsAny<int>()),
               Times.Once);
      }

      [Test]
      public void StopDragging_InFakeSpace_ShouldResetVisuals_WhenComponentNotFitOnPage()
      {
         var pageLayoutForTest = new CommonPageLayout(new PageSizeInfo(PaperKind.Custom, 5, 5), new Margin());
         var renderData = Mock.Of<IRenderedData>();
         var pages = Mock.Of<IVMPages>();
         var pageForComponent = new VMPage(pageLayoutForTest, _reportModeProperty, _VMReportHeader, _VMReportFooter);
         pageForComponent.StartVisualY = 5;

         Mock.Get(renderData)
            .Setup(x => x.IsInDisabledSpace(It.IsAny<int>()))
            .Returns(false);

         Mock.Get(renderData)
            .Setup(x => x.Pages)
            .Returns(pages);

         Mock.Get(renderData)
            .Setup(x => x.IsFakeSpace(It.IsAny<int>()))
            .Returns(true);

         Mock.Get(pages)
            .Setup(x => x.GetPageForPosition(It.IsAny<int>()))
            .Returns(pageForComponent);

         var vmPlacement = new VMReportBodyPlacement(_reportComponentId,
            _placementEntity,
            _placementControllerMock.Object,
            _selectedComponentControllerMock.Object,
            renderData);

         vmPlacement.PageVerticalOffset = pageForComponent.OffsetForComponentsOnThisPage;  //This would be done by Render logic, wich is not running on this test

         int previousVisualX = vmPlacement.VisualX;
         int previousVisualY = vmPlacement.VisualY;

         vmPlacement.StartDrag();
         vmPlacement.Drag(50, 75);
         vmPlacement.CompleteDrag();

         Assert.IsFalse(vmPlacement.IsDragging);
         Assert.AreEqual(_domainX, vmPlacement.DomainX);
         Assert.AreEqual(_domainY, vmPlacement.DomainY);

         Assert.AreEqual(previousVisualX, vmPlacement.VisualX);
         Assert.AreEqual(previousVisualY, vmPlacement.VisualY);

         _placementControllerMock
            .Verify(x =>
               x.SetPositionOnFakeSpace(
                  _reportComponentId,
                  It.IsAny<int>(),
                  It.IsAny<int>(),
                  It.IsAny<int>(),
                  It.IsAny<int>()),
               Times.Never);
      }
   }
}
