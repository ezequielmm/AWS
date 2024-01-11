// <copyright file="TextRenderer.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Media;
using Mitutoyo.MiCAT.ReportModule.PDFRenderer.Renderers.ConcreteRenderers;
using Mitutoyo.MiCAT.ReportModule.PDFRenderer.Utilities;
using Telerik.Windows.Documents.Fixed.Model.ColorSpaces;
using Telerik.Windows.Documents.Fixed.Model.Editing;

namespace Mitutoyo.MiCAT.ReportModule.PDFRenderer.Renderers.Text
{
   [ExcludeFromCodeCoverage]
   public class TextRenderer
   {
      public static void DrawTextLine(TextLineInfo textLineInfo, FixedDocumentRenderContext context)
      {
         float xOffSet = 0;

         foreach (var textInfo in textLineInfo)
         {
            textInfo.OffsetX(xOffSet);
            var currentOffset = DrawText(context, textInfo);

            if (!string.IsNullOrEmpty(textInfo.Text.Trim()))
               xOffSet += currentOffset;
         }
      }

      public static void DrawText(TextInformation text, FixedDocumentRenderContext context)
      {
         DrawText(context, text);
      }

      private static float DrawText(FixedDocumentRenderContext context, TextInformation text)
      {
         if (!text.IsZeroPosition)
            context.DrawingSurface.Position.Translate(text.XPosition, text.YPosition);

         float currentOffset;

         using (context.DrawingSurface.SaveProperties())
         {
            UIElementRendererBase.SetFill(context, text.Settings.Foreground, text.Width, text.Height);

            var block = CreateBlock(text, context);

            if (text.Settings.Underlined)
               SetUnderLine(block, text);

            block.InsertText(text.Settings.FontFamily, text.Settings.FontStyle, text.Settings.FontWeight, text.Text);

            currentOffset = CalculateOffset(block, text);

            if (text.Settings.IsHighlighted)
               DrawHighlightBlock(text, context, currentOffset);

            DrawBlock(context, block, text);

            if (!text.IsZeroPosition)
               context.DrawingSurface.Position.Translate(-text.XPosition, -text.YPosition);
         }

         return currentOffset;
      }

      private static void DrawBlock(FixedDocumentRenderContext context, Block block, TextInformation text)
      {
         if (text.TextWrapping == TextWrapping.NoWrap)
            context.DrawingSurface.DrawBlock(block);
         else
            context.DrawingSurface.DrawBlock(block, new Size(text.Width, text.Height));
      }

      private static float CalculateOffset(Block block, TextInformation text)
      {
         var measuredWidth = (float)block.Measure().Width;
         var offset = measuredWidth == 0f ? 0f : measuredWidth - text.Width;

         return offset;
      }

      private static void SetUnderLine(Block block, TextInformation text)
      {
         var underlineColor = text.Settings.UnderlineColor();
         block.TextProperties.UnderlineColor = new RgbColor(underlineColor.R, underlineColor.G, underlineColor.B);
         block.TextProperties.UnderlinePattern = Telerik.Windows.Documents.Fixed.Model.Editing.Flow.UnderlinePattern.Single;
      }

      private static Block CreateBlock(TextInformation text, FixedDocumentRenderContext context)
      {
         var block = new Block();

         block.TextProperties.FontSize = text.Settings.FontSize;
         block.GraphicProperties.CopyFrom(context.DrawingSurface.GraphicProperties);
         block.TextProperties.BaselineAlignment = text.Settings.BaselineAlignmentAsTelerikFlow();
         block.HorizontalAlignment = text.Settings.TextAlignmentAsHorizontalAlignment();

         return block;
      }

      private static void DrawHighlightBlock(TextInformation text, FixedDocumentRenderContext context, float currentOffset)
      {
            UIElementRendererBase.SetFill(context, new SolidColorBrush(text.Settings.HighlightColor), text.Width, text.Height);
            context.DrawingSurface.GraphicProperties.IsStroked = false;
            var yOffset = text.HighlightOffset;
            var height = text.HighlightHeight;

            context.DrawingSurface.Position.Translate(0, yOffset);
            context.DrawingSurface.DrawRectangle(new Rect(0d, 0d, text.Width + currentOffset, height));
            context.DrawingSurface.Position.Translate(0, -yOffset);
      }
   }
}
