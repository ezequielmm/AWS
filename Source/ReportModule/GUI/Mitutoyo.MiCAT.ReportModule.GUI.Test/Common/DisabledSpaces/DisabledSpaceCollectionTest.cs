// <copyright file="DisabledSpaceCollectionTest.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using System.Drawing.Printing;
using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.ReportModule.App.AppState;
using Mitutoyo.MiCAT.ReportModule.Domain.Components.ReportSections;
using Mitutoyo.MiCAT.ReportModule.Domain.Data;
using Mitutoyo.MiCAT.ReportModule.GUI.Common;
using Mitutoyo.MiCAT.ReportModule.GUI.Common.DisabledSpace;
using Mitutoyo.MiCAT.ReportModule.GUI.PageLayout;
using Mitutoyo.MiCAT.ReportModule.GUI.ReportBoundarySection;
using Mitutoyo.MiCAT.ReportModuleApp.Controllers;
using Moq;
using NUnit.Framework;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Test.Common.DisabledSpaces
{
   [ExcludeFromCodeCoverage]
   [TestFixture]
   public class DisabledSpaceCollectionTest
   {
      private VMPage[] pages;
      private DisabledSpaceDataCollection disabledSpaces;
      private Mock<IReportBoundarySectionUpdateReceiver> _reportBoundarySectionUpdateReceiverMock;
      private Mock<ISelectedSectionController> _sectionSelectionControllerMock;
      private Mock<IImageController> _imageControllerMock;
      private Mock<ITextBoxController> _textboxControllerMock;
      private Mock<IHeaderFormController> _headerFormControllerMock;
      private Mock<ISelectedComponentController> _selectedComponentControllerMock;
      private VMReportBoundarySectionFactory _vmReportBoundarySectionFactory;
      private VMReportBoundarySection _VMReportFooter;
      private VMReportBoundarySection _VMReportHeader;
      private IReportModeProperty _reportModeProperty;
      private Mock<IAppStateHistory> _historyMock;

      [SetUp]
      public void Setup()
      {
         var currentPageLayout = new CommonPageLayout(new PageSizeInfo(PaperKind.Custom, 500, 200), new Margin());
         _textboxControllerMock = new Mock<ITextBoxController>();
         _headerFormControllerMock = new Mock<IHeaderFormController>();
         _reportBoundarySectionUpdateReceiverMock = new Mock<IReportBoundarySectionUpdateReceiver>();
         _sectionSelectionControllerMock = new Mock<ISelectedSectionController>();
         _reportBoundarySectionUpdateReceiverMock.Setup(u => u.IsInEditMode).Returns(true);
         _imageControllerMock = new Mock<IImageController>();
         _selectedComponentControllerMock = new Mock<ISelectedComponentController>();

         _historyMock = new Mock<IAppStateHistory>();
         _historyMock.Setup(h => h.AddClient(It.IsAny<ISubscriptionClient>(), 0)).Returns(_historyMock.Object);
         var snapShot = new Mock<ISnapShot>();
         var reportMode = new ReportModeState(true);
         var componentSelection = new ReportComponentSelection();

         snapShot.Setup(s => s.GetItems<ReportComponentSelection>()).Returns(new ReportComponentSelection[] { componentSelection });
         snapShot.Setup(s => s.GetItems<ReportModeState>()).Returns(new ReportModeState[] { reportMode });
         snapShot.Setup(s => s.GetItems<ReportHeader>()).Returns(new ReportHeader[] { new ReportHeader() });
         snapShot.Setup(s => s.GetItems<ReportFooter>()).Returns(new ReportFooter[] { new ReportFooter() });
         _historyMock.Setup(h => h.CurrentSnapShot).Returns(snapShot.Object);

         _reportModeProperty = new ReportModeProperty(_historyMock.Object);

         _vmReportBoundarySectionFactory = new VMReportBoundarySectionFactory(_historyMock.Object, _reportBoundarySectionUpdateReceiverMock.Object, _imageControllerMock.Object, _textboxControllerMock.Object, _headerFormControllerMock.Object, _selectedComponentControllerMock.Object, _sectionSelectionControllerMock.Object);

         _VMReportFooter = _vmReportBoundarySectionFactory.CreateForFooter(5, 1);
         _VMReportHeader = _vmReportBoundarySectionFactory.CreateForHeader(5, 1);

         //Arrange
         pages = new VMPage[]
         {
            new VMPage(currentPageLayout, _reportModeProperty, _VMReportHeader, _VMReportFooter),
            new VMPage(currentPageLayout, _reportModeProperty, _VMReportHeader, _VMReportFooter),
            new VMPage(currentPageLayout, _reportModeProperty, _VMReportHeader, _VMReportFooter)
         };

         disabledSpaces = new DisabledSpaceDataCollection();
         disabledSpaces.Items.Add(new DisabledSpaceData(0, pages[0], 10, pages[0], 450, 440));
         disabledSpaces.Items.Add(new DisabledSpaceData(500, pages[1], 600, pages[2], 800, 500));
         disabledSpaces.Items.Add(new DisabledSpaceData(1000, pages[2], 1100, pages[2], 1300, 200));
      }

      [Test]
      [TestCase(20, 440)]
      [TestCase(300, 440)]
      [TestCase(600, 440)]
      [TestCase(750, 940)]
      [TestCase(1100, 940)]
      [TestCase(1250, 1140)]
      public void GetTotalUsableSpaceTaken(int verticalPosition, int expectedSpaceTaken)
      {
         //Act
         var spaceTaken = disabledSpaces.TotalUsableSpaceTaken(verticalPosition, null);

         //Assert
         Assert.AreEqual(expectedSpaceTaken, spaceTaken);
      }

      [Test]
      [TestCase(5, false)]
      [TestCase(20, true)]
      [TestCase(500, false)]
      [TestCase(700, true)]
      [TestCase(500, false)]
      public void GetCantOfDisabledSpacesAbove(int verticalPosition, bool expectNullValue)
      {
         //Act
         var pushArea = disabledSpaces.IsInDisabledSpace(verticalPosition);

         //Assert
         Assert.AreEqual(expectNullValue, pushArea);
      }

      [Test]
      [TestCase(5, false)]
      [TestCase(50, true)]
      [TestCase(500, false)]
      [TestCase(700, true)]
      [TestCase(900, false)]
      [TestCase(1250, true)]
      public void IsInDisabledSpace(int verticalPosition, bool expectedValue)
      {
         //Act
         bool isInDisabledSpace = disabledSpaces.IsInDisabledSpace(verticalPosition);

         //Assert
         Assert.AreEqual(expectedValue, isInDisabledSpace);
      }
   }
}
