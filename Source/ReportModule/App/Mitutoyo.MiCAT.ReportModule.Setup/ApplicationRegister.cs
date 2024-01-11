// <copyright file="ApplicationRegister.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using Mitutoyo.MiCAT.Applications.Services.Core;
using Mitutoyo.MiCAT.Common.GUI.MicatMessageBox;
using Mitutoyo.MiCAT.Common.Help;
using Mitutoyo.MiCAT.ReportModule.Domain;
using Mitutoyo.MiCAT.ReportModule.GUI;
using Mitutoyo.MiCAT.ReportModule.GUI.Common;
using Mitutoyo.MiCAT.ReportModule.GUI.Common.BusyIndicators;
using Mitutoyo.MiCAT.ReportModule.GUI.Common.DispatcherWrapping;
using Mitutoyo.MiCAT.ReportModule.GUI.Common.Filebar;
using Mitutoyo.MiCAT.ReportModule.GUI.Common.Filebar.MarginsSettings;
using Mitutoyo.MiCAT.ReportModule.GUI.ContextMenu;
using Mitutoyo.MiCAT.ReportModule.GUI.Dialog;
using Mitutoyo.MiCAT.ReportModule.GUI.Dialog.Confirmation.CloseWorkspace;
using Mitutoyo.MiCAT.ReportModule.GUI.Dialog.Confirmation.ReportTemplateDelete;
using Mitutoyo.MiCAT.ReportModule.GUI.Dialog.SaveAs;
using Mitutoyo.MiCAT.ReportModule.GUI.MainWindow;
using Mitutoyo.MiCAT.ReportModule.GUI.MainWindow.StatusBar;
using Mitutoyo.MiCAT.ReportModule.GUI.MainWindow.Zoom;
using Mitutoyo.MiCAT.ReportModule.GUI.Navigation;
using Mitutoyo.MiCAT.ReportModule.GUI.Navigation.TreeView;
using Mitutoyo.MiCAT.ReportModule.GUI.Providers;
using Mitutoyo.MiCAT.ReportModule.Persistence;
using Mitutoyo.MiCAT.ReportModuleApp.Controllers;
using Mitutoyo.MiCAT.ReportModuleApp.Manager;
using Mitutoyo.MiCAT.ReportModuleApp.Providers;
using Mitutoyo.MiCAT.ReportModuleApp.Providers.Inputs;
using Mitutoyo.MiCAT.ReportModuleApp.Services.PlansRuns;
using Mitutoyo.MiCAT.ReportModuleApp.Services.ReportComponents;
using Mitutoyo.MiCAT.ReportModuleApp.Utilities;
using Mitutoyo.MiCAT.Utilities.IoC;
using Prism.Mvvm;

namespace Mitutoyo.MiCAT.ReportModule.Setup
{
   public class ApplicationRegister
   {
      private readonly IServiceRegistrar _reportWorkspaceServiceRegistrar;
      private readonly IServiceLocator _serviceLocator;
      private readonly ICommonRegistrar _commonRegistrar;

      public ApplicationRegister(IServiceRegistrar reportWorkspaceServiceRegistrar, IServiceLocator serviceLocator, ICommonRegistrar commonRegistrar)
      {
         _reportWorkspaceServiceRegistrar = reportWorkspaceServiceRegistrar;
         _serviceLocator = serviceLocator;
         _commonRegistrar = commonRegistrar;
      }

      public void Registration()
      {
         _commonRegistrar.Register(_reportWorkspaceServiceRegistrar);

         BindViewModelsToViews();
         RegisterViewModels();
         RegisterControllers();
         RegisterServices();
         RegisterDomain();
         RegisterPersistence();
         RegisterUIServices();
         RegisterUILayoutServices();
      }

      private void BindViewModelsToViews()
      {
         BindViewModelToView<VMReportViewWorkspace, ReportViewWorkspace>();
         BindViewModelToView<VMReportView, ReportView>();
         BindViewModelToView<VMReportFilebar, ReportFilebar>();
         BindViewModelToView<VMToggleReportView, ToggleReportView>();
         BindViewModelToView<VMMainToolbar, MainToolbar>();
         BindViewModelToView<IVMZoomFactor, ZoomControlView>();
         BindViewModelToView<IVMPageNumberInfo, PageNumberInfo>();
         BindViewModelToView<VMParts, Parts>();
         BindViewModelToView<VMRuns, Runs>();
      }

      private void RegisterViewModels()
      {
         _reportWorkspaceServiceRegistrar.RegisterSingleton<VMParts>();
         _reportWorkspaceServiceRegistrar.RegisterSingleton<VMRuns>();
         _reportWorkspaceServiceRegistrar.RegisterSingleton<VMReportViewWorkspace>();
         _reportWorkspaceServiceRegistrar.RegisterSingleton<VMPageSizeSettings>();
         _reportWorkspaceServiceRegistrar.RegisterSingleton<VMMarginSizeSettings>();
         _reportWorkspaceServiceRegistrar.RegisterSingleton<IVMZoomFactor, VMZoomFactor>();
         _reportWorkspaceServiceRegistrar.RegisterSingleton<IVMPageNumberInfo, VMPageNumberInfo>();
         _reportWorkspaceServiceRegistrar.RegisterSingleton<IReportModeProperty, ReportModeProperty>();
      }

      private void RegisterUIServices()
      {
         _reportWorkspaceServiceRegistrar.RegisterSingleton<IMainContextMenu, PaletteContextMenu>();
         _reportWorkspaceServiceRegistrar.RegisterSingleton<ITemplateNameResolver, TemplateNameDialogResolver>();
         _reportWorkspaceServiceRegistrar.RegisterSingleton<IDialogManager, FileDialogManager>();
         _reportWorkspaceServiceRegistrar.RegisterSingleton<IDialogWithPreviewService, DialogWithPreviewService>();
         _reportWorkspaceServiceRegistrar.RegisterSingleton<IMicatMessageBoxShower, MicatMessageBoxShower>();
         _reportWorkspaceServiceRegistrar.RegisterSingleton<ICloseWorkspaceConfirmationInput, CloseWorkspaceConfirmationInput>();
         _reportWorkspaceServiceRegistrar.RegisterSingleton<IReportTemplateDeleteConfirmationInput, ReportTemplateDeleteInput>();
         _reportWorkspaceServiceRegistrar.RegisterSingleton<IUnsavedChangesService, UnsavedChangesService>();
         _reportWorkspaceServiceRegistrar.RegisterSingleton<IBusyIndicator, BusyIndicator>();
         _reportWorkspaceServiceRegistrar.RegisterSingleton<IActionCaller, BusyIndicatorActionCaller>();
         _reportWorkspaceServiceRegistrar.RegisterSingleton<IUnselectionArea, UnselectionArea>();
         _reportWorkspaceServiceRegistrar.RegisterSingleton<IPdfNameResolver, PDFSaveFileDialog>();
         _reportWorkspaceServiceRegistrar.RegisterSingleton<IMessageNotifier, DialogNotifier>();
         _reportWorkspaceServiceRegistrar.RegisterSingleton<IDialogService, DialogService>();
         _reportWorkspaceServiceRegistrar.RegisterSingleton<IDispatcherWrapper, DispatcherWrapper>();
         _reportWorkspaceServiceRegistrar.RegisterSingleton<IActiveWindowProvider, ActiveWindowProvider>();
      }

      private void RegisterUILayoutServices()
      {
         _reportWorkspaceServiceRegistrar.RegisterSingleton<ITreeViewItemListUpdater, TreeViewItemListUpdater>();
      }

      private void RegisterControllers()
      {
         _reportWorkspaceServiceRegistrar.RegisterSingleton<ICommonPageLayoutController, CommonPageLayoutController>();
         _reportWorkspaceServiceRegistrar.RegisterSingleton<ITextBoxController, TextBoxController>();
         _reportWorkspaceServiceRegistrar.RegisterSingleton<ITableViewController, TableViewController>();
         _reportWorkspaceServiceRegistrar.RegisterSingleton<ITessellationViewController, TessellationViewController>();
         _reportWorkspaceServiceRegistrar.RegisterSingleton<IImageController, ImageController>();
         _reportWorkspaceServiceRegistrar.RegisterSingleton<ISelectedComponentController, SelectedComponentController>();
         _reportWorkspaceServiceRegistrar.RegisterSingleton<ISelectedSectionController, SelectedSectionController>();
         _reportWorkspaceServiceRegistrar.RegisterSingleton<IDeleteComponentController, DeleteComponentController>();
         _reportWorkspaceServiceRegistrar.RegisterSingleton<IUndoRedoController, UndoRedoController>();
         _reportWorkspaceServiceRegistrar.RegisterSingleton<IReportFileBarController, ReportFileBarController>();
         _reportWorkspaceServiceRegistrar.RegisterSingleton<IPlanController, PlanController>();
         _reportWorkspaceServiceRegistrar.RegisterSingleton<IHeaderFormController, HeaderFormController>();
         _reportWorkspaceServiceRegistrar.RegisterSingleton<IHeaderFormFieldController, HeaderFormFieldController>();
         _reportWorkspaceServiceRegistrar.RegisterSingleton<IHelpTopicController, HelpTopicController>();
         _reportWorkspaceServiceRegistrar.RegisterSingleton<IHelpController, HelpController>();
         _reportWorkspaceServiceRegistrar.RegisterSingleton<IWorkspaceCloseManager, WorkspaceCloseManager>();
         _reportWorkspaceServiceRegistrar.RegisterSingleton<ICloseController, CloseController>();
         _reportWorkspaceServiceRegistrar.RegisterSingleton<IViewVisibilityController, ViewVisibilityController>();
         _reportWorkspaceServiceRegistrar.RegisterSingleton<IReportComponentPlacementController, ReportComponentPlacementController>();
      }

      private void RegisterServices()
      {
         _reportWorkspaceServiceRegistrar.RegisterSingleton<IMeasurementDataSourceRefresherService, MeasurementDataSourceRefresherService>();
         _reportWorkspaceServiceRegistrar.RegisterSingleton<IDomainSpaceService, DomainSpaceService>();
         _reportWorkspaceServiceRegistrar.RegisterSingleton<IPlacementService, PlacementService>();
         _reportWorkspaceServiceRegistrar.RegisterSingleton<IReportComponentService, ReportComponentService>();
      }

      private void RegisterDomain()
      {
         _reportWorkspaceServiceRegistrar.RegisterSingleton<IPageSizeList, PageSizeList>();
         _reportWorkspaceServiceRegistrar.RegisterSingleton<IMarginSizeList, MarginSizeList>();
      }

      private void RegisterPersistence()
      {
         _reportWorkspaceServiceRegistrar.RegisterSingleton<IPartPersistence, PartPersistence>();
         _reportWorkspaceServiceRegistrar.RegisterSingleton<IPlanPersistence, PlanPersistence>();
      }

      private void BindViewModelToView<TViewModel, TView>()
      {
         ViewModelLocationProvider.Register(typeof(TView).ToString(), () => _serviceLocator.Resolve<TViewModel>());
      }
   }
}
