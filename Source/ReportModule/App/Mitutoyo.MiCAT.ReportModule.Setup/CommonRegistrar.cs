// <copyright file="CommonRegistrar.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Reflection;
using Mitutoyo.MiCAT.DataServiceClient;
using Mitutoyo.MiCAT.ReportModule.GUI.Common;
using Mitutoyo.MiCAT.ReportModule.GUI.Common.ComponentsPageViewRelation;
using Mitutoyo.MiCAT.ReportModule.GUI.Common.DisabledSpace;
using Mitutoyo.MiCAT.ReportModule.GUI.Common.LayoutCalculator;
using Mitutoyo.MiCAT.ReportModule.GUI.Export;
using Mitutoyo.MiCAT.ReportModule.GUI.Export.ReportViewToRadFixedDocument;
using Mitutoyo.MiCAT.ReportModule.GUI.PageLayout;
using Mitutoyo.MiCAT.ReportModule.GUI.ReportBoundarySection;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElementList.ViewModels;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements;
using Mitutoyo.MiCAT.ReportModule.PDFRenderer.Renderers;
using Mitutoyo.MiCAT.ReportModule.Persistence;
using Mitutoyo.MiCAT.ReportModule.Persistence.Service;
using Mitutoyo.MiCAT.ReportModule.Persistence.Service.Helpers;
using Mitutoyo.MiCAT.ReportModule.Persistence.Template;
using Mitutoyo.MiCAT.ReportModule.Persistence.Template.CurrentVersion;
using Mitutoyo.MiCAT.ReportModule.Setup.Configurations;
using Mitutoyo.MiCAT.ReportModule.Setup.ReportWorkspace;
using Mitutoyo.MiCAT.ReportModuleApp.Clients;
using Mitutoyo.MiCAT.ReportModuleApp.Controllers;
using Mitutoyo.MiCAT.ReportModuleApp.Utilities;
using Mitutoyo.MiCAT.Utilities.IoC;

namespace Mitutoyo.MiCAT.ReportModule.Setup
{
   public class CommonRegistrar : ICommonRegistrar
   {
      public void Register(IServiceRegistrar serviceRegistrar)
      {
         RegisterStartup(serviceRegistrar);
         RegisterAppState(serviceRegistrar);
         RegisterViewModels(serviceRegistrar);
         RegisterUILayoutServices(serviceRegistrar);
         RegisterAutoMapper(serviceRegistrar);
         RegisterControllers(serviceRegistrar);
         RegisterPersistence(serviceRegistrar);
         RegisterViewLogic(serviceRegistrar);
         RegisterExportLogic(serviceRegistrar);
         RegisterClients(serviceRegistrar);
      }

      private void RegisterStartup(IServiceRegistrar serviceRegistrar)
      {
         serviceRegistrar.RegisterSingletonWithFactory<IReportWorkspaceStartUp>((s) => new ReportWorkspaceStartUp(s));
      }

      private void RegisterAppState(IServiceRegistrar serviceRegistrar)
      {
         var appStateModule = new AppStateModule();
         appStateModule.OnRegister(serviceRegistrar);
      }

      private void RegisterViewModels(IServiceRegistrar serviceRegistrar)
      {
         serviceRegistrar.RegisterSingleton<IVMReportElementList, VMReportElementList>();
         serviceRegistrar.RegisterSingleton<IVMPages, VMPages>();
         serviceRegistrar.RegisterSingleton<IReportBoundarySectionUpdateReceiver, ReportBoundarySectionUpdateReceiver>();
         serviceRegistrar.RegisterSingleton<IVMReportBoundarySectionComponentFactory, VMReportBoundarySectionComponentFactory>();
      }

      private void RegisterUILayoutServices(IServiceRegistrar serviceRegistrar)
      {
         serviceRegistrar.RegisterSingleton<IRenderedData, RenderedData>();
         serviceRegistrar.RegisterSingleton<IPagesRenderer, PagesRenderer>();
         serviceRegistrar.RegisterSingleton<IPageLayoutCalculator, PageLayoutCalculator>();
         serviceRegistrar.RegisterSingleton<IMultiPageElementManager, MultiPageElementManager>();
         serviceRegistrar.RegisterSingleton<IOnePiecePerPageAllocator, OnePiecePerPageAllocator>();
         serviceRegistrar.RegisterSingleton<IDisabledSpaceDataCollection, DisabledSpaceDataCollection>();
         serviceRegistrar.RegisterSingleton<IRenderer, Renderer>();
      }

      private void RegisterAutoMapper(IServiceRegistrar serviceRegistrar)
      {
         var mappingConfig = AutoMapperConfig.InitializeAutoMapper();
         mappingConfig.AssertConfigurationIsValid();
         serviceRegistrar.RegisterWithFactory(l => mappingConfig.CreateMapper());
      }

      private void RegisterControllers(IServiceRegistrar serviceRegistrar)
      {
         serviceRegistrar.RegisterSingleton<IPdfExportController, PdfExportController>();
         serviceRegistrar.RegisterSingleton<IRunController, RunController>();
         serviceRegistrar.RegisterSingleton<IRunRequestController, RunRequestController>();
         serviceRegistrar.RegisterSingleton<IReportTemplateController, ReportTemplateController>();
      }
      private void RegisterClients(IServiceRegistrar serviceRegistrar)
      {
         serviceRegistrar.RegisterSingleton<RunSelectionRequestClient>();
      }

      private void RegisterPersistence(IServiceRegistrar serviceRegistrar)
      {
         serviceRegistrar.RegisterSingleton<IDataServiceClientConfigurator, DataServiceClientConfigurator>();
         serviceRegistrar.RegisterSingletonWithFactory<IAssemblyConfigurationHelper>(l => new AssemblyConfigurationHelper(Assembly.GetExecutingAssembly()));
         serviceRegistrar.RegisterSingleton<IDataServiceClient, DataServiceClient.DataServiceClient>();
         serviceRegistrar.RegisterSingleton<IReportTemplatePersistence, ReportTemplatePersistence>();
         serviceRegistrar.RegisterSingleton<IPersistenceServiceLocator, PersistenceServiceLocator>();
         serviceRegistrar.RegisterSingleton<IDynamicPropertyPersistence, DynamicPropertyPersistence>();
         serviceRegistrar.RegisterSingleton<ICharacteristicDetailsProvider, CharacteristicDetailsProvider>();
         serviceRegistrar.RegisterSingleton<IMeasurementPersistence, MeasurementPersistence>();
         serviceRegistrar.RegisterSingleton<IReportTemplateSerializationManager, ReportTemplateSerializationManager>();
         serviceRegistrar.RegisterSingleton<IReportTemplateContentSerializerSelector, ReportTemplateContentSerializerSelector>();
         serviceRegistrar.RegisterSingleton<IReportTemplateVersionProvider, CurrentReportTemplateVersion>();
         serviceRegistrar.RegisterSingleton<IReportTemplateSerializer, ReportTemplateSerializer>();
         serviceRegistrar.RegisterSingleton<IPdfPersistence, PdfPersistence>();
      }

      private void RegisterViewLogic(IServiceRegistrar serviceRegistrar)
      {
         serviceRegistrar.RegisterSingleton<IViewToVMReportComponent, ViewToVMReportComponent>();
         serviceRegistrar.RegisterSingleton<IReportComponentsByPageArranger, ReportComponentsByPageArranger>();
         serviceRegistrar.RegisterSingleton<IPageViewsFromReportView, PageViewsFromReportView>();
         serviceRegistrar.RegisterSingleton<IReportComponentsFromReportView, ReportComponentsFromReportView>();
         serviceRegistrar.RegisterSingleton<IReportComponentsByPageFromReportView, ReportComponentsByPageFromReportView>();
      }

      private void RegisterExportLogic(IServiceRegistrar serviceRegistrar)
      {
         serviceRegistrar.RegisterSingleton<IRadFixedDocumentGenerator, RadFixedDocumentCreator>();
         serviceRegistrar.RegisterSingleton<IPdfGenerator, PdfGenerator>();
         serviceRegistrar.RegisterSingleton<IProcessLauncher, ProcessLauncher>();
      }
   }
}