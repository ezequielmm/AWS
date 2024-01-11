// <copyright file="LightDataGridMergedCellTests.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Windows.Controls;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.Pieces.Table.Views.LightDataGrid.Cells;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.Pieces.Table.Views.LightDataGrid.Columns;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.Pieces.Table.Views.LightDataGrid.Rows;
using NUnit.Framework;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Test.VisualElements.Pieces.Table.Views.LightDataGrid.Cells
{
   [TestFixture]
   [ExcludeFromCodeCoverage]
   public class LightDataGridMergedCellTests
   {
      [Test]
      [Apartment(ApartmentState.STA)]
      public void ConstructorShouldSetProperties()
      {
         var itemSource = LightDataGridFakeItemsSource.ItemsSource.First();
         var row = new LightDataGridBodyRow(0, itemSource);
         var columnInfo = LightDataGridFakeItemsSource.BuildNameColumnInfo();
         var column = new LightDataGridColumn(columnInfo);
         var content = new TextBlock();
         var cell = new LightDataGridCell(row, column, content);

         var mergedCell = new LightDataGridMergedCell(cell);

         Assert.AreEqual(row, mergedCell.Row);
         Assert.AreEqual(columnInfo.ColumnIndex, mergedCell.Column.ColumnIndex);
         Assert.AreEqual(columnInfo, mergedCell.Column.ColumnInfo);
         Assert.AreEqual(content, mergedCell.Content);
      }

      [Test]
      [Apartment(ApartmentState.STA)]
      public void GetValueForShouldReturnAValueForSpecificColumnInfoAndNotNullItemSourceOnReferencedCell()
      {
         var itemSource = LightDataGridFakeItemsSource.ItemsSource.First();
         var row = new LightDataGridBodyRow(0, itemSource);
         var columnInfo = LightDataGridFakeItemsSource.BuildNameColumnInfo();
         var column = new LightDataGridColumn(columnInfo);
         var content = new TextBlock();
         var cell = new LightDataGridCell(row, column, content);

         var mergedCell = new LightDataGridMergedCell(cell);

         Assert.AreEqual(itemSource.Name, mergedCell.GetValue());
      }

      [Test]
      [Apartment(ApartmentState.STA)]
      public void GetValueForShouldReturnNullForSpecificColumnInfoAndNullItemSourceOnReferencedCell()
      {
         var row = new LightDataGridBodyRow(0);
         var columnInfo = LightDataGridFakeItemsSource.BuildNameColumnInfo();
         var column = new LightDataGridColumn(columnInfo);
         var content = new TextBlock();
         var cell = new LightDataGridCell(row, column, content);

         var mergedCell = new LightDataGridMergedCell(cell);

         Assert.IsNull(mergedCell.GetValue());
      }
   }
}
