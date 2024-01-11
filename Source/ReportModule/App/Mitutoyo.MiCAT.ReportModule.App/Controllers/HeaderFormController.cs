// <copyright file="HeaderFormController.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Collections.Generic;
using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.ReportModule.Domain.Components;
using Mitutoyo.MiCAT.ReportModuleApp.AppState.Extensions;
using Mitutoyo.MiCAT.ReportModuleApp.Services.ReportComponents;

namespace Mitutoyo.MiCAT.ReportModuleApp.Controllers
{
   public class HeaderFormController : IHeaderFormController
   {
      private readonly IAppStateHistory _appStateHistory;
      private readonly IReportComponentService _reportComponentService;

      public HeaderFormController(IAppStateHistory appStateHistory, IReportComponentService reportComponentService)
      {
         _appStateHistory = appStateHistory;
         _reportComponentService = reportComponentService;
      }

      public void AddHeaderFormToBody(int x, int y)
      {
         AddHeaderFormToSection(_appStateHistory.CurrentSnapShot.GetReportBodyId(), x, y);
      }

      public void AddHeaderFormOnFakeSpace(int x, int y, int fakeSpaceStartPosition, int fakeSpaceHeight)
      {
         var headerFormInfo = CreateHeaderForm(_appStateHistory.CurrentSnapShot.GetReportBodyId(), x, y);

         _appStateHistory.RunUndoable(snapShot => _reportComponentService.AddComponentOnFakeSpace(snapShot, headerFormInfo.HeaderForm, headerFormInfo.ExtraElements, fakeSpaceStartPosition, fakeSpaceHeight));
      }

      public void AddHeaderFormToSection(IItemId reportSectionId, int x, int y)
      {
         var headerFormInfo = CreateHeaderForm(reportSectionId , x, y);

         _appStateHistory.RunUndoable(s => _reportComponentService.AddComponent(s, headerFormInfo.HeaderForm, headerFormInfo.ExtraElements));
      }

      private (ReportHeaderForm HeaderForm, IEnumerable<IStateItem> ExtraElements) CreateHeaderForm(IItemId reportSectionId, int x, int y)
      {
         var field = new ReportHeaderFormField();
         var row = new ReportHeaderFormRow(new[] { field.Id, });
         return (new ReportHeaderForm(reportSectionId, x, y, new[] { row.Id, }), new List<IStateItem> { field, row, });
      }
   }
}
