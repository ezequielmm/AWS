// <copyright file="ZoomContainerScaleModifier.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Mitutoyo.MiCAT.ReportModule.GUI.MainWindow.Zoom
{
   public class ZoomContainerScaleModifier
   {
      public void ModifyScale(FrameworkElement container, ScrollViewer scrollViewer, double oldScale, double newScale)
      {
         Point centerOfViewport = new Point(scrollViewer.ViewportWidth / 2, scrollViewer.ViewportHeight / 2);
         Point viewportPosition = new Point(scrollViewer.HorizontalOffset, scrollViewer.VerticalOffset);

         Point zoomedViewportPosition = GetNewViewportPosition(oldScale, newScale, viewportPosition, centerOfViewport);
         Point scaleCenter = container.TranslatePoint(centerOfViewport, scrollViewer);

         ScaleGrid(container, newScale, scaleCenter);

         container.UpdateLayout();
         scrollViewer.UpdateLayout();

         var scaleDiff = Math.Round(Math.Abs(oldScale - newScale), 1);

         if (viewportPosition.X == 0 && scaleDiff > 0.1)
            scrollViewer.ScrollToHorizontalOffset(scrollViewer.ScrollableWidth / 2);
         else
            scrollViewer.ScrollToHorizontalOffset(zoomedViewportPosition.X);

         scrollViewer.ScrollToVerticalOffset(zoomedViewportPosition.Y);
      }

      public Point GetNewViewportPosition(double oldScale, double newScale, Point viewportPosition, Point centerOfViewport)
      {
         var centerOfCanvas = new Point(centerOfViewport.X + viewportPosition.X, centerOfViewport.Y + viewportPosition.Y);
         var scalingRatio = newScale / oldScale;

         return new Point((scalingRatio * centerOfCanvas.X) - centerOfViewport.X, (scalingRatio * centerOfCanvas.Y) - centerOfViewport.Y);
      }

      private void ScaleGrid(FrameworkElement container, double scale, Point scaleCenter)
      {
         var transformGroup = container.LayoutTransform as TransformGroup;
         ScaleTransform existingScaleTransform = transformGroup.Children.FirstOrDefault() as ScaleTransform;
         SetScaleTransformValues(existingScaleTransform, scale, scaleCenter);
      }

      private void SetScaleTransformValues(ScaleTransform scaleTransform, double scale, Point scaleCenter)
      {
         scaleTransform.ScaleY = scale;
         scaleTransform.ScaleX = scale;
         scaleTransform.CenterX = scaleCenter.X;
         scaleTransform.CenterY = scaleCenter.Y;
      }
   }
}