// <copyright file="CameraSO.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

namespace Mitutoyo.MiCAT.ReportModule.Persistence.CADElements
{
   public class CameraSO
   {
      public Point3DSO Position { get; set; }
      public Point3DSO Target { get; set; }
      public Direction3DSO UpVector { get; set; }
      public float FieldHeight { get; set; }
      public float FieldWidht { get; set; }
      public bool PerspectiveProjection { get; set; }
   }
}
