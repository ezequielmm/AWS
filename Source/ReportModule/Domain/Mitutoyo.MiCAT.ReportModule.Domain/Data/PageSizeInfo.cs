// <copyright file="PageSizeInfo.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Drawing.Printing;

namespace Mitutoyo.MiCAT.ReportModule.Domain.Data
{
   public class PageSizeInfo
   {
      public PageSizeInfo()
         : this(PaperKind.Letter, 0, 0)
      {
      }

      public PageSizeInfo(PaperKind paperKind, int height, int width)
      {
         PaperKind = paperKind;
         Height = height;
         Width = width;
      }
      public PaperKind PaperKind { get; }
      public int Height { get; }
      public int Width { get; }
   }
}
