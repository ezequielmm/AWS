// <copyright file="VMAppStateTestManager.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using System.Drawing.Printing;
using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.ApplicationState.Controllers;
using Mitutoyo.MiCAT.Common;
using Mitutoyo.MiCAT.ReportModule.App.AppState;
using Mitutoyo.MiCAT.ReportModule.Domain;
using Mitutoyo.MiCAT.ReportModule.Domain.Components;
using Mitutoyo.MiCAT.ReportModule.Domain.Components.ReportSections;
using Mitutoyo.MiCAT.ReportModule.Domain.Data;
using Mitutoyo.MiCAT.ReportModule.Domain.Visibility;
using Mitutoyo.MiCAT.ReportModule.GUI.Test.VisualElements.ReportElements;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Test
{
   [ExcludeFromCodeCoverage]
   public class VMAppStateTestManager
   {
      private readonly AppStateConfig _appStateConfig;
      private readonly SnapShotCounters _snapShotCounters;
      private readonly FolderPath _baseFolder;

      internal IAppStateHistory History { get; }

      internal ISnapShot CurrentSnapShot { get; private set; }

      internal VMAppStateTestManager()
      {
         _baseFolder = CreateBaseFolder();

         _appStateConfig = _appStateConfig ?? CreateAppStateConfig(_baseFolder);
         _snapShotCounters = _snapShotCounters ?? SnapShotCountersCreator.Create(true, false);
         History = AppStateHistoryFactory.CreateEmptyHistory(_snapShotCounters.UniqueValueFactory, _appStateConfig);
         var firstSnapShot = AppStateHistoryFactory.CreateFirstSnapShot(AddCollections, History, _snapShotCounters);
         firstSnapShot = firstSnapShot.ClearChanges();
         History.AddFirstSnapShot(firstSnapShot, AppStateHistoryOption.AddButDontWrite);

         CurrentSnapShot = History.NextSnapShot(ControllerCall.Empty);
         CurrentSnapShot = CurrentSnapShot.AddItem(new ReportModeState(false));
         CurrentSnapShot = CurrentSnapShot.AddItem(new ReportComponentSelection());
         CurrentSnapShot = CurrentSnapShot.AddItem(new ReportSectionSelection());
         CurrentSnapShot = CurrentSnapShot.AddItem(new RunSelection());
         CurrentSnapShot = CurrentSnapShot.AddItem(new CommonPageLayout(new PageSizeInfo(PaperKind.Letter, 1056, 816), new Margin(MarginKind.Narrow, 0, 48, 0, 48), new HeaderData(100), new FooterData(100)));
         CurrentSnapShot = CurrentSnapShot.AddItem(new AllCharacteristicTypes(System.Collections.Immutable.ImmutableList.Create(string.Empty)));
         CurrentSnapShot = CurrentSnapShot.AddItem(new AllCharacteristicDetails(System.Collections.Immutable.ImmutableList.Create(string.Empty)));
         CurrentSnapShot = CurrentSnapShot.AddItem(new ReportHeader());
         CurrentSnapShot = CurrentSnapShot.AddItem(new ReportBody());
         CurrentSnapShot = CurrentSnapShot.AddItem(new ReportFooter());
         CurrentSnapShot = CurrentSnapShot.AddItem(new ViewVisibility(ViewElement.Plans, true));
         CurrentSnapShot = CurrentSnapShot.AddItem(new ViewVisibility(ViewElement.Runs, true));

         History.AddSnapShot(CurrentSnapShot);
      }

      public void UpdateSnapshot(IStateItem entity)
      {
         History.Run(snapShot => UpdateSnapShot(snapShot, entity));
         CurrentSnapShot = History.CurrentSnapShot;
      }
      private ISnapShot UpdateSnapShot(ISnapShot snapShot, IStateItem entity)
      {
         if (snapShot.ContainsItem(entity.Id))
         {
            var previousEntity = snapShot.GetItem(entity.Id);

            snapShot = snapShot.UpdateItem(previousEntity, entity);
         }
         else
         {
            snapShot = snapShot.AddItem(entity);
         }
         return snapShot;
      }

      public void DeleteItemFromSnapshot(IStateItem entity)
      {
         History.Run(snapShot => DeletItemFromSnapShot(snapShot, entity));
         CurrentSnapShot = History.CurrentSnapShot;
      }
      private ISnapShot DeletItemFromSnapShot(ISnapShot snapShot, IStateItem entity)
      {
         return snapShot.DeleteItem(entity.Id);
      }

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

      private ISnapShot AddCollections(ISnapShot snapShot)
      {
         snapShot = snapShot.AddCollection<ReportTextBox>(AppStateKinds.Undoable);
         snapShot = snapShot.AddCollection<ReportTableView>(AppStateKinds.Undoable);
         snapShot = snapShot.AddCollection<ReportImage>(AppStateKinds.Undoable);
         snapShot = snapShot.AddCollection<ReportHeaderForm>(AppStateKinds.Undoable);
         snapShot = snapShot.AddCollection<ReportTessellationView>(AppStateKinds.Undoable);
         snapShot = snapShot.AddCollection<ReportComponentFake>(AppStateKinds.Undoable);
         snapShot = snapShot.AddCollection<ReportComponentSelection>(AppStateKinds.NonUndoable);
         snapShot = snapShot.AddCollection<ReportSectionSelection>(AppStateKinds.NonUndoable);
         snapShot = snapShot.AddCollection<ReportModeState>(AppStateKinds.NonUndoable);
         snapShot = snapShot.AddCollection<AllCharacteristicTypes>(AppStateKinds.Undoable);
         snapShot = snapShot.AddCollection<AllCharacteristicDetails>(AppStateKinds.Undoable);
         snapShot = snapShot.AddCollection<CommonPageLayout>(AppStateKinds.Undoable);
         snapShot = snapShot.AddCollection<PlansState>(AppStateKinds.NonUndoable);
         snapShot = snapShot.AddCollection<RunSelection>(AppStateKinds.NonUndoable);
         snapShot = snapShot.AddCollection<RunSelectionRequest>(AppStateKinds.NonUndoable);
         snapShot = snapShot.AddCollection<ReportHeader>(AppStateKinds.NonUndoable);
         snapShot = snapShot.AddCollection<ReportBody>(AppStateKinds.Undoable);
         snapShot = snapShot.AddCollection<ReportFooter>(AppStateKinds.NonUndoable);
         snapShot = snapShot.AddCollection<ViewVisibility>(AppStateKinds.NonUndoable);

         return snapShot;
      }
   }
}
