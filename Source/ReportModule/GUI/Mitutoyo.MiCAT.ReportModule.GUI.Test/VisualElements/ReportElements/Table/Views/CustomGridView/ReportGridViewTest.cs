// <copyright file="ReportGridViewTest.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Linq;
using System.Threading;
using System.Windows;
using Mitutoyo.MiCAT.ReportModule.Domain.Components.TableView;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements.Table.Views.CustomGridView;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements.Table.Views.CustomGridView.CustomFilter;
using NUnit.Framework;
using Telerik.Windows.Controls.GridView;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Test.VisualElements.ReportElements.Table.Views.CustomGridView
{
   public class ReportGridViewTest
   {
      [Test]
      [Apartment(ApartmentState.STA)]
      public void ConfigureCellsBehaviour_WithNoGroupedColumns_ShouldSetCellBehavior()
      {
         var nameColumn = new Column("Name", 0, null, ContentAligment.Center);
         var nameColumnInfo = new ColumnInfo(
            nameColumn,
            0,
            false,
            -1,
            SortingMode.None,
            -1,
            null,
            TextAlignment.Center,
            new FilterInfo());

         var control = new ReportGridView()
         {
            AutoGenerateColumns = true,
            Width = 500,
            Height = 500,
            ColumnInfos = new[] { nameColumnInfo }.ToList(),
            ItemsSource = new[] { new ItemDTO { Name = "Item 1" }, },
         };

         Assert.AreEqual(MergedCellsDirection.None, control.MergedCellsDirection);
         Assert.AreEqual(GroupRenderMode.Nested, control.GroupRenderMode);
      }

      [Test]
      [Apartment(ApartmentState.STA)]
      public void ConfigureCellsBehaviour_WithGroupedColumns_ShouldSetCellBehavior()
      {
         var nameColumn = new Column("Name", 0, null, ContentAligment.Center);
         var nameColumnInfo = new ColumnInfo(
            nameColumn,
            0,
            true,
            0,
            SortingMode.None,
            -1,
            null,
            TextAlignment.Center,
            new FilterInfo());

         var control = new ReportGridView()
         {
            AutoGenerateColumns = true,
            Width = 500,
            Height = 500,
            ColumnInfos = new[] { nameColumnInfo }.ToList(),
            ItemsSource = new[] { new ItemDTO { Name = "Item 1" }, },
         };

         Assert.AreEqual(false, control.CanUserFreezeColumns);
         Assert.AreEqual(MergedCellsDirection.Vertical, control.MergedCellsDirection);
         Assert.AreEqual(GroupRenderMode.Flat, control.GroupRenderMode);
      }

      private class ItemDTO
      {
         public string Name { get; set; }
      }
   }
}
