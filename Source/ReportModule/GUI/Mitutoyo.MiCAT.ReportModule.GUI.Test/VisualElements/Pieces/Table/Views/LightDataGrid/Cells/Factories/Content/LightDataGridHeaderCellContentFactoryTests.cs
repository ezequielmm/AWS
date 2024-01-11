// <copyright file="LightDataGridHeaderCellContentFactoryTests.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Threading;
using System.Windows;
using System.Windows.Controls;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.Pieces.Table.Views.LightDataGrid.Cells.Factories.Content;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.Pieces.Table.Views.LightDataGrid.Columns;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.Pieces.Table.Views.LightDataGrid.Rows;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements.Table.Views.CustomGridView;
using Moq;
using NUnit.Framework;
using Gui = Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.Pieces.Table.Views.LightDataGrid;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Test.VisualElements.Pieces.Table.Views.LightDataGrid.Cells.Factories.Content
{
   [TestFixture]
   public class LightDataGridHeaderCellContentFactoryTests
   {
      [Test]
      [Apartment(ApartmentState.STA)]
      public void CreateShouldReturnUIElement()
      {
         var innerBorderStyle = new Style();
         var outerBorderStyle = new Style();
         var textBlockStyle = new Style();
         var dataGrid = new Gui.LightDataGrid()
         {
            Resources = new ResourceDictionary()
            {
               { LightDataGridHeaderCellContentFactory.OuterBorderCellResourceKey, outerBorderStyle },
               { LightDataGridHeaderCellContentFactory.TextBlockResourceKey, textBlockStyle },
            },
         };
         var row = new Mock<LightDataGridRow>(MockBehavior.Default, 0, new RowDefinition(), null).Object;
         var columnName = nameof(Properties.Resources.FeatureName);
         var localizedColumnName = Properties.Resources.FeatureName;
         var columnInfo = Mock.Of<ColumnInfo>(x => x.ColumnName == columnName);
         var column = new LightDataGridColumn(columnInfo);

         var factory = new LightDataGridHeaderCellContentFactory();
         var content = factory.Create(dataGrid, row, column);

         Assert.IsNotNull(content);

         var outerBorder = content as Border;
         Assert.IsNotNull(outerBorder);
         Assert.IsNotNull(outerBorder.Style);

         var textBlock = outerBorder.Child as TextBlock;
         Assert.IsNotNull(textBlock);
         Assert.IsNotNull(textBlock.Style);
         Assert.AreEqual(localizedColumnName, textBlock.Text);
      }
   }
}
