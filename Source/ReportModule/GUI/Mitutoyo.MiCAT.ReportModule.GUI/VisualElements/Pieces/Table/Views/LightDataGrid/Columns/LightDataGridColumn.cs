// <copyright file="LightDataGridColumn.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Windows;
using System.Windows.Controls;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements.Table.Views.CustomGridView;

namespace Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.Pieces.Table.Views.LightDataGrid.Columns
{
   public class LightDataGridColumn
   {
      public LightDataGridColumn(
         ColumnInfo columnInfo)
      {
         ColumnInfo = columnInfo;
         ColumnDefinition = new ColumnDefinition
         {
            Width = new GridLength(
               columnInfo.Visible ? columnInfo.Width : 0,
               GridUnitType.Star),
         };
      }

      public int ColumnIndex => ColumnInfo.ColumnIndex;
      public ColumnInfo ColumnInfo { get; }
      public ColumnDefinition ColumnDefinition { get; }
   }
}
