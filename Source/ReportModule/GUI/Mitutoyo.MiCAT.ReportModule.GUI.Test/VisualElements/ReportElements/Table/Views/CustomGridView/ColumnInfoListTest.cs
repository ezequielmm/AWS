// <copyright file="ColumnInfoListTest.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Mitutoyo.MiCAT.ReportModule.Domain.Components;
using Mitutoyo.MiCAT.ReportModule.Domain.Components.ReportSections;
using Mitutoyo.MiCAT.ReportModule.Domain.Components.TableView;
using Mitutoyo.MiCAT.ReportModule.Domain.Data;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements.Table.ViewModels;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements.Table.Views.CustomGridView;
using NUnit.Framework;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Test.VisualElements.ReportElements.Table.Views.CustomGridView
{
   [ExcludeFromCodeCoverage]
   [TestFixture]
   public class ColumnInfoListTest
   {
      [SetUp]
      public void Setup()
      {
      }

      [Test]
      public void ColumnInfoList_ShouldDefaultColumnsAreEqual()
      {
         //Arrange
         var reportTableView = new ReportTableView(new ReportComponentPlacement(new ReportBody().Id, 0, 0, 100, 100));
         reportTableView = reportTableView.WithDefaultColumns();

         var reportTableView2 = new ReportTableView(new ReportComponentPlacement(new ReportBody().Id, 0, 0, 100, 100));
         reportTableView2 = reportTableView2.WithDefaultColumns();

         var initialSource = new List<VMEvaluatedCharacteristic>();
         var characteristicTypes = new List<string>();
         var details = new List<string>();
         var columnInfoList = new ColumnInfoList(reportTableView, initialSource, characteristicTypes, details);
         var columnInfoList2 = new ColumnInfoList(reportTableView2, initialSource, characteristicTypes, details);

         //Act
         var areEqual = columnInfoList.Equals(columnInfoList2);

         //Assert
         Assert.True(areEqual);
      }

      [Test]
      public void ColumnInfoList_ShouldWithHiddenColumnsAreEqual()
      {
         //Arrange
         var reportTableView = new ReportTableView(new ReportComponentPlacement(new ReportBody().Id, 0, 0, 100, 100));
         reportTableView = reportTableView.WithDefaultColumns();
         reportTableView = reportTableView.WithHiddenColumn("Nominal");

         var reportTableView2 = new ReportTableView(new ReportComponentPlacement(new ReportBody().Id, 0, 0, 100, 100));
         reportTableView2 = reportTableView2.WithDefaultColumns();
         reportTableView2 = reportTableView2.WithHiddenColumn("Nominal");

         var initialSource = new List<VMEvaluatedCharacteristic>();
         var characteristicTypes = new List<string>();
         var details = new List<string>();

         var columnInfoList = new ColumnInfoList(reportTableView, initialSource, characteristicTypes, details);
         var columnInfoList2 = new ColumnInfoList(reportTableView2, initialSource, characteristicTypes, details);

         //Act
         var areEqual = columnInfoList.Equals(columnInfoList2);

         //Assert
         Assert.True(areEqual);
      }

      [Test]
      public void ColumnInfoList_ShouldWithSortingColumnsAreEqual()
      {
         //Arrange
         var reportTableView = new ReportTableView(new ReportComponentPlacement(new ReportBody().Id, 0, 0, 100, 100));
         reportTableView = reportTableView.WithDefaultColumns();
         reportTableView = reportTableView.WithSortingByColumn(new SortingColumn("Nominal", SortingMode.Descending));

         var reportTableView2 = new ReportTableView(new ReportComponentPlacement(new ReportBody().Id, 0, 0, 100, 100));
         reportTableView2 = reportTableView2.WithDefaultColumns();
         reportTableView2 = reportTableView2.WithSortingByColumn(new SortingColumn("Nominal", SortingMode.Descending));

         var initialSource = new List<VMEvaluatedCharacteristic>();
         var characteristicTypes = new List<string>();
         var details = new List<string>();

         var columnInfoList = new ColumnInfoList(reportTableView, initialSource, characteristicTypes, details);
         var columnInfoList2 = new ColumnInfoList(reportTableView2, initialSource, characteristicTypes, details);

         //Act
         var areEqual = columnInfoList.Equals(columnInfoList2);

         //Assert
         Assert.True(areEqual);
      }

      [Test]
      public void ColumnInfoList_ShouldWithGroupBygColumnsAreEqual()
      {
         //Arrange
         var reportTableView = new ReportTableView(new ReportComponentPlacement(new ReportBody().Id, 0, 0, 100, 100));
         reportTableView = reportTableView.WithDefaultColumns();
         reportTableView = reportTableView.WithGroupingByColumn("Nominal");

         var reportTableView2 = new ReportTableView(new ReportComponentPlacement(new ReportBody().Id, 0, 0, 100, 100));
         reportTableView2 = reportTableView2.WithDefaultColumns();
         reportTableView2 = reportTableView2.WithGroupingByColumn("Nominal");

         var initialSource = new List<VMEvaluatedCharacteristic>();
         var characteristicTypes = new List<string>();
         var details = new List<string>();

         var columnInfoList = new ColumnInfoList(reportTableView, initialSource, characteristicTypes, details);
         var columnInfoList2 = new ColumnInfoList(reportTableView2, initialSource, characteristicTypes, details);

         //Act
         var areEqual = columnInfoList.Equals(columnInfoList2);

         //Assert
         Assert.True(areEqual);
      }

      [Test]
      public void ColumnInfoList_ShouldWithFilterByColumnsAreEqual()
      {
         //Arrange
         var reportTableView = new ReportTableView(new ReportComponentPlacement(new ReportBody().Id, 0, 0, 100, 100));
         reportTableView = reportTableView.WithDefaultColumns();
         reportTableView = reportTableView.WithFilterByColumn(nameof(VMEvaluatedCharacteristic.FeatureName), new[] { "A value" });

         var reportTableView2 = new ReportTableView(new ReportComponentPlacement(new ReportBody().Id, 0, 0, 100, 100));
         reportTableView2 = reportTableView2.WithDefaultColumns();
         reportTableView2 = reportTableView2.WithFilterByColumn(nameof(VMEvaluatedCharacteristic.FeatureName), new[] { "A value" });

         var initialSource = new List<VMEvaluatedCharacteristic>();
         var characteristicTypes = new List<string>();
         var details = new List<string>();

         var columnInfoList = new ColumnInfoList(reportTableView, initialSource, characteristicTypes, details);
         var columnInfoList2 = new ColumnInfoList(reportTableView2, initialSource, characteristicTypes, details);

         //Act
         var areEqual = columnInfoList.Equals(columnInfoList2);

         //Assert
         Assert.True(areEqual);
      }

      [Test]
      public void ColumnInfoList_ShouldWithFilterByColumnsWithDifferentValuesAreNotEqual()
      {
         //Arrange
         var reportTableView = new ReportTableView(new ReportComponentPlacement(new ReportBody().Id, 0, 0, 100, 100));
         reportTableView = reportTableView.WithDefaultColumns();
         reportTableView = reportTableView.WithFilterByColumn(nameof(VMEvaluatedCharacteristic.FeatureName), new[] { "A value" });

         var reportTableView2 = new ReportTableView(new ReportComponentPlacement(new ReportBody().Id, 0, 0, 100, 100));
         reportTableView2 = reportTableView2.WithDefaultColumns();
         reportTableView2 = reportTableView2.WithFilterByColumn(nameof(VMEvaluatedCharacteristic.FeatureName), new[] { "Other value" });

         var initialSource = new List<VMEvaluatedCharacteristic>();
         var characteristicTypes = new List<string>();
         var details = new List<string>();

         var columnInfoList = new ColumnInfoList(reportTableView, initialSource, characteristicTypes, details);
         var columnInfoList2 = new ColumnInfoList(reportTableView2, initialSource, characteristicTypes, details);

         //Act
         var areEqual = columnInfoList.Equals(columnInfoList2);

         //Assert
         Assert.False(areEqual);
      }

      [Test]
      public void ColumnInfoList_ShouldWithDifferentColumnsAreNotEqual()
      {
         //Arrange
         var reportTableView = new ReportTableView(new ReportComponentPlacement(new ReportBody().Id, 0, 0, 100, 100));
         reportTableView = reportTableView.WithDefaultColumns();
         reportTableView = reportTableView.WithGroupingByColumn("Nominal");
         reportTableView = reportTableView.WithSortingByColumn(new SortingColumn("FeatureName", SortingMode.Descending));

         var reportTableView2 = new ReportTableView(new ReportComponentPlacement(new ReportBody().Id, 0, 0, 100, 100));
         reportTableView2 = reportTableView2.WithDefaultColumns();
         reportTableView2 = reportTableView2.WithHiddenColumn("Measured");

         var initialSource = new List<VMEvaluatedCharacteristic>();
         var characteristicTypes = new List<string>();
         var details = new List<string>();

         var columnInfoList = new ColumnInfoList(reportTableView, initialSource, characteristicTypes, details);
         var columnInfoList2 = new ColumnInfoList(reportTableView2, initialSource, characteristicTypes, details);

         //Act
         var areNotEqual = columnInfoList.Equals(columnInfoList2);

         //Assert
         Assert.False(areNotEqual);
      }

      [Test]
      public void ColumnInfoList_SholdInitializeFilterInfosAndHaveFilterableColumns()
      {
         //Arrange
         var reportTableView = new ReportTableView(new ReportComponentPlacement(new ReportBody().Id, 0, 0, 100, 100));
         reportTableView = reportTableView.WithDefaultColumns();

         var initialSource = new List<VMEvaluatedCharacteristic>();
         var characteristicTypes = new[] { "characteristic1", "characteristic2" };
         var details = new string[] { };

         var columnInfoList = new ColumnInfoList(reportTableView, initialSource, characteristicTypes, details);
         var filterInfoValues = columnInfoList.SelectMany(x => x.FilterInfo.Values);

         Assert.IsTrue(columnInfoList.All(x => x.FilterInfo != null));
         Assert.IsTrue(columnInfoList.Any(x => x.FilterInfo.IsFilterable));
         CollectionAssert.AllItemsAreNotNull(filterInfoValues);
         CollectionAssert.AllItemsAreNotNull(filterInfoValues.SelectMany(x => x.DisplayText));
         CollectionAssert.AllItemsAreNotNull(filterInfoValues.SelectMany(x => x.Value));
      }

      [Test]
      public void ColumnInfoList_ShouldInitializeCharacteristicTypeFilterInfoValuesAndShuldBeLocalized()
      {
         //Arrange
         var reportTableView = new ReportTableView(new ReportComponentPlacement(new ReportBody().Id, 0, 0, 100, 100));
         reportTableView = reportTableView.WithDefaultColumns();

         var initialSource = new List<VMEvaluatedCharacteristic>();
         var characteristicTypes = new[] { "AngleCoordinate", "RCoordinate", "No-Localized" };
         var details = new string[] { };

         var columnInfoList = new ColumnInfoList(reportTableView, initialSource, characteristicTypes, details);
         var characteristicTypeColumn =
            columnInfoList.Single(x => x.ColumnName == nameof(VMEvaluatedCharacteristic.CharacteristicType));
         var characteristicTypeColumnFilterValues = characteristicTypeColumn.FilterInfo.Values.Select(x => x.Value);

         Assert.IsTrue(columnInfoList.All(x => x.FilterInfo != null));
         Assert.IsTrue(columnInfoList.Any(x => x.FilterInfo.IsFilterable));
         Assert.IsTrue(characteristicTypeColumn.FilterInfo.IsFilterable);
         CollectionAssert.AreEqual(
            new[] { Properties.Resources.CharacteristicType_AngleCoordinate, Properties.Resources.CharacteristicType_RCoordinate, "No-Localized" }, characteristicTypeColumnFilterValues);
      }

      [Test]
      public void ColumnInfoList_ShouldInitializeCharacteristicDetailsFilterInfoValuesAndShuldBeLocalized()
      {
         //Arrange
         var reportTableView = new ReportTableView(new ReportComponentPlacement(new ReportBody().Id, 0, 0, 100, 100));
         reportTableView = reportTableView.WithDefaultColumns();

         var initialSource = new List<VMEvaluatedCharacteristic>();
         var characteristicDetails = new[] { "Min", "Max", "No-Localized", string.Empty, null };
         var characteristicTypes = new string[] { };

         var columnInfoList = new ColumnInfoList(reportTableView, initialSource, characteristicTypes, characteristicDetails);

         var characteristicDetailColumn =
            columnInfoList.Single(x => x.ColumnName == nameof(VMEvaluatedCharacteristic.Details));

         var characteristicDetailsColumnFilterOptions = characteristicDetailColumn.FilterInfo.Values;

         Assert.IsTrue(columnInfoList.All(x => x.FilterInfo != null));
         Assert.IsTrue(columnInfoList.Any(x => x.FilterInfo.IsFilterable));
         Assert.IsTrue(characteristicDetailColumn.FilterInfo.IsFilterable);
         Assert.AreEqual(6, characteristicDetailsColumnFilterOptions.ToArray().Length);
         Assert.AreEqual(Properties.Resources.CharacteristicDetail_Min, characteristicDetailsColumnFilterOptions[0].Value);
         Assert.AreEqual(Properties.Resources.CharacteristicDetail_Max, characteristicDetailsColumnFilterOptions[1].Value);
         Assert.AreEqual("No-Localized", characteristicDetailsColumnFilterOptions[2].Value);
         Assert.AreEqual(string.Empty, characteristicDetailsColumnFilterOptions[3].Value);
         Assert.AreEqual(string.Empty, characteristicDetailsColumnFilterOptions[4].Value);
         Assert.AreEqual(Properties.Resources.BlankStatus, characteristicDetailsColumnFilterOptions[5].DisplayText);
      }

      [Test]
      public void ColumnInfoList_ShouldInitializeStatusFilterInfoValuesAndShouldBeLocalized()
      {
         //Arrange
         var reportTableView = new ReportTableView(new ReportComponentPlacement(new ReportBody().Id, 0, 0, 100, 100));
         reportTableView = reportTableView.WithDefaultColumns();

         var initialSource = new List<VMEvaluatedCharacteristic>();
         var characteristicTypes = new List<string>();
         var details = new string[] { };

         var columnInfoList = new ColumnInfoList(reportTableView, initialSource, characteristicTypes, details);
         var statusColumn = columnInfoList.Single(x => x.ColumnName == nameof(VMEvaluatedCharacteristic.Status));
         var statusColumnFilterValues = statusColumn.FilterInfo.Values.Select(x => x.Value);

         Assert.IsTrue(columnInfoList.All(x => x.FilterInfo != null));
         Assert.IsTrue(columnInfoList.Any(x => x.FilterInfo.IsFilterable));
         Assert.IsTrue(statusColumn.FilterInfo.IsFilterable);
         Assert.AreEqual(4, statusColumnFilterValues.Count());
         CollectionAssert.AllItemsAreNotNull(statusColumnFilterValues);
         CollectionAssert.AreEqual(new[] { Properties.Resources.Pass, Properties.Resources.Fail, Properties.Resources.NotEvaluated, Properties.Resources.Invalid }, statusColumnFilterValues);
      }
      [Test]
      public void ColumnInfoList_ShoulGetOnlyInvalidFilterAvailable()
      {
         //Arrange
         var reportTableView = new ReportTableView(new ReportComponentPlacement(new ReportBody().Id, 0, 0, 100, 100));
         reportTableView = reportTableView.WithDefaultColumns();
         var evaluated = new VMEvaluatedCharacteristic(new EvaluatedCharacteristic(new Characteristic(), new CharacteristicActual(Guid.NewGuid(), "Invalid", null, null)));

         var initialSource = new List<VMEvaluatedCharacteristic>() { evaluated };
         var characteristicTypes = new List<string>();
         var details = new string[] { };

         var columnInfoList = new ColumnInfoList(reportTableView, initialSource, characteristicTypes, details);
         var statusColumn = columnInfoList.Single(x => x.ColumnName == nameof(VMEvaluatedCharacteristic.Status));

         var availableFilter = statusColumn.FilterInfo.Values.Where(x => x.IsDisabled == false);
         Assert.AreEqual(1, availableFilter.Count());
      }
   }
}
