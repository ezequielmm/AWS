// <copyright file="TextBoxRenderer.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using System.Windows;
using Mitutoyo.MiCAT.ReportModule.PDFRenderer.Renderers.Text;
using Mitutoyo.MiCAT.ReportModule.PDFRenderer.Utilities;

namespace Mitutoyo.MiCAT.ReportModule.PDFRenderer.Renderers.ConcreteRenderers
{
   [ExcludeFromCodeCoverage]
   public class TextBoxRenderer : UIElementRendererBase
   {
      public override bool Render(UIElement element, FixedDocumentRenderContext context)
      {
         var textBox = element as System.Windows.Controls.TextBox;
         if (textBox == null)
         {
            return false;
         }

         var textSetting = new TextSetting(textBox.Foreground, textBox.FontFamily, textBox.FontStyle, textBox.FontWeight, textBox.FontSize, textBox.TextAlignment);
         var textInformation = new TextInformation(textBox.Text, textSetting, (float) textBox.ActualWidth, (float) textBox.ActualHeight, TextWrapping.NoWrap);
         TextRenderer.DrawText(textInformation, context);

         return true;
      }
   }
}
