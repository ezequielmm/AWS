// <copyright file="ColumnSortEvent.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using Mitutoyo.MiCAT.ReportModule.Domain.Components.TableView;

namespace Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements.Table.Views.CustomGridView
{
   public class ColumnSortEvent
   {
      public ColumnSortEvent(string columnName, SortingMode sortingMode, bool isMultiple)
      {
         ColumnName = columnName;
         SortingMode = sortingMode;
         IsMultiple = isMultiple;
      }

      public string ColumnName { get; }
      public SortingMode SortingMode { get; }
      public bool IsMultiple { get; }
   }
}
