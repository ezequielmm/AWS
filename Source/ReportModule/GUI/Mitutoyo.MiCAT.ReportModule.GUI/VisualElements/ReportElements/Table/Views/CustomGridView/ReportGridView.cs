// <copyright file="ReportGridView.cs" company="Mitutoyo Europe GmbH">
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
using Mitutoyo.MiCAT.Core.StdLib.Extensions;
using Mitutoyo.MiCAT.ReportModule.Domain.Components.TableView;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;

namespace Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements.Table.Views.CustomGridView
{
   public partial class ReportGridView : RadGridView
   {
      public static readonly DependencyProperty ColumnInfosProperty;
      public static readonly DependencyProperty CellsStyleSelectorProperty;

      public static readonly RoutedEvent ApplyFilterEvent = EventManager.RegisterRoutedEvent(
        "ApplyFilter", RoutingStrategy.Bubble, typeof(EventHandler<CustomFilterInfoEventArgs>), typeof(InteractiveReportGridView));

      static ReportGridView()
      {
         DefaultStyleKeyProperty.OverrideMetadata(typeof(ReportGridView), new FrameworkPropertyMetadata(typeof(ReportGridView)));

         ColumnInfosProperty = RegisterColumnInfosDependencyProperty();

         CellsStyleSelectorProperty = DependencyProperty.Register(
            nameof(CellsStyleSelector),
            typeof(StyleSelector),
            typeof(ReportGridView),
            new PropertyMetadata());
      }

      public ReportGridView()
      {
         AutoGeneratingColumn += Grid_AutoGenerationColumn;
         LayoutUpdated += Grid_LayoutUpdated;
         MergedCellsStyleSelector = new StatusCellMergedStyleSelector();
      }
      public override void OnApplyTemplate()
      {
         base.OnApplyTemplate();

         //REFACTOR: We should have access to the ControlTemplate for the grid, which is on the Theme dll. We have to bring the xaml ControlTemplate definition to our project to avoid setting this margin at code behind.
         //          Look at PART_ItemsScrollViewer definition at the Theme definition.
         GridViewScrollViewer itemsScrollViewer = (GridViewScrollViewer)GetTemplateChild("PART_ItemsScrollViewer");
         itemsScrollViewer.Margin = new Thickness(1, 1, 0, 0);
         itemsScrollViewer.BorderThickness = new Thickness(0);
         itemsScrollViewer.Background = null;
      }

      public event RoutedEventHandler ApplyFilter
      {
         add { AddHandler(ApplyFilterEvent, value); }
         remove { RemoveHandler(ApplyFilterEvent, value); }
      }

      public IList<ColumnInfo> ColumnInfos
      {
         get => (IList<ColumnInfo>)GetValue(ColumnInfosProperty);
         set => SetValue(ColumnInfosProperty, value);
      }

      public StyleSelector CellsStyleSelector
      {
         get => (StyleSelector)GetValue(CellsStyleSelectorProperty);
         set => SetValue(CellsStyleSelectorProperty, value);
      }

      private static DependencyProperty RegisterColumnInfosDependencyProperty()
      {
         FrameworkPropertyMetadata metadata = new FrameworkPropertyMetadata();
         return DependencyProperty.Register(nameof(ColumnInfos), typeof(IList<ColumnInfo>), typeof(ReportGridView), metadata);
      }

      private void Grid_AutoGenerationColumn(object sender, GridViewAutoGeneratingColumnEventArgs e)
      {
         GridViewDataColumn dataColumn = e.Column as GridViewDataColumn;
         ColumnInfo columnInfo = this.ColumnInfos?.FirstOrDefault(x => x.ColumnName == dataColumn.UniqueName);

         if (dataColumn == null || columnInfo == null)
         {
            e.Cancel = true;
            return;
         }

         ConfigureCellsBehaviour();
         ConfigureDataColumn(dataColumn, columnInfo);
      }

      protected virtual void ConfigureDataColumn(GridViewDataColumn dataColumn, ColumnInfo columnInfo)
      {
         dataColumn.IsReadOnly = true;
         dataColumn.Focusable = false;
         dataColumn.DisplayIndex = columnInfo.Visible ? columnInfo.ColumnIndex : -1;
         dataColumn.IsCellMergingEnabled = columnInfo.GroupBy;
         dataColumn.IsVisible = columnInfo.Visible;
         dataColumn.Width = new GridViewLength(columnInfo.Width, GridViewLengthUnitType.Star);
         dataColumn.DataFormatString = columnInfo.Format;
         dataColumn.TextAlignment = columnInfo.TextAlignment;
         dataColumn.Header = columnInfo.CaptionText();
         dataColumn.IsFilterable = false;
         dataColumn.CellStyleSelector = CellsStyleSelector;
      }

      private void ConfigureCellsBehaviour()
      {
         var isAnyGroupedColumn = ColumnInfos.Any(x => x.GroupBy);
         if (isAnyGroupedColumn)
         {
            CanUserFreezeColumns = false;
            GroupRenderMode = GroupRenderMode.Flat;
            MergedCellsDirection = MergedCellsDirection.Vertical;
         }
         else
         {
            MergedCellsDirection = MergedCellsDirection.None;
            GroupRenderMode = GroupRenderMode.Nested;
         }
      }

      private void Grid_LayoutUpdated(object sender, EventArgs e)
      {
         var cells = this.ChildrenOfType<GridViewHeaderCell>();

         cells.ForEach(cell =>
         {
            ColumnInfo columnInfo = this.ColumnInfos?.SingleOrDefault(x => x.ColumnName == cell.DataColumn?.UniqueName);

            if (columnInfo == null)
               return;

            UpdateSortingState(columnInfo, cell);
            UpdateFilteringState(columnInfo, cell);
         });
      }

      private void UpdateSortingState(ColumnInfo columnInfo, GridViewHeaderCell headerCell)
      {
         headerCell.SortingState = MapSorting(columnInfo.SortingMode);
      }

      private void UpdateFilteringState(ColumnInfo columnInfo, GridViewHeaderCell headerCell)
      {
         if (headerCell.FilteringUIVisibility == Visibility.Visible)
         {
            headerCell.FilteringControl.IsActive = columnInfo.FilterInfo.SelectedValues.Any();
         }
      }

      private SortingState MapSorting(SortingMode sortingMode)
      {
         SortingState sortingState;

         switch (sortingMode)
         {
            case SortingMode.Ascending:
               sortingState = SortingState.Ascending;
               break;
            case SortingMode.Descending:
               sortingState = SortingState.Descending;
               break;
            case SortingMode.None:
            default:
               sortingState = SortingState.None;
               break;
         }

         return sortingState;
      }
   }
}