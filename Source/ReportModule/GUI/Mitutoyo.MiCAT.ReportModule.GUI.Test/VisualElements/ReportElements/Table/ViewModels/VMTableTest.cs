// <copyright file="VMTableTest.cs" company="Mitutoyo Europe GmbH">
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
using System.Linq;
using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.ReportModule.App.AppState;
using Mitutoyo.MiCAT.ReportModule.Domain;
using Mitutoyo.MiCAT.ReportModule.Domain.Components;
using Mitutoyo.MiCAT.ReportModule.Domain.Components.ReportSections;
using Mitutoyo.MiCAT.ReportModule.Domain.Components.TableView;
using Mitutoyo.MiCAT.ReportModule.Domain.Data;
using Mitutoyo.MiCAT.ReportModule.GUI.Common;
using Mitutoyo.MiCAT.ReportModule.GUI.Common.BusyIndicators;
using Mitutoyo.MiCAT.ReportModule.GUI.Common.DisabledSpace;
using Mitutoyo.MiCAT.ReportModule.GUI.Common.LayoutCalculator;
using Mitutoyo.MiCAT.ReportModule.GUI.PageLayout;
using Mitutoyo.MiCAT.ReportModule.GUI.ReportBoundarySection;
using Mitutoyo.MiCAT.ReportModule.GUI.Strings;
using Mitutoyo.MiCAT.ReportModule.GUI.Test.Components.TableView;
using Mitutoyo.MiCAT.ReportModule.GUI.Test.PageLayout;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.Pieces.Table.ViewModels;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElementList.ViewModels;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements.Table.ViewModels;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements.Table.Views.CustomGridView;
using Mitutoyo.MiCAT.ReportModuleApp.Controllers;
using Moq;
using NUnit.Framework;
using Telerik.Windows.Controls;
using static Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements.Table.Views.CustomGridView.ColumnWidthsChangedEventArgs;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Test.VisualElements.ReportElements.Table
{
   [ExcludeFromCodeCoverage]
   [TestFixture]
   public class VMTableViewTest
   {
      private Mock<ITableViewController> _tableViewControllerMock;
      private Mock<ISelectedComponentController> _selectedComponentController;
      private Mock<ISelectedSectionController> _sectionSelectionControllerMock;
      private Mock<IAppStateHistory> _historyMock;
      private Mock<ISnapShot> _snapShotMock;
      private Mock<IVMPages> _vmPages;
      private Mock<IRenderedData> _renderData;
      private Mock<IDisabledSpaceDataCollection> _disabledSpaces;
      private ReportTableView _reportTableView;
      private ReportComponentPlacement _placement;
      private RunSelection _runSelected;
      private Mock<IBusyIndicator> _busyIndicatorMock;
      private List<EvaluatedCharacteristic> _evaluatedCharacteristicList;
      private Mock<IReportBoundarySectionUpdateReceiver> _reportBoundarySectionUpdateReceiverMock;
      private VMReportBoundarySectionFactory _reportBoundaySectionFactory;
      private VMReportBoundarySection _VMReportFooter;
      private VMReportBoundarySection _VMReportHeader;
      private IReportModeProperty _reportModeProperty;
      private Mock<IImageController> _imageControllerMock;
      private Mock<ITextBoxController> _textboxControllerMock;
      private Mock<IHeaderFormController> _headerFormControllerMock;
      private Mock<IDeleteComponentController> _deleteComponentControllerMock;
      private Mock<IReportComponentPlacementController> _placementControllerMock;
      private IVMDisableSpacePlacement _vmPlacement;

      [SetUp]
      public void SetUp()
      {
         _textboxControllerMock = new Mock<ITextBoxController>();
         _headerFormControllerMock = new Mock<IHeaderFormController>();
         _tableViewControllerMock = new Mock<ITableViewController>();
         _selectedComponentController = new Mock<ISelectedComponentController>();
         _sectionSelectionControllerMock = new Mock<ISelectedSectionController>();
         _busyIndicatorMock = new Mock<IBusyIndicator>();
         _reportBoundarySectionUpdateReceiverMock = new Mock<IReportBoundarySectionUpdateReceiver>();
         _reportBoundarySectionUpdateReceiverMock.Setup(u => u.IsInEditMode).Returns(true);
         _imageControllerMock = new Mock<IImageController>();
         _placementControllerMock = new Mock<IReportComponentPlacementController>();

         _busyIndicatorMock.Setup(b => b.SetIsBusyTrueUntilUIIsIdle()).Callback(() => { });
         _deleteComponentControllerMock = new Mock<IDeleteComponentController>();

         _historyMock = new Mock<IAppStateHistory>();
         _historyMock.Setup(h => h.AddClient(It.IsAny<ISubscriptionClient>(), 0)).Returns(_historyMock.Object);
         _snapShotMock = new Mock<ISnapShot>();
         _vmPages = new Mock<IVMPages>();
         _renderData = new Mock<IRenderedData>();
         _renderData.Setup(rd => rd.Pages).Returns(_vmPages.Object);
         _reportModeProperty = new ReportModeProperty(_historyMock.Object);

         _disabledSpaces = new Mock<IDisabledSpaceDataCollection>();

         var reportBody = new ReportBody();
         _placement = new ReportComponentPlacement(reportBody.Id, 10, 10, 20, 20);
         _reportTableView = new ReportTableView(_placement).WithDefaultColumns();

         var changeList = new List<UpdateItemChange>() { new UpdateItemChange(_reportTableView.Id) };
         _snapShotMock.Setup(h => h.GetChanges()).Returns(new Changes(changeList.ToImmutableList<IItemChange>()));
         _snapShotMock.Setup(s => s.GetItem<ReportTableView>(_reportTableView.Id as IItemId)).Returns(_reportTableView);
         _snapShotMock.Setup(s => s.GetItem<IReportComponent>(_reportTableView.Id)).Returns(_reportTableView);
         _snapShotMock.Setup(s => s.GetItem<IReportComponent>(_reportTableView.Id as IItemId)).Returns(_reportTableView);
         _snapShotMock.Setup(s => s.GetItems<ReportModeState>()).Returns(new List<ReportModeState>() { new ReportModeState() });
         _snapShotMock.Setup(s => s.GetItems<AllCharacteristicTypes>()).Returns(new AllCharacteristicTypes[] { new AllCharacteristicTypes(ImmutableList.Create<string>()) });
         _snapShotMock.Setup(s => s.GetItems<AllCharacteristicDetails>()).Returns(new AllCharacteristicDetails[] { new AllCharacteristicDetails(ImmutableList.Create<string>()) });
         _snapShotMock.Setup(s => s.GetItems<ReportBody>()).Returns(new ReportBody[] { reportBody });
         _snapShotMock.Setup(s => s.GetItems<ReportHeader>()).Returns(new ReportHeader[] { new ReportHeader() });
         _snapShotMock.Setup(s => s.GetItems<ReportFooter>()).Returns(new ReportFooter[] { new ReportFooter() });

         _historyMock.Setup(h => h.CurrentSnapShot).Returns(_snapShotMock.Object);

         _evaluatedCharacteristicList = GetEvaluatedCharacteristicList(4, "Name");
         var evaluatedCharacteristicsStringList = ImmutableList.Create(_evaluatedCharacteristicList.Select(x => x.Characteristic.Name.ToString()).ToArray());
         MockResultState(_evaluatedCharacteristicList);

         _reportBoundaySectionFactory = new VMReportBoundarySectionFactory(_historyMock.Object, _reportBoundarySectionUpdateReceiverMock.Object, _imageControllerMock.Object, _textboxControllerMock.Object, _headerFormControllerMock.Object, _selectedComponentController.Object, _sectionSelectionControllerMock.Object);
         _VMReportFooter = _reportBoundaySectionFactory.CreateForFooter(5, 1);
         _VMReportHeader = _reportBoundaySectionFactory.CreateForHeader(5, 1);

         _vmPlacement = new VMDisableSpacePlacement(_reportTableView.Id, _placement, _placementControllerMock.Object, _selectedComponentController.Object, _renderData.Object);
      }

      [Test]
      public void VMTable_ShouldInitializeCommands()
      {
         var vmTableView = new VMTable(
            _historyMock.Object,
            _reportTableView.Id,
            _vmPlacement,
            _deleteComponentControllerMock.Object,
            _tableViewControllerMock.Object);

         Assert.IsNotNull(vmTableView);
         Assert.IsNotNull(vmTableView.ColumnReorderedCommand);
         Assert.IsNotNull(vmTableView.ColumnWidthsChangedCommand);
         Assert.IsNotNull(vmTableView.FilterCommand);
         Assert.IsNotNull(vmTableView.HeaderCommand);
         Assert.IsNotNull(vmTableView.SortCommand);
      }

      [Test]
      public void VMTable_ShouldGetFeatureNameString()
      {
         //Arrange and Act
         var vmTableView = new VMTable(
            _historyMock.Object,
            _reportTableView.Id,
            _vmPlacement,
            _deleteComponentControllerMock.Object,
            _tableViewControllerMock.Object);

         //Assert
         Assert.AreEqual(vmTableView.ColumnInfos[0].CaptionText(), DataServiceLocalizationFinder.FindLocalizedString("FeatureName"));
      }

      [Test]
      public void VMTable_ShouldGetTypeString()
      {
         //Arrange and Act
         var vmTableView = new VMTable(
            _historyMock.Object,
            _reportTableView.Id,
            _vmPlacement,
            _deleteComponentControllerMock.Object,
            _tableViewControllerMock.Object);

         //Assert
         Assert.AreEqual(vmTableView.ColumnInfos[1].CaptionText(), DataServiceLocalizationFinder.FindLocalizedString("CharacteristicType"));
      }

      [Test]
      public void VMTable_ConstructorShouldInitializeSourceWithList()
      {
         //Arrange

         //Act
         var vmTableView = new VMTable(
            _historyMock.Object,
            _reportTableView.Id,
            _vmPlacement,
            _deleteComponentControllerMock.Object,
            _tableViewControllerMock.Object);

         //Assert
         Assert.AreEqual(vmTableView.TotalDataToDisplay.Count(), _runSelected.SelectedRunData.CharacteristicList.Count());
      }

      [Test]
      public void VMTable_InitializeATableViewWithOneViewModel()
      {
         //Arrange
         var vmTableView = new VMTable(
            _historyMock.Object,
            _reportTableView.Id,
            _vmPlacement,
            _deleteComponentControllerMock.Object,
            _tableViewControllerMock.Object);

         //Act
         vmTableView.UpdateNewDataSource(_snapShotMock.Object);

         //Assert
         Assert.AreEqual(vmTableView.TotalDataToDisplay.Count(), _runSelected.SelectedRunData.CharacteristicList.Count());
      }

      [Test]
      public void VMTable_ApplyHeaderEvents_Ascending()
      {
         //Arrange
         var vmTableView = new VMTable(
            _historyMock.Object,
            _reportTableView.Id,
            _vmPlacement,
            _deleteComponentControllerMock.Object,
            _tableViewControllerMock.Object);
         vmTableView.UpdateNewDataSource(_snapShotMock.Object);

         //Act
         vmTableView.ApplyHeaderEvents(new GridViewColumn { UniqueName = "CharacteristicName" }, InteractiveReportGridView.DescriptorOption.SortAscending);

         //Assert
         _tableViewControllerMock.Verify(t => t.SortByAddColumn(vmTableView.Id as Id<ReportTableView>, It.Is<SortingColumn>(sc => sc.ColumnName == "CharacteristicName" && sc.Mode == SortingMode.Ascending)));
      }

      [Test]
      public void VMTable_ApplyHeaderEvents_Descending()
      {
         //Arrange
         var vmTableView = new VMTable(
            _historyMock.Object,
            _reportTableView.Id,
            _vmPlacement,
            _deleteComponentControllerMock.Object,
            _tableViewControllerMock.Object);
         vmTableView.UpdateNewDataSource(_snapShotMock.Object);

         //Act
         vmTableView.ApplyHeaderEvents(new GridViewColumn { UniqueName = "CharacteristicName" }, InteractiveReportGridView.DescriptorOption.SortDescending);

         //Assert
         _tableViewControllerMock.Verify(t => t.SortByAddColumn(vmTableView.Id as Id<ReportTableView>, It.Is<SortingColumn>(sc => sc.ColumnName == "CharacteristicName" && sc.Mode == SortingMode.Descending)));
      }

      [Test]
      public void VMTable_ApplyHeaderEvents_GroupBy()
      {
         //Arrange
         var vmTableView = new VMTable(
            _historyMock.Object,
            _reportTableView.Id,
            _vmPlacement,
            _deleteComponentControllerMock.Object,
            _tableViewControllerMock.Object);
         vmTableView.UpdateNewDataSource(_snapShotMock.Object);

         //Act
         vmTableView.ApplyHeaderEvents(new GridViewColumn { UniqueName = "CharacteristicType" }, InteractiveReportGridView.DescriptorOption.GroupBy);

         //Assert
         Assert.AreEqual(vmTableView.TotalDataToDisplay.ToArray()[2].CharacteristicType, "Circularity");
         Assert.AreEqual(vmTableView.TotalDataToDisplay.ToArray()[3].CharacteristicType, "Circularity");
      }

      private List<EvaluatedCharacteristic> GetEvaluatedCharacteristicList(int evaluatedCharacteristicCount, string characteristicNamePrefix)
      {
         var listCharacteristics = new List<EvaluatedCharacteristic>();

         for (int i = 0; i < evaluatedCharacteristicCount; i++)
         {
            Guid id = Guid.NewGuid();
            listCharacteristics.Add(new EvaluatedCharacteristic(new Characteristic(id, $"{characteristicNamePrefix}{i + 1}", "Circularity"), new CharacteristicActual(id, "Pass", null, null)));
         }

         return listCharacteristics;
      }

      private List<VMEvaluatedCharacteristic> GetVMEvaluatedCharacteristicList(List<EvaluatedCharacteristic> evaluatedCharacteristicList)
      {
         return evaluatedCharacteristicList.Select(x => new VMEvaluatedCharacteristic(x)).ToList();
      }

      private void MockResultState(List<EvaluatedCharacteristic> evaluatedCharacteristicList)
      {
         var runData = new RunData(DateTime.Now, Guid.NewGuid(), 0, evaluatedCharacteristicList.ToImmutableList(), ImmutableList<DynamicPropertyValue>.Empty, ImmutableList<Capture3D>.Empty);
         _runSelected = new RunSelection(Guid.NewGuid(), runData);
         _snapShotMock.Setup(s => s.GetItems<RunSelection>()).Returns(new List<RunSelection>() { _runSelected });
      }

      [Test]
      public void VMTable_RegeneratePiecesWithZeroRows()
      {
         // Arrange
         var pageLayoutForTest = new CommonPageLayout(new PageSizeInfo(PaperKind.Custom, 1000, 500), new Margin(MarginKind.Narrow, 0, 48, 0, 48));
         var evaluatedCharacteristicList = GetEvaluatedCharacteristicList(0, string.Empty);
         MockResultState(evaluatedCharacteristicList);

         var sut = new VMTableFake(_historyMock.Object, _reportTableView.Id, _vmPlacement, _deleteComponentControllerMock.Object, _tableViewControllerMock.Object);
         VMPage tablePage = new VMPage(pageLayoutForTest, _reportModeProperty, _VMReportHeader, _VMReportFooter);

         _vmPages.Setup(pg => pg.GetPageForPosition(_vmPlacement.VisualY)).Returns(tablePage);
         sut.UpdateNewDataSource(_snapShotMock.Object);

         // Act
         sut.RegeneratePieces();
         // Arrange
         Assert.AreEqual(sut.Pieces.Count(), 0);
      }

      [Test]
      public void VMTableView_UpdateDataWithNoPieces()
      {
         // Arrange
         var pageLayoutForTest = new CommonPageLayout(new PageSizeInfo(PaperKind.Custom, 500, 500), new Margin(MarginKind.Narrow, 0, 48, 0, 48));
         var evaluatedCharacteristicList = GetEvaluatedCharacteristicList(10, "Name");
         MockResultState(evaluatedCharacteristicList);

         var vmTableView = new VMTable(
            _historyMock.Object,
            _reportTableView.Id,
            _vmPlacement,
            _deleteComponentControllerMock.Object,
            _tableViewControllerMock.Object);

         VMPage tablePage = new VMPage(pageLayoutForTest, _reportModeProperty, _VMReportHeader, _VMReportFooter);
         vmTableView.UpdateNewDataSource(_snapShotMock.Object);

         _vmPages.Setup(pg => pg.GetPageForPosition(_vmPlacement.VisualY)).Returns(tablePage);

         // Act
         vmTableView.RegeneratePieces();

         // Assert
         Assert.AreEqual(0, vmTableView.Pieces.Count());
         Assert.AreEqual("Name1", vmTableView.DataToDisplay.ToList()[0].CharacteristicName);
      }

      [Test]
      [TestCase(200, 1000, 6, 2, "Name88")]
      [TestCase(600, 2500, 7, 5, "Name487")]
      [TestCase(1000, 2000, 15, 5, "Name385")]
      public void VMTableView_RegeneratePieces(int rowsCount, int pageHeight, int expectedPiecesCount, int pieceIndexToTest, string firstCharNameOnPiece)
      {
         // Arrange
         var pageLayoutForTest = new CommonPageLayout(new PageSizeInfo(PaperKind.Custom, pageHeight, 500), new Margin(MarginKind.Narrow, 0, 48, 0, 48));
         var evaluatedCharacteristicList = GetEvaluatedCharacteristicList(rowsCount, "Name");
         MockResultState(evaluatedCharacteristicList);

         var vmTableView = new VMTable(
            _historyMock.Object,
            _reportTableView.Id,
            _vmPlacement,
            _deleteComponentControllerMock.Object,
            _tableViewControllerMock.Object);

         VMPage tablePage = new VMPage(pageLayoutForTest, _reportModeProperty, _VMReportHeader, _VMReportFooter);
         vmTableView.UpdateNewDataSource(_snapShotMock.Object);
         _vmPages.Setup(pg => pg.GetPageForPosition(_vmPlacement.VisualY)).Returns(tablePage);

         // Act
         vmTableView.RegeneratePieces();
         var tablePiece = vmTableView.Pieces[pieceIndexToTest] as VMTablePiece;

         // Assert
         Assert.AreEqual(vmTableView.Pieces.Count(), expectedPiecesCount);
         Assert.AreEqual(tablePiece.DataToDisplay.ToList()[0].CharacteristicName, firstCharNameOnPiece);
      }

      [Test]
      public void VMTable_SetNewDataSourceWithRenderChange()
      {
         // Arrange
         var pageLayoutForTest = new CommonPageLayout(new PageSizeInfo(PaperKind.Custom, 1000, 500), new Margin(MarginKind.Narrow, 0, 48, 0, 48));
         var evaluatedCharacteristicList = GetEvaluatedCharacteristicList(150, "Name");
         MockResultState(evaluatedCharacteristicList);

         var vmTableView = new VMTableFake(
            _historyMock.Object,
            _reportTableView.Id,
            _vmPlacement,
            _deleteComponentControllerMock.Object,
            _tableViewControllerMock.Object);
         VMPage tablePage = new VMPage(pageLayoutForTest, _reportModeProperty, _VMReportHeader, _VMReportFooter);
         vmTableView.UpdateNewDataSource(_snapShotMock.Object);
         var layoutChangedEventLogger = new ReportComponentLayoutChangedLogger(_vmPlacement);
         _vmPages.Setup(pg => pg.GetPageForPosition(_vmPlacement.VisualY)).Returns(tablePage);

         vmTableView.RegeneratePieces();
         var updatedEvaluatedCharacteristicList = GetEvaluatedCharacteristicList(200, "UpdatedName");

         // Act
         vmTableView.ExecuteSetNewDataSource(GetVMEvaluatedCharacteristicList(updatedEvaluatedCharacteristicList));

         // Assert
         Assert.IsTrue(layoutChangedEventLogger.ChangeLog.Contains("ReportComponentLayoutChanged Event Called"));
      }

      [Test]
      public void VMTable_SetNewDataSourceWithoutRenderChange()
      {
         // Arrange
         var pageLayoutForTest = new CommonPageLayout(new PageSizeInfo(PaperKind.Custom, 1000, 500), new Margin(MarginKind.Narrow, 0, 48, 0, 48));
         var evaluatedCharacteristicList = GetEvaluatedCharacteristicList(150, "Name");
         MockResultState(evaluatedCharacteristicList);

         var vmTableView = new VMTableFake(
            _historyMock.Object,
            _reportTableView.Id,
            _vmPlacement,
            _deleteComponentControllerMock.Object,
            _tableViewControllerMock.Object);

         VMPage tablePage = new VMPage(pageLayoutForTest, _reportModeProperty, _VMReportHeader, _VMReportFooter);
         vmTableView.UpdateNewDataSource(_snapShotMock.Object);
         var layoutChangedEventLogger = new ReportComponentLayoutChangedLogger(_vmPlacement);
         _vmPages.Setup(pg => pg.GetPageForPosition(_vmPlacement.VisualY)).Returns(tablePage);

         vmTableView.RegeneratePieces();

         var updatedEvaluatedCharacteristicList = GetEvaluatedCharacteristicList(150, "UpdatedName");

         // Act
         vmTableView.ExecuteSetNewDataSource(GetVMEvaluatedCharacteristicList(updatedEvaluatedCharacteristicList));
         var tablePiece = vmTableView.Pieces[0] as VMTablePiece;

         // Assert
         Assert.AreEqual(layoutChangedEventLogger.ChangeLog.Count, 0);
         Assert.AreEqual(vmTableView.Pieces.Count, 5);
         Assert.AreEqual("UpdatedName30", tablePiece.DataToDisplay.ToList()[0].CharacteristicName);
      }

      [Test]
      public void VMTable_SetNewColumnInfoProperty()
      {
         // Arrange
         var pageLayoutForTest = new CommonPageLayout(new PageSizeInfo(PaperKind.Custom, 1000, 500), new Margin(MarginKind.Narrow, 0, 48, 0, 48));
         var evaluatedCharacteristicList = GetEvaluatedCharacteristicList(150, "Name");
         MockResultState(evaluatedCharacteristicList);

         var vmTableView = new VMTableFake(
            _historyMock.Object,
            _reportTableView.Id,
            _vmPlacement,
            _deleteComponentControllerMock.Object,
            _tableViewControllerMock.Object);
         VMPage tablePage = new VMPage(pageLayoutForTest, _reportModeProperty, _VMReportHeader, _VMReportFooter);
         vmTableView.UpdateNewDataSource(_snapShotMock.Object);
         _vmPages.Setup(pg => pg.GetPageForPosition(_vmPlacement.VisualY)).Returns(tablePage);

         vmTableView.RegeneratePieces();

         var columnInfoList = new ColumnInfoList(_reportTableView, new List<VMEvaluatedCharacteristic>(), new List<string>(), new string[] { });
         columnInfoList.RemoveAt(0);
         columnInfoList.RemoveAt(1);
         columnInfoList.RemoveAt(2);

         // Act
         vmTableView.ExecuteSetNewColumnInfoProperty(columnInfoList);

         // Assert
         Assert.AreEqual(columnInfoList, vmTableView.ColumnInfos);
         Assert.AreEqual(columnInfoList, (vmTableView.Pieces[0] as VMTablePiece).ColumnInfos);
         Assert.AreEqual(columnInfoList, (vmTableView.Pieces[1] as VMTablePiece).ColumnInfos);
         Assert.AreEqual(columnInfoList, (vmTableView.Pieces[2] as VMTablePiece).ColumnInfos);
         Assert.AreEqual(columnInfoList, (vmTableView.Pieces[3] as VMTablePiece).ColumnInfos);
      }

      [Test]
      public void VMTable_ShouldCallReorderColumnController()
      {
         //Arrange
         const string COLUMN_NAME_1 = "FeatureName";
         const string COLUMN_NAME_2 = "Nominal";

         const int NEW_POSITION_1 = 0;
         const int NEW_POSITION_2 = 1;

         var vmTableView = new VMTable(
            _historyMock.Object,
            _reportTableView.Id,
            _vmPlacement,
            _deleteComponentControllerMock.Object,
            _tableViewControllerMock.Object);

         //Act
         vmTableView.ColumnReorderedCommand.Execute(new ReorderColumn(COLUMN_NAME_1, NEW_POSITION_1));
         vmTableView.ColumnReorderedCommand.Execute(new ReorderColumn(COLUMN_NAME_2, NEW_POSITION_2));

         // Assert
         _tableViewControllerMock.Verify(t => t.ReorderColumn(
            It.Is<Id<ReportTableView>>(id => id == _reportTableView.Id),
            It.Is<string>(n => n == COLUMN_NAME_1),
            It.Is<int>(i => i == NEW_POSITION_1)
            ), Times.Once);

         _tableViewControllerMock.Verify(t => t.ReorderColumn(
            It.Is<Id<ReportTableView>>(id => id == _reportTableView.Id),
            It.Is<string>(n => n == COLUMN_NAME_2),
            It.Is<int>(i => i == NEW_POSITION_2)
            ), Times.Once);
      }

      [Test]
      public void VMTable_ShouldReturnDisabledSpaceWithTheSamePage()
      {
         //Arrange
         var pageLayoutForTest = new CommonPageLayout(new PageSizeInfo(PaperKind.Custom, 300, 200), new Margin(MarginKind.Narrow, 0, 48, 0, 48));
         var page = new VMPage(pageLayoutForTest, _reportModeProperty, _VMReportHeader, _VMReportFooter);
         page.OffsetForComponentsOnThisPage = 5;

         _vmPages.Setup(pg => pg.GetPageForPosition(_vmPlacement.VisualY)).Returns(page);

         var vmTable = new VMTable(_historyMock.Object, _reportTableView.Id, _vmPlacement, _deleteComponentControllerMock.Object, _tableViewControllerMock.Object);
         _vmPlacement.PageVerticalOffset = page.OffsetForComponentsOnThisPage;
         _vmPages.Setup(p => p.GetPageForPosition(_vmPlacement.VisualY)).Returns(page);

         //Act
         var disabledSpace = vmTable.GetDisabledSpace();

         //Assert
         Assert.AreEqual(50, disabledSpace.StartVisualY);
         Assert.AreEqual(166, disabledSpace.EndVisualY);

         Assert.AreEqual(page.StartVisualY, disabledSpace.StartPage.StartVisualY);
         Assert.AreEqual(page.EndVisualY, disabledSpace.StartPage.EndVisualY);
         Assert.AreEqual(page.StartDomainY, disabledSpace.StartPage.StartDomainY);
         Assert.AreEqual(page.EndDomainY, disabledSpace.StartPage.EndDomainY);

         Assert.AreEqual(page.StartVisualY, disabledSpace.EndPage.StartVisualY);
         Assert.AreEqual(page.EndVisualY, disabledSpace.EndPage.EndVisualY);
         Assert.AreEqual(page.StartDomainY, disabledSpace.EndPage.StartDomainY);
         Assert.AreEqual(page.EndDomainY, disabledSpace.EndPage.EndDomainY);
      }

      [Test]
      public void VMTable_ShouldReturnDisabledSpaceWithTwoPages()
      {
         //Arrange
         var margins = new Margin(MarginKind.Narrow, 0, 48, 0, 48);
         var commonPageLayout = new CommonPageLayout(new PageSizeInfo(PaperKind.Custom, 500, 300), margins, new HeaderData(5), new FooterData(5));

         var vmPages = new VMPages(_reportBoundaySectionFactory, _reportBoundarySectionUpdateReceiverMock.Object, _reportModeProperty);

         var page1 = vmPages.NextPage(commonPageLayout);
         var page2 = vmPages.NextPage(page1, commonPageLayout);

         var multiPageElementManager = new MultiPageElementManager(new OnePiecePerPageAllocator());
         var elementListMock = new Mock<IVMReportElementList>();
         var renderContent = new RenderedData(_disabledSpaces.Object, vmPages);
         var vmPlacement = new VMDisableSpacePlacement(_reportTableView.Id, _placement, _placementControllerMock.Object, _selectedComponentController.Object, renderContent);

         _snapShotMock
            .Setup(s => s.GetItem(_reportTableView.Id as IItemId<IReportComponent>))
           .Returns(_reportTableView.WithPosition(0, 380));

         var vmTable = new VMTable(_historyMock.Object, _reportTableView.Id, vmPlacement, _deleteComponentControllerMock.Object, _tableViewControllerMock.Object);
         vmPlacement.PageVerticalOffset = page1.OffsetForComponentsOnThisPage;

         vmTable.RegeneratePieces();

         multiPageElementManager.RenderMultiPageElement(vmTable, commonPageLayout, elementListMock.Object, renderContent);

         //Act
         var disabledSpace = vmTable.GetDisabledSpace();

         //Assert
         Assert.AreEqual(483, disabledSpace.StartVisualY);
         Assert.AreEqual(739, disabledSpace.EndVisualY);

         Assert.AreEqual(page1.StartVisualY, disabledSpace.StartPage.StartVisualY);
         Assert.AreEqual(page1.EndVisualY, disabledSpace.StartPage.EndVisualY);
         Assert.AreEqual(page1.StartDomainY, disabledSpace.StartPage.StartDomainY);
         Assert.AreEqual(page1.EndDomainY, disabledSpace.StartPage.EndDomainY);

         Assert.AreEqual(page2.StartVisualY, disabledSpace.EndPage.StartVisualY);
         Assert.AreEqual(page2.EndVisualY, disabledSpace.EndPage.EndVisualY);
         Assert.AreEqual(page2.StartDomainY, disabledSpace.EndPage.StartDomainY);
         Assert.AreEqual(page2.EndDomainY, disabledSpace.EndPage.EndDomainY);
      }

      [Test]
      public void VMTable_IsDisabledSpaceSaved()
      {
         var pageLayoutForTest = new CommonPageLayout(new PageSizeInfo(PaperKind.Custom, 1000, 100), new Margin(MarginKind.Narrow, 0, 48, 0, 48));
         _snapShotMock
          .Setup(s => s.GetItem(_reportTableView.Id as IItemId<IReportComponent>))
          .Returns(_reportTableView.WithPosition(0, 45));

         //Arrange
         var vmTable = new VMTable(_historyMock.Object, _reportTableView.Id, _vmPlacement, _deleteComponentControllerMock.Object, _tableViewControllerMock.Object);
         _vmPlacement.PageVerticalOffset = 5;
         _vmPages.Setup(p => p.GetPageForPosition(It.Is<int>(y => y == _vmPlacement.VisualY))).Returns(new VMPage(pageLayoutForTest, _reportModeProperty, _VMReportHeader, _VMReportFooter));

         //Act
         var disabledSpaceGenerated = vmTable.GetDisabledSpace();

         //Assert
         Assert.AreEqual(disabledSpaceGenerated, _vmPlacement.LastDisabledSpaceGenerated);
      }

      [Test]

      public void VMTable_UpdateValuesForDisabledSpaceMustSet100()
      {
         var pageLayoutForTest = new CommonPageLayout(new PageSizeInfo(PaperKind.Custom, 1000, 100), new Margin(MarginKind.Narrow, 0, 48, 0, 48));
         _snapShotMock
            .Setup(s => s.GetItem<IReportComponent>(_reportTableView.Id as IItemId))
          .Returns(_reportTableView.WithPosition(0, 45));

         //Arrange
         var vmTable = new VMTable(_historyMock.Object, _reportTableView.Id, _vmPlacement, _deleteComponentControllerMock.Object, _tableViewControllerMock.Object);
         _vmPlacement.PageVerticalOffset = 5;
         _vmPages.Setup(p => p.GetPageForPosition(It.Is<int>(y => y == _vmPlacement.VisualY))).Returns(new VMPage(pageLayoutForTest, _reportModeProperty, _VMReportHeader, _VMReportFooter));

         //Act
         var disabledSpaceGenerated = vmTable.GetDisabledSpace();

         //Assert
         Assert.AreEqual(116, disabledSpaceGenerated.UsableSpaceTaken);
      }

      [Test]
      [TestCase(4, 116)]
      [TestCase(10, 290)]
      [TestCase(50, 1500)]
      [TestCase(1000, 30170)]
      public void VMTable_UpdateValuesForDisabledSpaceMustBeValid(int tableRowCount, int expectedUsableSpaceTaken)
      {
         //Arrange
         var evaluatedCharacteristicList = GetEvaluatedCharacteristicList(tableRowCount, string.Empty);
         MockResultState(evaluatedCharacteristicList);

         var margins = new Margin(MarginKind.Narrow, 0, 48, 0, 48);
         var commonPageLayout = new CommonPageLayout(new PageSizeInfo(PaperKind.A4, 1122, 794), margins, new HeaderData(5), new FooterData(5));

         var vmPages = new VMPages(_reportBoundaySectionFactory, _reportBoundarySectionUpdateReceiverMock.Object, _reportModeProperty);

         var page1 = vmPages.NextPage(commonPageLayout);

         var multiPageElementManager = new MultiPageElementManager(new OnePiecePerPageAllocator());
         var elementListMock = new Mock<IVMReportElementList>();
         var renderContent = new RenderedData(_disabledSpaces.Object, vmPages);

         _snapShotMock
            .Setup(s => s.GetItem<IReportComponent>(_reportTableView.Id))
            .Returns(_reportTableView.WithPosition(0, 400));

         var vmTable = new VMTable(_historyMock.Object, _reportTableView.Id, _vmPlacement, _deleteComponentControllerMock.Object, _tableViewControllerMock.Object);
         _vmPlacement.PageVerticalOffset = 5;
         _vmPages.Setup(pg => pg.GetPageForPosition(_vmPlacement.VisualY)).Returns(page1);
         vmTable.RegeneratePieces();

         multiPageElementManager.RenderMultiPageElement(vmTable, commonPageLayout, elementListMock.Object, renderContent);

         //Act
         var disabledSpace = vmTable.GetDisabledSpace();

         //Assert
         Assert.AreEqual(expectedUsableSpaceTaken, disabledSpace.UsableSpaceTaken);
      }

      [Test]
      public void VMTable_GeneratedDisabledSpaceValuesShouldBeValid()
      {
         //Arrange
         var margins = new Margin(MarginKind.Narrow, 0, 48, 0, 48);
         var commonPageLayout = new CommonPageLayout(new PageSizeInfo(PaperKind.A4, 1122, 794), margins, new HeaderData(5), new FooterData(5));

         var vmPages = new VMPages(_reportBoundaySectionFactory, _reportBoundarySectionUpdateReceiverMock.Object, _reportModeProperty);

         var page1 = vmPages.NextPage(commonPageLayout);

         var multiPageElementManager = new MultiPageElementManager(new OnePiecePerPageAllocator());
         var elementListMock = new Mock<IVMReportElementList>();
         var renderContent = new RenderedData(_disabledSpaces.Object, vmPages);

         _snapShotMock
            .Setup(s => s.GetItem<IReportComponent>(_reportTableView.Id))
            .Returns(_reportTableView.WithPosition(0, 400));

         var vmTable = new VMTable(_historyMock.Object, _reportTableView.Id, _vmPlacement, _deleteComponentControllerMock.Object, _tableViewControllerMock.Object);
         _vmPlacement.PageVerticalOffset = 5;
         _vmPages.Setup(pg => pg.GetPageForPosition(_vmPlacement.VisualY)).Returns(page1);
         vmTable.RegeneratePieces();

         multiPageElementManager.RenderMultiPageElement(vmTable, commonPageLayout, elementListMock.Object, renderContent);

         //Act
         var disabledSpace = vmTable.GetDisabledSpace();

         //Assert
         Assert.AreEqual(440, disabledSpace.StartVisualY);
         Assert.AreEqual(556, disabledSpace.EndVisualY);
         Assert.AreEqual(435, disabledSpace.StartDomainY);
         Assert.AreEqual(116, disabledSpace.UsableSpaceTaken);

         Assert.AreEqual(68, disabledSpace.StartPage.StartVisualY);
         Assert.AreEqual(0, disabledSpace.StartPage.StartDomainY);
         Assert.AreEqual(1093, disabledSpace.StartPage.EndVisualY);
         Assert.AreEqual(1025, disabledSpace.StartPage.EndDomainY);

         Assert.AreEqual(68, disabledSpace.EndPage.StartVisualY);
         Assert.AreEqual(0, disabledSpace.EndPage.StartDomainY);
         Assert.AreEqual(1093, disabledSpace.EndPage.EndVisualY);
         Assert.AreEqual(1025, disabledSpace.EndPage.EndDomainY);
      }

      [Test]
      public void VMTable_ColumnWidthsChangedCommand_ShouldCallController()
      {
         var vmTable = new VMTable(_historyMock.Object, _reportTableView.Id, _vmPlacement, _deleteComponentControllerMock.Object, _tableViewControllerMock.Object);

         var columnWidthInfos = new[]
         {
            new ColumnWidthInfo("ColumnName0", 100),
            new ColumnWidthInfo("ColumnName1", 101),
            new ColumnWidthInfo("ColumnName2", 102),
            new ColumnWidthInfo("ColumnName3", 103),
         };

         vmTable.ColumnWidthsChangedCommand.Execute(new ColumnWidthsChangedEventArgs(columnWidthInfos));

         _tableViewControllerMock
            .Verify(x => x.ResizeColumnWidths(
               It.Is<Id<ReportTableView>>(i => i == _reportTableView.Id),
               It.Is<IEnumerable<KeyValuePair<string, double>>>(i => i.Count() == columnWidthInfos.Count())));
      }

      [Test]
      public void VMTable_Should_UseNormalDisplayMode_WhenReportIsOnViewMode()
      {
         var vmTable = new VMTable(_historyMock.Object, _reportTableView.Id, _vmPlacement, _deleteComponentControllerMock.Object, _tableViewControllerMock.Object);

         Assert.AreEqual(RenderMode.ViewMode, vmTable.RenderMode);
         Assert.AreEqual(DisplayMode.Normal, vmTable.DisplayMode);
      }

      [Test]
      public void VMTable_Should_UsePlacementDisplayMode_WhenReportIsOnEditMode()
      {
         _snapShotMock
            .Setup(x => x.GetItems<ReportModeState>())
            .Returns(new[] { new ReportModeState(true) });

         _snapShotMock
            .Setup(x => x.GetItems<ReportComponentSelection>())
            .Returns(new[] { new ReportComponentSelection() });
         var vmTable = new VMTable(_historyMock.Object, _reportTableView.Id, _vmPlacement, _deleteComponentControllerMock.Object, _tableViewControllerMock.Object);

         Assert.AreEqual(RenderMode.EditMode, vmTable.RenderMode);
         Assert.AreEqual(DisplayMode.Placement, vmTable.DisplayMode);
      }

      [Test]
      public void VMTable_EmptyTable_ShoulSetVisualHeight_AsHeaderHeight()
      {
         _ = new VMTable(_historyMock.Object, _reportTableView.Id, _vmPlacement, _deleteComponentControllerMock.Object, _tableViewControllerMock.Object);

         Assert.AreEqual(35, _vmPlacement.VisualHeight);
      }

      [Test]
      public void VMTable_EmptyTable_ShoulSetVisualHeight_BasedOnDataToDisplayCount()
      {
         var pageLayoutForTest = new CommonPageLayout(new PageSizeInfo(PaperKind.Custom, 1000, 900), new Margin());
         var vmTable = new VMTable(_historyMock.Object, _reportTableView.Id, _vmPlacement, _deleteComponentControllerMock.Object, _tableViewControllerMock.Object);

         var page = new VMPage(pageLayoutForTest, _reportModeProperty, _VMReportHeader, _VMReportFooter);
         page.StartVisualY = 0;
         _vmPages.Setup(pg => pg.GetPageForPosition(_vmPlacement.VisualY)).Returns(page);

         vmTable.RegeneratePieces();

         Assert.AreEqual(151, _vmPlacement.VisualHeight);
      }
   }
}