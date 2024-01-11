// <copyright file="LightDataGridCellFactory.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Windows;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.Pieces.Table.Views.LightDataGrid.Cells.Factories.Content;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.Pieces.Table.Views.LightDataGrid.Columns;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.Pieces.Table.Views.LightDataGrid.Rows;

namespace Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.Pieces.Table.Views.LightDataGrid.Cells.Factories
{
   public abstract class LightDataGridCellFactory : ILightDataGridCellFactory
   {
      public LightDataGridCellFactory(
         ILightDataGridCellContentFactory contentGenerator)
      {
         ContentFactory = contentGenerator;
      }

      protected ILightDataGridCellContentFactory ContentFactory { get; }

      public LightDataGridCell Create(
         LightDataGrid dataGrid,
         LightDataGridRow row,
         LightDataGridColumn column)
      {
         var content = ContentFactory.Create(dataGrid, row, column);
         var cell = CreateCell(dataGrid, row, column, content);

         ApplyCellStyleSelector(dataGrid, cell);

         return cell;
      }

      protected abstract LightDataGridCell CreateCell(
         LightDataGrid dataGrid,
         LightDataGridRow row,
         LightDataGridColumn column,
         FrameworkElement content);

      private void ApplyCellStyleSelector(
         LightDataGrid dataGrid,
         LightDataGridCell cell)
      {
         var style = dataGrid.CellStyleSelector?.SelectStyle(cell);

         if (style is Style)
            cell.Content.Style = style;
      }
   }
}
