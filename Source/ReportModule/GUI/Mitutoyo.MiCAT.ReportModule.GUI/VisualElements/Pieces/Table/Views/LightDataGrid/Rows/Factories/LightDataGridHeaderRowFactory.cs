// <copyright file="LightDataGridHeaderRowFactory.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.Pieces.Table.Views.LightDataGrid.Cells.Factories;

namespace Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.Pieces.Table.Views.LightDataGrid.Rows.Factories
{
   public class LightDataGridHeaderRowFactory : LightDataGridRowFactory
   {
      public LightDataGridHeaderRowFactory() :
         base(new LightDataGridHeaderCellFactory())
      {
      }

      protected override LightDataGridRow CreateEmptyRow(
         LightDataGrid dataGrid,
         int rowIndex,
         object itemSource = null)
      {
         var row = new LightDataGridHeaderRow(rowIndex);
         return row;
      }
   }
}
