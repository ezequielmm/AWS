// <copyright file="CommonPageLayout.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using Mitutoyo.MiCAT.ApplicationState;

namespace Mitutoyo.MiCAT.ReportModule.Domain.Data
{
   public class CommonPageLayout : BaseStateEntity<CommonPageLayout>
   {
      public CommonPageLayout()
         : this(new PageSizeInfo(), new Margin(), new HeaderData(), new FooterData())
      {
      }

      public CommonPageLayout(PageSizeInfo pageSize, Margin canvasMargin) : this(new Id<CommonPageLayout>(Guid.NewGuid()), pageSize, canvasMargin, null, null)
      {
      }

      public CommonPageLayout(PageSizeInfo pageSize, Margin canvasMargin, HeaderData header, FooterData footer) : this(new Id<CommonPageLayout>(Guid.NewGuid()), pageSize, canvasMargin, header, footer)
      {
      }
      private CommonPageLayout(Id<CommonPageLayout> id, PageSizeInfo pageSize, Margin canvasMargin, HeaderData header, FooterData footer) : base(id)
      {
         PageSize = pageSize;
         CanvasMargin = canvasMargin;
         Header = header;
         Footer = footer;
      }

      public PageSizeInfo PageSize { get; }
      public Margin CanvasMargin { get; }

      public HeaderData Header { get; }

      public FooterData Footer { get; }

      public CommonPageLayout With(PageSizeInfo pageSize)
      {
         return new CommonPageLayout(Id, pageSize, CanvasMargin, Header, Footer);
      }

      public CommonPageLayout With(Margin canvasMargin)
      {
         return new CommonPageLayout(Id, PageSize, canvasMargin, Header, Footer);
      }

      public int GetUpperSpace()
      {
         var space = CanvasMargin.Top;
         if (HasHeader())
            space = Math.Max(CanvasMargin.Top, Header.Height);
         return space;
      }
      public int GetBottomSpace()
      {
         var space = CanvasMargin.Bottom;
         if (HasFooter())
            space = Math.Max(CanvasMargin.Bottom, Footer.Height);
         return space;
      }
      public bool HasHeader()
      {
         return Header != null;
      }
      public bool HasFooter()
      {
         return Footer != null;
      }
   }
}
