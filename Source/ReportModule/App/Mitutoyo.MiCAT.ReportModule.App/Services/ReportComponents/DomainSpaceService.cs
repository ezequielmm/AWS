// <copyright file="DomainSpaceService.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Collections.Generic;
using System.Linq;
using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.ReportModule.Domain.Components;

namespace Mitutoyo.MiCAT.ReportModuleApp.Services.ReportComponents
{
   public class DomainSpaceService : IDomainSpaceService
   {
      public ISnapShot AddSpace(ISnapShot snapShot, int yPosition, int ySize, IEnumerable<IItemId<IReportComponent>> componentsNotBeingMoved)
      {
         var componentsBellowNewSpace = snapShot
            .GetItems<IReportComponent>()
            .Where(rc =>
            rc.Placement.Y > yPosition && !componentsNotBeingMoved.Contains(rc.Id));

         foreach (var componentBellowNewSpace in componentsBellowNewSpace)
         {
            snapShot = snapShot.UpdateItem(componentBellowNewSpace, componentBellowNewSpace.WithPosition(componentBellowNewSpace.Placement.X, componentBellowNewSpace.Placement.Y + ySize));
         }

         return snapShot;
      }
   }
}
