// <copyright file="UnselectionArea.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Shapes;
using Mitutoyo.MiCAT.ReportModule.GUI.PageLayout;
using Telerik.Windows.Controls;

namespace Mitutoyo.MiCAT.ReportModule.GUI.MainWindow
{
   public class UnselectionArea : IUnselectionArea
   {
      public bool CheckIsSelected(object originalSource, object source)
      {
         return
            (CheckIsReportBoundarySectionComponentsContainer(originalSource, source) ||
            CheckIsPageCanvasContainer(originalSource, source) ||
            CheckIsBetweenPages(originalSource, source) ||
            CheckIsGrayedArea(originalSource, source));
      }

      private bool CheckIsGrayedArea(object originalSource, object source)
      {
         return source is CarouselScrollViewer &&
            !(originalSource is Ellipse) &&
            !CheckIsScrollbar(originalSource);
      }

      private bool CheckIsReportBoundarySectionComponentsContainer(object originalSource, object source)
      {
         return originalSource is Canvas canvas && canvas.Name == "ReportBoundarySectionComponentsContainer" && source is PagesView;
      }

      private bool CheckIsPageCanvasContainer(object originalSource, object source)
      {
         return originalSource is Canvas canvas && canvas.Name == "PageCanvasContainer" && source is PagesView;
      }

      private bool CheckIsBetweenPages(object originalSource, object source)
      {
         return (originalSource is FrameworkElement frameworkElement) && frameworkElement.DataContext is IVMPages && source is PagesView;
      }

      private bool CheckIsScrollbar(object originalSource)
      {
         return originalSource is Rectangle rectangle && rectangle.TemplatedParent is Thumb thumb && thumb.TemplatedParent is ScrollBar;
      }
   }
}
