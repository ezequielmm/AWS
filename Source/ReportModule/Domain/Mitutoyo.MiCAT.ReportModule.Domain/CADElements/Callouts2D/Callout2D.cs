// <copyright file="Callout2D.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using Mitutoyo.MiCAT.Core.Geometry;

namespace Mitutoyo.MiCAT.ReportModule.Domain.CADElements.Callouts2D
{
   public abstract class Callout2D
   {
      public Anchor Anchor { get; protected set; }
      public Point2D Position { get; protected set; }
   }
}