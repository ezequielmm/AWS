// <copyright file="TextBoxBehavior.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Dialog.SaveAs
{
   [ExcludeFromCodeCoverage]
   public class TextBoxBehavior : Behavior<TextBox>
   {
      public static readonly DependencyProperty TripleClickSelectAllProperty = DependencyProperty.RegisterAttached(
         "TripleClickSelectAll",
         typeof(bool),
         typeof(TextBoxBehavior),
         new PropertyMetadata(false, OnPropertyChanged));

      private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
      {
         var tb = d as TextBox;
         if (tb != null)
         {
            var enable = (bool)e.NewValue;
            if (enable)
            {
               tb.PreviewMouseLeftButtonDown += OnTextBoxMouseDown;
            }
            else
            {
               tb.PreviewMouseLeftButtonDown -= OnTextBoxMouseDown;
            }
         }
      }

      private static void OnTextBoxMouseDown(object sender, MouseButtonEventArgs e)
      {
         if (e.ClickCount == 3)
         {
            ((TextBox)sender).SelectAll();
         }
      }

      public static void SetTripleClickSelectAll(DependencyObject element, bool value)
      {
         element.SetValue(TripleClickSelectAllProperty, value);
      }
   }
}