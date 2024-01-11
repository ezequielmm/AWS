// <copyright file="SelectedSectionController.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Collections.Generic;
using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.ReportModuleApp.AppState.Extensions;

namespace Mitutoyo.MiCAT.ReportModuleApp.Controllers
{
   public class SelectedSectionController : ISelectedSectionController
   {
      private readonly IAppStateHistory _appStateHistory;

      public SelectedSectionController(IAppStateHistory appStateHistory)
      {
         _appStateHistory = appStateHistory;
      }

      public void SelectPageBodySection()
      {
         _appStateHistory.Run((snapShot) => SelectPageBodySection(snapShot));
      }
      private ISnapShot SelectPageBodySection(ISnapShot snapShot)
      {
         var sectionSelection = snapShot.GetReportSectionSelection();
         var selectedSections = new List<IItemId>()
         {
            snapShot.GetReportBodyId()
         };
         return snapShot.UpdateItem(sectionSelection, sectionSelection.WithNewSelectedSectionsList(selectedSections));
      }

      public void SelectReportBoundarySections()
      {
         _appStateHistory.Run(snapShot => SelectReportBoundarySections(snapShot));
      }
      private ISnapShot SelectReportBoundarySections(ISnapShot snapShot)
      {
         var sectionSelection = snapShot.GetReportSectionSelection();
         var selectedSections = new List<IItemId>()
         {
            snapShot.GetReportHeaderId(),
            snapShot.GetReportFooterId(),
         };
         return snapShot.UpdateItem(sectionSelection, sectionSelection.WithNewSelectedSectionsList(selectedSections));
      }
   }
}
