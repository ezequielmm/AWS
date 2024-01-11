// <copyright file="GridViewHeaderMenu.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Telerik.Windows;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;
using GridViewColumn = Telerik.Windows.Controls.GridViewColumn;

namespace Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements.Table.Views.CustomGridView
{
   [ExcludeFromCodeCoverage]
   public partial class InteractiveReportGridView
   {
      public class GridViewHeaderMenu
      {
         private readonly InteractiveReportGridView _reportGrid = null;

         public GridViewHeaderMenu(
            InteractiveReportGridView reportGrid,
            Theme theme)
         {
            _reportGrid = reportGrid;
            Attach(theme);
         }

         public void Attach(
            Theme theme)
         {
            if (_reportGrid != null)
            {
               var contextMenu = new RadContextMenu();

               StyleManager.SetTheme(contextMenu, theme);

               contextMenu.Opening += OnMenuOpening;
               contextMenu.ItemClick += OnMenuItemClick;

               RadContextMenu.SetContextMenu(_reportGrid, contextMenu);
            }
         }

         private void OnMenuOpening(object sender, RoutedEventArgs e)
         {
            var menu = (RadContextMenu) sender;
            var cell = menu.GetClickedElement<GridViewHeaderCell>();

            if (cell != null)
            {
               InitializeContextMenuItems(menu, cell);
            }
            else
            {
               menu.IsOpen = false;
            }
         }

         private void InitializeContextMenuItems(RadContextMenu menu, GridViewHeaderCell cell)
         {
            menu.Items.Clear();

            AddMenuItem(menu.Items, Properties.Resources.SortAscendingString, cell.Column.Header);

            AddMenuItem(menu.Items, Properties.Resources.SortDescendingString, cell.Column.Header);

            AddMenuItem(menu.Items, Properties.Resources.ClearSortingString, cell.Column.Header);

            AddMenuItem(menu.Items,
               _reportGrid.ColumnInfos.First(x => x.ColumnName == cell.Column.UniqueName).GroupBy
                  ? Properties.Resources.UngroupByString
                  : Properties.Resources.GroupByString, cell.Column.Header);

            var chooseColumnsMenu = AddMenuItem(menu.Items, Properties.Resources.ChooseColumnsString,
               cell.Column.Header);
            AddChooseColumnsMenuItems(chooseColumnsMenu);
         }

         private RadMenuItem AddMenuItem(ItemCollection items, string menuItemLabel, object header)
         {
            var item = new RadMenuItem {Header = String.Format(menuItemLabel, header)};
            items.Add(item);

            return item;
         }

         private void AddChooseColumnsMenuItems(RadMenuItem item)
         {
            foreach (var column in _reportGrid.Columns)
            {
               AddMenuItem(column, item);
            }
         }

         private void AddMenuItem(GridViewColumn column, RadMenuItem item)
         {
            var subMenu = CreateSubMenu(column);

            var isCheckedBinding = CreateIsCheckedBinding(column);

            subMenu.SetBinding(RadMenuItem.IsCheckedProperty, isCheckedBinding);

            item.Items.Add(subMenu);
         }

         private Binding CreateIsCheckedBinding(GridViewColumn column)
         {
            var isCheckedBinding = new Binding("IsVisible")
            {
               Mode = BindingMode.TwoWay,
               Source = column
            };
            return isCheckedBinding;
         }

         private RadMenuItem CreateSubMenu(GridViewColumn column)
         {
            var subMenu = new RadMenuItem
            {
               Header = column.Header,
               IsCheckable = true,
               IsChecked = true
            };
            return subMenu;
         }

         private void OnMenuItemClick(object sender, RoutedEventArgs e)
         {
            var menu = (RadContextMenu) sender;

            var cell = menu.GetClickedElement<GridViewHeaderCell>();
            var clickedItem = ((RadRoutedEventArgs) e).OriginalSource as RadMenuItem;
            var column = cell.Column;

            if (clickedItem.Parent is RadMenuItem)
            {
               GridViewColumn columnSelected = null;

               foreach (var columnToVisible in _reportGrid.Columns)
               {
                  if (columnToVisible.Header == clickedItem.Header)
                  {
                     columnSelected = columnToVisible;
                  }
               }

               _reportGrid.RaiseApplyHeaderEvent(columnSelected, DescriptorOption.VisibleColumn);
            }
            else
            {
               var descriptorOption = GetDescriptorOption(Convert.ToString(clickedItem.Header), (string)column.Header);

               _reportGrid.RaiseApplyHeaderEvent(column, descriptorOption);
            }
         }

         private static DescriptorOption GetDescriptorOption(string header, string columnHeader)
         {
            if (header.Contains(string.Format(Properties.Resources.SortAscendingString, columnHeader)))
               return DescriptorOption.SortAscending;
            if (header.Contains(string.Format(Properties.Resources.SortDescendingString, columnHeader)))
               return DescriptorOption.SortDescending;
            if (header.Contains(string.Format(Properties.Resources.ClearSortingString, columnHeader)))
               return DescriptorOption.ClearSorting;
            if (header.Contains(string.Format(Properties.Resources.GroupByString, columnHeader)))
               return DescriptorOption.GroupBy;
            if (header.Contains(string.Format(Properties.Resources.UngroupByString, columnHeader)))
               return DescriptorOption.Ungroup;

            return DescriptorOption.None;
         }
      }

      public enum DescriptorOption
      {
         SortAscending,
         SortDescending,
         ClearSorting,
         GroupBy,
         Ungroup,
         None,
         VisibleColumn
      }
   }
}
