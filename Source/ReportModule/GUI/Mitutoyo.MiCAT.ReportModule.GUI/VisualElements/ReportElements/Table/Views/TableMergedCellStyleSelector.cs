// <copyright file="TableMergedCellStyleSelector.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Windows;
using System.Windows.Controls;
using Mitutoyo.MiCAT.ReportModule.GUI.Properties;
using Telerik.Windows.Controls.GridView;

namespace Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements.Table.Views
{
   public class TableMergedCellStyleSelector : StyleSelector
   {
      public Style GenericMergedCellStyle { get; set; }
      public Style PassStatusStyle { get; set; }
      public Style FailStatusStyle { get; set; }
      public Style InvalidStatusStyle { get; set; }

      public override Style SelectStyle(
         object item,
         DependencyObject container)
      {
         var cell = (GridViewMergedCell)container;
         var cellValue = (string)cell.Value;

         if (cellValue == Resources.Pass)
            return PassStatusStyle;
         else if (cellValue == Resources.Fail)
            return FailStatusStyle;
         else if (cellValue == Resources.Invalid)
            return InvalidStatusStyle;

         return GenericMergedCellStyle;
      }
   }
}
