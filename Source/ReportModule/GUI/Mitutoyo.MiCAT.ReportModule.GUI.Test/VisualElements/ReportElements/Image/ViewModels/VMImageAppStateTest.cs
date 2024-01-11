// <copyright file="VMImageAppStateTest.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

namespace Mitutoyo.MiCAT.ReportModule.GUI.Test.VisualElements.ReportElements.Image.ViewModels
{
   using System;
   using System.Diagnostics.CodeAnalysis;
   using Mitutoyo.MiCAT.ReportModule.Domain.Components;
   using Mitutoyo.MiCAT.ReportModule.Domain.Components.ReportSections;
   using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements;
   using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements.Image.ViewModels;
   using Mitutoyo.MiCAT.ReportModuleApp.Controllers;
   using Mitutoyo.MiCAT.ReportModuleApp.Utilities;
   using Moq;
   using NUnit.Framework;

   [TestFixture]
   [ExcludeFromCodeCoverage]
   public class VMImageAppStateTest
   {
      private VMImage _viewModel;
      private VMAppStateTestManager _vmAppStateTestManager;
      private Mock<IImageController> imageControllerMock;
      private Mock<IDeleteComponentController> _deleteComponentControllerMock;
      private Mock<IActionCaller> _actionCallerMock;

      [SetUp]
      public void Setup()
      {
         imageControllerMock = new Mock<IImageController>();
         _deleteComponentControllerMock = new Mock<IDeleteComponentController>();
         _actionCallerMock = new Mock<IActionCaller>();

         _vmAppStateTestManager = new VMAppStateTestManager();
      }

      [TearDown]
      public void TearDown()
      {
         _viewModel?.Dispose();
      }

      [Test]
      public void VMImage_ShouldUpdateImage()
      {
         //Arange
         const string NEW_IMAGE_BASE64 = "ODc2NTQzMjE=";
         var newImageBytes = Convert.FromBase64String(NEW_IMAGE_BASE64);
         var reportBody = new ReportBody();
         var placement = new ReportComponentPlacement(reportBody.Id, 10, 10, 100, 50);
         var reportImage = new ReportImage(placement, "MTIzNDU2Nzg=");
         var vmPlacement = new Mock<IVMReportComponentPlacement>();
         _vmAppStateTestManager.UpdateSnapshot(reportBody);
         _vmAppStateTestManager.UpdateSnapshot(reportImage);

         _viewModel = new VMImage(
            _vmAppStateTestManager.History,
            reportImage.Id,
            vmPlacement.Object,
            _deleteComponentControllerMock.Object,
            imageControllerMock.Object,
            _actionCallerMock.Object);

         //Act
         _vmAppStateTestManager.UpdateSnapshot(reportImage.WithImage(NEW_IMAGE_BASE64) as ReportImage);

         //Assert
         Assert.AreEqual(newImageBytes, _viewModel.Image);
      }
   }
}
