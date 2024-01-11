// <copyright file="VMToggleReportViewTest.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.ReportModule.App.AppState;
using Mitutoyo.MiCAT.ReportModule.GUI.Common.Filebar;
using Mitutoyo.MiCAT.ReportModuleApp.Controllers;
using Mitutoyo.MiCAT.ReportModuleApp.Utilities;
using Moq;
using NUnit.Framework;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Test.Common.Filebar
{
   [TestFixture]
   [ExcludeFromCodeCoverage]
   public class VMToggleReportViewTest
   {
      private Mock<IReportFileBarController> _reportFileBarControllerMock;
      private Mock<IAppStateHistory> _historyMock;
      private Mock<IActionCaller> _actionCallerMock;
      private VMToggleReportView _toggleView;

      [SetUp]
      public void SetUp ()
      {
         _reportFileBarControllerMock = new Mock<IReportFileBarController>();
         _historyMock = new Mock<IAppStateHistory>();
         _historyMock.Setup(h => h.AddClient(It.IsAny<ISubscriptionClient>(), 0)).Returns(_historyMock.Object);
         _actionCallerMock = new Mock<IActionCaller>();
         _toggleView = new VMToggleReportView(_reportFileBarControllerMock.Object, _historyMock.Object, _actionCallerMock.Object);
      }

      [Test]
      public void ToggleViewDefaultStateTest()
      {
         Assert.IsFalse(_toggleView.IsChecked);
      }

      [Test]
      public void ToggleViewUpdateEditModeTest()
      {
         //Arrange
         var snapShot = new Mock<ISnapShot>();
         var rms = new ReportModeState(true);

         snapShot.Setup(s => s.GetItems<ReportModeState>()).Returns(new ReportModeState[] { rms });

         //Act
         _toggleView.Update(snapShot.Object);

         //Assert
         Assert.IsTrue(_toggleView.IsChecked);
      }

      [Test]
      public void ToggleViewUpdatePreviewModeTest()
      {
         //Arrange
         var snapShot = new Mock<ISnapShot>();
         var rms = new ReportModeState(false);

         snapShot.Setup(s => s.GetItems<ReportModeState>()).Returns(new ReportModeState[] { rms });

         //Act
         _toggleView.Update(snapShot.Object);

         //Assert
         Assert.IsFalse(_toggleView.IsChecked);
      }
   }
}
