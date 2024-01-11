// <copyright file="PersistenceDataService.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using System.Net;
using System.Threading.Tasks;
using Mitutoyo.MiCAT.DataServiceClient;
using Mitutoyo.MiCAT.ReportModule.Domain.DataResult;
using Mitutoyo.MiCAT.Web.Data;
using Mitutoyo.MiCAT.Web.Data.Common;

namespace Mitutoyo.MiCAT.ReportModule.Persistence.Service
{
   public class PersistenceDataService : IPersistenceDataService
   {
      private readonly IDataServiceClient _dataServiceClient;

      public PersistenceDataService(IDataServiceClient availableDataServiceClient)
      {
         _dataServiceClient = availableDataServiceClient;
      }

      public async Task<Result> GetAllCharacteristicActuals(Guid measurementResultId)
      {
         var characteristicActuals = await _dataServiceClient.GetAllCharacteristicActuals(measurementResultId);
         return ConvertToResult(characteristicActuals);
      }

      public async Task<Result> GetAllLatestPlansLowByPart(Guid partId)
      {
         var plans = await _dataServiceClient.GetAllPlansForPart<PlanLowDTO>(partId, VersionFilter.Latest()).ConfigureAwait(false);
         return ConvertToResult(plans);
      }

      public async Task<Result> GetPlanVersionsMed(Guid planId, VersionFilter versionFilter)
      {
         var plan = await _dataServiceClient.GetPlansById<PlanMediumDTO>(planId, versionFilter);
         return ConvertToResult(plan);
      }

      public async Task<Result> GetAllLatestPlansLow()
      {
         var plans = await _dataServiceClient.GetAllPlans<PlanLowDTO>(VersionFilter.Latest(), null);
         return ConvertToResult(plans);
      }

      public async Task<Result> GetPlanByIdLow(Guid planId)
      {
         var plans = await _dataServiceClient.GetPlansById<PlanLowDTO>(planId, VersionFilter.Latest());
         return ConvertToResult(plans);
      }

      public async Task<Result> GetPropertyDefinitions(EntityType entityType)
      {
         var properties = await _dataServiceClient.GetPropertyDefinitions(entityType);
         return ConvertToResult(properties);
      }

      public async Task<Result> GetAllMeasurementResults()
      {
         var measurements = await _dataServiceClient.GetAllMeasurementResults<MeasurementResultLowDTO>().ConfigureAwait(false);
         return ConvertToResult(measurements);
      }
      public async Task<Result> GetAllMeasurementResultsForPlan(Guid planId)
      {
         var measurements = await _dataServiceClient.GetAllMeasurementResultsForPlan<MeasurementResultLowDTO>(planId);
         return ConvertToResult(measurements);
      }

      public async Task<Result> GetCharacteristicActual(Guid measurementResultId, Guid characteristicActualId)
      {
         var characteristicActual = await _dataServiceClient.GetCharacteristicActual(measurementResultId, characteristicActualId);
         return ConvertToResult(characteristicActual);
      }
      public async Task<Result> GetReportTemplatesLow()
      {
         var reportTemplatesActual = await _dataServiceClient.GetAllReportTemplates<ReportTemplateLowDTO>();
         return ConvertToResult(reportTemplatesActual);
      }
      public async Task<Result> GetReportTemplateMed(Guid reportTemplateGuid)
      {
         var reportTemplateActual = await _dataServiceClient.GetReportTemplate<ReportTemplateMediumDTO>(reportTemplateGuid);
         return ConvertToResult(reportTemplateActual);
      }

      public async Task<Result> AddReportTemplateMed(string name, string template)
      {
         var reportTemplateDTO = new ReportTemplateCreateDTO()
         {
            Name = name,
            Template = template
         };

         var result = await _dataServiceClient.AddReportTemplate(reportTemplateDTO);
         return ConvertToResult(result);
      }

      public async Task<Result> UpdateReportTemplate(Guid reportTemplateId, ReportTemplateUpdateDTO reportTemplateUpdateDto)
      {
         var result = await _dataServiceClient.UpdateReportTemplate(reportTemplateId, reportTemplateUpdateDto);

         Func<Result> successResultFunc = () => new SuccessResult<ReportTemplateMediumDTO>(new ReportTemplateMediumDTO()
         {
            Id = reportTemplateId,
            Name = reportTemplateUpdateDto.Name,
            Template = reportTemplateUpdateDto.Template
         });

         return ConvertToResult(result, successResultFunc);
      }

      public async Task<Result> GetParts()
      {
         var partsActual = await _dataServiceClient.GetAllParts();
         return ConvertToResult(partsActual);
      }

      public async Task<Result> DeleteReportTemplate(Guid reportTemplateId)
      {
         var deleteResponse = await _dataServiceClient.DeleteReportTemplate(reportTemplateId);
         return ConvertToResult(deleteResponse);
      }

      public async Task<Result> GetMeasurementResult(Guid measurementResultId)
      {
         var measurementResult = await _dataServiceClient.GetMeasurementResult<MeasurementResultMediumDTO>(measurementResultId);
         return ConvertToResult(measurementResult);
      }

      public async Task<Result> DeletePlan(Guid planId, VersionFilter version)
      {
         var deleteResponse = await _dataServiceClient.DeletePlan(planId, version);
         return ConvertToResult(deleteResponse);
      }

      public async Task<Result> DeletePart(Guid partId)
      {
         var deleteResponse = await _dataServiceClient.DeletePart(partId);
         return ConvertToResult(deleteResponse);
      }

      public async Task<Result> DeleteMeasurementResult(Guid measurementResultId)
      {
         var deleteResponse = await _dataServiceClient.DeleteMeasurementResult(measurementResultId);
         return ConvertToResult(deleteResponse);
      }

      private Result ConvertToResult(DataServiceResult dataServiceResult)
      {
         return ConvertToResult(dataServiceResult, () => new SuccessResult());
      }

      private Result ConvertToResult<T>(DataServiceResult<T> dataServiceResult)
      {
         return ConvertToResult(dataServiceResult, () => new SuccessResult<T>(dataServiceResult.Success));
      }

      private Result ConvertToResult(DataServiceResult dataServiceResult, Func<Result> buildSuccessResultFunc)
      {
         return dataServiceResult.IsSuccess
            ? buildSuccessResultFunc.Invoke()
            : new ErrorResult(dataServiceResult.Error?.Message, ConvertToResultErrorCode(dataServiceResult.StatusCode));
      }

      private static ResultErrorCode ConvertToResultErrorCode(HttpStatusCode httpStatusCode)
      {
         switch (httpStatusCode)
         {
            case HttpStatusCode.NotFound:
               return ResultErrorCode.NotFound;
            case HttpStatusCode.BadRequest:
               return ResultErrorCode.BadRequest;
            default:
               return ResultErrorCode.BadRequest;
         }
      }

      public async Task<Result> GetMeasurementResultWithAttachments(Guid measurementResultId)
      {
         var measurementResultWithAttachments = await _dataServiceClient.GetMeasurementResultWithAttachments(measurementResultId);
         return ConvertToResult(measurementResultWithAttachments);
      }
   }
}
