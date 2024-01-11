// <copyright file="ApplicationController.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using Mitutoyo.MiCAT.ReportModule.Persistence.Service;
using Mitutoyo.MiCAT.ReportModuleApp.Clients;
using Mitutoyo.MiCAT.Utilities.IoC;

namespace Mitutoyo.MiCAT.ReportModule.Setup
{
   public class ApplicationController
   {
      private readonly IServiceLocator _serviceLocator;

      public ApplicationController(IServiceLocator serviceLocator)
      {
         _serviceLocator = serviceLocator;
      }

      public void ConfigureApplication()
      {
         ConfigureDataService();
      }
      public void CreateAppClients()
      {
         _ = _serviceLocator.Resolve<RunSelectionRequestClient>();
      }

      private void ConfigureDataService()
      {
         var dataServiceClientConfigurator = _serviceLocator.Resolve<IDataServiceClientConfigurator>();
         dataServiceClientConfigurator.Configure();
      }
   }
}
