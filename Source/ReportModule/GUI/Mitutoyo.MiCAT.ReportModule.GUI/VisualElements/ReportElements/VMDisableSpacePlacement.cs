// <copyright file="VMDisableSpacePlacement.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.ReportModule.Domain.Components;
using Mitutoyo.MiCAT.ReportModule.GUI.Common.DisabledSpace;
using Mitutoyo.MiCAT.ReportModule.GUI.Common.LayoutCalculator;
using Mitutoyo.MiCAT.ReportModuleApp.Controllers;

namespace Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements
{
   public class VMDisableSpacePlacement : VMReportBodyPlacement, IVMDisableSpacePlacement
   {
      public VMDisableSpacePlacement(IItemId<IReportComponent> reportComponentId,
         ReportComponentPlacement placement,
         IReportComponentPlacementController placementController,
         ISelectedComponentController selectedComponentController,
         IRenderedData renderedData)
         : base(reportComponentId, placement, placementController, selectedComponentController, renderedData)
      {
      }

      public DisabledSpaceData LastDisabledSpaceGenerated { get; set; }

      protected override int ConvertVisualToDomain(int visualY)
      {
         return RenderedData.ConvertToDomainY(visualY, this.LastDisabledSpaceGenerated);
      }

      protected override bool IsFakeSpace(int visualY)
      {
         if (IsMovingDown(visualY))
            return false;
         else
            return base.IsFakeSpace(visualY);
      }

      protected override bool IsValidPosition(int visualY)
      {
         return !RenderedData.IsInDisabledSpace(visualY, this.LastDisabledSpaceGenerated);
      }
   }
}
