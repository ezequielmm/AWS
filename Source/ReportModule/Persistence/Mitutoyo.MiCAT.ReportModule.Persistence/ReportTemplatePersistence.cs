// <copyright file="ReportTemplatePersistence.cs" company="Mitutoyo Europe GmbH">
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
using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.Core.StdLib.Extensions;
using Mitutoyo.MiCAT.ReportModule.Domain.CADElements;
using Mitutoyo.MiCAT.ReportModule.Domain.Components;
using Mitutoyo.MiCAT.ReportModule.Domain.Components.TableView;
using Mitutoyo.MiCAT.ReportModule.Domain.Data;
using Mitutoyo.MiCAT.ReportModule.Domain.DataResult;
using Mitutoyo.MiCAT.ReportModule.Domain.DataResult.Exceptions;
using Mitutoyo.MiCAT.ReportModule.Persistence.CADElements;
using Mitutoyo.MiCAT.ReportModule.Persistence.Components;
using Mitutoyo.MiCAT.ReportModule.Persistence.Components.TableView;
using Mitutoyo.MiCAT.ReportModule.Persistence.Service;
using Mitutoyo.MiCAT.ReportModule.Persistence.Template;
using Mitutoyo.MiCAT.Web.Data;

namespace Mitutoyo.MiCAT.ReportModule.Persistence
{
   public class ReportTemplatePersistence : IReportTemplatePersistence
   {
      private readonly IPersistenceDataService _service;
      private readonly DomainMapper _domainMapper;
      private readonly IMapper _mapper;
      private IReportTemplateSerializationManager _serializationManager;

      public ReportTemplatePersistence(IPersistenceServiceLocator persistenceServiceLocator, IMapper mapper, IReportTemplateSerializationManager serializationManager)
      {
         _service = persistenceServiceLocator.GetService<IPersistenceDataService>(Location.DataService,
            Mechanism.DataService);
         _domainMapper = new DomainMapper(mapper);
         _mapper = mapper;
         _serializationManager = serializationManager;
      }
      public async Task<IEnumerable<ReportTemplateDescriptor>> GetReportTemplatesDescriptors()
      {
         var reportTemplatesResult = await _service.GetReportTemplatesLow();
         ResultHelper.ThrowIfFailure(reportTemplatesResult, "DataServiceConnectionError");
         var reportTemplates = GetListOrEmpty<ReportTemplateLowDTO>(reportTemplatesResult);
         return _mapper.Map<IEnumerable<ReportTemplateDescriptor>>(reportTemplates);
      }
      public IEnumerable<ReportTemplateDescriptor> GetDefaultReportTemplateDescriptors()
      {
         return _mapper.Map<IEnumerable<ReportTemplateDescriptor>>(ReportTemplateDefaultHelper
            .GetReportTemplateDescriptorDefaults());
      }

      public Task<ReportTemplate> AddTemplate(ReportTemplate reportTemplate)
      {
         return PersistReportTemplate(reportTemplate, AddReportTemplate);
      }

      public Task<ReportTemplate> UpdateTemplate(ReportTemplate reportTemplate)
      {
         return PersistReportTemplate(reportTemplate, UpdateReportTemplate);
      }

      private async Task<ReportTemplate> PersistReportTemplate(ReportTemplate reportTemplate, Func<ReportTemplate, string, Task<Result>> persistenceFunc)
      {
         string serializeTemplate = GetSerializedTemplate(reportTemplate);

         var result = await persistenceFunc(reportTemplate, serializeTemplate);

         ResultHelper.ThrowIfFailure(result, errorCode =>
         {
            switch (errorCode)
            {
               case ResultErrorCode.NotFound: return new ReportTemplateNotFoundException(result.Message);
               default: return new ResultException(result.Message, "DataServiceConnectionError");
            }
         });
         return GetReportTemplate((result as SuccessResult<ReportTemplateMediumDTO>).Result);
      }

      private async Task<Result> UpdateReportTemplate(ReportTemplate reportTemplate, string xmlTemplate)
      {
         Result result;
         var reportTemplateUpdateDto = new ReportTemplateUpdateDTO()
         {
            Name = reportTemplate.TemplateDescriptor.Name,
            Template = xmlTemplate
         };
         result = await _service.UpdateReportTemplate(reportTemplate.TemplateDescriptor.TemplateId,
            reportTemplateUpdateDto);
         return result;
      }

      private async Task<Result> AddReportTemplate(ReportTemplate reportTemplate, string xmlTemplate)
      {
         Result result;
         result = await _service.AddReportTemplateMed(reportTemplate.TemplateDescriptor.Name, xmlTemplate);
         return result;
      }

      public async Task<ReportTemplate> GetReportTemplate(Guid reportTemplateGuid)
      {
         var reportTemplatesResult = await _service.GetReportTemplateMed(reportTemplateGuid);
         ResultHelper.ThrowIfFailure(reportTemplatesResult, "DataServiceConnectionError");
         var template = GetReportTemplate((reportTemplatesResult as SuccessResult<ReportTemplateMediumDTO>).Result);
         return template;
      }
      public ReportTemplate GetDefaultReportTemplate(Guid reportTemplateGuid)
      {
         var reportTemplatesResultDefault =
            ReportTemplateDefaultHelper.GetReportTemplateDefaultById(reportTemplateGuid);
         if (reportTemplatesResultDefault == null) return null;
         var template = GetReportTemplate(_mapper.Map<ReportTemplateMediumDTO>(reportTemplatesResultDefault));
         return template;
      }
      public async Task DeleteReportTemplate(Guid reportTemplateId)
      {
         var result = await _service.DeleteReportTemplate(reportTemplateId);
         ResultHelper.ThrowIfFailure(result, "DeletingDataError");
      }

      private IEnumerable<T> GetListOrEmpty<T>(Result result)
      {
         return result is SuccessResult<IEnumerable<T>> succesResult ? succesResult.Result : Enumerable.Empty<T>();
      }

      private string GetSerializedTemplate(ReportTemplate reportTemplate)
      {
         var reportTemplateSO = MapToReportTemplateSO(reportTemplate);
         return _serializationManager.Serialize(reportTemplateSO);
      }

      private ReportTemplate GetReportTemplate(ReportTemplateMediumDTO reportTemplateDTO)
      {
         return _serializationManager.Deserialize(reportTemplateDTO);
      }

      private ReportTemplateSO MapToReportTemplateSO(ReportTemplate reportTemplate)
      {
         var reportTemplateSO = new ReportTemplateSO
         {
            CommonPageLayout = _mapper.Map<CommonPageLayoutSO>(reportTemplate.CommonPageLayout),
            CadLayouts = MapEnumerable<CADLayout, CADLayoutSO>(reportTemplate.CadLayouts),
            ReportComponents = MapToEnumerableReportComponentSO(reportTemplate.ReportComponents),
            ReportComponentDataItems = MapEnumerable<IStateItem, object>(reportTemplate.ReportComponentDataItems),
         };

         return reportTemplateSO;
      }

      private List<ReportComponentSO> MapToEnumerableReportComponentSO(IEnumerable<IReportComponent> reportComponents)
      {
         var list = new List<ReportComponentSO>();

         reportComponents.ForEach(rc =>
         {
            ReportComponentSO reportComponentSO = null;

            switch (rc)
            {
               case ReportTextBox reportTextBox:
                  reportComponentSO = CreateReportComponentSO<ReportTextBoxSO, ReportTextBox>(reportTextBox, MapToReportTextBoxSO);
                  break;

               case ReportImage reportImage:
                  reportComponentSO = CreateReportComponentSO<ReportImageSO, ReportImage>(reportImage, MapToReportImageSO);
                  break;

               case ReportHeaderForm reportHeaderForm:
                  reportComponentSO = CreateReportComponentSO<ReportHeaderFormSO, ReportHeaderForm>(reportHeaderForm, MapToReportHeaderFormSO);
                  break;

               case ReportTableView reportTableView:
                  reportComponentSO = CreateReportComponentSO<ReportTableViewSO, ReportTableView>(reportTableView, MapToReportTableViewSO);
                  break;

               case ReportTessellationView reportTessellationView:
                  reportComponentSO = CreateReportComponentSO<ReportTessellationViewSO, ReportTessellationView>(reportTessellationView, MapToTessellationViewSO);
                  break;

               default:
                  throw new NotImplementedException();
            }

            list.Add(reportComponentSO);
         });

         return list;
      }

      private TReportComponentSO CreateReportComponentSO<TReportComponentSO, TReportComponent>(TReportComponent reportComponent, Func<TReportComponentSO, TReportComponent, TReportComponentSO> MapToFunc)
         where TReportComponentSO : ReportComponentSO, new()
         where TReportComponent : IReportComponent
      {
         var reportComponentSO = new TReportComponentSO
         {
            Id = reportComponent.Id.UniqueValue.Value,
            X = reportComponent.Placement.X,
            Y = reportComponent.Placement.Y,
            Width = reportComponent.Placement.Width,
            Height = reportComponent.Placement.Height
         };

         return MapToFunc(reportComponentSO, reportComponent);
      }

      private ReportTextBoxSO MapToReportTextBoxSO(ReportTextBoxSO reportTextBoxSO, ReportTextBox reportTextbox)
      {
         reportTextBoxSO.Text = reportTextbox.Text;
         return reportTextBoxSO;
      }

      private ReportImageSO MapToReportImageSO(ReportImageSO reportImageSO, ReportImage reportImage)
      {
         reportImageSO.Image = reportImage.Image;
         return reportImageSO;
      }

      private ReportHeaderFormSO MapToReportHeaderFormSO(ReportHeaderFormSO reportHeaderFormSO, ReportHeaderForm reportHeaderForm)
      {
         reportHeaderFormSO.RowIds = reportHeaderForm.RowIds.Select(r => r.UniqueValue.Value).ToList();
         return reportHeaderFormSO;
      }

      private ReportTableViewSO MapToReportTableViewSO(ReportTableViewSO reportTableViewSO, ReportTableView reportTableView)
      {
         reportTableViewSO.Columns = MapEnumerable<Column, ColumnSO>(reportTableView.Columns);
         reportTableViewSO.Sorting = MapEnumerable<SortingColumn, SortingColumnSO>(reportTableView.Sorting);
         reportTableViewSO.Filters = MapEnumerable<FilterColumn, FilterColumnSO>(reportTableView.Filters);
         reportTableViewSO.GroupBy = reportTableView.GroupBy.ToList();

         return reportTableViewSO;
      }

      private ReportTessellationViewSO MapToTessellationViewSO(ReportTessellationViewSO reportTessellationViewSO, ReportTessellationView reportTessellationView)
      {
         return reportTessellationViewSO;
      }

      private List<TDestination> MapEnumerable<TSource, TDestination>(IEnumerable<TSource> sources)
      {
         return sources.Select(s => _mapper.Map<TDestination>(s)).ToList();
      }
   }
}
