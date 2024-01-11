// <copyright file="UIElementRendererBase.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Media;
using Mitutoyo.MiCAT.ReportModule.PDFRenderer.Helpers;
using Mitutoyo.MiCAT.ReportModule.PDFRenderer.Utilities;
using Telerik.Windows.Documents.Fixed.Model.Editing;

namespace Mitutoyo.MiCAT.ReportModule.PDFRenderer.Renderers.ConcreteRenderers
{
   [ExcludeFromCodeCoverage]
   public abstract class UIElementRendererBase: IUIElementRendererBase
   {
       public static IDisposable SaveClip(FixedContentEditor drawingSurface, UIElement element)
        {
            Geometry clip = null;
            var frameworkElement = element as FrameworkElement;
            if (frameworkElement != null)
            {
                clip = System.Windows.Controls.Primitives.LayoutInformation.GetLayoutClip(frameworkElement);
            }

            if (clip == null)
            {
                clip = element.Clip;
            }

            var rectangleClip = clip as RectangleGeometry;
            if (rectangleClip == null)
            {
                return null;
            }

            return SaveClip(drawingSurface, rectangleClip.Rect);
        }

       public static IDisposable SaveClip(FixedContentEditor drawingSurface, Rect rectangle)
        {
            var geometry = MathHelper.TransformRectangle(drawingSurface.Position.Matrix, rectangle);
            var pdfGeometry = GeometryHelper.ConvertPathGeometry(geometry);

            return drawingSurface.PushClipping(pdfGeometry);
        }

       public static IDisposable SaveMatrixPosition(FixedContentEditor drawingSurface, FrameworkElement element, UIElement positionRelativeElement)
        {
            if (element == null)
            {
                return null;
            }

            var transform = MathHelper.GetGeneralTransform(element, positionRelativeElement);
            var matrix = MathHelper.CreateMatrix(transform);
            if (matrix.IsIdentity)
            {
                return null;
            }

            matrix = MathHelper.Multiply(matrix, drawingSurface.Position.Matrix);
            var savePosition = drawingSurface.SavePosition();
            drawingSurface.Position = new Telerik.Windows.Documents.Fixed.Model.Data.MatrixPosition(matrix);

            return savePosition;
        }

       public static IDisposable SaveOpacity(FixedDocumentRenderContext context, double newOpacity)
        {
            if (context.Opacity != newOpacity)
            {
                var disposableOpacity = new DisposableOpacity(context);
                context.Opacity = newOpacity;
                return disposableOpacity;
            }
            else
            {
                return null;
            }
        }

       public static void SetFill(FixedDocumentRenderContext context, Brush brush, double width, double height)
        {
            var fill = BrushHelper.ConvertBrush(brush, context.Opacity, context.DrawingSurface.Position, width, height);

            context.DrawingSurface.GraphicProperties.IsFilled = fill != null;
            context.DrawingSurface.GraphicProperties.FillColor = fill;
        }

       public static void SetStroke(FixedDocumentRenderContext context, double thickness, Brush brush, double width, double height, DoubleCollection dashArray)
        {
            var stroke = BrushHelper.ConvertBrush(brush, context.Opacity, context.DrawingSurface.Position, width, height);
            context.DrawingSurface.GraphicProperties.IsStroked = thickness != 0 && stroke != null;

            if (context.DrawingSurface.GraphicProperties.IsStroked)
            {
                context.DrawingSurface.GraphicProperties.StrokeThickness = thickness;
                context.DrawingSurface.GraphicProperties.StrokeColor = stroke;

                if (dashArray != null)
                {
                    context.DrawingSurface.GraphicProperties.StrokeDashArray = dashArray;
                }
            }
        }

        public abstract bool Render(UIElement element, FixedDocumentRenderContext context);
    }
}
