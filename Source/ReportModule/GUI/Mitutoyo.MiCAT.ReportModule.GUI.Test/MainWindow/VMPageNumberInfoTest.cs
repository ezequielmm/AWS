// <copyright file="VMPageNumberInfoTest.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Drawing.Printing;
using System.Linq;
using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.ReportModule.Domain.Components;
using Mitutoyo.MiCAT.ReportModule.Domain.Components.ReportSections;
using Mitutoyo.MiCAT.ReportModule.Domain.Data;
using Mitutoyo.MiCAT.ReportModule.GUI.Common;
using Mitutoyo.MiCAT.ReportModule.GUI.Common.LayoutCalculator;
using Mitutoyo.MiCAT.ReportModule.GUI.PageLayout;
using Mitutoyo.MiCAT.ReportModule.GUI.ReportBoundarySection;
using Mitutoyo.MiCAT.ReportModule.GUI.Test.PageLayout;
using Mitutoyo.MiCAT.ReportModule.GUI.Test.VisualElements.ReportElements;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElementList.ViewModels;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements;
using Mitutoyo.MiCAT.ReportModuleApp.Controllers;
using Moq;
using NUnit.Framework;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Test.MainWindow
{
   [ExcludeFromCodeCoverage]
   [TestFixture]
   public class VMPageNumberInfoTest
   {
      private Mock<IPagesRenderer> _pagesRenderer;
      private Mock<IAppStateHistory> _historyMock;
      private VMAppStateTestManager _vmAppStateTestManager;
      private Mock<IReportBoundarySectionUpdateReceiver> _reportBoundarySectionUpdateReceiverMock;
      private VMReportBoundarySectionFactory _vmReportBoundarySectionFactory;
      private IReportModeProperty _reportModeProperty;
      private Mock<IImageController> _imageControllerMock;
      private Mock<ITextBoxController> _textboxControllerMock;
      private Mock<IHeaderFormController> _headerFormControllerMock;
      private Mock<ISelectedComponentController> _selectedComponentControllerMock;
      private Mock<ISelectedSectionController> _sectionSelectionControllerMock;
      private Mock<ISnapShot> _snapShotMock;

      [SetUp]
      public void Setup()
      {
         _historyMock = new Mock<IAppStateHistory>();
         _textboxControllerMock = new Mock<ITextBoxController>();
         _headerFormControllerMock = new Mock<IHeaderFormController>();
         _reportBoundarySectionUpdateReceiverMock = new Mock<IReportBoundarySectionUpdateReceiver>();
         _reportBoundarySectionUpdateReceiverMock.Setup(u => u.IsInEditMode).Returns(true);
         _imageControllerMock = new Mock<IImageController>();
         _selectedComponentControllerMock = new Mock<ISelectedComponentController>();
         _sectionSelectionControllerMock = new Mock<ISelectedSectionController>();
         _snapShotMock = new Mock<ISnapShot>();

         _vmReportBoundarySectionFactory = new VMReportBoundarySectionFactory(_historyMock.Object, _reportBoundarySectionUpdateReceiverMock.Object, _imageControllerMock.Object, _textboxControllerMock.Object, _headerFormControllerMock.Object, _selectedComponentControllerMock.Object, _sectionSelectionControllerMock.Object);
         _pagesRenderer = new Mock<IPagesRenderer>();
         _historyMock.Setup(h => h.AddClient(It.IsAny<ISubscriptionClient>(), 0)).Returns(_historyMock.Object);
         _historyMock.Setup(h => h.CurrentSnapShot).Returns(_snapShotMock.Object);
         _reportModeProperty = new ReportModeProperty(_historyMock.Object);
         _vmAppStateTestManager = new VMAppStateTestManager();
      }
      [Test]
      public void ShouldConstructProperly()
      {
         // Act
         var sut = new VMPageNumberInfo(_historyMock.Object, _pagesRenderer.Object) { };

         // Assert
         string pageNumber = sut.PageNumberInfo;
         Assert.AreEqual("Page 1 of 1", pageNumber);
      }

      [Test]
      public void ShouldGetPageNumber()
      {
         // Arrange
         var sut = new VMPageNumberInfo(_historyMock.Object, _pagesRenderer.Object) { };

         // Act
         sut.TotalPage = 3;
         sut.CurrentPage = 2;

         // Assert
         string pageNumber = sut.PageNumberInfo;
         Assert.AreEqual("Page 2 of 3", pageNumber);
      }
      [Test]
      public void ShouldNotifyPropertyChanged()
      {
         // Arrange
         var sut = new VMPageNumberInfo(_historyMock.Object, _pagesRenderer.Object) { CurrentPage = 1, TotalPage = 2 };

         // Act
         var logger = new NotifyPropertyChangedLogger(sut);
         sut.TotalPage = 4;
         sut.CurrentPage = 3;

         // Assert
         Assert.AreEqual(2, logger.ChangeLog.Count);
         Assert.AreEqual(nameof(sut.PageNumberInfo), logger.ChangeLog[0]);
         Assert.AreEqual(nameof(sut.PageNumberInfo), logger.ChangeLog[1]);
      }
      [Test]
      public void ShouldNotifyCurrentPageChanged()
      {
         // Arrange
         var sut = new VMPageNumberInfo(_historyMock.Object, _pagesRenderer.Object);
         sut.CurrentPage = 2;
         sut.TotalPage = 3;
         // Act
         var logger = new NotifyPropertyChangedLogger(sut);
         sut.CurrentPage = 1;

         // Assert
         Assert.AreEqual(1, logger.ChangeLog.Count);
         Assert.AreEqual(nameof(sut.PageNumberInfo), logger.ChangeLog[0]);
      }
      [Test]
      public void ShouldNotNotifyPropertyChanged()
      {
         // Arrange
         var sut = new VMPageNumberInfo(_historyMock.Object, _pagesRenderer.Object) { CurrentPage = 2, TotalPage = 2 };

         // Act
         var logger = new NotifyPropertyChangedLogger(sut);
         sut.TotalPage = 2;
         sut.CurrentPage = 2;

         // Assert
         Assert.AreEqual(0, logger.ChangeLog.Count);
      }
      [Test]
      public void ShouldNotNotifyTotalPageChanged()
      {
         // Arrange
         var sut = new VMPageNumberInfo(_historyMock.Object, _pagesRenderer.Object) { CurrentPage = 2, TotalPage = 2 };

         // Act
         var logger = new NotifyPropertyChangedLogger(sut);
         sut.TotalPage = 2;

         // Assert
         Assert.AreEqual(0, logger.ChangeLog.Count);
      }
      [Test]
      public void ShouldNotNotifyCurrentPageChanged()
      {
         // Arrange
         var sut = new VMPageNumberInfo(_historyMock.Object, _pagesRenderer.Object) { CurrentPage = 2, TotalPage = 2 };

         // Act
         var logger = new NotifyPropertyChangedLogger(sut);
         sut.CurrentPage = 2;

         // Assert
         Assert.AreEqual(0, logger.ChangeLog.Count);
      }
      [Test]
      public void UpdateWithSelectedNullShouldReturnStringWithHashTag()
      {
         // Arrange
         var sut = new VMPageNumberInfo(_historyMock.Object, _pagesRenderer.Object) { TotalPage = 2 };
         var selectedComponent = _vmAppStateTestManager.CurrentSnapShot.GetItems<ReportComponentSelection>().Single();

         _vmAppStateTestManager.UpdateSnapshot(selectedComponent.WithNonSelectedComponent());
         // Act
         var snapShot = _vmAppStateTestManager.CurrentSnapShot;
         sut.Update(snapShot);
         // Assert
         Assert.That(sut.PageNumberInfo.Equals("Page # of 2"));
      }
      [Test]
      public void UpdateWithSelectedComponentShouldReturnStringWithHashTag()
      {
         // Arrange
         var _reportComponentControllerMock = new Mock<IReportComponentPlacementController>();
         var _selectedComponentControllerMock = new Mock<ISelectedComponentController>();
         var _itemIdMock = new Mock<IItemId>();
         var _rendererDataMock = new Mock<IRenderedData>();
         var pageLayoutForTest = new CommonPageLayout(new PageSizeInfo(PaperKind.Custom, 150, 20), new Margin());
         var repComponent = new Mock<IVMReportComponent>();
         var vmPlacement = new Mock<IVMReportComponentPlacement>();
         var placementId = new Id<ReportComponentPlacement>(Guid.NewGuid());
         var reportComponentId = new Id<ReportComponentFake>(Guid.NewGuid());
         var reportHeader = new ReportHeader();
         var reportFooter = new ReportFooter();

         repComponent.Setup(vm => vm.Id).Returns(reportComponentId);
         repComponent.Setup(r => r.VMPlacement).Returns(vmPlacement.Object);
         _pagesRenderer.Setup(p => p.ElementList).Returns(new Mock<IVMReportElementList>().Object);

         _pagesRenderer.Setup(p => p.ElementList.Elements).Returns(ImmutableList.Create(repComponent.Object));
         var sut = new VMPageNumberInfo(_historyMock.Object, _pagesRenderer.Object) { TotalPage = 2 };
         var selectedComponent = _vmAppStateTestManager.CurrentSnapShot.GetItems<ReportComponentSelection>().Single();

         _snapShotMock.Setup(s => s.GetItems<ReportHeader>()).Returns(new List<ReportHeader>() { reportHeader });
         _snapShotMock.Setup(s => s.GetItems<ReportFooter>()).Returns(new List<ReportFooter>() { reportFooter });

         var vmPages = new Mock<IVMPages>();
         var vmPagesValue = new VMPage(pageLayoutForTest, _reportModeProperty, _vmReportBoundarySectionFactory.CreateForHeader(5, 1), _vmReportBoundarySectionFactory.CreateForFooter(5, 1));
         vmPages.Setup(p => p.GetPageForPosition(It.IsAny<int>())).Returns(vmPagesValue);
         _pagesRenderer.Setup(p => p.RenderedData).Returns(new Mock<IRenderedData>().Object);
         _pagesRenderer.Setup(p => p.RenderedData.Pages).Returns(vmPages.Object);
         vmPages.Setup(p => p.Pages).Returns(new ObservableCollection<VMPage>(new List<VMPage>() { vmPagesValue }));

         _vmAppStateTestManager.UpdateSnapshot(selectedComponent.WithJustThisSelectedComponent(reportComponentId));
         var snapShot = _vmAppStateTestManager.History.CurrentSnapShot;

         // Act
         sut.Update(snapShot);

         // Assert
         Assert.That(sut.PageNumberInfo.Equals("Page 1 of 2"));
      }
   }
}
