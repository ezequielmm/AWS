// <copyright file="LightDataGridTests.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.Pieces.Table.Views.LightDataGrid.Rows;
using NUnit.Framework;
using Gui = Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.Pieces.Table.Views.LightDataGrid;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Test.VisualElements.Pieces.Table.Views.LightDataGrid
{
   [TestFixture]
   [ExcludeFromCodeCoverage]
   public class LightDataGridTests
   {
      [Test]
      [Apartment(ApartmentState.STA)]
      public void TryFindLightDataGridStyleResourceOnLoad()
      {
         var dataGrid = new Gui.LightDataGrid();

         dataGrid.Resources.Add(Gui.LightDataGrid.LightDataGridStyle, new Style() {});
         dataGrid.BeginInit();
         dataGrid.EndInit();

         Assert.IsNotNull(dataGrid.Style);
      }

      [Test]
      [Apartment(ApartmentState.STA)]
      public void ShouldAddHeaderRowWhenColumnInfosHasChanged()
      {
         var dataGrid = new Gui.LightDataGrid
         {
            ColumnInfos = new[]
            {
               LightDataGridFakeItemsSource.BuildIndexColumnInfo(),
               LightDataGridFakeItemsSource.BuildNameColumnInfo(),
            },
         };

         dataGrid.BeginInit();
         dataGrid.EndInit();
         dataGrid.Dispatcher?.Invoke(new Action(() => { }), DispatcherPriority.ApplicationIdle);

         Assert.AreEqual(1, dataGrid.Rows.Count());
         Assert.IsTrue(dataGrid.Rows.First() is LightDataGridHeaderRow);
      }

      [Test]
      [Apartment(ApartmentState.STA)]
      public void ShoulAddEmptyRowsWhenColumnInfosIsNullAndItemsSourceHasChanged()
      {
         var dataGrid = new Gui.LightDataGrid
         {
            ItemsSource = LightDataGridFakeItemsSource.ItemsSource,
         };

         dataGrid.BeginInit();
         dataGrid.EndInit();
         dataGrid.Dispatcher?.Invoke(new Action(() => { }), DispatcherPriority.ApplicationIdle);

         CollectionAssert.IsNotEmpty(dataGrid.Rows);
         Assert.AreEqual(LightDataGridFakeItemsSource.ItemsSource.Count() + 1, dataGrid.Rows.Count());
         Assert.IsTrue(dataGrid.Rows.All(x => !x.Cells.Any()));
      }

      [Test]
      [Apartment(ApartmentState.STA)]
      public void ShouldAddHeaderAndBodyRowsWhenColumnInfosAndItemsSourceHasChanged()
      {
         var dataGrid = new Gui.LightDataGrid
         {
            ColumnInfos = new[]
            {
               LightDataGridFakeItemsSource.BuildIndexColumnInfo(),
               LightDataGridFakeItemsSource.BuildNameColumnInfo(),
            },
            ItemsSource = LightDataGridFakeItemsSource.ItemsSource,
         };

         dataGrid.BeginInit();
         dataGrid.EndInit();
         dataGrid.Dispatcher?.Invoke(new Action(() => { }), DispatcherPriority.ApplicationIdle);

         CollectionAssert.IsNotEmpty(dataGrid.Rows);
         Assert.IsTrue(dataGrid.Rows.First() is LightDataGridHeaderRow);
         Assert.AreEqual(
            LightDataGridFakeItemsSource.ItemsSource.Count(),
            dataGrid.Rows.OfType<LightDataGridBodyRow>().Count());
      }

      [Test]
      [Apartment(ApartmentState.STA)]
      public void ShouldAddAllCellContentToDataGridChildrenCollectionInCertainGridPosition()
      {
         var dataGrid = new Gui.LightDataGrid
         {
            ColumnInfos = new[]
            {
               LightDataGridFakeItemsSource.BuildIndexColumnInfo(),
               LightDataGridFakeItemsSource.BuildNameColumnInfo(),
            },
            ItemsSource = LightDataGridFakeItemsSource.ItemsSource,
         };

         dataGrid.BeginInit();
         dataGrid.EndInit();

         foreach (var row in dataGrid.Rows)
         {
            foreach (var cell in row.Cells)
            {
               var cellContent = cell.Content;

               Assert.IsTrue(dataGrid.Children.Contains(cellContent));
               Assert.AreEqual(cell.Column.ColumnIndex, Gui.LightDataGrid.GetColumn(cellContent));
               Assert.AreEqual(cell.Row.RowIndex, Gui.LightDataGrid.GetRow(cellContent));
            }
         }
      }
   }
}
