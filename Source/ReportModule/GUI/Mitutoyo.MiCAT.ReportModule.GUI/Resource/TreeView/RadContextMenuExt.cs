// <copyright file="RadContextMenuExt.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Windows;
using Telerik.Windows.Controls;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Resource.TreeView
{
   public class RadContextMenuExt : RadContextMenu
   {
      public static readonly DependencyProperty IsRadContextMenuOpenProperty =
         DependencyProperty.RegisterAttached("IsRadContextMenuOpen",
         typeof(bool),
         typeof(RadContextMenuExt),
         new PropertyMetadata(false));

      public static bool GetIsRadContextMenuOpen(DependencyObject obj)
      {
         return (bool)obj.GetValue(IsRadContextMenuOpenProperty);
      }

      public static void SetIsRadContextMenuOpen(DependencyObject obj, bool value)
      {
         obj.SetValue(IsRadContextMenuOpenProperty, value);
      }

      public RadContextMenuExt()
      {
         Opening += OnOpened;
         Closed += OnClosed;
      }

      private void OnOpened(object source, RoutedEventArgs e)
      {
         UpdateIsRadContextMenuOpen();
      }

      private void OnClosed(object source, RoutedEventArgs e)
      {
         UpdateIsRadContextMenuOpen();
      }

      private void UpdateIsRadContextMenuOpen()
      {
         if (UIElement is FrameworkElement element)
         {
            SetIsRadContextMenuOpen(element, IsOpen);
         }
      }
   }
}
