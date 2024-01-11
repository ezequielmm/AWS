// <copyright file="Direction3D.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

namespace Mitutoyo.MiCAT.ReportModule.Domain.CADElements
{
   public class Direction3D
   {
      public double X { get; set; }
      public double Y { get; set; }
      public double Z { get; set; }

      public Direction3D() : this(1, -1, 1)
      {
      }

      public Direction3D(double x, double y, double z)
      {
         X = x;
         Y = y;
         Z = z;
      }
   }
}
