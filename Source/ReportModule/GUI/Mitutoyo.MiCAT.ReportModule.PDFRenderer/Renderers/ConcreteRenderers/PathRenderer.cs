// <copyright file="PathRenderer.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Shapes;
using Mitutoyo.MiCAT.ReportModule.PDFRenderer.Helpers;
using Mitutoyo.MiCAT.ReportModule.PDFRenderer.Utilities;

namespace Mitutoyo.MiCAT.ReportModule.PDFRenderer.Renderers.ConcreteRenderers
{
   [ExcludeFromCodeCoverage]
   public class PathRenderer : UIElementRendererBase
    {
        public override bool Render(UIElement element, FixedDocumentRenderContext context)
        {
            Path path = element as Path;
            if (path == null)
            {
                return false;
            }

            var pdfGeometry = GeometryHelper.ConvertGeometry(path.Data);
            if (pdfGeometry == null)
            {
                return false;
            }

            using (context.DrawingSurface.SaveGraphicProperties())
            {
                SetFill(context, path.Fill, path.ActualWidth, path.ActualHeight);
                SetStroke(context, path.StrokeThickness, path.Stroke, path.ActualWidth, path.ActualHeight, path.StrokeDashArray);

                if (context.DrawingSurface.GraphicProperties.IsFilled || context.DrawingSurface.GraphicProperties.IsStroked)
                {
                    context.DrawingSurface.DrawPath(pdfGeometry);
                }

                return true;
            }
        }
    }
}
