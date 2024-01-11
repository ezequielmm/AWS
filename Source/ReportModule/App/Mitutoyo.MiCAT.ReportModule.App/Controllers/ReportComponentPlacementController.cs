// <copyright file="ReportComponentPlacementController.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.ReportModule.Domain.Components;
using Mitutoyo.MiCAT.ReportModuleApp.Services.ReportComponents;

namespace Mitutoyo.MiCAT.ReportModuleApp.Controllers
{
   public class ReportComponentPlacementController : IReportComponentPlacementController
   {
      private readonly IPlacementService _placementService;
      private readonly IAppStateHistory _appStateHistory;

      public ReportComponentPlacementController(IAppStateHistory appStateHistory, IPlacementService placementService)
      {
         _appStateHistory = appStateHistory;
         _placementService = placementService;
      }

      public void SetResize(IItemId<IReportComponent> reportComponentId, int x, int y, int width, int height)
      {
         _appStateHistory.RunUndoable(snapShot => _placementService.SetResize(snapShot, reportComponentId, x, y, width, height));
      }

      public void SetResize(IItemId<IReportComponent> reportComponentId, int width, int height)
      {
         _appStateHistory.RunUndoable(snapShot => _placementService.SetResize(snapShot, reportComponentId, width, height));
      }

      public void SetResizeOnFakeSpace(IItemId<IReportComponent> reportComponentId, int x, int y, int width, int height, int yFakeSpaceBegin, int heightFakeSpace)
      {
         _appStateHistory.RunUndoable(snapShot => _placementService.SetResizeOnFakeSpace(snapShot, reportComponentId, x, y, width, height, yFakeSpaceBegin, heightFakeSpace));
      }

      public void SetPosition(IItemId<IReportComponent> reportComponentId, int x, int y)
      {
         _appStateHistory.RunUndoable(snapShot => _placementService.SetPosition(snapShot, reportComponentId, x, y));
      }

      public void SetPositionOnFakeSpace(IItemId<IReportComponent> reportComponentId, int x, int y, int yFakeSpaceBegin, int heightFakeSpace)
      {
         _appStateHistory.RunUndoable(snapShot => _placementService.SetPositionOnFakeSpace(snapShot, reportComponentId, x, y, yFakeSpaceBegin, heightFakeSpace));
      }
   }
}
