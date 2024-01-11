// <copyright file="Callout3DSO.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

namespace Mitutoyo.MiCAT.ReportModule.Persistence.CADElements.Callouts3D
{
   public class Callout3DSO
   {
      public AnchorSO Anchor { get; set; }
      public Point3DSO Position { get; set; }
   }
}
