// <copyright file="VMReportComponentFake.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.ReportModule.Domain.Components;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements;
using Mitutoyo.MiCAT.ReportModuleApp.Controllers;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Test.VisualElements.ReportElements
{
   [ExcludeFromCodeCoverage]
   public class VMReportComponentFake : VMReportComponent
   {
      public VMReportComponentFake(
         IAppStateHistory appStateHistory,
         IItemId<IReportComponent> reportComponentId,
         IVMReportComponentPlacement reportComponentPlacement,
         IDeleteComponentController deleteComponentController)
         : base(
              appStateHistory,
              reportComponentId,
              reportComponentPlacement,
              deleteComponentController)
      {
      }
   }
}
