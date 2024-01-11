// <copyright file="PartPersistenceFake.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mitutoyo.MiCAT.ReportModule.Domain.Data;
using Mitutoyo.MiCAT.ReportModule.Persistence;

namespace Mitutoyo.MiCAT.ReportModule.Setup.Export.Fakes
{
   public class PartPersistenceFake : IPartPersistence
   {
      public Task DeletePart(Guid partId)
      {
         return Task.CompletedTask;
      }

      public Task<IEnumerable<PartDescriptor>> GetParts()
      {
         return Task.FromResult<IEnumerable<PartDescriptor>>(new PartDescriptor[0]);
      }
   }
}
