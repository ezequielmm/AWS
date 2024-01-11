// <copyright file="LightDataGridCell.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Windows;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.Pieces.Table.Views.LightDataGrid.Columns;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.Pieces.Table.Views.LightDataGrid.Rows;

namespace Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.Pieces.Table.Views.LightDataGrid.Cells
{
   public class LightDataGridCell
   {
      public LightDataGridCell(
         LightDataGridRow row,
         LightDataGridColumn column,
         FrameworkElement content)
      {
         Row = row;
         Column = column;
         Content = content;
      }

      public LightDataGridRow Row { get; }
      public LightDataGridColumn Column { get; }
      public FrameworkElement Content { get; }

      public object GetValue()
      {
         var itemSource = Row.ItemSource;

         if (itemSource is null)
            return null;

         var columnInfo = Column.ColumnInfo;
         var columnName = columnInfo.ColumnName;
         var propertyInfo = itemSource.GetType().GetProperty(columnName);
         var value = propertyInfo.GetValue(itemSource);

         return value;
      }
   }
}
