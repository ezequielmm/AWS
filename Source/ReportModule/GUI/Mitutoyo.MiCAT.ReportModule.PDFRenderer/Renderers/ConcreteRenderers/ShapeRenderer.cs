// <copyright file="ShapeRenderer.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using Mitutoyo.MiCAT.ReportModule.PDFRenderer.Helpers;
using Mitutoyo.MiCAT.ReportModule.PDFRenderer.Utilities;

namespace Mitutoyo.MiCAT.ReportModule.PDFRenderer.Renderers.ConcreteRenderers
{
   [ExcludeFromCodeCoverage]
   public class ShapeRenderer : UIElementRendererBase
    {
        public override bool Render(UIElement element, FixedDocumentRenderContext context)
        {
            var shape = element as Shape;
            if (shape == null)
            {
                return false;
            }

            var type = shape.GetType();
            var geometryProperty = type.GetProperty("DefiningGeometry", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            var geometry = (Geometry)geometryProperty.GetValue(shape, null);
            var pdfGeometry = GeometryHelper.ConvertGeometry(geometry);
            if (pdfGeometry == null)
            {
                return false;
            }

            using (context.DrawingSurface.SaveGraphicProperties())
            {
                SetFill(context, shape.Fill, shape.ActualWidth, shape.ActualHeight);
                SetStroke(context, shape.StrokeThickness, shape.Stroke, shape.ActualWidth, shape.ActualHeight, shape.StrokeDashArray);

                if (context.DrawingSurface.GraphicProperties.IsFilled || context.DrawingSurface.GraphicProperties.IsStroked)
                {
                    context.DrawingSurface.DrawPath(pdfGeometry);
                }

                return true;
            }
        }
    }
}
