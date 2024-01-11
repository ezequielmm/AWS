// <copyright file="VMSelectTemplateBase.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Mitutoyo.MiCAT.ReportModule.Domain.Data;
using Mitutoyo.MiCAT.ReportModule.GUI.Common;
using Mitutoyo.MiCAT.ReportModule.GUI.Properties;
using Mitutoyo.MiCAT.ReportModule.GUI.Strings;
using Mitutoyo.MiCAT.ReportModule.GUI.Utilities;
using Mitutoyo.MiCAT.ReportModule.GUI.Utilities.Events;
using Mitutoyo.MiCAT.ReportModuleApp.Controllers;
using Telerik.Windows.Controls;

namespace Mitutoyo.MiCAT.ReportModule.GUI.SelectTemplates
{
   public abstract class VMSelectTemplateBase : VMBase
   {
      private readonly IReportTemplateController _reportTemplateController;
      private bool _isOneSelected;
      private IEnumerable<ReportTemplateDescriptor> _reportTemplates;
      private ReportTemplateDescriptor _selectedTemplate;
      private Func<SelectedReportTemplateInfo, Task> _okAction;

      public VMSelectTemplateBase(IReportTemplateController reportTemplateController)
      {
         _reportTemplateController = reportTemplateController;
         _isOneSelected = false;

         SelectCommand = new AsyncCommand<Guid>(OnSelectClick);
         SelectionChangedCommand = new RelayCommand<RadTileView>(OnSelectChanged);
         DeleteSpecificTemplateCommand = new RelayCommand<ReportTemplateDescriptor>(OnDeleteSpecificReportTemplateCommand);
         DeleteSelectedTemplateCommand = new RelayCommand<object>(OnDeleteSelectedReportTemplateCommand);
      }

      public ICommand SelectCommand { get; }
      public ICommand SelectionChangedCommand { get; }
      public ICommand DeleteSpecificTemplateCommand { get; }
      public ICommand DeleteSelectedTemplateCommand { get; }

      public ReportTemplateDescriptor SelectedTemplate
      {
         get => _selectedTemplate;
         set
         {
            _selectedTemplate = value;
            RaisePropertyChanged();
         }
      }

      public IEnumerable<ReportTemplateDescriptor> ReportTemplates
      {
         get => _reportTemplates;
         set
         {
            _reportTemplates = value;
            RaisePropertyChanged();
            RaisePropertyChanged(nameof(TemplatesCountLabel));
         }
      }

      public bool IsOneSelected
      {
         get => _isOneSelected;
         set
         {
            _isOneSelected = value;
            RaisePropertyChanged();
         }
      }

      public string TemplatesCountLabel
      {
         get => string.Format(
                  Resources.SelectTemplateSubTitle,
                  TemplateItemsCount.ToString(CultureInfo.CurrentUICulture));
      }

      public int TemplateItemsCount => _reportTemplates.ToList()?.Count ?? 0;

      public void SetOkAction(Func<SelectedReportTemplateInfo, Task> okAction)
      {
         _okAction = okAction;
      }

      private async void OnDeleteSelectedReportTemplateCommand(object obj = null)
      {
         await DeleteReportTemplate(_selectedTemplate);
      }

      private async void OnDeleteSpecificReportTemplateCommand(ReportTemplateDescriptor reportTemplateDescriptor)
      {
         await DeleteReportTemplate(reportTemplateDescriptor);
      }

      private async Task DeleteReportTemplate(ReportTemplateDescriptor reportTemplateDescriptor)
      {
         if (CanDeleteTemplate(reportTemplateDescriptor))
         {
            var isReportTemplateDeleted = await _reportTemplateController.DeleteReportTemplate(reportTemplateDescriptor.Id);
            if (isReportTemplateDeleted)
            {
               await ResetReportTemplateList();
            }
         }
      }

      private bool CanDeleteTemplate(ReportTemplateDescriptor selectedTemplate)
      {
         return selectedTemplate != null && !selectedTemplate.ReadOnly;
      }

      public async Task ResetReportTemplateList()
      {
         ReportTemplates = await GetReportTemplates();
         SelectedTemplate = null;
         IsOneSelected = false;
      }

      private async Task OnSelectClick(Guid templateId)
      {
         var reportTemplate = await _reportTemplateController.GetReportTemplateById(templateId);

         if (reportTemplate != null)
         {
            var selectedReportTemplateInfo = BuildSelectedReportTemplateInfo(reportTemplate);
            await _okAction?.Invoke(selectedReportTemplateInfo);
         }
      }

      private void OnSelectChanged(RadTileView tileList)
      {
         SelectedTemplate = (ReportTemplateDescriptor)tileList.SelectedItem;
         IsOneSelected = tileList.SelectedItem != null;
      }

      protected void LocalizeReportTemplates(IEnumerable<ReportTemplateDescriptor> reportTemplates)
      {
         reportTemplates
            .Where(n => n.LocalizedName)
            .ToList()
            .ForEach(f => f.Name = StringFinder.FindLocalizedString(f.Name));
      }
      private async Task<IEnumerable<ReportTemplateDescriptor>> GetReportTemplates()
      {
         var reportTemplates = await GetReportTemplatesFromContoller(_reportTemplateController);
         LocalizeReportTemplates(reportTemplates);
         return reportTemplates.OrderBy(x => x.LocalizedName ? 0 : 1).ThenBy(x => x.Name);
      }

      public abstract string Title { get; }

      public abstract string ProceedButtonText { get; }

      protected abstract Task<IEnumerable<ReportTemplateDescriptor>> GetReportTemplatesFromContoller(IReportTemplateController reportTemplateController);

      protected abstract SelectedReportTemplateInfo BuildSelectedReportTemplateInfo(ReportTemplate reportTemplate);
   }
}
