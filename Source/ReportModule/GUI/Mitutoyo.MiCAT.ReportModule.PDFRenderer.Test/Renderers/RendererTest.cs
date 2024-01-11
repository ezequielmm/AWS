// <copyright file="RendererTest.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Windows;
using System.Windows.Shapes;
using Mitutoyo.MiCAT.ReportModule.PDFRenderer.Renderers;
using NUnit.Framework;
using Telerik.Windows.Documents.Fixed.Model;
using Telerik.Windows.Documents.Fixed.Model.Editing;

namespace Mitutoyo.MiCAT.ReportModule.PDFRenderer.Test.Renderers
{
   [ExcludeFromCodeCoverage]
   [TestFixture]
   public class RendererTest
   {
      [Test]
      public void Render_AddRenderer()
      {
         // Arrange
         var render = new Renderer();

         // Act

         // Assert
         Assert.AreEqual(11, render.Renderers.Count);
      }

      [Test]
      public void Render_NullElementShouldNotBeRenderer()
      {
         // Arrange
        var editor = new FixedContentEditor(new RadFixedPage(),
            Telerik.Windows.Documents.Fixed.Model.Data.MatrixPosition.Default);
         var render = new Renderer();

         // Act
         var result = render.Render(null, null, editor);

         // Assert
         Assert.IsFalse(result);
      }

      [Test]
      [Apartment(ApartmentState.STA)]
      public void Render_NotVisibleElementShouldNotBeRenderer()
      {
         // Arrange
         var editor = new FixedContentEditor(new RadFixedPage(),
            Telerik.Windows.Documents.Fixed.Model.Data.MatrixPosition.Default);
         var render = new Renderer();
         var element = new Line(){Visibility = Visibility.Hidden};

         // Act
         var result = render.Render(element, null, editor);

         // Assert
         Assert.IsFalse(result);
      }

      [Test]
      [Apartment(ApartmentState.STA)]
      public void Render_ZeroOpacityElementShouldNotBeRenderer()
      {
         // Arrange
         var editor = new FixedContentEditor(new RadFixedPage(),
            Telerik.Windows.Documents.Fixed.Model.Data.MatrixPosition.Default);
         var render = new Renderer();
         var element = new Line() { Opacity = 0 };

         // Act
         var result = render.Render(element, null, editor);

         // Assert
         Assert.IsFalse(result);
      }

      [Test]
      public void Render_ElementsShouldNotBeRenderer()
      {
         // Arrange
         var element = new UIElement();
         var editor = new FixedContentEditor(new RadFixedPage(),
            Telerik.Windows.Documents.Fixed.Model.Data.MatrixPosition.Default);
         var render = new Renderer();

         // Act
         var result = render.Render(element, null, editor);

         // Assert
         Assert.IsFalse(result);
      }
   }
}
