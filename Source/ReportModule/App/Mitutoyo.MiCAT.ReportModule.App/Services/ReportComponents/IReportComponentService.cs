// <copyright file="IReportComponentService.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Collections.Generic;
using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.ReportModule.Domain.Components;

namespace Mitutoyo.MiCAT.ReportModuleApp.Services.ReportComponents
{
   public interface IReportComponentService
   {
      ISnapShot AddComponent(ISnapShot snapShot, IReportComponent reportComponent);
      ISnapShot AddComponent(ISnapShot snapShot, IReportComponent reportComponent, IEnumerable<IStateItem> extraElementsToAdd);
      ISnapShot AddComponentOnFakeSpace(ISnapShot newSnapShot, IReportComponent reportComponent, int yFakeSpaceBegin, int heightFakeSpace);
      ISnapShot AddComponentOnFakeSpace(ISnapShot newSnapShot, IReportComponent reportComponent, IEnumerable<IStateItem> extraElementsToAdd, int yFakeSpaceBegin, int heightFakeSpace);
   }
}
