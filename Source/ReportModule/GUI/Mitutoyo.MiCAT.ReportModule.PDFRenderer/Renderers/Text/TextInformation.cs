// <copyright file="TextInformation.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using System.Windows;

namespace Mitutoyo.MiCAT.ReportModule.PDFRenderer.Renderers.Text
{
   public class TextInformation
   {
      public TextInformation(string text, ITextSetting textSetting, float width, float height, TextWrapping textWrapping)
         : this(text, textSetting, width, height, 0, 0, 0, height, textWrapping)
      { }

      public TextInformation(string text, ITextSetting textSetting, float width, float height, float xPosition, float yPosition, float highlightOffset, float highlightHeight, TextWrapping textWrapping)
      {
         Text = text;
         Settings = textSetting;
         Width = width;
         Height = height;
         XPosition = xPosition;
         YPosition = yPosition;
         HighlightOffset = highlightOffset;
         HighlightHeight = highlightHeight;
         TextWrapping = textWrapping;
      }

      public String Text { get; private set; }
      public ITextSetting Settings { get; }
      public float XPosition { get; private set; }
      public float YPosition { get; }
      public float HighlightOffset { get; }
      public float HighlightHeight { get; }
      public TextWrapping TextWrapping { get; }
      public float Width { get; private set; }
      public float Height { get; }

      public bool IsZeroPosition { get => XPosition == 0 && YPosition == 0; }

      public bool TryAndAppend(TextInformation textToAppend)
      {
         if (IsPositionSizeCompatible(textToAppend) && Settings.SameAs(textToAppend.Settings))
         {
            Width += WidthToIncrease(textToAppend);
            Text += textToAppend.Text;
            return true;
         }
         else
            return false;
      }

      public void OffsetX(float xOffset)
      {
         XPosition += xOffset;
      }

      public bool IsPositionSizeCompatible(TextInformation textToAppend)
      {
         return (YPosition == textToAppend.YPosition) &&
            PositionIsVeryCloseToTheEndOfThisText(textToAppend) &&
               (Height == textToAppend.Height);
      }

      public bool PositionIsVeryCloseToTheEndOfThisText(TextInformation textToAppend)
      {
         return (Math.Round((XPosition + Width) - textToAppend.XPosition, 3) == 0);
      }
      private float WidthToIncrease(TextInformation textToAppend)
      {
         if ((XPosition + Width) > textToAppend.XPosition)
            return textToAppend.Width + ((XPosition + Width) - textToAppend.XPosition);
         else
            return textToAppend.Width;
      }
   }
}
