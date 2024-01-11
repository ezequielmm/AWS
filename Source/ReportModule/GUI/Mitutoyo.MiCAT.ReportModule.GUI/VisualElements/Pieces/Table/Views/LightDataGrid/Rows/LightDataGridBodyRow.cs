// <copyright file="LightDataGridBodyRow.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Windows;
using System.Windows.Controls;

namespace Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.Pieces.Table.Views.LightDataGrid.Rows
{
   public class LightDataGridBodyRow : LightDataGridRow
   {
      public LightDataGridBodyRow(
         int rowIndex,
         object itemSource = null) :
         base(rowIndex, new BodyRowDefinition(), itemSource)
      {
      }

      public class BodyRowDefinition : RowDefinition
      {
         public const int RowHeight = 29;

         public BodyRowDefinition()
         {
            Height = new GridLength(RowHeight);
         }
      }
   }
}
