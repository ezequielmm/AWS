// <copyright file="LightDataGridHeaderRowFactoryTests.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Linq;
using System.Threading;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.Pieces.Table.Views.LightDataGrid.Cells;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.Pieces.Table.Views.LightDataGrid.Columns;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.Pieces.Table.Views.LightDataGrid.Rows;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.Pieces.Table.Views.LightDataGrid.Rows.Factories;
using Moq;
using NUnit.Framework;
using Gui = Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.Pieces.Table.Views.LightDataGrid;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Test.VisualElements.Pieces.Table.Views.LightDataGrid.Rows.Factories
{
   [TestFixture]
   public class LightDataGridHeaderRowFactoryTests
   {
      [Test]
      [Apartment(ApartmentState.STA)]
      public void GenerateShouldReturnsFullFilledHeaderRow()
      {
         var indexColumnInfo = LightDataGridFakeItemsSource.BuildIndexColumnInfo(columnIndex: 0);
         var nameColumnInfo = LightDataGridFakeItemsSource.BuildNameColumnInfo(columnIndex: 1);

         var dataGrid = Mock.Of<Gui.LightDataGrid>();
         Mock
            .Get(dataGrid)
            .Setup(x => x.Columns)
            .Returns(new[]
            {
               new LightDataGridColumn(indexColumnInfo),
               new LightDataGridColumn(nameColumnInfo),
            });

         var rowIndex = 0;

         var factory = new LightDataGridHeaderRowFactory();
         var row = factory.Create(dataGrid, rowIndex);

         Assert.IsNotNull(row);
         Assert.IsInstanceOf<LightDataGridHeaderRow>(row);
         Assert.AreEqual(rowIndex, row.RowIndex);
         Assert.IsNull(row.ItemSource);
         Assert.IsNotNull(row.Cells);
         Assert.AreEqual(2, row.Cells.Count());
         CollectionAssert.AllItemsAreInstancesOfType(row.Cells, typeof(LightDataGridCell));
         Assert.AreEqual(indexColumnInfo, row.Cells.ElementAt(indexColumnInfo.ColumnIndex).Column.ColumnInfo);
         Assert.AreEqual(nameColumnInfo, row.Cells.ElementAt(nameColumnInfo.ColumnIndex).Column.ColumnInfo);
      }

      [Test]
      [Apartment(ApartmentState.STA)]
      public void GenerateShouldDiscardItemSource()
      {
         var indexColumnInfo = LightDataGridFakeItemsSource.BuildIndexColumnInfo();
         var nameColumnInfo = LightDataGridFakeItemsSource.BuildNameColumnInfo();

         var dataGrid = Mock.Of<Gui.LightDataGrid>();
         Mock
            .Get(dataGrid)
            .Setup(x => x.Columns)
            .Returns(new[]
            {
               new LightDataGridColumn(indexColumnInfo),
               new LightDataGridColumn(nameColumnInfo),
            });

         var rowIndex = 0;
         var itemSource = new object();

         var generator = new LightDataGridHeaderRowFactory();
         var row = generator.Create(dataGrid, rowIndex, itemSource);

         Assert.IsNotNull(row);
         Assert.IsNull(row.ItemSource);
      }
   }
}
