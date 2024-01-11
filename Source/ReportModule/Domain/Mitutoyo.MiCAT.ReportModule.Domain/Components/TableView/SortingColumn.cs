﻿// <copyright file="SortingColumn.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

namespace Mitutoyo.MiCAT.ReportModule.Domain.Components.TableView
{
   public enum SortingMode { None, Ascending, Descending }

   public class SortingColumn
   {
      public SortingColumn(string columnName, SortingMode mode)
      {
         ColumnName = columnName;
         Mode = mode;
      }

      public string ColumnName { get; }
      public SortingMode Mode { get; }
   }
}
