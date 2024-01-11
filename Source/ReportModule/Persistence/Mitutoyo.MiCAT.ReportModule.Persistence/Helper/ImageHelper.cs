// <copyright file="ImageHelper.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Drawing;

namespace Mitutoyo.MiCAT.ReportModule.Persistence.Helper
{
   public static class ImageHelper
   {
      public static Image GetImageFromByteArray(byte[] data)
      {
         return (Bitmap)((new ImageConverter()).ConvertFrom(data));
      }
   }
}
