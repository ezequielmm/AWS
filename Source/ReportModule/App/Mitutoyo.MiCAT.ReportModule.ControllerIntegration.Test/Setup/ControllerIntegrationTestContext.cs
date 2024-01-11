// <copyright file="ControllerIntegrationTestContext.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using Mitutoyo.MiCAT.ReportModule.Setup;

namespace Mitutoyo.MiCAT.ReportModule.ControllerIntegration.Test.Setup
{
   [ExcludeFromCodeCoverage]
   public class ControllerIntegrationTestContext
   {
      private ApplicationManager _applicationManager;

      public ControllerIntegrationTestContext()
      {
         _applicationManager = new ApplicationManager();
      }

      public void Initialize()
      {
         _applicationManager.Start();
      }

      public TService GetService<TService>() => _applicationManager.ServiceLocator.Resolve<TService>();
   }
}
