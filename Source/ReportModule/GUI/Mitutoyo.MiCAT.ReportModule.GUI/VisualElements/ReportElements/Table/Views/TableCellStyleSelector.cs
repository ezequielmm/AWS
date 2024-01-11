// <copyright file="TableCellStyleSelector.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Windows;
using System.Windows.Controls;
using Mitutoyo.MiCAT.ReportModule.GUI.Properties;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements.Table.ViewModels;
using Telerik.Windows.Controls.GridView;

namespace Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements.Table.Views
{
   public class TableCellStyleSelector : StyleSelector
   {
      public Style PassStatusStyle { get; set; }
      public Style FailStatusStyle { get; set; }
      public Style InvalidStatusStyle { get; set; }

      public override Style SelectStyle(
         object item,
         DependencyObject container)
      {
         var cell = (GridViewCell)container;
         var dataColumn = cell.DataColumn;

         if (dataColumn.UniqueName== nameof(VMEvaluatedCharacteristic.Status)
            && item is VMEvaluatedCharacteristic characteristic)
            return SelectStatusCellStyle(characteristic.Status);

         return base.SelectStyle(item, container);
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
