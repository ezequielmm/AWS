// <copyright file="DeleteComponentControllerFake.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.ReportModule.Domain.Components;
using Mitutoyo.MiCAT.ReportModuleApp.Controllers;

namespace Mitutoyo.MiCAT.ReportModule.Setup.Export.Fakes
{
   [ExcludeFromCodeCoverage]
   public class DeleteComponentControllerFake : IDeleteComponentController
   {
      public Task DeleteComponent(IItemId<IReportComponent> selectedReportComponentId)
      {
         return Task.CompletedTask;
      }

      public Task DeleteSelectedComponents()
      {
         return Task.CompletedTask;
      }
   }
}