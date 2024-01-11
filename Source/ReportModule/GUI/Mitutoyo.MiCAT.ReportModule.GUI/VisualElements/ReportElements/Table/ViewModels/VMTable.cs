// <copyright file="VMTable.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Windows.Input;
using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.Core.StdLib.Extensions;
using Mitutoyo.MiCAT.ReportModule.Domain.Components;
using Mitutoyo.MiCAT.ReportModule.Domain.Components.TableView;
using Mitutoyo.MiCAT.ReportModule.Domain.Data;
using Mitutoyo.MiCAT.ReportModule.GUI.Common.DisabledSpace;
using Mitutoyo.MiCAT.ReportModule.GUI.PageLayout;
using Mitutoyo.MiCAT.ReportModule.GUI.Utilities;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.Pieces;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.Pieces.Table.ViewModels;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements.Table.Views.CustomGridView;
using Mitutoyo.MiCAT.ReportModuleApp.AppState.Extensions;
using Mitutoyo.MiCAT.ReportModuleApp.Controllers;
using Telerik.Windows.Controls;

namespace Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements.Table.ViewModels
{
   //REFACTOR: Class is too large
   public class VMTable : VMReportComponent, IVMMultiPageSplittableElement, IDisabledSpaceGenerator
   {
      private const int HeightHeader = 35;
      private const int HeightEachRow = 29;

      private ColumnInfoList _columnInfos;
      private IImmutableList<VMEvaluatedCharacteristic> _originalSourceData = ImmutableList<VMEvaluatedCharacteristic>.Empty;
      private IImmutableList<VMEvaluatedCharacteristic> _totalDataToDisplay = ImmutableList<VMEvaluatedCharacteristic>.Empty;
      private IImmutableList<VMEvaluatedCharacteristic> _dataToDisplay = ImmutableList<VMEvaluatedCharacteristic>.Empty;
      private ITableViewController _tableViewController;
      private IEnumerable<string> _characteristicTypes;
      private IEnumerable<string> _details;

      public VMTable(
         IAppStateHistory appStateHistory,
         Id<ReportTableView> tableViewId,
         IVMDisableSpacePlacement vmPlacement,
         IDeleteComponentController deleteComponentController,
         ITableViewController tableViewController)
         : base(
            appStateHistory,
            tableViewId,
            vmPlacement,
            deleteComponentController,
            new TypeFilter[] { new TypeFilter(typeof(RunSelection)) })
      {
         _tableViewController = tableViewController;

         VMPlacement.MinWidth = 300;
         VMPlacement.ResizeType = ResizeType.None;

         UpdateDisplayMode();
         InitializeCommands();
      }
      new private IVMDisableSpacePlacement VMPlacement => base.VMPlacement as IVMDisableSpacePlacement;
      new private Id<ReportTableView> Id => base.Id as Id<ReportTableView>;

      public IImmutableList<VMEvaluatedCharacteristic> TotalDataToDisplay { get => _totalDataToDisplay; }
      public IImmutableList<VMEvaluatedCharacteristic> DataToDisplay
      {
         get => _dataToDisplay;
         private set
         {
            _dataToDisplay = value;
            RaisePropertyChanged();
            UpdateVisualHeightBasedOnDataToDisplay();
         }
      }
      public ColumnInfoList ColumnInfos
      {
         get => _columnInfos;
         private set
         {
            _columnInfos = value;
            RaisePropertyChanged();
         }
      }
      public ICommand SortCommand { get; set; }
      public ICommand HeaderCommand { get; set; }
      public IImmutableList<IVMVisualElementPiece> Pieces { get; private set; } = ImmutableList<IVMVisualElementPiece>.Empty;
      public ICommand FilterCommand { get; set; }
      public ICommand ColumnReorderedCommand { get; set; }
      public ICommand ColumnWidthsChangedCommand { get; set; }

      public void RegeneratePieces()
      {
         var newPieces = new List<IVMVisualElementPiece>();
         var actualPage = GetMainElementPage();
         var pixelsLeftOnThisPage = actualPage.SpaceLeftOnThisPage(VMPlacement.VisualY);

         if (PixelsNeededForSource() <= pixelsLeftOnThisPage)
         {
            DataToDisplay = _totalDataToDisplay;
         }
         else
         {
            int rowsPerPage = AmountOfRowsThatFitsOnHeight(actualPage.UsableSpaceHeight());
            int actualFirstRowIndex;
            DataToDisplay = _totalDataToDisplay.Take(AmountOfRowsThatFitsOnHeight(pixelsLeftOnThisPage)).ToImmutableList();

            actualFirstRowIndex = DataToDisplay.Count();

            double rowsToRender = _totalDataToDisplay.Count - actualFirstRowIndex;
            var piecesCount = (int)Math.Ceiling(rowsToRender / rowsPerPage) + 1;
            var pieceIndex = 2;

            while (actualFirstRowIndex < _totalDataToDisplay.Count())
            {
               var newTablePiece = new VMTablePiece(new VMVisualPlacement(), this);

               newTablePiece.ColumnInfos = ColumnInfos;
               newTablePiece.VMPlacement.SetVisualSize(VMPlacement.DomainWidth, 0);
               newTablePiece.DataToDisplay = _totalDataToDisplay.Skip(actualFirstRowIndex).Take(rowsPerPage);
               newTablePiece.PieceIndex = pieceIndex++;
               newTablePiece.PiecesCount = piecesCount;
               newTablePiece.SetRenderMode(RenderMode);
               newTablePiece.SetDisplayMode(DisplayMode);

               actualFirstRowIndex += rowsPerPage;

               newPieces.Add(newTablePiece);
            }
         }

         Pieces = newPieces.ToImmutableList();
      }

      private int PixelsNeededForSource()
      {
         if (_totalDataToDisplay == null)
            return HeightHeader;
         else
            return HeightHeader + (_totalDataToDisplay.Count() * HeightEachRow);
      }

      private int AmountOfRowsThatFitsOnHeight(int totalHeight)
      {
         return (totalHeight - HeightHeader) / HeightEachRow;
      }

      private void InitializeCommands()
      {
         SortCommand = new RelayCommand<ColumnSortEvent>(OnSortingTable);
         HeaderCommand = new RelayCommand<CustomHeadersFiltersChangeEventArgs>(OnHeaderEventsCommand);
         FilterCommand = new RelayCommand<CustomFilterInfoEventArgs>(OnFilterEventsCommand);
         ColumnReorderedCommand = new RelayCommand<ReorderColumn>(OnColumnReordered);
         ColumnWidthsChangedCommand = new RelayCommand<ColumnWidthsChangedEventArgs>(OnColumnWidthsChanged);
      }

      private void OnHeaderEventsCommand(CustomHeadersFiltersChangeEventArgs parameters)
      {
         //ActionCaller.RunUIThreadAction(() =>
         //{
            //REFACTOR: CustomHeadersFiltersChangeEventArgs is a view stuff
            var args = parameters;
            args.Handled = true;

            ApplyHeaderEvents(args.Column, args.Descriptor);
         //});
      }

      private void OnFilterEventsCommand(CustomFilterInfoEventArgs parameters)
      {
         //ActionCaller.RunUIThreadAction(() =>
         //{
            //REFACTOR: CustomFilterInfoEventArgs is a view stuff
            var args = parameters;
            args.Handled = true;

            ApplyFilterColumn(args.ColumnName, args.SelectedValues);
         //});
      }

      private void OnSortingTable(ColumnSortEvent parameters)
      {
         //ActionCaller.RunUIThreadAction(() =>
         SortingTables(parameters);
         //);
      }

      private void OnColumnReordered(ReorderColumn columnReorderEvent)
      {
         //ActionCaller.RunUIThreadAction(() =>
         ReorderColumn(columnReorderEvent);
         //);
      }

      private void OnColumnWidthsChanged(ColumnWidthsChangedEventArgs args)
      {
         //ActionCaller.RunUIThreadAction(() =>
         // {
             var columnWidths = args.ColumnWidthInfos.Select(x => new KeyValuePair<string, double>(x.ColumnName, x.Width));

             _tableViewController.ResizeColumnWidths(Id, columnWidths);
          //});
      }

      protected override void OnUpdate(ISnapShot snapShot)
      {
         base.OnUpdate(snapShot);
         UpdateDisplayMode();
         UpdateSourceFromResultSetIfChanged(snapShot);
      }

      public override void SetDisplayMode(DisplayMode newDisplayMode)
      {
         base.SetDisplayMode(newDisplayMode);
         Pieces.ForEach(p => ((VMTablePiece)p).SetDisplayMode(newDisplayMode));
      }
      public override void SetRenderMode(RenderMode newRenderMode)
      {
         base.SetRenderMode(newRenderMode);
         Pieces.ForEach(p => ((VMTablePiece)p).SetRenderMode(newRenderMode));
      }

      protected void UpdateSourceFromResultSetIfChanged(ISnapShot snapShot)
      {
         if (snapShot.GetChanges().Where(c => c.ItemType == typeof(RunSelection)).Any())
         {
            UpdateNewDataSource(snapShot);
         }
      }
      protected void UpdateSourceFromResultSet(ISnapShot snapShot)
      {
         UpdateNewDataSource(snapShot);
      }

      private void UpdateDisplayMode()
      {
         var newDisplayMode = RenderMode == RenderMode.EditMode ? DisplayMode.Placement : DisplayMode.Normal;
         SetDisplayMode(newDisplayMode);
      }

      protected override void InitializeFromSnapShot(ISnapShot snapShot)
      {
         _characteristicTypes = snapShot.GetCharacteristicTypes();
         _details = snapShot.GetCharacteristicDetails();

         base.InitializeFromSnapShot(snapShot);

         UpdateSourceFromResultSet(snapShot);
      }

      protected override void UpdateFromBusinessEntity(IReportComponent reportComponentBefore, IReportComponent reportComponentAfter)
      {
         base.UpdateFromBusinessEntity(reportComponentBefore, reportComponentAfter);

         var reportTableView = reportComponentAfter as ReportTableView;
         var newColumnInfos = new ColumnInfoList(reportTableView, _originalSourceData, _characteristicTypes, _details);

         if (!newColumnInfos.Equals(ColumnInfos))
         {
            SetNewColumnInfoProperty(newColumnInfos);

            SetNewDataSource(ColumnInfos.Update(_originalSourceData));
         }
      }

      public void UpdateNewDataSource(ISnapShot snapShot)
      {
         var runSelected = snapShot.GetItems<RunSelection>().Single();

         if (runSelected.SelectedRunData != null)
            _originalSourceData = runSelected.SelectedRunData.CharacteristicList.Select(x => new VMEvaluatedCharacteristic(x)).ToImmutableList();
         else
            _originalSourceData = ImmutableList<VMEvaluatedCharacteristic>.Empty;

         SetNewDataSource(ColumnInfos?.Update(_originalSourceData)?.ToList());
      }

      public void ApplyHeaderEvents(GridViewColumn column, InteractiveReportGridView.DescriptorOption descriptor)
      {
         switch (descriptor)
         {
            case InteractiveReportGridView.DescriptorOption.SortAscending:
               ApplySortMultiple(SortingMode.Ascending, column.UniqueName);
               break;
            case InteractiveReportGridView.DescriptorOption.SortDescending:
               ApplySortMultiple(SortingMode.Descending, column.UniqueName);
               break;
            case InteractiveReportGridView.DescriptorOption.ClearSorting:
               ApplySortMultiple(SortingMode.None, column.UniqueName);
               break;
            case InteractiveReportGridView.DescriptorOption.GroupBy:
               ApplyGroupBy(column.UniqueName);
               break;
            case InteractiveReportGridView.DescriptorOption.Ungroup:
               ApplyUnGroupBy(column.UniqueName);
               break;
            case InteractiveReportGridView.DescriptorOption.VisibleColumn:
               ApplyVisible(column.UniqueName);
               break;
            case InteractiveReportGridView.DescriptorOption.None:
               break;
            default:
               throw new ArgumentOutOfRangeException(nameof(descriptor), descriptor, null);
         }
      }

      protected void SetNewDataSource(IEnumerable<VMEvaluatedCharacteristic> newDataSource)
      {
         if (NeedsRenderToSetNewDataSource(newDataSource))
         {
            _totalDataToDisplay = newDataSource.ToImmutableList();
            VMPlacement.RaiseReportComponentLayoutChanged();
         }
         else
         {
            int actualRowIndex = DataToDisplay.Count();
            _totalDataToDisplay = newDataSource.ToImmutableList();
            DataToDisplay = null;
            DataToDisplay = _totalDataToDisplay.Take(actualRowIndex).ToImmutableList();

            foreach (VMTablePiece tablePiece in Pieces)
            {
               var amountOfRowsForThisPiece = tablePiece.DataToDisplay.Count();
               tablePiece.DataToDisplay = null;
               tablePiece.DataToDisplay = _totalDataToDisplay.Skip(actualRowIndex).Take(amountOfRowsForThisPiece);
               actualRowIndex += tablePiece.DataToDisplay.Count();
            }
         }
      }

      private void SortingTables(ColumnSortEvent argumentsColumn)
      {
         if (argumentsColumn.IsMultiple)
         {
            ApplySortMultiple(argumentsColumn.SortingMode, argumentsColumn.ColumnName);
         }
         else
         {
            ApplySortSingle(argumentsColumn.SortingMode, argumentsColumn.ColumnName);
         }
      }

      private void ReorderColumn(ReorderColumn reorderColumn)
      {
         _tableViewController.ReorderColumn(Id, reorderColumn.ColumnName, reorderColumn.NewIndex);
      }

      private bool NeedsRenderToSetNewDataSource(IEnumerable<VMEvaluatedCharacteristic> newDataSource)
      {
         return (_totalDataToDisplay.Count() != newDataSource.Count());
      }

      protected void SetNewColumnInfoProperty(ColumnInfoList newColumnInfo)
      {
         ColumnInfos = newColumnInfo;

         //REFACTOR: See how to implement Pieces using a List of VMTableViewPiece
         Pieces.ForEach(tablePiece => ((VMTablePiece)tablePiece).ColumnInfos = newColumnInfo);
      }

      private void ApplySortSingle(SortingMode sortingMode, string columnName)
      {
         _tableViewController.SortByColumn(Id, new SortingColumn(columnName, sortingMode));
      }

      private void ApplySortMultiple(SortingMode sortingMode, string columnName)
      {
         _tableViewController.SortByAddColumn(Id, new SortingColumn(columnName, sortingMode));
      }

      private void ApplyGroupBy(string columnName)
      {
         _tableViewController.GroupByColumn(Id, columnName);
      }

      private void ApplyUnGroupBy(string columnName)
      {
         _tableViewController.RemoveGroupByColumn(Id, columnName);
      }

      private void ApplyVisible(string name)
      {
         var columnInfo = ColumnInfos.First(x => x.ColumnName == name);

         if (!columnInfo.Visible)
         {
            _tableViewController.ShowColumn(Id, name);
         }
         else if (ColumnInfos.Count(x => x.Visible) > 1)
         {
            _tableViewController.HideColumn(Id, name);
         }
         else
         {
            // NOTE: Fix to restore hidden/shown columns on context menu when the column could not be hidden.
            SetNewDataSource(_totalDataToDisplay);
         }
      }

      private void ApplyFilterColumn(string columnName, string[] selectedValues)
      {
         _tableViewController.FilterByColumn(Id, columnName, selectedValues);
      }

      public DisabledSpaceData GetDisabledSpace()
      {
         var startPage = VMPlacement.GetPage();
         VMPage endPage;
         int endVisualY;

         if (Pieces.Count > 0)
         {
            var lastPiece = Pieces.Last();
            endPage = VMPlacement.GetPageForPlacement(lastPiece.VMPlacement);
            endVisualY = lastPiece.VMPlacement.VisualY + lastPiece.VMPlacement.VisualHeight;
         }
         else
         {
            endPage = startPage;
            endVisualY = VMPlacement.VisualY + PixelsNeededForSource();
         }

         VMPlacement.LastDisabledSpaceGenerated = new DisabledSpaceData(StartingDisbledSpaceDomain(), startPage, StartingDisbledSpaceVisual(), endPage, endVisualY, RecalculateUsableSpaceTaken());

         return VMPlacement.LastDisabledSpaceGenerated;
      }

      private int RelativeStartingDisabledSpace()
      {
         return HeightHeader;
      }
      private int StartingDisbledSpaceDomain()
      {
         return VMPlacement.DomainY + RelativeStartingDisabledSpace();
      }
      private int StartingDisbledSpaceVisual()
      {
         return VMPlacement.VisualY + RelativeStartingDisabledSpace();
      }
      private int RecalculateUsableSpaceTaken()
      {
         int usableSpaceTaken;

         if (Pieces.Count == 0)
         {
            usableSpaceTaken = PixelsNeededForSource();
         }
         else
         {
            var actualPage = VMPlacement.GetPage();
            usableSpaceTaken = actualPage.SpaceLeftOnThisPage(VMPlacement.VisualY);
            usableSpaceTaken += actualPage.UsableSpaceHeight() * (Pieces.Count - 1);
            usableSpaceTaken += Pieces.Last().VMPlacement.VisualHeight;
         }

         usableSpaceTaken -= RelativeStartingDisabledSpace();
         return usableSpaceTaken;
      }

      private void UpdateVisualHeightBasedOnDataToDisplay()
      {
         if (DataToDisplay == null)
            VMPlacement.SetVisualSize(VMPlacement.VisualWidth, HeightHeader);
         else
            VMPlacement.SetVisualSize(VMPlacement.VisualWidth, HeightHeader + (DataToDisplay.Count() * HeightEachRow));
      }

      public VMPage GetMainElementPage()
      {
         return VMPlacement.GetPage();
      }
   }
}