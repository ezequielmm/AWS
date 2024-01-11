// <copyright file="VMVisualPlacementTests.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements;
using NUnit.Framework;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Test.VisualElements
{
   [TestFixture]
   [ExcludeFromCodeCoverage]
   public class VMVisualPlacementTests
   {
      [Test]
      public void Properties_ShouldHaveDefaultValues()
      {
         var element = new VMFake();

         Assert.IsNotNull(element);
         Assert.AreEqual(default(int), element.VisualX);
         Assert.AreEqual(default(int), element.VisualY);
         Assert.AreEqual(default(int), element.VisualWidth);
         Assert.AreEqual(default(int), element.VisualHeight);
         Assert.AreEqual(default(bool), element.AutoAdjustVisualWidth);
         Assert.AreEqual(default(bool), element.AutoAdjustVisualHeight);
      }

      [Test]
      [TestCase(1, 1, 1, 1, true, true)]
      [TestCase(5, 10, 15, 20, false, true)]
      [TestCase(0, 10, 0, 10, false, false)]
      public void Setters_ShouldSetValues(int x, int y, int width, int height, bool autoWidth, bool autoHeight)
      {
         var element = new VMFake(x, y, width, height, autoWidth, autoHeight);

         Assert.IsNotNull(element);
         Assert.AreEqual(x, element.VisualX);
         Assert.AreEqual(y, element.VisualY);
         Assert.AreEqual(width, element.VisualWidth);
         Assert.AreEqual(height, element.VisualHeight);
         Assert.AreEqual(autoWidth, element.AutoAdjustVisualWidth);
         Assert.AreEqual(autoHeight, element.AutoAdjustVisualHeight);
      }

      private class VMFake : VMVisualPlacement
      {
         public VMFake()
         {
         }

         public VMFake(int x, int y, int width, int height, bool autoWidth, bool autoHeight)
         {
            VisualX = x;
            VisualY = y;
            VisualWidth = width;
            VisualHeight = height;
            AutoAdjustVisualWidth = autoWidth;
            AutoAdjustVisualHeight = autoHeight;
         }
      }
   }
}
