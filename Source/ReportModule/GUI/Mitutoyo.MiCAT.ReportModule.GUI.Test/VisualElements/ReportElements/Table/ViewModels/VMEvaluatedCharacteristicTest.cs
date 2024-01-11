// <copyright file="VMEvaluatedCharacteristicTest.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using System.Diagnostics.CodeAnalysis;
using Mitutoyo.MiCAT.ReportModule.Domain.Data;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements.Table.ViewModels;
using NUnit.Framework;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Test.Components.TableView
{
   [ExcludeFromCodeCoverage]
   [TestFixture]
   public class VMEvaluatedCharacteristicTest
   {
      [Test]
      public void VMEvaluatedCharacteristic_ShouldInitializeWithEmptyPropertiesByDefault()
      {
         var sut = new VMEvaluatedCharacteristic(new EvaluatedCharacteristic(new Characteristic(), new CharacteristicActual(Guid.NewGuid(), "Invalid", null, null)));

         Assert.AreEqual(sut.CharacteristicName, string.Empty);
         Assert.IsNull(sut.CharacteristicType);
      }

      [Test]
      public void VMEvaluatedCharacteristic_ShouldInitializeProperties()
      {
         var characteristic = new Characteristic(id: Guid.NewGuid(),name: "CharacteristicName",
             characteristicType: "Cylindricity");
         var characteristicActual = new CharacteristicActual(characteristic.Id, "Pass", null, null);
         var sut = new VMEvaluatedCharacteristic(new EvaluatedCharacteristic(characteristic, characteristicActual));

         Assert.AreEqual(sut.CharacteristicName, "CharacteristicName");
         Assert.AreEqual(sut.CharacteristicType, "Cylindricity");
      }

      [Test]
      public void VMEvaluatedCharacteristic_CharacteristicTypeShouldBeLocalized()
      {
         var characteristic = new Characteristic(id: Guid.NewGuid(), name: "CharacteristicName",
             characteristicType: "TaperAngle");
         var characteristicActual = new CharacteristicActual(characteristic.Id, "Pass", null, null);
         var sut = new VMEvaluatedCharacteristic(new EvaluatedCharacteristic(characteristic, characteristicActual));

         Assert.AreEqual(sut.CharacteristicName, "CharacteristicName");
         Assert.AreEqual(sut.CharacteristicType, Properties.Resources.CharacteristicType_TaperAngle);
      }

      [Test]
      public void VMEvaluatedCharacteristic_CharacteristicDetailShouldBeLocalized()
      {
         var characteristic = new Characteristic(Guid.NewGuid(), "CharacteristicName", null, null, null, "Min", null, null, null);
         var characteristicActual = new CharacteristicActual(characteristic.Id, "Pass", null, null);
         var sut = new VMEvaluatedCharacteristic(new EvaluatedCharacteristic(characteristic, characteristicActual));

         Assert.AreEqual(sut.CharacteristicName, "CharacteristicName");
         Assert.AreEqual(sut.Details, Properties.Resources.CharacteristicDetail_Min);
      }

      [Test]
      public void VMEvaluatedCharacteristic_CharacteristicDetailNotLocalizedShouldNotBeNull()
      {
         var characteristic = new Characteristic(Guid.NewGuid(), "CharacteristicName", null, null, null, "Not-Localized", null, null, null);
         var characteristicActual = new CharacteristicActual(characteristic.Id, "Pass", null, null);
         var sut = new VMEvaluatedCharacteristic(new EvaluatedCharacteristic(characteristic, characteristicActual));

         Assert.AreEqual(sut.CharacteristicName, "CharacteristicName");
         Assert.AreEqual(sut.Details, "Not-Localized");
         Assert.IsNotNull(sut.Details);
      }

      [Test]
      public void VMEvaluatedCharacteristic_CharacteristicTypeNotLocalizedShouldNotBeEmpty()
      {
         var characteristic = new Characteristic(id: Guid.NewGuid(), name: "CharacteristicName",
             characteristicType: "Not-Localized");
         var characteristicActual = new CharacteristicActual(characteristic.Id, "Pass", null, null);
         var sut = new VMEvaluatedCharacteristic(new EvaluatedCharacteristic(characteristic, characteristicActual));

         Assert.AreEqual(sut.CharacteristicName, "CharacteristicName");
         Assert.AreEqual(sut.CharacteristicType, "Not-Localized");
         Assert.IsNotNull(sut.CharacteristicType);
      }

      [Test]
      public void VMEvaluatedCharacteristic_Should()
      {
         var characteristic = new Characteristic(id: Guid.NewGuid(), name: "CharacteristicName",
            characteristicType: "Cylindricity");
         var characteristicActual = new CharacteristicActual(characteristic.Id, "Pass", null, null);
         var sut = new VMEvaluatedCharacteristic(new EvaluatedCharacteristic(characteristic, characteristicActual));

         Assert.AreEqual(sut.CharacteristicName, "CharacteristicName");
         Assert.AreEqual(sut.CharacteristicType, "Cylindricity");
      }

      [Test]
      public void VMEvaluatedCharacteristic_FixedAmountDefaultColumns()
      {
         Assert.AreEqual(typeof(VMEvaluatedCharacteristic).GetProperties().Length, 10);
      }
   }
}
