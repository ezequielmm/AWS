// <copyright file="TextBoxToolbar.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>
using System.Windows;
using Mitutoyo.MiCAT.ReportModule.GUI.Strings;
using Telerik.Windows.Controls.RichTextBoxUI;

namespace Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements.TextBox.Views
{
   public class TextBoxToolbar : SelectionMiniToolBar
   {
      public TextBoxToolbar()
      {
         Loaded += TextBoxToolbar_Loaded;
      }

      private void TextBoxToolbar_Loaded(object sender, RoutedEventArgs e)
      {
         FontFamilyComboBox.ToolTip = new TextBoxToolbarTooltip(StringFinder.FindLocalizedString("TextBoxToolbarFontFamilyToolTip"));
         FontSizeComboBox.ToolTip = new TextBoxToolbarTooltip(StringFinder.FindLocalizedString("TextBoxToolbarFontSizeToolTip"));
         ButtonIncrementFontSize.ToolTip = new TextBoxToolbarTooltip(StringFinder.FindLocalizedString("TextBoxToolbarIncreaseFontSizeToolTip"));
         ButtonDecrementFontSize.ToolTip = new TextBoxToolbarTooltip(StringFinder.FindLocalizedString("TextBoxToolbarDecreaseFontSizeToolTip"));
         ButtonIncrementParagraphLeftIndent.ToolTip = new TextBoxToolbarTooltip(StringFinder.FindLocalizedString("TextBoxToolbarIncreaseLeftIndentToolTip"));
         ButtonDecrementParagraphLeftIndent.ToolTip = new TextBoxToolbarTooltip(StringFinder.FindLocalizedString("TextBoxToolbarDecreaseLeftIndentToolTip"));
         ButtonBold.ToolTip = new TextBoxToolbarTooltip(StringFinder.FindLocalizedString("TextBoxToolbarBoldToolTip"));
         ButtonItalic.ToolTip = new TextBoxToolbarTooltip(StringFinder.FindLocalizedString("TextBoxToolbarItalicToolTip"));
         ButtonUnderline.ToolTip = new TextBoxToolbarTooltip(StringFinder.FindLocalizedString("TextBoxToolbarUnderlineToolTip"));
         ButtonAlignCenter.ToolTip = new TextBoxToolbarTooltip(StringFinder.FindLocalizedString("TextBoxToolbarAlignCenterToolTip"));
         ButtonBullets.ToolTip = new TextBoxToolbarTooltip(StringFinder.FindLocalizedString("TextBoxToolbarBulletsToolTip"));
         ButtonNumbered.ToolTip = new TextBoxToolbarTooltip(StringFinder.FindLocalizedString("TextBoxToolbarNumberedToolTip"));
         HighlightColorPicker.ToolTip = new TextBoxToolbarTooltip(StringFinder.FindLocalizedString("TextBoxToolbarHighlightColorToolTip"));
         ForeColorPicker.ToolTip = new TextBoxToolbarTooltip(StringFinder.FindLocalizedString("TextBoxToolbarForeColorToolTip"));

         popup.StaysOpen = false;

         FontSizeComboBox.IsReadOnly = true;
         FontSizeComboBox.IsTextSearchEnabled = false;
         FontSizeComboBox.OpenDropDownOnFocus = true;

         FontSizeComboBox.UpdateSelectionOnLostFocus = false;
         FontSizeComboBox.IsEditable = true;
         FontSizeComboBox.AllowMultipleSelection = false;
         FontSizeComboBox.CanAutocompleteSelectItems = false;

         ButtonBullets.Visibility = Visibility.Collapsed;
         ButtonNumbered.Visibility = Visibility.Collapsed;
      }

      // REFACTOR: We couldn't find a better way to disable the Fading behavior from the TextBox Toolbar yet.
      // This property will be removed when we can find a way to improve this fix.
      protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
      {
         base.OnPropertyChanged(e);

         if (e.Property.Name == nameof(Opacity))
            Opacity = 1.0;
      }
   }
}
