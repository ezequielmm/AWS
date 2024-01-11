// <copyright file="TextBlockRenderer.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Controls;
using Mitutoyo.MiCAT.ReportModule.PDFRenderer.Renderers.Text;
using Mitutoyo.MiCAT.ReportModule.PDFRenderer.Utilities;

namespace Mitutoyo.MiCAT.ReportModule.PDFRenderer.Renderers.ConcreteRenderers
{
   [ExcludeFromCodeCoverage]
   public class TextBlockRenderer : UIElementRendererBase
   {
      public override bool Render(UIElement element, FixedDocumentRenderContext context)
      {
         var textBlock = element as TextBlock;
         if (textBlock == null)
         {
            return false;
         }

         //REFACTOR: Remove this when a Printing State for reports were created; or when TextBox autoresize were implemented (removing Ellipses)
         if (textBlock.Name == "EllipsisTextBlock")
            return true;

         var textSetting = new TextSetting(textBlock.Foreground, textBlock.FontFamily, textBlock.FontStyle, textBlock.FontWeight, textBlock.FontSize, textBlock.TextAlignment);
         var textInformation = new TextInformation(textBlock.Text, textSetting, (float)Math.Ceiling(textBlock.ActualWidth), (float)Math.Ceiling(textBlock.ActualHeight), TextWrapping.Wrap);
         TextRenderer.DrawText(textInformation, context);

         return true;
      }
   }
}
