// <copyright file="LightDataGridColumnFactory.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements.Table.Views.CustomGridView;

namespace Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.Pieces.Table.Views.LightDataGrid.Columns.Factories
{
   public class LightDataGridColumnFactory : ILightDataGridColumnFactory
   {
      public LightDataGridColumn Create(
         ColumnInfo columnInfo)
      {
         var column = new LightDataGridColumn(columnInfo);
         return column;
      }
   }
}
