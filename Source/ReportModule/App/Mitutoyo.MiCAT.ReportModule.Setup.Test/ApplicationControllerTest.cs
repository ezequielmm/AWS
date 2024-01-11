// <copyright file="ApplicationControllerTest.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Mitutoyo.MiCAT.Applications.Services.Core;
using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.Common;
using Mitutoyo.MiCAT.Core;
using Mitutoyo.MiCAT.ReportModule.App.AppState;
using Mitutoyo.MiCAT.ReportModule.Domain.CADElements;
using Mitutoyo.MiCAT.ReportModule.Domain.Components;
using Mitutoyo.MiCAT.ReportModule.Domain.Components.ReportSections;
using Mitutoyo.MiCAT.ReportModule.Domain.Data;
using Mitutoyo.MiCAT.ReportModule.Domain.Visibility;
using Mitutoyo.MiCAT.ReportModule.GUI.Utilities.Events;
using Mitutoyo.MiCAT.ReportModule.Persistence;
using Mitutoyo.MiCAT.ReportModuleApp.AppState;
using Mitutoyo.MiCAT.Utilities.IoC;
using Moq;
using NUnit.Framework;

namespace Mitutoyo.MiCAT.ReportModule.Setup.Test
{
   [ExcludeFromCodeCoverage]
   [TestFixture]
   public class ApplicationControllerTest
   {
      private IAppStateHistory _history;
      private AppStateConfig _appStateConfig;
      private SnapShotCounters _snapShotCounters;
      private Mock<IUnsavedChangesService> _unsavedChangesServices;
      private DataServiceStateDataLoader _dataServiceStateDataLoader;
      private Mock<IServiceLocator> _serviceLocator;
      private FolderPath CreateBaseFolder()
      {
         var foldersConfig = new FoldersConfig();
         var baseFolder = foldersConfig.TestAppStateFolder;
         baseFolder.Delete();

         return baseFolder;
      }
      private AppStateConfig CreateAppStateConfig(FolderPath baseFolder)
      {
         var appStateConfig = new AppStateConfig(baseFolder);
         appStateConfig.HistoryOption = AppStateHistoryOption.AddButDontWrite;
         appStateConfig.WriteDataFiles = false;
         appStateConfig.FullSnapShotFrequency = 1;
         appStateConfig.NestedFolderDepth = 0;
         return appStateConfig;
      }
      [SetUp]
      public void Setup()
      {
         _serviceLocator = new Mock<IServiceLocator>();
         _unsavedChangesServices = new Mock<IUnsavedChangesService>();
         var _baseFolder = CreateBaseFolder();

         _appStateConfig = _appStateConfig ?? CreateAppStateConfig(_baseFolder);
         _snapShotCounters = _snapShotCounters ?? SnapShotCountersCreator.Create(true, false);
         _history = AppStateHistoryFactory.CreateEmptyHistory(_snapShotCounters.UniqueValueFactory, _appStateConfig);
         var firstSnapShot = AppStateHistoryFactory.CreateFirstSnapShot(AddCollections, _history, _snapShotCounters);
         firstSnapShot = firstSnapShot.ClearChanges();
         _history.AddFirstSnapShot(firstSnapShot, AppStateHistoryOption.AddButDontWrite);

         Mock<IPartPersistence> partPersistence = new Mock<IPartPersistence>();
         Mock<IPlanPersistence> planPersistence = new Mock<IPlanPersistence>();
         Mock<IDynamicPropertyPersistence> dynamicPropertyPersistence = new Mock<IDynamicPropertyPersistence>();
         Mock<IMeasurementPersistence> measurementPersistence = new Mock<IMeasurementPersistence>();
         Mock<ICharacteristicDetailsProvider> detailsProvider = new Mock<ICharacteristicDetailsProvider>();

         _dataServiceStateDataLoader = new DataServiceStateDataLoader(partPersistence.Object, planPersistence.Object, dynamicPropertyPersistence.Object,
            measurementPersistence.Object, detailsProvider.Object);

         dynamicPropertyPersistence.Setup(d => d.GetDynamicProperties()).
            Returns(Task.FromResult(Enumerable.Repeat(new DynamicPropertyDescriptor(), 2)));
         measurementPersistence.Setup(m => m.GetCharacteristicTypes()).Returns(Array.Empty<string>());
         detailsProvider.Setup(d => d.GetAllCharacteristicDetails()).Returns(Array.Empty<string>().ToImmutableList());

         _serviceLocator.Setup(s => s.Resolve<IAppStateHistory>()).Returns(_history);
         _serviceLocator.Setup(s => s.Resolve<DataServiceStateDataLoader>()).Returns(_dataServiceStateDataLoader);
         _serviceLocator.Setup(s => s.Resolve<IUnsavedChangesService>()).Returns(_unsavedChangesServices.Object);

         _unsavedChangesServices.Setup(u => u.SaveSnapShot(It.IsAny<ISnapShot>()));

         var compositeHasTask = new CompositeHasTask(new TimingContext(ContextKey.None, "key"));
         compositeHasTask.Close();
      }
      private ISnapShot AddCollections(ISnapShot snapShot)
      {
         snapShot = snapShot.AddCollection<ReportTextBox>(AppStateKinds.Undoable);
         snapShot = snapShot.AddCollection<ReportTableView>(AppStateKinds.Undoable);
         snapShot = snapShot.AddCollection<ReportImage>(AppStateKinds.Undoable);
         snapShot = snapShot.AddCollection<ReportHeaderForm>(AppStateKinds.Undoable);
         snapShot = snapShot.AddCollection<ReportTessellationView>(AppStateKinds.Undoable);
         snapShot = snapShot.AddCollection<ReportComponentSelection>(AppStateKinds.Undoable);
         snapShot = snapShot.AddCollection<ReportModeState>(AppStateKinds.NonUndoable);
         snapShot = snapShot.AddCollection<AllCharacteristicTypes>(AppStateKinds.Undoable);
         snapShot = snapShot.AddCollection<AllCharacteristicDetails>(AppStateKinds.Undoable);
         snapShot = snapShot.AddCollection<CommonPageLayout>(AppStateKinds.Undoable);
         snapShot = snapShot.AddCollection<PlansState>(AppStateKinds.NonUndoable);
         snapShot = snapShot.AddCollection<RunSelection>(AppStateKinds.NonUndoable);
         snapShot = snapShot.AddCollection<RunSelectionRequest>(AppStateKinds.NonUndoable);
         snapShot = snapShot.AddCollection<ReportHeader>(AppStateKinds.Undoable);
         snapShot = snapShot.AddCollection<ReportFooter>(AppStateKinds.Undoable);
         snapShot = snapShot.AddCollection<TemplateDescriptorState>(AppStateKinds.Undoable);
         snapShot = snapShot.AddCollection<DynamicPropertiesState>(AppStateKinds.Undoable);
         snapShot = snapShot.AddCollection<CADLayout>(AppStateKinds.Undoable);
         snapShot = snapShot.AddCollection<ReportTextBox>(AppStateKinds.Undoable);
         snapShot = snapShot.AddCollection<ReportImage>(AppStateKinds.Undoable);
         snapShot = snapShot.AddCollection<ReportSectionSelection>(AppStateKinds.Undoable);
         snapShot = snapShot.AddCollection<ViewVisibility>(AppStateKinds.NonUndoable);

         snapShot = snapShot.AddCollection<ReportBody>(AppStateKinds.Undoable);

         return snapShot;
      }

      [Test]
      public async Task AddFirstSnapShotToHistory_Test()
      {
         // Arrange
         SelectedReportTemplateInfo initOptions = new SelectedReportTemplateInfo(new ReportTemplate(), ReportMode.EditMode);

         var sut = new ApplicationControllerFake(_serviceLocator.Object);

         // Act
         await sut.ExecuteAddFirstSnapShotToHistory(initOptions, _history);
         // Assert
         Assert.IsTrue(_history.CurrentSnapShot.GetItems<ReportModeState>().Single().EditMode);
         Assert.AreEqual(_history.CurrentSnapShot.GetItems<TemplateDescriptorState>().Single().TemplateDescriptor, initOptions.ReportTemplate.TemplateDescriptor);
         Assert.AreEqual(2, _history.CurrentSnapShot.GetItems<DynamicPropertiesState>().Single().PropertyValues.Count());
         Assert.AreEqual(0, _history.CurrentSnapShot.GetItems<PlansState>().Single().PlanList.Count());
      }

      [Test]
      public async Task AddTemplateDataToSnapShotWithComponents_Test()
      {
         // Arrange
         var sut = new ApplicationControllerFake(_serviceLocator.Object);
         ReportTemplate reportTemplate = new ReportTemplate();
         var componentList = new List<IReportComponent>();
         reportTemplate.ReportBody = new ReportBody();
         var placementTextbox = new ReportComponentPlacement(reportTemplate.ReportBody.Id, 12, 10, 100, 400);
         var placementImage = new ReportComponentPlacement(reportTemplate.ReportBody.Id, 22, 20, 200, 300);
         IReportComponent textBox = new ReportTextBox(placementTextbox);
         IReportComponent image = new ReportImage(placementImage);

         componentList.Add(textBox);
         componentList.Add(image);

         reportTemplate.ReportComponents = componentList;
         var cadLayout = new CADLayout();
         var cadLayoutList = new List<CADLayout>();
         cadLayoutList.Add(cadLayout);
         reportTemplate.CadLayouts = cadLayoutList;
         SelectedReportTemplateInfo initOptions = new SelectedReportTemplateInfo(reportTemplate, ReportMode.EditMode);
         // Act
         await sut.ExecuteAddFirstSnapShotToHistory(initOptions, _history);

         // Assert
         Assert.AreEqual(1, _history.CurrentSnapShot.GetItems<ReportTextBox>().Count());
         Assert.AreEqual(1, _history.CurrentSnapShot.GetItems<ReportImage>().Count());
         Assert.AreEqual(1, _history.CurrentSnapShot.GetItems<CommonPageLayout>().Count());
         Assert.NotZero(_history.CurrentSnapShot.GetItems<ReportComponentSelection>().First().SelectedReportComponentIds.Count());
      }

      [Test]
      public async Task AddTemplateDataToSnapShotWithoutComponents_Test()
      {
         // Arrange
         var sut = new ApplicationControllerFake(_serviceLocator.Object);
         ReportTemplate reportTemplate = new ReportTemplate();

         var cadLayout = new CADLayout();
         var cadLayoutList = new List<CADLayout>();
         cadLayoutList.Add(cadLayout);
         reportTemplate.CadLayouts = cadLayoutList;
         SelectedReportTemplateInfo initOptions = new SelectedReportTemplateInfo(reportTemplate, ReportMode.EditMode);
         // Act
         await sut.ExecuteAddFirstSnapShotToHistory(initOptions, _history);

         // Assert
         Assert.AreEqual(1, _history.CurrentSnapShot.GetItems<CommonPageLayout>().Count());
         Assert.AreEqual(1, _history.CurrentSnapShot.GetItems<CADLayout>().Count());
         Assert.Zero(_history.CurrentSnapShot.GetItems<ReportComponentSelection>().First().SelectedReportComponentIds.Count());
      }
   }
}
