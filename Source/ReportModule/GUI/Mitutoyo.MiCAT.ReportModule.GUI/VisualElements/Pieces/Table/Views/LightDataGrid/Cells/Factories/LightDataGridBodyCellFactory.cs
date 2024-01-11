// <copyright file="LightDataGridBodyCellFactory.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Linq;
using System.Windows;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.Pieces.Table.Views.LightDataGrid.Cells.Factories.Content;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.Pieces.Table.Views.LightDataGrid.Columns;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.Pieces.Table.Views.LightDataGrid.Rows;

namespace Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.Pieces.Table.Views.LightDataGrid.Cells.Factories
{
   public class LightDataGridBodyCellFactory : LightDataGridCellFactory
   {
      public LightDataGridBodyCellFactory()
         : base(new LightDataGridBodyCellContentFactory())
      {
      }

      protected override LightDataGridCell CreateCell(
         LightDataGrid dataGrid,
         LightDataGridRow row,
         LightDataGridColumn column,
         FrameworkElement content)
      {
         var cell = new LightDataGridCell(row, column, content);
         var columnInfo = column.ColumnInfo;

         if (columnInfo.GroupBy)
         {
            var cellFromPreviousRow = GetCellFromPreviousRow(dataGrid, row, column);

            if (CanMergeCells(cell, cellFromPreviousRow))
               cell = new LightDataGridMergedCell(cellFromPreviousRow);
         }

         return cell;
      }

      private bool CanMergeCells(
         LightDataGridCell previousCell,
         LightDataGridCell newCell)
      {
         if (previousCell == null || newCell == null)
            return false;

         return previousCell.GetValue() == newCell.GetValue();
      }

      private LightDataGridCell GetCellFromPreviousRow(
         LightDataGrid dataGrid,
         LightDataGridRow currentRow,
         LightDataGridColumn currentColumn)
      {
         var previousRow = GetPreviousRow(dataGrid, currentRow);
         return previousRow?.Cells.ElementAt(currentColumn.ColumnIndex);
      }

      private LightDataGridRow GetPreviousRow(
         LightDataGrid dataGrid,
         LightDataGridRow row)
      {
         var previousRowIndex = row.RowIndex - 1;
         var previousRow = dataGrid.Rows.ElementAtOrDefault(previousRowIndex);
         return previousRow;
      }
   }
}
