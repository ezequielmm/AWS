// <copyright file="ReportTableViewTest.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.ReportModule.Domain.Components;
using Mitutoyo.MiCAT.ReportModule.Domain.Components.ReportSections;
using Mitutoyo.MiCAT.ReportModule.Domain.Components.TableView;
using NUnit.Framework;

namespace Mitutoyo.MiCAT.ReportModule.Domain.Test.Components
{
   [ExcludeFromCodeCoverage]
   [TestFixture]
   public class ReportTableViewTest
   {
      [Test]
      public void ReportTableView_ShouldCreateWithDefaultColumns()
      {
         // Arrange
         var placement = new ReportComponentPlacement(new Id<ReportBody>(Guid.NewGuid()), 10, 10, 10, 10);
         var reportTableView = new ReportTableView(placement);

         //Act
         var newReportTableView = reportTableView.WithDefaultColumns();

         // Assert
         Assert.AreEqual(9, newReportTableView.Columns.Count);
         Assert.IsInstanceOf<FeatureNameColumn>(newReportTableView.Columns[0]);
         Assert.IsInstanceOf<CharacteristicTypeColumn>(newReportTableView.Columns[1]);
         Assert.IsInstanceOf<NominalColumn>(newReportTableView.Columns[3]);
         Assert.IsInstanceOf<UpperToleranceColumn>(newReportTableView.Columns[4]);
         Assert.IsInstanceOf<LowerToleranceColumn>(newReportTableView.Columns[5]);
         Assert.IsInstanceOf<MeasuredColumn>(newReportTableView.Columns[6]);
         Assert.IsInstanceOf<StatusColumn>(newReportTableView.Columns[8]);
         Assert.IsInstanceOf<DetailsColumn>(newReportTableView.Columns[2]);
         Assert.IsInstanceOf<DeviationColumn>(newReportTableView.Columns[7]);
      }

      [Test]
      public void ReportTableView_ShouldCreateWithHiddenColumn()
      {
         // Arrange
         var placement = new ReportComponentPlacement(new Id<ReportBody>(Guid.NewGuid()), 10, 10, 10, 10);
         var reportTableView = new ReportTableView(placement).WithDefaultColumns();
         //Act
         var newReportTableView = reportTableView
                                    .WithHiddenColumn("FeatureName")
                                    .WithHiddenColumn("Nominal")
                                    .WithHiddenColumn("Measured");

         // Assert
         Assert.IsFalse(newReportTableView.Columns.Single(c => c.Name == "FeatureName").IsVisible);
         Assert.IsFalse(newReportTableView.Columns.Single(c => c.Name == "Nominal").IsVisible);
         Assert.IsFalse(newReportTableView.Columns.Single(c => c.Name == "Measured").IsVisible);
         Assert.IsTrue(newReportTableView.Columns.Single(c => c.Name == "CharacteristicType").IsVisible);
         Assert.IsTrue(newReportTableView.Columns.Single(c => c.Name == "UpperTolerance").IsVisible);
         Assert.IsTrue(newReportTableView.Columns.Single(c => c.Name == "LowerTolerance").IsVisible);
         Assert.IsTrue(newReportTableView.Columns.Single(c => c.Name == "Details").IsVisible);
         Assert.IsTrue(newReportTableView.Columns.Single(c => c.Name == "Deviation").IsVisible);
         Assert.IsTrue(newReportTableView.Columns.Single(c => c.Name == "Status").IsVisible);
      }

      [Test]
      public void ReportTableView_ShouldCreateWithVisibleColumn()
      {
         // Arrange
         var placement = new ReportComponentPlacement(new Id<ReportBody>(Guid.NewGuid()), 10, 10, 10, 10);
         var reportTableViewHiddenColumns = new ReportTableView(placement)
                                                .WithDefaultColumns()
                                                .WithHiddenColumn("FeatureName")
                                                .WithHiddenColumn("Nominal")
                                                .WithHiddenColumn("Measured");

         // Act
         var newReportTableView = reportTableViewHiddenColumns
                                    .WithVisibleColumn("FeatureName")
                                    .WithVisibleColumn("Nominal")
                                    .WithVisibleColumn("Measured");
         // Assert
         Assert.IsTrue(newReportTableView.Columns.Single(c => c.Name == "FeatureName").IsVisible);
         Assert.IsTrue(newReportTableView.Columns.Single(c => c.Name == "Nominal").IsVisible);
         Assert.IsTrue(newReportTableView.Columns.Single(c => c.Name == "Measured").IsVisible);
         Assert.IsTrue(newReportTableView.Columns.Single(c => c.Name == "CharacteristicType").IsVisible);
         Assert.IsTrue(newReportTableView.Columns.Single(c => c.Name == "UpperTolerance").IsVisible);
         Assert.IsTrue(newReportTableView.Columns.Single(c => c.Name == "LowerTolerance").IsVisible);
         Assert.IsTrue(newReportTableView.Columns.Single(c => c.Name == "Details").IsVisible);
         Assert.IsTrue(newReportTableView.Columns.Single(c => c.Name == "Deviation").IsVisible);
         Assert.IsTrue(newReportTableView.Columns.Single(c => c.Name == "Status").IsVisible);
      }

      [Test]
      public void ReportTableView_ShouldCreateWithSortingColumn()
      {
         // Arrange
         var placement = new ReportComponentPlacement(new Id<ReportBody>(Guid.NewGuid()), 10, 10, 10, 10);
         var reportTableView = new ReportTableView(placement)
                                    .WithDefaultColumns();

         //Act
         var newReportTableView = reportTableView
                                 .WithSortingByColumn(new SortingColumn("FeatureName", SortingMode.Ascending))
                                 .WithSortingByColumn(new SortingColumn("Deviation", SortingMode.Ascending))
                                 .WithSortingByColumn(new SortingColumn("Nominal", SortingMode.Descending))
                                 .WithSortingByColumn(new SortingColumn("Deviation", SortingMode.None))
                                 .WithSortingByColumn(new SortingColumn("Measured", SortingMode.Ascending));

         // Assert
         Assert.AreEqual(3, newReportTableView.Sorting.Count());

         Assert.AreEqual("FeatureName", newReportTableView.Sorting[0].ColumnName);
         Assert.AreEqual(SortingMode.Ascending, newReportTableView.Sorting[0].Mode);

         Assert.AreEqual("Nominal", newReportTableView.Sorting[1].ColumnName);
         Assert.AreEqual(SortingMode.Descending, newReportTableView.Sorting[1].Mode);

         Assert.AreEqual("Measured", newReportTableView.Sorting[2].ColumnName);
         Assert.AreEqual(SortingMode.Ascending, newReportTableView.Sorting[2].Mode);
      }

      [Test]
      public void ReportTableView_ShouldCreateWithoutSorting()
      {
         // Arrange
         var placement = new ReportComponentPlacement(new Id<ReportBody>(Guid.NewGuid()), 10, 10, 10, 10);
         var reportTableView = new ReportTableView(placement)
                                    .WithDefaultColumns()
                                    .WithSortingByColumn(new SortingColumn("FeatureName", SortingMode.Ascending))
                                    .WithSortingByColumn(new SortingColumn("Nominal", SortingMode.Ascending))
                                    .WithSortingByColumn(new SortingColumn("Deviation", SortingMode.Ascending));
         // Act
         var newReportTableView = reportTableView.WithoutSorting();

         // Assert
         CollectionAssert.IsEmpty(newReportTableView.Sorting);
      }

      [Test]
      public void ReportTableView_ShouldCreateWithGroupByColumn()
      {
         // Arrange
         var placement = new ReportComponentPlacement(new Id<ReportBody>(Guid.NewGuid()), 10, 10, 10, 10);
         var reportTableView = new ReportTableView(placement)
                                    .WithDefaultColumns();

         //Act
         var newReportTableView = reportTableView
                                 .WithGroupingByColumn("FeatureName")
                                 .WithGroupingByColumn("Nominal")
                                 .WithGroupingByColumn("Measured");

         // Assert
         Assert.AreEqual(3, newReportTableView.GroupBy.Count);
         Assert.AreEqual("FeatureName", newReportTableView.GroupBy[0]);
         Assert.AreEqual("Nominal", newReportTableView.GroupBy[1]);
         Assert.AreEqual("Measured", newReportTableView.GroupBy[2]);
      }

      [Test]
      public void ReportTableView_ShouldCreateWithoutGroupByColumn()
      {
         // Arrange
         var placement = new ReportComponentPlacement(new Id<ReportBody>(Guid.NewGuid()), 10, 10, 10, 10);
         var reportTableView = new ReportTableView(placement)
                                    .WithDefaultColumns()
                                    .WithGroupingByColumn("FeatureName")
                                    .WithGroupingByColumn("Nominal")
                                    .WithGroupingByColumn("Measured");
         // Act
         reportTableView = reportTableView.WithoutGroupingByColumn("Nominal");

         // Assert
         Assert.AreEqual(2, reportTableView.GroupBy.Count);
         Assert.AreEqual("FeatureName", reportTableView.GroupBy[0]);
         Assert.AreEqual("Measured", reportTableView.GroupBy[1]);
      }

      [Test]
      public void ReportTableView_ShouldReorderColumns()
      {
         const string COLUMN_NAME_1 = "FeatureName";
         const string COLUMN_NAME_2 = "Nominal";

         const int NEW_POSITION_1 = 0;
         const int NEW_POSITION_2 = 1;

         // Arrange
         var placement = new ReportComponentPlacement(new Id<ReportBody>(Guid.NewGuid()), 10, 10, 10, 10);
         var reportTableView = new ReportTableView(placement)
                                    .WithDefaultColumns();

         // Act
         reportTableView = reportTableView.WithReorderingColumn(COLUMN_NAME_1, NEW_POSITION_1);
         reportTableView = reportTableView.WithReorderingColumn(COLUMN_NAME_2, NEW_POSITION_2);

         // Assert
         Assert.IsTrue(reportTableView.Columns.Count(c => c.Name == COLUMN_NAME_1) == 1);
         Assert.AreEqual(COLUMN_NAME_1, reportTableView.Columns[NEW_POSITION_1].Name);

         Assert.IsTrue(reportTableView.Columns.Count(c => c.Name == COLUMN_NAME_2) == 1);
         Assert.AreEqual(COLUMN_NAME_2, reportTableView.Columns[NEW_POSITION_2].Name);
      }
   }
}
