// <copyright file="ReportModePropertyTest.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Mitutoyo.MiCAT.ReportModule.App.AppState;
using Mitutoyo.MiCAT.ReportModule.GUI.Common;
using Mitutoyo.MiCAT.ReportModule.GUI.Test.AppStateHelper;
using NUnit.Framework;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Test.Common
{
   [ExcludeFromCodeCoverage]
   public class ReportModePropertyTest : BaseAppStateTest
   {
      private VMAppStateTestManager _vmAppStateTestManager;

      [SetUp]
      public void Setup()
      {
         _vmAppStateTestManager = new VMAppStateTestManager();
      }

      [Test]
      public void Update_ShouldRefreshIsEditMode()
      {
         // Arrange
         var sut = new ReportModeProperty(_vmAppStateTestManager.History);
         var reportModeState = _vmAppStateTestManager.History.CurrentSnapShot.GetItems<ReportModeState>().First();

         //Act
         _vmAppStateTestManager.UpdateSnapshot(reportModeState.With(true));
         sut.Update(_vmAppStateTestManager.History.CurrentSnapShot);

         //Assert
         Assert.IsTrue(sut.IsEditMode);
      }

      [Test]
      public void Update_ShouldNotRefreshIsEditMode()
      {
         // Arrange
         var sut = new ReportModeProperty(_vmAppStateTestManager.History);

         //Act
         sut.Update(_vmAppStateTestManager.History.CurrentSnapShot);

         //Assert
         Assert.IsFalse(sut.IsEditMode);
      }
   }
}
