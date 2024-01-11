﻿// <copyright file="IMeasurementDataSourceRefresherService.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Collections.Generic;
using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.ReportModule.Domain.Data;

namespace Mitutoyo.MiCAT.ReportModuleApp.Services.PlansRuns
{
   public interface IMeasurementDataSourceRefresherService
   {
      ISnapShot Refresh(ISnapShot snapShot, IEnumerable<PlanDescriptor> plans);
      ISnapShot Refresh(ISnapShot snapShot, IEnumerable<PlanDescriptor> plans, IEnumerable<RunDescriptor> runs);
      ISnapShot ClearRunSelection(ISnapShot snapShot);
   }
}
