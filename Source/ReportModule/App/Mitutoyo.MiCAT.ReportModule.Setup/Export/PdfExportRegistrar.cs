// <copyright file="PdfExportRegistrar.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.IO;
using Mitutoyo.MiCAT.Applications.Services.Core;
using Mitutoyo.MiCAT.ReportModule.GUI.Common;
using Mitutoyo.MiCAT.ReportModule.GUI.ContextMenu;
using Mitutoyo.MiCAT.ReportModule.Persistence;
using Mitutoyo.MiCAT.ReportModule.Setup.Export.Fakes;
using Mitutoyo.MiCAT.ReportModuleApp.Controllers;
using Mitutoyo.MiCAT.ReportModuleApp.Providers;
using Mitutoyo.MiCAT.ReportModuleApp.Providers.Inputs;
using Mitutoyo.MiCAT.ReportModuleApp.Services.PlansRuns;
using Mitutoyo.MiCAT.ReportModuleApp.Utilities;
using Mitutoyo.MiCAT.Utilities.IoC;

namespace Mitutoyo.MiCAT.ReportModule.Setup.Export
{
   public class PdfExportRegistrar : IPdfExportRegistrar
   {
      private readonly ICommonRegistrar _commonRegistrar;
      private readonly FileInfo _saveFileInfo;

      public PdfExportRegistrar(ICommonRegistrar commonRegistrar, FileInfo saveFileInfo)
      {
         _commonRegistrar = commonRegistrar;
         _saveFileInfo = saveFileInfo;
      }

      public void Register(IServiceRegistrar serviceRegistrar)
      {
         _commonRegistrar.Register(serviceRegistrar);

         RegisterViewModels(serviceRegistrar);
         RegisterControllers(serviceRegistrar);
         RegisterPersistence(serviceRegistrar);
         RegisterUIServices(serviceRegistrar);
      }

      private void RegisterViewModels(IServiceRegistrar serviceRegistrar)
      {
         serviceRegistrar.RegisterSingleton<IReportModeProperty, ReportModePropertyFake>();
      }

      private void RegisterUIServices(IServiceRegistrar serviceRegistrar)
      {
         serviceRegistrar.RegisterSingleton<IActionCaller, ActionCallerFake>();
         serviceRegistrar.RegisterSingleton<IMainContextMenu, PaletteContextMenuFake>();
         serviceRegistrar.RegisterSingleton<ITemplateNameResolver, TemplateNameResolverFake>();
         serviceRegistrar.RegisterSingleton<IUnsavedChangesService, UnsavedChangesServiceFake>();
         serviceRegistrar.RegisterSingleton<IReportTemplateDeleteConfirmationInput, ReportTemplateDeleteConfirmationInputFake>();
         serviceRegistrar.RegisterSingleton<IMessageNotifier, PdfBackgroundNotifierFake>();
         serviceRegistrar.RegisterSingleton<IMeasurementDataSourceRefresherService, MeasurementDataSourceRefresherServiceFake>();
         serviceRegistrar.RegisterSingletonWithFactory<IPdfNameResolver>((s) => new PdfBackgroundExportFileNameResolver(_saveFileInfo));
      }

      private void RegisterControllers(IServiceRegistrar serviceRegistrar)
      {
         serviceRegistrar.RegisterSingleton<ITextBoxController, TextBoxControllerFake>();
         serviceRegistrar.RegisterSingleton<ITableViewController, TableViewControllerFake>();
         serviceRegistrar.RegisterSingleton<ITessellationViewController, TessellationViewControllerFake>();
         serviceRegistrar.RegisterSingleton<IImageController, ImageControllerFake>();
         serviceRegistrar.RegisterSingleton<IHeaderFormController, HeaderFormControllerFake>();
         serviceRegistrar.RegisterSingleton<IHeaderFormFieldController, HeaderFormFieldControllerFake>();
         serviceRegistrar.RegisterSingleton<ISelectedComponentController, SelectedComponentControllerFake>();
         serviceRegistrar.RegisterSingleton<ISelectedSectionController, SelectedSectionControllerFake>();
         serviceRegistrar.RegisterSingleton<IDeleteComponentController, DeleteComponentControllerFake>();
         serviceRegistrar.RegisterSingleton<IReportComponentPlacementController, ReportComponentPlacementControllerFake>();
      }

      private void RegisterPersistence(IServiceRegistrar serviceRegistrar)
      {
         serviceRegistrar.RegisterSingleton<IPlanPersistence, PlanPersistenceFake>();
         serviceRegistrar.RegisterSingleton<IPartPersistence, PartPersistenceFake>();
      }
   }
}
