// <copyright file="BrushHelper.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Linq;
using System.Windows;
using System.Windows.Media;
using Telerik.Windows.Documents.Fixed.Model.Data;
using TelerikColor = Telerik.Windows.Documents.Fixed.Model.ColorSpaces;

namespace Mitutoyo.MiCAT.ReportModule.PDFRenderer.Helpers
{
   public static class BrushHelper
   {
      public static TelerikColor.ColorBase ConvertBrush(Brush brush, double opacity, IPosition position, double width, double height)
      {
         SolidColorBrush solidBrush = brush as SolidColorBrush;
         if (solidBrush != null)
         {
            return ConvertSolidColorBrush(solidBrush, opacity);
         }

         LinearGradientBrush linearGradientBrush = brush as LinearGradientBrush;
         if (linearGradientBrush != null)
         {
            return ConvertLinearGradientBrush(linearGradientBrush, opacity, position, width, height);
         }

         RadialGradientBrush radialGradientBrush = brush as RadialGradientBrush;
         if (radialGradientBrush != null)
         {
            return ConvertRadialGradientBrush(radialGradientBrush, opacity, position, width, height);
         }

         return null;
      }

      public static TelerikColor.RgbColor ConvertSolidColorBrush(SolidColorBrush brush, double opacity)
      {
         return new TelerikColor.RgbColor((byte)(brush.Color.A * opacity), brush.Color.R, brush.Color.G, brush.Color.B);
      }

      public static TelerikColor.LinearGradient ConvertLinearGradientBrush(LinearGradientBrush brush, double opacity, IPosition position, double width, double height)
      {
         Point startPoint = new Point(brush.StartPoint.X * width, brush.StartPoint.Y * height);
         Point endPoint = new Point(brush.EndPoint.X * width, brush.EndPoint.Y * height);

         var pdfGradient = new TelerikColor.LinearGradient(startPoint, endPoint);
         pdfGradient.Position = position;

         foreach (GradientStop gradientStop in brush.GradientStops.OrderBy(x => x.Offset))
         {
            var rgbColor = new TelerikColor.RgbColor((byte)(gradientStop.Color.A * opacity), gradientStop.Color.R, gradientStop.Color.G, gradientStop.Color.B);
            pdfGradient.GradientStops.Add(new TelerikColor.GradientStop(rgbColor, gradientStop.Offset));
         }

         return pdfGradient;
      }

      public static TelerikColor.RadialGradient ConvertRadialGradientBrush(RadialGradientBrush brush, double opacity, IPosition position, double width, double height)
      {
         if (width * height == 0)
         {
            return null;
         }

         var center1 = new Point(brush.GradientOrigin.X * height, brush.GradientOrigin.Y * height);
         var center2 = new Point(brush.Center.X * height, brush.Center.Y * height);
         TelerikColor.RadialGradient pdfGradient = new TelerikColor.RadialGradient(center1, center2, 0, height / 2);

         var matrix = new Matrix(width / height, 0, 0, 1, 0, 0);
         var newMatrix = MathHelper.Multiply(position.Matrix, matrix);
         pdfGradient.Position = new Telerik.Windows.Documents.Fixed.Model.Data.MatrixPosition(newMatrix);

         foreach (var gradientStop in brush.GradientStops)
         {
            var rgbColor = new TelerikColor.RgbColor((byte)(gradientStop.Color.A * opacity), gradientStop.Color.R, gradientStop.Color.G, gradientStop.Color.B);
            pdfGradient.GradientStops.Add(new TelerikColor.GradientStop(rgbColor, gradientStop.Offset));
         }

         return pdfGradient;
      }
      public static bool SolidBrushEquals(Brush brush1, Brush brush2)
      {
         if ((brush1 is SolidColorBrush solid1) && (brush2 is SolidColorBrush solid2))
            return ((solid1.Color == solid2.Color) && (solid1.Opacity == solid2.Opacity));
         else
            return false;
      }
   }
}