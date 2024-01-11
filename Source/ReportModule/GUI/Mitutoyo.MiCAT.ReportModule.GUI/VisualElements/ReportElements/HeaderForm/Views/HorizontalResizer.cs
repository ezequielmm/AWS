// <copyright file="HorizontalResizer.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using System.Windows;
using System.Windows.Controls.Primitives;
using Mitutoyo.MiCAT.ReportModule.GUI.Resource.EmbeddedCursors;

namespace Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements.HeaderForm.Views
{
   public class HorizontalResizer : Thumb
   {
      public static readonly DependencyProperty WidthLengthProperty =
        DependencyProperty.Register(
           nameof(WidthLength),
           typeof(double),
           typeof(HorizontalResizer),
           new PropertyMetadata(0.0));

      public static readonly DependencyProperty WidthPercentageProperty =
         DependencyProperty.Register(
            nameof(WidthPercentage),
            typeof(double),
            typeof(HorizontalResizer),
            new PropertyMetadata(0.0, OnPropertyChanged));

      public static readonly DependencyProperty MinLengthProperty =
        DependencyProperty.Register(
           nameof(MinLength),
           typeof(double?),
           typeof(HorizontalResizer),
           new PropertyMetadata(null, OnPropertyChanged));

      public static readonly DependencyProperty MaxLengthProperty =
        DependencyProperty.Register(
           nameof(MaxLength),
           typeof(double),
           typeof(HorizontalResizer),
           new PropertyMetadata(double.MaxValue, OnPropertyChanged));

      private static void OnPropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs args)
      {
         var resizer = (HorizontalResizer)source;
         resizer.UpdateWidthLength();
      }

      public HorizontalResizer()
      {
         DragDelta += OnDragDelta;
         Cursor = Cursors.BlackResizeWE;
         UpdateWidthLength();
      }

      public double WidthLength
      {
         get => GetValue(WidthLengthProperty) as double? ?? 0;
         set => SetValue(WidthLengthProperty, value);
      }

      public double WidthPercentage
      {
         get => GetValue(WidthPercentageProperty) as double? ?? 0;
         set => SetValue(WidthPercentageProperty, value);
      }

      public double? MinLength
      {
         get => GetValue(MinLengthProperty) as double?;
         set => SetValue(MinLengthProperty, value);
      }

      public double MaxLength
      {
         get => GetValue(MaxLengthProperty) as double? ?? double.MaxValue;
         set => SetValue(MaxLengthProperty, value);
      }

      private void OnDragDelta(object sender, DragDeltaEventArgs args)
      {
         var newLength = WidthLength + args.HorizontalChange;
         newLength = Math.Max(newLength, (MinLength ?? 0 + ActualWidth));
         newLength = Math.Min(newLength, MaxLength);

         var newPercentage = (newLength - MinLength ?? 0) * 100 / (MaxLength - MinLength ?? 0);

         WidthLength = newLength;
         WidthPercentage = newPercentage;
      }

      private void UpdateWidthLength()
      {
         WidthLength = CalculateWidthLength();
      }

      private double CalculateWidthLength()
      {
         var newWidth = (MinLength ?? 0) + (WidthPercentage * (MaxLength - MinLength ?? 0) / 100);
         newWidth = Math.Max(newWidth, MinLength ?? 0 + ActualWidth);
         newWidth = Math.Min(newWidth, MaxLength);

         return newWidth;
      }
   }
}
