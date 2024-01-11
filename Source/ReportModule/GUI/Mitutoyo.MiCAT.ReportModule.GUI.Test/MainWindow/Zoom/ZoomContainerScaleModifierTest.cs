// <copyright file="ZoomContainerScaleModifierTest.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Media;
using Mitutoyo.MiCAT.ReportModule.GUI.MainWindow.Zoom;
using NUnit.Framework;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Test.MainWindow.Zoom
{
   [ExcludeFromCodeCoverage]
   [TestFixture]
   public class ZoomContainerScaleModifierTest
   {
      private ZoomContainerScaleModifier zoomScaleModifier;

      [SetUp]
      public void Setup()
      {
         zoomScaleModifier = new ZoomContainerScaleModifier();
      }

      [Test]
      [Apartment(ApartmentState.STA)]
      public void VMMainToolbar_ShouldExportAsEnabledIfIsNotEditMode()
      {
         // Arrange
         Grid grid = new Grid();
         TransformGroup transformGroup = new TransformGroup();
         transformGroup.Children.Add(new ScaleTransform());
         grid.LayoutTransform = transformGroup;

         // Act
         zoomScaleModifier.ModifyScale(grid, new ScrollViewer(), 1, 3);

         // Assert
         var scaleTransform = transformGroup.Children.FirstOrDefault() as ScaleTransform;

         Assert.AreEqual(scaleTransform.ScaleX, 3);
         Assert.AreEqual(scaleTransform.ScaleY, 3);
      }
   }
}
