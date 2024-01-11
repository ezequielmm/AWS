// <copyright file="LightDataGridBodyCellFactoryTests.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Linq;
using System.Threading;
using System.Windows.Controls;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.Pieces.Table.Views.LightDataGrid.Cells;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.Pieces.Table.Views.LightDataGrid.Cells.Factories;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.Pieces.Table.Views.LightDataGrid.Columns;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.Pieces.Table.Views.LightDataGrid.Rows;
using Moq;
using NUnit.Framework;
using Gui = Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.Pieces.Table.Views.LightDataGrid;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Test.VisualElements.Pieces.Table.Views.LightDataGrid.Cells.Generators
{
   [TestFixture]
   public class LightDataGridBodyCellFactoryTests
   {
      [Test]
      [Apartment(ApartmentState.STA)]
      public void CreateShouldReturnsAGridCell()
      {
         var dataGrid = new Gui.LightDataGrid();
         var itemSource = LightDataGridFakeItemsSource.ItemsSource.First();
         var row = new LightDataGridBodyRow(0, itemSource);
         var columnInfo = LightDataGridFakeItemsSource.BuildNameColumnInfo(0);
         var column = new LightDataGridColumn(columnInfo);

         var factory = new LightDataGridBodyCellFactory();
         var cell = factory.Create(dataGrid, row, column);

         Assert.IsNotNull(cell);
         Assert.AreEqual(columnInfo, cell.Column.ColumnInfo);
         Assert.AreEqual(row, cell.Row);
         Assert.IsNotNull(cell.Content);
         Assert.IsTrue(cell.GetType() == typeof(LightDataGridCell));
      }

      [Test]
      [Apartment(ApartmentState.STA)]
      public void CreateShouldReturnsAMergedGridCellIfIsGroupedByAndTheCellFromPrevoiusRowHasTheSameValue()
      {
         var itemSource = LightDataGridFakeItemsSource.ItemsSource.First();
         var nameColumnInfo = LightDataGridFakeItemsSource.BuildNameColumnInfo(0, true, 1);
         var nameColumn = new LightDataGridColumn(nameColumnInfo);
         var firstRow = new LightDataGridBodyRow(0, itemSource);
         var nameCell = new LightDataGridCell(firstRow, nameColumn, new TextBlock());
         firstRow.AddCell(nameCell);
         var dataGrid = Mock.Of<Gui.LightDataGrid>();

         Mock
            .Get(dataGrid)
            .Setup(x => x.Rows)
            .Returns(new[] { firstRow });

         var secondRow = new LightDataGridBodyRow(1, itemSource);

         var factory = new LightDataGridBodyCellFactory();
         var cell = factory.Create(dataGrid, secondRow, nameColumn);

         Assert.IsNotNull(cell);
         Assert.AreEqual(nameColumn.ColumnInfo, cell.Column.ColumnInfo);
         Assert.AreEqual(firstRow, cell.Row);
         Assert.IsNotNull(cell.Content);
         Assert.IsTrue(cell.GetType() == typeof(LightDataGridMergedCell));
      }
   }
}
