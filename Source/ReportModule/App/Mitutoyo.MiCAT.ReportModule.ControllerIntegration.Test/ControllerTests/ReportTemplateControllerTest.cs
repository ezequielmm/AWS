// <copyright file="ReportTemplateControllerTest.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Mitutoyo.MiCAT.Applications.Services.Core;
using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.ApplicationState.Controllers;
using Mitutoyo.MiCAT.ReportModule.Domain.CADElements;
using Mitutoyo.MiCAT.ReportModule.Domain.Components.ReportSections;
using Mitutoyo.MiCAT.ReportModule.Domain.Data;
using Mitutoyo.MiCAT.ReportModule.Domain.DataResult.Exceptions;
using Mitutoyo.MiCAT.ReportModule.Persistence;
using Mitutoyo.MiCAT.ReportModuleApp.AppState;
using Mitutoyo.MiCAT.ReportModuleApp.Controllers;
using Mitutoyo.MiCAT.ReportModuleApp.Providers;
using Mitutoyo.MiCAT.ReportModuleApp.Providers.Inputs;
using Mitutoyo.MiCAT.ReportModuleApp.Utilities;
using Moq;
using NUnit.Framework;

namespace Mitutoyo.MiCAT.ReportModule.ControllerIntegration.Test.ControllerTests
{
   [ExcludeFromCodeCoverage]
   [TestFixture]
   public class ReportTemplateControllerTest : BaseAppStateTest
   {
      private Mock<IReportTemplatePersistence> _reportTemplatePersistence;
      private Mock<ITemplateNameResolver> _templateNameProvider;
      private ReportTemplateController controller;
      private IUnsavedChangesService _unsavedChangesService;
      private Mock<IMessageNotifier> _notifierMock;
      private Mock<IReportTemplateDeleteConfirmationInput> _reportTemplateDeleteConfirmationInput;

      public static ISnapShot BuildHelper(ISnapShot snapShot)
      {
         snapShot = snapShot.AddCollection<CommonPageLayout>(AppStateKinds.Undoable);
         snapShot = snapShot.AddCollection<TemplateDescriptorState>(AppStateKinds.Undoable);
         snapShot = snapShot.AddCollection<CADLayout>(AppStateKinds.Undoable);

         return snapShot;
      }

      [SetUp]
      public virtual void Setup()
      {
         SetUpHelper(BuildHelper);
         _reportTemplatePersistence = new Mock<IReportTemplatePersistence>();
         _reportTemplateDeleteConfirmationInput = new Mock<IReportTemplateDeleteConfirmationInput>();

         _templateNameProvider = new Mock<ITemplateNameResolver>();
         _templateNameProvider.Setup(t => t.QueryTemplateName()).Returns(new TemplateNameResult("TemplateName", false));
         _notifierMock = new Mock<IMessageNotifier>();

         _unsavedChangesService = Mock.Of<IUnsavedChangesService>();

         controller = new ReportTemplateController(
            _reportTemplatePersistence.Object,
            _history,
            _templateNameProvider.Object,
            _unsavedChangesService,
            _notifierMock.Object,
            _reportTemplateDeleteConfirmationInput.Object);
      }
      [Test]
      public void ReportTemplate_GetReportTemplateDescriptorsShouldConcatDBAndDefaultReports()
      {
         // Arrange
         var reportTemplates = new List<ReportTemplateDescriptor>() { new ReportTemplateDescriptor() { Name = "report1" }, new ReportTemplateDescriptor { Name = "report2" } };
         var defaultReportTemplates = new List<ReportTemplateDescriptor>() { new ReportTemplateDescriptor { Name = "Default" } };
         _reportTemplatePersistence.Setup(x => x.GetReportTemplatesDescriptors()).ReturnsAsync(reportTemplates);
         _reportTemplatePersistence.Setup(x => x.GetDefaultReportTemplateDescriptors()).Returns(defaultReportTemplates);

         // Act
         var allReports = controller.GetReportTemplateDescriptorsForEdit().Result;

         // Assert
         _reportTemplatePersistence.Verify(x => x.GetReportTemplatesDescriptors(), Times.Once);
         _reportTemplatePersistence.Verify(x => x.GetDefaultReportTemplateDescriptors(), Times.Once);
         Assert.AreEqual(allReports.Count(), 3);
         Assert.AreEqual(allReports, reportTemplates.Concat(defaultReportTemplates));
      }
      [Test]
      public void ReportTemplate_GetReportTemplateByIdShouldTryTryToFindItOnDefaultFirst()
      {
         // Arrange
         var id = Guid.NewGuid();
         var reportTemplateDefault = new ReportTemplate { TemplateDescriptor = new TemplateDescriptor() { TemplateId = id, Name = "Name1" } };
         _reportTemplatePersistence.Setup(x => x.GetDefaultReportTemplate(id)).Returns(reportTemplateDefault);
         _reportTemplatePersistence.Setup(x => x.GetReportTemplate(id)).ReturnsAsync(default(ReportTemplate));

         // Act
         var report = controller.GetReportTemplateById(id).Result;

         // Assert
         Assert.AreEqual(report, reportTemplateDefault);
      }

      [Test]
      public void ReportTemplate_GetReportTemplateByIdIfItDoesntExistOnDefaultShouldTryOnDB()
      {
         // Arrange
         var id = Guid.NewGuid();
         ReportTemplate reportTemplateDefault = null;
         var reportTemplateDB = new ReportTemplate { TemplateDescriptor = new TemplateDescriptor() { TemplateId = id, Name = "Name1" } };
         _reportTemplatePersistence.Setup(x => x.GetDefaultReportTemplate(id)).Returns(reportTemplateDefault);
         _reportTemplatePersistence.Setup(x => x.GetReportTemplate(id)).ReturnsAsync(reportTemplateDB);

         // Act
         var reportResult = controller.GetReportTemplateById(id).Result;

         // Assert
         Assert.AreEqual(reportResult, reportTemplateDB);
      }

      [Test]
      public void ReportTemplate_ShouldCallDeleteIfTemplateIsNotBlankTemplate()
      {
         // Arrange
         var id = Guid.NewGuid();
         ReportTemplate reportDefaultTemplateResult = new ReportTemplate { TemplateDescriptor = new TemplateDescriptor() { TemplateId = Guid.NewGuid(), Name = "Blank", ReadOnly = false } };
         _reportTemplatePersistence.Setup(x => x.GetReportTemplate(id)).ReturnsAsync(reportDefaultTemplateResult);
         _reportTemplateDeleteConfirmationInput.Setup(r => r.ConfirmDeleteReportTemplate()).Returns(true);

         // Act
         var reportResult = controller.DeleteReportTemplate(id);

         // Assert
         _reportTemplatePersistence.Verify(x => x.DeleteReportTemplate(id), Times.Once);
      }

      [Test]
      public void ReportTemplate_ShouldNotCallDeleteIfTemplateIsBlankTemplate()
      {
         // Arrange
         var id = Guid.NewGuid();
         var reportDefaultTemplateResult = new ReportTemplate
         { TemplateDescriptor = new TemplateDescriptor() { TemplateId = id, Name = "Blank" } };
         _reportTemplatePersistence.Setup(x => x.GetDefaultReportTemplate(id)).Returns(reportDefaultTemplateResult);

         // Act
         var reportResult = controller.DeleteReportTemplate(id);

         // Assert
         _reportTemplatePersistence.Verify(x => x.DeleteReportTemplate(id), Times.Never);
      }

      [Test]
      public async Task ReportTemplate_SaveCurrentTemplate()
      {
         // Arrange
         var newReportTemplate = GetTestReportTemplate(Guid.NewGuid(), false);
         var templateDescriptorState = new TemplateDescriptorState(newReportTemplate.TemplateDescriptor);

         var historyMock = new Mock<IAppStateHistory>();
         var snapShotMock = new Mock<ISnapShot>();

         snapShotMock.Setup(s => s.GetItems<ReportBody>()).Returns(new[] { new ReportBody() });

         historyMock.Setup(h => h.CurrentSnapShot).Returns(snapShotMock.Object);
         snapShotMock.Setup(s => s.GetItems<TemplateDescriptorState>()).Returns(new[] { templateDescriptorState });

         var templateController = new ReportTemplateController(_reportTemplatePersistence.Object, historyMock.Object, _templateNameProvider.Object, _unsavedChangesService, _notifierMock.Object, _reportTemplateDeleteConfirmationInput.Object);

         // Act
         await templateController.SaveCurrentTemplate();

         // Assert
         _reportTemplatePersistence.Verify(s => s.UpdateTemplate(It.Is<ReportTemplate>(t => t.TemplateDescriptor.TemplateId == newReportTemplate.TemplateDescriptor.TemplateId)), Times.Once);

         Mock
            .Get(_unsavedChangesService)
            .Verify(x => x.SaveSnapShot(It.IsAny<ISnapShot>()), Times.Once);
      }

      [Test]
      public async Task ReportTemplate_SaveCurrentTemplateAs()
      {
         // Arrange
         var newReportTemplate = GetTestReportTemplate(Guid.NewGuid(), false);
         var templateDescriptorState = new TemplateDescriptorState(newReportTemplate.TemplateDescriptor);

         var historyMock = new Mock<IAppStateHistory>();
         var snapShotMock = new Mock<ISnapShot>();

         snapShotMock.Setup(s => s.GetItems<ReportBody>()).Returns(new[] { new ReportBody() });

         snapShotMock
            .Setup(s => s.GetItems<TemplateDescriptorState>())
            .Returns(new[] { templateDescriptorState });

         snapShotMock
            .Setup(x => x.UpdateItem(It.IsAny<TemplateDescriptorState>(), It.IsAny<TemplateDescriptorState>()))
            .Returns(snapShotMock.Object);

         historyMock
            .Setup(h => h.CurrentSnapShot)
            .Returns(snapShotMock.Object);

         historyMock
            .Setup(x => x.NextSnapShot(It.IsAny<ControllerCall>()))
            .Returns(snapShotMock.Object);

         historyMock
            .Setup(x => x.GetSnapShot(It.IsAny<SnapShotId>()))
            .Returns(snapShotMock.Object);

         _reportTemplatePersistence
            .Setup(x => x.AddTemplate(It.IsAny<ReportTemplate>()))
            .Returns(Task.FromResult(new ReportTemplate()));

         var templateController = new ReportTemplateController(_reportTemplatePersistence.Object, historyMock.Object, _templateNameProvider.Object, _unsavedChangesService, _notifierMock.Object, _reportTemplateDeleteConfirmationInput.Object);

         // Act
         await templateController.SaveAsCurrentTemplate();

         // Assert
         _reportTemplatePersistence
            .Verify(x => x.AddTemplate(It.Is<ReportTemplate>(t => t.TemplateDescriptor.TemplateId == newReportTemplate.TemplateDescriptor.TemplateId)), Times.Once);

         Mock
            .Get(_unsavedChangesService)
            .Verify(x => x.SaveSnapShot(It.IsAny<ISnapShot>()), Times.Once);
      }
      [Test]
      public void ReportTemplate_GetCurrentReportTemplate()
      {
         // Arrange
         var newReportTemplate = GetTestReportTemplate(Guid.NewGuid(), false);
         var templateDescriptorState = new TemplateDescriptorState(newReportTemplate.TemplateDescriptor);

         var historyMock = new Mock<IAppStateHistory>();
         var snapShotMock = new Mock<ISnapShot>();

         snapShotMock.Setup(s => s.GetItems<ReportBody>()).Returns(new[] { new ReportBody() });

         snapShotMock
            .Setup(s => s.GetItems<TemplateDescriptorState>())
            .Returns(new[] { templateDescriptorState });

         snapShotMock
            .Setup(x => x.UpdateItem(It.IsAny<TemplateDescriptorState>(), It.IsAny<TemplateDescriptorState>()))
            .Returns(snapShotMock.Object);

         historyMock
            .Setup(h => h.CurrentSnapShot)
            .Returns(snapShotMock.Object);

         historyMock
            .Setup(x => x.NextSnapShot(It.IsAny<ControllerCall>()))
            .Returns(snapShotMock.Object);

         historyMock
            .Setup(x => x.GetSnapShot(It.IsAny<SnapShotId>()))
            .Returns(snapShotMock.Object);

         _reportTemplatePersistence
            .Setup(x => x.AddTemplate(It.IsAny<ReportTemplate>()))
            .Returns(Task.FromResult(new ReportTemplate()));

         var templateController = new ReportTemplateController(_reportTemplatePersistence.Object, historyMock.Object, _templateNameProvider.Object, _unsavedChangesService, _notifierMock.Object, _reportTemplateDeleteConfirmationInput.Object);

         // Act
         var template = templateController.GetCurrentReportTemplate();

         // Assert
         Assert.That(template.TemplateDescriptor.TemplateId == newReportTemplate.TemplateDescriptor.TemplateId);
      }

      private ReportTemplate GetTestReportTemplate(Guid templateId, bool readOnly)
      {
         return new ReportTemplate()
         {
            TemplateDescriptor = new TemplateDescriptor()
            {
               TemplateId = templateId,
               ReadOnly = readOnly
            }
         };
      }
      [Test]
      public async Task ReportTemplate_SaveCurrentDeletedTemplateReturnSaveAsResult()
      {
         // Arrange
         var newReportTemplate = GetTestReportTemplate(Guid.NewGuid(), false);
         var templateDescriptorState = new TemplateDescriptorState(newReportTemplate.TemplateDescriptor);

         var historyMock = new Mock<IAppStateHistory>();
         var snapShotMock = new Mock<ISnapShot>();

         snapShotMock.Setup(s => s.GetItems<ReportBody>()).Returns(new[] { new ReportBody() });

         historyMock.Setup(h => h.CurrentSnapShot).Returns(snapShotMock.Object);
         snapShotMock.Setup(s => s.GetItems<TemplateDescriptorState>()).Returns(new[] { templateDescriptorState });
         historyMock
         .Setup(x => x.NextSnapShot(It.IsAny<ControllerCall>()))
         .Returns(snapShotMock.Object);

         snapShotMock
         .Setup(x => x.UpdateItem(It.IsAny<TemplateDescriptorState>(), It.IsAny<TemplateDescriptorState>()))
         .Returns(snapShotMock.Object);

         _reportTemplatePersistence
            .Setup(x => x.AddTemplate(It.IsAny<ReportTemplate>()))
            .Returns(Task.FromResult(new ReportTemplate()));

         _reportTemplatePersistence.Setup(r => r.UpdateTemplate(It.IsAny<ReportTemplate>())).Throws(new ReportTemplateNotFoundException("error"));

         var templateController = new ReportTemplateController(_reportTemplatePersistence.Object, historyMock.Object, _templateNameProvider.Object, _unsavedChangesService, _notifierMock.Object, _reportTemplateDeleteConfirmationInput.Object);

         // Act
         var saveAsResult = await templateController.SaveCurrentTemplate();

         // Assert
         _reportTemplatePersistence.Verify(s => s.AddTemplate(It.Is<ReportTemplate>(t => t.TemplateDescriptor.TemplateId == newReportTemplate.TemplateDescriptor.TemplateId)), Times.Once);

         Mock
            .Get(_unsavedChangesService)
            .Verify(x => x.SaveSnapShot(It.IsAny<ISnapShot>()), Times.Once);
         Assert.AreEqual(SaveAsResult.Saved, saveAsResult);
      }
   }
}
