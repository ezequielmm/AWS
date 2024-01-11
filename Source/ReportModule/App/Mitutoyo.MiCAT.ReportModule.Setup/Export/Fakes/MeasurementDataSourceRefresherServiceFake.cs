// <copyright file="MeasurementDataSourceRefresherServiceFake.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Collections.Generic;
using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.ReportModule.Domain.Data;
using Mitutoyo.MiCAT.ReportModuleApp.Services.PlansRuns;

namespace Mitutoyo.MiCAT.ReportModule.Setup.Export.Fakes
{
   public class MeasurementDataSourceRefresherServiceFake : IMeasurementDataSourceRefresherService
   {
      public ISnapShot Refresh(ISnapShot snapShot, IEnumerable<PlanDescriptor> plans)
      {
         return snapShot;
      }

      public ISnapShot Refresh(ISnapShot snapShot, IEnumerable<PlanDescriptor> plans, IEnumerable<RunDescriptor> runs)
      {
         return snapShot;
      }

      public ISnapShot ClearRunSelection(ISnapShot snapShot)
      {
         return snapShot;
      }
   }
}
