// <copyright file="DeleteComponentController.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Linq;
using System.Threading.Tasks;
using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.Core.StdLib.Extensions;
using Mitutoyo.MiCAT.ReportModule.Domain.Components;
using Mitutoyo.MiCAT.ReportModule.Domain.Data;
using Mitutoyo.MiCAT.ReportModuleApp.AppState.Extensions;

namespace Mitutoyo.MiCAT.ReportModuleApp.Controllers
{
   public class DeleteComponentController : IDeleteComponentController
   {
      private readonly IAppStateHistory _history;

      public DeleteComponentController(IAppStateHistory history)
      {
         _history = history;
      }

      public Task DeleteSelectedComponents()
      {
         return _history.RunUndoableAsync((snapShot) => DeleteSelectedComponents(snapShot));
      }

      private ISnapShot DeleteSelectedComponents(ISnapShot snapShot)
      {
         var currentSelection = snapShot.GetSelectedReportComponentIds();

         if (currentSelection.Any())
         {
            foreach (var selectedReportComponentId in currentSelection)
            {
               snapShot = DeleteComponent(snapShot, selectedReportComponentId);
            }

            snapShot = UnselectAllComponents(snapShot);
         }
         return snapShot;
      }

      public Task DeleteComponent(IItemId<IReportComponent> selectedReportComponentId)
      {
         return _history.RunUndoableAsync(snapShot => DeleteComponent(snapShot, selectedReportComponentId));
      }

      private ISnapShot DeleteComponent(ISnapShot snapShot, IItemId<IReportComponent> reportComponentId)
      {
         snapShot = DeleteItemFromSnapShot(snapShot, reportComponentId);
         return UnselectComponent(snapShot, reportComponentId);
      }

      private ISnapShot DeleteItemFromSnapShot(ISnapShot snapShot, IItemId itemId)
      {
         var item = snapShot.GetItem(itemId);

         if (item is IReportComponentDataItemsContainer container)
            return DeleteComponentDataItemsFromSnapShot(snapShot, container);
         else
            return snapShot.DeleteItem(itemId);
      }
      private ISnapShot DeleteComponentDataItemsFromSnapShot(ISnapShot snapShot, IReportComponentDataItemsContainer childData)
      {
         childData.ReportComponentDataItemIds
            .ForEach(x => snapShot = DeleteItemFromSnapShot(snapShot, x));

         return snapShot.DeleteItem(childData);
      }

      private ISnapShot UnselectAllComponents(ISnapShot snapShot)
      {
         var reportComponentSelection = snapShot.GetItems<ReportComponentSelection>().SingleOrDefault();
         return snapShot.UpdateItem(reportComponentSelection, reportComponentSelection.WithNonSelectedComponent());
      }

      private ISnapShot UnselectComponent(ISnapShot snapShot, IItemId<IReportComponent> reportComponentId)
      {
         var reportComponentSelection = snapShot.GetItems<ReportComponentSelection>().SingleOrDefault();

         return snapShot.UpdateItem(reportComponentSelection, reportComponentSelection.WithThisIdComponentUnselected(reportComponentId));
      }
   }
}
