// <copyright file="ColumnWidthsChangedEventArgs.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using System.Collections.Generic;

namespace Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements.Table.Views.CustomGridView
{
   public class ColumnWidthsChangedEventArgs : EventArgs
   {
      public IEnumerable<ColumnWidthInfo> ColumnWidthInfos { get; }

      public ColumnWidthsChangedEventArgs(IEnumerable<ColumnWidthInfo> columnWidthInfos)
      {
         ColumnWidthInfos = columnWidthInfos;
      }

      public class ColumnWidthInfo
      {
         public ColumnWidthInfo(string columnName, double width)
         {
            ColumnName = columnName;
            Width = width;
         }

         public string ColumnName { get; }
         public double Width { get; }
      }
   }
}
