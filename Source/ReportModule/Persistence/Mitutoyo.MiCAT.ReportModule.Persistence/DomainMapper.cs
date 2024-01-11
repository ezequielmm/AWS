// <copyright file="DomainMapper.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Collections.Generic;
using AutoMapper;
using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.ReportModule.Domain.CADElements;
using Mitutoyo.MiCAT.ReportModule.Domain.Components;
using Mitutoyo.MiCAT.ReportModule.Domain.Components.ReportSections;
using Mitutoyo.MiCAT.ReportModule.Domain.Data;
using Mitutoyo.MiCAT.ReportModule.Persistence.CADElements;
using Mitutoyo.MiCAT.ReportModule.Persistence.Components;

namespace Mitutoyo.MiCAT.ReportModule.Persistence
{
   internal class DomainMapper
   {
      private readonly IMapper _mapper;

      internal DomainMapper(IMapper mapper)
      {
         _mapper = mapper;
      }

      internal ReportTemplate Map(ReportTemplateSO template, TemplateDescriptor descriptor)
      {
         var commonPageLayout = _mapper.Map<CommonPageLayoutSO, CommonPageLayout>(template.CommonPageLayout);
         var reportsComponents = new List<IReportComponent>();

         var reportBody = _mapper.Map<CommonPageLayoutSO, ReportBody>(template.CommonPageLayout);
         var reportHeader = _mapper.Map<CommonPageLayoutSO, ReportHeader>(template.CommonPageLayout);
         var reportFooter = _mapper.Map<CommonPageLayoutSO, ReportFooter>(template.CommonPageLayout);

         template.ReportComponents.ForEach(componentSo =>
         {
            var reportComponentToAdd = _mapper.Map<ReportComponentSO, IReportComponent>(componentSo);

            reportsComponents.Add(reportComponentToAdd);
         });

         List<CADLayout> cadLayouts = new List<CADLayout>();
         foreach (var cl in template.CadLayouts)
         {
            var cadLayoutToAdd = _mapper.Map<CADLayoutSO, CADLayout>(cl);
            cadLayouts.Add(cadLayoutToAdd);
         }

         return new ReportTemplate()
         {
            TemplateDescriptor = descriptor,
            CommonPageLayout = commonPageLayout,
            ReportComponents = reportsComponents,
            ReportComponentDataItems = _mapper.Map<IEnumerable<IStateItem>>(template.ReportComponentDataItems),
            CadLayouts = cadLayouts,
            ReportHeader = reportHeader,
            ReportBody = reportBody,
            ReportFooter = reportFooter,
         };
      }
   }
}
