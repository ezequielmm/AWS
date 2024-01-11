// <copyright file="ReportTextBoxControl.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Telerik.Windows.Controls;
using Telerik.Windows.Documents;

namespace Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements.TextBox.Views
{
   [ExcludeFromCodeCoverage]
   public class ReportTextBoxControl : RadRichTextBox
   {
      private const int SIZE_OFFSET_TO_DISPLAY_ELLIPSIS = 21;

      private readonly Key[] _disabledShortcutKeys = { Key.C, Key.V, Key.X, Key.D, Key.F, Key.K, Key.N, Key.O, Key.P, Key.S, Key.OemPlus, Key.Add, Key.J, Key.R, Key.L, Key.E };

      public static readonly DependencyProperty IsSelectedProperty =
         DependencyProperty.Register(
            nameof(IsSelected),
            typeof(bool),
            typeof(ReportTextBoxControl),
            null);

      public static readonly DependencyProperty EllipsisVisibleProperty =
         DependencyProperty.Register(
            nameof(EllipsisVisible),
            typeof(bool),
            typeof(ReportTextBoxControl),
            null);

      public ReportTextBoxControl()
      {
         IsSelected = false;
         EllipsisVisible = false;

         TypeDescriptor.GetProperties(this)[nameof(IsSelected)].AddValueChanged(this, OnSelectionChange);
         Unloaded += ReportTextBoxControl_Unloaded;
      }

      protected override void OnPreviewEditorKeyDown(PreviewEditorKeyEventArgs e)
      {
         if ((Keyboard.Modifiers.HasFlag(ModifierKeys.Control) && _disabledShortcutKeys.Contains(e.Key)) ||
             (Keyboard.Modifiers.HasFlag(ModifierKeys.Control) && Keyboard.Modifiers.HasFlag(ModifierKeys.Shift) && (e.Key == Key.OemPlus || e.Key == Key.Add)))
         {
            e.SuppressDefaultAction = true;
            e.OriginalArgs.Handled = true;
         }
         else
         {
            base.OnPreviewEditorKeyDown(e);
         }
      }

      public bool EllipsisVisible
      {
         get { return (bool)GetValue(EllipsisVisibleProperty); }
         set { SetValue(EllipsisVisibleProperty, value); }
      }

      public bool IsSelected
      {
         get { return (bool)GetValue(IsSelectedProperty); }
         set { SetValue(IsSelectedProperty, value); }
      }

      protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
      {
         ScrollUp();
         base.OnRenderSizeChanged(sizeInfo);

         UpdateContentFittingValue();
      }

      protected override void OnDocumentChanged()
      {
         base.OnDocumentChanged();

         UpdateContentFittingValue();
      }

      private void ScrollUp()
      {
         this.ScrollToVerticalOffset(0.0);
      }

      protected override void OnDocumentContentChanged()
      {
         base.OnDocumentContentChanged();

         UpdateContentFittingValue();
      }

      private void OnSelectionChange(object sender, EventArgs e)
      {
         ScrollUp();
         ClearSelectionIfUnselected();
      }
      private void ClearSelectionIfUnselected()
      {
         if (!IsSelected)
            Document.Selection.Clear();
      }

      private void UpdateContentFittingValue()
      {
         EllipsisVisible = (RenderSize.Width < Document.DesiredSize.Width - SIZE_OFFSET_TO_DISPLAY_ELLIPSIS || RenderSize.Height < Document.DesiredSize.Height - SIZE_OFFSET_TO_DISPLAY_ELLIPSIS);
      }

      private void ReportTextBoxControl_Unloaded(object sender, RoutedEventArgs e)
      {
         TypeDescriptor.GetProperties(this)[nameof(IsSelected)].RemoveValueChanged(this, OnSelectionChange);
      }
   }
}
