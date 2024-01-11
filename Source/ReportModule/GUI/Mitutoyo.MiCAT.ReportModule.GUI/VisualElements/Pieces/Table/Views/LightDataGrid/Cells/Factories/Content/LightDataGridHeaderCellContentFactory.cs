// <copyright file="LightDataGridHeaderCellContentFactory.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Windows;
using System.Windows.Controls;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.Pieces.Table.Views.LightDataGrid.Columns;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.Pieces.Table.Views.LightDataGrid.Rows;

namespace Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.Pieces.Table.Views.LightDataGrid.Cells.Factories.Content
{
   public class LightDataGridHeaderCellContentFactory : LightDataGridCellContentFactory
   {
      public const string OuterBorderCellResourceKey = "LightDataGrid_HeaderCellStyle";
      public const string TextBlockResourceKey = "LightDataGrid_HeaderCellTextBlockStyle";

      public override FrameworkElement Create(
         LightDataGrid dataGrid,
         LightDataGridRow row,
         LightDataGridColumn column)
      {
         var outerBorderCellStyle = TryFindStyle(dataGrid, OuterBorderCellResourceKey);
         var textBlockStyle = TryFindStyle(dataGrid, TextBlockResourceKey);
         var columnInfo = column.ColumnInfo;

         return new Border
         {
            Style = outerBorderCellStyle,
            Child =  new TextBlock
            {
               Style = textBlockStyle,
               Text = columnInfo.CaptionText(),
            }
         };
      }
   }
}
