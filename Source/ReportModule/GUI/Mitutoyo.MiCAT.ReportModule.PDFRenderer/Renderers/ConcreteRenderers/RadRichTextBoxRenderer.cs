// <copyright file="RadRichTextBoxRenderer.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using System.Windows;
using Mitutoyo.MiCAT.ReportModule.PDFRenderer.Renderers.Text;
using Mitutoyo.MiCAT.ReportModule.PDFRenderer.Utilities;
using Telerik.Windows.Controls;

namespace Mitutoyo.MiCAT.ReportModule.PDFRenderer.Renderers.ConcreteRenderers
{
   [ExcludeFromCodeCoverage]
   public class RadRichTextBoxRenderer : UIElementRendererBase
   {
      public override bool Render(UIElement element, FixedDocumentRenderContext context)
      {
         RadRichTextBox radRichTextBox = element as RadRichTextBox;
         if (radRichTextBox == null)
         {
            return false;
         }

         var radDocumentConverter = new RadDocumentToTextInformation();

         foreach (var textLineInfo in radDocumentConverter.GetTextInformation(radRichTextBox.Document))
            TextRenderer.DrawTextLine(textLineInfo, context);

         return true;
      }
   }
}
