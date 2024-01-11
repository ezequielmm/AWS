// <copyright file="ZoomBehavior.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;
using Telerik.Windows.Controls;

namespace Mitutoyo.MiCAT.ReportModule.GUI.MainWindow.Zoom
{
   [ExcludeFromCodeCoverage]
   public class ZoomBehavior : Behavior<CarouselScrollViewer>
   {
      private ZoomContainerScaleModifier zoomScaleModifier = new ZoomContainerScaleModifier();
      private static Dictionary<ScrollViewer, ZoomBehavior> _behaviorsForScrollViewContent;
      public ScrollViewer ScrollViewer { get; set; }
      public FrameworkElement Container { get; set; } = null;

      static ZoomBehavior()
      {
         _behaviorsForScrollViewContent = new Dictionary<ScrollViewer, ZoomBehavior>();
      }

      public static readonly DependencyProperty IsEnabledProperty =
         DependencyProperty.RegisterAttached("IsEnabled", typeof(bool), typeof(ZoomBehavior),
            new PropertyMetadata(false, new PropertyChangedCallback(OnIsEnabledPropertyChanged)));

      public static void OnIsEnabledPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
      {
         SetIsEnabled(dependencyObject, (bool)e.NewValue);
      }

      public static void SetIsEnabled(DependencyObject obj, bool value)
      {
         var scrollViewer = obj as ScrollViewer;
         if (value)
            AddBehaviorToScrollViewContent(scrollViewer);
         else
            RemoveAndDettachBehavior(scrollViewer);
      }

      public static readonly DependencyProperty ContainerProperty =
         DependencyProperty.RegisterAttached("Container", typeof(FrameworkElement), typeof(ZoomBehavior),
         new PropertyMetadata());

      public static void SetContainer(ScrollViewer scrollViewer, FrameworkElement container)
      {
         scrollViewer.SetValue(ContainerProperty, container);
      }

      public static FrameworkElement GetContainer(ScrollViewer scrollViewer)
      {
         return (FrameworkElement)scrollViewer.GetValue(ContainerProperty);
      }

      public static readonly DependencyProperty ZoomScaleProperty =
            DependencyProperty.RegisterAttached("ZoomScale", typeof(double), typeof(ZoomBehavior),
            new PropertyMetadata(default(double), new PropertyChangedCallback(OnZoomScalePropertyChanged)));

      public static void SetZoomScale(FrameworkElement element, double value)
      {
         element.SetValue(ZoomScaleProperty, value);
      }

      public static double GetZoomScale(FrameworkElement element)
      {
         return (double)element.GetValue(ZoomScaleProperty);
      }

      private static void OnZoomScalePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
      {
         var scrollViewer = d as ScrollViewer;
         if (_behaviorsForScrollViewContent.ContainsKey(scrollViewer))
            GetAttachedBehavior(scrollViewer).UpdateScale((double)e.OldValue, (double)e.NewValue);
      }

      private static ZoomBehavior GetAttachedBehavior(ScrollViewer scrollViewer)
      {
         return _behaviorsForScrollViewContent[scrollViewer];
      }

      private static void RemoveAndDettachBehavior(ScrollViewer scrollViewer)
      {
         _behaviorsForScrollViewContent.Remove(scrollViewer);
      }

      private static ZoomBehavior AddBehaviorToScrollViewContent(ScrollViewer scrollViewer)
      {
         var newBehavior = new ZoomBehavior();

         newBehavior.ScrollViewer = scrollViewer;
         newBehavior.Container = (FrameworkElement)scrollViewer.GetValue(ZoomBehavior.ContainerProperty);

         _behaviorsForScrollViewContent[scrollViewer] = newBehavior;

         return newBehavior;
      }

      private void UpdateScale(double oldScale, double newScale)
      {
         zoomScaleModifier.ModifyScale(Container, ScrollViewer, oldScale, newScale);
      }
   }
}