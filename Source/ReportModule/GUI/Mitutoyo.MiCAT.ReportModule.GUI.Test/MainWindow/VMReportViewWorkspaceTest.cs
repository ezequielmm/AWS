// <copyright file="VMReportViewWorkspaceTest.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Windows.Input;
using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.ReportModule.App.AppState;
using Mitutoyo.MiCAT.ReportModule.Domain;
using Mitutoyo.MiCAT.ReportModule.GUI.Common;
using Mitutoyo.MiCAT.ReportModule.GUI.Common.BusyIndicators;
using Mitutoyo.MiCAT.ReportModule.GUI.Common.LayoutCalculator;
using Mitutoyo.MiCAT.ReportModule.GUI.ContextMenu;
using Mitutoyo.MiCAT.ReportModule.GUI.MainWindow;
using Mitutoyo.MiCAT.ReportModule.GUI.MainWindow.Zoom;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements;
using Mitutoyo.MiCAT.ReportModuleApp.Controllers;
using Mitutoyo.MiCAT.ReportModuleApp.Utilities;
using Moq;
using NUnit.Framework;
using Unity;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Test.MainWindow
{
   [ExcludeFromCodeCoverage]
   [TestFixture]
   public class VMReportViewWorkspaceTest
   {
      private Mock<IUnityContainer> _iocContainerMock;
      private Mock<ISelectedComponentController> _selectionComponentControllerMock;
      private Mock<IDeleteComponentController> _deleteControllerMock;
      private Mock<IBusyIndicator> _busyIndicatorMock;
      private Mock<IActionCaller> _actionCallerMock;
      private IActionCaller _actionCaller;
      private PageSizeList pageSizeList;
      private Mock<IAppStateHistory> _appStateHistoryMock;
      private VMAppStateTestManager _vmAppStateTestManager;
      private Mock<IUndoRedoController> _UndoRedoControllerMock;
      private Mock<IRenderedData> _renderedData;
      private Mock<IViewToVMReportComponent> _viewToVMReportComponentMock;
      private Mock<IPagesRenderer> _pagesRendererMock;
      private Mock<IVMZoomFactor> _vmZoomFactorMock;
      private Mock<VMReportView> _vmReportView;
      private Mock<ISelectedSectionController> _sectionSelectionControllerMock;
      private Mock<IReportModeProperty> _reportModeProperty;
      private Mock<IViewVisibilityController> _viewVisibilityControllerMock;
      private Mock<IUnselectionArea> _unselectionAreaMock;

      [SetUp]
      public void Setup()
      {
         _iocContainerMock = new Mock<IUnityContainer>();
         _vmAppStateTestManager = new VMAppStateTestManager();

         var commonPageLayoutControllerMock = new Mock<ICommonPageLayoutController>();
         _selectionComponentControllerMock = new Mock<ISelectedComponentController>();
         _sectionSelectionControllerMock = new Mock<ISelectedSectionController>();
         _reportModeProperty = new Mock<IReportModeProperty>();

         _vmZoomFactorMock = new Mock<IVMZoomFactor>();
         _deleteControllerMock = new Mock<IDeleteComponentController>();
         _appStateHistoryMock = new Mock<IAppStateHistory>();
         _appStateHistoryMock.Setup(h => h.AddClient(It.IsAny<ISubscriptionClient>(), 0)).Returns(_appStateHistoryMock.Object);

         _vmReportView = new Mock<VMReportView>(MockBehavior.Strict, new object[]
         {
            new Mock<IPagesRenderer>().Object,
            new Mock<IReportModeProperty>().Object,new Mock<PaletteContextMenu>(MockBehavior.Strict, new object[]
            {
               new Mock<ITextBoxController>().Object,
               new Mock<IImageController>().Object,
               new Mock<ITessellationViewController>().Object,
               new Mock<IHeaderFormController>().Object,
               new Mock<ITableViewController>().Object,
               new Mock<IRenderedData>().Object
            }).Object
         });

         _viewToVMReportComponentMock = new Mock<IViewToVMReportComponent>();

         _busyIndicatorMock = new Mock<IBusyIndicator>();

         _busyIndicatorMock.Setup(b => b.SetIsBusyTrueUntilUIIsIdle()).Callback(() => { });
         _actionCaller = new BusyIndicatorActionCaller(_busyIndicatorMock.Object);

         _actionCallerMock = new Mock<IActionCaller>();

         _renderedData = new Mock<IRenderedData>();
         _pagesRendererMock = new Mock<IPagesRenderer>();
         _pagesRendererMock.Setup(p => p.RenderedData).Returns(_renderedData.Object);

         pageSizeList = new PageSizeList();
         pageSizeList.Initialize();

         _iocContainerMock.Setup(ioc => ioc.Resolve(typeof(IPlanController), null)).Returns(new Mock<IPlanController>().Object);
         _iocContainerMock.Setup(ioc => ioc.Resolve(typeof(IRunController), null)).Returns(new Mock<IRunController>().Object);
         _iocContainerMock.Setup(ioc => ioc.Resolve(typeof(IActionCaller), null)).Returns(new Mock<IActionCaller>().Object);
         _iocContainerMock.Setup(ioc => ioc.Resolve(typeof(IAppStateHistory), null)).Returns(new Mock<IAppStateHistory>().Object);

         _UndoRedoControllerMock = new Mock<IUndoRedoController>();
         _viewVisibilityControllerMock = new Mock<IViewVisibilityController>();
         _unselectionAreaMock = new Mock<IUnselectionArea>();
      }

      [Test]
      public void VMReportViewerWorkspace_ShouldCreateProperly()
      {
         var sut = new VMReportViewWorkspace(
            _viewToVMReportComponentMock.Object,
            _pagesRendererMock.Object,
            _deleteControllerMock.Object,
            _selectionComponentControllerMock.Object,
            _sectionSelectionControllerMock.Object,
            _UndoRedoControllerMock.Object,
            _busyIndicatorMock.Object,
            _actionCallerMock.Object,
            _appStateHistoryMock.Object,
            _vmZoomFactorMock.Object,
            _vmReportView.Object,
            _reportModeProperty.Object,
            _viewVisibilityControllerMock.Object,
            _unselectionAreaMock.Object);

         Assert.NotNull(sut.ZoomFactor);
         Assert.NotNull(sut.PageLayoutCalculator);
      }

      [Test]
      [Apartment(ApartmentState.STA)]
      public void VMPageLayout_ShouldDeleteIfThereIsASelectedItem()
      {
         // Arrange
         _deleteControllerMock.Setup(del => del.DeleteSelectedComponents()).Verifiable();
         var reportModeState = _vmAppStateTestManager.CurrentSnapShot.GetItems<ReportModeState>().Single();

         var sut = new VMReportViewWorkspace(
            _viewToVMReportComponentMock.Object,
            _pagesRendererMock.Object,
            _deleteControllerMock.Object,
            _selectionComponentControllerMock.Object,
            _sectionSelectionControllerMock.Object,
            _UndoRedoControllerMock.Object,
            _busyIndicatorMock.Object, _actionCaller,
            _vmAppStateTestManager.History,
            _vmZoomFactorMock.Object,
            _vmReportView.Object,
            _reportModeProperty.Object,
            _viewVisibilityControllerMock.Object,
            _unselectionAreaMock.Object);

         _vmAppStateTestManager.UpdateSnapshot(reportModeState.With(true));
         sut.DeleteSelectedItemsCommand.Execute(null);

         // Assert
         _deleteControllerMock.Verify(dc => dc.DeleteSelectedComponents(), Times.Once);
      }

      [Test]
      [TestCase(true)]
      [TestCase(false)]
      public void VMReportViewWorkspace_AppstateShouldUpdateEditModeValue(bool editMode)
      {
         // Arrange
         var reportModeState = _vmAppStateTestManager.CurrentSnapShot.GetItems<ReportModeState>().Single();

         if (reportModeState.EditMode == editMode)
            _vmAppStateTestManager.UpdateSnapshot(reportModeState.With(!editMode));

         var _viewModel = new VMReportViewWorkspace(
                    _viewToVMReportComponentMock.Object,
                    _pagesRendererMock.Object,
                    _deleteControllerMock.Object,
                    _selectionComponentControllerMock.Object,
                    _sectionSelectionControllerMock.Object,
                    _UndoRedoControllerMock.Object,
                    _busyIndicatorMock.Object,
                    _actionCallerMock.Object,
                    _vmAppStateTestManager.History,
                    _vmZoomFactorMock.Object,
                    _vmReportView.Object,
                    _reportModeProperty.Object,
                    _viewVisibilityControllerMock.Object,
                    _unselectionAreaMock.Object);

         // Act
         _vmAppStateTestManager.UpdateSnapshot(reportModeState.With(editMode));

         // Assert
         Assert.AreEqual(_viewModel.EditMode, editMode);
      }

      [Apartment(ApartmentState.STA)]
      [Test]
      public void VMReportViewWorkspace_ClearSelectionShouldUnSelectAll()
      {
         // Arrange
         var _viewModel = new VMReportViewWorkspace(
            _viewToVMReportComponentMock.Object,
            _pagesRendererMock.Object,
            _deleteControllerMock.Object,
            _selectionComponentControllerMock.Object,
            _sectionSelectionControllerMock.Object,
            _UndoRedoControllerMock.Object,
            _busyIndicatorMock.Object,
            _actionCallerMock.Object,
            _vmAppStateTestManager.History,
            _vmZoomFactorMock.Object,
            _vmReportView.Object,
            _reportModeProperty.Object,
            _viewVisibilityControllerMock.Object,
            _unselectionAreaMock.Object);

         _unselectionAreaMock.Setup(sa => sa.CheckIsSelected(It.IsAny<object>(), It.IsAny<object>()))
            .Returns(true);

         var leftClickEventArgs = new MouseButtonEventArgs(Mouse.PrimaryDevice, 0, MouseButton.Left);
         // Act
         _viewModel.ClearSelection.Execute(leftClickEventArgs);

         // Assert
         _selectionComponentControllerMock.Verify(s => s.UnSelectAll(), Times.Once);
      }

      [Apartment(ApartmentState.STA)]
      [Test]
      public void VMReportViewWorkspace_ClearSelectionShouldNotUnSelectAllOnHeaderFooter()
      {
         // Arrange
         var _viewModel = new VMReportViewWorkspace(
            _viewToVMReportComponentMock.Object,
            _pagesRendererMock.Object,
            _deleteControllerMock.Object,
            _selectionComponentControllerMock.Object,
            _sectionSelectionControllerMock.Object,
            _UndoRedoControllerMock.Object,
            _busyIndicatorMock.Object,
            _actionCallerMock.Object,
            _vmAppStateTestManager.History,
            _vmZoomFactorMock.Object,
            _vmReportView.Object,
            _reportModeProperty.Object,
            _viewVisibilityControllerMock.Object,
            _unselectionAreaMock.Object);

         _unselectionAreaMock.Setup(sa => sa.CheckIsSelected(It.IsAny<object>(), It.IsAny<object>()))
            .Returns(false);

         var leftClickEventArgs = new MouseButtonEventArgs(Mouse.PrimaryDevice, 0, MouseButton.Left);

         // Act
         _viewModel.ClearSelection.Execute(leftClickEventArgs);

         // Assert
         _selectionComponentControllerMock.Verify(s => s.UnSelectAll(), Times.Never());
      }

      [Test]
      public void VMReportViewWorkspace_UndoShouldCallUndoRedoController()
      {
         // Arrange
         var busyIndicatorMock = new Mock<IBusyIndicator>();
         busyIndicatorMock.Setup(b => b.SetIsBusyTrueUntilUIIsIdle()).Callback(() => { });
         var actionCaller = new BusyIndicatorActionCaller(_busyIndicatorMock.Object);
         var _viewModel = new VMReportViewWorkspace(
               _viewToVMReportComponentMock.Object,
               _pagesRendererMock.Object,
               _deleteControllerMock.Object,
               _selectionComponentControllerMock.Object,
               _sectionSelectionControllerMock.Object,
               _UndoRedoControllerMock.Object,
               _busyIndicatorMock.Object,
               actionCaller,
               _vmAppStateTestManager.History,
               _vmZoomFactorMock.Object,
               _vmReportView.Object,
               _reportModeProperty.Object,
               _viewVisibilityControllerMock.Object,
               _unselectionAreaMock.Object);

         // Act
         _viewModel.UndoCommand.Execute(null);

         // Assert
         _UndoRedoControllerMock.Verify(v => v.Undo(), Times.Once);
      }
      [Test]
      public void VMReportViewWorkspace_RedoShouldCallUndoRedoController()
      {
         // Arrange
         var busyIndicatorMock = new Mock<IBusyIndicator>();
         busyIndicatorMock.Setup(b => b.SetIsBusyTrueUntilUIIsIdle()).Callback(() => { });
         var actionCaller = new BusyIndicatorActionCaller(_busyIndicatorMock.Object);
         var _viewModel = new VMReportViewWorkspace(
               _viewToVMReportComponentMock.Object,
               _pagesRendererMock.Object,
               _deleteControllerMock.Object,
               _selectionComponentControllerMock.Object,
               _sectionSelectionControllerMock.Object,
               _UndoRedoControllerMock.Object,
               _busyIndicatorMock.Object,
               actionCaller,
               _vmAppStateTestManager.History,
               _vmZoomFactorMock.Object,
               _vmReportView.Object,
               _reportModeProperty.Object,
               _viewVisibilityControllerMock.Object,
               _unselectionAreaMock.Object);

         // Act
         _viewModel.RedoCommand.Execute(null);

         // Assert
         _UndoRedoControllerMock.Verify(v => v.Redo(), Times.Once);
      }
      [Test]
      public void VMReportViewWorkspace_OnDisposeWhouldSetAllowDragOnFalse()
      {
         // Arrange
         var _viewModel = new VMReportViewWorkspace(
               _viewToVMReportComponentMock.Object,
               _pagesRendererMock.Object,
               _deleteControllerMock.Object,
               _selectionComponentControllerMock.Object,
               _sectionSelectionControllerMock.Object,
               _UndoRedoControllerMock.Object,
               _busyIndicatorMock.Object,
               _actionCallerMock.Object,
               _vmAppStateTestManager.History,
               _vmZoomFactorMock.Object,
               _vmReportView.Object,
               _reportModeProperty.Object,
               _viewVisibilityControllerMock.Object,
               _unselectionAreaMock.Object);

         // Act
         _viewModel.Dispose();

         // Assert
         Assert.IsFalse(_viewModel.AllowDragDrop);
      }
   }
}