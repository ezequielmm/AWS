// <copyright file="IPlanPersistence.cs" company="Mitutoyo Europe GmbH">
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
   public interface IPlanPersistence
   {
      Task<IEnumerable<PlanDescriptor>> GetPlans();
      Task CheckIfPlanExist(Guid planId);
      Task<IEnumerable<PlanDescriptor>> GetPlansForPart(Guid partId);
      Task DeletePlan(Guid planId);
   }
}
