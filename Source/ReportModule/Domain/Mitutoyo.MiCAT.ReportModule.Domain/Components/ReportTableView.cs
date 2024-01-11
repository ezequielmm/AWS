// <copyright file="ReportTableView.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.ReportModule.Domain.Components.TableView;

namespace Mitutoyo.MiCAT.ReportModule.Domain.Components
{
   public sealed class ReportTableView : ReportComponentBase<ReportTableView>, IReportComponent
   {
      private const int DEFAULT_HEIGHT = 54;
      private const int DEFAULT_WIDTH = 707;

      public ReportTableView (IItemId reportSectionId, int x, int y)
         : this(new ReportComponentPlacement(reportSectionId, x, y, DEFAULT_WIDTH, DEFAULT_HEIGHT))
      { }
      public ReportTableView(ReportComponentPlacement placement)
         : this(Guid.NewGuid(), placement, Array.Empty<Column>(), Array.Empty<SortingColumn>(), Array.Empty<string>(), Array.Empty<FilterColumn>())
      { }

      public ReportTableView(
         Id<ReportTableView> id,
         ReportComponentPlacement placement,
         IEnumerable<Column> columns,
         IEnumerable<SortingColumn> sorting,
         IEnumerable<string> groupBy,
         IEnumerable<FilterColumn> filters)
         : base(id, placement)
      {
         Columns = columns.ToImmutableList();
         Sorting = sorting.ToImmutableList();
         GroupBy = groupBy.ToImmutableList();
         Filters = filters.ToImmutableList();
      }

      public IImmutableList<Column> Columns { get; }
      public IImmutableList<SortingColumn> Sorting { get; }
      public IEnumerable<FilterColumn> Filters { get; }
      public IImmutableList<string> GroupBy { get; }

      public ReportTableView WithHiddenColumn(string columnName)
      {
         return WithSetVisibilityColumn(columnName, false);
      }

      public ReportTableView WithVisibleColumn(string columnName)
      {
         return WithSetVisibilityColumn(columnName, true);
      }

      private ReportTableView WithSetVisibilityColumn(string columnName, bool isVisible)
      {
         var column = Columns.SingleOrDefault(c => c.Name == columnName);

         var newColumn = column.WithIsVisible(isVisible);
         var newColumnList = Columns.Replace(column, newColumn);

         return new ReportTableView(Id, Placement, newColumnList, Sorting, GroupBy, Filters);
      }

      public ReportTableView WithDefaultColumns()
      {
         return new ReportTableView(Id, Placement, GetDefaultColumns(), Sorting, GroupBy, Filters);
      }

      public ReportTableView WithSortingByColumn(SortingColumn sortingColumn)
      {
         var newSorting = Sorting.ToList();

         newSorting.RemoveAll(sc => sc.ColumnName == sortingColumn.ColumnName);

         if (sortingColumn.Mode != SortingMode.None)
            newSorting.Add(sortingColumn);

         return new ReportTableView(Id, Placement, Columns, newSorting, GroupBy, Filters);
      }

      public ReportTableView WithoutSorting()
      {
         var newSorting = Sorting.Clear();

         return new ReportTableView(Id, Placement, Columns, newSorting, GroupBy, Filters);
      }

      public ReportTableView WithGroupingByColumn(string columnName)
      {
         var newGrouping = GroupBy.ToList();

         if (GroupBy.Contains(columnName))
            newGrouping.Remove(columnName);

         newGrouping.Add(columnName);
         return new ReportTableView(Id, Placement, Columns, Sorting, newGrouping, Filters);
      }

      public ReportTableView WithoutGroupingByColumn(string columnName)
      {
         var newGrouping = GroupBy.ToList();

         if (GroupBy.Contains(columnName))
            newGrouping.Remove(columnName);

         return new ReportTableView(Id, Placement, Columns, Sorting, newGrouping, Filters);
      }

      public ReportTableView WithFilterByColumn(string columnName, string[] selectedValues)
      {
         var newFilters = Filters.Where(x => x.ColumnName != columnName).ToList();
         newFilters.Add(new FilterColumn(columnName, selectedValues));

         return new ReportTableView(Id, Placement, Columns, Sorting, GroupBy, newFilters);
      }

      public ReportTableView WithReorderingColumn(string columnName, int newIndex)
      {
         var column = Columns.Single(c => c.Name == columnName);

         var columns = Columns
            .RemoveAll(c => c.Name == columnName)
            .Insert(newIndex, column);

         return new ReportTableView(Id, Placement, columns, Sorting, GroupBy, Filters);
      }

      public ReportTableView WithColumnWidth(string columnName, double width)
      {
         var column = Columns.Single(x => x.Name == columnName);
         var newColumns = Columns.Replace(column, column.WithWidth(width));

         return new ReportTableView(Id, Placement, newColumns, Sorting, GroupBy, Filters);
      }

      public override ReportTableView WithPosition(int x, int y)
      {
         return new ReportTableView(Id, Placement.WithPosition(x, y), Columns, Sorting, GroupBy, Filters);
      }

      public override ReportTableView WithSize(int widht, int height)
      {
         return new ReportTableView(Id, Placement.WithSize(widht, height), Columns, Sorting, GroupBy, Filters);
      }

      private IEnumerable<Column> GetDefaultColumns()
      {
         return new Column[]
         {
            new FeatureNameColumn(),
            new CharacteristicTypeColumn(),
            new DetailsColumn(),
            new NominalColumn(),
            new UpperToleranceColumn(),
            new LowerToleranceColumn(),
            new MeasuredColumn(),
            new DeviationColumn(),
            new StatusColumn()
         };
      }
   }
}
