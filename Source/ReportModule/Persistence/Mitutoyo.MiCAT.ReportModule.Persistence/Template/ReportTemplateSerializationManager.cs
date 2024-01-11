// <copyright file="ReportTemplateSerializationManager.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using AutoMapper;
using Mitutoyo.MiCAT.ReportModule.Domain;
using Mitutoyo.MiCAT.ReportModule.Domain.Data;
using Mitutoyo.MiCAT.ReportModule.Domain.DataResult;
using Mitutoyo.MiCAT.ReportModule.Persistence.Components;
using Mitutoyo.MiCAT.ReportModule.Persistence.Template.CurrentVersion;
using Mitutoyo.MiCAT.Web.Data;

namespace Mitutoyo.MiCAT.ReportModule.Persistence.Template
{
   public class ReportTemplateSerializationManager : IReportTemplateSerializationManager
   {
      private readonly DomainMapper _domainMapper;
      private IReportTemplateContentSerializerSelector _serializerSelector;
      private IReportTemplateVersionProvider _currentVersionProvider;
      private IReportTemplateSerializer _reportTemplateSerializer;

      public ReportTemplateSerializationManager(IMapper mapper, IReportTemplateContentSerializerSelector serializerSelector,
         IReportTemplateVersionProvider currentVersionProvider, IReportTemplateSerializer reportTemplateSerializer)
      {
         _domainMapper = new DomainMapper(mapper);
         _serializerSelector = serializerSelector;
         _currentVersionProvider = currentVersionProvider;
         _reportTemplateSerializer = reportTemplateSerializer;
      }

      public ReportTemplate Deserialize(ReportTemplateMediumDTO reportTemplateDTO)
      {
         var template = TryDeserializeTemplate(reportTemplateDTO.Template);

         var templateSO = TryDeserializeContentData((template as SuccessResult<ReportTemplateDataSO>).Result);

         var templateDescriptor = new TemplateDescriptor()
         {
            TemplateId = reportTemplateDTO.Id,
            Name = reportTemplateDTO.Name,
            ReadOnly = reportTemplateDTO.ReadOnly
         };
         var templaSoResult = (templateSO as SuccessResult<ReportTemplateSO>).Result;

         //REFACTOR_VERSION: this should be addressed when backward compatibility is done
         if (templaSoResult.CommonPageLayout.CanvasMargin.MarginKind == MarginKind.None)
         {
            templaSoResult.CommonPageLayout.CanvasMargin = new MarginSO() { Top = 48, Left = 0, Bottom = 48, Right = 0, MarginKind = MarginKind.Narrow };
         }

         return _domainMapper.Map(templaSoResult, templateDescriptor);
      }

      public string Serialize(ReportTemplateSO reportTemplate)
      {
         var reportTemplateMetadaSO = new VersionIdentifierSO();
         reportTemplateMetadaSO.Version = _currentVersionProvider.GetCurrentVersion();
         reportTemplateMetadaSO.DataType = "ReportTemplate";

         var templateSO = TrySerializeContentData(reportTemplateMetadaSO, reportTemplate);

         var reportTemplateDataSO = new ReportTemplateDataSO();
         reportTemplateDataSO.VersionIdentifier = reportTemplateMetadaSO;
         reportTemplateDataSO.ReportTemplateContent = (templateSO as SuccessResult<string>).Result;

         var template = TrySerializeDataSO(reportTemplateDataSO);

         return (template as SuccessResult<string>).Result;
      }

      private Result TryDeserializeContentData(ReportTemplateDataSO template)
      {
         var contentSerializer = _serializerSelector.Select(template.VersionIdentifier);
         var templateSO = contentSerializer.Deserialize(template.ReportTemplateContent);
         ResultHelper.ThrowIfFailure(templateSO, "InvalidTemplateError");
         return templateSO;
      }

      private Result TrySerializeContentData(VersionIdentifierSO versionIdentifier, ReportTemplateSO template)
      {
         var contentSerializer = _serializerSelector.Select(versionIdentifier);
         var templateSO = contentSerializer.Serialize(template);
         ResultHelper.ThrowIfFailure(templateSO, "InvalidTemplateError");
         return templateSO;
      }

      private Result TrySerializeDataSO(ReportTemplateDataSO templateSO)
      {
         var template = _reportTemplateSerializer.Serialize(templateSO);
         ResultHelper.ThrowIfFailure(template, "InvalidTemplateError");
         return template;
      }

      private Result TryDeserializeTemplate(string template)
      {
         var templateResult = _reportTemplateSerializer.Deserialize(template);
         ResultHelper.ThrowIfFailure(templateResult, "InvalidTemplateError");
         return templateResult;
      }
   }
}
