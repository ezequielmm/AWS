// <copyright file="DynamicPropertyItemValueResolverTest.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Mitutoyo.MiCAT.ReportModule.Domain.Data;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements.HeaderForm.ViewModels;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements.HeaderForm.ViewModels.ValueResolvers;
using NUnit.Framework;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Test.VisualElements.ReportElements.HeaderForm.ViewModels.ValueResolvers
{
   [TestFixture]
   [ExcludeFromCodeCoverage]
   public class DynamicPropertyItemValueResolverTest
   {
      private IEnumerable<DynamicPropertyValue> _dynamicPropertyValues;

      [SetUp]
      public void SetUp()
      {
         _dynamicPropertyValues = new[]
         {
            new DynamicPropertyValue { Name = "Prop1", Value = "ValueProp1" },
            new DynamicPropertyValue { Name = "Prop2", Value = "ValueProp2" },
            new DynamicPropertyValue { Name = "Prop3", Value = "ValueProp3" },
         };
      }

      [Test]
      [TestCase("Prop1", "ValueProp1")]
      [TestCase("Prop2", "ValueProp2")]
      [TestCase("Prop3", "ValueProp3")]
      public void GetValueForAGivenProperty(string name, string value)
      {
         var resolver = new DynamicPropertyValueResolver(() => _dynamicPropertyValues, new VMDynamicPropertyItem { Name = name });
         var result = resolver.GetValue();
         Assert.AreEqual(value, result);
      }

      [Test]
      [TestCase("Fake")]
      [TestCase("Unknown")]
      [TestCase("Invalid")]
      public void GetValueForAUnknownPropertyShouldReturnEmptyString(string name)
      {
         var resolver = new DynamicPropertyValueResolver(() => _dynamicPropertyValues, new VMDynamicPropertyItem { Name = name });
         var result = resolver.GetValue();
         Assert.IsEmpty(result);
      }
   }
}
