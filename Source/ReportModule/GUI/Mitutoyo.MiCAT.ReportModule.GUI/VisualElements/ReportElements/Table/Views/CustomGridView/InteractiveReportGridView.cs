// <copyright file="InteractiveReportGridView.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using Mitutoyo.MiCAT.ReportModule.Domain.Components.TableView;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements.Table.ViewModels;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements.Table.Views.CustomGridView.CustomFilter;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;
using static Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements.Table.Views.CustomGridView.CustomFilter.VMSelectDistinctFilterControl;

namespace Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements.Table.Views.CustomGridView
{
   public partial class InteractiveReportGridView : ReportGridView
   {
      public static readonly DependencyPropertyKey HeaderPropertyKey;
      public static readonly DependencyProperty HeaderProperty;

      public event EventHandler<ColumnWidthsChangedEventArgs> ColumnWidthsChanged;

      public static readonly RoutedEvent ApplyHeadersEvent = EventManager.RegisterRoutedEvent(
         "ApplyHeader", RoutingStrategy.Bubble, typeof(EventHandler<CustomHeadersFiltersChangeEventArgs>), typeof(InteractiveReportGridView));

      static InteractiveReportGridView()
      {
         DefaultStyleKeyProperty.OverrideMetadata(typeof(InteractiveReportGridView), new FrameworkPropertyMetadata(typeof(InteractiveReportGridView)));

         HeaderPropertyKey = RegisterHeaderDependencyProperty();
         HeaderProperty = HeaderPropertyKey.DependencyProperty;
      }

      public InteractiveReportGridView() : base()
      {
         var theme = new Office2016Theme();
         SetValue(HeaderPropertyKey, new GridViewHeaderMenu(this, theme));
         StyleManager.SetTheme(this, theme);

         PreviewMouseLeftButtonUp += Grid_PreviewMouseLeftButtonUp;
         ColumnReordered += Grid_ColumnReordered;
         ColumnWidthChanged += OnColumnWidthChanged;
      }

      public GridViewHeaderMenu Header => (GridViewHeaderMenu)GetValue(HeaderProperty);

      public event RoutedEventHandler ApplyHeader
      {
         add { AddHandler(ApplyHeadersEvent, value); }
         remove { RemoveHandler(ApplyHeadersEvent, value); }
      }

      protected override void ConfigureDataColumn(GridViewDataColumn dataColumn, ColumnInfo columnInfo)
      {
         base.ConfigureDataColumn(dataColumn, columnInfo);

         if (columnInfo.FilterInfo.IsFilterable)
         {
            dataColumn.IsFilterable = true;
            var filterControl = new SelectDistinctFilterControl();
            dataColumn.FilteringControl = filterControl;
            filterControl.IsActive = columnInfo.FilterInfo.SelectedValues.Any();
            filterControl.Loaded += (sender, e) =>
            {
               var column = ColumnInfos.First(x => x.ColumnName == columnInfo.ColumnName);
               var options = column.FilterInfo.Values?.Select(x => new SelectDistinctFilterOption
               {
                  IsChecked = column.FilterInfo.SelectedValues.Contains(x.Value),
                  Name = x.DisplayText,
                  Value = x.Value,
                  IsEnabled = !x.IsDisabled,
               }).ToList();
               var vmFilterControl = new VMSelectDistinctFilterControl(options);
               vmFilterControl.ApplyFilter += (s, el) =>
               {
                  filterControl.IsActive = el.SelectedItems.Any();
                  VmFilterControl_ApplyFilter(columnInfo, el);
               };
               vmFilterControl.CancelFilter += (s, el) => CloseFilterPopups();
               filterControl.DataContext = vmFilterControl;
            };
         }
      }

      private static DependencyPropertyKey RegisterHeaderDependencyProperty()
      {
         FrameworkPropertyMetadata metadata = new FrameworkPropertyMetadata();
         return DependencyProperty.RegisterReadOnly("Header", typeof(GridViewHeaderMenu), typeof(InteractiveReportGridView), metadata);
      }

      private void RaiseApplyHeaderEvent(GridViewColumn column, DescriptorOption descriptor)
      {
         CustomHeadersFiltersChangeEventArgs newEventArgs = new CustomHeadersFiltersChangeEventArgs(ApplyHeadersEvent, column, descriptor);
         RaiseEvent(newEventArgs);
      }

      private void Grid_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
      {
         bool isMultiple = Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift);

         var header = GetHeaderCellFromMouseEvent(e);

         if (header == null)
            return;

         e.Handled = true;
         var sortMode = GetNextSort(header.SortingState);

         var vm = this.DataContext as VMTable;
         vm.SortCommand.Execute(new ColumnSortEvent(header.Column.UniqueName, sortMode, isMultiple));
      }

      private void Grid_ColumnReordered(object sender, GridViewColumnEventArgs e)
      {
         var vm = this.DataContext as VMTable;
         vm.ColumnReorderedCommand.Execute(new ReorderColumn(e.Column.UniqueName, e.Column.DisplayIndex));
      }

      private GridViewHeaderCell GetHeaderCellFromMouseEvent(MouseButtonEventArgs e)
      {
         var element = e.OriginalSource as FrameworkElement;
         var header = element as GridViewHeaderCell ?? element?.Parent as GridViewHeaderCell;

         return header;
      }

      private SortingMode GetNextSort(SortingState currentSortingState)
      {
         SortingMode sortingMode;

         switch (currentSortingState)
         {
            case SortingState.Ascending:
               sortingMode = SortingMode.Descending;
               break;
            case SortingState.Descending:
               sortingMode = SortingMode.None;
               break;
            case SortingState.None:
            default:
               sortingMode = SortingMode.Ascending;
               break;
         }

         return sortingMode;
      }

      private void VmFilterControl_ApplyFilter(ColumnInfo columnInfo, CustomFilterEventArgs e)
      {
         CloseFilterPopups();
         RaiseApplyFilterEvent(columnInfo, e.SelectedItems);
      }

      private void CloseFilterPopups()
      {
         this.ChildrenOfType<Popup>()
            .Where(p => p.Name == "PART_DropDownPopup")
            .ToList()
            .ForEach(d => d.IsOpen = false);
      }

      private void RaiseApplyFilterEvent(ColumnInfo columnInfo, IEnumerable<SelectDistinctFilterOption> selectedItems)
      {
         var selectedValues = selectedItems.Select(x => x.Value).ToArray();
         var newEventArgs = new CustomFilterInfoEventArgs(ApplyFilterEvent, columnInfo.ColumnName, selectedValues);
         RaiseEvent(newEventArgs);
      }

      private void OnColumnWidthChanged(object sender, ColumnWidthChangedEventArgs args)
      {
         var column = args.Column;
         var columnInfo = GetColumnInfoFor(args.Column);
         var columnWidthHasChanged = columnInfo.Width != column.Width.Value;

         if (columnWidthHasChanged)
         {
            RaiseColumnWidthsChangedEvent();
         }
      }

      private void RaiseColumnWidthsChangedEvent()
      {
         ColumnWidthsChanged?.Invoke(this, GetColumnWidthsChangedEventArgs());
      }

      private ColumnWidthsChangedEventArgs GetColumnWidthsChangedEventArgs()
      {
         var columnWidthInfoList = GetColumnWidthInfoList();

         return new ColumnWidthsChangedEventArgs(columnWidthInfoList);
      }

      private IEnumerable<ColumnWidthsChangedEventArgs.ColumnWidthInfo> GetColumnWidthInfoList()
      {
         return ColumnInfos.Select(GetColumnWidthInfoFor);
      }

      private ColumnWidthsChangedEventArgs.ColumnWidthInfo GetColumnWidthInfoFor(ColumnInfo columnInfo)
      {
         var column = GetGridViewColumnFor(columnInfo);

         return new ColumnWidthsChangedEventArgs.ColumnWidthInfo(column.UniqueName, column.Width.Value);
      }

      private ColumnInfo GetColumnInfoFor(GridViewColumn column)
      {
         return ColumnInfos.First(x => x.ColumnName == column.UniqueName);
      }

      private GridViewColumn GetGridViewColumnFor(ColumnInfo columnInfo)
      {
         return Columns.Cast<GridViewColumn>().First(x => x.UniqueName == columnInfo.ColumnName);
      }
   }
}