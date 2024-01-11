// <copyright file="TextBoxController.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.ReportModule.Domain.Components;
using Mitutoyo.MiCAT.ReportModuleApp.AppState.Extensions;
using Mitutoyo.MiCAT.ReportModuleApp.Services.ReportComponents;

namespace Mitutoyo.MiCAT.ReportModuleApp.Controllers
{
   public class TextBoxController : ITextBoxController
   {
      private readonly IAppStateHistory _appStateHistory;
      private readonly IReportComponentService _reportComponentService;

      public TextBoxController(IAppStateHistory appStateHistory, IReportComponentService reportComponentService)
      {
         _appStateHistory = appStateHistory;
         _reportComponentService = reportComponentService;
      }

      public void ModifyText(Id<ReportTextBox> reportTextBoxId, string text)
      {
         var reportTextBox = _appStateHistory.CurrentSnapShot.GetItem(reportTextBoxId);

         if (reportTextBox.Text != text)
            _appStateHistory.RunUndoable(snapShot => snapShot.UpdateItem(reportTextBox, reportTextBox.WithText(text)));
      }

      public void AddTextboxToBody(int x, int y)
      {
         AddTextboxToSection(_appStateHistory.CurrentSnapShot.GetReportBodyId(), x, y);
      }

      public void AddTextboxOnFakeSpace(int x, int y, int fakeSpaceStartPosition, int fakeSpaceHeight)
      {
         IReportComponent reportComponent = new ReportTextBox(_appStateHistory.CurrentSnapShot.GetReportBodyId(), x, y);
         _appStateHistory.RunUndoable(s => _reportComponentService.AddComponentOnFakeSpace(s, reportComponent, fakeSpaceStartPosition, fakeSpaceHeight));
      }

      public void AddTextboxToSection(IItemId sectionId, int x, int y)
      {
         IReportComponent reportComponent = new ReportTextBox(sectionId, x, y);
         _appStateHistory.RunUndoable(s => _reportComponentService.AddComponent(s, reportComponent));
      }
   }
}