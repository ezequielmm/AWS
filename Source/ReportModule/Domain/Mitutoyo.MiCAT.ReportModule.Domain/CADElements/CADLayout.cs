// <copyright file="CADLayout.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using System.Collections.Generic;
using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.ReportModule.Domain.CADElements.Callouts2D;
using Mitutoyo.MiCAT.ReportModule.Domain.CADElements.Callouts3D;

namespace Mitutoyo.MiCAT.ReportModule.Domain.CADElements
{
   public class CADLayout : BaseStateEntity<CADLayout>
   {
      public Guid PlanID { get; }
      public Camera Camera { get; }
      public List<Callout2D> Callouts2D { get; }
      public List<Callout3D> Callouts3D { get; }

      public CADLayout() : this(Guid.NewGuid(), Guid.Empty, new Camera(), new List<Callout2D>(), new List<Callout3D>())
      {
      }

      public CADLayout(Id<CADLayout> id, Guid planID, Camera camera, List<Callout2D> callouts2D, List<Callout3D> callouts3D)
      :base(id)
      {
         PlanID = planID;
         Camera = camera;
         Callouts2D = callouts2D;
         Callouts3D = callouts3D;
      }
   }
}
