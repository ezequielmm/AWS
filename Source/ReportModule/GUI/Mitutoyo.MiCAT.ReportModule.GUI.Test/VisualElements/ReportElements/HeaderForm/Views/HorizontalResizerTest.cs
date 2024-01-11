// <copyright file="HorizontalResizerTest.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using System.Threading;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements.HeaderForm.Views;
using NUnit.Framework;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Test.VisualElements.ReportElements.HeaderForm.Views
{
   [ExcludeFromCodeCoverage]
   [TestFixture]
   public class HorizontalResizerTest
   {
      [Test]
      [TestCase(25, 75, 0, 25)]
      [TestCase(25, 75, 50, 50)]
      [TestCase(25, 75, 100, 75)]
      [TestCase(0, 100, 34, 34)]
      [Apartment(ApartmentState.STA)]
      public void HorizontalResizer_ShouldCalculateWidthLength(double minLength, double maxLength, double widthPercentage, double expectedWidthLength)
      {
         var resizer = new HorizontalResizer { MinLength = minLength, MaxLength = maxLength, WidthPercentage = widthPercentage};
         Assert.AreEqual(expectedWidthLength, resizer.WidthLength);
      }

      [Test]
      [Apartment(ApartmentState.STA)]
      public void HorizontalResizer_ShouldUseMinLengthWith0Percentage()
      {
         var resizer = new HorizontalResizer { MinLength = 25, MaxLength = 100, WidthPercentage = 0 };
         Assert.AreEqual(25, resizer.WidthLength);
      }

      [Test]
      [Apartment(ApartmentState.STA)]
      public void HorizontalResizer_ShouldUseMaxLengthWith100Percentage()
      {
         var resizer = new HorizontalResizer { MinLength = 25, MaxLength = 100, WidthPercentage = 100 };
         Assert.AreEqual(100, resizer.WidthLength);
      }
   }
}
