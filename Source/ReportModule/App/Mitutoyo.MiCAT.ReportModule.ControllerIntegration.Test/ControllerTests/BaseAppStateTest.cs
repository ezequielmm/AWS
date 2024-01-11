// <copyright file="BaseAppStateTest.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using System.Diagnostics.CodeAnalysis;
using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.Common;
using NUnit.Framework;

namespace Mitutoyo.MiCAT.ReportModule.ControllerIntegration.Test.ControllerTests
{
   [ExcludeFromCodeCoverage]
   public abstract class BaseAppStateTest
   {
      protected AppStateConfig _appStateConfig;
      protected SnapShotCounters _snapShotCounters;
      protected FolderPath _baseFolder;
      protected IAppStateHistory _history;

      [SetUp]
      public void SetUpBaseAppStateTest()
      {
         var foldersConfig = new FoldersConfig();
         _baseFolder = foldersConfig.TestAppStateFolder;
         _baseFolder.Delete();
      }

      protected void SetUpHelper(Func<ISnapShot, ISnapShot> collectionAdder = null)
      {
         _appStateConfig = _appStateConfig ?? CreateAppStateConfig(_baseFolder);
         _snapShotCounters = _snapShotCounters ?? SnapShotCountersCreator.Create(true, false);
         _history = AppStateHistoryFactory.CreateEmptyHistory(_snapShotCounters.UniqueValueFactory, _appStateConfig);
         collectionAdder = collectionAdder ?? (s => s);

         var firstSnapShot = AppStateHistoryFactory.CreateFirstSnapShot(collectionAdder, _history, _snapShotCounters);
         firstSnapShot = firstSnapShot.ClearChanges();

         _history.AddFirstSnapShot(firstSnapShot, AppStateHistoryOption.AddButDontWrite);
      }

      private static AppStateConfig CreateAppStateConfig(FolderPath baseFolder)
      {
         var appStateConfig = new AppStateConfig(baseFolder);
         appStateConfig.HistoryOption = AppStateHistoryOption.AddButDontWrite;
         appStateConfig.WriteDataFiles = false;
         appStateConfig.FullSnapShotFrequency = 1;
         appStateConfig.NestedFolderDepth = 0;
         return appStateConfig;
      }
   }
}