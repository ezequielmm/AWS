// <copyright file="TableViewControllerTest.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.ApplicationState.Controllers;
using Mitutoyo.MiCAT.ReportModule.ControllerIntegration.Test.ControllerTests.TestHelper;
using Mitutoyo.MiCAT.ReportModule.Domain.Components;
using Mitutoyo.MiCAT.ReportModule.Domain.Components.TableView;
using Mitutoyo.MiCAT.ReportModule.Domain.Data;
using Mitutoyo.MiCAT.ReportModuleApp.Controllers;
using Mitutoyo.MiCAT.ReportModuleApp.Services.ReportComponents;
using NUnit.Framework;

namespace Mitutoyo.MiCAT.ReportModule.ControllerIntegration.Test.ControllerTests
{
   [ExcludeFromCodeCoverage]
   public class TableViewControllerTest : BaseAppStateTest
   {
      private TableViewController _controller;
      public static ISnapShot BuildHelper(ISnapShot snapShot)
      {
         snapShot = AppStateReportComponentTestHelper.InitializeSnapShot(snapShot);
         snapShot = snapShot.AddCollection<ReportTableView>(AppStateKinds.Undoable);

         return snapShot;
      }

      [SetUp]
      public void Setup()
      {
         SetUpHelper(BuildHelper);

         var reportComponentService = new ReportComponentService(new DomainSpaceService());
         _controller = new TableViewController(_history, reportComponentService);
      }

      [Test]
      public void TableViewController_AddTableView()
      {
         // Arrange

         // Act
         _controller.AddTableView(10, 25);

         // Assert
         var tableViewComponentToTest = _history.CurrentSnapShot.GetItems<ReportTableView>().Single();
         var placementToTest = tableViewComponentToTest.Placement;
         var selectedComponentToTest = _history.CurrentSnapShot.GetItems<ReportComponentSelection>().Single();

         Assert.AreEqual(tableViewComponentToTest.Id, selectedComponentToTest.SelectedReportComponentIds.Single());
         Assert.AreEqual(placementToTest.ReportSectionId, _history.CurrentSnapShot.GetReportBodyId());
         Assert.AreEqual(10, placementToTest.X);
         Assert.AreEqual(25, placementToTest.Y);
         Assert.AreEqual(707, placementToTest.Width);
         Assert.AreEqual(54, placementToTest.Height);
      }

      [Test]
      public void TableViewController_SortByASingleColumn()
      {
         //Arrange
         const string COLUMN_NAME = "UpperTolerance";

         var placement = new ReportComponentPlacement(_history.CurrentSnapShot.GetReportBodyId(), 1, 2, 3, 4);
         ReportTableView tableView = new ReportTableView(placement).WithDefaultColumns();

         var snapshot = _history.NextSnapShot(ControllerCall.Empty);
         snapshot = snapshot.AddItem(tableView);
         _history.AddSnapShot(snapshot);

         //Act
         _controller.SortByColumn(tableView.Id, new SortingColumn(COLUMN_NAME, SortingMode.Descending));

         //Assert
         var newTableView = _history.CurrentSnapShot.GetItem(tableView.Id);

         Assert.AreEqual(newTableView.Sorting.Count, 1);
         Assert.AreEqual(newTableView.Sorting[0].ColumnName, COLUMN_NAME);
         Assert.AreEqual(newTableView.Sorting[0].Mode, SortingMode.Descending);
      }

      [Test]
      public void TableViewController_ResetSortByASingleColumn()
      {
         //Arrange
         const string COLUMN_NAME = "UpperTolerance";

         var placement = new ReportComponentPlacement(_history.CurrentSnapShot.GetReportBodyId(), 1, 2, 3, 4);
         ReportTableView tableView = new ReportTableView(placement).WithDefaultColumns();

         var snapshot = _history.NextSnapShot(ControllerCall.Empty);
         snapshot = snapshot.AddItem(tableView);
         _history.AddSnapShot(snapshot);

         _controller.SortByAddColumn(tableView.Id, new SortingColumn("Nominal", SortingMode.Ascending));
         _controller.SortByAddColumn(tableView.Id, new SortingColumn("Measured", SortingMode.Descending));

         //Act
         _controller.SortByColumn(tableView.Id, new SortingColumn(COLUMN_NAME, SortingMode.Descending));

         //Assert
         var newTableView = _history.CurrentSnapShot.GetItem(tableView.Id);

         Assert.AreEqual(newTableView.Sorting.Count, 1);
         Assert.AreEqual(newTableView.Sorting[0].ColumnName, COLUMN_NAME);
         Assert.AreEqual(newTableView.Sorting[0].Mode, SortingMode.Descending);
      }

      [Test]
      public void TableViewController_SortByAAddedColumns()
      {
         //Arrange
         const string COLUMN_NAME_FIRST = "UpperTolerance";
         const string COLUMN_NAME_SECOND = "Nominal";

         var placement = new ReportComponentPlacement(_history.CurrentSnapShot.GetReportBodyId(), 1, 2, 3, 4);
         ReportTableView tableView = new ReportTableView(placement).WithDefaultColumns();

         var snapshot = _history.NextSnapShot(ControllerCall.Empty);
         snapshot = snapshot.AddItem(tableView);
         _history.AddSnapShot(snapshot);

         //Act
         _controller.SortByAddColumn(tableView.Id, new SortingColumn(COLUMN_NAME_FIRST, SortingMode.Ascending));
         _controller.SortByAddColumn(tableView.Id, new SortingColumn(COLUMN_NAME_SECOND, SortingMode.Descending));

         //Assert
         var newTableView = _history.CurrentSnapShot.GetItem(tableView.Id);

         Assert.AreEqual(newTableView.Sorting.Count, 2);

         Assert.AreEqual(newTableView.Sorting[0].ColumnName, COLUMN_NAME_FIRST);
         Assert.AreEqual(newTableView.Sorting[0].Mode, SortingMode.Ascending);

         Assert.AreEqual(newTableView.Sorting[1].ColumnName, COLUMN_NAME_SECOND);
         Assert.AreEqual(newTableView.Sorting[1].Mode, SortingMode.Descending);
      }

      [Test]
      public void TableViewController_HideColumn()
      {
         // Arrange
         const string COLUMN_NAME = "UpperTolerance";

         var placement = new ReportComponentPlacement(_history.CurrentSnapShot.GetReportBodyId(), 1, 2, 3, 4);
         ReportTableView tableView = new ReportTableView(placement).WithDefaultColumns();

         var allColumnsCount = tableView.Columns.Count();

         var snapshot = _history.NextSnapShot(ControllerCall.Empty);
         snapshot = snapshot.AddItem(tableView);

         _history.AddSnapShot(snapshot);

         // Act
         _controller.HideColumn(tableView.Id, COLUMN_NAME);

         // Assert
         var newTableView = _history.CurrentSnapShot.GetItem(tableView.Id);

         var hiddenColumn = newTableView.Columns.SingleOrDefault(c => c.Name == COLUMN_NAME);
         var visibleColumnsCount = newTableView.Columns.Where(c => c.IsVisible).Count();

         Assert.NotNull(hiddenColumn);
         Assert.False(hiddenColumn.IsVisible);
         Assert.AreEqual(visibleColumnsCount, allColumnsCount - 1);
      }

      [Test]
      public void TableViewController_ShowColumn()
      {
         const string COLUMN_NAME = "UpperTolerance";

         // Arrange
         var placement = new ReportComponentPlacement(_history.CurrentSnapShot.GetReportBodyId(), 1, 2, 3, 4);
         ReportTableView tableView = new ReportTableView(placement).WithDefaultColumns();

         var allColumnsCount = tableView.Columns.Count();

         tableView = tableView.WithHiddenColumn(COLUMN_NAME);

         var snapshot = _history.NextSnapShot(ControllerCall.Empty);
         snapshot = snapshot.AddItem(tableView);
         _history.AddSnapShot(snapshot);

         // Act
         _controller.ShowColumn(tableView.Id, COLUMN_NAME);

         // Assert
         var newTableView = _history.CurrentSnapShot.GetItem(tableView.Id);
         var visibleColumn = newTableView.Columns.SingleOrDefault(c => c.Name == COLUMN_NAME);

         Assert.NotNull(visibleColumn);
         Assert.True(visibleColumn.IsVisible);
         Assert.AreEqual(newTableView.Columns.Count(), allColumnsCount);
      }

      [Test]
      public void TableViewController_SortByColumn()
      {
         const string COLUMN_NAME_1 = "FeatureName";
         const string COLUMN_NAME_2 = "Nominal";
         const string COLUMN_NAME_3 = "Measured";
         const string COLUMN_NAME_4 = "Deviation";

         // Arrange
         var placement = new ReportComponentPlacement(_history.CurrentSnapShot.GetReportBodyId(), 1, 2, 3, 4);
         ReportTableView tableView = new ReportTableView(placement).WithDefaultColumns();

         var snapshot = _history.NextSnapShot(ControllerCall.Empty);
         snapshot = snapshot.AddItem(tableView);
         _history.AddSnapShot(snapshot);

         // Act
         _controller.SortByAddColumn(tableView.Id, new SortingColumn(COLUMN_NAME_1, SortingMode.Ascending));
         _controller.SortByAddColumn(tableView.Id, new SortingColumn(COLUMN_NAME_2, SortingMode.Descending));
         _controller.SortByAddColumn(tableView.Id, new SortingColumn(COLUMN_NAME_3, SortingMode.Descending));
         _controller.SortByAddColumn(tableView.Id, new SortingColumn(COLUMN_NAME_4, SortingMode.Ascending));
         _controller.SortByAddColumn(tableView.Id, new SortingColumn(COLUMN_NAME_4, SortingMode.None));

         // Assert
         var newTableView = _history.CurrentSnapShot.GetItem(tableView.Id);

         Assert.AreEqual(3, newTableView.Sorting.Count);
         Assert.AreEqual(COLUMN_NAME_1, newTableView.Sorting[0].ColumnName);
         Assert.AreEqual(SortingMode.Ascending, newTableView.Sorting[0].Mode);
         Assert.AreEqual(COLUMN_NAME_2, newTableView.Sorting[1].ColumnName);
         Assert.AreEqual(SortingMode.Descending, newTableView.Sorting[1].Mode);
         Assert.AreEqual(COLUMN_NAME_3, newTableView.Sorting[2].ColumnName);
         Assert.AreEqual(SortingMode.Descending, newTableView.Sorting[2].Mode);
      }

      [Test]
      public void TableViewController_GroupByColumn()
      {
         const string COLUMN_NAME_1 = "FeatureName";
         const string COLUMN_NAME_2 = "Nominal";
         const string COLUMN_NAME_3 = "Measured";

         // Arrange
         var placement = new ReportComponentPlacement(_history.CurrentSnapShot.GetReportBodyId(), 1, 2, 3, 4);
         ReportTableView tableView = new ReportTableView(placement).WithDefaultColumns();

         var snapshot = _history.NextSnapShot(ControllerCall.Empty);
         snapshot = snapshot.AddItem(tableView);

         _history.AddSnapShot(snapshot);

         // Act
         _controller.GroupByColumn(tableView.Id, COLUMN_NAME_1);
         _controller.GroupByColumn(tableView.Id, COLUMN_NAME_2);
         _controller.GroupByColumn(tableView.Id, COLUMN_NAME_3);

         // Assert
         var newTableView = _history.CurrentSnapShot.GetItem(tableView.Id);

         Assert.AreEqual(3, newTableView.GroupBy.Count);
         Assert.AreEqual(COLUMN_NAME_1, newTableView.GroupBy[0]);
         Assert.AreEqual(COLUMN_NAME_2, newTableView.GroupBy[1]);
         Assert.AreEqual(COLUMN_NAME_3, newTableView.GroupBy[2]);
      }

      [Test]
      public void TableViewController_RemoveGroupByColumn()
      {
         const string COLUMN_NAME_1 = "FeatureName";
         const string COLUMN_NAME_2 = "Nominal";
         const string COLUMN_NAME_3 = "Measured";

         // Arrange
         var placement = new ReportComponentPlacement(_history.CurrentSnapShot.GetReportBodyId(), 1, 2, 3, 4);
         ReportTableView tableView = new ReportTableView(placement).WithDefaultColumns();

         var snapshot = _history.NextSnapShot(ControllerCall.Empty);
         snapshot = snapshot.AddItem(tableView);
         _history.AddSnapShot(snapshot);

         // Act
         _controller.GroupByColumn(tableView.Id, COLUMN_NAME_1);
         _controller.GroupByColumn(tableView.Id, COLUMN_NAME_2);
         _controller.GroupByColumn(tableView.Id, COLUMN_NAME_3);

         _controller.RemoveGroupByColumn(tableView.Id, COLUMN_NAME_2);

         // Assert
         var newTableView = _history.CurrentSnapShot.GetItem(tableView.Id);

         Assert.AreEqual(2, newTableView.GroupBy.Count);
         Assert.AreEqual(COLUMN_NAME_1, newTableView.GroupBy[0]);
         Assert.AreEqual(COLUMN_NAME_3, newTableView.GroupBy[1]);
      }

      [Test]
      public void TableViewController_ReorderColumn()
      {
         const string COLUMN_NAME_1 = "FeatureName";
         const string COLUMN_NAME_2 = "Nominal";
         const string COLUMN_NAME_3 = "Measured";

         const int NEW_POSITION_1 = 0;
         const int NEW_POSITION_2 = 1;
         const int NEW_POSITION_3 = 2;

         // Arrange
         var placement = new ReportComponentPlacement(_history.CurrentSnapShot.GetReportBodyId(), 1, 2, 3, 4);
         ReportTableView tableView = new ReportTableView(placement).WithDefaultColumns();

         var snapshot = _history.NextSnapShot(ControllerCall.Empty);
         snapshot = snapshot.AddItem(tableView);
         _history.AddSnapShot(snapshot);

         // Act
         _controller.ReorderColumn(tableView.Id, COLUMN_NAME_1, NEW_POSITION_1);
         _controller.ReorderColumn(tableView.Id, COLUMN_NAME_2, NEW_POSITION_2);
         _controller.ReorderColumn(tableView.Id, COLUMN_NAME_3, NEW_POSITION_3);

         // Assert
         var newTableView = _history.CurrentSnapShot.GetItem(tableView.Id);

         Assert.IsTrue(newTableView.Columns.Count(c => c.Name == COLUMN_NAME_1) == 1);
         Assert.AreEqual(COLUMN_NAME_1, newTableView.Columns[NEW_POSITION_1].Name);

         Assert.IsTrue(newTableView.Columns.Count(c => c.Name == COLUMN_NAME_2) == 1);
         Assert.AreEqual(COLUMN_NAME_2, newTableView.Columns[NEW_POSITION_2].Name);

         Assert.IsTrue(newTableView.Columns.Count(c => c.Name == COLUMN_NAME_3) == 1);
         Assert.AreEqual(COLUMN_NAME_3, newTableView.Columns[NEW_POSITION_3].Name);
      }
   }
}
