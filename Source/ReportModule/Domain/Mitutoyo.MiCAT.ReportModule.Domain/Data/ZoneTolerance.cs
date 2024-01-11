﻿// <copyright file="ZoneTolerance.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

namespace Mitutoyo.MiCAT.ReportModule.Domain.Data
{
   public class ZoneTolerance: ITolerance
   {
      public double ToleranceZone { get; }
      public ZoneTolerance() : this(0D)
      {
      }
      public ZoneTolerance(double toleranceZone)
      {
         ToleranceZone = toleranceZone;
      }

      public ZoneTolerance WithToleranceZone(double toleranceZone)
      {
         return new ZoneTolerance(toleranceZone);
      }
   }
}
