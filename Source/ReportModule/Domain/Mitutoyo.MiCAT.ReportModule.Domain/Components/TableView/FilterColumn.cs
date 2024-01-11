// <copyright file="FilterColumn.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

namespace Mitutoyo.MiCAT.ReportModule.Domain.Components.TableView
{
   public class FilterColumn
   {
      public FilterColumn(string columnName, string[] selectedValues)
      {
         ColumnName = columnName;
         SelectedValues = selectedValues;
      }

      public string ColumnName { get; }
      public string[] SelectedValues { get; }
   }
}
