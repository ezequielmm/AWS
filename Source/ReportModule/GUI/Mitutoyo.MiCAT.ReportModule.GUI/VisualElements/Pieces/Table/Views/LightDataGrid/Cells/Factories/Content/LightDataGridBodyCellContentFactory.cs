// <copyright file="LightDataGridBodyCellContentFactory.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.Pieces.Table.Views.LightDataGrid.Columns;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.Pieces.Table.Views.LightDataGrid.Rows;

namespace Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.Pieces.Table.Views.LightDataGrid.Cells.Factories.Content
{
   public class LightDataGridBodyCellContentFactory : LightDataGridCellContentFactory
   {
      public const string OuterBorderCellResourceKey = "LightDataGrid_CellStyle";
      public const string TextBlockResourceKey = "LightDataGrid_BodyCellTextBlockStyle";

      public override FrameworkElement Create(
         LightDataGrid dataGrid,
         LightDataGridRow row,
         LightDataGridColumn column)
      {
         var cell = CreateCell(dataGrid, row, column);
         return cell;
      }

      private Border CreateCell(
         LightDataGrid dataGrid,
         LightDataGridRow row,
         LightDataGridColumn column)
      {
         var outerBorderCellStyle = TryFindStyle(dataGrid, OuterBorderCellResourceKey);
         var textBlock = CreateTextBlock(dataGrid, row, column);

         return new Border
         {
            Style = outerBorderCellStyle,
            Child = textBlock,
         };
      }

      private TextBlock CreateTextBlock(
         LightDataGrid dataGrid,
         LightDataGridRow row,
         LightDataGridColumn column)
      {
         var textBlockStyle = TryFindStyle(dataGrid, TextBlockResourceKey);
         var columnInfo = column.ColumnInfo;
         var textBlock = new TextBlock
         {
            Style = textBlockStyle,
            TextAlignment = columnInfo.TextAlignment,
         };

         textBlock.SetBinding(
            TextBlock.TextProperty,
            new Binding(columnInfo.ColumnName)
            {
               Source = row.ItemSource,
               StringFormat = columnInfo.Format,
               Mode = BindingMode.OneTime,
            });

         return textBlock;
      }
   }
}
