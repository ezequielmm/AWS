// <copyright file="ReportComponentFake.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using Mitutoyo.MiCAT.ReportModule.Domain.Components;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Test.VisualElements.ReportElements
{
   [ExcludeFromCodeCoverage]
   public class ReportComponentFake : ReportComponentBase<ReportComponentFake>
   {
      public ReportComponentFake(ReportComponentPlacement placement)
         : base(System.Guid.NewGuid(), placement)
      {
      }

      public override ReportComponentFake WithPosition(int x, int y)
      {
         return new ReportComponentFake(Placement.WithPosition(x, y));
      }

      public override ReportComponentFake WithSize(int widht, int height)
      {
         return new ReportComponentFake(Placement.WithSize(widht, height));
      }
   }
}
