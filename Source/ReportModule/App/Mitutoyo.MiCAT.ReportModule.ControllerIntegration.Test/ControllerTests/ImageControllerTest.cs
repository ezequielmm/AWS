// <copyright file="ImageControllerTest.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.ApplicationState.Controllers;
using Mitutoyo.MiCAT.ReportModule.ControllerIntegration.Test.ControllerTests.TestHelper;
using Mitutoyo.MiCAT.ReportModule.Domain.Components;
using Mitutoyo.MiCAT.ReportModule.Domain.Data;
using Mitutoyo.MiCAT.ReportModuleApp.Controllers;
using Mitutoyo.MiCAT.ReportModuleApp.Services.ReportComponents;
using NUnit.Framework;

namespace Mitutoyo.MiCAT.ReportModule.ControllerIntegration.Test.ControllerTests
{
   [ExcludeFromCodeCoverage]
   [TestFixture]
   public class ImageControllerTest : BaseAppStateTest
   {
      private ImageController _controller;

      [SetUp]
      public virtual void Setup()
      {
         SetUpHelper(AppStateReportComponentTestHelper.InitializeSnapShot);

         var domainSpaceService = new DomainSpaceService();
         var reportComponentService = new ReportComponentService(domainSpaceService);

         _controller = new ImageController(_history, reportComponentService);
      }

      [Test]
      public void ImageController_AddNewImageToBody()
      {
         TestAddNewImageToReportSection(_history.CurrentSnapShot.GetReportBodyId(), 150, 150, (_, x, y) => _controller.AddImageToBody(x, y));
      }

      [Test]
      public void ImageController_AddNewImageToHeader()
      {
         TestAddNewImageToReportSection(_history.CurrentSnapShot.GetReportHeaderId(), 100, 100, (sectionId, x, y) => _controller.AddImageToBoundarySection(sectionId, x, y));
      }

      [Test]
      public void ImageController_AddNewImageToFooter()
      {
         TestAddNewImageToReportSection(_history.CurrentSnapShot.GetReportFooterId(), 100, 100, (sectionId, x, y) => _controller.AddImageToBoundarySection(sectionId, x, y));
      }

      [Test]
      public void ImageController_UpdateImageToBody()
      {
         TestUpdateImageToReportSection(_history.CurrentSnapShot.GetReportBodyId());
      }

      [Test]
      public void ImageController_UpdateImageToHeader()
      {
         TestUpdateImageToReportSection(_history.CurrentSnapShot.GetReportHeaderId());
      }

      [Test]
      public void ImageController_UpdateImageToFooter()
      {
         TestUpdateImageToReportSection(_history.CurrentSnapShot.GetReportFooterId());
      }

      private void TestAddNewImageToReportSection(IItemId reportSectionId, int expectedWidth, int expectedHeight, Action<IItemId, int, int> addImageAction)
      {
         // Arrange
         int x = 10;
         int y = 20;

         // Act
         addImageAction(reportSectionId, x, y);

         // Assert
         AssertSingleImageCreation(reportSectionId, x, y, expectedWidth, expectedHeight);
      }

      private void TestUpdateImageToReportSection(IItemId reportSectionId)
      {
         // Arrange
         var originalPlacement = new ReportComponentPlacement(reportSectionId, 1, 2, 3, 4);
         var originalImage = new ReportImage(originalPlacement).WithImage("Image Default");

         var snapshot = _history.NextSnapShot(ControllerCall.Empty);
         snapshot = snapshot.AddItem(originalImage);

         _history.AddSnapShot(snapshot);

         int newWidth = 10;
         int newHeight = 20;
         string newImage = "Image Updated";

         // Act
         _controller.UpdateImage(originalImage.Id, newImage, newWidth, newHeight);

         // Assert
         AssertImageUpdate(originalImage.Id, originalPlacement, newImage, newWidth, newHeight);
      }

      private void AssertSingleImageCreation(IItemId expectedReportSectionId, int expectedX, int expectedY, int expectedWidth, int expectedHeight)
      {
         var imageToTest = _history.CurrentSnapShot.GetItems<ReportImage>().Single();
         var selectionComponentToTest = _history.CurrentSnapShot.GetItems<ReportComponentSelection>().Single();

         Assert.AreEqual(selectionComponentToTest.SelectedReportComponentIds.Single(), imageToTest.Id);
         Assert.AreEqual(expectedReportSectionId, imageToTest.Placement.ReportSectionId);
         Assert.AreEqual(expectedX, imageToTest.Placement.X);
         Assert.AreEqual(expectedY, imageToTest.Placement.Y);
         Assert.AreEqual(expectedWidth, imageToTest.Placement.Width);
         Assert.AreEqual(expectedHeight, imageToTest.Placement.Height);
      }

      private void AssertImageUpdate(Id<ReportImage> originalReportImageId, ReportComponentPlacement originalPlacement, string expectedImage, int expectedWidth, int expectedHeight)
      {
         var currentImage = _history.CurrentSnapShot.GetItem(originalReportImageId);
         var currentPlacement = currentImage.Placement;

         Assert.AreEqual(originalPlacement.ReportSectionId, currentPlacement.ReportSectionId);
         Assert.AreEqual(originalReportImageId, currentImage.Id);
         Assert.AreEqual(expectedImage, currentImage.Image);
         Assert.AreEqual(expectedWidth, currentPlacement.Width);
         Assert.AreEqual(expectedHeight, currentPlacement.Height);
      }
   }
}
