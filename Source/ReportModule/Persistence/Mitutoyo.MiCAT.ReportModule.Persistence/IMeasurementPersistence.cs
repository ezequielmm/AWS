// <copyright file="IMeasurementPersistence.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mitutoyo.MiCAT.ReportModule.Domain.Data;

namespace Mitutoyo.MiCAT.ReportModule.Persistence
{
   public interface IMeasurementPersistence
   {
      Task<IEnumerable<RunDescriptor>> GetRunsByPartId(Guid partId);
      Task<IEnumerable<RunDescriptor>> GetRunsByPlanId(Guid planId);
      Task<IEnumerable<RunDescriptor>> GetRunsForSelectedPlanOrPart(IDescriptor selectedPlanPart);
      Task<RunData> GetRunDetail(Guid runId);
      IEnumerable<string> GetCharacteristicTypes();
      Task DeleteMeasurementResultById(Guid measurementResultId);
   }
}
