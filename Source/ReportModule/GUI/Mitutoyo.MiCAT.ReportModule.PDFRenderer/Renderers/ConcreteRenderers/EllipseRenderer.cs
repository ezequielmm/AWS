// <copyright file="EllipseRenderer.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Shapes;
using Mitutoyo.MiCAT.ReportModule.PDFRenderer.Utilities;

namespace Mitutoyo.MiCAT.ReportModule.PDFRenderer.Renderers.ConcreteRenderers
{
   [ExcludeFromCodeCoverage]
   public class EllipseRenderer : UIElementRendererBase
    {
       public override bool Render(UIElement element, FixedDocumentRenderContext context)
        {
            var ellipse = element as Ellipse;
            if (ellipse == null)
            {
                return false;
            }

            using (context.DrawingSurface.SaveGraphicProperties())
            {
                SetStroke(context, ellipse.StrokeThickness, ellipse.Stroke, ellipse.ActualWidth, ellipse.ActualHeight, ellipse.StrokeDashArray);
                SetFill(context, ellipse.Fill, ellipse.ActualWidth, ellipse.ActualHeight);

                if (context.DrawingSurface.GraphicProperties.IsFilled || context.DrawingSurface.GraphicProperties.IsStroked)
                {
                    context.DrawingSurface.DrawEllipse(new Point(ellipse.ActualWidth / 2, ellipse.ActualHeight / 2), ellipse.ActualWidth / 2, ellipse.ActualHeight / 2);
                }
            }

            return true;
        }
    }
}
