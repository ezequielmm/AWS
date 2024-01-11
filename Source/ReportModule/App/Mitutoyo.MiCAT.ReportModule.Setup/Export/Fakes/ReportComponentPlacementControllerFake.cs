// <copyright file="ReportComponentPlacementControllerFake.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.ReportModule.Domain.Components;
using Mitutoyo.MiCAT.ReportModuleApp.Controllers;

namespace Mitutoyo.MiCAT.ReportModule.Setup.Export.Fakes
{
   [ExcludeFromCodeCoverage]
   internal class ReportComponentPlacementControllerFake : IReportComponentPlacementController
   {
      public void SetPosition(IItemId<IReportComponent> reportComponentId, int x, int y)
      {
      }

      public void SetPositionOnFakeSpace(IItemId<IReportComponent> reportComponentId, int x, int y, int yFakeSpaceBegin, int heightFakeSpace)
      {
      }

      public void SetResize(IItemId<IReportComponent> reportComponentId, int x, int y, int width, int height)
      {
      }

      public void SetResize(IItemId<IReportComponent> reportComponentId, int width, int height)
      {
      }

      public void SetResizeOnFakeSpace(IItemId<IReportComponent> reportComponentId, int x, int y, int width, int height, int yFakeSpaceBegin, int heightFakeSpace)
      {
      }
   }
}
