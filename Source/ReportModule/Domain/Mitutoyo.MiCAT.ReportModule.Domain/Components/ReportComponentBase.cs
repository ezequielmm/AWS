// <copyright file="ReportComponentBase.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Collections.Generic;
using Mitutoyo.MiCAT.ApplicationState;

namespace Mitutoyo.MiCAT.ReportModule.Domain.Components
{
   public abstract class ReportComponentBase<T> : BaseStateEntity<T>, IReportComponent where T : class, IReportComponent
   {
      protected ReportComponentBase(Id<T> id, ReportComponentPlacement placement)
      : base(id)
      {
         Placement = placement;
      }

      public ReportComponentPlacement Placement { get; }

      public override bool HasForeignIds => true;

      public override IEnumerable<IItemId> ForeignIds()
      {
         yield return Placement.ReportSectionId;
      }

      public abstract T WithPosition(int x, int y);

      public abstract T WithSize(int widht, int height);

      IReportComponent IReportComponent.WithPosition(int x, int y)
      {
         return WithPosition(x, y);
      }

      IReportComponent IReportComponent.WithSize(int widht, int height)
      {
         return WithSize(widht, height);
      }
   }
}
