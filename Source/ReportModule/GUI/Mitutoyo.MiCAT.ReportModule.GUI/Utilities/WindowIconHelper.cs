// <copyright file="WindowIconHelper.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Utilities
{
   public static class WindowIconHelper
   {
      public static readonly DependencyProperty ShowIconProperty =
         DependencyProperty.RegisterAttached(
            "ShowIcon",
            typeof(bool),
            typeof(WindowIconHelper),
            new FrameworkPropertyMetadata(true, new PropertyChangedCallback((d, e) => HideIcon((Window) d))));

      /// <summary>
      /// Hides icon for window.
      /// Called after InitializeComponent() then an empty image is used but there will be empty space between window border and title
      /// </summary>
      /// <param name="window">Window class</param>
      public static void HideIcon(this Window window)
      {
         window.SourceInitialized += delegate
         {
            window.Icon = BitmapSource.Create(1, 1, 96, 96, PixelFormats.Bgra32, null, new byte[] { 0, 0, 0, 0 }, 4);
         };
      }

      public static bool GetShowIcon(UIElement element)
      {
         return (bool)element.GetValue(ShowIconProperty);
      }

      public static void SetShowIcon(UIElement element, Boolean value)
      {
         element.SetValue(ShowIconProperty, value);
      }
   }
}
