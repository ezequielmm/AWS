// <copyright file="PlacementService.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using System.Collections.Generic;
using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.ReportModule.Domain.Components;

namespace Mitutoyo.MiCAT.ReportModuleApp.Services.ReportComponents
{
   public class PlacementService : IPlacementService
   {
      private readonly IDomainSpaceService _domainSpaceService;

      public PlacementService(IDomainSpaceService domainSpaceService)
      {
         _domainSpaceService = domainSpaceService;
      }

      public ISnapShot SetResize(ISnapShot snapShot, IItemId<IReportComponent> reportComponentId, int width, int height)
      {
         return UpdateSnapShot(snapShot, reportComponentId,
            ph => CheckDiffSize(ph, width, height),
            ph => ph.WithSize(width, height));
      }

      public ISnapShot SetResize(ISnapShot snapShot, IItemId<IReportComponent> reportComponentId, int x, int y, int width, int height)
      {
         return UpdateSnapShot(snapShot, reportComponentId,
            ph => CheckDiffSizePosition(ph, x, y, width, height),
            ph => ph.WithPosition(x, y).WithSize(width, height));
      }

      public ISnapShot SetResizeOnFakeSpace(ISnapShot snapShot, IItemId<IReportComponent> reportComponentId, int x, int y, int width, int height, int yFakeSpaceBegin, int heightFakeSpace)
      {
         return UpdateSnapShotFakeSpace(snapShot, reportComponentId, yFakeSpaceBegin, heightFakeSpace,
            ph => CheckDiffSizePosition(ph, x, y, width, height),
            ph => ph.WithPosition(x, y).WithSize(width, height));
      }

      public ISnapShot SetPosition(ISnapShot snapShot, IItemId<IReportComponent> reportComponentId, int x, int y)
      {
         return UpdateSnapShot(snapShot, reportComponentId,
            ph => CheckDiffPosition(ph, x, y),
            ph => ph.WithPosition(x, y));
      }

      public ISnapShot SetPositionOnFakeSpace(ISnapShot snapShot, IItemId<IReportComponent> reportComponentId, int x, int y, int yFakeSpaceBegin, int heightFakeSpace)
      {
         return UpdateSnapShotFakeSpace(snapShot, reportComponentId, yFakeSpaceBegin, heightFakeSpace,
            ph => CheckDiffPosition(ph, x, y),
            ph => ph.WithPosition(x, y));
      }

      private ISnapShot UpdateSnapShot(ISnapShot snapShot, IItemId<IReportComponent> reportComponentId, Func<ReportComponentPlacement, bool> CheckDiffFunc, Func<IReportComponent, IReportComponent> UpdateReportComponentFunc)
      {
         if (snapShot.ContainsItem(reportComponentId))
         {
            var reportComponent = snapShot.GetItem(reportComponentId);

            if (CheckDiffFunc(reportComponent.Placement))
               snapShot = snapShot.UpdateItem(reportComponent, UpdateReportComponentFunc(reportComponent));
         }

         return snapShot;
      }

      private ISnapShot UpdateSnapShotFakeSpace(ISnapShot snapShot, IItemId<IReportComponent> reportComponentId, int yFakeSpaceBegin, int heightFakeSpace, Func<ReportComponentPlacement, bool> CheckDiffFunc, Func<IReportComponent, IReportComponent> UpdateReportComponentFunc)
      {
         if (snapShot.ContainsItem(reportComponentId))
         {
            var reportComponent = snapShot.GetItem(reportComponentId);

            if (CheckDiffFunc(reportComponent.Placement))
               snapShot = _domainSpaceService.AddSpace(snapShot, yFakeSpaceBegin, heightFakeSpace, new List<IItemId<IReportComponent>>() { reportComponentId });

            snapShot = snapShot.UpdateItem(reportComponent, UpdateReportComponentFunc(reportComponent));
         }

         return snapShot;
      }

      private bool CheckDiffPosition(ReportComponentPlacement placement, int x, int y)
      {
         return placement.X != x || placement.Y != y;
      }

      private bool CheckDiffSize(ReportComponentPlacement placement, int width, int height)
      {
         return placement.Width != width || placement.Height != height;
      }

      private bool CheckDiffSizePosition(ReportComponentPlacement placement, int x, int y, int width, int height)
      {
         return CheckDiffPosition(placement, x, y) || CheckDiffSize(placement, width, height);
      }
   }
}
