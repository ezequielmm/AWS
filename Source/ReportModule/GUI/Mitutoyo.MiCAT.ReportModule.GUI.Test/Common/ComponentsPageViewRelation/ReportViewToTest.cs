// <copyright file="ReportViewToTest.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Drawing.Printing;
using System.Windows;
using System.Windows.Threading;
using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.ReportModule.App.AppState;
using Mitutoyo.MiCAT.ReportModule.Domain;
using Mitutoyo.MiCAT.ReportModule.Domain.Components;
using Mitutoyo.MiCAT.ReportModule.Domain.Components.ReportSections;
using Mitutoyo.MiCAT.ReportModule.Domain.Data;
using Mitutoyo.MiCAT.ReportModule.GUI.Common;
using Mitutoyo.MiCAT.ReportModule.GUI.Common.DispatcherWrapping;
using Mitutoyo.MiCAT.ReportModule.GUI.Common.LayoutCalculator;
using Mitutoyo.MiCAT.ReportModule.GUI.ContextMenu;
using Mitutoyo.MiCAT.ReportModule.GUI.MainWindow;
using Mitutoyo.MiCAT.ReportModule.GUI.MainWindow.Zoom;
using Mitutoyo.MiCAT.ReportModule.GUI.PageLayout;
using Mitutoyo.MiCAT.ReportModule.GUI.ReportBoundarySection;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.Pieces;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.Pieces.Table.ViewModels;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElementList.ViewModels;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements.Image.ViewModels;
using Mitutoyo.MiCAT.ReportModuleApp.Controllers;
using Mitutoyo.MiCAT.ReportModuleApp.Utilities;
using Moq;
using Unity;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Test.Common.ComponentsPageViewRelation
{
   //REFACTOR_REVIEW_THIS
   [ExcludeFromCodeCoverage]
   public class ReportViewToTest
   {
      private Mock<IRenderedData> _renderedData;
      private Mock<IImageController> _imageControllerrMock;
      private Mock<IHeaderFormController> _headerFormControllerMock;
      private Mock<ISelectedComponentController> _selectionComponentControllerMock;
      private Mock<ISelectedSectionController> _sectionSelectionControllerMock;
      private Mock<IVMReportElementList> _elementList;
      private Mock<ISnapShot> _snapShotMock;
      private Mock<IActionCaller> _actionCallerMock;
      private Mock<IDeleteComponentController> _deleteComponentControllerMock;
      private Mock<VMReportView> _vmReportViewMock;
      private Mock<PaletteContextMenu> _palleteContextMenuMock;
      private Mock<IAppStateHistory> _historyMock;
      private CommonPageLayout _commonPageLayout;
      private VMPages _vmPages;
      private IReportModeProperty _reportModeProperty;

      public ReportViewToTest()
      {
         Mock<IPagesRenderer> pagesRendererMock = new Mock<IPagesRenderer>();
         Mock<IDeleteComponentController> deleteControllerMock = new Mock<IDeleteComponentController>();
         Mock<IUndoRedoController> undoRedoControllerMock = new Mock<IUndoRedoController>();
         Mock<IImageController> reportBoundarySectionImageControllerMock = new Mock<IImageController>();
         Mock<ITextBoxController> reportBoundarySectionTextboxControllerMock = new Mock<ITextBoxController>();
         Mock<IVMZoomFactor> zoomFactorMock = new Mock<IVMZoomFactor>();
         Mock<IDispatcherWrapper> dispatcherWrapper = new Mock<IDispatcherWrapper>();
         _headerFormControllerMock = new Mock<IHeaderFormController>();

         _selectionComponentControllerMock = new Mock<ISelectedComponentController>();
         _sectionSelectionControllerMock = new Mock<ISelectedSectionController>();

         Mock<IReportBoundarySectionUpdateReceiver> _reportBoundarySectionUpdateReceiverMock = new Mock<IReportBoundarySectionUpdateReceiver>();
         _reportBoundarySectionUpdateReceiverMock.Setup(u => u.IsInEditMode).Returns(true);

         _historyMock = new Mock<IAppStateHistory>();
         _historyMock.Setup(h => h.AddClient(It.IsAny<ISubscriptionClient>(), 0)).Returns(_historyMock.Object);

         var vmReportBoundarySectionFactory = new VMReportBoundarySectionFactory(_historyMock.Object, _reportBoundarySectionUpdateReceiverMock.Object, reportBoundarySectionImageControllerMock.Object, reportBoundarySectionTextboxControllerMock.Object, _headerFormControllerMock.Object, _selectionComponentControllerMock.Object, _sectionSelectionControllerMock.Object);

         _actionCallerMock = new Mock<IActionCaller>();
         _elementList = new Mock<IVMReportElementList>();
         _elements = ImmutableList<IVMReportComponent>.Empty;
         _pieces = ImmutableList<IVMVisualElementPiece>.Empty;
         _reportModeProperty = new ReportModeProperty(_historyMock.Object);
         _vmPages = new VMPages(vmReportBoundarySectionFactory, _reportBoundarySectionUpdateReceiverMock.Object, _reportModeProperty);
         _commonPageLayout = new CommonPageLayout(new PageSizeInfo(PaperKind.Letter, 830, 210), new Margin(MarginKind.Narrow, 0, 48, 0, 48), new HeaderData(100), new FooterData(100));

         _renderedData = new Mock<IRenderedData>();
         _imageControllerrMock = new Mock<IImageController>();

         _snapShotMock = new Mock<ISnapShot>();
         _snapShotMock.Setup(s => s.GetItems<ReportHeader>()).Returns(new List<ReportHeader>() { new ReportHeader() });
         _snapShotMock.Setup(s => s.GetItems<ReportFooter>()).Returns(new List<ReportFooter>() { new ReportFooter() });

         _historyMock.Setup(h => h.CurrentSnapShot).Returns(_snapShotMock.Object);

         _deleteComponentControllerMock = new Mock<IDeleteComponentController>();
         _vmReportViewMock = new Mock<VMReportView>();
         _palleteContextMenuMock = new Mock<PaletteContextMenu>(MockBehavior.Strict, new object[]
         {
            new Mock<ITextBoxController>().Object,
            new Mock<IImageController>().Object,
            new Mock<ITessellationViewController>().Object,
            new Mock<IHeaderFormController>().Object,
            new Mock<ITableViewController>().Object,
            new Mock<IRenderedData>().Object
         });

         var iocContainerMock = new Mock<IUnityContainer>();
         iocContainerMock.Setup(ioc => ioc.Resolve(typeof(IPlanController), null)).Returns(new Mock<IPlanController>().Object);
         iocContainerMock.Setup(ioc => ioc.Resolve(typeof(IRunController), null)).Returns(new Mock<IRunController>().Object);
         iocContainerMock.Setup(ioc => ioc.Resolve(typeof(IActionCaller), null)).Returns(new Mock<IActionCaller>().Object);

         VMReportView vmReportView = new VMReportView(
                        pagesRendererMock.Object,
                        _reportModeProperty,
                        _palleteContextMenuMock.Object);

         pagesRendererMock.Setup(pr => pr.RenderedData).Returns(_renderedData.Object);
         pagesRendererMock.Setup(pr => pr.ElementList).Returns(_elementList.Object);

         _renderedData.Setup(rd => rd.Pages).Returns(_vmPages);
         _elementList.Setup(el => el.Elements).Returns(_elements);
         _elementList.Setup(el => el.ElementPieces).Returns(_pieces);

         ReportView = new ReportView();
         ReportView.DataContext = vmReportView;
      }

      private ImmutableList<IVMReportComponent> _elements;
      private ImmutableList<IVMVisualElementPiece> _pieces;

      public ReportView ReportView { get; }

      public VMPage AddPage()
      {
         return _vmPages.NextPage(_commonPageLayout);
      }

      public IVMVisualElement AddElement(int x, int y, int height, int width)
      {
         var placement = new ReportComponentPlacement(new ReportBody().Id, x, y, width, height);
         var newDomainImage = new ReportImage(placement);

         _snapShotMock.Setup(s => s.GetItem(newDomainImage.Id as IItemId<IReportComponent>)).Returns(newDomainImage);
         _snapShotMock.Setup(s => s.GetChanges()).Returns(new Changes(new IItemChange[] { new UpdateItemChange(newDomainImage.Id) }.ToImmutableList()));
         _snapShotMock.Setup(s => s.GetItems<ReportModeState>()).Returns(new List<ReportModeState>() { new ReportModeState() });

         var vmPlacementMock = Mock.Of<IVMReportComponentPlacement>();
         var vmImage = new VMImage(_historyMock.Object, newDomainImage.Id, vmPlacementMock, _deleteComponentControllerMock.Object, _imageControllerrMock.Object, _actionCallerMock.Object);

         _elements = _elements.Add(vmImage).ToImmutableList();
         _elementList.Setup(e => e.Elements).Returns(_elements);

         return vmImage;
      }

      public VMVisualElementPiece AddPiece(int x, int y, int height, int width)
      {
         var vmComponent = Mock.Of<IVMReportComponent>();
         var vmReportComponentPlacement = Mock.Of<IVMReportComponentPlacement>();

         Mock.Get(vmComponent).Setup(vm => vm.VMPlacement).Returns(vmReportComponentPlacement);

         var vmReportComponentPlacementMock = Mock.Get(vmReportComponentPlacement);
         vmReportComponentPlacementMock.Setup(vm => vm.VisualX).Returns(x);
         vmReportComponentPlacementMock.Setup(vm => vm.VisualY).Returns(y);
         vmReportComponentPlacementMock.Setup(vm => vm.VisualWidth).Returns(width);
         vmReportComponentPlacementMock.Setup(vm => vm.VisualHeight).Returns(height);

         var newPiece = new VMTablePiece(vmReportComponentPlacement, vmComponent);

         _pieces = _pieces.Add(newPiece);
         _elementList.Setup(e => e.ElementPieces).Returns(_pieces);

         return newPiece;
      }

      public void Render()
      {
         ReportView.Measure(new Size(Double.PositiveInfinity, Double.PositiveInfinity));
         ReportView.Arrange(new Rect(ReportView.DesiredSize));

         WaitUntilRenderEnds();
      }

      private void WaitUntilRenderEnds()
      {
         ReportView.Dispatcher?.Invoke(new Action(() => { }), DispatcherPriority.ContextIdle, null);
      }
   }
}
