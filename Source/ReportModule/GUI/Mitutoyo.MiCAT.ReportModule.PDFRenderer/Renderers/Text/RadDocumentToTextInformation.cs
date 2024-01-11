// <copyright file="RadDocumentToTextInformation.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using Telerik.Windows.Documents.Layout;
using Telerik.Windows.Documents.Model;

namespace Mitutoyo.MiCAT.ReportModule.PDFRenderer.Renderers.Text
{
   public class RadDocumentToTextInformation
   {
      public IEnumerable<TextLineInfo> GetTextInformation(RadDocument document)
      {
         var result = new List<TextLineInfo>();

         foreach (var paragraph in document.EnumerateChildrenOfType<Paragraph>())
         {
            var layoutBoxes = paragraph
                              .EnumerateChildrenOfType<Span>()
                              .SelectMany(s => s.GetAssociatedLayoutBoxes());

            if (!TryGetBottomPosition(layoutBoxes, 0f, out var currentBottomY))
               continue;

            TextLineInfo textLineInfo = null;
            bool isNewParagraph = true;

            foreach (SpanLayoutBox layoutBox in layoutBoxes)
            {
               if (IsRenderableLayoutBox(layoutBox))
               {
                  var rectangle = layoutBox.BoundingRectangle;

                  if (isNewParagraph || IsANewLineLayoutBox(rectangle.Y, currentBottomY))
                  {
                     textLineInfo = new TextLineInfo();
                     result.Add(textLineInfo);

                     isNewParagraph = false;

                     TryGetBottomPosition(layoutBoxes, rectangle.Y, out currentBottomY);
                  }

                  var textSettings = CreateTextSetting(layoutBox.AssociatedSpan);

                  var hightlightOffset = layoutBox.BaselineOffset - layoutBox.LineInfo.BaselineOffset;
                  var newTextInformation = new TextInformation(layoutBox.Text, textSettings, rectangle.Width, rectangle.Height, rectangle.Location.X, rectangle.Location.Y, hightlightOffset, layoutBox.LineInfo.Height, TextWrapping.NoWrap);

                  textLineInfo.Add(newTextInformation);
               }
            }
         }

         return result;
      }

      private TextSetting CreateTextSetting(Span span)
      {
         var textSettings = new TextSetting(
                              new SolidColorBrush(span.ForeColor),
                              span.FontFamily,
                              span.FontStyle,
                              span.FontWeight,
                              span.FontSize,
                              TextAlignment.Left,
                              span.BaselineAlignment,
                              span.HighlightColor,
                              span.Underline);

         return textSettings;
      }

      private bool IsANewLineLayoutBox(float currentY, float currentBottomY)
      {
         return currentY > currentBottomY;
      }

      private bool TryGetBottomPosition(IEnumerable<LayoutBox> layoutBoxes, float fromY, out float currentBottomY)
      {
         currentBottomY = 0f;

         var validLayoutBoxes = layoutBoxes.Where(lb => lb.BoundingRectangle.Y >= fromY && IsRenderableLayoutBox(lb));

         if (!validLayoutBoxes.Any())
            return false;

         var minY = validLayoutBoxes.Min(vlb => vlb.BoundingRectangle.Y);
         var minYLayoutBox = layoutBoxes.Where(lb => lb.BoundingRectangle.Y == minY).First();
         currentBottomY = minYLayoutBox.BoundingRectangle.Bottom;

         return true;
      }

      private bool IsRenderableLayoutBox(LayoutBox spanLayoutBox)
      {
         return !IsEnterLayoutBox(spanLayoutBox);
      }

      private bool IsEnterLayoutBox(LayoutBox spanLayoutBox)
      {
         return (spanLayoutBox is FormattingSymbolLayoutBox symbolLayout) && symbolLayout.FormattingSymbol == FormattingSymbols.Enter;
      }
   }
}
