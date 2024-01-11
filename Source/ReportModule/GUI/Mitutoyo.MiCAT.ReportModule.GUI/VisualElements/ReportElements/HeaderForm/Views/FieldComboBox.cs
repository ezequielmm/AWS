// <copyright file="FieldComboBox.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements.HeaderForm.Views
{
   [ExcludeFromCodeCoverage]
   public class FieldComboBox : ComboBox
   {
      private const string EditableTextBoxPartName = "PART_EditableTextBox";
      private const string EditButtonName = "EditButton";

      private System.Windows.Controls.TextBox EditableTextBox { get; set; }
      private Button EditButton { get; set; }

      public event EventHandler EditableTextBoxGotFocus;
      public event EventHandler EditableTextBoxPreviewLostKeyboardFocus;
      public event EventHandler EditableTextBoxLostFocus;

      public override void OnApplyTemplate()
      {
         base.OnApplyTemplate();

         EditableTextBox = (System.Windows.Controls.TextBox)GetTemplateChild(EditableTextBoxPartName);
         EditButton = (Button)GetTemplateChild(EditButtonName);

         EditableTextBox.GotFocus += OnEditableTextBoxGotFocus;
         EditableTextBox.PreviewLostKeyboardFocus += OnEditableTextBoxPreviewLostKeyboardFocus;
         EditableTextBox.LostFocus += OnEditableTextBoxLostFocus;

         EditButton.Click += OnEditButtonClicked;
      }

      protected override void OnPreviewMouseWheel(MouseWheelEventArgs e)
      {
         if (!IsDropDownOpen)
         {
            var parentElement = Parent as UIElement;
            if (Parent == null)
               return;

            parentElement.RaiseEvent(
               new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta)
               {
                  RoutedEvent = UIElement.MouseWheelEvent
               });

            e.Handled = true;
         }

         base.OnPreviewMouseWheel(e);
      }

      private void OnEditableTextBoxGotFocus(object sender, RoutedEventArgs e)
      {
         EditableTextBoxGotFocus?.Invoke(sender, e);
      }

      private void OnEditableTextBoxPreviewLostKeyboardFocus(object sender, RoutedEventArgs e)
      {
         EditableTextBoxPreviewLostKeyboardFocus?.Invoke(this, e);
      }

      private void OnEditableTextBoxLostFocus(object sender, RoutedEventArgs e)
      {
         EditableTextBoxLostFocus?.Invoke(sender, e);
      }

      private void OnEditButtonClicked(object sender, RoutedEventArgs e)
      {
         Dispatcher.BeginInvoke(new Action(() => EditableTextBox.Focus()));
      }
   }
}
