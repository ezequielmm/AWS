// <copyright file="MathHelper.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Media;

namespace Mitutoyo.MiCAT.ReportModule.PDFRenderer.Helpers
{
   [ExcludeFromCodeCoverage]
   public static class MathHelper
    {
        public static Matrix Multiply(Matrix m1, Matrix m2)
        {
            return new Matrix(
                (m1.M11 * m2.M11) + (m1.M12 * m2.M21), (m1.M11 * m2.M12) + (m1.M12 * m2.M22),
                (m1.M21 * m2.M11) + (m1.M22 * m2.M21), (m1.M21 * m2.M12) + (m1.M22 * m2.M22),
                (m1.OffsetX * m2.M11) + (m1.OffsetY * m2.M21) + m2.OffsetX, (m1.OffsetX * m2.M12) + (m1.OffsetY * m2.M22) + m2.OffsetY);
        }

        public static Matrix CreateMatrix(GeneralTransform transform)
        {
            if (transform != null && !IsIdentity(transform))
            {
                Point offset = transform.Transform(new Point(0, 0));
                Point i = transform.Transform(new Point(1, 0)).Minus(offset);
                Point j = transform.Transform(new Point(0, 1)).Minus(offset);
                Matrix matrix = new Matrix(i.X, i.Y, j.X, j.Y, offset.X, offset.Y);
                return matrix;
            }
            else
            {
                return Matrix.Identity;
            }
        }

        public static GeneralTransform GetGeneralTransform(FrameworkElement element, UIElement positionRelativeElement)
        {
            GeneralTransform transform;
            Visual parent;

            if (positionRelativeElement != null)
               parent = positionRelativeElement;
            else
               parent = VisualTreeHelper.GetParent(element) as Visual;

            if (parent != null)
            {
                transform = element.TransformToVisual(parent);
            }
            else
            {
                TransformGroup transformGroup = new TransformGroup();
                if (element.LayoutTransform != null)
                {
                    transformGroup.Children.Add(element.LayoutTransform);
                }
                if (element.RenderTransform != null)
                {
                    transformGroup.Children.Add(element.RenderTransform);
                }
                transform = transformGroup;
            }
            return transform;
        }

        public static bool IsIdentity(GeneralTransform transform)
        {
            var matrixTransform = transform as MatrixTransform;
            if (matrixTransform != null)
            {
                return matrixTransform.Matrix.IsIdentity;
            }

            var translateTransform = transform as TranslateTransform;
            if (translateTransform != null)
            {
                return translateTransform.X == 0 && translateTransform.Y == 0;
            }

            var rotateTransform = transform as RotateTransform;
            if (rotateTransform != null)
            {
                return rotateTransform.Angle == 0;
            }

            return false;
        }

        public static PathGeometry TransformRectangle(Matrix matrix, Rect rectangle)
        {
            Point topLeft = matrix.Transform(new Point(rectangle.Left, rectangle.Top));
            Point topRight = matrix.Transform(new Point(rectangle.Right, rectangle.Top));
            Point bottomRight = matrix.Transform(new Point(rectangle.Right, rectangle.Bottom));
            Point bottomLeft = matrix.Transform(new Point(rectangle.Left, rectangle.Bottom));

            var path = new PathGeometry();
            PathFigure figure = new PathFigure();
            path.Figures.Add(figure);
            figure.IsClosed = true;

            figure.StartPoint = topLeft;
            figure.Segments.Add(new LineSegment(topRight, false));
            figure.Segments.Add(new LineSegment(bottomRight, false));
            figure.Segments.Add(new LineSegment(bottomLeft, false));
            return path;
        }

        private static Point Minus(this Point first, Point second)
        {
            return new Point(first.X - second.X, first.Y - second.Y);
        }
    }
}