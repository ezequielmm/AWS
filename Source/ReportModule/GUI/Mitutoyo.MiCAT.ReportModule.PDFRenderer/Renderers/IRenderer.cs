// <copyright file="IRenderer.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Windows;
using Mitutoyo.MiCAT.ReportModule.PDFRenderer.Utilities;
using Telerik.Windows.Documents.Fixed.Model.Editing;

namespace Mitutoyo.MiCAT.ReportModule.PDFRenderer.Renderers
{
   public interface IRenderer
   {
      bool Render(UIElement element, UIElement positionRelativeElement, FixedContentEditor drawingSurface);
      bool Render(UIElement element, UIElement positionRelativeElement, FixedDocumentRenderContext context);
      bool Render(UIElement element, FixedDocumentRenderContext context);
   }
}
