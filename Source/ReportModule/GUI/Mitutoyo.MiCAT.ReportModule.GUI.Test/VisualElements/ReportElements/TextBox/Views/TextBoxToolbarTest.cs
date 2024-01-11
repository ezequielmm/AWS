// <copyright file="TextBoxToolbarTest.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Windows;
using Mitutoyo.MiCAT.ReportModule.GUI.Strings;
using NUnit.Framework;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Test.VisualElements.ReportElements.TextBox.Views
{
   [TestFixture]
   [Apartment(ApartmentState.STA)]
   [ExcludeFromCodeCoverage]
   public class TextBoxToolbarTest
   {
      [Test]
      public void TextBoxToolBar_ShouldInitializeProperly()
      {
         //Arrange
         var textBoxToolbar = new TextBoxToolbarFake();
         textBoxToolbar.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
         textBoxToolbar.Arrange(new Rect(textBoxToolbar.DesiredSize));

         //Act
         textBoxToolbar.RaiseEvent(new RoutedEventArgs(FrameworkElement.LoadedEvent));

         //Assert
         Assert.AreEqual(StringFinder.FindLocalizedString("TextBoxToolbarFontFamilyToolTip"), textBoxToolbar.FontFamilyToolTipText);
         Assert.AreEqual(StringFinder.FindLocalizedString("TextBoxToolbarFontSizeToolTip"), textBoxToolbar.FontSizeToolTipText);
         Assert.AreEqual(StringFinder.FindLocalizedString("TextBoxToolbarIncreaseFontSizeToolTip"), textBoxToolbar.IncrementFontSizeToolTipText);
         Assert.AreEqual(StringFinder.FindLocalizedString("TextBoxToolbarDecreaseFontSizeToolTip"), textBoxToolbar.DecrementFontSizeToolTipText);
         Assert.AreEqual(StringFinder.FindLocalizedString("TextBoxToolbarIncreaseLeftIndentToolTip"), textBoxToolbar.IncrementIndentToolTipText);
         Assert.AreEqual(StringFinder.FindLocalizedString("TextBoxToolbarDecreaseLeftIndentToolTip"), textBoxToolbar.DecrementIndentToolTipText);
         Assert.AreEqual(StringFinder.FindLocalizedString("TextBoxToolbarBoldToolTip"), textBoxToolbar.BoldToolTipText);
         Assert.AreEqual(StringFinder.FindLocalizedString("TextBoxToolbarItalicToolTip"), textBoxToolbar.ItalicToolTipText);
         Assert.AreEqual(StringFinder.FindLocalizedString("TextBoxToolbarUnderlineToolTip"), textBoxToolbar.UnderlineToolTipText);
         Assert.AreEqual(StringFinder.FindLocalizedString("TextBoxToolbarAlignCenterToolTip"), textBoxToolbar.AlignCenterToolTipText);
         Assert.AreEqual(StringFinder.FindLocalizedString("TextBoxToolbarBulletsToolTip"), textBoxToolbar.BulletsToolTipText);
         Assert.AreEqual(StringFinder.FindLocalizedString("TextBoxToolbarNumberedToolTip"), textBoxToolbar.NumberedToolTipText);
         Assert.AreEqual(StringFinder.FindLocalizedString("TextBoxToolbarHighlightColorToolTip"), textBoxToolbar.HighLightColorToolTipText);
         Assert.AreEqual(StringFinder.FindLocalizedString("TextBoxToolbarForeColorToolTip"), textBoxToolbar.FontColorToolTipText);

         Assert.AreEqual(false, textBoxToolbar.PopupStaysOpen);
         Assert.AreEqual(true, textBoxToolbar.FontSizeReadOnly);
         Assert.AreEqual(true, textBoxToolbar.FontSizeEditable);
         Assert.AreEqual(false, textBoxToolbar.FontSizeTextSearchEnabled);
         Assert.AreEqual(true, textBoxToolbar.FontSizeDropDownOnFocus);
      }

      [Test]
      public void TextBoxToolBar_ShouldForceFullOpacityValue()
      {
         //Arrange
         var textBoxToolbar = new TextBoxToolbarFake();

         //Act
         textBoxToolbar.Opacity = 0.5f;

         //Assert
         Assert.AreEqual(1, textBoxToolbar.Opacity);
      }
   }
}