// <copyright file="ReportComponentServiceTest.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.ReportModule.Domain.Components;
using Mitutoyo.MiCAT.ReportModule.Domain.Components.ReportSections;
using Mitutoyo.MiCAT.ReportModule.Domain.Data;
using Mitutoyo.MiCAT.ReportModuleApp.Services.ReportComponents;
using Moq;
using NUnit.Framework;

namespace Mitutoyo.MiCAT.ReportModule.App.Test.Services.ReportComponents
{
   [ExcludeFromCodeCoverage]
   [TestFixture]
   public class ReportComponentServiceTest
   {
      private ReportComponentService _sut;
      private Mock<ISnapShot> _snapShotMock;
      private Mock<IDomainSpaceService> _domainSpaceServiceMock;
      private ReportComponentPlacement _placement;
      private Id<ReportComponentSelection> _reportComponentSelectionId;

      [SetUp]
      public void Setup()
      {
         var reportComponentSelection = new ReportComponentSelection();
         var reportBody = new ReportBody();

         _reportComponentSelectionId = reportComponentSelection.Id;
         _placement = new ReportComponentPlacement(reportBody.Id, 1, 2, 3, 4);

         _snapShotMock = new Mock<ISnapShot>();
         _snapShotMock.Setup(s => s.AddItem(It.IsAny<IReportComponent>())).Returns(_snapShotMock.Object);
         _snapShotMock.Setup(s => s.AddItem(It.IsAny<IStateItem>())).Returns(_snapShotMock.Object);
         _snapShotMock.Setup(s => s.GetItems<ReportComponentSelection>()).Returns(new[] { reportComponentSelection });

         ServicesTestHelper.SetupUpdateItem<ReportComponentSelection>(_snapShotMock);

         _domainSpaceServiceMock = new Mock<IDomainSpaceService>();
         _domainSpaceServiceMock.SetupAddSpace(_snapShotMock.Object);

         _sut = new ReportComponentService(_domainSpaceServiceMock.Object);
      }

      [Test]
      public void ReportComponentService_AddComponent()
      {
         //Arrange
         var reportTextbox = new ReportTextBox(_placement);

         //Act
         var snapShotResult = _sut.AddComponent(_snapShotMock.Object, reportTextbox);

         //Assert
         var actualSelection = snapShotResult.GetItem(_reportComponentSelectionId);

         Assert.AreEqual(reportTextbox.Id, actualSelection.SelectedReportComponentIds.Single());
         _snapShotMock.Verify(s => s.AddItem(It.Is<IReportComponent>(p => p.Id.Equals(reportTextbox.Id))), Times.Once);
         _snapShotMock.Verify(s => s.AddItem(It.IsAny<IStateItem>()), Times.Never());
      }

      [Test]
      public void ReportComponentService_AddComponentExtraElements()
      {
         //Arrange
         var headerField = new ReportHeaderFormField();
         var headerRow = new ReportHeaderFormRow(new[] { headerField.Id });
         var headerForm = new ReportHeaderForm(_placement, new[] { headerRow.Id });

         //Act
         var snapShotResult = _sut.AddComponent(_snapShotMock.Object, headerForm, new IStateItem[] { headerRow, headerField });

         //Assert
         var actualSelection = snapShotResult.GetItem(_reportComponentSelectionId);

         Assert.AreEqual(headerForm.Id, actualSelection.SelectedReportComponentIds.Single());
         _snapShotMock.Verify(s => s.AddItem(It.Is<IReportComponent>(c => c.Id.UniqueValue == headerForm.Id.UniqueValue)), Times.Once);
         _snapShotMock.Verify(s => s.AddItem(It.Is<IStateItem>(i => i.Id.UniqueValue == headerRow.Id.UniqueValue)), Times.Once());
         _snapShotMock.Verify(s => s.AddItem(It.Is<IStateItem>(i => i.Id.UniqueValue == headerField.Id.UniqueValue)), Times.Once());
      }

      [Test]
      public void ReportComponentService_AddComponentOnFakeSpace()
      {
         //Arrange
         var reportTextbox = new ReportTextBox(_placement);

         //Act
         var snapShotResult = _sut.AddComponentOnFakeSpace(_snapShotMock.Object, reportTextbox, 25, 35);

         //Assert
         var actualSelection = snapShotResult.GetItem(_reportComponentSelectionId);

         Assert.AreEqual(reportTextbox.Id, actualSelection.SelectedReportComponentIds.Single());
         _snapShotMock.Verify(s => s.AddItem(It.Is<IReportComponent>(c => c.Id.Equals(reportTextbox.Id))), Times.Once);
         _snapShotMock.Verify(s => s.AddItem(It.IsAny<IStateItem>()), Times.Never());
         _domainSpaceServiceMock.VerifyCallAddSpace(25, 35);
      }

      [Test]
      public void ReportComponentService_AddComponentOnFakeSpaceExtraElements()
      {
         //Arrange
         var headerField = new ReportHeaderFormField();
         var headerRow = new ReportHeaderFormRow(new[] { headerField.Id });
         var headerForm = new ReportHeaderForm(_placement, new[] { headerRow.Id });

         //Act
         var snapShotResult = _sut.AddComponentOnFakeSpace(_snapShotMock.Object, headerForm, new IStateItem[] { headerRow, headerField }, 25, 35);

         //Assert
         var actualSelection = snapShotResult.GetItem(_reportComponentSelectionId);

         Assert.AreEqual(headerForm.Id, actualSelection.SelectedReportComponentIds.Single());
         _snapShotMock.Verify(s => s.AddItem(It.Is<IReportComponent>(c => c.Id.Equals(headerForm.Id))), Times.Once);
         _snapShotMock.Verify(s => s.AddItem(It.Is<IStateItem>(i => i.Id.UniqueValue == headerRow.Id.UniqueValue)), Times.Once());
         _snapShotMock.Verify(s => s.AddItem(It.Is<IStateItem>(i => i.Id.UniqueValue == headerField.Id.UniqueValue)), Times.Once());
         _domainSpaceServiceMock.VerifyCallAddSpace(25, 35);
      }
   }
}
