// <copyright file="TableViewControllerFake.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.ReportModule.Domain.Components;
using Mitutoyo.MiCAT.ReportModule.Domain.Components.TableView;
using Mitutoyo.MiCAT.ReportModuleApp.Controllers;

namespace Mitutoyo.MiCAT.ReportModule.Setup.Export.Fakes
{
   [ExcludeFromCodeCoverage]
   public class TableViewControllerFake : ITableViewController
   {
      public void AddTableView(int x, int y)
      {
      }

      public void AddTableViewOnFakeSpace(int x, int y, int fakeSpaceStartPosition, int fakeSpaceHeight)
      {
      }

      public void FilterByColumn(Id<ReportTableView> tableViewId, string columnName, string[] selectedValues)
      {
      }

      public void GroupByColumn(Id<ReportTableView> tableViewId, string columnName)
      {
      }

      public void HideColumn(Id<ReportTableView> tableViewId, string columnName)
      {
      }

      public void RemoveGroupByColumn(Id<ReportTableView> tableViewId, string columnName)
      {
      }

      public void ReorderColumn(Id<ReportTableView> tableViewId, string columnName, int newIndex)
      {
      }

      public void ResizeColumnWidths(Id<ReportTableView> tableViewId, IEnumerable<KeyValuePair<string, double>> columnWidths)
      {
      }

      public void ShowColumn(Id<ReportTableView> tableViewId, string columnName)
      {
      }

      public void SortByAddColumn(Id<ReportTableView> tableViewId, SortingColumn columnSorting)
      {
      }

      public void SortByColumn(Id<ReportTableView> tableViewId, SortingColumn columnSorting)
      {
      }
   }
}