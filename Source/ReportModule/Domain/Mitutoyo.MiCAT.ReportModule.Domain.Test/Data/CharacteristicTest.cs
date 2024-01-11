// <copyright file="CharacteristicTest.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;
namespace Mitutoyo.MiCAT.ReportModule.Domain.Test.Data
{
   [ExcludeFromCodeCoverage]
   [TestFixture]
   public class CharacteristicTest
   {
      [Test]
      [TestCase("Characteristic1", "Flatness", 0d, 0d)]
      [TestCase("Characteristic2", "Cylindricity", 10d, 11d)]
      public void With_ShouldCreateACopyIncludingNewNameCharacteristicChange(string name,
         string type,
         double lowerTolerance,
         double upperTolerance)
      {
         // Arrange
         var characteristic = new Domain.Data.Characteristic(Guid.NewGuid(), name, type);

         // Act
         var lastCharacteristic = characteristic.WithName("newName");

         // Assert
         Assert.AreEqual(lastCharacteristic.Name, "newName");
         Assert.AreEqual(lastCharacteristic.CharacteristicType, type);
      }

      [Test]
      [TestCase("Characteristic1", "Flatness", 0d)]
      [TestCase("Characteristic2", "Cylindricity", 12d)]
      public void With_ShouldCreateACopyIncludingNewTypeCharacteristicChange(string name,
         string type,
         double toleranceZone)
      {
         // Arrange
         var characteristic = new Domain.Data.Characteristic(Guid.NewGuid(), name, type);

         // Act
         var lastCharacteristic = characteristic.WithCharacteristicType("Flatness");

         // Assert
         Assert.AreEqual(lastCharacteristic.Name, name);
         Assert.AreEqual(lastCharacteristic.CharacteristicType, "Flatness");
      }

      [Test]
      public void Constructor_CreateValueByDefault()
      {
         // Arrange
         var characteristic = new Domain.Data.Characteristic();

         // Act
         var name = characteristic.Name;
         var type = characteristic.CharacteristicType;

         // Assert
         Assert.IsEmpty(name);
         Assert.IsNull(type);
      }
   }
}
