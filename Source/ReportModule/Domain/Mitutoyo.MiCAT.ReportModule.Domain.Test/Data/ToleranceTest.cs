// <copyright file="ToleranceTest.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using Mitutoyo.MiCAT.ReportModule.Domain.Data;
using NUnit.Framework;

namespace Mitutoyo.MiCAT.ReportModule.Domain.Test.Data
{
   [ExcludeFromCodeCoverage]
   [TestFixture]
   public class ToleranceTest
   {
      [Test]
      [TestCase(0d, 0d)]
      [TestCase(10d, 11d)]
      public void With_ShouldCreateACopyIncludingNewLowerToleranceChange(double lowerTolerance,
         double upperTolerance)
      {
         // Arrange
         var toleranceActual = new UpLowTolerance(upperTolerance, lowerTolerance);

         // Act
         var lastTolerance = toleranceActual.WithLowerTolerance(15d);

         // Assert
         Assert.AreEqual(lastTolerance.LowerTolerance, 15d);
         Assert.AreEqual(lastTolerance.UpperTolerance, upperTolerance);
      }

      [Test]
      [TestCase(0d, 0d)]
      [TestCase(10d, 11d)]
      public void With_ShouldCreateACopyIncludingNewUpperToleranceChange(double lowerTolerance,
         double upperTolerance)
      {
         // Arrange
         var toleranceActual = new UpLowTolerance(upperTolerance, lowerTolerance);

         // Act
         var lastTolerance = toleranceActual.WithUpperTolerance(25d);

         // Assert
         Assert.AreEqual(lastTolerance.UpperTolerance, 25d);
         Assert.AreEqual(lastTolerance.LowerTolerance, lowerTolerance);
      }

      [Test]
      [TestCase(0d)]
      [TestCase(12d)]
      public void With_ShouldCreateACopyIncludingNewToleranceZoneChange(double toleranceZone)
      {
         // Arrange
         var toleranceActual = new ZoneTolerance(toleranceZone);

         // Act
         var lastTolerance = toleranceActual.WithToleranceZone(35d);

         // Assert
         Assert.AreEqual(lastTolerance.ToleranceZone, 35d);
      }

      [Test]
      public void Constructor_CreateValueByDefault()
      {
         // Arrange
         var toleranceActual = new ZoneTolerance();

         // Act
         var toleranceZone = toleranceActual.ToleranceZone;

         // Assert
         Assert.AreEqual(toleranceZone, 0D);
      }
   }
}
