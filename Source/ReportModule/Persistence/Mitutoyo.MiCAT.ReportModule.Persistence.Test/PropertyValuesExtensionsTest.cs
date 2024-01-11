// <copyright file="PropertyValuesExtensionsTest.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Mitutoyo.MiCAT.Web.Data;
using NUnit.Framework;

namespace Mitutoyo.MiCAT.ReportModule.Persistence.Test
{
   [ExcludeFromCodeCoverage]
   [TestFixture]
   public class PropertyValuesExtensionsTest
   {
      private List<PropertyValueDTO> propertyValuesDouble;
      private List<PropertyValueDTO> propertyValuesString;

      [SetUp]
      public virtual void SetUp()
      {
         propertyValuesDouble = new List<PropertyValueDTO>();
         propertyValuesDouble.Add(new PropertyValueDTO() { Name = "TestName 1", Value = "1.25" });
         propertyValuesDouble.Add(new PropertyValueDTO() { Name = "TestName 2", Value = "2.15" });
         propertyValuesDouble.Add(new PropertyValueDTO() { Name = "TestName 3", Value = "3.45" });

         propertyValuesString = new List<PropertyValueDTO>();
         propertyValuesString.Add(new PropertyValueDTO() { Name = "TestName A", Value = "TestValue A" });
         propertyValuesString.Add(new PropertyValueDTO() { Name = "TestName B", Value = "TestValue B" });
         propertyValuesString.Add(new PropertyValueDTO() { Name = "TestName C", Value = "TestValue C" });
      }

      [Test]
      [TestCase("TestName 1", 1.25)]
      [TestCase("TestName 2", 2.15)]
      [TestCase("TestName 3", 3.45)]
      [TestCase("Invalid Name", null)]
      [TestCase("", null)]
      [TestCase(null, null)]
      public void GetSafeDoubleValueTest(string propertyName, double? expectedValue)
      {
         // Act
         var result = propertyValuesDouble.GetSafeDoubleValue(propertyName);

         // Assert
         Assert.AreEqual(expectedValue, result);
      }

      [Test]
      [TestCase("TestName A", "TestValue A")]
      [TestCase("TestName B", "TestValue B")]
      [TestCase("TestName C", "TestValue C")]
      [TestCase("Invalid Name", null)]
      [TestCase("", null)]
      [TestCase(null, null)]
      public void GetSafeStringValueTest(string propertyName, string expectedValue)
      {
         // Act
         var result = propertyValuesString.GetSafeStringValue(propertyName);

         // Assert
         Assert.AreEqual(expectedValue, result);
      }
   }
}
