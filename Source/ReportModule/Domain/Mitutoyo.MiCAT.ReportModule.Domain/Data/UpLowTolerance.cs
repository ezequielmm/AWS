// <copyright file="UpLowTolerance.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

namespace Mitutoyo.MiCAT.ReportModule.Domain.Data
{
   public class UpLowTolerance: ITolerance
   {
      public double UpperTolerance { get; }
      public double LowerTolerance { get; }
      public UpLowTolerance() : this(0D, 0D)
      {
      }
      public UpLowTolerance(double upperTolerance, double lowerTolerance)
      {
         UpperTolerance = upperTolerance;
         LowerTolerance = lowerTolerance;
      }

      public UpLowTolerance WithUpperTolerance(double upperTolerance)
      {
         return new UpLowTolerance(upperTolerance, LowerTolerance);
      }

      public UpLowTolerance WithLowerTolerance(double lowerTolerance)
      {
         return new UpLowTolerance(UpperTolerance, lowerTolerance);
      }
   }
}
