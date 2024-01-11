// <copyright file="ReportWorkspaceStartup.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Mitutoyo.MiCAT.Applications.Services.Core;
using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.Core.StdLib.Extensions;
using Mitutoyo.MiCAT.ReportModule.App.AppState;
using Mitutoyo.MiCAT.ReportModule.Domain.Components;
using Mitutoyo.MiCAT.ReportModule.Domain.Data;
using Mitutoyo.MiCAT.ReportModule.Domain.Visibility;
using Mitutoyo.MiCAT.ReportModule.GUI.Utilities.Events;
using Mitutoyo.MiCAT.ReportModuleApp.AppState;
using Mitutoyo.MiCAT.Utilities.IoC;

namespace Mitutoyo.MiCAT.ReportModule.Setup.ReportWorkspace
{
   public class ReportWorkspaceStartUp : IReportWorkspaceStartUp
   {
      private readonly IServiceLocator _serviceLocator;

      public ReportWorkspaceStartUp(IServiceLocator serviceLocator)
      {
         _serviceLocator = serviceLocator;
      }

      public async Task Start(SelectedReportTemplateInfo initOptions)
      {
         var history = _serviceLocator.Resolve<IAppStateHistory>();
         var unsavedChangesService = _serviceLocator.Resolve<IUnsavedChangesService>();

         var dataServiceStateData = await _serviceLocator.Resolve<DataServiceStateDataLoader>().GetData();

         await history.RunUndoableAsync(snapShot => AddFirstSnapShotToHistory(snapShot, initOptions, dataServiceStateData));

         unsavedChangesService.SaveSnapShot(history.CurrentSnapShot);
      }
      private ISnapShot AddFirstSnapShotToHistory(ISnapShot snapShot,
         SelectedReportTemplateInfo initOptions,
         DataServiceSetupData dataServiceStateData)
      {
         snapShot = snapShot
            .AddItem(CreateDefaultReportModeState(initOptions.ReportMode))
            .AddItem(CreateDefaultTemplateDescriptorState(initOptions.ReportTemplate))
            .AddItem(CreateDefaultPlansState(dataServiceStateData.Plans))
            .AddItem(CreateDefaultRunSelection())
            .AddItem(CreateDefaultRunSelectionRequest())
            .AddItem(CreateDefaultDynamicPropertiesState(dataServiceStateData.DynamicProperties))
            .AddItem(CreateDefaultCharacteristicTypes(dataServiceStateData.CharacteristicsTypes))
            .AddItem(CreateDefaultDetails(dataServiceStateData.AllCharacteristicDetails))
            .AddItem(CreateDefaultPlansVisibility())
            .AddItem(CreateDefaultRunsVisibility());

         return AddTemplateDataToSnapShot(initOptions.ReportTemplate, snapShot);
      }

      private ISnapShot AddTemplateDataToSnapShot(ReportTemplate template, ISnapShot snapShot)
      {
         snapShot = snapShot
            .AddItem(template.CommonPageLayout)
            .AddItem(template.ReportHeader)
            .AddItem(template.ReportBody)
            .AddItem(template.ReportFooter);

         template
            .ReportComponentDataItems
            .ForEach(x => snapShot = snapShot.AddItem(x));

         template.ReportComponents.ForEach(rc => snapShot = snapShot.AddItem(rc));

         template.CadLayouts.ForEach(cl => snapShot = snapShot.AddItem(cl));

         snapShot = snapShot.AddItem(CreateSectionSelectionState(new List<IItemId> { template.ReportBody.Id }));

         if (template.ReportComponents.Count() > 0)
            snapShot = snapShot.AddItem(CreateSelectedComponentState(template.ReportComponents));
         else
            snapShot = snapShot.AddItem(CreateDefaultSelectedComponentState());

         return snapShot;
      }

      private TemplateDescriptorState CreateDefaultTemplateDescriptorState(ReportTemplate template) =>
         new TemplateDescriptorState(template.TemplateDescriptor);

      private ReportModeState CreateDefaultReportModeState(ReportMode reportMode) =>
         new ReportModeState(reportMode == ReportMode.EditMode);

      private ReportComponentSelection CreateSelectedComponentState(IEnumerable<IReportComponent> reportComponents) =>
         new ReportComponentSelection(new[] { reportComponents.First().Id as IItemId<IReportComponent>});

      private PlansState CreateDefaultPlansState(IEnumerable<PlanDescriptor> plans) =>
         new PlansState(plans.ToImmutableList());

      private DynamicPropertiesState CreateDefaultDynamicPropertiesState(IEnumerable<DynamicPropertyDescriptor> dynamicProperties) =>
         new DynamicPropertiesState(new UniqueValueFactory(), dynamicProperties.ToImmutableList());

      private AllCharacteristicTypes CreateDefaultCharacteristicTypes(IEnumerable<string> characteristicTypes) =>
         new AllCharacteristicTypes(characteristicTypes.ToImmutableList());

      private AllCharacteristicDetails CreateDefaultDetails(IEnumerable<string> details) =>
         new AllCharacteristicDetails(details.ToImmutableList());

      private ReportSectionSelection CreateSectionSelectionState(List<IItemId> sectionIdsSelected) =>
         new ReportSectionSelection(sectionIdsSelected);

      private ViewVisibility CreateDefaultPlansVisibility() =>
         new ViewVisibility(ViewElement.Plans, true);

      private ViewVisibility CreateDefaultRunsVisibility() =>
         new ViewVisibility(ViewElement.Runs, true);

      private RunSelectionRequest CreateDefaultRunSelectionRequest() =>
         new RunSelectionRequest();

      private RunSelection CreateDefaultRunSelection() =>
         new RunSelection();

      private ReportComponentSelection CreateDefaultSelectedComponentState() =>
         new ReportComponentSelection();
   }
}
