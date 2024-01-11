// <copyright file="ColumnInfoList.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Mitutoyo.MiCAT.ReportModule.Domain.Components;
using Mitutoyo.MiCAT.ReportModule.Domain.Components.TableView;
using Mitutoyo.MiCAT.ReportModule.GUI.Strings;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements.Table.ViewModels;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements.Table.Views.CustomGridView.CustomFilter;
using static Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements.Table.Views.CustomGridView.CustomFilter.FilterInfo;
using DomainColumn = Mitutoyo.MiCAT.ReportModule.Domain.Components.TableView.Column;
using OrderFunc = System.Func<System.Func<Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements.Table.ViewModels.VMEvaluatedCharacteristic, object>, System.Linq.IOrderedEnumerable<Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements.Table.ViewModels.VMEvaluatedCharacteristic>>;

namespace Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements.Table.Views.CustomGridView
{
   public class ColumnInfoList : List<ColumnInfo>, IEquatable<ColumnInfoList>
   {
      private readonly IEnumerable<SortingColumn> _sortingColumns;
      private readonly IEnumerable<string> _groupbyColumns;
      private readonly IEnumerable<string> _characteristicTypes;
      private readonly IEnumerable<string> _details;

      public ColumnInfoList(ReportTableView reportTableView, IEnumerable<VMEvaluatedCharacteristic> initialSource, IEnumerable<string> characteristicTypes, IEnumerable<string> details)
      {
         _sortingColumns = reportTableView.Sorting.Where(s => s.Mode != SortingMode.None);
         _groupbyColumns = reportTableView.GroupBy;
         _characteristicTypes = characteristicTypes;
         _details = details;

         int columnIndex = 0;

         foreach (var domainColumn in reportTableView.Columns)
         {
            AddNewColumn(domainColumn, columnIndex, reportTableView.Sorting, reportTableView.GroupBy, reportTableView.Filters);
            columnIndex++;
         }

         UpdateFilterInfoValues(initialSource);
      }

      public IEnumerable<VMEvaluatedCharacteristic> Update(IEnumerable<VMEvaluatedCharacteristic> initialSource)
      {
         UpdateFilterInfoValues(initialSource);

         return OrderBySource(FilterBySource(initialSource));
      }

      public override bool Equals(object obj)
      {
         return Equals(obj as ColumnInfoList);
      }

      public bool Equals(ColumnInfoList other)
      {
         if (other == null || other.Count != this.Count)
            return false;

         for (int i = 0; i < this.Count; i++)
         {
            if (!this[i].Equals(other[i]))
               return false;
         }

         return true;
      }

      private void UpdateFilterInfoValues(IEnumerable<VMEvaluatedCharacteristic> initialSource)
      {
         this
            .Where(x => x.FilterInfo.IsFilterable)
            .ToList()
            .ForEach(columnInfo =>
            {
               var values = initialSource
                  ?.Select(x => GetColumnValue(columnInfo.ColumnName, x))
                     .Distinct()
                     .Cast<string>()
                     .ToArray() ??
                  Array.Empty<string>();

               columnInfo.FilterInfo.Values = GetFilterInfoValues(columnInfo, initialSource, _characteristicTypes, _details);
            });
      }

      private FilterValueItem[] GetFilterInfoValues(ColumnInfo columnInfo, IEnumerable<VMEvaluatedCharacteristic> initialSource, IEnumerable<string> characteristicTypes, IEnumerable<string> characteristicDetails)
      {
         switch (columnInfo.ColumnName)
         {
            case nameof(VMEvaluatedCharacteristic.Status):
               return GetStatuses(initialSource);

            case nameof(VMEvaluatedCharacteristic.CharacteristicType):
               return GetFilterLocalizedCharacteristicType(initialSource, characteristicTypes);

            case nameof(VMEvaluatedCharacteristic.Details):
               return GetFilterLocalizedCharacteristicDetails(initialSource, characteristicDetails);

            default:
               return GetDistinctColumnValuesFromSource(columnInfo, initialSource);
         }
      }

      private FilterValueItem[] GetFilterLocalizedCharacteristicType(IEnumerable<VMEvaluatedCharacteristic> initialSource, IEnumerable<string> characteristicTypes)
      {
         var localizedCharacteristicTypes = characteristicTypes?.Select(c => DataServiceLocalizationFinder.FindCharacteristicTypeName(c));
         return GetFilterValues(initialSource, localizedCharacteristicTypes, vm => vm.CharacteristicType);
      }

      private FilterValueItem[] GetFilterLocalizedCharacteristicDetails(IEnumerable<VMEvaluatedCharacteristic> initialSource, IEnumerable<string> characteristicDetails)
      {
         var localizedCharacteristicDetails = characteristicDetails?.Select(c => DataServiceLocalizationFinder.FindCharacteristicDetailValue(c));
         return GetFilterValuesWithEmptyOption(initialSource, localizedCharacteristicDetails, vm => vm.Details);
      }

      private FilterValueItem[] GetFilterValues(IEnumerable<VMEvaluatedCharacteristic> initialSource, IEnumerable<string> values, Func<VMEvaluatedCharacteristic, string> sourcePropertyFunc)
      {
         return values
                  ?.Select(x => new FilterValueItem
                  {
                     Value = x,
                     DisplayText = x,
                     IsDisabled = initialSource.Count() > 0 && !initialSource.Any(source => sourcePropertyFunc(source).Equals(x)),
                  })
                  .ToArray();
      }

      private FilterValueItem[] GetFilterValuesWithEmptyOption(IEnumerable<VMEvaluatedCharacteristic> initialSource, IEnumerable<string> values, Func<VMEvaluatedCharacteristic, string> sourcePropertyFunc)
      {
         var filterValueItems = GetFilterValues(initialSource, values, sourcePropertyFunc).ToList();
         filterValueItems.Add(new FilterValueItem()
         {
            DisplayText = DataServiceLocalizationFinder.FindLocalizedString("BlankStatus"),
            Value = string.Empty
         });

         return filterValueItems.ToArray();
      }

      private FilterValueItem[] GetStatuses(IEnumerable<VMEvaluatedCharacteristic> initialSource)
      {
         var passValue = DataServiceLocalizationFinder.FindLocalizedString("Pass");
         var failValue = DataServiceLocalizationFinder.FindLocalizedString("Fail");
         var notEvaluatedValue = DataServiceLocalizationFinder.FindLocalizedString("NotEvaluated");

         var invalidValue = DataServiceLocalizationFinder.FindLocalizedString("Invalid");
         return new[]
               {
                  new FilterValueItem
                  {
                     DisplayText = passValue,
                     Value = passValue,
                     IsDisabled = initialSource.Count() > 0 && !initialSource.Any(i => i.Status.Equals(passValue)),
                  },
                  new FilterValueItem
                  {
                     DisplayText = failValue,
                     Value = failValue,
                     IsDisabled = initialSource.Count() > 0 && !initialSource.Any(i => i.Status.Equals(failValue)),
                  },
                  new FilterValueItem
                  {
                     DisplayText = notEvaluatedValue == String.Empty ? DataServiceLocalizationFinder.FindLocalizedString("BlankStatus"): notEvaluatedValue,
                     Value = notEvaluatedValue,
                     IsDisabled = initialSource.Count() > 0 && !initialSource.Any(i => i.Status.Equals(notEvaluatedValue)),
                  },
                  new FilterValueItem
                  {
                     DisplayText = invalidValue,
                     Value = invalidValue,
                     IsDisabled = initialSource.Count() > 0 && !initialSource.Any(i => i.Status.Equals(invalidValue)),
                  }
               };
      }

      private FilterValueItem[] GetDistinctColumnValuesFromSource(ColumnInfo columnInfo, IEnumerable<VMEvaluatedCharacteristic> initialSource)
      {
         return initialSource
                  ?.Select(x => GetColumnValue(columnInfo.ColumnName, x))
                     .Cast<string>()
                     .Distinct()
                     .Select(x => new FilterValueItem
                     {
                        // REFACTOR: Will be revisited after we get further feedback.
                        DisplayText = x == null ? DataServiceLocalizationFinder.FindLocalizedString("BlankStatus") : x,
                        Value = x,
                     })
                     .ToArray() ??
                  Array.Empty<FilterValueItem>();
      }

      private IEnumerable<VMEvaluatedCharacteristic> OrderBySource(IEnumerable<VMEvaluatedCharacteristic> initialSource)
      {
         if (initialSource == null || initialSource.Count() == 0)
         {
            return initialSource;
         }

         var allColumnsToSort = GetOrderedColumnsToSort();

         if (allColumnsToSort.Count() == 0)
         {
            return initialSource;
         }

         var firstColumn = allColumnsToSort.First();
         var orderedSource = OrderSource(firstColumn, initialSource.OrderBy, initialSource.OrderByDescending);

         var nextColumns = allColumnsToSort.Skip(1);

         foreach (var column in nextColumns)
         {
            orderedSource = OrderSource(column, orderedSource.ThenBy, orderedSource.ThenByDescending);
         }

         return orderedSource;
      }

      private IEnumerable<VMEvaluatedCharacteristic> FilterBySource(IEnumerable<VMEvaluatedCharacteristic> initialSource)
      {
         if (initialSource == null || initialSource.Count() == 0)
         {
            return initialSource;
         }

         var columnsToFilter = this.Where(x => x.FilterInfo.IsFilterable && x.FilterInfo.SelectedValues.Any()).ToList();

         var filteredSoruce = initialSource;

         columnsToFilter.ForEach(x => filteredSoruce = FilterSource(filteredSoruce, x));

         return filteredSoruce;
      }

      private IEnumerable<VMEvaluatedCharacteristic> FilterSource(IEnumerable<VMEvaluatedCharacteristic> initialSource, ColumnInfo columnInfo)
      {
         var filteredSource = initialSource
            .Where(x => columnInfo.FilterInfo.SelectedValues.Contains(GetColumnValue(columnInfo.ColumnName, x)));

         return filteredSource;
      }

      private IEnumerable<SortingColumn> GetOrderedColumnsToSort()
      {
         var groupByColumns = _groupbyColumns
            .Select(groupedColumn => new SortingColumn(groupedColumn, GetGroupBySortingMode(groupedColumn)));

         IEnumerable<SortingColumn> allColumns = new List<SortingColumn>(groupByColumns);
         allColumns = allColumns.Union(_sortingColumns);

         return allColumns;
      }

      private SortingMode GetGroupBySortingMode(string columnName)
      {
         var alreadySortedColumn = GetSortedColumn(columnName);
         var defaultSorting = SortingMode.Ascending;

         return alreadySortedColumn != null ? alreadySortedColumn.Mode : defaultSorting;
      }

      private SortingColumn GetSortedColumn(string columnName)
      {
         return _sortingColumns.SingleOrDefault(sc => sc.ColumnName == columnName);
      }

      private IOrderedEnumerable<VMEvaluatedCharacteristic> OrderSource(SortingColumn sortingColumn, OrderFunc orderAscendingFunc, OrderFunc orderDescendingFunc)
      {
         IOrderedEnumerable<VMEvaluatedCharacteristic> orderedSource = null;

         switch (sortingColumn.Mode)
         {
            case SortingMode.Ascending:
               orderedSource = orderAscendingFunc(s => GetColumnValue(sortingColumn.ColumnName, s));
               break;
            case SortingMode.Descending:
               orderedSource = orderDescendingFunc(s => GetColumnValue(sortingColumn.ColumnName, s));
               break;
            default:
               break;
         }

         return orderedSource;
      }

      private void AddNewColumn(DomainColumn domainColumn, int columnIndex, IEnumerable<SortingColumn> sortingColumns, IEnumerable<string> groupByColumns, IEnumerable<FilterColumn> filterColumns)
      {
         var sortingInfo = GetSortingInfo(domainColumn, sortingColumns);
         var isColumnGrouped = IsColumnGrouped(domainColumn, groupByColumns);
         var filterInfo = GetColumnFilterInfo(domainColumn, filterColumns);
         var gridColumn = new ColumnInfo(domainColumn, columnIndex, isColumnGrouped.Item1, isColumnGrouped.Item2, sortingInfo.Mode, sortingInfo.Index, domainColumn.DataFormat, MapAligment(domainColumn.ContentAligment), filterInfo);
         Add(gridColumn);
      }

      private object GetColumnValue(string columnName, VMEvaluatedCharacteristic source)
      {
         var sourceType = source.GetType();
         var propertyInfo = sourceType.GetProperty(columnName);
         return propertyInfo.GetValue(source);
      }

      private (bool, int) IsColumnGrouped(DomainColumn domainColumn, IEnumerable<string> groupByColumns)
      {
         var columns = groupByColumns.Select((c, index) => new { Name = c, Index = index });
         var column = columns.SingleOrDefault(c => c.Name == domainColumn.Name);

         return ((column != null) ? (true, column.Index) : (false, -1));
      }

      private readonly string[] FilterableColumnNames = new[]
      {
            nameof(VMEvaluatedCharacteristic.FeatureName),
            nameof(VMEvaluatedCharacteristic.CharacteristicType),
            nameof(VMEvaluatedCharacteristic.Status),
            nameof(VMEvaluatedCharacteristic.Details)
      };

      private FilterInfo GetColumnFilterInfo(DomainColumn domainColumn, IEnumerable<FilterColumn> filterColumns)
      {
         var isFilterable = FilterableColumnNames.Contains(domainColumn.Name);
         var filterColumn = filterColumns.SingleOrDefault(x => x.ColumnName == domainColumn.Name);
         var selectedValues = filterColumn?.SelectedValues ?? Array.Empty<string>();

         return new FilterInfo
         {
            IsFilterable = isFilterable,
            SelectedValues = selectedValues,
            Values = Array.Empty<FilterValueItem>(),
         };
      }

      private SortingInfo GetSortingInfo(DomainColumn domainColumn, IEnumerable<SortingColumn> sortingColumns)
      {
         var sortColumn = sortingColumns
           .Select((column, index) => new { Name = column.ColumnName, column.Mode, Index = index })
           .SingleOrDefault(c => c.Name == domainColumn.Name);

         var sortingMode = sortColumn?.Mode ?? SortingMode.None;
         var sortingIndex = sortColumn?.Index ?? -1;

         return new SortingInfo(sortingMode, sortingIndex);
      }

      private TextAlignment MapAligment(ContentAligment contentAligment)
      {
         switch (contentAligment)
         {
            case ContentAligment.Left:
               return TextAlignment.Left;
            case ContentAligment.Center:
               return TextAlignment.Center;
            case ContentAligment.Right:
               return TextAlignment.Right;
         }

         return TextAlignment.Left;
      }

      public override int GetHashCode()
      {
         var baseHashCode = base.GetHashCode();
         return (baseHashCode, _sortingColumns, _groupbyColumns).GetHashCode();
      }
   }
}
