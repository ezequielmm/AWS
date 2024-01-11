// <copyright file="TableViewController.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using System.Collections.Generic;
using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.ReportModule.Domain.Components;
using Mitutoyo.MiCAT.ReportModule.Domain.Components.TableView;
using Mitutoyo.MiCAT.ReportModuleApp.AppState.Extensions;
using Mitutoyo.MiCAT.ReportModuleApp.Services.ReportComponents;

namespace Mitutoyo.MiCAT.ReportModuleApp.Controllers
{
   public class TableViewController : ITableViewController
   {
      private readonly IAppStateHistory _appStateHistory;
      private readonly IReportComponentService _reportComponentService;

      public TableViewController(IAppStateHistory appStateHistory, IReportComponentService reportComponentService)
      {
         _appStateHistory = appStateHistory;
         _reportComponentService = reportComponentService;
      }

      public void AddTableView(int x, int y)
      {
         IReportComponent component = new ReportTableView(_appStateHistory.CurrentSnapShot.GetReportBodyId(), x, y).WithDefaultColumns();

         _appStateHistory.RunUndoable(s => _reportComponentService.AddComponent(s, component));
      }

      public void AddTableViewOnFakeSpace(int x, int y, int fakeSpaceStartPosition, int fakeSpaceHeight)
      {
         IReportComponent component = new ReportTableView(_appStateHistory.CurrentSnapShot.GetReportBodyId(), x, y).WithDefaultColumns();

         _appStateHistory.RunUndoable(s => _reportComponentService.AddComponentOnFakeSpace(s, component, fakeSpaceStartPosition, fakeSpaceHeight));
      }

      public void HideColumn(Id<ReportTableView> tableViewId, string columnName)
      {
         SetVisibilityColumn(tableViewId, columnName, tableview => tableview.WithHiddenColumn);
      }

      public void ShowColumn(Id<ReportTableView> tableViewId, string columnName)
      {
         SetVisibilityColumn(tableViewId, columnName, tableview => tableview.WithVisibleColumn);
      }

      private void SetVisibilityColumn(Id<ReportTableView> tableViewId, string columnName, Func<ReportTableView, Func<string, ReportTableView>> getVisibilityFunc)
      {
         var oldReportTableView = _appStateHistory.CurrentSnapShot.GetItem(tableViewId);

         var visibilityFunc = getVisibilityFunc(oldReportTableView);
         var newReportTableView = visibilityFunc.Invoke(columnName);

         _appStateHistory.RunUndoable(s => s.UpdateItem(oldReportTableView, newReportTableView));
      }

      public void SortByAddColumn(Id<ReportTableView> tableViewId, SortingColumn columnSorting)
      {
         var reportTableView = _appStateHistory.CurrentSnapShot.GetItem(tableViewId);
         _appStateHistory.RunUndoable(s => s.UpdateItem(reportTableView, reportTableView.WithSortingByColumn(columnSorting)));
      }

      public void SortByColumn(Id<ReportTableView> tableViewId, SortingColumn columnSorting)
      {
         var reportTableView = _appStateHistory.CurrentSnapShot.GetItem(tableViewId);
         _appStateHistory.RunUndoable(s => s.UpdateItem(reportTableView, reportTableView.WithoutSorting().WithSortingByColumn(columnSorting)));
      }

      public void GroupByColumn(Id<ReportTableView> tableViewId, string columnName)
      {
         var reportTableView = _appStateHistory.CurrentSnapShot.GetItem(tableViewId);
         _appStateHistory.RunUndoable(s => s.UpdateItem(reportTableView, reportTableView.WithGroupingByColumn(columnName)));
      }

      public void RemoveGroupByColumn(Id<ReportTableView> tableViewId, string columnName)
      {
         var reportTableView = _appStateHistory.CurrentSnapShot.GetItem(tableViewId);
         _appStateHistory.RunUndoable(s => s.UpdateItem(reportTableView, reportTableView.WithoutGroupingByColumn(columnName)));
      }

      public void FilterByColumn(Id<ReportTableView> tableViewId, string columnName, string[] selectedValues)
      {
         var reportTableView = _appStateHistory.CurrentSnapShot.GetItem(tableViewId);
         _appStateHistory.RunUndoable(s => s.UpdateItem(reportTableView, reportTableView.WithFilterByColumn(columnName, selectedValues)));
      }

      public void ReorderColumn(Id<ReportTableView> tableViewId, string columnName, int newIndex)
      {
         var reportTableView = _appStateHistory.CurrentSnapShot.GetItem(tableViewId);
         _appStateHistory.RunUndoable(s => s.UpdateItem(reportTableView, reportTableView.WithReorderingColumn(columnName, newIndex)));
      }

      public void ResizeColumnWidths(Id<ReportTableView> tableViewId, IEnumerable<KeyValuePair<string, double>> columnWidths)
      {
         var reportTableView = _appStateHistory.CurrentSnapShot.GetItem(tableViewId);
         var newReportTableView = reportTableView;
         foreach (var columnWidth in columnWidths)
         {
            newReportTableView = newReportTableView.WithColumnWidth(columnWidth.Key, columnWidth.Value);
         }

         _appStateHistory.RunUndoable(s => s.UpdateItem(reportTableView, newReportTableView));
      }
   }
}
