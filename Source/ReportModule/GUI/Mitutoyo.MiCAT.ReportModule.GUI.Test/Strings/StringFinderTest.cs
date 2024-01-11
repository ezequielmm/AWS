// <copyright file="StringFinderTest.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using Mitutoyo.MiCAT.ReportModule.GUI.Strings;
using NUnit.Framework;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Test.Strings
{
   [ExcludeFromCodeCoverage]
   [TestFixture]
   public class StringFinderTest
   {
      [TestCase("CADViewString", "CAD View")]
      [TestCase("CharacteristicName", "Characteristic Name")]
      public void StringFinder_ShouldLocalizeExistingResources(string resourceKey, string expected)
      {
         var result = StringFinder.FindLocalizedString(resourceKey);

         Assert.AreEqual(expected, result);
      }

      [TestCase("BadName")]
      [TestCase("Unknown-Resource")]
      public void StringFinder_ShouldReturnEmptyStringWhenResourceNotExists(string resourceKey)
      {
         var result = StringFinder.FindLocalizedString(resourceKey);

         Assert.AreEqual(string.Empty, result);
      }

      [TestCase("CADViewString", "default-value","CAD View")]
      [TestCase("CharacteristicName", null, "Characteristic Name")]
      public void StringFinder_ShouldLocalizeExistingResourcesWithDefaultValue(string resourceKey, string defaultValue, string expected)
      {
         var result = StringFinder.FindLocalizedString(resourceKey, defaultValue);

         Assert.AreEqual(expected, result);
      }

      [TestCase("BadName", "default-value", "default-value")]
      [TestCase("Unknown-Resource", null, null)]
      [TestCase("Unknown-Resource", "", "")]
      public void StringFinder_ShouldUseDefaultValueWhenResourceNotExists(string resourceKey, string defaultValue, string expected)
      {
         var result = StringFinder.FindLocalizedString(resourceKey, defaultValue);

         Assert.AreEqual(expected, result);
      }
   }
}
