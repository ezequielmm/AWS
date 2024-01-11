// <copyright file="ColumnInfoTest.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using Mitutoyo.MiCAT.ReportModule.Domain.Components.TableView;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements.Table.Views.CustomGridView;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements.Table.Views.CustomGridView.CustomFilter;
using NUnit.Framework;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Test.VisualElements.ReportElements.Table.CustomGridView
{
   [TestFixture]
   [ExcludeFromCodeCoverage]
   public class ColumnInfoTest
   {
      [Test]
      [TestCase("ColumnName", 100, true, "DataFormat", 1, true, 2, SortingMode.Descending, 3, "DataFormat", TextAlignment.Right)]
      [TestCase("OtherName", 205, false, "SomeFormat", 0, false, -1, SortingMode.None, -1, "OtherFormat", TextAlignment.Center)]
      public void ColumnInfo_ShouldInitializeValues(
         string columnName,
         double width,
         bool isVisible,
         string dataFormat,
         int columnIndex,
         bool groupBy,
         int groupByIndex,
         SortingMode sortingMode,
         int sortingIndex,
         string format,
         TextAlignment textAlignment)
      {
         var column = new Column(columnName, width, isVisible, dataFormat, ContentAligment.Center);

         var columnInfo =
            new ColumnInfo(column, columnIndex, groupBy, groupByIndex, sortingMode, sortingIndex, format, textAlignment, new FilterInfo());

         Assert.IsNotNull(columnInfo);
         Assert.AreEqual(columnName, columnInfo.ColumnName);
         Assert.AreEqual(width, columnInfo.Width);
         Assert.AreEqual(isVisible, columnInfo.Visible);
         Assert.AreEqual(format, columnInfo.Format);
         Assert.AreEqual(columnIndex, columnInfo.ColumnIndex);
         Assert.AreEqual(groupBy, columnInfo.GroupBy);
         Assert.AreEqual(groupByIndex, columnInfo.GroupByIndex);
         Assert.AreEqual(sortingMode, columnInfo.SortingMode);
         Assert.AreEqual(sortingIndex, columnInfo.SortingIndex);
         Assert.AreEqual(format, columnInfo.Format);
         Assert.AreEqual(textAlignment, columnInfo.TextAlignment);
      }

      [Test]
      [TestCase("FeatureName")]
      [TestCase("CharacteristicType")]
      [TestCase("LowerTolerance")]
      [TestCase("UpperTolerance")]
      public void ColumnInfo_ShouldLocalizeCaptionText(string columnName)
      {
         var columnInfo = new ColumnInfo { ColumnName = columnName };

         Assert.AreEqual(Properties.Resources.ResourceManager.GetString(columnName), columnInfo.CaptionText());
      }

      [Test]
      [TestCase(1, 2, "ColumnName", true, 3, true, 150)]
      [TestCase(0, 0, "OtherColumnName", false, 0, false, 200)]
      public void ColumnInfo_WithSameValuesShouldBeEquals(int columnIndex, int defaultColumnIndex, string columnName, bool groupBy, int groupByIndex, bool visible, double width)
      {
         var columnInfo =
            new ColumnInfo
            {
               ColumnIndex = columnIndex,
               ColumnName = columnName,
               DefaultColumnIndex = defaultColumnIndex,
               GroupBy = groupBy,
               GroupByIndex = groupByIndex,
               Visible = visible,
               Width = width,
               FilterInfo = new FilterInfo
               {
                  Values = Array.Empty<FilterInfo.FilterValueItem>(),
                  SelectedValues = Array.Empty<string>(),
               },
            };
         var otherColumnInfo =
            new ColumnInfo
            {
               ColumnIndex = columnIndex,
               ColumnName = columnName,
               DefaultColumnIndex = defaultColumnIndex,
               GroupBy = groupBy,
               GroupByIndex = groupByIndex,
               Visible = visible,
               Width = width,
               FilterInfo = new FilterInfo
               {
                  Values = Array.Empty<FilterInfo.FilterValueItem>(),
                  SelectedValues = Array.Empty<string>(),
               },
            };

         Assert.IsNotNull(columnInfo);
         Assert.IsNotNull(otherColumnInfo);
         Assert.IsTrue(columnInfo.Equals(otherColumnInfo));
      }

      [Test]
      [TestCase("ColumnName", "OtherColumnName")]
      [TestCase("a value", "any")]
      [TestCase("", "Column")]
      [TestCase("", null)]
      public void ColumnInfo_WithDifferentColumnNameShouldNotBeEquals(string value, string otherValue)
      {
         var columnInfo = new ColumnInfo
         {
            ColumnName = value,
            FilterInfo = new FilterInfo
            {
               Values = Array.Empty<FilterInfo.FilterValueItem>(),
               SelectedValues = Array.Empty<string>(),
            },
         };
         var otherColumnInfo = new ColumnInfo
         {
            ColumnName = otherValue,
            FilterInfo = new FilterInfo
            {
               Values = Array.Empty<FilterInfo.FilterValueItem>(),
               SelectedValues = Array.Empty<string>(),
            },
         };

         Assert.IsNotNull(columnInfo);
         Assert.IsNotNull(otherColumnInfo);
         Assert.IsFalse(columnInfo.Equals(otherColumnInfo));
      }

      [Test]
      [TestCase(1, 2)]
      [TestCase(0, 1)]
      [TestCase(-1, -2)]
      [TestCase(0, 100)]
      public void ColumnInfo_WidthDifferentColumnIndexShouldNotBeEquals(int value, int otherValue)
      {
         var columnInfo = new ColumnInfo { ColumnIndex = value, FilterInfo = new FilterInfo(), };
         var otherColumnInfo = new ColumnInfo { ColumnIndex = otherValue, FilterInfo = new FilterInfo(), };

         Assert.IsNotNull(columnInfo);
         Assert.IsNotNull(otherColumnInfo);
         Assert.IsFalse(columnInfo.Equals(otherColumnInfo));
      }

      [Test]
      [TestCase(1, 2)]
      [TestCase(1, 1.2)]
      [TestCase(-1, -2)]
      [TestCase(0, 100)]
      public void ColumnInfo_WidthDifferentWidthShouldNotBeEquals(double value, double otherValue)
      {
         var columnInfo = new ColumnInfo { Width = value, FilterInfo = new FilterInfo(), };
         var otherColumnInfo = new ColumnInfo { Width = otherValue, FilterInfo = new FilterInfo(), };

         Assert.IsNotNull(columnInfo);
         Assert.IsNotNull(otherColumnInfo);
         Assert.IsFalse(columnInfo.Equals(otherColumnInfo));
      }
   }
}
