// <copyright file="VMDynamicPropertyItemTest.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using Mitutoyo.MiCAT.ReportModule.GUI.Properties;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements.HeaderForm.ViewModels;
using NUnit.Framework;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Test.VisualElements.ReportElements.HeaderForm.ViewModels
{
   [TestFixture]
   [ExcludeFromCodeCoverage]
   public class VMDynamicPropertyItemTest
   {
      [Test]
      [TestCase("FakeXYZ")]
      [TestCase("UnExistingName")]
      public void VMDynamicPropertyItem_LocalizeUnknownResource_ShouldNotBeLocalized(string name)
      {
         var result = VMDynamicPropertyItem.LocalizeDynamicPropertyItemName(name);
         Assert.AreEqual(name, result);
      }

      [Test]
      [TestCase(nameof(Resources.FieldItem_CompanyName))]
      [TestCase(nameof(Resources.FieldItem_DrawingNumber))]
      public void VMDynamicPropertyItem_LocalizeExistingName_ShouldBeLocalized(string resourceName)
      {
         var expected = new System.Resources.ResourceManager(typeof(Resources)).GetString(resourceName);

         var fieldItemName = resourceName.Replace("FieldItem_", string.Empty);
         var result = VMDynamicPropertyItem.LocalizeDynamicPropertyItemName(fieldItemName);
         Assert.AreEqual(expected, result);
      }
   }
}
