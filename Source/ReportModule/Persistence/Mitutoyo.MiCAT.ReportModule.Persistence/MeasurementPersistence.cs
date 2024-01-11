// <copyright file="MeasurementPersistence.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Mitutoyo.MiCAT.DataServiceClient;
using Mitutoyo.MiCAT.ReportModule.Domain.Data;
using Mitutoyo.MiCAT.ReportModule.Domain.DataResult;
using Mitutoyo.MiCAT.ReportModule.Domain.DataResult.Exceptions;
using Mitutoyo.MiCAT.ReportModule.Persistence.Helper;
using Mitutoyo.MiCAT.ReportModule.Persistence.Service;
using Mitutoyo.MiCAT.Web.Data;
using Mitutoyo.MiCAT.Web.Data.Common;

namespace Mitutoyo.MiCAT.ReportModule.Persistence
{
   public class MeasurementPersistence : IMeasurementPersistence
   {
      private const string NOMINAL_VALUE_KEY = "NominalValue";
      private const string UPPER_TOLERANCE_VALUE_KEY = "ToleranceUp";
      private const string LOWER_TOLERANCE_VALUE_KEY = "ToleranceLow";
      private const string TOLERANCE_ZONE_VALUE_KEY = "ToleranceZone";
      private const string DEVIATION_VALUE_KEY = "Deviation";
      private const string DETAIL_VALUE_KEY = "CharacteristicDetail";
      private const string MEASURED_VALUE_KEY = "Measured";

      private readonly IPersistenceDataService _service;
      private readonly IMapper _mapper;

      public MeasurementPersistence(IPersistenceServiceLocator persistenceServiceLocator, IMapper mapper)
      {
         _service = persistenceServiceLocator.GetService<IPersistenceDataService>(Location.DataService, Mechanism.DataService);
         _mapper = mapper;
      }

      private async Task<PlanMediumDTO> GetPlanById(Guid planId, int planVersion)
      {
         var planVersionsResult = await _service.GetPlanVersionsMed(planId, VersionFilter.SpecificVersion(planVersion));

         if (!planVersionsResult.IsSuccess) throw new PlanNotFoundException();

         return GetListOrEmpty<PlanMediumDTO>(planVersionsResult).FirstOrDefault();
      }

      private Characteristic MapCharacteristicFromDTO(PlanMediumDTO plan, CharacteristicDTO characteristicDto)
      {
         var nominal = characteristicDto.PropertyValues.GetSafeDoubleValue(NOMINAL_VALUE_KEY);
         var upperTolerance = characteristicDto.PropertyValues.GetSafeDoubleValue(UPPER_TOLERANCE_VALUE_KEY);
         var lowerTolerance = characteristicDto.PropertyValues.GetSafeDoubleValue(LOWER_TOLERANCE_VALUE_KEY);
         var toleranceZone = characteristicDto.PropertyValues.GetSafeDoubleValue(TOLERANCE_ZONE_VALUE_KEY);
         var detail = characteristicDto.PropertyValues.GetSafeStringValue(DETAIL_VALUE_KEY);
         return new Characteristic(
            characteristicDto.Id,
            characteristicDto.Name,
            Enum.GetName(typeof(CharacteristicType), characteristicDto.CharacteristicType),
            new Feature(characteristicDto.FeatureId, plan.Features?.SingleOrDefault(f => f.Id == characteristicDto.FeatureId)?.Name),
            nominal,
            detail,
            upperTolerance,
            lowerTolerance,
            toleranceZone
         );
      }

      private CharacteristicActual MapCharacteristicActualFromDTO(MeasurementResultMediumDTO measurementResult, CharacteristicDTO characteristicDto)
      {
         var characteristicActualDto = measurementResult.CharacteristicActuals.SingleOrDefault(a => a.CharacteristicId == characteristicDto.Id);
         var deviation = characteristicActualDto?.PropertyValues?.GetSafeDoubleValue(DEVIATION_VALUE_KEY);
         var measured = characteristicActualDto?.PropertyValues?.GetSafeDoubleValue(MEASURED_VALUE_KEY);
         return new CharacteristicActual(
            characteristicDto.Id,
            characteristicActualDto == null ? "NotEvaluated" : characteristicActualDto.Status.ToString(),
            measured,
            deviation
         );
      }

      public Task<IEnumerable<RunDescriptor>> GetRunsForSelectedPlanOrPart(IDescriptor selectedPlanPart)
      {
         if (selectedPlanPart == null)
            return Task.FromResult(Array.Empty<RunDescriptor>() as IEnumerable<RunDescriptor>);
         else
            return GetRunsForSelectedPlanOrPart((dynamic) selectedPlanPart);
      }

      private Task<IEnumerable<RunDescriptor>> GetRunsForSelectedPlanOrPart(PlanDescriptor selectedPlanPart)
      {
         return GetRunsByPlanId(selectedPlanPart.Id);
      }
      private Task<IEnumerable<RunDescriptor>> GetRunsForSelectedPlanOrPart(PartDescriptor selectedPlanPart)
      {
         return GetRunsByPartId(selectedPlanPart.Id);
      }

      public async Task<IEnumerable<RunDescriptor>> GetRunsByPartId(Guid partId)
      {
         var planResultTask = _service.GetAllLatestPlansLowByPart(partId);
         var runsResultTask = _service.GetAllMeasurementResults();

         await Task.WhenAll(planResultTask, runsResultTask).ConfigureAwait(false);

         var plansResult = planResultTask.Result;
         var runsResult = runsResultTask.Result;

         var planVersionList = GetListOrEmpty<PlanLowDTO>(plansResult).Select(p => new { p.Id });

         ResultHelper.ThrowIfFailure(runsResult, "RetrievingDataError");

         var runs = GetListOrEmpty<MeasurementResultLowDTO>(runsResult).Where(r => planVersionList.Any(y => r.PlanId == y.Id));

         return _mapper.Map<IEnumerable<RunDescriptor>>(runs);
      }

      public async Task<IEnumerable<RunDescriptor>> GetRunsByPlanId(Guid planId)
      {
         var runsResult = await _service.GetAllMeasurementResultsForPlan(planId).ConfigureAwait(false);

         ResultHelper.ThrowIfFailure(runsResult, "RetrievingDataError");

         return _mapper.Map<IEnumerable<RunDescriptor>>(GetListOrEmpty<MeasurementResultLowDTO>(runsResult));
      }

      public async Task<RunData> GetRunDetail(Guid runId)
      {
         var measurementDataserviceResult = await _service.GetMeasurementResultWithAttachments(runId).ConfigureAwait(false);

         ResultHelper.ThrowIfFailure(measurementDataserviceResult, "RunNotFound");

         var measurementResult = measurementDataserviceResult is SuccessResult<WithAttachments<MeasurementResultMediumDTO>> succesResult ? succesResult.Result : new WithAttachments<MeasurementResultMediumDTO>();

         PlanMediumDTO plan = await GetPlanById(measurementResult.Entity.PlanId, measurementResult.Entity.PlanVersion).ConfigureAwait(false);

         if (plan == null)
            throw new PlanNotFoundException();
         else
         {
            return new RunData(
               measurementResult.Entity.TimeStamp,
               measurementResult.Entity.PlanId,
               measurementResult.Entity.PlanVersion,
               CreateEvaluatedCharacteristicsFromDtos(plan, measurementResult.Entity),
               CreateDynamicPropertyValueListFromDtos(measurementResult.Entity.PropertyValues),
               AttachmentHelper.GetCapture3DFromDto(measurementResult)
               );
         }
      }

      private ImmutableList<EvaluatedCharacteristic> CreateEvaluatedCharacteristicsFromDtos(PlanMediumDTO plan, MeasurementResultMediumDTO measurementResult)
      {
         return plan.Characteristics
            .Select(ch => new EvaluatedCharacteristic(MapCharacteristicFromDTO(plan, ch),MapCharacteristicActualFromDTO(measurementResult, ch)))
            .ToImmutableList();
      }
      private ImmutableList<DynamicPropertyValue> CreateDynamicPropertyValueListFromDtos(IEnumerable<PropertyValueDTO> propertyValues)
      {
         return _mapper.Map<IEnumerable<DynamicPropertyValue>>(propertyValues).ToImmutableList();
      }

      public IEnumerable<string> GetCharacteristicTypes()
      {
         return Enum.GetNames(typeof(Web.Data.CharacteristicType));
      }

      private IEnumerable<T> GetListOrEmpty<T>(Result result)
      {
         return result is SuccessResult<IEnumerable<T>> succesResult ? succesResult.Result : Enumerable.Empty<T>();
      }
      public async Task DeleteMeasurementResultById(Guid measurementResultId)
      {
         var result = await _service.DeleteMeasurementResult(measurementResultId);
         ResultHelper.ThrowIfFailure(result, errorCode =>
         {
            switch (errorCode)
            {
               case ResultErrorCode.NotFound: return new RunCouldNotBeDeletedException(result.Message);
               default: return new ResultException(result.Message, "DeletingDataError");
            }
         });
      }
   }
}
