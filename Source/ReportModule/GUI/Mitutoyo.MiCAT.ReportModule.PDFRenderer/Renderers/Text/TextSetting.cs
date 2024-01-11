// <copyright file="TextSetting.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Windows;
using System.Windows.Media;
using Mitutoyo.MiCAT.ReportModule.PDFRenderer.Helpers;
using TelerikFlow = Telerik.Windows.Documents.Fixed.Model.Editing.Flow;
using TelerikModel = Telerik.Windows.Documents.Model;

namespace Mitutoyo.MiCAT.ReportModule.PDFRenderer.Renderers.Text
{
   public class TextSetting : ITextSetting
   {
      public static Color NoHighlightColor { get => Color.FromArgb(0, 255, 255, 255); }
      public static Color DefaultUnderlineColor { get => Color.FromArgb(0, 0, 0, 0); }

      public TextSetting(Brush foreground, FontFamily fontFamily, FontStyle fontStyle, FontWeight fontWeight, double fontSize, TextAlignment textAlignment)
         : this(foreground, fontFamily, fontStyle, fontWeight, fontSize, textAlignment, BaselineAlignment.Baseline, NoHighlightColor, false)
      { }
      public TextSetting(Brush foreground, FontFamily fontFamily, FontStyle fontStyle, FontWeight fontWeight, double fontSize, TextAlignment textAlignment, TelerikModel.BaselineAlignment baselineAlignment, Color highlightColor, bool underlined)
         :this(foreground, fontFamily, fontStyle, fontWeight, fontSize, textAlignment, ConvertFromTelerikModel(baselineAlignment), highlightColor, underlined)
      { }
      public TextSetting(Brush foreground, FontFamily fontFamily, FontStyle fontStyle, FontWeight fontWeight, double fontSize, TextAlignment textAlignment, BaselineAlignment baselineAlignment, Color highlightColor, bool underlined)
      {
         Foreground = foreground;
         FontFamily = fontFamily;
         FontStyle = fontStyle;
         FontWeight = fontWeight;
         FontSize = fontSize;
         TextAlignment = textAlignment;
         BaselineAlignment = baselineAlignment;
         HighlightColor = highlightColor;
         Underlined = underlined;
      }
      public Brush Foreground { get; private set; }
      public FontFamily FontFamily { get; private set; }
      public FontStyle FontStyle { get; private set; }
      public FontWeight FontWeight { get; private set; }
      public double FontSize { get; private set; }
      public TextAlignment TextAlignment { get; private set; }
      public BaselineAlignment BaselineAlignment { get; private set; }
      public Color HighlightColor { get; private set; }
      public bool Underlined { get; }
      public Color UnderlineColor()
      {
         if (Foreground is SolidColorBrush solidForeground)
            return solidForeground.Color;
         else
            return DefaultUnderlineColor;
      }

      public bool IsHighlighted { get => HighlightColor != NoHighlightColor; }

      public bool SameAs(ITextSetting settingsToCompare)
      {
         return settingsToCompare != null &&
                BrushHelper.SolidBrushEquals(Foreground, settingsToCompare.Foreground) &&
                FontFamily.Equals(settingsToCompare.FontFamily) &&
                FontStyle.Equals(settingsToCompare.FontStyle) &&
                FontWeight.Equals(settingsToCompare.FontWeight) &&
                FontSize == settingsToCompare.FontSize &&
                TextAlignment == settingsToCompare.TextAlignment &&
                BaselineAlignment == settingsToCompare.BaselineAlignment &&
                HighlightColor.Equals(settingsToCompare.HighlightColor) &&
                Underlined == settingsToCompare.Underlined &&
                UnderlineColor() == settingsToCompare.UnderlineColor();
      }

      public TelerikFlow.HorizontalAlignment TextAlignmentAsHorizontalAlignment()
      {
         switch (TextAlignment)
         {
            case TextAlignment.Right:
               return TelerikFlow.HorizontalAlignment.Right;
            case TextAlignment.Center:
               return TelerikFlow.HorizontalAlignment.Center;
            default:
               return TelerikFlow.HorizontalAlignment.Left;
         }
      }
      public static BaselineAlignment ConvertFromTelerikModel(TelerikModel.BaselineAlignment baselineAlignment)
      {
         switch (baselineAlignment)
         {
            case TelerikModel.BaselineAlignment.Subscript:
               return BaselineAlignment.Subscript;
            case TelerikModel.BaselineAlignment.Superscript:
               return BaselineAlignment.Superscript;
            default:
               return BaselineAlignment.Baseline;
         }
      }
      public TelerikFlow.BaselineAlignment BaselineAlignmentAsTelerikFlow()
      {
         switch (BaselineAlignment)
         {
            case BaselineAlignment.Baseline:
               return TelerikFlow.BaselineAlignment.Baseline;
            case BaselineAlignment.Subscript:
               return TelerikFlow.BaselineAlignment.Subscript;
            case BaselineAlignment.Superscript:
               return TelerikFlow.BaselineAlignment.Superscript;
            default:
               return TelerikFlow.BaselineAlignment.Baseline;
         }
      }
   }
}