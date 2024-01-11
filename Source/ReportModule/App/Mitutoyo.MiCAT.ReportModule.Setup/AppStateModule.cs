// <copyright file="AppStateModule.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.ApplicationState.MetaData;
using Mitutoyo.MiCAT.ApplicationState.Serialization;
using Mitutoyo.MiCAT.Common;
using Mitutoyo.MiCAT.ReportModule.App.AppState;
using Mitutoyo.MiCAT.ReportModule.Domain.CADElements;
using Mitutoyo.MiCAT.ReportModule.Domain.Components;
using Mitutoyo.MiCAT.ReportModule.Domain.Components.ReportSections;
using Mitutoyo.MiCAT.ReportModule.Domain.Data;
using Mitutoyo.MiCAT.ReportModule.Domain.Visibility;
using Mitutoyo.MiCAT.ReportModuleApp.AppState;
using Mitutoyo.MiCAT.Utilities.IoC;

namespace Mitutoyo.MiCAT.ReportModule.Setup
{
   public class AppStateModule : IModule
   {
      private readonly Func<AppStateConfig, AppStateConfig> _appStateConfigModifier;

      static AppStateModule()
      {
         FactoryGetter.Add<RunSelection>(i => RunSelection.PropertiesForAppState(i.Id, i.SelectedRun));
      }

      public AppStateModule()
         : this(null)
      {
      }

      public AppStateModule(Func<AppStateConfig, AppStateConfig> appStateConfigModifier)
      {
         _appStateConfigModifier = appStateConfigModifier;
      }

      public void OnRegister(IServiceRegistrar registrar)
      {
         registrar
            .RegisterSingletonWithFactory(l => CreateAndRegisterAppStateHistory());
      }

      private IAppStateHistory CreateAndRegisterAppStateHistory()
      {
         var baseFolder = new FoldersConfig().ProgramAppStateFolder;
         var snapShotCounters = SnapShotCountersCreator.Create(false);
         var appStateConfig = CreateAppStateConfig(snapShotCounters.UniqueValueFactory, baseFolder);
         if (_appStateConfigModifier != null)
            appStateConfig = _appStateConfigModifier(appStateConfig);
         var history = CreateAppStateHistory(baseFolder, snapShotCounters, appStateConfig);
         return history;
      }
      public static IAppStateHistory CreateAppStateHistory(FolderPath baseFolder, SnapShotCounters shotCounters, AppStateConfig appStateConfig)
      {
         var uniqueValueFactory = shotCounters.UniqueValueFactory;
         var history = AppStateHistoryFactory.CreateEmptyHistory(uniqueValueFactory, baseFolder, appStateConfig);
         var firstSnapShot = CreateFirstSnapShot(history, shotCounters, appStateConfig);
         firstSnapShot = ReadRecentSnapShot(history, firstSnapShot);
         firstSnapShot = firstSnapShot.ClearChanges();
         history.AddFirstSnapShot(firstSnapShot, AppStateHistoryOption.AddAndWrite);
         return history;
      }

      public static AppStateConfig CreateAppStateConfig(UniqueValueFactory uniqueValueFactory, FolderPath baseFolder, bool writeDataFiles = false)
      {
         var appStateFolder = new FolderPath("ReportModule")
            .Combine(uniqueValueFactory.UniqueValue().ToString());
         var appStateConfig = AppStateHistoryFactory.CreateAppStateConfig(baseFolder.Combine(appStateFolder), writeDataFiles, 0);
         appStateConfig.VerifyOptions = VerifyOptions.Read;
         return appStateConfig;
      }

      private static ISnapShot CreateFirstSnapShot(IAppStateHistory history, SnapShotCounters snapShotCounters, AppStateConfig appStateConfig)
      {
         var firstSnapShot = AppStateHistoryFactory.CreateFirstSnapShot(AddReportEntitiesCollections, history, snapShotCounters);
         AppStateHistoryFactory.VerifyCollections(firstSnapShot, appStateConfig);

         firstSnapShot = AppStateInitialize.Initialize(firstSnapShot);
         return firstSnapShot;
      }

      private static ISnapShot ReadRecentSnapShot(IAppStateHistory history, ISnapShot firstSnapShot)
      {
         var recentId = history.ReadSnapShotId();
         if (recentId.IsEmpty)
            return firstSnapShot;
         ISnapShot readSnapShot;
         try
         {
            readSnapShot = history.ReadSnapShot(recentId, firstSnapShot, Variant.Full);
         }
         catch (StateSerializationException)
         {
            return firstSnapShot;
         }
         firstSnapShot = firstSnapShot.ReplaceWith(readSnapShot, AppStateKinds.NonUndoable);
         return firstSnapShot;
      }

      private static ISnapShot AddReportEntitiesCollections(ISnapShot snapShot)
      {
         snapShot = snapShot.AddCollection<CommonPageLayout>(AppStateKinds.Undoable);

         snapShot = snapShot.AddCollection<ReportHeaderForm>(AppStateKinds.Undoable);
         snapShot = snapShot.AddCollection<ReportHeaderFormRow>(AppStateKinds.Undoable);
         snapShot = snapShot.AddCollection<ReportHeaderFormField>(AppStateKinds.Undoable);
         snapShot = snapShot.AddCollection<ReportHeader>(AppStateKinds.Undoable);
         snapShot = snapShot.AddCollection<ReportBody>(AppStateKinds.Undoable);
         snapShot = snapShot.AddCollection<ReportFooter>(AppStateKinds.Undoable);
         snapShot = snapShot.AddCollection<ReportImage>(AppStateKinds.Undoable);
         snapShot = snapShot.AddCollection<ReportTableView>(AppStateKinds.Undoable);
         snapShot = snapShot.AddCollection<ReportTessellationView>(AppStateKinds.Undoable);
         snapShot = snapShot.AddCollection<ReportTextBox>(AppStateKinds.Undoable);
         snapShot = snapShot.AddCollection<PlansState>(AppStateKinds.NonUndoable);
         snapShot = snapShot.AddCollection<RunSelection>(AppStateKinds.NonUndoable);
         snapShot = snapShot.AddCollection<RunSelectionRequest>(AppStateKinds.NonUndoable);
         snapShot = snapShot.AddCollection<ReportModeState>(AppStateKinds.NonUndoable);
         snapShot = snapShot.AddCollection<AllCharacteristicTypes>(AppStateKinds.NonUndoable);
         snapShot = snapShot.AddCollection<TemplateDescriptorState>(AppStateKinds.NonUndoable);
         snapShot = snapShot.AddCollection<ReportComponentSelection>(AppStateKinds.NonUndoable);
         snapShot = snapShot.AddCollection<ReportSectionSelection>(AppStateKinds.NonUndoable);
         snapShot = snapShot.AddCollection<DynamicPropertiesState>(AppStateKinds.NonUndoable);
         snapShot = snapShot.AddCollection<CADLayout>(AppStateKinds.NonUndoable);
         snapShot = snapShot.AddCollection<AllCharacteristicDetails>(AppStateKinds.NonUndoable);
         snapShot = snapShot.AddCollection<ViewVisibility>(AppStateKinds.NonUndoable);

         return snapShot;
      }
   }
}