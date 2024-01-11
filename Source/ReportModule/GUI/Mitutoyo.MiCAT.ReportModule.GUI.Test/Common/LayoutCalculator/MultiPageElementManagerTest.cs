// <copyright file="MultiPageElementManagerTest.cs" company="Mitutoyo Europe GmbH">
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
using Mitutoyo.MiCAT.ReportModule.Domain;
using Mitutoyo.MiCAT.ReportModule.Domain.Components;
using Mitutoyo.MiCAT.ReportModule.Domain.Components.ReportSections;
using Mitutoyo.MiCAT.ReportModule.Domain.Data;
using Mitutoyo.MiCAT.ReportModule.GUI.Common;
using Mitutoyo.MiCAT.ReportModule.GUI.Common.DisabledSpace;
using Mitutoyo.MiCAT.ReportModule.GUI.Common.LayoutCalculator;
using Mitutoyo.MiCAT.ReportModule.GUI.PageLayout;
using Mitutoyo.MiCAT.ReportModule.GUI.ReportBoundarySection;
using Mitutoyo.MiCAT.ReportModule.GUI.Test.VisualElements.ReportElements;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.Pieces;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElementList.ViewModels;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements;
using Mitutoyo.MiCAT.ReportModuleApp.Controllers;
using Moq;
using NUnit.Framework;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Test.Common.LayoutCalculator
{
   [ExcludeFromCodeCoverage]
   [TestFixture]
   public class MultiPageElementManagerTest
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
      private ReportComponentPlacement _placement;
      private IReportComponent _reportComponent;

      [SetUp]
      public void Setup()
      {
         _textboxControllerMock = new Mock<ITextBoxController>();
         _imageControllerMock = new Mock<IImageController>();
         _headerFormControllerMock = new Mock<IHeaderFormController>();
         _reportBoundarySectionUpdateReceiverMock = new Mock<IReportBoundarySectionUpdateReceiver>();
         _selectedComponentControllerMock = new Mock<ISelectedComponentController>();
         _sectionSelectionControllerMock = new Mock<ISelectedSectionController>();
         _reportBoundarySectionUpdateReceiverMock.Setup(u => u.IsInEditMode).Returns(true);
         _historyMock = new Mock<IAppStateHistory>();
         _vmReportBoundarySectionFactory = new VMReportBoundarySectionFactory(_historyMock.Object, _reportBoundarySectionUpdateReceiverMock.Object, _imageControllerMock.Object, _textboxControllerMock.Object, _headerFormControllerMock.Object, _selectedComponentControllerMock.Object, _sectionSelectionControllerMock.Object);
         _historyMock.Setup(h => h.AddClient(It.IsAny<ISubscriptionClient>(), 0)).Returns(_historyMock.Object);
         _reportModeProperty = new ReportModeProperty(_historyMock.Object);
         var reportBody = new ReportBody();
         _placement = new ReportComponentPlacement(reportBody.Id, 10, 20, 30, 40);
         _reportComponent = new ReportComponentFake(_placement);

         var snapShot = new Mock<ISnapShot>();
         var reportMode = new ReportModeState(true);
         var componentSelection = new ReportComponentSelection(new IItemId<IReportComponent>[] { _reportComponent.Id as IItemId<IReportComponent> });

         snapShot.Setup(s => s.GetItems<ReportModeState>()).Returns(new ReportModeState[] { reportMode });
         snapShot.Setup(s => s.GetItems<ReportComponentSelection>()).Returns(new ReportComponentSelection[] { componentSelection });
         snapShot.Setup(s => s.GetItems<ReportBody>()).Returns(new ReportBody[] { reportBody });
         snapShot.Setup(s => s.GetItems<ReportHeader>()).Returns(new ReportHeader[] { new ReportHeader() });
         snapShot.Setup(s => s.GetItems<ReportFooter>()).Returns(new ReportFooter[] { new ReportFooter() });
         snapShot.Setup(s => s.GetItem<IReportComponent>(_reportComponent.Id)).Returns(_reportComponent);

         _historyMock.Setup(h => h.CurrentSnapShot).Returns(snapShot.Object);
      }

      [Test]
      public void RenderMultiPageElement_Test()
      {
         // Arrange
         Mock<ISelectedComponentController> selectedComponentController = new Mock<ISelectedComponentController>();
         Mock<IVMReportElementList> elementList = new Mock<IVMReportElementList>();
         Mock<IDisabledSpaceDataCollection> disabledSpaces = new Mock<IDisabledSpaceDataCollection>();

         var pages = new VMPages(_vmReportBoundarySectionFactory, _reportBoundarySectionUpdateReceiverMock.Object, _reportModeProperty);
         var pageSettings = new CommonPageLayout(
            new PageSizeInfo(PaperKind.A4, 297, 210),
            new Margin(MarginKind.Narrow, 0, 48, 0, 48),
            new HeaderData(50),
            new FooterData(50)
         );

         var vmComponent = new Mock<IVMReportComponent>();
         var placementController = new Mock<IReportComponentPlacementController>();

         var vmPlacement1 = new VMVisualPlacement();
         var vmPlacement2 = new VMVisualPlacement();
         var vmPlacement3 = new VMVisualPlacement();

         var piece1 = new VMVisualElementPiece(vmPlacement1, vmComponent.Object);
         var piece2 = new VMVisualElementPiece(vmPlacement2, vmComponent.Object);
         var piece3 = new VMVisualElementPiece(vmPlacement3, vmComponent.Object);
         var pieces = (new List<IVMVisualElementPiece>() { piece1, piece2, piece3 }).ToImmutableList();
         var renderContent = new RenderedData(disabledSpaces.Object, pages);

         Mock<IVMMultiPageSplittableElement> element = new Mock<IVMMultiPageSplittableElement>();
         element.Setup(e => e.VMPlacement.VisualY).Returns(100);
         element.Setup(e => e.Pieces).Returns(pieces);

         var sut = new MultiPageElementManager(new OnePiecePerPageAllocator());

         // Act
         sut.RenderMultiPageElement(element.Object, pageSettings, elementList.Object, renderContent);

         // Assert
         elementList.Verify(el => el.RemovePieces(pieces), Times.Once());
         elementList.Verify(el => el.AddPieces(pieces), Times.Once());
         element.Verify(el => el.RegeneratePieces(), Times.Once());

         Assert.AreEqual(pages.Pages.Count, 3);

         Assert.AreEqual(piece1.VMPlacement.VisualY, pages.Pages[0].StartVisualY);
         Assert.AreEqual(70, piece1.VMPlacement.VisualY);
         Assert.AreEqual(piece2.VMPlacement.VisualY, pages.Pages[1].StartVisualY);
         Assert.AreEqual(387, piece2.VMPlacement.VisualY);
         Assert.AreEqual(piece3.VMPlacement.VisualY, pages.Pages[2].StartVisualY);
         Assert.AreEqual(704, piece3.VMPlacement.VisualY);
      }
   }
}
