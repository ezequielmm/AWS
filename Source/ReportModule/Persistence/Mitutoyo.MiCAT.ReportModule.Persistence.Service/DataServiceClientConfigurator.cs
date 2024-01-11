// <copyright file="DataServiceClientConfigurator.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using Mitutoyo.MiCAT.DataServiceClient;
using Mitutoyo.MiCAT.ReportModule.Persistence.Service.Helpers;

namespace Mitutoyo.MiCAT.ReportModule.Persistence.Service
{
   public class DataServiceClientConfigurator : IDataServiceClientConfigurator
   {
      private readonly IAssemblyConfigurationHelper _assemblyConfigurationHelper;
      private readonly IDataServiceClient _dataServiceClient;

      public DataServiceClientConfigurator(IDataServiceClient dataServiceClient, IAssemblyConfigurationHelper assemblyConfigurationHelper)
      {
         _dataServiceClient = dataServiceClient;
         _assemblyConfigurationHelper = assemblyConfigurationHelper;
      }

      public void Configure()
      {
         _dataServiceClient.SetDataServiceUri(_assemblyConfigurationHelper.GetDataServiceApiUrl());
      }
   }
}
