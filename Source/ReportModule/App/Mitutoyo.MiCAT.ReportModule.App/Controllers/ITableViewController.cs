// <copyright file="ITableViewController.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Collections.Generic;
using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.ReportModule.Domain.Components;
using Mitutoyo.MiCAT.ReportModule.Domain.Components.TableView;

namespace Mitutoyo.MiCAT.ReportModuleApp.Controllers
{
   public interface ITableViewController
   {
      void AddTableViewOnFakeSpace(int x, int y, int fakeSpaceStartPosition, int fakeSpaceHeight);
      void AddTableView(int x, int y);

      void HideColumn(Id<ReportTableView> tableViewId, string columnName);
      void ShowColumn(Id<ReportTableView> tableViewId, string columnName);
      void SortByAddColumn(Id<ReportTableView> tableViewId, SortingColumn columnSorting);
      void SortByColumn(Id<ReportTableView> tableViewId, SortingColumn columnSorting);
      void GroupByColumn(Id<ReportTableView> tableViewId, string columnName);
      void RemoveGroupByColumn(Id<ReportTableView> tableViewId, string columnName);
      void FilterByColumn(Id<ReportTableView> tableViewId, string columnName, string[] selectedValues);
      void ReorderColumn(Id<ReportTableView> tableViewId, string columnName, int newIndex);
      void ResizeColumnWidths(Id<ReportTableView> tableViewId, IEnumerable<KeyValuePair<string, double>> columnWidths);
   }
}
