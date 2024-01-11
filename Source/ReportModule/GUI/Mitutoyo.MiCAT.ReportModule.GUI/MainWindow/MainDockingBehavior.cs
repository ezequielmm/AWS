// <copyright file="MainDockingBehavior.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Windows;
using Mitutoyo.MiCAT.Common.GUI.RadDocking.Controls;

namespace Mitutoyo.MiCAT.ReportModule.GUI.MainWindow
{
   public static class MainDockingBehavior
   {
      public static readonly DependencyProperty IsEnabledProperty =
       DependencyProperty.RegisterAttached(
           "IsEnabled",
           typeof(bool),
           typeof(MainDockingBehavior),
           new PropertyMetadata(false, OnIsEnabledPropertyChanged)
       );

      public static bool GetIsEnabled(DependencyObject obj)
      {
         var val = obj.GetValue(IsEnabledProperty);
         return (bool)val;
      }

      public static void SetIsEnabled(DependencyObject obj, bool value)
      {
         obj.SetValue(IsEnabledProperty, value);
      }

      private static void OnIsEnabledPropertyChanged(DependencyObject dpo, DependencyPropertyChangedEventArgs args)
      {
         if (dpo is RadDockingEx docking)
         {
            var newValue = (bool)args.NewValue;

            if (newValue)
            {
               docking.PreviewShowCompass += OnPreviewShowCompass;
            }
            else
            {
               docking.PreviewShowCompass -= OnPreviewShowCompass;
            }
         }
      }

      private static void OnPreviewShowCompass(object sender, Telerik.Windows.Controls.Docking.PreviewShowCompassEventArgs e)
      {
         e.Compass.IsCenterIndicatorVisible = false;
         e.Compass.IsTopIndicatorVisible = false;
         e.Compass.IsBottomIndicatorVisible = false;

         e.Compass.IsLeftIndicatorVisible = true;
         e.Compass.IsRightIndicatorVisible = true;
      }
   }
}
