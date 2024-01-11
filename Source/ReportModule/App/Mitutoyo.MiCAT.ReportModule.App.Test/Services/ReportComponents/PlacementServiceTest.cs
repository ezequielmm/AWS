// <copyright file="PlacementServiceTest.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.ReportModule.Domain.Components;
using Mitutoyo.MiCAT.ReportModule.Domain.Components.ReportSections;
using Mitutoyo.MiCAT.ReportModuleApp.Services.ReportComponents;
using Moq;
using NUnit.Framework;

namespace Mitutoyo.MiCAT.ReportModule.App.Test.Services.ReportComponents
{
   [ExcludeFromCodeCoverage]
   [TestFixture]
   public class PlacementServiceTest
   {
      private Mock<ISnapShot> _snapShotMock;
      private PlacementService _sut;
      private Mock<IDomainSpaceService> _domainSpaceServiceMock;
      private IItemId<IReportComponent> _reportComponentId;
      private IItemId _reportSectionId;

      [SetUp]
      public void Setup()
      {
         var reportBody = new ReportBody();
         var placement = new ReportComponentPlacement(reportBody.Id, 1, 2, 3, 4);
         var reportComponent = new ReportImage(placement);
         _reportSectionId = reportBody.Id;
         _reportComponentId = reportComponent.Id;

         _domainSpaceServiceMock = new Mock<IDomainSpaceService>();

         _snapShotMock = new Mock<ISnapShot>();
         ServicesTestHelper.SetupUpdateItem<IReportComponent>(_snapShotMock);
         _domainSpaceServiceMock.SetupAddSpace(_snapShotMock.Object);

         _sut = new PlacementService(_domainSpaceServiceMock.Object);

         _snapShotMock.Setup(s => s.ContainsItem(reportComponent.Id)).Returns(true);
         _snapShotMock.Setup(s => s.GetItem(_reportComponentId)).Returns(reportComponent);
         _snapShotMock.Setup(s => s.GetItems<IReportComponent>()).Returns(new[] { reportComponent });
      }

      [Test]
      public void PlacementService_SetResizeShouldModifySize()
      {
         //Arrange

         //Act
         var snapShotResult = _sut.SetResize(_snapShotMock.Object, _reportComponentId, 10, 20);

         //Assert
         AssertPlacement(snapShotResult, 1, 2, 10, 20);
      }

      [Test]
      public void PlacementService_SetResizeShouldNotModifySizeAndNotUpdateSnapShot()
      {
         //Arrange

         //Act
         var snapShotResult = _sut.SetResize(_snapShotMock.Object, _reportComponentId, 3, 4);

         //Assert
         VerifyNeverCallUpdateItem();
         AssertPlacement(snapShotResult, 1, 2, 3, 4);
      }

      [Test]
      public void PlacementService_SetResizeShouldModifySizeAndPosition()
      {
         //Arrange

         //Act
         var snapShotResult = _sut.SetResize(_snapShotMock.Object, _reportComponentId, 10, 20, 30, 40);

         //Assert
         AssertPlacement(snapShotResult, 10, 20, 30, 40);
      }

      [Test]
      public void PlacementService_SetResizeShouldNotModifySizePositionAndNotUpdateSnapShot()
      {
         //Arrange

         //Act
         var snapShotResult = _sut.SetResize(_snapShotMock.Object, _reportComponentId, 1, 2, 3, 4);

         //Assert
         VerifyNeverCallUpdateItem();
         AssertPlacement(snapShotResult, 1, 2, 3, 4);
      }

      [Test]
      public void PlacementService_SetResizeOnFakeSpaceShouldModifySizeAndPosition()
      {
         //Arrange

         //Act
         var snapShotResult = _sut.SetResizeOnFakeSpace(_snapShotMock.Object, _reportComponentId, 10, 20, 30, 40, 25, 35);

         //Assert
         _domainSpaceServiceMock.VerifyCallAddSpace(_reportComponentId, 25, 35);
         AssertPlacement(snapShotResult, 10, 20, 30, 40);
      }

      [Test]
      public void PlacementService_SetResizeOnFakeSpaceShouldNotModifySizeAndPosition()
      {
         //Arrange

         //Act
         var snapShotResult = _sut.SetResizeOnFakeSpace(_snapShotMock.Object, _reportComponentId, 1, 2, 3, 4, 25, 35);

         //Assert
         VerifyNeverCallAddSpace();
         AssertPlacement(snapShotResult, 1, 2, 3, 4);
      }

      [Test]
      public void PlacementService_SetPositionShouldModifyPosition()
      {
         //Arrange

         //Act
         var snapShotResult = _sut.SetPosition(_snapShotMock.Object, _reportComponentId, 10, 20);

         //Assert
         AssertPlacement(snapShotResult, 10, 20, 3, 4);
      }

      [Test]
      public void PlacementService_SetPositionShouldNotModifyPositionAndNotUpdateSnapShot()
      {
         //Arrange

         //Act
         var snapShotResult = _sut.SetPosition(_snapShotMock.Object, _reportComponentId, 1, 2);

         //Assert
         VerifyNeverCallUpdateItem();
         AssertPlacement(snapShotResult, 1, 2, 3, 4);
      }

      [Test]
      public void PlacementService_SetPositionOnFakeSpaceShouldModifyPosition()
      {
         //Arrange

         //Act
         var snapShotResult = _sut.SetPositionOnFakeSpace(_snapShotMock.Object, _reportComponentId, 10, 20, 25, 35);

         //Assert
         _domainSpaceServiceMock.VerifyCallAddSpace(_reportComponentId, 25, 35);
         AssertPlacement(snapShotResult, 10, 20, 3, 4);
      }

      [Test]
      public void PlacementService_SetPositionOnFakeSpaceShouldNotModifyPosition()
      {
         //Arrange

         //Act
         var snapShotResult = _sut.SetPositionOnFakeSpace(_snapShotMock.Object, _reportComponentId, 1, 2, 25, 35);

         //Assert
         VerifyNeverCallAddSpace();
         AssertPlacement(snapShotResult, 1, 2, 3, 4);
      }

      private void AssertPlacement(ISnapShot snapShotResult, int expectedX, int expectedY, int expectedWidth, int expectedHeight)
      {
         var actualPlacement = snapShotResult.GetItem(_reportComponentId).Placement;

         Assert.AreEqual(_reportSectionId, actualPlacement.ReportSectionId);
         Assert.AreEqual(expectedX, actualPlacement.X);
         Assert.AreEqual(expectedY, actualPlacement.Y);
         Assert.AreEqual(expectedWidth, actualPlacement.Width);
         Assert.AreEqual(expectedHeight, actualPlacement.Height);
      }

      private void VerifyNeverCallUpdateItem()
      {
         _snapShotMock.Verify(s => s.UpdateItem(It.IsAny<IReportComponent>(), It.IsAny<IReportComponent>()), Times.Never());
      }

      private void VerifyNeverCallAddSpace()
      {
         _domainSpaceServiceMock.Verify(d => d.AddSpace(It.IsAny<ISnapShot>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<IEnumerable<IItemId<IReportComponent>>>()), Times.Never());
      }
   }
}
