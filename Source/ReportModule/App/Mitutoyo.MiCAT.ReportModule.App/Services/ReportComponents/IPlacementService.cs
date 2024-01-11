// <copyright file="IPlacementService.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.ReportModule.Domain.Components;

namespace Mitutoyo.MiCAT.ReportModuleApp.Services.ReportComponents
{
   public interface IPlacementService
   {
      ISnapShot SetResize(ISnapShot snapShot, IItemId<IReportComponent> reportComponentId, int x, int y, int width, int height);
      ISnapShot SetResize(ISnapShot snapShot, IItemId<IReportComponent> reportComponentId, int width, int height);
      ISnapShot SetResizeOnFakeSpace(ISnapShot snapShot, IItemId<IReportComponent> reportComponentId, int x, int y, int width, int height, int yFakeSpaceBegin, int heightFakeSpace);
      ISnapShot SetPosition(ISnapShot snapShot, IItemId<IReportComponent> reportComponentId, int x, int y);
      ISnapShot SetPositionOnFakeSpace(ISnapShot snapShot, IItemId<IReportComponent> reportComponentId, int x, int y, int yFakeSpaceBegin, int heightFakeSpace);
   }
}
