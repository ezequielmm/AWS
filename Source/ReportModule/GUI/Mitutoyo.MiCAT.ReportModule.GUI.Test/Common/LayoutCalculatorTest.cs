// <copyright file="LayoutCalculatorTest.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Drawing.Printing;
using System.Linq;
using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.ReportModule.App.AppState;
using Mitutoyo.MiCAT.ReportModule.Domain;
using Mitutoyo.MiCAT.ReportModule.Domain.Components;
using Mitutoyo.MiCAT.ReportModule.Domain.Components.ReportSections;
using Mitutoyo.MiCAT.ReportModule.Domain.Data;
using Mitutoyo.MiCAT.ReportModule.GUI.Common;
using Mitutoyo.MiCAT.ReportModule.GUI.Common.DisabledSpace;
using Mitutoyo.MiCAT.ReportModule.GUI.Common.LayoutCalculator;
using Mitutoyo.MiCAT.ReportModule.GUI.PageLayout;
using Mitutoyo.MiCAT.ReportModule.GUI.ReportBoundarySection;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElementList.ViewModels;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements.Image.ViewModels;
using Mitutoyo.MiCAT.ReportModuleApp.Controllers;
using Mitutoyo.MiCAT.ReportModuleApp.Utilities;
using Moq;
using NUnit.Framework;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Test.Common
{
   [ExcludeFromCodeCoverage]
   [TestFixture]
   public class LayoutCalculatorTest
   {
      private Mock<ISelectedComponentController> _selectedComponentControllerMock;
      private Mock<IDeleteComponentController> _deleteComponentControllerMock;
      private IPageLayoutCalculator _pageLayoutCalculator;
      private Mock<IAppStateHistory> _historyMock;
      private Mock<ISnapShot> _snapShotMock;
      private Mock<IMultiPageElementManager> _multiPageElementManagerMock;
      private Mock<IDisabledSpaceDataCollection> _disabledSpaces;
      private Mock<IVMReportElementList> _elementList;
      private Mock<IActionCaller> _actionCallerMock;
      private Mock<IReportBoundarySectionUpdateReceiver> _reportBoundarySectionUpdateReceiverMock;
      private VMReportBoundarySectionFactory _vmReportBoundarySectionFactory;
      private IReportModeProperty _reportModeProperty;
      private Mock<IImageController> _imageControllerMock;
      private Mock<ITextBoxController> _textboxControllerMock;
      private Mock<IHeaderFormController> _headerFormControllerMock;
      private Mock<ISelectedSectionController> _sectionSelectionControllerMock;
      private Mock<IReportComponentPlacementController> _placementControllerMock;

      [SetUp]
      public void Setup()
      {
         _textboxControllerMock = new Mock<ITextBoxController>();
         _headerFormControllerMock = new Mock<IHeaderFormController>();
         _reportBoundarySectionUpdateReceiverMock = new Mock<IReportBoundarySectionUpdateReceiver>();
         _reportBoundarySectionUpdateReceiverMock.Setup(u => u.IsInEditMode).Returns(true);
         _imageControllerMock = new Mock<IImageController>();
         _selectedComponentControllerMock = new Mock<ISelectedComponentController>();
         _sectionSelectionControllerMock = new Mock<ISelectedSectionController>();
         _deleteComponentControllerMock = new Mock<IDeleteComponentController>();
         _placementControllerMock = new Mock<IReportComponentPlacementController>();
         var componentSelection = new ReportComponentSelection();

         _snapShotMock = new Mock<ISnapShot>();
         _snapShotMock.Setup(s => s.GetItems<ReportComponentSelection>()).Returns(new ReportComponentSelection[] { componentSelection });
         _snapShotMock.Setup(s => s.GetItems<ReportHeader>()).Returns(new ReportHeader[] { new ReportHeader() });
         _snapShotMock.Setup(s => s.GetItems<ReportFooter>()).Returns(new ReportFooter[] { new ReportFooter() });
         _multiPageElementManagerMock = new Mock<IMultiPageElementManager>();
         _pageLayoutCalculator = new PageLayoutCalculator(_multiPageElementManagerMock.Object);
         _historyMock = new Mock<IAppStateHistory>();
         _historyMock.Setup(h => h.AddClient(It.IsAny<ISubscriptionClient>(), 0)).Returns(_historyMock.Object);
         _historyMock.Setup(h => h.CurrentSnapShot).Returns(_snapShotMock.Object);

         _elementList = new Mock<IVMReportElementList>();
         _disabledSpaces = new Mock<IDisabledSpaceDataCollection>();
         _disabledSpaces.Setup(ds => ds.Items).Returns(new List<DisabledSpaceData>());
         _actionCallerMock = new Mock<IActionCaller>();
         _reportModeProperty = new ReportModeProperty(_historyMock.Object);
         _vmReportBoundarySectionFactory = new VMReportBoundarySectionFactory(_historyMock.Object, _reportBoundarySectionUpdateReceiverMock.Object, _imageControllerMock.Object, _textboxControllerMock.Object, _headerFormControllerMock.Object, _selectedComponentControllerMock.Object, _sectionSelectionControllerMock.Object);
      }

      [Test]
      public void Update_ShouldSetActualPageSettings()
      {
         //Arrange
         var layoutCalculator = BuildLayoutCalculator();

         //Act
         layoutCalculator.Update(_snapShotMock.Object);
         //Assert
         Assert.AreEqual(layoutCalculator.ActualPageSettings.CanvasMargin.MarginKind, MarginKind.Narrow);
         Assert.AreEqual(layoutCalculator.ActualPageSettings.CanvasMargin.ToString(), "0,48,0,48");
         Assert.AreEqual(layoutCalculator.ActualPageSettings.CanvasMargin.Left, 0);
         Assert.AreEqual(layoutCalculator.ActualPageSettings.CanvasMargin.Top, 48);
         Assert.AreEqual(layoutCalculator.ActualPageSettings.CanvasMargin.Right, 0);
         Assert.AreEqual(layoutCalculator.ActualPageSettings.CanvasMargin.Bottom, 48);

         Assert.AreEqual(layoutCalculator.ActualPageSettings.PageSize.PaperKind, PaperKind.A4);
         Assert.AreEqual(layoutCalculator.ActualPageSettings.PageSize.Width, 794);
         Assert.AreEqual(layoutCalculator.ActualPageSettings.PageSize.Height, 1122);
      }

      [Test]
      public void Update_ShouldSetElementListPositionsAndSizes()
      {
         //Arrange
         var pagesRenderer = BuildLayoutCalculator();

         //Act
         pagesRenderer.Update(_snapShotMock.Object);

         //Assert
         Assert.AreEqual(pagesRenderer.ElementList.Elements.Count, 2);

         var vmPlacement1 = pagesRenderer.ElementList.Elements[0].VMPlacement as IVMReportBodyPlacement;
         var vmPlacement2 = pagesRenderer.ElementList.Elements[1].VMPlacement as IVMReportBodyPlacement;

         Assert.IsNotNull(vmPlacement1);
         Assert.AreEqual(vmPlacement1.DomainHeight, 15);
         Assert.AreEqual(vmPlacement1.DomainY, 30);
         Assert.AreEqual(vmPlacement1.PageVerticalOffset, 68);
         Assert.AreEqual(vmPlacement1.VisualY, 98);
         Assert.AreEqual(vmPlacement1.DomainWidth, 10);
         Assert.AreEqual(vmPlacement1.DomainX, 20);

         Assert.IsNotNull(vmPlacement2);
         Assert.AreEqual(vmPlacement2.DomainHeight, 20);
         Assert.AreEqual(vmPlacement2.DomainY, 35);
         Assert.AreEqual(vmPlacement2.PageVerticalOffset, 68);
         Assert.AreEqual(vmPlacement2.VisualY, 103);
         Assert.AreEqual(vmPlacement2.DomainWidth, 15);
         Assert.AreEqual(vmPlacement2.DomainX, 25);
      }

      private PagesRenderer BuildLayoutCalculator()
      {
         _elementList = new Mock<IVMReportElementList>();
         var vmPages = new VMPages(_vmReportBoundarySectionFactory, _reportBoundarySectionUpdateReceiverMock.Object, _reportModeProperty);
         var renderContent = new RenderedData(_disabledSpaces.Object, vmPages);
         var pageSettings = new CommonPageLayout(new PageSizeInfo(PaperKind.A4, 1122, 794), new Margin(MarginKind.Narrow, 0, 48, 0, 48), new HeaderData(5), new FooterData(5));

         var firtPage = vmPages.NextPage(pageSettings);
         vmPages.NextPage(pageSettings);

         var pagesRenderer = new PagesRenderer(renderContent, _elementList.Object, _pageLayoutCalculator, _historyMock.Object, _actionCallerMock.Object);

         var imageControllerMock = new Mock<IImageController>();
         var changesMock = new Mock<IImmutableList<IItemChange>>();

         var reportBody = new ReportBody();

         var placement1 = new ReportComponentPlacement(reportBody.Id, 20, 30, 10, 15);
         var img1 = new ReportImage(placement1).WithImage("dfghfgdfgd667878nnvghv==");

         var placement2 = new ReportComponentPlacement(reportBody.Id, 25, 35, 15, 20);
         var img2 = new ReportImage(placement2).WithImage("dfghfgdfgd667878nnvghvsadfasdfasdfasdf==");

         var item1 = new Mock<IItemChange>();
         item1.Setup(i => i.ItemId).Returns(img1.Id);

         var item2 = new Mock<IItemChange>();
         item2.Setup(i => i.ItemId).Returns(img2.Id);

         _snapShotMock.Setup(ss => ss.GetItems<CommonPageLayout>()).Returns(new[] { pageSettings });
         _snapShotMock.Setup(ss => ss.GetItem(img1.Id as IItemId<IReportComponent>)).Returns(img1);
         _snapShotMock.Setup(ss => ss.GetItem(img2.Id as IItemId<IReportComponent>)).Returns(img2);

         _snapShotMock.Setup(ss => ss.GetChanges()).Returns(new Changes(ImmutableList<IItemChange>.Empty.AddRange(new[] { item1.Object, item2.Object })));
         _snapShotMock.Setup(ss => ss.GetItems<ReportModeState>()).Returns(new[] { new ReportModeState(false) });

         var vmReportBodyPlacement1 = new VMReportBodyPlacement(img1.Id, img2.Placement, _placementControllerMock.Object, _selectedComponentControllerMock.Object, renderContent);
         var vmReportBodyPlacement2 = new VMReportBodyPlacement(img2.Id, img2.Placement, _placementControllerMock.Object, _selectedComponentControllerMock.Object, renderContent);

         _elementList.Setup(rel => rel.Elements)
            .Returns(new[]
               {
                  new VMImage(_historyMock.Object, img1.Id, vmReportBodyPlacement1, _deleteComponentControllerMock.Object,_imageControllerMock.Object,_actionCallerMock.Object),
                  new VMImage(_historyMock.Object, img2.Id, vmReportBodyPlacement2, _deleteComponentControllerMock.Object,_imageControllerMock.Object,_actionCallerMock.Object),
               }
               .Cast<IVMReportComponent>()
               .ToImmutableList());

         _pageLayoutCalculator.CalculateComponentPositions(pageSettings, _elementList.Object, renderContent);

         return pagesRenderer;
      }
   }
}
