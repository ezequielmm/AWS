// <copyright file="CloseOnClickBehavior.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Controls;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Common
{
   [ExcludeFromCodeCoverage]
   public static class CloseOnClickBehavior
   {
      public static readonly DependencyProperty IsEnabledProperty =
       DependencyProperty.RegisterAttached(
           "IsEnabled",
           typeof(bool),
           typeof(CloseOnClickBehavior),
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
         if (dpo is Button button)
         {
            var newValue = (bool)args.NewValue;

            if (newValue)
            {
               button.Click += OnClick;
            }
            else
            {
               button.Click -= OnClick;
            }
         }
      }

      private static void OnClick(object sender, RoutedEventArgs e)
      {
         if (sender is Button button)
         {
            var window = Window.GetWindow(button);
            window?.Close();
         }
      }
   }
}
