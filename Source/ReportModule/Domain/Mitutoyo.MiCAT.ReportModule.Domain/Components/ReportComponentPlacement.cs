// <copyright file="ReportComponentPlacement.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using Mitutoyo.MiCAT.ApplicationState;

namespace Mitutoyo.MiCAT.ReportModule.Domain.Components
{
   public class ReportComponentPlacement
   {
      public ReportComponentPlacement(IItemId reportSectionId, int x, int y, int width, int height)
      {
         ReportSectionId = reportSectionId;
         X = x;
         Y = y;
         Width = width;
         Height = height;
      }

      public IItemId ReportSectionId { get; }
      public int X { get; }
      public int Y { get; }
      public int Width { get; }
      public int Height { get; }

      public ReportComponentPlacement WithPosition(int x, int y)
      {
         return new ReportComponentPlacement(ReportSectionId, x, y, Width, Height);
      }

      public ReportComponentPlacement WithSize(int width, int height)
      {
         return new ReportComponentPlacement(ReportSectionId, X, Y, width, height);
      }

      public bool IsSamePosition(ReportComponentPlacement placementToCompareWith)
      {
         return ReportSectionId == placementToCompareWith.ReportSectionId &&
                X == placementToCompareWith.X &&
                Y == placementToCompareWith.Y;
      }
      public bool IsSameSize(ReportComponentPlacement placementToCompareWith)
      {
         return Width == placementToCompareWith.Width &&
                Height == placementToCompareWith.Height;
      }
   }
}
