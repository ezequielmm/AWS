// <copyright file="DataServicePreloader.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Reflection;
using System.Threading.Tasks;
using Mitutoyo.MiCAT.DataServiceClient;
using Mitutoyo.MiCAT.ReportModule.Persistence.Service.Helpers;

namespace Mitutoyo.MiCAT.ReportModule.Setup
{
   public static class DataServicePreloader
   {
      /// <summary>
      /// This method is used to perform an early awake of the DataService
      /// during MiCAT initialization by adhering to the ISharedService
      /// mechanism. This avoids first time DataService usage lag.
      /// </summary>
      public static async Task AwakeDataServiceServer()
      {
         try
         {
            var configurationHelper = new AssemblyConfigurationHelper(Assembly.GetExecutingAssembly());
            var dsUri = configurationHelper.GetDataServiceApiUrl();
            using (var dsClient = DataServiceClientFactory.CreateDataServiceClient(dsUri))
            {
               await dsClient.GetServiceInfo();
            }
         }
#pragma warning disable CA1031 // Do not catch general exception types
         catch
         {
            // This is to prevent a possible uncaught exception from the DataService
         }
#pragma warning restore CA1031 // Do not catch general exception types
      }
   }
}
