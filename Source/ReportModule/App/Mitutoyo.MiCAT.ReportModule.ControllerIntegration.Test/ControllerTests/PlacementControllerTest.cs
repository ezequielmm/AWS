// <copyright file="PlacementControllerTest.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using Mitutoyo.MiCAT.ReportModule.ControllerIntegration.Test.ControllerTests.TestHelper;
using Mitutoyo.MiCAT.ReportModule.Domain.Components;
using Mitutoyo.MiCAT.ReportModuleApp.Controllers;
using Mitutoyo.MiCAT.ReportModuleApp.Services.ReportComponents;
using NUnit.Framework;

namespace Mitutoyo.MiCAT.ReportModule.ControllerIntegration.Test.ControllerTests
{
   [TestFixture]
   [ExcludeFromCodeCoverage]
   public class PlacementControllerTest : BaseAppStateTest
   {
      private ReportComponentPlacementController _controller;

      [SetUp]
      public virtual void Setup()
      {
         SetUpHelper(AppStateReportComponentTestHelper.InitializeSnapShot);

         var placementService = new PlacementService(new DomainSpaceService());
         _controller = new ReportComponentPlacementController(_history, placementService);
      }

      [Test]
      public void PlacementControllerTest_SetPosition()
      {
         // Arrange
         var placement = new ReportComponentPlacement(_history.CurrentSnapShot.GetReportBodyId(), 1, 2, 3, 4);
         var component = new ReportTextBox(placement);

         var snapshot = _history.NextSnapShot(ApplicationState.Controllers.ControllerCall.Empty);
         snapshot = snapshot.AddItem(component);

         _history.AddSnapShot(snapshot);

         var newPosX = 15;
         var newPosY = 15;

         // Act
         _controller.SetPosition(component.Id, newPosX, newPosY);

         // Assert
         var actualReportComponent = _history.CurrentSnapShot.GetItem(component.Id);
         Assert.AreEqual(newPosX, actualReportComponent.Placement.X);
         Assert.AreEqual(newPosY, actualReportComponent.Placement.Y);
      }

      [Test]
      public void PlacementControllerTest_SetResizePosition()
      {
         // Arrange
         var placement = new ReportComponentPlacement(_history.CurrentSnapShot.GetReportBodyId(), 1, 2, 3, 4);
         var component = new ReportTextBox(placement);

         var snapshot = _history.NextSnapShot(ApplicationState.Controllers.ControllerCall.Empty);
         snapshot = snapshot.AddItem(component);

         _history.AddSnapShot(snapshot);

         var newPosX = 15;
         var newPosY = 16;
         var newWidth = 17;
         var newHeight = 18;

         // Act
         _controller.SetResize(component.Id, newPosX, newPosY, newWidth, newHeight);

         // Assert
         var actualPlacement = _history.CurrentSnapShot.GetItem(component.Id).Placement;
         Assert.AreEqual(newPosX, actualPlacement.X);
         Assert.AreEqual(newPosY, actualPlacement.Y);
         Assert.AreEqual(newWidth, actualPlacement.Width);
         Assert.AreEqual(newHeight, actualPlacement.Height);
      }

      [Test]
      public void PlacementControllerTest_SetResize()
      {
         // Arrange
         var placement = new ReportComponentPlacement(_history.CurrentSnapShot.GetReportBodyId(), 1, 2, 3, 4);
         var component = new ReportTextBox(placement);

         var snapshot = _history.NextSnapShot(ApplicationState.Controllers.ControllerCall.Empty);
         snapshot = snapshot.AddItem(component);

         _history.AddSnapShot(snapshot);

         var newWidth = 17;
         var newHeight = 18;

         // Act
         _controller.SetResize(component.Id, newWidth, newHeight);

         // Assert
         var actualPlacement = _history.CurrentSnapShot.GetItem(component.Id).Placement;
         Assert.AreEqual(newWidth, actualPlacement.Width);
         Assert.AreEqual(newHeight, actualPlacement.Height);
      }

      [Test]
      public void PlacementControllerTest_SetResizeOnFakeSpace()
      {
         // Arrange
         var placement = new ReportComponentPlacement(_history.CurrentSnapShot.GetReportBodyId(), 1, 2, 3, 4);
         var placementToMove = new ReportComponentPlacement(_history.CurrentSnapShot.GetReportBodyId(), 20, 30, 40, 50);
         var componentToResize = new ReportImage(placement);
         var componentToMove = new ReportImage(placementToMove);

         var snapshot = _history.NextSnapShot(ApplicationState.Controllers.ControllerCall.Empty);
         snapshot = snapshot.AddItem(componentToResize);
         snapshot = snapshot.AddItem(componentToMove);

         _history.AddSnapShot(snapshot);

         var newPosX = 15;
         var newPosY = 16;
         var newWidth = 17;
         var newHeight = 18;

         // Act
         _controller.SetResizeOnFakeSpace(componentToResize.Id, newPosX, newPosY, newWidth, newHeight, 10, 20);

         // Assert
         var actualPlacement = _history.CurrentSnapShot.GetItem(componentToResize.Id).Placement;
         var actualPlacementToMove = _history.CurrentSnapShot.GetItem(componentToMove.Id).Placement;

         Assert.AreEqual(_history.CurrentSnapShot.GetReportBodyId(), actualPlacement.ReportSectionId);
         Assert.AreEqual(newPosX, actualPlacement.X);
         Assert.AreEqual(newPosY, actualPlacement.Y);
         Assert.AreEqual(newWidth, actualPlacement.Width);
         Assert.AreEqual(newHeight, actualPlacement.Height);

         Assert.AreEqual(_history.CurrentSnapShot.GetReportBodyId(), actualPlacementToMove.ReportSectionId);
         Assert.AreEqual(20, actualPlacementToMove.X);
         Assert.AreEqual(50, actualPlacementToMove.Y);
         Assert.AreEqual(40, actualPlacementToMove.Width);
         Assert.AreEqual(50, actualPlacementToMove.Height);
      }

      [Test]
      public void PlacementControllerTest_SetPositionOnFakeSpace()
      {
         // Arrange
         var placement = new ReportComponentPlacement(_history.CurrentSnapShot.GetReportBodyId(), 1, 2, 3, 4);
         var placementToMove = new ReportComponentPlacement(_history.CurrentSnapShot.GetReportBodyId(), 20, 30, 40, 50);
         var component = new ReportImage(placement);
         var componentToMove = new ReportImage(placementToMove);

         var snapshot = _history.NextSnapShot(ApplicationState.Controllers.ControllerCall.Empty);
         snapshot = snapshot.AddItem(component);
         snapshot = snapshot.AddItem(componentToMove);

         _history.AddSnapShot(snapshot);

         var newPosX = 15;
         var newPosY = 16;

         // Act
         _controller.SetPositionOnFakeSpace(component.Id, newPosX, newPosY, 10, 20);

         // Assert
         var actualPlacement = _history.CurrentSnapShot.GetItem(component.Id).Placement;
         var actualPlacementToMove = _history.CurrentSnapShot.GetItem(componentToMove.Id).Placement;

         Assert.AreEqual(_history.CurrentSnapShot.GetReportBodyId(), actualPlacement.ReportSectionId);
         Assert.AreEqual(newPosX, actualPlacement.X);
         Assert.AreEqual(newPosY, actualPlacement.Y);
         Assert.AreEqual(3, actualPlacement.Width);
         Assert.AreEqual(4, actualPlacement.Height);

         Assert.AreEqual(_history.CurrentSnapShot.GetReportBodyId(), actualPlacementToMove.ReportSectionId);
         Assert.AreEqual(20, actualPlacementToMove.X);
         Assert.AreEqual(50, actualPlacementToMove.Y);
         Assert.AreEqual(40, actualPlacementToMove.Width);
         Assert.AreEqual(50, actualPlacementToMove.Height);
      }
   }
}
