// <copyright file="TablePieceCellStyleSelectorTests.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Windows;
using Mitutoyo.MiCAT.ReportModule.Domain.Data;
using Mitutoyo.MiCAT.ReportModule.GUI.Properties;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.Pieces.Table.Views;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.Pieces.Table.Views.LightDataGrid.Cells;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.Pieces.Table.Views.LightDataGrid.Columns;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.Pieces.Table.Views.LightDataGrid.Rows;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements.Table.ViewModels;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements.Table.Views.CustomGridView;
using NUnit.Framework;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Test.VisualElements.Pieces.Table.Views
{
   [TestFixture]
   public class TablePieceCellStyleSelectorTests
   {
      [Test]
      public void ShouldReturnNullFromBaseImplementationWhenColumnInfoIsNotForStatus()
      {
         var characteristic = new Characteristic();
         var characteristicActual = new CharacteristicActual(characteristic.Id, "status", null, null);
         var evaluatedCharacteristic = new EvaluatedCharacteristic(characteristic, characteristicActual);
         var itemSource = new VMEvaluatedCharacteristic(evaluatedCharacteristic);
         var columnInfo = new ColumnInfo
         {
            ColumnName = nameof(VMEvaluatedCharacteristic.FeatureName),
         };
         var row = new LightDataGridBodyRow(0, itemSource);
         var column = new LightDataGridColumn(columnInfo);
         var cell = new LightDataGridCell(row, column, null);
         var selector = new TablePieceCellStyleSelector();
         var style = selector.SelectStyle(cell);

         Assert.IsNull(style);
      }

      [Test]
      public void ShouldReturnPassStatusStyleWhenColumnInfoIsStatusAndValueIsPass()
      {
         var characteristic = new Characteristic();
         var characteristicActual = new CharacteristicActual(characteristic.Id, Resources.Pass, null, null);
         var evaluatedCharacteristic = new EvaluatedCharacteristic(characteristic, characteristicActual);
         var itemSource = new VMEvaluatedCharacteristic(evaluatedCharacteristic);
         var columnInfo = new ColumnInfo
         {
            ColumnName = nameof(VMEvaluatedCharacteristic.Status),
         };
         var row = new LightDataGridBodyRow(0, itemSource);
         var column = new LightDataGridColumn(columnInfo);
         var cell = new LightDataGridCell(row, column, null);
         var passStatusStyle = new Style();

         var selector = new TablePieceCellStyleSelector { PassStatusStyle = passStatusStyle };
         var style = selector.SelectStyle(cell);

         Assert.AreEqual(passStatusStyle, style);
      }

      [Test]
      public void ShouldReturnFailStatusStyleWhenColumnInfoIsStatusAndValueIsFail()
      {
         var characteristic = new Characteristic();
         var characteristicActual = new CharacteristicActual(characteristic.Id, Resources.Fail, null, null);
         var evaluatedCharacteristic = new EvaluatedCharacteristic(characteristic, characteristicActual);
         var itemSource = new VMEvaluatedCharacteristic(evaluatedCharacteristic);
         var columnInfo = new ColumnInfo
         {
            ColumnName = nameof(VMEvaluatedCharacteristic.Status),
         };
         var row = new LightDataGridBodyRow(0, itemSource);
         var column = new LightDataGridColumn(columnInfo);
         var cell = new LightDataGridCell(row, column, null);
         var failStatusStyle = new Style();

         var selector = new TablePieceCellStyleSelector { FailStatusStyle = failStatusStyle };
         var style = selector.SelectStyle(cell);

         Assert.AreEqual(failStatusStyle, style);
      }

      [Test]
      public void ShouldReturnInvalidStatusStyleWhenColumnInfoIsStatusAndValueIsInvalid()
      {
         var characteristic = new Characteristic();
         var characteristicActual = new CharacteristicActual(characteristic.Id, Resources.Invalid, null, null);
         var evaluatedCharacteristic = new EvaluatedCharacteristic(characteristic, characteristicActual);
         var itemSource = new VMEvaluatedCharacteristic(evaluatedCharacteristic);
         var columnInfo = new ColumnInfo
         {
            ColumnName = nameof(VMEvaluatedCharacteristic.Status),
         };
         var row = new LightDataGridBodyRow(0, itemSource);
         var column = new LightDataGridColumn(columnInfo);
         var cell = new LightDataGridCell(row, column, null);
         var invalidStatusStyle = new Style();

         var selector = new TablePieceCellStyleSelector { InvalidStatusStyle = invalidStatusStyle };
         var style = selector.SelectStyle(cell);

         Assert.AreEqual(invalidStatusStyle, style);
      }
   }
}
