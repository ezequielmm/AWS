// <copyright file="LightDataGridHeaderRow.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Windows;
using System.Windows.Controls;

namespace Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.Pieces.Table.Views.LightDataGrid.Rows
{
   public class LightDataGridHeaderRow : LightDataGridRow
   {
      public LightDataGridHeaderRow(
         int rowIndex) :
         base(rowIndex, new HeaderRowDefinition(), null)
      {
      }

      public class HeaderRowDefinition : RowDefinition
      {
         public const int RowHeight = 34;

         public HeaderRowDefinition()
         {
            Height = new GridLength(RowHeight);
         }
      }
   }
}
