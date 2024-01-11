// <copyright file="CloseRadDropDownButtonOnRadContextMenuItemClickBehavior.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Windows;
using Telerik.Windows.Controls;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Common
{
   public class CloseRadDropDownButtonOnRadContextMenuItemClickBehavior
   {
      public static readonly DependencyProperty RadContextMenuProperty;

      static CloseRadDropDownButtonOnRadContextMenuItemClickBehavior()
      {
         RadContextMenuProperty = DependencyProperty.RegisterAttached(
           "RadContextMenu",
           typeof(RadContextMenu),
           typeof(CloseRadDropDownButtonOnRadContextMenuItemClickBehavior),
           new PropertyMetadata(OnTargetPropertyChanged));
      }

      public static RadContextMenu GetRadContextMenu(
         DependencyObject obj)
      {
         var val = obj.GetValue(RadContextMenuProperty);
         return (RadContextMenu)val;
      }

      public static void SetRadContextMenu(
         DependencyObject obj,
         RadContextMenu value)
      {
         obj.SetValue(RadContextMenuProperty, value);
      }

      private static void OnTargetPropertyChanged(
         DependencyObject dpo,
         DependencyPropertyChangedEventArgs args)
      {
         if (dpo is RadDropDownButton radDropDownButton)
            RegisterEventHandlers(radDropDownButton);
      }

      private static void RegisterEventHandlers(
         RadDropDownButton radDropDownButton)
      {
         var radContextMenu = GetRadContextMenu(radDropDownButton);
         radContextMenu.ItemClick += (s, args) => OnRadContextMenuItemClicked(radDropDownButton);
      }

      private static void OnRadContextMenuItemClicked(
         RadDropDownButton radDropDownButton)
      {
         radDropDownButton.IsOpen = false;
      }
   }
}
