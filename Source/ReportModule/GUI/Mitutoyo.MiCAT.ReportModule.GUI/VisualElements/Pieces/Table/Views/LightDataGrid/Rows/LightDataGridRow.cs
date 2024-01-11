// <copyright file="LightDataGridRow.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Collections.Generic;
using System.Windows.Controls;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.Pieces.Table.Views.LightDataGrid.Cells;

namespace Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.Pieces.Table.Views.LightDataGrid.Rows
{
   public abstract class LightDataGridRow
   {
      private readonly List<LightDataGridCell> _cells;

      public LightDataGridRow(
         int rowIndex,
         RowDefinition rowDefinition,
         object itemSource = null)
      {
         RowIndex = rowIndex;
         RowDefinition = rowDefinition;
         ItemSource = itemSource;

         _cells = new List<LightDataGridCell>();
      }

      public int RowIndex { get; }

      public RowDefinition RowDefinition { get; }
      public object ItemSource { get; }
      public IEnumerable<LightDataGridCell> Cells => _cells;

      public void AddCell(
         LightDataGridCell cell)
      {
         _cells.Add(cell);
      }
   }
}
