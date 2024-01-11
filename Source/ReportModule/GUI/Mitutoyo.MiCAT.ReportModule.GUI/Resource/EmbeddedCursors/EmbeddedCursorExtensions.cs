// <copyright file="EmbeddedCursorExtensions.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Resource.EmbeddedCursors
{
   public static class EmbeddedCursorExtensions
   {
      public static readonly DependencyProperty EmbeddedCursorProperty =
        DependencyProperty.RegisterAttached(
           "EmbeddedCursor",
           typeof(EmbeddedCursorPropertyValue),
           typeof(EmbeddedCursorExtensions),
           new PropertyMetadata(EmbeddedCursorPropertyValue.None, EmbeddedCursorPropertyChanged));

      private static void EmbeddedCursorPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
      {
         var value = (EmbeddedCursorPropertyValue)e.NewValue;
         var cursor = GetFromEnum(value);
         if (obj is FrameworkElement element) element.Cursor = cursor;
      }

      private static readonly Dictionary<EmbeddedCursorPropertyValue, Cursor> PropertyValueToEmbeddedCursorConverter =
         new Dictionary<EmbeddedCursorPropertyValue, Cursor>()
         {
            { EmbeddedCursorPropertyValue.Move, EmbeddedCursors.Cursors.MoveCursor },
            { EmbeddedCursorPropertyValue.BlackResizeNS, EmbeddedCursors.Cursors.BlackResizeNS },
            { EmbeddedCursorPropertyValue.BlackResizeNWSE, EmbeddedCursors.Cursors.BlackResizeNWSE },
            { EmbeddedCursorPropertyValue.BlackResizeNESW, EmbeddedCursors.Cursors.BlackResizeNESW },
            { EmbeddedCursorPropertyValue.BlackResizeWE, EmbeddedCursors.Cursors.BlackResizeWE }
         };

      public static EmbeddedCursorPropertyValue GetEmbeddedCursor(DependencyObject obj)
      {
         if (obj == null) throw new ArgumentNullException();

         return (EmbeddedCursorPropertyValue)obj.GetValue(EmbeddedCursorProperty);
      }

      public static void SetEmbeddedCursor(DependencyObject obj, EmbeddedCursorPropertyValue value)
      {
         if (obj == null) throw new ArgumentNullException();

         obj.SetValue(EmbeddedCursorProperty, value);
      }

      public enum EmbeddedCursorPropertyValue
      {
         None,
         Move,
         BlackResizeNS,
         BlackResizeNWSE,
         BlackResizeNESW,
         BlackResizeWE
      }

      private static Cursor GetFromEnum(EmbeddedCursorPropertyValue value)
      {
         Cursor cursor;
         PropertyValueToEmbeddedCursorConverter.TryGetValue(value, out cursor);
         return cursor;
      }
   }
}
