// <copyright file="VMDisableSpacePlacementTest.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using System.Drawing.Printing;
using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.ReportModule.App.AppState;
using Mitutoyo.MiCAT.ReportModule.Domain;
using Mitutoyo.MiCAT.ReportModule.Domain.Components;
using Mitutoyo.MiCAT.ReportModule.Domain.Components.ReportSections;
using Mitutoyo.MiCAT.ReportModule.Domain.Data;
using Mitutoyo.MiCAT.ReportModule.GUI.Common.DisabledSpace;
using Mitutoyo.MiCAT.ReportModule.GUI.Common.LayoutCalculator;
using Mitutoyo.MiCAT.ReportModule.GUI.PageLayout;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements;
using Mitutoyo.MiCAT.ReportModuleApp.Controllers;
using Moq;
using NUnit.Framework;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Test.VisualElements.ReportElements
{
   [ExcludeFromCodeCoverage]
   [TestFixture]
   public class VMDisableSpacePlacementTest
   {
      private Mock<ISelectedComponentController> _selectedComponentController;
      private Mock<IReportComponentPlacementController> _placementController;
      private Mock<IAppStateHistory> _historyMock;
      private Mock<ISnapShot> _snapShotMock;
      private Mock<IRenderedData> _renderData;
      private ReportComponentPlacement _placement;
      private IItemId<IReportComponent> _reportComponentId;

      [SetUp]
      public void SetUp()
      {
         _placementController = new Mock<IReportComponentPlacementController>();
         _selectedComponentController = new Mock<ISelectedComponentController>();
         _renderData = new Mock<IRenderedData>();

         var reportBody = new ReportBody();
         _placement = new ReportComponentPlacement(reportBody.Id, 300, 10, 20, 20);
         _reportComponentId = new ReportComponentFake(_placement).Id;

         _historyMock = new Mock<IAppStateHistory>();
         _historyMock.Setup(h => h.AddClient(It.IsAny<ISubscriptionClient>(), 0)).Returns(_historyMock.Object);
         _snapShotMock = new Mock<ISnapShot>();
         _snapShotMock.Setup(s => s.GetItems<ReportModeState>()).Returns(new ReportModeState[] { new ReportModeState() });

         _renderData = new Mock<IRenderedData>();

         _historyMock.Setup(h => h.CurrentSnapShot).Returns(_snapShotMock.Object);
      }

      [Test]
      public void NoFakeSpaceMovingDownATableTest()
      {
         //Arrange
         int testMovingDeltaY = 100;
         var vmPlacement = new VMDisableSpacePlacement(_reportComponentId, _placement, _placementController.Object, _selectedComponentController.Object, _renderData.Object);

         vmPlacement.PageVerticalOffset = 10;
         vmPlacement.LastDisabledSpaceGenerated = new DisabledSpaceData(25, null, 100, null, 175, 50);

         _renderData.Setup(rd => rd.IsFakeSpace(vmPlacement.VisualY + testMovingDeltaY)).Returns(true);
         _renderData.Setup(rd => rd.IsInDisabledSpace(vmPlacement.VisualY + testMovingDeltaY, vmPlacement.LastDisabledSpaceGenerated)).Returns(false);

         //Act
         vmPlacement.StartDrag();
         vmPlacement.Drag(0, testMovingDeltaY);
         vmPlacement.CompleteDrag();

         //Assert
         _placementController.Verify(c => c.SetPositionOnFakeSpace(_reportComponentId, It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()), Times.Never);
         _placementController.Verify(c => c.SetPosition(_reportComponentId, It.IsAny<int>(), It.IsAny<int>()), Times.Once);
      }

      [Test]
      public void FakeSpaceMovingUpATableTest()
      {
         //Arrange
         int testMovingDeltaY = -100;
         int newDomainY = 100;
         var pageLayoutForTest = new CommonPageLayout(new PageSizeInfo(PaperKind.Custom, 1000, 100), new Margin(MarginKind.Narrow, 0, 48, 0, 48));
         var destinationPage = new VMPage(pageLayoutForTest, null, null, null);
         var pages = new Mock<IVMPages>();
         var vmPlacement = new VMDisableSpacePlacement(_reportComponentId, _placement, _placementController.Object, _selectedComponentController.Object, _renderData.Object);

         vmPlacement.PageVerticalOffset = 50;
         vmPlacement.LastDisabledSpaceGenerated = new DisabledSpaceData(25, null, 100, null, 175, 50);

         _renderData.Setup(rd => rd.IsFakeSpace(vmPlacement.VisualY + testMovingDeltaY)).Returns(true);
         _renderData.Setup(rd => rd.IsInDisabledSpace(vmPlacement.VisualY + testMovingDeltaY, vmPlacement.LastDisabledSpaceGenerated)).Returns(false);
         _renderData.Setup(rd => rd.ConvertToDomainY(vmPlacement.VisualY + testMovingDeltaY, vmPlacement.LastDisabledSpaceGenerated)).Returns(newDomainY);
         _renderData.Setup(rd => rd.Pages).Returns(pages.Object);
         pages.Setup(p => p.GetPageForPosition(vmPlacement.VisualY + testMovingDeltaY)).Returns(destinationPage);

         //Act
         vmPlacement.StartDrag();
         vmPlacement.Drag(0, testMovingDeltaY);
         vmPlacement.CompleteDrag();

         //Assert
         _placementController.Verify(c => c.SetPositionOnFakeSpace(_reportComponentId, It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()), Times.Once);
         _placementController.Verify(c => c.SetPosition(_reportComponentId, It.IsAny<int>(), It.IsAny<int>()), Times.Never);
      }
   }
}
