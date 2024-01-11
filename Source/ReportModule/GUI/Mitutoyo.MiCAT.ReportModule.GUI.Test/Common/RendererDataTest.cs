// <copyright file="RendererDataTest.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using System.Drawing.Printing;
using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.ReportModule.Domain;
using Mitutoyo.MiCAT.ReportModule.Domain.Components.ReportSections;
using Mitutoyo.MiCAT.ReportModule.Domain.Data;
using Mitutoyo.MiCAT.ReportModule.GUI.Common;
using Mitutoyo.MiCAT.ReportModule.GUI.Common.DisabledSpace;
using Mitutoyo.MiCAT.ReportModule.GUI.Common.LayoutCalculator;
using Mitutoyo.MiCAT.ReportModule.GUI.PageLayout;
using Mitutoyo.MiCAT.ReportModule.GUI.ReportBoundarySection;
using Mitutoyo.MiCAT.ReportModuleApp.Controllers;
using Moq;
using NUnit.Framework;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Test.Common
{
   [ExcludeFromCodeCoverage]
   [TestFixture]
   public class RendererDataTest
   {
      private Mock<IDisabledSpaceDataCollection> _disabledSpaceDataCollection;
      private Mock<IVMPages> _vmPages;
      private RenderedData _rendererData;
      private IReportModeProperty _reportModeProperty;
      private Mock<IAppStateHistory> _historyMock;
      private Mock<IImageController> _imageControllerMock;
      private Mock<ITextBoxController> _textboxControllerMock;
      private Mock<IHeaderFormController> _headerFormControllerMock;
      private Mock<ISelectedComponentController> _selectedComponentControllerMock;
      private Mock<ISelectedSectionController> _sectionSelectionControllerMock;

      [SetUp]
      public void Setup()
      {
         _textboxControllerMock = new Mock<ITextBoxController>();
         _headerFormControllerMock = new Mock<IHeaderFormController>();
         _sectionSelectionControllerMock = new Mock<ISelectedSectionController>();
         _selectedComponentControllerMock = new Mock<ISelectedComponentController>();
         _disabledSpaceDataCollection = new Mock<IDisabledSpaceDataCollection>();
         _vmPages = new Mock<IVMPages>();

         _rendererData = new RenderedData(_disabledSpaceDataCollection.Object, _vmPages.Object);

         _historyMock = new Mock<IAppStateHistory>();
         _historyMock.Setup(h => h.AddClient(It.IsAny<ISubscriptionClient>(), 0)).Returns(_historyMock.Object);
         _reportModeProperty = new ReportModeProperty(_historyMock.Object);

         _imageControllerMock = new Mock<IImageController>();
      }

      [Test]
      public void IsInDisabledSpaceTest()
      {
         //Arrange
         int visualY = 10;

         //Act
         _rendererData.IsInDisabledSpace(visualY);

         //Assert
         _disabledSpaceDataCollection.Verify(c => c.IsInDisabledSpace(visualY, null), Times.Once);
      }

      [Test]
      public void ConvertToDomainYTest()
      {
         //Arrange
         var reportHeader = new ReportHeader();
         var reportFooter = new ReportFooter();
         int visualY = 100;
         int spaceTaken = 50;
         var vmHeader = new VMReportBoundarySection(reportHeader.Id, 0, 1, _imageControllerMock.Object, _textboxControllerMock.Object, _headerFormControllerMock.Object, _selectedComponentControllerMock.Object, _sectionSelectionControllerMock.Object);
         var vmFooter = new VMReportBoundarySection(reportFooter.Id, 0, 1, _imageControllerMock.Object, _textboxControllerMock.Object, _headerFormControllerMock.Object, _selectedComponentControllerMock.Object, _sectionSelectionControllerMock.Object);
         var pageLayoutForTest = new CommonPageLayout(new PageSizeInfo(PaperKind.Custom, 2000, 1000), new Margin(MarginKind.Narrow, 0, 48, 0, 48));

         VMPage page = new VMPage(pageLayoutForTest, _reportModeProperty, vmHeader, vmFooter);

         _vmPages.Setup(p => p.GetPageForPosition(visualY)).Returns(page);
         _disabledSpaceDataCollection.Setup(c => c.TotalUsableSpaceTaken(visualY, null)).Returns(spaceTaken);

         //Act
         var result = _rendererData.ConvertToDomainY(visualY);

         //Assert
         _disabledSpaceDataCollection.Verify(c => c.TotalUsableSpaceTaken(visualY, null), Times.Once);
         Assert.AreEqual((visualY - page.OffsetForComponentsOnThisPage - spaceTaken), result);
      }

      [Test]
      public void ConvertToDomainYWhenBetweenPagesTest()
      {
         //Arrange
         int visualY = 100;
         VMPage nullPage = null;
         int expedtedResult = 500;

         _vmPages.Setup(p => p.GetPageForPosition(visualY)).Returns(nullPage);
         _vmPages.Setup(p => p.GetDomainOutsidePages(visualY)).Returns(expedtedResult);

         //Act
         var result = _rendererData.ConvertToDomainY(visualY);

         //Assert
         _vmPages.Verify(p => p.GetDomainOutsidePages(visualY), Times.Once);
         Assert.AreEqual(expedtedResult, result);
      }
   }
}
