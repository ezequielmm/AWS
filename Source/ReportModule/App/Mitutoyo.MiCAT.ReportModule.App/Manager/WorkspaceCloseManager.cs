// <copyright file="WorkspaceCloseManager.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Threading.Tasks;
using Mitutoyo.MiCAT.Applications.Services.Core;
using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.ReportModuleApp.Controllers;
using Mitutoyo.MiCAT.ReportModuleApp.Providers.Inputs;

namespace Mitutoyo.MiCAT.ReportModuleApp.Manager
{
   public class WorkspaceCloseManager : IWorkspaceCloseManager
   {
      private readonly IUnsavedChangesService _unsavedChangesService;
      private readonly IAppStateHistory _appStateHistory;
      private readonly ICloseWorkspaceConfirmationInput _closeWorkspaceConfirmationInput;
      private readonly IReportTemplateController _reportTemplateController;

      public WorkspaceCloseManager(
         IUnsavedChangesService unsavedChangesService,
         IAppStateHistory appStateHistory,
         ICloseWorkspaceConfirmationInput closeWOrkspaceConfirmationInput,
         IReportTemplateController reportTemplateController)
      {
         _unsavedChangesService = unsavedChangesService;
         _appStateHistory = appStateHistory;
         _closeWorkspaceConfirmationInput = closeWOrkspaceConfirmationInput;
         _reportTemplateController = reportTemplateController;
      }

      public async Task<WorkspaceClosingResult> OnWorkspaceClosing()
      {
         if (HasUnsavedChanges())
         {
            return await PromptForSavingChanges();
         }

         return WorkspaceClosingResult.ContinueClosing;
      }

      public bool HasUnsavedChanges()
      {
         return _unsavedChangesService.HaveUnsavedChanges(_appStateHistory.CurrentSnapShot);
      }

      private async Task<WorkspaceClosingResult> PromptForSavingChanges()
      {
         var confirmResult = _closeWorkspaceConfirmationInput.ConfirmCloseWorkspace();
         if (confirmResult == CloseWorkspaceConfirmationResult.SaveAndClose)
         {
            return await SaveChanges();
         }

         return confirmResult == CloseWorkspaceConfirmationResult.Close ?
            WorkspaceClosingResult.ContinueClosing :
            WorkspaceClosingResult.Abort;
      }

      private async Task<WorkspaceClosingResult> SaveChanges()
      {
         var saveResult = await _reportTemplateController.SaveCurrentTemplate();
         return saveResult == SaveAsResult.Canceled ?
            WorkspaceClosingResult.Abort :
            WorkspaceClosingResult.ContinueClosing;
      }
   }
}
