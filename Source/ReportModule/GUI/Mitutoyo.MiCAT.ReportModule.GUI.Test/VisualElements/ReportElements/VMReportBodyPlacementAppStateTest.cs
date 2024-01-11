// <copyright file="VMReportBodyPlacementAppStateTest.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.ReportModule.Domain.Components;
using Mitutoyo.MiCAT.ReportModule.Domain.Components.ReportSections;
using Mitutoyo.MiCAT.ReportModule.GUI.Common.LayoutCalculator;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements;
using Mitutoyo.MiCAT.ReportModuleApp.Controllers;
using Moq;
using NUnit.Framework;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Test.VisualElements.ReportElements
{
   [TestFixture]
   [ExcludeFromCodeCoverage]
   public class VMReportBodyPlacementAppStateTest
   {
      private VMReportBodyPlacement _viewModel;
      private Mock<IReportComponentPlacementController> _placementController;
      private Mock<ISelectedComponentController> selectedComponentController;
      private Mock<IRenderedData> _renderData;

      [SetUp]
      public void Setup()
      {
         //Arrange
         _placementController = new Mock<IReportComponentPlacementController>();
         selectedComponentController = new Mock<ISelectedComponentController>();

         _renderData = new Mock<IRenderedData>();
      }

      [Test]
      public void VMReportBodyPlacement_ShouldUpdateSizeAndPosition()
      {
         //Arrange
         ReportBody bodySection = new ReportBody();
         ReportComponentPlacement placement = new ReportComponentPlacement(bodySection.Id, 0, 0, 0, 0);
         ReportComponentFake fake = new ReportComponentFake(placement);

         _viewModel = new VMReportBodyPlacement(
            fake.Id,
            fake.Placement,
            _placementController.Object,
            selectedComponentController.Object,
            _renderData.Object);

         //Act
         _viewModel.UpdateFromPlacementEntity(placement, new ReportComponentPlacement(bodySection.Id, 1, 2, 3, 4));

         //Assert
         Assert.AreEqual(1, _viewModel.DomainX);
         Assert.AreEqual(2, _viewModel.DomainY);
         Assert.AreEqual(3, _viewModel.DomainWidth);
         Assert.AreEqual(4, _viewModel.DomainHeight);
      }

      [Test]
      public void VMReportComponent_OnEditModeShouldBeSelected()
      {
         //Arrange
         ReportBody bodySection = new ReportBody();
         ReportComponentPlacement placement = new ReportComponentPlacement(bodySection.Id, 0, 0, 0, 0);
         ReportComponentFake fake = new ReportComponentFake(placement);

         _viewModel = new VMReportBodyPlacement(
            fake.Id,
            fake.Placement,
            _placementController.Object,
            selectedComponentController.Object,
            _renderData.Object);

         _viewModel.IsSelectable = true;

         //Act
         _viewModel.Select(null);

         //Assert
         selectedComponentController.Verify(s => s.SetSelected(It.Is<Id<ReportComponentFake>>(id => id == fake.Id)), Times.Once());
      }

      [Test]
      public void VMReportComponent_OnEditModeShouldBeUnselected()
      {
         //Arrange
         ReportBody bodySection = new ReportBody();
         ReportComponentPlacement placement = new ReportComponentPlacement(bodySection.Id, 0, 0, 0, 0);
         ReportComponentFake fake = new ReportComponentFake(placement);

         _viewModel = new VMReportBodyPlacement(
            fake.Id,
            fake.Placement,
            _placementController.Object,
            selectedComponentController.Object,
            _renderData.Object);

         //Act
         _viewModel.UpdateSelectionState(false);

         //Assert
         Assert.IsFalse(_viewModel.IsSelected);
      }
   }
}
