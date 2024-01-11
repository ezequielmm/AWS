// <copyright file="LightDataGrid.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.Pieces.Table.Views.LightDataGrid.Cells;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.Pieces.Table.Views.LightDataGrid.Columns;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.Pieces.Table.Views.LightDataGrid.Columns.Factories;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.Pieces.Table.Views.LightDataGrid.Rows;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.Pieces.Table.Views.LightDataGrid.Rows.Factories;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements.Table.Views.CustomGridView;

namespace Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.Pieces.Table.Views.LightDataGrid
{
   public class LightDataGrid : Grid
   {
      private readonly List<LightDataGridColumn> _columns;
      private readonly List<LightDataGridRow> _rows;

      public const string LightDataGridStyle = "LightDataGridStyle";

      public static DependencyProperty ColumnInfosProperty;
      public static DependencyProperty ItemsSourceProperty;
      public static DependencyProperty CellStyleSelectorProperty;

      static LightDataGrid()
      {
         ColumnInfosProperty = DependencyProperty.Register(
            nameof(ColumnInfos),
            typeof(IEnumerable<ColumnInfo>),
            typeof(LightDataGrid),
            new PropertyMetadata(OnColumnInfosPropertyChanged));

         ItemsSourceProperty = DependencyProperty.Register(
            nameof(ItemsSource),
            typeof(IEnumerable<object>),
            typeof(LightDataGrid),
            new PropertyMetadata(OnItemsSourcePropertyChanged));

         CellStyleSelectorProperty = DependencyProperty.Register(
            nameof(CellStyleSelector),
            typeof(LightDataGridCellStyleSelector),
            typeof(LightDataGrid));
      }

      public LightDataGrid()
      {
         ColumnFactory = new LightDataGridColumnFactory();
         HeaderRowFactory = new LightDataGridHeaderRowFactory();
         BodyRowFactory = new LightDataGridBodyRowFactory();

         _columns = new List<LightDataGridColumn>();
         _rows = new List<LightDataGridRow>();
      }

      public IEnumerable<ColumnInfo> ColumnInfos
      {
         get => (IEnumerable<ColumnInfo>)GetValue(ColumnInfosProperty);
         set => SetValue(ColumnInfosProperty, value);
      }

      public IEnumerable<object> ItemsSource
      {
         get => (IEnumerable<object>)GetValue(ItemsSourceProperty);
         set => SetValue(ItemsSourceProperty, value);
      }

      public LightDataGridCellStyleSelector CellStyleSelector
      {
         get => (LightDataGridCellStyleSelector)GetValue(CellStyleSelectorProperty);
         set => SetValue(CellStyleSelectorProperty, value);
      }

      public virtual IEnumerable<LightDataGridColumn> Columns => _columns;
      public virtual IEnumerable<LightDataGridRow> Rows => _rows;

      private ILightDataGridColumnFactory ColumnFactory { get; }
      private ILightDataGridRowFactory HeaderRowFactory { get; }
      private ILightDataGridRowFactory BodyRowFactory { get; }

      private static void OnColumnInfosPropertyChanged(
         DependencyObject d,
         DependencyPropertyChangedEventArgs e)
      {
         var lightDataGrid = (LightDataGrid)d;
         lightDataGrid.UpdateVisualTree();
      }

      private static void OnItemsSourcePropertyChanged(
         DependencyObject d,
         DependencyPropertyChangedEventArgs e)
      {
         var lightDataGrid = (LightDataGrid)d;
         lightDataGrid.UpdateVisualTree();
      }

      public void AddColumn(
         LightDataGridColumn column)
      {
         _columns.Add(column);
         AddColumnToVisualTree(column);
      }

      public void AddRow(
         LightDataGridRow row)
      {
         _rows.Add(row);
         AddRowToVisualTree(row);
      }

      protected override void OnInitialized(
         EventArgs e)
      {
         Style = TryFindResource(LightDataGridStyle) as Style;
      }

      private void UpdateVisualTree()
      {
         Clear();
         CreateColumns();
         CreateRows();
      }

      private void Clear()
      {
         ColumnDefinitions.Clear();
         RowDefinitions.Clear();
         Children.Clear();
         _columns.Clear();
         _rows.Clear();
      }

      private void CreateColumns()
      {
         if (ColumnInfos is null)
            return;

         foreach (var columnInfo in ColumnInfos)
         {
            var column = ColumnFactory.Create(columnInfo);
            AddColumn(column);
         }
      }

      private void CreateRows()
      {
         CreateHeaderRows();

         if (ItemsSource is null)
            return;

         CreateBodyRows();
      }

      private void CreateHeaderRows()
      {
         var rowIndex = 0;
         var row = HeaderRowFactory.Create(this, rowIndex);
         AddRow(row);
      }

      private void CreateBodyRows()
      {
         var rowIndex = Rows.Count();

         foreach (var itemSource in ItemsSource)
         {
            var row = BodyRowFactory.Create(this, rowIndex++, itemSource);
            AddRow(row);
         }
      }

      private void AddColumnToVisualTree(
         LightDataGridColumn column)
      {
         ColumnDefinitions.Add(column.ColumnDefinition);
      }

      private void AddRowToVisualTree(
         LightDataGridRow row)
      {
         RowDefinitions.Add(row.RowDefinition);

         foreach (var cell in row.Cells)
            AddCellToVisualTree(cell);
      }

      private void AddCellToVisualTree(
         LightDataGridCell cell)
      {
         if (cell is LightDataGridMergedCell)
            return;

         SetRow(cell.Content, cell.Row.RowIndex);
         SetColumn(cell.Content, cell.Column.ColumnIndex);
         Children.Add(cell.Content);
      }
   }
}
