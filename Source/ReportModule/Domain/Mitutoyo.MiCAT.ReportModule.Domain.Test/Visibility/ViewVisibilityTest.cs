// <copyright file="ViewVisibilityTest.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using Mitutoyo.MiCAT.ReportModule.Domain.Visibility;
using NUnit.Framework;

namespace Mitutoyo.MiCAT.ReportModule.Domain.Test.Visibility
{
   [ExcludeFromCodeCoverage]
   [TestFixture]
   public class ViewVisibilityTest
   {
      [Test]
      [TestCase(false, false)]
      [TestCase(false, true)]
      [TestCase(true, false)]
      [TestCase(true, true)]
      public void WithVisibleShouldReturnExpectedVisibility(bool initialVisible, bool newVisibie)
      {
         //Arrange
         var sut = new ViewVisibility(ViewElement.Plans, initialVisible);

         //Act
         var newVisibility = sut.WithVisible(newVisibie);

         //Assert
         Assert.AreEqual(newVisibie, newVisibility.IsVisible);
      }

      [Test]
      [TestCase(false)]
      [TestCase(true)]
      public void WithToggleShouldChangeVisibility(bool isVisible)
      {
         //Arrange
         var sut = new ViewVisibility(ViewElement.Plans, isVisible);

         //Act
         var newVisibility = sut.WithToggle();

         //Assert
         Assert.AreEqual(!isVisible, newVisibility.IsVisible);
      }
   }
}
