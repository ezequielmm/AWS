// <copyright file="WorkspaceCloseManagerTest.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Mitutoyo.MiCAT.Applications.Services.Core;
using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.ReportModule.App.Test.AppStateHelper;
using Mitutoyo.MiCAT.ReportModuleApp.Controllers;
using Mitutoyo.MiCAT.ReportModuleApp.Manager;
using Mitutoyo.MiCAT.ReportModuleApp.Providers.Inputs;
using Moq;
using NUnit.Framework;

namespace Mitutoyo.MiCAT.ReportModule.App.Test.Manager
{
   [ExcludeFromCodeCoverage]
   public class WorkspaceCloseManagerTest : BaseAppStateTest
   {
      private IWorkspaceCloseManager _controller;
      private IReportTemplateController _reportTemplateController;
      private ICloseWorkspaceConfirmationInput _closeWorkspaceConfirmationInput;
      private IUnsavedChangesService _unsavedChangesService;

      public static ISnapShot BuildHelper(ISnapShot snapShot)
      {
         return snapShot;
      }

      [SetUp]
      public virtual void Setup()
      {
         SetUpHelper(BuildHelper);
         _reportTemplateController = Mock.Of<IReportTemplateController>();
         _closeWorkspaceConfirmationInput = Mock.Of<ICloseWorkspaceConfirmationInput>();
         _unsavedChangesService = Mock.Of<IUnsavedChangesService>();

         _controller = new WorkspaceCloseManager(
            _unsavedChangesService,
            _history,
            _closeWorkspaceConfirmationInput,
            _reportTemplateController);
      }

      [Test]
      public async Task OnWorkspaceClosingShouldContinueClosingWhenHasNoUnsavedChanges()
      {
         Mock
            .Get(_unsavedChangesService)
            .Setup(x => x.HaveUnsavedChanges(It.IsAny<ISnapShot>()))
            .Returns(false);

         var result = await _controller.OnWorkspaceClosing();

         Assert.AreEqual(WorkspaceClosingResult.ContinueClosing, result);
      }

      [Test]
      public async Task OnWorkspaceClosingShouldPromptForSavingChangesWhenUnsavedChangesExists()
      {
         Mock
            .Get(_unsavedChangesService)
            .Setup(x => x.HaveUnsavedChanges(It.IsAny<ISnapShot>()))
            .Returns(true);

         var result = await _controller.OnWorkspaceClosing();

         Mock
            .Get(_closeWorkspaceConfirmationInput)
            .Verify(x => x.ConfirmCloseWorkspace(), Times.Once);
      }

      [Test]
      public async Task OnWorkspaceClosingShouldContinueClosingWhenHasUnsavedChangesAndPromptForSavingChangesReturnsClose()
      {
         Mock
            .Get(_unsavedChangesService)
            .Setup(x => x.HaveUnsavedChanges(It.IsAny<ISnapShot>()))
            .Returns(true);

         Mock
           .Get(_closeWorkspaceConfirmationInput)
           .Setup(x => x.ConfirmCloseWorkspace())
           .Returns(CloseWorkspaceConfirmationResult.Close);

         var result = await _controller.OnWorkspaceClosing();

         Assert.AreEqual(WorkspaceClosingResult.ContinueClosing, result);
      }

      [Test]
      public async Task OnWorkspaceClosingShouldAbortWhenHasUnsavedChangesAndPromptForSavingChangesReturnsCancel()
      {
         Mock
            .Get(_unsavedChangesService)
            .Setup(x => x.HaveUnsavedChanges(It.IsAny<ISnapShot>()))
            .Returns(true);

         Mock
           .Get(_closeWorkspaceConfirmationInput)
           .Setup(x => x.ConfirmCloseWorkspace())
           .Returns(CloseWorkspaceConfirmationResult.Cancel);

         var result = await _controller.OnWorkspaceClosing();

         Assert.AreEqual(WorkspaceClosingResult.Abort, result);
      }

      [Test]
      public async Task OnWorkspaceClosingShouldSaveCurrentTemplateWhenHasUnsavedChangesAndPromptForSavingChangesReturnsSaveAndClose()
      {
         Mock
            .Get(_unsavedChangesService)
            .Setup(x => x.HaveUnsavedChanges(It.IsAny<ISnapShot>()))
            .Returns(true);

         Mock
           .Get(_closeWorkspaceConfirmationInput)
           .Setup(x => x.ConfirmCloseWorkspace())
           .Returns(CloseWorkspaceConfirmationResult.SaveAndClose);

         var result = await _controller.OnWorkspaceClosing();

         Mock
            .Get(_reportTemplateController)
            .Verify(x => x.SaveCurrentTemplate(), Times.Once);
      }

      [Test]
      public async Task OnWorkspaceClosingShouldContinueClosingWhenHasUnsavedChangesAndSaveCurrentTemplateIsSuccess()
      {
         Mock
            .Get(_unsavedChangesService)
            .Setup(x => x.HaveUnsavedChanges(It.IsAny<ISnapShot>()))
            .Returns(false);

         Mock
           .Get(_closeWorkspaceConfirmationInput)
           .Setup(x => x.ConfirmCloseWorkspace())
           .Returns(CloseWorkspaceConfirmationResult.SaveAndClose);

         Mock
            .Get(_reportTemplateController)
            .Setup(x => x.SaveCurrentTemplate())
            .Returns(Task.FromResult(SaveAsResult.Saved));

         var result = await _controller.OnWorkspaceClosing();

         Assert.AreEqual(WorkspaceClosingResult.ContinueClosing, result);
      }

      [Test]
      public async Task OnWorkspaceClosingShouldAbortWhenHasUnsavedChangesAndSaveCurrentTemplateIsCanceled()
      {
         Mock
            .Get(_unsavedChangesService)
            .Setup(x => x.HaveUnsavedChanges(It.IsAny<ISnapShot>()))
            .Returns(true);

         Mock
           .Get(_closeWorkspaceConfirmationInput)
           .Setup(x => x.ConfirmCloseWorkspace())
           .Returns(CloseWorkspaceConfirmationResult.SaveAndClose);

         Mock
            .Get(_reportTemplateController)
            .Setup(x => x.SaveCurrentTemplate())
            .Returns(Task.FromResult(SaveAsResult.Canceled));

         var result = await _controller.OnWorkspaceClosing();

         Assert.AreEqual(WorkspaceClosingResult.Abort, result);
      }
   }
}
