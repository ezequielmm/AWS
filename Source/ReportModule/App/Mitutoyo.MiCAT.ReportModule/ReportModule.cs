// <copyright file="ReportModule.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.ReportModule.Connectors;
using Mitutoyo.MiCAT.ReportModule.ReportModuleManager;
using Mitutoyo.MiCAT.ReportModule.Services;
using Mitutoyo.MiCAT.ReportModule.Setup;
using Mitutoyo.MiCAT.ReportModuleApp.Utilities;
using Mitutoyo.MiCAT.ReportModuleConnector.ExportConnector;
using Mitutoyo.MiCAT.ShellModule;
using Mitutoyo.MiCAT.Utilities.IoC;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Unity;
using Unity;
using Unity.Lifetime;

namespace Mitutoyo.MiCAT.ReportModule
{
   public class ReportModule : IReportModule
   {
      private IUnityContainer _shellUnityContainer;

      public void RegisterTypes(IContainerRegistry containerRegistry)
      {
         _shellUnityContainer = containerRegistry.GetContainer();
         RegisterModuleForShell(_shellUnityContainer);
      }

      public void OnInitialized(IContainerProvider containerProvider)
      {
         var shellUnityContainer = containerProvider.GetContainer();
         BindViewModelToView<VMReportWorkspace, ReportWorkspaceView>(shellUnityContainer);
         containerProvider.Resolve<ReportModuleUpdateReceiver>();
         containerProvider.Resolve<IApplicationCommandDelegator>();
      }

      public void InitializeWorkspace(Id<NavigationModuleChildInfo> containerId, IUnityContainer reportViewUnityContainer)
      {
         var serviceLocator = new ServiceLocator(reportViewUnityContainer);
         var serviceRegistrator = new ServiceRegistrar(reportViewUnityContainer);
         var applicationManager = new ApplicationManager(reportViewUnityContainer, serviceLocator, serviceRegistrator);

         RegisterUtilitiesForReportView(reportViewUnityContainer, containerId);
         applicationManager.Start();
      }

      private void RegisterUtilitiesForReportView(IUnityContainer reportViewUnityContainer, Id<NavigationModuleChildInfo> containerId)
      {
         reportViewUnityContainer.RegisterInstance<IHelpService>(
            ServicesResolver.ResolveHelpServiceFromShellContainer(_shellUnityContainer));
         reportViewUnityContainer.RegisterInstance<ICloseService>(
            ServicesResolver.ResolveCloseServiceFromShellContainer(_shellUnityContainer, containerId));
      }

      private void RegisterModuleForShell(IUnityContainer unityContainer)
      {
         unityContainer.RegisterType<IReportExporter, ReportExporter>();
         unityContainer.RegisterType<object, ReportWorkspaceView>("ReportWorkspaceView");
         unityContainer.RegisterType<IReportModuleInfo, ReportModuleInfoPopulator>(new ContainerControlledLifetimeManager());
         unityContainer.RegisterType<ReportModuleUpdateReceiver>(new ContainerControlledLifetimeManager());
         unityContainer.RegisterType<IReportModuleNavigation, ReportModuleNavigation>(new ContainerControlledLifetimeManager());
         unityContainer.RegisterType<IApplicationCommandDelegator, ApplicationCommandDelegator>(new ContainerControlledLifetimeManager());
         unityContainer.RegisterType<IReportModuleCloseManager, ReportModuleCloseManager>(new ContainerControlledLifetimeManager());
         unityContainer.RegisterType<IWorkspaceSelectionManager, WorkspaceSelectionManager>(new ContainerControlledLifetimeManager());
         unityContainer.RegisterType<IWorskpaceContainerResolver, WorskpaceContainerResolver>(new ContainerControlledLifetimeManager());
         unityContainer.RegisterInstance<IReportModule>(this);
      }

      private static void BindViewModelToView<TViewModel, TView>(IUnityContainer unityContainer)
      {
         ViewModelLocationProvider.Register(typeof(TView).ToString(), () => unityContainer.Resolve<TViewModel>());
      }
   }
}
