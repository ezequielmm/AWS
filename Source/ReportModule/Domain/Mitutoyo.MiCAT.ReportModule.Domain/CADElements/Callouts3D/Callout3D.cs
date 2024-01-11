// <copyright file="Callout3D.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using Mitutoyo.MiCAT.Core.Geometry;

namespace Mitutoyo.MiCAT.ReportModule.Domain.CADElements.Callouts3D
{
   public class Callout3D
   {
      public Anchor Anchor { get; }
      public Point3D Position { get; }

      public Callout3D(Anchor anchor, Point3D position)
      {
         Anchor = anchor;
         Position = position;
      }
   }
}
