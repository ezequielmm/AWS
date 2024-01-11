// <copyright file="PlanPersistence.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Mitutoyo.MiCAT.Core.StdLib.Extensions;
using Mitutoyo.MiCAT.ReportModule.Domain.Data;
using Mitutoyo.MiCAT.ReportModule.Domain.DataResult;
using Mitutoyo.MiCAT.ReportModule.Domain.DataResult.Exceptions;
using Mitutoyo.MiCAT.ReportModule.Persistence.Service;
using Mitutoyo.MiCAT.Web.Data;
using Mitutoyo.MiCAT.Web.Data.Common;

namespace Mitutoyo.MiCAT.ReportModule.Persistence
{
   public class PlanPersistence : IPlanPersistence
   {
      private readonly IPersistenceDataService _service;
      private readonly IMapper _mapper;
      private readonly IPartPersistence _partPersistence;

      public PlanPersistence(IPersistenceServiceLocator persistenceServiceLocator, IMapper mapper, IPartPersistence partPersistence)
      {
         _service = persistenceServiceLocator.GetService<IPersistenceDataService>(Location.DataService, Mechanism.DataService);
         _mapper = mapper;
         _partPersistence = partPersistence;
      }

      public async Task CheckIfPlanExist(Guid planId)
      {
         var plansResult = await _service.GetPlanByIdLow(planId);
         var plansDto = GetListOrEmpty<PlanLowDTO>(plansResult);

         if (!plansDto.Any(p => p.Id == planId))
            throw new PlanNotFoundException();
      }

      public async Task<IEnumerable<PlanDescriptor>> GetPlans()
      {
         var plansResult = await _service.GetAllLatestPlansLow();

         ResultHelper.ThrowIfFailure(plansResult, "RetrievingDataError");

         var plansDto = GetListOrEmpty<PlanLowDTO>(plansResult);
         var plans = _mapper.Map<IEnumerable<PlanDescriptor>>(plansDto);

         return await AttachPartsToPlans(plans);
      }

      public async Task<IEnumerable<PlanDescriptor>> GetPlansForPart(Guid partId)
      {
         var plansResult = await _service.GetAllLatestPlansLowByPart(partId);

         ResultHelper.ThrowIfFailure(plansResult, "RetrievingDataError");

         var plans = GetListOrEmpty<PlanLowDTO>(plansResult);

         return _mapper.Map<IEnumerable<PlanDescriptor>>(plans);
      }

      private IEnumerable<T> GetListOrEmpty<T>(Result result)
      {
         return result is SuccessResult<IEnumerable<T>> succesResult ? succesResult.Result : Enumerable.Empty<T>();
      }
      public async Task DeletePlan(Guid planId)
      {
         var result = await _service.DeletePlan(planId, VersionFilter.All());

         ResultHelper.ThrowIfFailure(result, errorCode =>
         {
            switch (errorCode)
            {
               case ResultErrorCode.NotFound: return new PlanCouldNotBeDeletedException(result.Message);
               default: return new ResultException(result.Message, "DeletingDataError");
            }
         });
      }

      private async Task<IEnumerable<PlanDescriptor>> AttachPartsToPlans(IEnumerable<PlanDescriptor> plans)
      {
         var parts = await _partPersistence.GetParts();

         plans.
            Where(p => p.Part.Id != Guid.Empty).
            ForEach(plan => plan.Part = parts.First(p => p.Id == plan.Part.Id));

         return plans;
      }
   }
}
