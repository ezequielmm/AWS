// <copyright file="LightDataGridBodyRowFactory.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.Pieces.Table.Views.LightDataGrid.Cells.Factories;

namespace Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.Pieces.Table.Views.LightDataGrid.Rows.Factories
{
   public class LightDataGridBodyRowFactory : LightDataGridRowFactory
   {
      public LightDataGridBodyRowFactory() :
         base(new LightDataGridBodyCellFactory())
      {
      }

      protected override LightDataGridRow CreateEmptyRow(
         LightDataGrid dataGrid,
         int rowIndex,
         object itemSource = null)
      {
         var row = new LightDataGridBodyRow(rowIndex, itemSource);
         return row;
      }
   }
}
