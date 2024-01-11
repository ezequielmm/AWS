// <copyright file="ColumnInfo.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using System.Windows;
using Mitutoyo.MiCAT.ReportModule.Domain.Components.TableView;
using Mitutoyo.MiCAT.ReportModule.GUI.Strings;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements.Table.Views.CustomGridView.CustomFilter;
using DomainColumn = Mitutoyo.MiCAT.ReportModule.Domain.Components.TableView.Column;

namespace Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements.Table.Views.CustomGridView
{
   public class ColumnInfo : IEquatable<ColumnInfo>
   {
      public ColumnInfo()
      {
      }

      public ColumnInfo(DomainColumn column, int columnIndex, bool groupBy, int groupByIndex, SortingMode sortingMode, int sortingIndex, string format, TextAlignment textAlignment, FilterInfo filterInfo)
      {
         ColumnIndex = columnIndex;
         DefaultColumnIndex = columnIndex;
         ColumnName = column.Name;
         Visible = column.IsVisible;
         GroupBy = groupBy;
         GroupByIndex = groupByIndex;
         SortingMode = sortingMode;
         SortingIndex = sortingIndex;
         Format = format;
         TextAlignment = textAlignment;
         Width = column.Width;
         FilterInfo = filterInfo;
      }

      public int ColumnIndex { get; set; }
      public int DefaultColumnIndex { get; set; }
      public string ColumnName { get; set; }
      public bool GroupBy { get; set; }
      public int GroupByIndex { get; set; }
      public SortingMode SortingMode { get; }
      public int SortingIndex { get; }
      public bool Visible { get; set; }
      public double Width { get; set; }
      public bool ImageColumn { get; internal set; }
      public string Format { get; }
      public TextAlignment TextAlignment { get; }
      public FilterInfo FilterInfo { get; set; }

      public string CaptionText()
      {
         return DataServiceLocalizationFinder.FindLocalizedString(ColumnName);
      }

      public override bool Equals(object obj)
      {
         return Equals(obj as ColumnInfo);
      }

      public bool Equals(ColumnInfo other)
      {
         return other != null
            && other.ColumnName == ColumnName
            && other.ColumnIndex == ColumnIndex
            && other.GroupBy == GroupBy
            && other.GroupByIndex == GroupByIndex
            && other.SortingMode == SortingMode
            && other.SortingIndex == SortingIndex
            && other.Visible == Visible
            && other.Width == Width
            && other.FilterInfo.Equals(FilterInfo);
      }

      public override int GetHashCode()
      {
         return ColumnName.GetHashCode();
      }
   }
}
