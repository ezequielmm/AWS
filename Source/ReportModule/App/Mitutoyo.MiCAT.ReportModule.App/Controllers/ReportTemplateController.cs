// <copyright file="ReportTemplateController.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mitutoyo.MiCAT.Applications.Services.Core;
using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.ReportModule.Domain.CADElements;
using Mitutoyo.MiCAT.ReportModule.Domain.Data;
using Mitutoyo.MiCAT.ReportModule.Domain.DataResult;
using Mitutoyo.MiCAT.ReportModule.Domain.DataResult.Exceptions;
using Mitutoyo.MiCAT.ReportModule.Persistence;
using Mitutoyo.MiCAT.ReportModuleApp.AppState;
using Mitutoyo.MiCAT.ReportModuleApp.AppState.Extensions;
using Mitutoyo.MiCAT.ReportModuleApp.Providers;
using Mitutoyo.MiCAT.ReportModuleApp.Providers.Inputs;
using Mitutoyo.MiCAT.ReportModuleApp.Utilities;

namespace Mitutoyo.MiCAT.ReportModuleApp.Controllers
{
   public class ReportTemplateController : IReportTemplateController
   {
      private readonly IReportTemplatePersistence _reportTemplatePersistence;
      private readonly IAppStateHistory _history;
      private readonly ITemplateNameResolver _templateNameResolver;
      private readonly IUnsavedChangesService _unsavedChangesService;
      private readonly IMessageNotifier _notifier;
      private readonly IReportTemplateDeleteConfirmationInput _reportTemplateDeleteConfirmationInput;

      public ReportTemplateController(
         IReportTemplatePersistence reportTemplatePersistence,
         IAppStateHistory history,
         ITemplateNameResolver templateNameResolver,
         IUnsavedChangesService unsavedChangesService,
         IMessageNotifier notifier,
         IReportTemplateDeleteConfirmationInput reportTemplateConfirmationInput)
      {
         _reportTemplatePersistence = reportTemplatePersistence;
         _history = history;
         _templateNameResolver = templateNameResolver;
         _unsavedChangesService = unsavedChangesService;
         _notifier = notifier;
         _reportTemplateDeleteConfirmationInput = reportTemplateConfirmationInput;
      }

      public async Task<IEnumerable<ReportTemplateDescriptor>> GetReportTemplateDescriptorsForCreate()
      {
         try
         {
            return await _reportTemplatePersistence.GetReportTemplatesDescriptors();
         }
         catch (ResultException ex)
         {
            _notifier.NotifyError(ex);
            return new ReportTemplateDescriptor[] { };
         }
      }

      public async Task<IEnumerable<ReportTemplateDescriptor>> GetReportTemplateDescriptorsForEdit()
      {
         try
         {
            var reportTemplates = await _reportTemplatePersistence.GetReportTemplatesDescriptors();
            var reportTemplateDefaults = _reportTemplatePersistence.GetDefaultReportTemplateDescriptors();

            return reportTemplates.Concat(reportTemplateDefaults);
         }
         catch (ResultException ex)
         {
            _notifier.NotifyError(ex);
            return new ReportTemplateDescriptor[] { };
         }
      }

      public async Task<ReportTemplate> GetReportTemplateById(Guid guid)
      {
         try
         {
            return _reportTemplatePersistence.GetDefaultReportTemplate(guid) ?? await _reportTemplatePersistence.GetReportTemplate(guid);
         }
         catch (ResultException ex)
         {
            _notifier.NotifyError(ex);
            return null;
         }
      }

      public ReportTemplate GetCurrentReportTemplate()
      {
         return new ReportTemplate()
         {
            CommonPageLayout = _history.CurrentSnapShot.GetItems<CommonPageLayout>().SingleOrDefault(),
            CadLayouts = _history.CurrentSnapShot.GetItems<CADLayout>(),
            TemplateDescriptor = _history.CurrentSnapShot.GetItems<TemplateDescriptorState>().SingleOrDefault()?.TemplateDescriptor,
            ReportComponents = _history.CurrentSnapShot.GetAllReportComponentsOnBody(),
            ReportComponentDataItems = _history.CurrentSnapShot.GetAllChildReportComponentItemsOnBody(),
         };
      }

      public async Task<bool> DeleteReportTemplate(Guid reportTemplateId)
      {
         if (!ConfirmDeleteReportTemplate()) return false;
         try
         {
            await _reportTemplatePersistence.DeleteReportTemplate(reportTemplateId);
         }
         catch (ResultException ex)
         {
            _notifier.NotifyError(ex);
            return false;
         }
         return true;
      }
      private bool ConfirmDeleteReportTemplate()
      {
         return _reportTemplateDeleteConfirmationInput.ConfirmDeleteReportTemplate();
      }
      public async Task<SaveAsResult> SaveCurrentTemplate()
      {
         var reportTemplate = GetCurrentReportTemplate();

         if (reportTemplate.TemplateDescriptor.ReadOnly)
         {
            return await SaveTemplateAs(reportTemplate);
         }

         try
         {
            await _reportTemplatePersistence.UpdateTemplate(reportTemplate);
         }
         catch (ReportTemplateNotFoundException)
         {
            return await SaveTemplateAs(reportTemplate);
         }
         catch (ResultException ex)
         {
            _notifier.NotifyError(ex);
            return SaveAsResult.Canceled;
         }

         UpdateUnsavedChangesSnapshot();

         return SaveAsResult.Saved;
      }

      public async Task<SaveAsResult> SaveAsCurrentTemplate()
      {
         var reportTemplate = GetCurrentReportTemplate();
         return await SaveTemplateAs(reportTemplate);
      }

      private async Task<SaveAsResult> SaveTemplateAs(ReportTemplate reportTemplate)
      {
         var newName = _templateNameResolver.QueryTemplateName();
         if (newName.IsCanceled)
            return SaveAsResult.Canceled;

         reportTemplate.TemplateDescriptor.Name = newName.Name;

         try
         {
            var reportTemplateResult = await _reportTemplatePersistence.AddTemplate(reportTemplate);
            _history.Run(snapShot => UpdateTemplateDescriptorSnapShot(snapShot, reportTemplateResult));
         }
         catch (ResultException ex)
         {
            _notifier.NotifyError(ex);
            return SaveAsResult.Canceled;
         }

         UpdateUnsavedChangesSnapshot();

         return SaveAsResult.Saved;
      }

      private ISnapShot UpdateTemplateDescriptorSnapShot(ISnapShot snapShot, ReportTemplate reportTemplate)
      {
         var templateDescriptorState = snapShot.GetItems<TemplateDescriptorState>().SingleOrDefault();
         return snapShot.UpdateItem(templateDescriptorState, templateDescriptorState.With(reportTemplate.TemplateDescriptor));
      }

      private void UpdateUnsavedChangesSnapshot()
      {
         _unsavedChangesService.SaveSnapShot(_history.CurrentSnapShot);
      }
   }
}
