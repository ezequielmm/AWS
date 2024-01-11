// <copyright file="RectangleRenderer.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using Mitutoyo.MiCAT.ReportModule.PDFRenderer.Utilities;

namespace Mitutoyo.MiCAT.ReportModule.PDFRenderer.Renderers.ConcreteRenderers
{
   [ExcludeFromCodeCoverage]
   public class RectangleRenderer : UIElementRendererBase
    {
        public override bool Render(UIElement element, FixedDocumentRenderContext context)
        {
            var rectangle = element as Rectangle;
            if (rectangle == null)
            {
                return false;
            }

            var hasStroke = rectangle.Stroke != null && rectangle.StrokeThickness != 0;
            var isLine = rectangle.ActualWidth == 1 || rectangle.ActualHeight == 1;

            if (isLine && !hasStroke)
            {
                // export the thin Rectangle as a Line to work-around a visual bug in Adobe Reader

                // account for the different rendering in wpf's Rectangle and pdf's line
                Point startPoint;
                Point endPoint;
                if (rectangle.ActualWidth == 1)
                {
                    startPoint = new Point(0.5, 0);
                    endPoint = new Point(0.5, rectangle.ActualHeight);
                }
                else
                {
                    startPoint = new Point(0.5, 0.5);
                    endPoint = new Point(0.5 + rectangle.ActualWidth, 0.5);
                }

                LineRenderer.DrawLine(context, startPoint.X, startPoint.Y, endPoint.X, endPoint.Y, 1, rectangle.Fill, null);
            }
            else
            {
                DrawRectangle(context, rectangle.Fill, rectangle.Stroke, rectangle.StrokeThickness, rectangle.ActualWidth, rectangle.ActualHeight, rectangle.StrokeDashArray);
            }

            return true;
        }

        public static void DrawRectangle(FixedDocumentRenderContext context, Brush fill, Brush stroke, double strokeThickness, double actualWidth, double actualHeight, DoubleCollection dashArray)
        {
            using (context.DrawingSurface.SaveGraphicProperties())
            {
                SetFill(context, fill, actualWidth, actualHeight);
                SetStroke(context, strokeThickness, stroke, actualWidth, actualHeight, dashArray);

                if (context.DrawingSurface.GraphicProperties.IsFilled || context.DrawingSurface.GraphicProperties.IsStroked)
                {
                    // account for the difference in the notion of stroke in wpf's Rectangle/Border and pdf's Rectangle
                    var thickness = context.DrawingSurface.GraphicProperties.IsStroked ? strokeThickness : 0;

                    context.DrawingSurface.DrawRectangle(new Rect(thickness / 2, thickness / 2, actualWidth - thickness, actualHeight - thickness));
                }
            }
        }
    }
}
