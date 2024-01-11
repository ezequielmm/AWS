// <copyright file="DataServiceLocalizationFinderTest.cs" company="Mitutoyo Europe GmbH">
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
   public class DataServiceLocalizationFinderTest
   {
      [TestCase("CADViewString", "CAD View")]
      [TestCase("CharacteristicName", "Characteristic Name")]
      public void DataServiceLocalizationFinder_ShouldLocalizeExistingResources(string resourceKey, string expected)
      {
         var result = DataServiceLocalizationFinder.FindLocalizedString(resourceKey);

         Assert.AreEqual(expected, result);
      }

      [TestCase("BadName")]
      [TestCase("Unknown-Resource")]
      public void DataServiceLocalizationFinder_ShouldReturnEmptyStringWhenResourceNotExists(string resourceKey)
      {
         var result = DataServiceLocalizationFinder.FindLocalizedString(resourceKey);

         Assert.AreEqual(string.Empty, result);
      }

      [TestCase("AngleCoordinate", "Angle coordinate")]
      [TestCase("CircularRunout", "Circular runout")]
      public void DataServiceLocalizationFinder_ShouldLocalizeExistingCharacteristicTypeNames(string resourceKey, string expected)
      {
         var result = DataServiceLocalizationFinder.FindCharacteristicTypeName(resourceKey);

         Assert.AreEqual(expected, result);
      }

      [TestCase("Unknown")]
      [TestCase("BadKey")]
      public void DataServiceLocalizationFinder_ShouldUseTheKeyWhenResourceNotExists(string resourceKey)
      {
         var result = DataServiceLocalizationFinder.FindCharacteristicTypeName(resourceKey);

         Assert.AreEqual(resourceKey, result);
      }

      [TestCase("Min", "Min")]
      [TestCase("Max", "Max")]
      [TestCase("2D", "2D")]
      [TestCase("3D", "3D")]
      public void DataServiceLocalizationFinder_ShouldLocalizeExistingCharacteristicDetail(string resourceKey, string expected)
      {
         var result = DataServiceLocalizationFinder.FindCharacteristicDetailValue(resourceKey);

         Assert.AreEqual(expected, result);
      }

      [TestCase("Unknown")]
      [TestCase("BadKey")]
      public void DataServiceLocalizationFinder_ShouldUseTheKeyWhenCharacteriticDetailNotExists(string resourceKey)
      {
         var result = DataServiceLocalizationFinder.FindCharacteristicDetailValue(resourceKey);

         Assert.AreEqual(resourceKey, result);
      }
   }
}
