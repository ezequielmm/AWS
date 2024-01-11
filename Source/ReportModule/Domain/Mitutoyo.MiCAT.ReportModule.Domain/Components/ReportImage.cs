// <copyright file="ReportImage.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using Mitutoyo.MiCAT.ApplicationState;

namespace Mitutoyo.MiCAT.ReportModule.Domain.Components
{
   public class ReportImage : ReportComponentBase<ReportImage>
   {
      public static ReportImage CreateDefaultForBoundarySection(IItemId reportSectionId, int x, int y)
      {
         const int BOUNDARY_SECTION_WIDTH = 100;
         const int BOUNDARY_SECTION_HEIGHT = 100;

         return new ReportImage(new ReportComponentPlacement(reportSectionId, x, y, BOUNDARY_SECTION_WIDTH, BOUNDARY_SECTION_HEIGHT));
      }

      public static ReportImage CreateDefaultForBody(IItemId reportSectionId, int x, int y)
      {
         const int BODY_IMAGE_WIDTH = 150;
         const int BODY_IMAGE_HEIGHT = 150;

         return new ReportImage(new ReportComponentPlacement(reportSectionId, x, y, BODY_IMAGE_WIDTH, BODY_IMAGE_HEIGHT));
      }

      public ReportImage(ReportComponentPlacement placement)
         : this(placement, string.Empty)
      {
      }

      public ReportImage(ReportComponentPlacement placement, string image)
         : this(Guid.NewGuid(), placement, image)
      {
      }

      private ReportImage(Id<ReportImage> id, ReportComponentPlacement placement, string image)
         : base(id, placement)
      {
         Image = image;
      }

      public string Image { get; }

      public ReportImage WithImage(string image)
      {
         return new ReportImage(Id, Placement, image);
      }

      public override ReportImage WithPosition(int x, int y)
      {
         return new ReportImage(Id, Placement.WithPosition(x, y), Image);
      }

      public override ReportImage WithSize(int widht, int height)
      {
         return new ReportImage(Id, Placement.WithSize(widht, height), Image);
      }
   }
}