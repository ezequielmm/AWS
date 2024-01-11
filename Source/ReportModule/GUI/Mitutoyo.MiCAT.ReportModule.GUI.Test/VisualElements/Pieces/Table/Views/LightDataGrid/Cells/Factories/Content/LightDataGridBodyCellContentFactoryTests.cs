// <copyright file="LightDataGridBodyCellContentFactoryTests.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Linq;
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
   public class LightDataGridBodyCellContentFactoryTests
   {
      [Test]
      [Apartment(ApartmentState.STA)]
      public void CreateShouldReturnUIElement()
      {
         var outerBorderStyle = new Style();
         var textBlockStyle = new Style();
         var dataGrid = new Gui.LightDataGrid()
         {
            Resources = new ResourceDictionary()
            {
               { LightDataGridBodyCellContentFactory.OuterBorderCellResourceKey, outerBorderStyle },
               { LightDataGridBodyCellContentFactory.TextBlockResourceKey, textBlockStyle },
            },
         };
         var itemSource = LightDataGridFakeItemsSource.ItemsSource.First();
         var row = new Mock<LightDataGridRow>(
               MockBehavior.Default,
               0,
               new RowDefinition(),
               itemSource)
            .Object;
         var columnInfo = Mock.Of<ColumnInfo>(x => x.ColumnName == nameof(itemSource.Name));
         var column = new LightDataGridColumn(columnInfo);

         var factory = new LightDataGridBodyCellContentFactory();
         var content = factory.Create(dataGrid, row, column);

         Assert.IsNotNull(content);

         var outerBorder = content as Border;
         Assert.IsNotNull(outerBorder);
         Assert.IsNotNull(outerBorder.Style);

         var textBlock = outerBorder.Child as TextBlock;
         Assert.IsNotNull(textBlock);
         Assert.IsNotNull(textBlock.Style);
         Assert.AreEqual(itemSource.Name, textBlock.Text);
      }
   }
}
