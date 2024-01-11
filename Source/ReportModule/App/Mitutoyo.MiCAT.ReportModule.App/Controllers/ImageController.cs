// <copyright file="ImageController.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.ReportModule.Domain.Components;
using Mitutoyo.MiCAT.ReportModuleApp.AppState.Extensions;
using Mitutoyo.MiCAT.ReportModuleApp.Services.ReportComponents;

namespace Mitutoyo.MiCAT.ReportModuleApp.Controllers
{
   public class ImageController : IImageController
   {
      private readonly IAppStateHistory _appStateHistory;
      private readonly IReportComponentService _reportComponentService;

      public ImageController(IAppStateHistory appStateHistory, IReportComponentService reportComponentService)
      {
         _appStateHistory = appStateHistory;
         _reportComponentService = reportComponentService;
      }

      public void AddImageToBoundarySection(IItemId reportSectionId, int x, int y)
      {
         AddImage(ReportImage.CreateDefaultForBoundarySection(reportSectionId, x, y));
      }

      public void AddImageToBody(int x, int y)
      {
         AddImage(ReportImage.CreateDefaultForBody(_appStateHistory.CurrentSnapShot.GetReportBodyId(), x, y));
      }

      public void AddImageOnFakeSpace(int x, int y, int fakeSpaceStartPosition, int fakeSpaceHeight)
      {
         var reportImage = ReportImage.CreateDefaultForBody(_appStateHistory.CurrentSnapShot.GetReportBodyId(), x, y);

         _appStateHistory.RunUndoable(s => _reportComponentService.AddComponentOnFakeSpace(s, reportImage, fakeSpaceStartPosition, fakeSpaceHeight));
      }

      public void UpdateImage(Id<ReportImage> id, string image, int width, int height)
      {
         _appStateHistory.RunUndoable(s => UpdateImage(s, id, image, width, height));
      }

      private void AddImage(ReportImage reportImage)
      {
         _appStateHistory.RunUndoable(s => _reportComponentService.AddComponent(s, reportImage));
      }

      private ISnapShot UpdateImage(ISnapShot snapshot, Id<ReportImage> id, string image, int width, int height)
      {
         var currentImageComponent = _appStateHistory.CurrentSnapShot.GetItem(id);

         snapshot = snapshot.UpdateItem(currentImageComponent, currentImageComponent.WithSize(width, height).WithImage(image));

         return snapshot;
      }
   }
}