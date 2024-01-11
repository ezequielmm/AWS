// <copyright file="CharacteristicDetailsProviderTest.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using System.Linq;
using NUnit.Framework;

namespace Mitutoyo.MiCAT.ReportModule.Persistence.Test
{
   [ExcludeFromCodeCoverage]
   [TestFixture]
   public class CharacteristicDetailsProviderTest
   {
      [Test]
      public void GetAllCharacteristicDetails()
      {
         // Arrange
         var characteristicDetailsProvider = new CharacteristicDetailsProvider();

         // Act
         var result = characteristicDetailsProvider.GetAllCharacteristicDetails();

         // Assert
         Assert.AreEqual(33, result.Count());
         Assert.AreEqual("DX", result.ElementAt(0));
         Assert.AreEqual("Radius 2", result.ElementAt(32));
      }
   }
}
