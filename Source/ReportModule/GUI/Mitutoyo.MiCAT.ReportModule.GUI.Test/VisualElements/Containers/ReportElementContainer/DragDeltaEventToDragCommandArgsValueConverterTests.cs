// <copyright file="DragDeltaEventToDragCommandArgsValueConverterTests.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Windows.Controls.Primitives;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.Containers.ReportElementContainer;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements;
using NUnit.Framework;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Test.VisualElements.Containers.ReportElementContainer
{
   [TestFixture]
   [ExcludeFromCodeCoverage]
   public class DragDeltaEventToDragCommandArgsValueConverterTests
   {
      [Test]
      [TestCase(0, 0)]
      [TestCase(10, 20)]
      [TestCase(100, 200)]
      public void DragDeltaEvent_ShouldBeConvertedToDragCommandArgs(int horizontalChange, int verticalChange)
      {
         var args = new DragDeltaEventArgs(horizontalChange, verticalChange);
         var converter = new DragDeltaEventToDragCommandArgsValueConverter();

         var result = converter.Convert(args, typeof(DragCommandArgs), null, CultureInfo.InvariantCulture);

         Assert.IsNotNull(result);
         Assert.IsInstanceOf<DragCommandArgs>(result);
         Assert.AreEqual(horizontalChange, ((DragCommandArgs)result).HorizontalDelta);
         Assert.AreEqual(verticalChange, ((DragCommandArgs)result).VerticalDelta);
      }
   }
}
