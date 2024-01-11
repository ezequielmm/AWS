// <copyright file="LightDataGridMergedCell.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Windows.Controls;

namespace Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.Pieces.Table.Views.LightDataGrid.Cells
{
   public class LightDataGridMergedCell : LightDataGridCell
   {
      public LightDataGridMergedCell(
         LightDataGridCell cell) :
         base(
            cell.Row,
            cell.Column,
            cell.Content)
      {
         var rowSpan = Grid.GetRowSpan(cell.Content);
         Grid.SetRowSpan(cell.Content, rowSpan + 1);
      }
   }
}
