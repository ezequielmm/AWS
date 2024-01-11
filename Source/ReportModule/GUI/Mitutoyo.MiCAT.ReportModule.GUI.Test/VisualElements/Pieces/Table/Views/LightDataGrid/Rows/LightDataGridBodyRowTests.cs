// <copyright file="LightDataGridBodyRowTests.cs" company="Mitutoyo Europe GmbH">
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
   public class LightDataGridBodyRowTests
   {
      [Test]
      public void ConstructorShouldSetProperties()
      {
         var rowIndex = 1;
         var itemSource = new object();
         var row = new LightDataGridBodyRow(rowIndex, itemSource);

         Assert.IsNotNull(row);
         Assert.AreEqual(rowIndex, row.RowIndex);
         Assert.IsNotNull(row.RowDefinition);
         Assert.AreEqual(LightDataGridBodyRow.BodyRowDefinition.RowHeight, row.RowDefinition.Height.Value);
         Assert.IsNotNull(row.ItemSource);
         Assert.AreEqual(itemSource, row.ItemSource);
         Assert.IsNotNull(row.Cells);
         CollectionAssert.IsEmpty(row.Cells);
      }
   }
}
