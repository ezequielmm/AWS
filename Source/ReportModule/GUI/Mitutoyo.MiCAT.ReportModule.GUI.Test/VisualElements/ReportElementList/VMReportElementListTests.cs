// <copyright file="VMReportElementListTests.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Mitutoyo.MiCAT.ReportModule.Domain.Components;
using Mitutoyo.MiCAT.ReportModule.Domain.Components.ReportSections;
using Mitutoyo.MiCAT.ReportModule.GUI.Common.LayoutCalculator;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElementList.ViewModels;
using Mitutoyo.MiCAT.ReportModuleApp.Controllers;
using Mitutoyo.MiCAT.ReportModuleApp.Utilities;
using Moq;
using NUnit.Framework;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Test.VisualElements.ReportElementList
{
   [TestFixture]
   [ExcludeFromCodeCoverage]
   public class VMReportElementListTests
   {
      private VMAppStateTestManager _vmAppStateTestManager;

      private Mock<ISelectedComponentController> _selectionComponentControllerMock;
      private Mock<ITextBoxController> _textBoxControllerMock;
      private Mock<ITableViewController> _tableViewControllerMock;
      private Mock<ITessellationViewController> _tessellationViewControllerMock;
      private Mock<IImageController> _imageControllerMock;
      private Mock<IDeleteComponentController> _deleteControllerMock;
      private Mock<IHeaderFormController> _headerFormControllerMock;
      private Mock<IHeaderFormFieldController> _headerFormFieldControllerMock;
      private Mock<IRenderedData> _renderData;
      private Mock<IActionCaller> _actionCallerMock;
      private Mock<IReportComponentPlacementController> _placementControllerMock;
      private ReportBody _reportBody;

      [SetUp]
      public void Setup()
      {
         _textBoxControllerMock = new Mock<ITextBoxController>();
         _tableViewControllerMock = new Mock<ITableViewController>();
         _tessellationViewControllerMock = new Mock<ITessellationViewController>();
         _headerFormControllerMock = new Mock<IHeaderFormController>();
         _headerFormFieldControllerMock = new Mock<IHeaderFormFieldController>();
         _imageControllerMock = new Mock<IImageController>();
         _selectionComponentControllerMock = new Mock<ISelectedComponentController>();
         _deleteControllerMock = new Mock<IDeleteComponentController>();
         _actionCallerMock = new Mock<IActionCaller>();
         _renderData = new Mock<IRenderedData>();
         _placementControllerMock = new Mock<IReportComponentPlacementController>();

         _vmAppStateTestManager = new VMAppStateTestManager();

         _reportBody = _vmAppStateTestManager.CurrentSnapShot.GetItems<ReportBody>().First();
      }

      [Test]
      public void VMReportElementList_AppstateShouldAddNewAndRemoveVMReportComponents()
      {
         // Arrange
         var _viewModel = new VMReportElementList(
             _renderData.Object,
             _vmAppStateTestManager.History,
             _placementControllerMock.Object,
             _textBoxControllerMock.Object,
             _tableViewControllerMock.Object,
             _tessellationViewControllerMock.Object,
             _imageControllerMock.Object,
             _headerFormControllerMock.Object,
             _headerFormFieldControllerMock.Object,
             _selectionComponentControllerMock.Object,
             _deleteControllerMock.Object,
             _actionCallerMock.Object);

         // Act
         var reportImagePlacement = new ReportComponentPlacement(_reportBody.Id, 12, 5, 50, 100);
         var reportImage = new ReportImage(reportImagePlacement);

         _vmAppStateTestManager.UpdateSnapshot(reportImage);

         // Assert
         Assert.AreEqual(1, _viewModel.Elements.Count);
         Assert.AreEqual(true, _viewModel.Elements[0].Id.UniqueValue == reportImage.Id);

         // Act
         _vmAppStateTestManager.DeleteItemFromSnapshot(reportImage);

         // Assert
         CollectionAssert.IsEmpty(_viewModel.Elements);
      }
   }
}
