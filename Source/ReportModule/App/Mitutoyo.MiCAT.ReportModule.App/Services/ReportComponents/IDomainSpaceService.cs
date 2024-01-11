// <copyright file="IDomainSpaceService.cs" company="Mitutoyo Europe GmbH">
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
   public interface IDomainSpaceService
   {
      ISnapShot AddSpace(ISnapShot snapShot, int yPosition, int ySize, IEnumerable<IItemId<IReportComponent>> componentsNotBeingMoved);
   }
}
