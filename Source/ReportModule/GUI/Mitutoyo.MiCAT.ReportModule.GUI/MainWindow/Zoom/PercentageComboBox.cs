// <copyright file="PercentageComboBox.cs" company="Mitutoyo Europe GmbH">
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
using System.Windows.Media;
using Telerik.Windows.Controls;

namespace Mitutoyo.MiCAT.ReportModule.GUI.MainWindow.Zoom
{
   [ExcludeFromCodeCoverage]
   public class PercentageComboBox : RadComboBox
   {
      private const string EDITABLE_TEXTBOX_PARTNAME = "PART_EditableTextBox";
      private const string DISPLAY_FORMAT = "{0}%";

      private TextBox EditableTextBox { get; set; }
      private PercentageInputValidator zoomInputValidator;

      public PercentageComboBox()
      {
         zoomInputValidator = new PercentageInputValidator();
      }

      public static readonly DependencyProperty PercentValueProperty =
       DependencyProperty.Register(
       "PercentValue", typeof(double),
       typeof(PercentageComboBox),
       new PropertyMetadata(0d, new PropertyChangedCallback(ChangePercentValue))
       );

      public static readonly DependencyProperty SelectionBrushProperty =
       DependencyProperty.Register(
       "SelectionBrush", typeof(SolidColorBrush),
       typeof(PercentageComboBox),
       new PropertyMetadata(null, new PropertyChangedCallback(ChangeSelectionBrushValue))
       );

      protected override void OnMouseDoubleClick(MouseButtonEventArgs e)
      {
         EditableTextBox.SelectAll();
         e.Handled = true;
         base.OnMouseDoubleClick(e);
      }

      public override void OnApplyTemplate()
      {
         base.OnApplyTemplate();
         EditableTextBox = (TextBox)GetTemplateChild(EDITABLE_TEXTBOX_PARTNAME);
         EditableTextBox.MaxLength = 4;
         EditableTextBox.SelectionBrush = SelectionBrush;
         EditableTextBox.SelectionOpacity = 0.5;
         CommandManager.AddPreviewCanExecuteHandler(EditableTextBox, EditableTextBoxHandleCanExecute);
      }

      public double PercentValue
      {
         get { return (double)GetValue(PercentValueProperty); }
         set { SetValue(PercentValueProperty, value); }
      }

      public SolidColorBrush SelectionBrush
      {
         get { return (SolidColorBrush)GetValue(SelectionBrushProperty); }
         set
         {
            SetValue(SelectionBrushProperty, value);
            if (EditableTextBox != null)
               EditableTextBox.SelectionBrush = value;
         }
      }

      private static void ChangeSelectionBrushValue(DependencyObject d, DependencyPropertyChangedEventArgs e)
      {
         SolidColorBrush newValue = (SolidColorBrush)e.NewValue;
         (d as PercentageComboBox).SelectionBrush = newValue;
      }

      private static void ChangePercentValue(DependencyObject d, DependencyPropertyChangedEventArgs e)
      {
         double newValue = (double)e.NewValue;
         (d as PercentageComboBox).SetFormattedTextForPercentValue(newValue);
      }

      public void SetFormattedTextForPercentValue(double percentValue)
      {
         Text = string.Format(DISPLAY_FORMAT, Math.Truncate(percentValue * 100));
      }

      private void EditableTextBoxHandleCanExecute(object sender, CanExecuteRoutedEventArgs e)
      {
         if (e.Command == ApplicationCommands.Paste)
         {
            e.CanExecute = false;
            e.Handled = true;
         }
      }

      protected override void OnPreviewTextInput(TextCompositionEventArgs e)
      {
         if (e.Text == "\r")
         {
            var result = zoomInputValidator.ValidateInput(Text);

            if (result.IsValid)
               PercentValue = result.PercentValue;

            SetFormattedTextForPercentValue(PercentValue);
         }
         else
         {
            e.Handled = !zoomInputValidator.IsAllowedCharacter(e.Text);
         }

         base.OnPreviewTextInput(e);
      }
   }
}
