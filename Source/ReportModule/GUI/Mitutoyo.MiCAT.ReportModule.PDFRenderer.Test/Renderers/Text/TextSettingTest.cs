// <copyright file="TextSettingTest.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Media;
using Mitutoyo.MiCAT.ReportModule.PDFRenderer.Renderers.Text;
using NUnit.Framework;
using TelerikFlow = Telerik.Windows.Documents.Fixed.Model.Editing.Flow;
using TelerikModel = Telerik.Windows.Documents.Model;

namespace Mitutoyo.MiCAT.ReportModule.PDFRenderer.Test.Renderers.Text
{
   [ExcludeFromCodeCoverage]
   [TestFixture]
   public class TextSettingTest
   {
      private SolidColorBrush GetNewBrushForTest()
      {
         return new SolidColorBrush(Color.FromRgb(100, 100, 100));
      }
      private FontFamily GetNewFontFamilyForTest()
      {
         return new FontFamily("Arial");
      }
      private FontStyle GetFontStyleForTest()
      {
         return new FontStyle();
      }
      private FontWeight GetFontWeightForTest()
      {
         return FontWeight.FromOpenTypeWeight(12);
      }
      private Color GetHighlightColorForTest()
      {
         return Color.FromRgb(10, 10, 10);
      }

      private double _defaultFontSizeToTest = 10;
      private TextAlignment _defaultTextAlignamentToTest = TextAlignment.Center;
      private BaselineAlignment _defaultBaseAlignmentToTest = BaselineAlignment.Center;
      private bool _defaultUnderlinedToTest = true;

      [Test]
      public void ConstructorAndPropertiesTest()
      {
         //Arrange
         TextSetting sut;
         var brush = GetNewBrushForTest();
         var fontFamily = GetNewFontFamilyForTest();
         var fontStyle = GetFontStyleForTest();
         var fontWeight = GetFontWeightForTest();
         var highlightColor = GetHighlightColorForTest();

         //Act
         sut = new TextSetting(brush, fontFamily, fontStyle, fontWeight, _defaultFontSizeToTest, _defaultTextAlignamentToTest, _defaultBaseAlignmentToTest, highlightColor, _defaultUnderlinedToTest);

         //Assert
         Assert.AreEqual(brush, sut.Foreground);
         Assert.AreEqual(fontFamily, sut.FontFamily);
         Assert.AreEqual(fontStyle, sut.FontStyle);
         Assert.AreEqual(fontWeight, sut.FontWeight);
         Assert.AreEqual(_defaultFontSizeToTest, sut.FontSize);
         Assert.AreEqual(_defaultTextAlignamentToTest, sut.TextAlignment);
         Assert.AreEqual(_defaultBaseAlignmentToTest, sut.BaselineAlignment);
         Assert.AreEqual(highlightColor, sut.HighlightColor);
         Assert.AreEqual(_defaultUnderlinedToTest, sut.Underlined);
      }
      [Test]
      public void DefaultBaselineHightlightUnderlineConstructorTest()
      {
         //Arrange
         TextSetting sut;

         //Act
         sut = new TextSetting(GetNewBrushForTest(), GetNewFontFamilyForTest(), GetFontStyleForTest(), GetFontWeightForTest(), _defaultFontSizeToTest, _defaultTextAlignamentToTest);

         //Assert
         Assert.AreEqual(BaselineAlignment.Baseline, sut.BaselineAlignment);
         Assert.AreEqual(TextSetting.NoHighlightColor, sut.HighlightColor);
         Assert.IsFalse(sut.Underlined);
      }

      [Test]
      [TestCase(TelerikModel.BaselineAlignment.Baseline, BaselineAlignment.Baseline)]
      [TestCase(TelerikModel.BaselineAlignment.Subscript, BaselineAlignment.Subscript)]
      [TestCase(TelerikModel.BaselineAlignment.Superscript, BaselineAlignment.Superscript)]
      public void ConstructorConvertingBaselineTypeTest(TelerikModel.BaselineAlignment telerikBaseline, BaselineAlignment expectedBaseline)
      {
         //Arrange
         TextSetting sut;

         //Act
         sut = new TextSetting(GetNewBrushForTest(), GetNewFontFamilyForTest(), GetFontStyleForTest(), GetFontWeightForTest(), _defaultFontSizeToTest, _defaultTextAlignamentToTest, telerikBaseline, GetHighlightColorForTest(), _defaultUnderlinedToTest);

         //Assert
         Assert.AreEqual(expectedBaseline, sut.BaselineAlignment);
      }

      [Test]
      [TestCase(BaselineAlignment.Baseline, TelerikFlow.BaselineAlignment.Baseline)]
      [TestCase(BaselineAlignment.Subscript, TelerikFlow.BaselineAlignment.Subscript)]
      [TestCase(BaselineAlignment.Superscript, TelerikFlow.BaselineAlignment.Superscript)]
      [TestCase(BaselineAlignment.Bottom, TelerikFlow.BaselineAlignment.Baseline)]
      [TestCase(BaselineAlignment.Center, TelerikFlow.BaselineAlignment.Baseline)]
      [TestCase(BaselineAlignment.TextBottom, TelerikFlow.BaselineAlignment.Baseline)]
      [TestCase(BaselineAlignment.TextTop, TelerikFlow.BaselineAlignment.Baseline)]
      [TestCase(BaselineAlignment.Top, TelerikFlow.BaselineAlignment.Baseline)]
      public void ConvertingBaselineTypeTest(BaselineAlignment baseline, TelerikFlow.BaselineAlignment expectedTelerikBaseline)
      {
         //Arrange
         TextSetting sut;

         //Act
         sut = new TextSetting(GetNewBrushForTest(), GetNewFontFamilyForTest(), GetFontStyleForTest(), GetFontWeightForTest(), _defaultFontSizeToTest, _defaultTextAlignamentToTest, baseline, GetHighlightColorForTest(), _defaultUnderlinedToTest);

         //Assert
         Assert.AreEqual(expectedTelerikBaseline, sut.BaselineAlignmentAsTelerikFlow());
      }

      [Test]
      [TestCase(TextAlignment.Right, TelerikFlow.HorizontalAlignment.Right)]
      [TestCase(TextAlignment.Center, TelerikFlow.HorizontalAlignment.Center)]
      [TestCase(TextAlignment.Left, TelerikFlow.HorizontalAlignment.Left)]
      [TestCase(TextAlignment.Justify, TelerikFlow.HorizontalAlignment.Left)]
      public void ConvertingTextAlignmentToHorizontalAlignmentTest(TextAlignment textAlignment, TelerikFlow.HorizontalAlignment expectedHorizontalAlignment)
      {
         //Arrange
         TextSetting sut;

         //Act
         sut = new TextSetting(GetNewBrushForTest(), GetNewFontFamilyForTest(), GetFontStyleForTest(), GetFontWeightForTest(), _defaultFontSizeToTest, textAlignment, _defaultBaseAlignmentToTest, GetHighlightColorForTest(), _defaultUnderlinedToTest);

         //Assert
         Assert.AreEqual(expectedHorizontalAlignment, sut.TextAlignmentAsHorizontalAlignment());
      }

      [Test]

      public void IsHighlightedPropertyFalseTest()
      {
         //Arrange
         TextSetting sut;

         //Act
         sut = new TextSetting(GetNewBrushForTest(), GetNewFontFamilyForTest(), GetFontStyleForTest(), GetFontWeightForTest(), _defaultFontSizeToTest, _defaultTextAlignamentToTest, _defaultBaseAlignmentToTest, TextSetting.NoHighlightColor, _defaultUnderlinedToTest);

         //Assert
         Assert.IsFalse(sut.IsHighlighted);
      }

      [Test]
      public void IsHighlightedPropertyTrueTest()
      {
         //Arrange
         TextSetting sut;

         //Act
         sut = new TextSetting(GetNewBrushForTest(), GetNewFontFamilyForTest(), GetFontStyleForTest(), GetFontWeightForTest(), _defaultFontSizeToTest, _defaultTextAlignamentToTest, _defaultBaseAlignmentToTest, Color.FromRgb(0, 0, 0), _defaultUnderlinedToTest);

         //Assert
         Assert.IsTrue(sut.IsHighlighted);
      }

      [Test]
      public void UnderlineColorFromForegroundTest()
      {
         //Arrange
         TextSetting sut;
         Color foreColor = Color.FromRgb(50, 50, 50);

         //Act
         sut = new TextSetting(new SolidColorBrush(foreColor), GetNewFontFamilyForTest(), GetFontStyleForTest(), GetFontWeightForTest(), _defaultFontSizeToTest, _defaultTextAlignamentToTest, _defaultBaseAlignmentToTest, GetHighlightColorForTest(), true);

         //Assert
         Assert.AreEqual(foreColor, sut.UnderlineColor());
      }

      [Test]
      public void UnderlineColorBlackTest()
      {
         //Arrange
         TextSetting sut;

         //Act
         sut = new TextSetting(new LinearGradientBrush(), GetNewFontFamilyForTest(), GetFontStyleForTest(), GetFontWeightForTest(), _defaultFontSizeToTest, _defaultTextAlignamentToTest, _defaultBaseAlignmentToTest, GetHighlightColorForTest(), true);

         //Assert
         Assert.AreEqual(TextSetting.DefaultUnderlineColor, sut.UnderlineColor());
      }

      [Test]
      public void AreSameTest()
      {
         //Arrange
         TextSetting sut1;
         var brush1 = GetNewBrushForTest();
         var fontFamily1 = GetNewFontFamilyForTest();
         var fontStyle1 = GetFontStyleForTest();
         var fontWeight1 = GetFontWeightForTest();
         var highlightColor1 = GetHighlightColorForTest();

         sut1 = new TextSetting(brush1, fontFamily1, fontStyle1, fontWeight1, _defaultFontSizeToTest, _defaultTextAlignamentToTest, _defaultBaseAlignmentToTest, highlightColor1, _defaultUnderlinedToTest);

         TextSetting sut2;
         var brush2 = GetNewBrushForTest();
         var fontFamily2 = GetNewFontFamilyForTest();
         var fontStyle2 = GetFontStyleForTest();
         var fontWeight2 = GetFontWeightForTest();
         var highlightColor2 = GetHighlightColorForTest();

         sut2 = new TextSetting(brush2, fontFamily2, fontStyle2, fontWeight2, _defaultFontSizeToTest, _defaultTextAlignamentToTest, _defaultBaseAlignmentToTest, highlightColor2, _defaultUnderlinedToTest);

         //Act
         var areSame = sut1.SameAs(sut2);

         //Assert
         Assert.IsTrue(areSame);
      }

      [Test]
      public void AreNotSameBecauseNullTest()
      {
         //Arrange
         TextSetting sut1;
         var brush1 = GetNewBrushForTest();
         var fontFamily1 = GetNewFontFamilyForTest();
         var fontStyle1 = GetFontStyleForTest();
         var fontWeight1 = GetFontWeightForTest();
         var highlightColor1 = GetHighlightColorForTest();

         sut1 = new TextSetting(brush1, fontFamily1, fontStyle1, fontWeight1, _defaultFontSizeToTest, _defaultTextAlignamentToTest, _defaultBaseAlignmentToTest, highlightColor1, _defaultUnderlinedToTest);

         TextSetting sut2 = null;

         //Act
         var areSame = sut1.SameAs(sut2);

         //Assert
         Assert.IsFalse(areSame);
      }

      [Test]
      public void AreNotSameBecauseBrushTypeColorTest()
      {
         //Arrange
         TextSetting sut1;
         var brush1 = new LinearGradientBrush();
         var fontFamily1 = GetNewFontFamilyForTest();
         var fontStyle1 = GetFontStyleForTest();
         var fontWeight1 = GetFontWeightForTest();
         var highlightColor1 = GetHighlightColorForTest();

         sut1 = new TextSetting(brush1, fontFamily1, fontStyle1, fontWeight1, _defaultFontSizeToTest, _defaultTextAlignamentToTest, _defaultBaseAlignmentToTest, highlightColor1, _defaultUnderlinedToTest);

         TextSetting sut2;
         var brush2 = new SolidColorBrush();
         var fontFamily2 = GetNewFontFamilyForTest();
         var fontStyle2 = GetFontStyleForTest();
         var fontWeight2 = GetFontWeightForTest();
         var highlightColor2 = GetHighlightColorForTest();

         sut2 = new TextSetting(brush2, fontFamily2, fontStyle2, fontWeight2, _defaultFontSizeToTest, _defaultTextAlignamentToTest, _defaultBaseAlignmentToTest, highlightColor2, _defaultUnderlinedToTest);

         //Act
         var areSame = sut1.SameAs(sut2);

         //Assert
         Assert.IsFalse(areSame);
      }

      [Test]
      public void AreNotSameBecauseSolidBrushColorTest()
      {
         //Arrange
         TextSetting sut1;
         var brush1 = new SolidColorBrush(Color.FromRgb(250, 250, 250));
         var fontFamily1 = GetNewFontFamilyForTest();
         var fontStyle1 = GetFontStyleForTest();
         var fontWeight1 = GetFontWeightForTest();
         var highlightColor1 = GetHighlightColorForTest();

         sut1 = new TextSetting(brush1, fontFamily1, fontStyle1, fontWeight1, _defaultFontSizeToTest, _defaultTextAlignamentToTest, _defaultBaseAlignmentToTest, highlightColor1, _defaultUnderlinedToTest);

         TextSetting sut2;
         var brush2 = new SolidColorBrush(Color.FromRgb(10, 10, 10));
         var fontFamily2 = GetNewFontFamilyForTest();
         var fontStyle2 = GetFontStyleForTest();
         var fontWeight2 = GetFontWeightForTest();
         var highlightColor2 = GetHighlightColorForTest();

         sut2 = new TextSetting(brush2, fontFamily2, fontStyle2, fontWeight2, _defaultFontSizeToTest, _defaultTextAlignamentToTest, _defaultBaseAlignmentToTest, highlightColor2, _defaultUnderlinedToTest);

         //Act
         var areSame = sut1.SameAs(sut2);

         //Assert
         Assert.IsFalse(areSame);
      }

      [Test]
      public void AreNotSameBecauseBrushOpacityTest()
      {
         //Arrange
         TextSetting sut1;
         var brush1 = GetNewBrushForTest();
         brush1.Opacity = 0.5;
         var fontFamily1 = GetNewFontFamilyForTest();
         var fontStyle1 = GetFontStyleForTest();
         var fontWeight1 = GetFontWeightForTest();
         var highlightColor1 = GetHighlightColorForTest();

         sut1 = new TextSetting(brush1, fontFamily1, fontStyle1, fontWeight1, _defaultFontSizeToTest, _defaultTextAlignamentToTest, _defaultBaseAlignmentToTest, highlightColor1, _defaultUnderlinedToTest);

         TextSetting sut2;
         var brush2 = GetNewBrushForTest();
         brush2.Opacity = 1;
         var fontFamily2 = GetNewFontFamilyForTest();
         var fontStyle2 = GetFontStyleForTest();
         var fontWeight2 = GetFontWeightForTest();
         var highlightColor2 = GetHighlightColorForTest();

         sut2 = new TextSetting(brush2, fontFamily2, fontStyle2, fontWeight2, _defaultFontSizeToTest, _defaultTextAlignamentToTest, _defaultBaseAlignmentToTest, highlightColor2, _defaultUnderlinedToTest);

         //Act
         var areSame = sut1.SameAs(sut2);

         //Assert
         Assert.IsFalse(areSame);
      }

      [Test]
      public void AreNotSameBecauseFontFamilyTest()
      {
         //Arrange
         TextSetting sut1;
         var brush1 = GetNewBrushForTest();
         var fontFamily1 = new FontFamily("Arial");
         var fontStyle1 = GetFontStyleForTest();
         var fontWeight1 = GetFontWeightForTest();
         var highlightColor1 = GetHighlightColorForTest();

         sut1 = new TextSetting(brush1, fontFamily1, fontStyle1, fontWeight1, _defaultFontSizeToTest, _defaultTextAlignamentToTest, _defaultBaseAlignmentToTest, highlightColor1, _defaultUnderlinedToTest);

         TextSetting sut2;
         var brush2 = GetNewBrushForTest();
         var fontFamily2 = new FontFamily("Comic Sans");
         var fontStyle2 = GetFontStyleForTest();
         var fontWeight2 = GetFontWeightForTest();
         var highlightColor2 = GetHighlightColorForTest();

         sut2 = new TextSetting(brush2, fontFamily2, fontStyle2, fontWeight2, _defaultFontSizeToTest, _defaultTextAlignamentToTest, _defaultBaseAlignmentToTest, highlightColor2, _defaultUnderlinedToTest);

         //Act
         var areSame = sut1.SameAs(sut2);

         //Assert
         Assert.IsFalse(areSame);
      }

      [Test]
      public void AreNotSameBecauseFontWeightTest()
      {
         //Arrange
         TextSetting sut1;
         var brush1 = GetNewBrushForTest();
         var fontFamily1 = GetNewFontFamilyForTest();
         var fontStyle1 = GetFontStyleForTest();
         var fontWeight1 = FontWeight.FromOpenTypeWeight(10);
         var highlightColor1 = GetHighlightColorForTest();

         sut1 = new TextSetting(brush1, fontFamily1, fontStyle1, fontWeight1, _defaultFontSizeToTest, _defaultTextAlignamentToTest, _defaultBaseAlignmentToTest, highlightColor1, _defaultUnderlinedToTest);

         TextSetting sut2;
         var brush2 = GetNewBrushForTest();
         var fontFamily2 = GetNewFontFamilyForTest();
         var fontStyle2 = GetFontStyleForTest();
         var fontWeight2 = FontWeight.FromOpenTypeWeight(20);
         var highlightColor2 = GetHighlightColorForTest();

         sut2 = new TextSetting(brush2, fontFamily2, fontStyle2, fontWeight2, _defaultFontSizeToTest, _defaultTextAlignamentToTest, _defaultBaseAlignmentToTest, highlightColor2, _defaultUnderlinedToTest);

         //Act
         var areSame = sut1.SameAs(sut2);

         //Assert
         Assert.IsFalse(areSame);
      }

      [Test]
      public void AreNotSameBecauseHighLightColorTest()
      {
         //Arrange
         TextSetting sut1;
         var brush1 = GetNewBrushForTest();
         var fontFamily1 = GetNewFontFamilyForTest();
         var fontStyle1 = GetFontStyleForTest();
         var fontWeight1 = GetFontWeightForTest();
         var highlightColor1 = Color.FromRgb(10, 10, 10);

         sut1 = new TextSetting(brush1, fontFamily1, fontStyle1, fontWeight1, _defaultFontSizeToTest, _defaultTextAlignamentToTest, _defaultBaseAlignmentToTest, highlightColor1, _defaultUnderlinedToTest);

         TextSetting sut2;
         var brush2 = GetNewBrushForTest();
         var fontFamily2 = GetNewFontFamilyForTest();
         var fontStyle2 = GetFontStyleForTest();
         var fontWeight2 = GetFontWeightForTest();
         var highlightColor2 = Color.FromRgb(20, 20, 20);

         sut2 = new TextSetting(brush2, fontFamily2, fontStyle2, fontWeight2, _defaultFontSizeToTest, _defaultTextAlignamentToTest, _defaultBaseAlignmentToTest, highlightColor2, _defaultUnderlinedToTest);

         //Act
         var areSame = sut1.SameAs(sut2);

         //Assert
         Assert.IsFalse(areSame);
      }

      [Test]
      public void AreNotSameBecauseFontSizeTest()
      {
         //Arrange
         TextSetting sut1;
         var brush1 = GetNewBrushForTest();
         var fontFamily1 = GetNewFontFamilyForTest();
         var fontStyle1 = GetFontStyleForTest();
         var fontWeight1 = GetFontWeightForTest();
         var highlightColor1 = GetHighlightColorForTest();
         double fontSize1 = 12;

         sut1 = new TextSetting(brush1, fontFamily1, fontStyle1, fontWeight1, fontSize1, _defaultTextAlignamentToTest, _defaultBaseAlignmentToTest, highlightColor1, _defaultUnderlinedToTest);

         TextSetting sut2;
         var brush2 = GetNewBrushForTest();
         var fontFamily2 = GetNewFontFamilyForTest();
         var fontStyle2 = GetFontStyleForTest();
         var fontWeight2 = GetFontWeightForTest();
         var highlightColor2 = GetHighlightColorForTest();
         double fontSize2 = 14;

         sut2 = new TextSetting(brush2, fontFamily2, fontStyle2, fontWeight2, fontSize2, _defaultTextAlignamentToTest, _defaultBaseAlignmentToTest, highlightColor2, _defaultUnderlinedToTest);

         //Act
         var areSame = sut1.SameAs(sut2);

         //Assert
         Assert.IsFalse(areSame);
      }

      [Test]
      public void AreNotSameBecauseTextAlignmentTest()
      {
         //Arrange
         TextSetting sut1;
         var brush1 = GetNewBrushForTest();
         var fontFamily1 = GetNewFontFamilyForTest();
         var fontStyle1 = GetFontStyleForTest();
         var fontWeight1 = GetFontWeightForTest();
         var highlightColor1 = GetHighlightColorForTest();
         TextAlignment textAlignment1 = TextAlignment.Left;

         sut1 = new TextSetting(brush1, fontFamily1, fontStyle1, fontWeight1, _defaultFontSizeToTest, textAlignment1, _defaultBaseAlignmentToTest, highlightColor1, _defaultUnderlinedToTest);

         TextSetting sut2;
         var brush2 = GetNewBrushForTest();
         var fontFamily2 = GetNewFontFamilyForTest();
         var fontStyle2 = GetFontStyleForTest();
         var fontWeight2 = GetFontWeightForTest();
         var highlightColor2 = GetHighlightColorForTest();
         TextAlignment textAlignment2 = TextAlignment.Center;

         sut2 = new TextSetting(brush2, fontFamily2, fontStyle2, fontWeight2, _defaultFontSizeToTest, textAlignment2, _defaultBaseAlignmentToTest, highlightColor2, _defaultUnderlinedToTest);

         //Act
         var areSame = sut1.SameAs(sut2);

         //Assert
         Assert.IsFalse(areSame);
      }

      [Test]
      public void AreNotSameBecauseBaseAlignmentTest()
      {
         //Arrange
         TextSetting sut1;
         var brush1 = GetNewBrushForTest();
         var fontFamily1 = GetNewFontFamilyForTest();
         var fontStyle1 = GetFontStyleForTest();
         var fontWeight1 = GetFontWeightForTest();
         var highlightColor1 = GetHighlightColorForTest();
         BaselineAlignment baselineAlignment1 = BaselineAlignment.Center;

         sut1 = new TextSetting(brush1, fontFamily1, fontStyle1, fontWeight1, _defaultFontSizeToTest, _defaultTextAlignamentToTest, baselineAlignment1, highlightColor1, _defaultUnderlinedToTest);

         TextSetting sut2;
         var brush2 = GetNewBrushForTest();
         var fontFamily2 = GetNewFontFamilyForTest();
         var fontStyle2 = GetFontStyleForTest();
         var fontWeight2 = GetFontWeightForTest();
         var highlightColor2 = GetHighlightColorForTest();
         BaselineAlignment baselineAlignment2 = BaselineAlignment.Subscript;

         sut2 = new TextSetting(brush2, fontFamily2, fontStyle2, fontWeight2, _defaultFontSizeToTest, _defaultTextAlignamentToTest, baselineAlignment2, highlightColor2, _defaultUnderlinedToTest);

         //Act
         var areSame = sut1.SameAs(sut2);

         //Assert
         Assert.IsFalse(areSame);
      }

      [Test]
      public void AreNotSameBecauseUnderlinedTest()
      {
         //Arrange
         TextSetting sut1;
         var brush1 = GetNewBrushForTest();
         var fontFamily1 = GetNewFontFamilyForTest();
         var fontStyle1 = GetFontStyleForTest();
         var fontWeight1 = GetFontWeightForTest();
         var highlightColor1 = GetHighlightColorForTest();
         bool underlined1 = true;

         sut1 = new TextSetting(brush1, fontFamily1, fontStyle1, fontWeight1, _defaultFontSizeToTest, _defaultTextAlignamentToTest, _defaultBaseAlignmentToTest, highlightColor1, underlined1);

         TextSetting sut2;
         var brush2 = GetNewBrushForTest();
         var fontFamily2 = GetNewFontFamilyForTest();
         var fontStyle2 = GetFontStyleForTest();
         var fontWeight2 = GetFontWeightForTest();
         var highlightColor2 = GetHighlightColorForTest();
         bool underlined2 = false;

         sut2 = new TextSetting(brush2, fontFamily2, fontStyle2, fontWeight2, _defaultFontSizeToTest, _defaultTextAlignamentToTest, _defaultBaseAlignmentToTest, highlightColor2, underlined2);

         //Act
         var areSame = sut1.SameAs(sut2);

         //Assert
         Assert.IsFalse(areSame);
      }
   }
}
