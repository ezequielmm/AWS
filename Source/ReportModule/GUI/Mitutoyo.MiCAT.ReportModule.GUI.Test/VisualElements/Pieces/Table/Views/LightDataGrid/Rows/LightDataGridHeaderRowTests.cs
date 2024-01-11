// <copyright file="LightDataGridHeaderRowTests.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.Pieces.Table.Views.LightDataGrid.Rows;
using NUnit.Framework;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Test.VisualElements.Pieces.Table.Views.LightDataGrid.Rows
{
   [TestFixture]
   public class LightDataGridHeaderRowTests
   {
      [Test]
      public void ConstructorShouldSetProperties()
      {
         var rowIndex = 1;
         var row = new LightDataGridHeaderRow(rowIndex);

         Assert.IsNotNull(row);
         Assert.AreEqual(rowIndex, row.RowIndex);
         Assert.IsNotNull(row.RowDefinition);
         Assert.AreEqual(LightDataGridHeaderRow.HeaderRowDefinition.RowHeight, row.RowDefinition.Height.Value);
         Assert.IsNull(row.ItemSource);
         Assert.IsNotNull(row.Cells);
         CollectionAssert.IsEmpty(row.Cells);
      }
   }
}
