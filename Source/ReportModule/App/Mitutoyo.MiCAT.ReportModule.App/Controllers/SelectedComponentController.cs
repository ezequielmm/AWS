// <copyright file="SelectedComponentController.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Linq;
using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.ReportModule.Domain.Components;
using Mitutoyo.MiCAT.ReportModule.Domain.Data;

namespace Mitutoyo.MiCAT.ReportModuleApp.Controllers
{
   public class SelectedComponentController : ISelectedComponentController
   {
      private readonly IAppStateHistory _appStateHistory;

      public SelectedComponentController(IAppStateHistory appStateHistory)
      {
         _appStateHistory = appStateHistory;
      }

      public void SetBoundarySectionComponentSelected(IItemId<IReportComponent> reportComponentId)
      {
         var componentSelection = _appStateHistory.CurrentSnapShot.GetItems<ReportComponentSelection>().SingleOrDefault();
         if (!componentSelection.SelectedReportComponentIds.Contains(reportComponentId))
            _appStateHistory.Run(snapShot => SetBoundarySectionComponentSelected(snapShot, reportComponentId, componentSelection));
      }

      private ISnapShot SetBoundarySectionComponentSelected(ISnapShot snapShot, IItemId<IReportComponent> reportComponentId, ReportComponentSelection currentReportComponentSelection)
      {
         return snapShot.UpdateItem(currentReportComponentSelection, currentReportComponentSelection.WithJustThisSelectedComponent(reportComponentId));
      }

      public void SetSelected(IItemId<IReportComponent> reportComponentId)
      {
         var componentSelection = _appStateHistory.CurrentSnapShot.GetItems<ReportComponentSelection>().SingleOrDefault();
         if (!componentSelection.SelectedReportComponentIds.Contains(reportComponentId))
            _appStateHistory.Run(snapShot => SetSelected(snapShot, reportComponentId, componentSelection));
      }

      private ISnapShot SetSelected(ISnapShot snapShot, IItemId<IReportComponent> reportComponentId, ReportComponentSelection currentReportComponentSelection)
      {
         return snapShot.UpdateItem(currentReportComponentSelection, currentReportComponentSelection.WithJustThisSelectedComponent(reportComponentId));
      }

      public void UnSelectAll()
      {
         var componentSelection = _appStateHistory.CurrentSnapShot.GetItems<ReportComponentSelection>().SingleOrDefault();
         if ((componentSelection != null) && (componentSelection.SelectedReportComponentIds.Any()))
            _appStateHistory.Run(snapShot => UnSelectAll(snapShot, componentSelection));
      }

      private ISnapShot UnSelectAll(ISnapShot snapShot, ReportComponentSelection currentReportComponentSelection)
      {
         return snapShot.UpdateItem(currentReportComponentSelection, currentReportComponentSelection.WithNonSelectedComponent());
      }
   }
}