// <copyright file="Callout2DSO.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

namespace Mitutoyo.MiCAT.ReportModule.Persistence.CADElements.Callouts2D
{
   public abstract class Callout2DSO
   {
      public AnchorSO Anchor { get; set; }
      public Point2DSO Position { get; set; }
   }
}
