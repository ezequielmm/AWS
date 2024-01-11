// <copyright file="VMReportFilebarTest.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.Common.Help;
using Mitutoyo.MiCAT.ReportModule.GUI.Common.BusyIndicators;
using Mitutoyo.MiCAT.ReportModule.GUI.Common.Filebar;
using Mitutoyo.MiCAT.ReportModule.GUI.Common.Filebar.MarginsSettings;
using Mitutoyo.MiCAT.ReportModule.GUI.Dialog;
using Mitutoyo.MiCAT.ReportModuleApp.Controllers;
using Mitutoyo.MiCAT.ReportModuleApp.Utilities;
using Moq;
using NUnit.Framework;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Test.ReportFileBar
{
   [TestFixture]
   [ExcludeFromCodeCoverage]
   public class VMReportFileBarTestcs
   {
      private Mock<IAppStateHistory> _historyMock;
      private Mock<IUndoRedoController> undoRedoControllerMock;
      private Mock<IHelpController> helpControllerMock;
      private Mock<VMPageSizeSettings> vmPageSizeSettingsMock;
      private Mock<VMMarginSizeSettings> _vmMarginSizeSettingsMock;
      private Mock<IReportTemplateController> reportTemplateControllerMock;
      private Mock<IDialogService> dialogServiceMock;
      private Mock<IPdfExportController> _pdfExportControllerMock;
      private Mock<IBusyIndicator> _busyIndicatorMock;
      private Mock<IActionCaller> _actionCallerMock;
      private Mock<ICloseController> _closeControllerMock;
      private IActionCaller  _actionCaller;
      private Mock<IViewVisibilityController> _viewVisibilityControllerMock;

      [SetUp]
      public void Setup()
      {
         _historyMock = new Mock<IAppStateHistory>();
         _historyMock.Setup(h => h.AddClient(It.IsAny<ISubscriptionClient>(), 0)).Returns(_historyMock.Object);
         undoRedoControllerMock = new Mock<IUndoRedoController>();
         vmPageSizeSettingsMock = new Mock<VMPageSizeSettings>();
         _vmMarginSizeSettingsMock = new Mock<VMMarginSizeSettings>();
         helpControllerMock = new Mock<IHelpController>();
         reportTemplateControllerMock = new Mock<IReportTemplateController>();
         reportTemplateControllerMock.Setup(x => x.SaveAsCurrentTemplate()).Returns(Task.FromResult(SaveAsResult.Saved));
         dialogServiceMock = new Mock<IDialogService>();
         _pdfExportControllerMock = new Mock<IPdfExportController>();
         _busyIndicatorMock = new Mock<IBusyIndicator>();
         _actionCallerMock = new Mock<IActionCaller>();
         _closeControllerMock = new Mock<ICloseController>();

         _busyIndicatorMock = new Mock<IBusyIndicator>();

         _busyIndicatorMock.Setup(b => b.SetIsBusyTrueUntilUIIsIdle()).Callback(() => { });
         _actionCaller = new BusyIndicatorActionCaller(_busyIndicatorMock.Object);
         _viewVisibilityControllerMock = new Mock<IViewVisibilityController>();
      }

      [Test]
      public async Task VMReportFileBar_SaveCommand()
      {
         var sut = new VMReportFilebar(
           _historyMock.Object,
           undoRedoControllerMock.Object,
           _pdfExportControllerMock.Object,
           vmPageSizeSettingsMock.Object,
           _vmMarginSizeSettingsMock.Object,
           reportTemplateControllerMock.Object,
           dialogServiceMock.Object,
           helpControllerMock.Object,
           _actionCaller,
           _busyIndicatorMock.Object,
           _closeControllerMock.Object,
           _viewVisibilityControllerMock.Object
           );

         // Act
         await sut.SaveCommand.ExecuteAsync(null);

         // Assert
         reportTemplateControllerMock.Verify(c => c.SaveCurrentTemplate(), Times.Once);
      }

      [Test]
      public async Task VMReportFileBar_SaveAsCommand()
      {
         var _actionCaller = (IActionCaller)new BusyIndicatorActionCaller(_busyIndicatorMock.Object);
         var sut = new VMReportFilebar(
           _historyMock.Object,
           undoRedoControllerMock.Object,
           _pdfExportControllerMock.Object,
           vmPageSizeSettingsMock.Object,
           _vmMarginSizeSettingsMock.Object,
           reportTemplateControllerMock.Object,
           dialogServiceMock.Object,
           helpControllerMock.Object,
           _actionCaller,
           _busyIndicatorMock.Object,
           _closeControllerMock.Object,
           _viewVisibilityControllerMock.Object
           );

         // Act
         await sut.SaveAsCommand.ExecuteAsync(null);

         // Assert
         reportTemplateControllerMock.Verify(c => c.SaveAsCurrentTemplate(), Times.Once);
      }

      [Test]
      public void OpenHelpCommand_ShouldCallHelpTopicControllerThroughShellControllerRunner()
      {
         var sut = new VMReportFilebar(
           _historyMock.Object,
           undoRedoControllerMock.Object,
           _pdfExportControllerMock.Object,
           vmPageSizeSettingsMock.Object,
           _vmMarginSizeSettingsMock.Object,
           reportTemplateControllerMock.Object,
           dialogServiceMock.Object,
           helpControllerMock.Object,
           _actionCallerMock.Object,
           _busyIndicatorMock.Object,
           _closeControllerMock.Object,
           _viewVisibilityControllerMock.Object
           );

         sut.OpenHelpCommand.Execute(null);

         helpControllerMock.Verify(x => x.OpenHelp(HelpTopicPath.Home), Times.Once);
      }
      [Test]
      public void VMReportFileBar_CloseCommand()
      {
         var sut = new VMReportFilebar(
           _historyMock.Object,
           undoRedoControllerMock.Object,
           _pdfExportControllerMock.Object,
           vmPageSizeSettingsMock.Object,
           _vmMarginSizeSettingsMock.Object,
           reportTemplateControllerMock.Object,
           dialogServiceMock.Object,
           helpControllerMock.Object,
           _actionCaller,
           _busyIndicatorMock.Object,
           _closeControllerMock.Object,
           _viewVisibilityControllerMock.Object
           );

         // Act
         sut.CloseCommand.Execute(null);

         // Assert
         _closeControllerMock.Verify(c => c.CloseWorkspace(), Times.Once);
      }
   }
}
