// <copyright file="LightDataGridFakeItemsSource.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Collections.Generic;
using System.Windows;
using Mitutoyo.MiCAT.ReportModule.Domain.Components.TableView;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements.Table.Views.CustomGridView;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements.Table.Views.CustomGridView.CustomFilter;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Test.VisualElements.Pieces.Table.Views.LightDataGrid
{
   internal static class LightDataGridFakeItemsSource
   {
      static LightDataGridFakeItemsSource()
      {
         ItemsSource = new[]
         {
            new Item  { Index = 0, Name = "One Name" },
            new Item  { Index = 1, Name = "One Name" },
            new Item  { Index = 2, Name = "Unique Name" },
            new Item  { Index = 3, Name = "Other Name" },
            new Item  { Index = 4, Name = "Other Name" },
         };
      }

      public static IEnumerable<Item> ItemsSource { get; }

      public static ColumnInfo BuildIndexColumnInfo(
         int columnIndex)
      {
         return new ColumnInfo(
              column: new Column(
                 name: nameof(Item.Index),
                 width: 100,
                 dataFormat: "#0",
                 contentAligment: ContentAligment.Right,
                 isVisible: true),
              columnIndex: 0,
              groupBy: false,
              groupByIndex: -1,
              sortingMode: SortingMode.None,
              sortingIndex: -1,
              format: "0",
              textAlignment: TextAlignment.Right,
              filterInfo: new FilterInfo());
      }

      public static ColumnInfo BuildIndexColumnInfo()
      {
         return BuildIndexColumnInfo(0);
      }

      public static ColumnInfo BuildNameColumnInfo(
         int columnIndex,
         bool isGrouped,
         int groupByIndex)
      {
         return new ColumnInfo(
               column: new Column(
                  name: nameof(Item.Name),
                  width: 100,
                  dataFormat: string.Empty,
                  contentAligment: ContentAligment.Left,
                  isVisible: true),
               columnIndex: columnIndex,
               groupBy: isGrouped,
               groupByIndex: groupByIndex,
               sortingMode: SortingMode.None,
               sortingIndex: -1,
               format: string.Empty,
               textAlignment: TextAlignment.Left,
               filterInfo: new FilterInfo());
      }

      public static ColumnInfo BuildNameColumnInfo(
         int columnIndex)
      {
         return BuildNameColumnInfo(columnIndex, false, -1);
      }

      public static ColumnInfo BuildNameColumnInfo()
      {
         return BuildNameColumnInfo(0, false, -1);
      }

      public class Item
      {
         public int Index { get; set; }
         public string Name { get; set; }
      }
   }
}
