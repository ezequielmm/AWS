// <copyright file="ImageHelper.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using System.Drawing;
using System.IO;

namespace Mitutoyo.MiCAT.ReportModuleApp.Helpers
{
   public static class ImageHelper
   {
      public static ImageData GetBase64StringFromFile(string file)
      {
         using (MemoryStream ms = new MemoryStream())
         {
            var imageDataHelper = new ImageData();
            if (file.Contains(".ico"))
            {
               var imageSelected = (Icon)System.Drawing.Icon.ExtractAssociatedIcon(file);
               imageSelected.Save(ms);
               imageDataHelper.HeightData = imageSelected.Height;
               imageDataHelper.WidthData = imageSelected.Width;
               imageDataHelper.ImageString = Convert.ToBase64String(ms.ToArray());
            }
            else
            {
               var imageSelected = (Image)System.Drawing.Image.FromFile(file);
               imageSelected.Save(ms, imageSelected.RawFormat);
               imageDataHelper.HeightData = imageSelected.Height;
               imageDataHelper.WidthData = imageSelected.Width;
               imageDataHelper.ImageString = Convert.ToBase64String(ms.ToArray());
            }
            return imageDataHelper;
         }
      }

      public static string GetBase64StringFromImage(Image img)
      {
         using (MemoryStream ms = new MemoryStream())
         {
            img.Save(ms, img.RawFormat);
            return Convert.ToBase64String(ms.ToArray());
         }
      }
   }
}
