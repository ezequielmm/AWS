// <copyright file="TablePieceCellStyleSelector.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Windows;
using Mitutoyo.MiCAT.ReportModule.GUI.Properties;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.Pieces.Table.Views.LightDataGrid.Cells;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements.Table.ViewModels;

namespace Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.Pieces.Table.Views
{
   public class TablePieceCellStyleSelector : LightDataGridCellStyleSelector
   {
      public Style PassStatusStyle { get; set; }
      public Style FailStatusStyle { get; set; }
      public Style InvalidStatusStyle { get; set; }

      public override Style SelectStyle(
         LightDataGridCell cell)
      {
         var columnName = cell.Column.ColumnInfo.ColumnName;
         var itemSource = cell.Row.ItemSource;

         if (columnName == nameof(VMEvaluatedCharacteristic.Status)
            && itemSource is VMEvaluatedCharacteristic characteristic)
            return SelectStatusCellStyle(characteristic.Status);

         return base.SelectStyle(cell);
      }

      private Style SelectStatusCellStyle(
         string status)
      {
         if (status == Resources.Pass)
            return PassStatusStyle;
         else if (status == Resources.Fail)
            return FailStatusStyle;
         else if (status == Resources.Invalid)
            return InvalidStatusStyle;

         return null;
      }
   }
}
