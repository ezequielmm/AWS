// <copyright file="FixedDocumentRenderContextTest.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using Mitutoyo.MiCAT.ReportModule.PDFRenderer.Utilities;
using Moq;
using NUnit.Framework;
using Telerik.Windows.Documents.Fixed.Model;
using Telerik.Windows.Documents.Fixed.Model.Editing;

namespace Mitutoyo.MiCAT.ReportModule.PDFRenderer.Test.Utilities
{
   [ExcludeFromCodeCoverage]
   [TestFixture]
   public class FixedDocumentRenderContextTest
   {
      [Test]
      public void FixedDocumentRenderContext_ShouldInitializeProperties()
      {
         // Arrange
         var editor = new FixedContentEditor(new RadFixedPage(), Telerik.Windows.Documents.Fixed.Model.Data.MatrixPosition.Default);
         var facadeMock = new Mock<PDFRenderer.Renderers.IRenderer>();

         // Act
         var context = new FixedDocumentRenderContext(editor, facadeMock.Object);

         // Assert
         Assert.AreEqual(context.DrawingSurface, editor);
         Assert.AreEqual(context.Facade, facadeMock.Object);
         Assert.AreEqual(context.Opacity, 1);
      }
   }
}
