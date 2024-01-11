// <copyright file="LineRenderer.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using Mitutoyo.MiCAT.ReportModule.PDFRenderer.Utilities;

namespace Mitutoyo.MiCAT.ReportModule.PDFRenderer.Renderers.ConcreteRenderers
{
   [ExcludeFromCodeCoverage]
   public class LineRenderer : UIElementRendererBase
    {
        public override bool Render(UIElement element, FixedDocumentRenderContext context)
        {
            var line = element as Line;
            if (line == null)
            {
                return false;
            }

            DrawLine(context, line.X1, line.Y1, line.X2, line.Y2, line.StrokeThickness, line.Stroke, line.StrokeDashArray);

            return true;
        }

       public static void DrawLine(FixedDocumentRenderContext context, double x1, double y1, double x2, double y2, double strokeThickness, Brush stroke, DoubleCollection dashArray)
        {
            using (context.DrawingSurface.SaveGraphicProperties())
            {
                if (x1 == x2 && System.Math.Abs(y1 - y2) > 10)
                {
                    if (y2 < y1)
                    {
                        var max = y1;
                        y1 = y2;
                        y2 = max;
                    }

                    // offset the start position of the line to resolve a visual bug in Adobe Reader, where the axis line seems to continue after the last tick
                    y1 += 0.5;
                }

                SetStroke(context, strokeThickness, stroke, Math.Abs(x2 - x1), Math.Abs(y2 - y1), dashArray);
                if (context.DrawingSurface.GraphicProperties.IsStroked)
                {
                    context.DrawingSurface.DrawLine(new Point(x1, y1), new Point(x2, y2));
                }
            }
        }
    }
}
