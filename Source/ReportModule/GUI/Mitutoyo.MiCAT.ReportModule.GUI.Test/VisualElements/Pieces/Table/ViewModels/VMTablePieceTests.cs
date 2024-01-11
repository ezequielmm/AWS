// <copyright file="VMTablePieceTests.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Mitutoyo.MiCAT.ReportModule.Domain.Data;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.Pieces.Table.ViewModels;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements.Table.ViewModels;
using Moq;
using NUnit.Framework;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Test.VisualElements.Pieces.Table.ViewModels
{
   [TestFixture]
   [ExcludeFromCodeCoverage]
   public class VMTablePieceTests
   {
      [Test]
      public void VMTablePiece_ShouldSetOwner()
      {
         var vmComponet = Mock.Of<IVMReportComponent>();
         var vmPlacement = new VMVisualPlacement();
         VMTablePiece vmTableViewPiece = new VMTablePiece(vmPlacement, vmComponet);

         Assert.IsNotNull(vmTableViewPiece.Owner);
      }

      [Test]
      public void VMTablePiece_NullPartialSourceShouldSetHeaderHeight()
      {
         // Arrange
         var vmComponet = Mock.Of<IVMReportComponent>();
         var vmPlacement = new VMVisualPlacement();
         VMTablePiece vmTableViewPiece = new VMTablePiece(vmPlacement, vmComponet);

         // ActvmTableViewPiece
         vmTableViewPiece.DataToDisplay = null;

         // Assert
         Assert.AreEqual(35, vmTableViewPiece.VMPlacement.VisualHeight);
      }

      [Test]
      [TestCase(0)]
      [TestCase(5)]
      [TestCase(10)]
      public void SettingDataToDisplay_ShouldUpdateVisualHeight(int rowCount)
      {
         //Arrange
         var component = Mock.Of<IVMReportComponent>();
         var characteristic = new Characteristic();
         var characteristicActual = new CharacteristicActual(Guid.NewGuid(), string.Empty, 0, 0);
         var evaluatedCharacteristic =
            new EvaluatedCharacteristic(characteristic, characteristicActual);
         var vmEvaluatedCharacteristic = new VMEvaluatedCharacteristic(evaluatedCharacteristic);
         var vmEvaluatedCharacteristics = Enumerable.Repeat(vmEvaluatedCharacteristic, rowCount);
         var vmPlacement = new VMVisualPlacement();
         var piece = new VMTablePiece(vmPlacement, component);

         //Act
         piece.DataToDisplay = vmEvaluatedCharacteristics;

         //Assert
         Assert.IsNotNull(piece.DataToDisplay);
         Assert.AreEqual(35 + (rowCount * 29), piece.VMPlacement.VisualHeight);
      }

      [Test]
      [TestCase(1, 1)]
      [TestCase(2, 10)]
      [TestCase(20, 20)]
      public void Label_ShouldBeLocalized(int index, int count)
      {
         var component = Mock.Of<IVMReportComponent>();
         var vmPlacement = new VMVisualPlacement();
         var piece = new VMTablePiece(vmPlacement, component);

         piece.PieceIndex = index;
         piece.PiecesCount = count;

         Assert.IsNotNull(piece.Label);
         Assert.AreEqual(
            string.Format(
               Properties.Resources.CharacteristicTable_PlaceHolder_FirstLine,
               index,
               count),
            piece.Label);
      }
   }
}
