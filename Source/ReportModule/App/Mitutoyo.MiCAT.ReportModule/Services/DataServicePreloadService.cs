// <copyright file="DataServicePreloadService.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Threading.Tasks;
using Mitutoyo.MiCAT.Applications.Services.Core;
using Mitutoyo.MiCAT.ReportModule.Setup;

namespace Mitutoyo.MiCAT.ReportModule.Services
{
   public class DataServicePreloadService : ISharedService
   {
      public void Close()
      {
      }

      public void Initialize()
      {
         Task.Run(async () => await DataServicePreloader.AwakeDataServiceServer().ConfigureAwait(false));
      }
   }
}
