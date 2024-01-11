// <copyright file="LightDataGridHeaderCellFactoryTests.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Linq;
using System.Threading;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.Pieces.Table.Views.LightDataGrid.Cells;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.Pieces.Table.Views.LightDataGrid.Cells.Factories;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.Pieces.Table.Views.LightDataGrid.Columns;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.Pieces.Table.Views.LightDataGrid.Rows;
using NUnit.Framework;
using Gui = Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.Pieces.Table.Views.LightDataGrid;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Test.VisualElements.Pieces.Table.Views.LightDataGrid.Cells.Generators
{
   [TestFixture]
   public class LightDataGridHeaderCellFactoryTests
   {
      [Test]
      [Apartment(ApartmentState.STA)]
      public void GenerateShouldGenerateAGridCell()
      {
         var dataGrid = new Gui.LightDataGrid();
         var itemSource = LightDataGridFakeItemsSource.ItemsSource.First();
         var row = new LightDataGridBodyRow(0, itemSource);
         var columnInfo = LightDataGridFakeItemsSource.BuildNameColumnInfo();
         var column = new LightDataGridColumn(columnInfo);

         var factory = new LightDataGridHeaderCellFactory();
         var cell = factory.Create(dataGrid, row, column);

         Assert.IsNotNull(cell);
         Assert.AreEqual(columnInfo, cell.Column.ColumnInfo);
         Assert.AreEqual(row, cell.Row);
         Assert.IsNotNull(cell.Content);
         Assert.IsTrue(cell.GetType() == typeof(LightDataGridCell));
      }
   }
}
