// <copyright file="ResizeDeltaEventToResizeCommandArgsValueConverterTests.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.Containers.InteractiveControlContainer;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.Containers.ReportElementContainer;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements;
using NUnit.Framework;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Test.VisualElements.Containers.ReportElementContainer
{
   [TestFixture]
   [ExcludeFromCodeCoverage]
   public class ResizeDeltaEventToResizeCommandArgsValueConverterTests
   {
      [Test]
      [TestCase(0, false, 0, false)]
      [TestCase(10, true, 20, true)]
      [TestCase(100, false, 200, true)]
      public void ResizeDeltaEvent_ShouldBeConvertedToResizeCommandArgs(
         int horizontalChange,
         bool fromLeft,
         int verticalChange,
         bool fromTop)
      {
         var args = new ResizeDeltaEventArgs(horizontalChange, fromLeft, verticalChange, fromTop);
         var converter = new ResizeDeltaEventToResizeCommandArgsValueConverter();

         var result = converter.Convert(args, typeof(ResizeCommandArgs), null, CultureInfo.InvariantCulture);

         Assert.IsNotNull(result);
         Assert.IsInstanceOf<ResizeCommandArgs>(result);
         Assert.AreEqual(horizontalChange, ((ResizeCommandArgs)result).HorizontalDelta);
         Assert.AreEqual(fromLeft, ((ResizeCommandArgs)result).IsFromLeftSide);
         Assert.AreEqual(verticalChange, ((ResizeCommandArgs)result).VerticalDelta);
         Assert.AreEqual(fromTop, ((ResizeCommandArgs)result).IsFromTopSide);
      }
   }
}
