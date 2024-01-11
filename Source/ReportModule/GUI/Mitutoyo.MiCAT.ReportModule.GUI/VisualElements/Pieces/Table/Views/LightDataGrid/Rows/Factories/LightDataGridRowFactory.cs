// <copyright file="LightDataGridRowFactory.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.Pieces.Table.Views.LightDataGrid.Cells.Factories;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.Pieces.Table.Views.LightDataGrid.Columns;

namespace Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.Pieces.Table.Views.LightDataGrid.Rows.Factories
{
   public abstract class LightDataGridRowFactory : ILightDataGridRowFactory
   {
      public LightDataGridRowFactory(
         ILightDataGridCellFactory cellFactory)
      {
         CellFactory = cellFactory;
      }

      protected ILightDataGridCellFactory CellFactory { get; }

      public LightDataGridRow Create(
         LightDataGrid dataGrid,
         int rowIndex,
         object itemSource = null)
      {
         var row = CreateEmptyRow(dataGrid, rowIndex, itemSource);
         AddCells(dataGrid, row);

         return row;
      }

      protected abstract LightDataGridRow CreateEmptyRow(
         LightDataGrid dataGrid,
         int rowIndex,
         object itemSource = null);

      private void AddCells(
         LightDataGrid dataGrid,
         LightDataGridRow row)
      {
         foreach (var column in dataGrid.Columns)
            AddCell(dataGrid, row, column);
      }

      private void AddCell(
         LightDataGrid dataGrid,
         LightDataGridRow row,
         LightDataGridColumn column)
      {
         var cell = CellFactory.Create(dataGrid, row, column);
         row.AddCell(cell);
      }
   }
}
