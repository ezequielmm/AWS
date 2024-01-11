// <copyright file="ReportFileBarControllerTest.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.ReportModule.App.AppState;
using Mitutoyo.MiCAT.ReportModuleApp.Controllers;
using NUnit.Framework;

namespace Mitutoyo.MiCAT.ReportModule.ControllerIntegration.Test.ControllerTests
{
   [ExcludeFromCodeCoverage]
   [TestFixture]
   public class ReportFileBarControllerTest : BaseAppStateTest
   {
      private IReportFileBarController _controller;

      private static ISnapShot BuildHelper(ISnapShot snapShot)
      {
         snapShot = snapShot.AddCollection<ReportModeState>(AppStateKinds.Undoable);

         return snapShot;
      }
      private ISnapShot AddComponent(ISnapShot snapShot, ReportModeState reportModeState)
      {
         return snapShot.AddItem(reportModeState);
      }
      [SetUp]
      public virtual void SetUp()
      {
         SetUpHelper(BuildHelper);
         _controller = new ReportFileBarController(_history);
      }

      [Test]
      public void WhenTogleChangeTheChangesShouldBeNotifier()
      {
         //Arrange
         var reportModeState = new ReportModeState(true);
         _history.Run((snapShot) => AddComponent(snapShot, reportModeState));
         // Act
         _controller.ChangeStateToggleButton(false);

         // Assert
         var result = _history.CurrentSnapShot.GetItems<ReportModeState>().First();
         Assert.IsFalse(result.EditMode);
      }
   }
}
