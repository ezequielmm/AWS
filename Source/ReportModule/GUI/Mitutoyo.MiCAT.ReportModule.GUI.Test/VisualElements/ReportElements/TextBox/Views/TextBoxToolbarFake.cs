// <copyright file="TextBoxToolbarFake.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements.TextBox.Views;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Test.VisualElements.ReportElements.TextBox.Views
{
   [ExcludeFromCodeCoverage]
   public class TextBoxToolbarFake : TextBoxToolbar
   {
      public string FontFamilyToolTipText { get { return ((TextBoxToolbarTooltip)FontFamilyComboBox.ToolTip).Content.ToString(); } }
      public string FontSizeToolTipText { get { return ((TextBoxToolbarTooltip)FontSizeComboBox.ToolTip).Content.ToString(); } }
      public string IncrementFontSizeToolTipText { get { return ((TextBoxToolbarTooltip)ButtonIncrementFontSize.ToolTip).Content.ToString(); } }
      public string DecrementFontSizeToolTipText { get { return ((TextBoxToolbarTooltip)ButtonDecrementFontSize.ToolTip).Content.ToString(); } }
      public string IncrementIndentToolTipText { get { return ((TextBoxToolbarTooltip)ButtonIncrementParagraphLeftIndent.ToolTip).Content.ToString(); } }
      public string DecrementIndentToolTipText { get { return ((TextBoxToolbarTooltip)ButtonDecrementParagraphLeftIndent.ToolTip).Content.ToString(); } }
      public string BoldToolTipText { get { return ((TextBoxToolbarTooltip)ButtonBold.ToolTip).Content.ToString(); } }
      public string ItalicToolTipText { get { return ((TextBoxToolbarTooltip)ButtonItalic.ToolTip).Content.ToString(); } }
      public string UnderlineToolTipText { get { return ((TextBoxToolbarTooltip)ButtonUnderline.ToolTip).Content.ToString(); } }
      public string AlignCenterToolTipText { get { return ((TextBoxToolbarTooltip)ButtonAlignCenter.ToolTip).Content.ToString(); } }
      public string BulletsToolTipText { get { return ((TextBoxToolbarTooltip)ButtonBullets.ToolTip).Content.ToString(); } }
      public string NumberedToolTipText { get { return ((TextBoxToolbarTooltip)ButtonNumbered.ToolTip).Content.ToString(); } }
      public string HighLightColorToolTipText { get { return ((TextBoxToolbarTooltip)HighlightColorPicker.ToolTip).Content.ToString(); } }
      public string FontColorToolTipText { get { return ((TextBoxToolbarTooltip)ForeColorPicker.ToolTip).Content.ToString(); } }

      public bool PopupStaysOpen { get { return popup.StaysOpen; } }
      public bool FontSizeReadOnly { get { return FontSizeComboBox.IsReadOnly; } }
      public bool FontSizeEditable { get { return FontSizeComboBox.IsEditable; } }
      public bool FontSizeTextSearchEnabled { get { return FontSizeComboBox.IsTextSearchEnabled; } }
      public bool FontSizeDropDownOnFocus { get { return FontSizeComboBox.OpenDropDownOnFocus; } }
   }
}
