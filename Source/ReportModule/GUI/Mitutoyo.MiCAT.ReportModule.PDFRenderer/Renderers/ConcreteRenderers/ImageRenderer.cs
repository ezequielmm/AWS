// <copyright file="ImageRenderer.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using Mitutoyo.MiCAT.ReportModule.PDFRenderer.Utilities;
using Telerik.Windows.Media.Imaging;

namespace Mitutoyo.MiCAT.ReportModule.PDFRenderer.Renderers.ConcreteRenderers
{
   [ExcludeFromCodeCoverage]
   public class ImageRenderer : UIElementRendererBase
   {
      private const double DPIForExportedImage = 300;

      public override bool Render(UIElement element, FixedDocumentRenderContext context)
      {
         var image = element as System.Windows.Controls.Image;
         if (image == null)
         {
            return false;
         }

         if (image.ActualWidth > 0 && image.ActualHeight > 0)
         {
           using (var stream = new MemoryStream())
           {
              ExportExtensions.ExportToImage(image, stream, DPIForExportedImage, DPIForExportedImage, new PngBitmapEncoder());
              context.DrawingSurface.DrawImage(stream, image.ActualWidth, image.ActualHeight);
           }
         }

         return true;
      }
   }
}
