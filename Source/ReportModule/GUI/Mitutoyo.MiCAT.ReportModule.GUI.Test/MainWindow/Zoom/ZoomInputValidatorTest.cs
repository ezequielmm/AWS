// <copyright file="ZoomInputValidatorTest.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using Mitutoyo.MiCAT.ReportModule.GUI.MainWindow.Zoom;
using NUnit.Framework;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Test.MainWindow.Zoom
{
   [ExcludeFromCodeCoverage]
   [TestFixture]
   public class ZoomInputValidatorTest
   {
      private PercentageInputValidator zoomInputValidator;

      [SetUp]
      public void Setup()
      {
         zoomInputValidator = new PercentageInputValidator();
      }

      [Test]
      [TestCase("50%", true, 0.5d)]
      [TestCase("150%", true, 1.5d)]
      [TestCase("500%", true, 5d)]
      [TestCase("InvalidText", false, 0d)]
      public void ShouldValidateInput(string inputText, bool isValid, double percentValue)
      {
         // Act
         var result = zoomInputValidator.ValidateInput(inputText);

         // Assert
         Assert.AreEqual(isValid, result.IsValid);
         Assert.AreEqual(result.PercentValue, percentValue);
      }

      [Test]
      [TestCase("1", true)]
      [TestCase("5", true)]
      [TestCase("%", true)]
      [TestCase("A", false)]
      [TestCase("", false)]
      public void ShouldValidateAllowedCharacter(string inputText, bool isValid)
      {
         // Act
         bool isValidCharacter = zoomInputValidator.IsAllowedCharacter(inputText);

         // Assert
         Assert.AreEqual(isValid, isValidCharacter);
      }
   }
}
