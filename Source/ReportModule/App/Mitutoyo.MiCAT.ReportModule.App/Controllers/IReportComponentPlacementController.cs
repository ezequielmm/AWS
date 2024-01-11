// <copyright file="IReportComponentPlacementController.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.ReportModule.Domain.Components;

namespace Mitutoyo.MiCAT.ReportModuleApp.Controllers
{
   public interface IReportComponentPlacementController
   {
      void SetPosition(IItemId<IReportComponent> reportComponentId, int x, int y);
      void SetPositionOnFakeSpace(IItemId<IReportComponent> reportComponentId, int x, int y, int yFakeSpaceBegin, int heightFakeSpace);
      void SetResize(IItemId<IReportComponent> reportComponentId, int x, int y, int width, int height);
      void SetResize(IItemId<IReportComponent> reportComponentId, int width, int height);
      void SetResizeOnFakeSpace(IItemId<IReportComponent> reportComponentId, int x, int y, int width, int height, int yFakeSpaceBegin, int heightFakeSpace);
   }
}
