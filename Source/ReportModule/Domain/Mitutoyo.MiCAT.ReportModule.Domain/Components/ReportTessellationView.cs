// <copyright file="ReportTessellationView.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.ReportModule.Domain.CADElements;

namespace Mitutoyo.MiCAT.ReportModule.Domain.Components
{
   public class ReportTessellationView : ReportComponentBase<ReportTessellationView>, IReportComponent
   {
      public Id<CADLayout> CADLayoutId { get; set; }

      public ReportTessellationView(ReportComponentPlacement placement)
         : base(Guid.NewGuid(), placement)
      {
      }

      public override ReportTessellationView WithPosition(int x, int y)
      {
         throw new NotImplementedException();
      }

      public override ReportTessellationView WithSize(int widht, int height)
      {
         throw new NotImplementedException();
      }
   }
}
