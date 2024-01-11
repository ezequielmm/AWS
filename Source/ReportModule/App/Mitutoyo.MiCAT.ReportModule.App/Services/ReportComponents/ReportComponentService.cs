// <copyright file="ReportComponentService.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.Core.StdLib.Extensions;
using Mitutoyo.MiCAT.ReportModule.Domain.Components;
using Mitutoyo.MiCAT.ReportModule.Domain.Data;

namespace Mitutoyo.MiCAT.ReportModuleApp.Services.ReportComponents
{
   public class ReportComponentService : IReportComponentService
   {
      private readonly IDomainSpaceService _domainSpaceService;

      public ReportComponentService(IDomainSpaceService domainSpaceService)
      {
         _domainSpaceService = domainSpaceService;
      }

      public ISnapShot AddComponent(ISnapShot snapShot, IReportComponent reportComponent)
      {
         return AddComponent(snapShot, reportComponent, Array.Empty<IStateItem>());
      }

      public ISnapShot AddComponent(ISnapShot snapShot, IReportComponent reportComponent, IEnumerable<IStateItem> extraElementsToAdd)
      {
         extraElementsToAdd.ForEach(x => snapShot = snapShot.AddItem(x));
         snapShot = snapShot.AddItem(reportComponent);
         snapShot = SetComponentAsSelected(reportComponent, snapShot);

         return snapShot;
      }

      public ISnapShot AddComponentOnFakeSpace(ISnapShot newSnapShot, IReportComponent reportComponent, int yFakeSpaceBegin, int heightFakeSpace)
      {
         return AddComponentOnFakeSpace(newSnapShot, reportComponent, Array.Empty<IStateItem>(), yFakeSpaceBegin, heightFakeSpace);
      }

      public ISnapShot AddComponentOnFakeSpace(ISnapShot newSnapShot, IReportComponent reportComponent, IEnumerable<IStateItem> extraElementsToAdd, int yFakeSpaceBegin, int heightFakeSpace)
      {
         newSnapShot = _domainSpaceService.AddSpace(newSnapShot, yFakeSpaceBegin, heightFakeSpace, Array.Empty<IItemId<IReportComponent>>());
         newSnapShot = AddComponent(newSnapShot, reportComponent, extraElementsToAdd);
         return newSnapShot;
      }

      private ISnapShot SetComponentAsSelected(IReportComponent reportComponent, ISnapShot snapShot)
      {
         var currentSelection = snapShot.GetItems<ReportComponentSelection>().SingleOrDefault();
         return snapShot.UpdateItem(currentSelection, currentSelection.WithJustThisSelectedComponent(reportComponent.Id as IItemId<IReportComponent>));
      }
   }
}
