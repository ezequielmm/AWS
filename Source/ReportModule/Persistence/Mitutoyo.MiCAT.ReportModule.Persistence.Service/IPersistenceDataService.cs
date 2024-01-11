// <copyright file="IPersistenceDataService.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using System.Threading.Tasks;
using Mitutoyo.MiCAT.ReportModule.Domain.DataResult;
using Mitutoyo.MiCAT.Web.Data;
using Mitutoyo.MiCAT.Web.Data.Common;

namespace Mitutoyo.MiCAT.ReportModule.Persistence.Service
{
   public interface IPersistenceDataService : IPersistenceService
   {
      Task<Result> GetAllMeasurementResults();
      Task<Result> GetAllMeasurementResultsForPlan(Guid planId);
      Task<Result> GetAllCharacteristicActuals(Guid measurementResultId);
      Task<Result> GetCharacteristicActual(Guid measurementResultId, Guid characteristicActualId);
      Task<Result> GetReportTemplatesLow();
      Task<Result> GetReportTemplateMed(Guid reportTemplateGuid);
      Task<Result> GetParts();
      Task<Result> GetPropertyDefinitions(Web.Data.EntityType entityType);
      Task<Result> GetAllLatestPlansLowByPart(Guid partId);
      Task<Result> GetPlanVersionsMed(Guid planId, VersionFilter versionFilter);
      Task<Result> AddReportTemplateMed(string name, string template);
      Task<Result> UpdateReportTemplate(Guid reportTemplateId, ReportTemplateUpdateDTO reportTemplateUpdateDto);
      Task<Result> DeleteReportTemplate(Guid reportTemplateId);
      Task<Result> GetAllLatestPlansLow();
      Task<Result> GetPlanByIdLow(Guid planId);
      Task<Result> GetMeasurementResult(Guid measurementResultId);
      Task<Result> GetMeasurementResultWithAttachments(Guid measurementResultId);
      Task<Result> DeletePlan(Guid planId, VersionFilter version);
      Task<Result> DeletePart(Guid partId);
      Task<Result> DeleteMeasurementResult(Guid measurementResultId);
   }
}
