// <copyright file="BorderRenderer.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Media;
using Mitutoyo.MiCAT.ReportModule.PDFRenderer.Utilities;

namespace Mitutoyo.MiCAT.ReportModule.PDFRenderer.Renderers.ConcreteRenderers
{
    [ExcludeFromCodeCoverage]
    public class BorderRenderer : UIElementRendererBase
    {
       public override bool Render(UIElement element, FixedDocumentRenderContext context)
        {
            var border = element as System.Windows.Controls.Border;
            if (border == null)
            {
                return false;
            }

            if (IsSimpleStrokeThickness(border.BorderThickness))
            {
                RectangleRenderer.DrawRectangle(context, border.Background, border.BorderBrush, border.BorderThickness.Left, border.ActualWidth, border.ActualHeight, null);
            }
            else
            {
                DrawBackground(context, border.Background, border.BorderThickness, border.ActualWidth, border.ActualHeight);
                DrawBorderStroke(context, border.BorderThickness, border.BorderBrush, border.ActualWidth, border.ActualHeight);
            }

            var firstChild = border.Child;
            context.Facade.Render(firstChild, context);

            return true;
        }

        private static bool IsSimpleStrokeThickness(Thickness thickness)
        {
            return thickness.Top == thickness.Right &&
                thickness.Top == thickness.Bottom &&
                thickness.Top == thickness.Left;
        }

        private void DrawBackground(FixedDocumentRenderContext context, Brush fill, Thickness thickness, double actualWidth, double actualHeight)
        {
            using (context.DrawingSurface.SaveGraphicProperties())
            {
                context.DrawingSurface.GraphicProperties.IsStroked = false;
                SetFill(context, fill, actualWidth, actualHeight);
                var innerRect = new Rect(thickness.Left, thickness.Top, actualWidth - thickness.Left - thickness.Right, actualHeight - thickness.Top - thickness.Bottom);
                context.DrawingSurface.DrawRectangle(innerRect);
            }
        }

        private void DrawBorderStroke(FixedDocumentRenderContext context, Thickness thickness, Brush stroke, double actualWidth, double actualHeight)
        {
            if (stroke == null)
            {
                return;
            }

            if (thickness.Left != 0)
            {
                LineRenderer.DrawLine(context, thickness.Left / 2, 0, thickness.Left / 2, actualHeight, thickness.Left, stroke, null);
            }
            if (thickness.Top != 0)
            {
                LineRenderer.DrawLine(context, 0, thickness.Top / 2, actualWidth, thickness.Top / 2, thickness.Top, stroke, null);
            }
            if (thickness.Right != 0)
            {
                var x = actualWidth - (thickness.Right / 2);
                LineRenderer.DrawLine(context, x, 0, x, actualHeight, thickness.Right, stroke, null);
            }
            if (thickness.Bottom != 0)
            {
                var y = actualHeight - (thickness.Bottom / 2);
                LineRenderer.DrawLine(context, 0, y, actualWidth, y, thickness.Bottom, stroke, null);
            }
        }
    }
}
