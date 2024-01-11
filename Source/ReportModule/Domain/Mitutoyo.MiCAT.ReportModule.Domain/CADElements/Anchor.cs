// <copyright file="Anchor.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using Mitutoyo.MiCAT.Core.Geometry;

namespace Mitutoyo.MiCAT.ReportModule.Domain.CADElements
{
   public class Anchor
   {
      public Point3D LeaderLineOrigin { get; }

      public Anchor()
      {
         LeaderLineOrigin = new Point3D();
      }

      public Anchor(Anchor anchorData)
      {
         LeaderLineOrigin = anchorData.LeaderLineOrigin;
      }

      public Anchor(Point3D leaderLineOrigin)
      {
         LeaderLineOrigin = leaderLineOrigin;
      }
   }
}
