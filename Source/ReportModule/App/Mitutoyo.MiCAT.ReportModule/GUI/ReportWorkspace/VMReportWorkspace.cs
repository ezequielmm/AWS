// <copyright file="VMReportWorkspace.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.Common.GUI.Commands;
using Mitutoyo.MiCAT.ReportModule.GUI.Common;
using Mitutoyo.MiCAT.ReportModule.GUI.SelectTemplates;
using Mitutoyo.MiCAT.ReportModule.GUI.Utilities.Events;
using Mitutoyo.MiCAT.ReportModule.Setup.ReportWorkspace;
using Mitutoyo.MiCAT.ShellModule;
using Unity;

namespace Mitutoyo.MiCAT.ReportModule
{
   public class VMReportWorkspace : VMBase
   {
      private readonly IAppStateHistory _history;
      private readonly IReportModuleInfo _reportModulePopulator;
      private readonly IReportModuleNavigation _reportModuleNavigation;
      private NavigationModuleChildInfo _moduleChildInfo;
      private IUnityContainer _childModuleInstaceContainer;
      private VMSelectTemplateBase _selectTemplateControl;
      private bool _isEditTemplateSelected;
      private bool _isCreateTemplateSelected;

      private VMSelectTemplateForCreate _vmSelectTemplateForCreate;
      private VMSelectTemplateForEdit _vmSelectTemplateForEdit;

      public ICommand CreateReportCommand { get; private set; }
      public ICommand EditTemplateCommand { get; private set; }

      public VMSelectTemplateBase SelectTemplateControl
      {
         get { return _selectTemplateControl; }
         set
         {
            _selectTemplateControl = value;
            RaisePropertyChanged();
         }
      }

      public bool IsEditTemplateSelected
      {
         get { return _isEditTemplateSelected; }
         set
         {
            _isEditTemplateSelected = value;
            RaisePropertyChanged();
         }
      }

      public bool IsCreateTemplateSelected
      {
         get { return _isCreateTemplateSelected; }
         set
         {
            _isCreateTemplateSelected = value;
            RaisePropertyChanged();
         }
      }

      public VMReportWorkspace(IAppStateHistory history,
         IReportModuleInfo reportModulePopulator,
         IReportModuleNavigation reportModuleNavigation)
      {
         _history = history;
         CreateReportCommand = new RelayCommand(OnCreateReport);
         EditTemplateCommand = new RelayCommand(OnEditTemplate);
         _reportModulePopulator = reportModulePopulator;
         _reportModuleNavigation = reportModuleNavigation;
         _selectTemplateControl = null;

         CreateContainerForChildModuleInstance();

         OnEditTemplate();
      }

      private async void OnCreateReport(object obj = null)
      {
         IsCreateTemplateSelected = true;
         IsEditTemplateSelected = false;
         await SetSelectTemplateControl(_vmSelectTemplateForCreate);
      }

      private async void OnEditTemplate(object obj = null)
      {
         IsEditTemplateSelected = true;
         IsCreateTemplateSelected = false;
         await SetSelectTemplateControl(_vmSelectTemplateForEdit);
      }

      private async Task SetSelectTemplateControl(VMSelectTemplateBase vmSelectTemplateBase)
      {
         await vmSelectTemplateBase.ResetReportTemplateList();
         SelectTemplateControl = vmSelectTemplateBase;
      }

      private void CreateContainerForChildModuleInstance()
      {
         _moduleChildInfo = _reportModulePopulator.CreateChildItem();
         _childModuleInstaceContainer = _reportModuleNavigation.CreateContainerForWorkspace(_moduleChildInfo.Id);

         _vmSelectTemplateForCreate = GetVMSelectTemplate<VMSelectTemplateForCreate>();
         _vmSelectTemplateForEdit = GetVMSelectTemplate<VMSelectTemplateForEdit>();
      }

      private TVMSelectTemplate GetVMSelectTemplate<TVMSelectTemplate>() where TVMSelectTemplate : VMSelectTemplateBase
      {
         var vmSelectTemplate = _childModuleInstaceContainer.Resolve<TVMSelectTemplate>();
         vmSelectTemplate.SetOkAction(OnReportTemplateSelection);

         return vmSelectTemplate;
      }

      private async Task OnReportTemplateSelection(SelectedReportTemplateInfo selectedReportTemplateInfo)
      {
         if (CheckReportTemplateIsSelected(selectedReportTemplateInfo))
         {
            SelectTemplateControl = null;

            await AddReportModuleToPopulator();
            await StartReporViewer(selectedReportTemplateInfo);

            CreateContainerForChildModuleInstance();
         }
      }

      private async Task AddReportModuleToPopulator()
      {
         await _history.RunAsync((snapShot) => AddReportModuleToPopulator(snapShot));
      }
      private ISnapShot AddReportModuleToPopulator(ISnapShot snapShot)
      {
         var module = snapShot.GetItems<NavigationModuleInfo>()
            .Single(x => x.ResourceKey == "ReportWorkspace_iconDrawingImage");
         return _reportModulePopulator.AddChildItems(module, snapShot, _moduleChildInfo);
      }

      private async Task StartReporViewer(SelectedReportTemplateInfo selectedReportTemplateInfo)
      {
         var reportViewer = _childModuleInstaceContainer.Resolve<IReportWorkspaceStartUp>();
         await reportViewer.Start(selectedReportTemplateInfo);
      }

      private bool CheckReportTemplateIsSelected(SelectedReportTemplateInfo selectedReportTemplateInfo)
      {
         return selectedReportTemplateInfo.ReportTemplate != null;
      }
   }
}
