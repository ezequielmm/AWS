// <copyright file="IVMReportComponentPlacement.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using Mitutoyo.MiCAT.ReportModule.Domain.Components;

namespace Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements
{
   public interface IVMReportComponentPlacement : IVMResizablePlacement
   {
      int DomainX { get; }
      int DomainY { get; }
      int DomainHeight { get; }
      int DomainWidth { get; }
      void UpdateSelectionState(bool newIsSelectedValue);
      void UpdateFromPlacementEntity(ReportComponentPlacement placementBefore, ReportComponentPlacement placementAfter);
   }
}
