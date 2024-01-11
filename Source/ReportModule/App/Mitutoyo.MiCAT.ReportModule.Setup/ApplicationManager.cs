// <copyright file="ApplicationManager.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>
using Mitutoyo.MiCAT.Utilities.IoC;
using Unity;

namespace Mitutoyo.MiCAT.ReportModule.Setup
{
   // This class manages the appliction root
   public class ApplicationManager
   {
      private readonly IServiceRegistrar _serviceRegistrar;
      private readonly ApplicationRegister _applicationRegister;
      private readonly ApplicationController _applicationController;

      public ApplicationManager()
      {
         IOCContainer = new UnityContainer();
         ServiceLocator = new ServiceLocator(IOCContainer);
         _serviceRegistrar = new ServiceRegistrar(IOCContainer);
         var commonRegistrar = new CommonRegistrar();
         _applicationRegister = new ApplicationRegister(_serviceRegistrar, ServiceLocator, commonRegistrar);
         _applicationController = new ApplicationController(ServiceLocator);
      }

      public ApplicationManager(
         IUnityContainer reportWorkspaceUnityContainer,
         IServiceLocator serviceLocator,
         IServiceRegistrar serviceRegistrar)
      {
         IOCContainer = reportWorkspaceUnityContainer;
         ServiceLocator = serviceLocator;
         _serviceRegistrar = serviceRegistrar;
         var commonRegistrar = new CommonRegistrar();
         _applicationRegister = new ApplicationRegister(_serviceRegistrar, ServiceLocator, commonRegistrar);
         _applicationController = new ApplicationController(ServiceLocator);
      }

      public IServiceLocator ServiceLocator { get; }
      public IUnityContainer IOCContainer { get; }

      public void Start()
      {
         _applicationRegister.Registration();
         _applicationController.ConfigureApplication();
         _applicationController.CreateAppClients();
      }
   }
}