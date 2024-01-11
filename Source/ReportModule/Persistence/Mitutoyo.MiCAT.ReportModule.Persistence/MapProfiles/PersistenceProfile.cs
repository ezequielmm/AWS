// <copyright file="PersistenceProfile.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.Core.Geometry;
using Mitutoyo.MiCAT.ReportModule.Domain.CADElements;
using Mitutoyo.MiCAT.ReportModule.Domain.CADElements.Callouts2D;
using Mitutoyo.MiCAT.ReportModule.Domain.CADElements.Callouts2D.FeatureCallouts;
using Mitutoyo.MiCAT.ReportModule.Domain.CADElements.Callouts3D;
using Mitutoyo.MiCAT.ReportModule.Domain.Components;
using Mitutoyo.MiCAT.ReportModule.Domain.Components.ReportSections;
using Mitutoyo.MiCAT.ReportModule.Domain.Components.TableView;
using Mitutoyo.MiCAT.ReportModule.Domain.Data;
using Mitutoyo.MiCAT.ReportModule.Persistence.CADElements;
using Mitutoyo.MiCAT.ReportModule.Persistence.CADElements.Callouts2D;
using Mitutoyo.MiCAT.ReportModule.Persistence.CADElements.Callouts2D.FeatureCallouts;
using Mitutoyo.MiCAT.ReportModule.Persistence.CADElements.Callouts3D;
using Mitutoyo.MiCAT.ReportModule.Persistence.Components;
using Mitutoyo.MiCAT.ReportModule.Persistence.Components.TableView;
using Mitutoyo.MiCAT.ReportModule.Persistence.ExtraStateItems.ReportHeaderForm;
using Mitutoyo.MiCAT.Web.Data;

namespace Mitutoyo.MiCAT.ReportModule.Persistence.MapProfiles
{
   public class PersistenceProfile : Profile
   {
      public PersistenceProfile()
      {
         MapFromDomainToSerializationObjects();
         MapFromSerializationToDomainObjects();
         MapToListItem();
      }

      private void MapFromSerializationToDomainObjects()
      {
         CreateMap<CommonPageLayoutSO, CommonPageLayout>().ForMember(r => r.PropertyMetas, org => org.Ignore());

         CreateMap<CommonPageLayoutSO, ReportHeader>()
            .ConvertUsing((so, _, context) => new ReportHeader(context.Mapper.Map<CommonPageLayoutSO, CommonPageLayout>(so).HasHeader()));

         CreateMap<CommonPageLayoutSO, ReportFooter>()
            .ConvertUsing((so, _, context) => new ReportFooter(context.Mapper.Map<CommonPageLayoutSO, CommonPageLayout>(so).HasHeader()));

         var reportBody = new ReportBody(true);

         CreateMap<CommonPageLayoutSO, ReportBody>()
            .ConvertUsing((so) => reportBody);

         CreateMap<MarginSO, Margin>();
         CreateMap<PageSizeInfoSO, PageSizeInfo>();

         CreateMap<ReportComponentSO, IReportComponent>()
            .ForMember(d => d.PropertyMetas, o => o.Ignore());

         CreateMapToDomain<ReportTextBoxSO, ReportTextBox>((context, so) => new ReportTextBox(new ReportComponentPlacement(reportBody.Id, so.X, so.Y, so.Width, so.Height), so.Text));

         CreateMapToDomain<ReportTableViewSO, ReportTableView>((context, so) =>
           new ReportTableView(
                so.Id,
                new ReportComponentPlacement(reportBody.Id, so.X, so.Y, so.Width, so.Height),
                Map<ColumnSO, Column>(context, so.Columns),
                Map<SortingColumnSO, SortingColumn>(context, so.Sorting),
                so.GroupBy,
                Map<FilterColumnSO, FilterColumn>(context, so.Filters)));

         CreateMapToDomain<ReportTessellationViewSO, ReportTessellationView>((context, so) => new ReportTessellationView(new ReportComponentPlacement(reportBody.Id, so.X, so.Y, so.Width, so.Height)));
         CreateMapToDomain<ReportImageSO, ReportImage>((context, so) => new ReportImage(new ReportComponentPlacement(reportBody.Id, so.X, so.Y, so.Width, so.Height), so.Image));
         CreateMapToDomain<ReportHeaderFormSO, ReportHeaderForm>((context, so) => new ReportHeaderForm(new ReportComponentPlacement(reportBody.Id, so.X, so.Y, so.Width, so.Height), Map<Guid, Id<ReportHeaderFormRow>>(context, so.RowIds)));

         CreateMap<object, IStateItem>()
            .Include<ReportHeaderFormRowSO, ReportHeaderFormRow>()
            .Include<ReportHeaderFormFieldSO, ReportHeaderFormField>();

         CreateMap<ColumnSO, Column>();
         CreateMap<SortingColumnSO, SortingColumn>();
         CreateMap<FilterColumnSO, FilterColumn>();

         CreateMap<ReportHeaderFormRowSO, ReportHeaderFormRow>()
            .ForMember(d => d.PropertyMetas, o => o.Ignore())
            .ForMember(d => d.FieldIds, o => o.MapFrom(s => s.FieldIds));

         CreateMap<ReportHeaderFormFieldSO, ReportHeaderFormField>()
            .ForMember(d => d.PropertyMetas, o => o.Ignore())
            .ForMember(d => d.SelectedFieldId, o => o.MapFrom(s => s.SelectedFieldId))
            .ForMember(d => d.CustomLabel, o => o.MapFrom(s => s.CustomLabel))
            .ForMember(d => d.LabelWidthPercentage, o => o.MapFrom(s => s.LabelWidthPercentage));

         CreateMap<AnchorSO, Anchor>();
         CreateMap<Direction3DSO, Domain.CADElements.Direction3D>();
         CreateMap<CADLayoutSO, CADLayout>()
            .ForMember(r => r.PropertyMetas, org => org.Ignore());

         CreateMap<Callout2DSO, Callout2D>()
            .Include<CircleCalloutSO, CircleCallout>()
            .Include<ConeCalloutSO, ConeCallout>()
            .Include<CylinderCalloutSO, CylinderCallout>()
            .Include<LineCalloutSO, LineCallout>()
            .Include<PlaneCalloutSO, PlaneCallout>()
            .Include<PointCalloutSO, PointCallout>()
            .Include<SphereCalloutSO, SphereCallout>();
         CreateMap<CircleCalloutSO, CircleCallout>();
         CreateMap<ConeCalloutSO, ConeCallout>();
         CreateMap<CylinderCalloutSO, CylinderCallout>();
         CreateMap<LineCalloutSO, LineCallout>();
         CreateMap<PlaneCalloutSO, PlaneCallout>();
         CreateMap<PointCalloutSO, PointCallout>();
         CreateMap<SphereCalloutSO, SphereCallout>();
         CreateMap<Callout3DSO, Callout3D>();
         CreateMap<CameraSO, Camera>();
         CreateMap<Point3DSO, Point3D>();
         CreateMap<Point2DSO, Point2D>();
      }

      private void MapFromDomainToSerializationObjects()
      {
         CreateMap<CommonPageLayout, CommonPageLayoutSO>()
            .ForMember(dest => dest.Id, org => org.MapFrom(src => src.Id.UniqueValue.Value));

         CreateMap<Margin, MarginSO>();

         CreateMap<PageSizeInfo, PageSizeInfoSO>();

         CreateMap<Anchor, AnchorSO>();
         CreateMap<CADLayout, CADLayoutSO>()
         .ForMember(so => so.Id, org => org.MapFrom(r => r.Id.UniqueValue.Value));

         CreateMap<Callout2D, Callout2DSO>()
            .Include<CircleCallout, CircleCalloutSO>()
            .Include<ConeCallout, ConeCalloutSO>()
            .Include<CylinderCallout, CylinderCalloutSO>()
            .Include<LineCallout, LineCalloutSO>()
            .Include<PlaneCallout, PlaneCalloutSO>()
            .Include<PointCallout, PointCalloutSO>()
            .Include<SphereCallout, SphereCalloutSO>();
         CreateMap<CircleCallout, CircleCalloutSO>();
         CreateMap<ConeCallout, ConeCalloutSO>();
         CreateMap<CylinderCallout, CylinderCalloutSO>();
         CreateMap<LineCallout, LineCalloutSO>();
         CreateMap<PlaneCallout, PlaneCalloutSO>();
         CreateMap<PointCallout, PointCalloutSO>();
         CreateMap<SphereCallout, SphereCalloutSO>();

         CreateMap<Callout3D, Callout3DSO>();

         CreateMap<Camera, CameraSO>();
         CreateMap<Point3D, Point3DSO>();
         CreateMap<Point2D, Point2DSO>();
         CreateMap<Domain.CADElements.Direction3D, Direction3DSO>();

         CreateMap<IStateItem, object>()
            .Include<ReportHeaderFormRow, ReportHeaderFormRowSO>()
            .Include<ReportHeaderFormField, ReportHeaderFormFieldSO>();

         CreateMap<Column, ColumnSO>();
         CreateMap<SortingColumn, SortingColumnSO>();
         CreateMap<FilterColumn, FilterColumnSO>();

         CreateMap<ReportHeaderFormRow, ReportHeaderFormRowSO>()
            .ForMember(so => so.Id, org => org.MapFrom(r => r.Id.UniqueValue.Value))
            .ForMember(d => d.FieldIds, o => o.ResolveUsing(s => s.FieldIds.Select(x => x.UniqueValue.Value)));

         CreateMap<ReportHeaderFormField, ReportHeaderFormFieldSO>()
            .ForMember(so => so.Id, org => org.MapFrom(r => r.Id.UniqueValue.Value))
            .ForMember(d => d.SelectedFieldId, o => o.MapFrom(s => s.SelectedFieldId))
            .ForMember(d => d.CustomLabel, o => o.MapFrom(s => s.CustomLabel))
            .ForMember(d => d.LabelWidthPercentage, o => o.MapFrom(s => s.LabelWidthPercentage));
      }

      private void MapToListItem()
      {
         CreateMap<ReportTemplateLowDTO, ReportTemplateDescriptor>()
            .ForMember(dest => dest.LocalizedName, org => org.MapFrom(o => o.ReadOnly))
            .ReverseMap();

         CreateMap<PropertyDefinitionDTO, DynamicPropertyDescriptor>();
         CreateMap<PlanMediumDTO, PlanDescriptor>()
            .ForMember(dest => dest.Part, org => org.MapFrom(src => new PartDescriptor { Id = src.PartId ?? Guid.Empty }));

         CreateMap<PlanLowDTO, PlanDescriptor>()
            .ForMember(dest => dest.Part, org => org.MapFrom(src => new PartDescriptor { Id = src.PartId ?? Guid.Empty }));

         CreateMap<PartDTO, PartDescriptor>();

         CreateMap<PropertyValueDTO, DynamicPropertyValue>();
      }

      private IMappingExpression<TReportComponentSO, TReportComponent> CreateMapToDomain<TReportComponentSO, TReportComponent>(Func<ResolutionContext, TReportComponentSO, TReportComponent> ctorFunc)
         where TReportComponent : ReportComponentBase<TReportComponent>
         where TReportComponentSO : ReportComponentSO
      {
         return CreateMap<TReportComponentSO, TReportComponent>()
           .ConstructUsing((so, context) => ctorFunc(context, so))
           .ForMember(so => so.PropertyMetas, cfg => cfg.Ignore())
           .IncludeBase<ReportComponentSO, IReportComponent>();
      }

      private IEnumerable<T2> Map<T1, T2>(ResolutionContext context, IEnumerable<T1> list)
      {
         return list.Select(item => context.Mapper.Map<T1, T2>(item));
      }
   }
}
