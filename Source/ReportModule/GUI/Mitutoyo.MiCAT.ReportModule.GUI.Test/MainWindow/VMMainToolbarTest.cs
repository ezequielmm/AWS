// <copyright file="VMMainToolbarTest.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.ReportModule.App.AppState;
using Mitutoyo.MiCAT.ReportModule.GUI.Common.BusyIndicators;
using Mitutoyo.MiCAT.ReportModule.GUI.MainWindow;
using Mitutoyo.MiCAT.ReportModuleApp.Controllers;
using Mitutoyo.MiCAT.ReportModuleApp.Utilities;
using Moq;
using NUnit.Framework;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Test.MainWindow
{
   [ExcludeFromCodeCoverage]
   [TestFixture]
   public class VMMainToolbarTest
   {
      private Mock<IAppStateHistory> _historyMock;
      private Mock<IPdfExportController> _pdfPersistenceControllerMock;
      private Mock<IActionCaller> _actionCallerMock;
      private Mock<IBusyIndicator> _busyIndicatorMock;

      [SetUp]
      public void Setup()
      {
         _historyMock = new Mock<IAppStateHistory>();
         _historyMock.Setup(h => h.AddClient(It.IsAny<ISubscriptionClient>(), 0)).Returns(_historyMock.Object);
         _pdfPersistenceControllerMock = new Mock<IPdfExportController>();
         _actionCallerMock = new Mock<IActionCaller>();
         _busyIndicatorMock = new Mock<IBusyIndicator>();
      }

      [Test]
      public void VMMainToolbar_ShouldExportAsEnabledIfIsNotEditMode()
      {
         // Arrange
         var vm = new VMMainToolbar(
           _historyMock.Object,
           _actionCallerMock.Object,
           _busyIndicatorMock.Object,
           _pdfPersistenceControllerMock.Object
           );

         var snapShotMock = new Mock<ISnapShot>();

         snapShotMock
            .Setup(ss => ss.GetItems<ReportModeState>()).Returns(new[] { new ReportModeState(false) });

         // Act
         vm.Update(snapShotMock.Object);

         // Assert
         Assert.IsTrue(vm.IsExportAsEnabled);
      }

      [Test]
      public void VMMainToolbar_ShouldNotExportAsEnabledIfIsEditMode()
      {
         // Arrange
         var vm = new VMMainToolbar(
           _historyMock.Object,
           _actionCallerMock.Object,
           _busyIndicatorMock.Object,
           _pdfPersistenceControllerMock.Object
           );

         var snapShotMock = new Mock<ISnapShot>();

         snapShotMock
            .Setup(ss => ss.GetItems<ReportModeState>()).Returns(new[] { new ReportModeState(true) });

         // Act
         vm.Update(snapShotMock.Object);

         // Assert
         Assert.IsFalse(vm.IsExportAsEnabled);
      }
   }
}