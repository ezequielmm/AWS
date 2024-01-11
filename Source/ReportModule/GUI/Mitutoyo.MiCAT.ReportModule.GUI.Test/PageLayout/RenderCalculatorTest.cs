// <copyright file="RenderCalculatorTest.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.ReportModule.Domain.Data;
using Mitutoyo.MiCAT.ReportModule.GUI.Common;
using Mitutoyo.MiCAT.ReportModule.GUI.Common.DisabledSpace;
using Mitutoyo.MiCAT.ReportModule.GUI.Common.LayoutCalculator;
using Mitutoyo.MiCAT.ReportModule.GUI.PageLayout;
using Mitutoyo.MiCAT.ReportModule.GUI.ReportBoundarySection;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElementList.ViewModels;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements;
using Mitutoyo.MiCAT.ReportModuleApp.Controllers;
using Mitutoyo.MiCAT.ReportModuleApp.Utilities;
using Moq;
using NUnit.Framework;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Test.PageLayout
{
   [ExcludeFromCodeCoverage]
   public class RenderCalculatorTest
   {
      private VMAppStateTestManager _vmAppStateTestManager;
      private Mock<IVMReportElementList> _reportElementListMock;
      private Mock<IDisabledSpaceDataCollection> _disabledSpaces;
      private Mock<IReportBoundarySectionUpdateReceiver> _reportBoundarySectionUpdateReceiverMock;
      private VMReportBoundarySectionFactory _vmReportBoundarySectionFactory;
      private IReportModeProperty _reportModeProperty;
      private Mock<IAppStateHistory> _historyMock;
      private Mock<IActionCaller> _actionCallerMock;
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
         _reportBoundarySectionUpdateReceiverMock = new Mock<IReportBoundarySectionUpdateReceiver>();
         _reportBoundarySectionUpdateReceiverMock.Setup(u => u.IsInEditMode).Returns(true);
         _imageControllerMock = new Mock<IImageController>();
         _selectedComponentControllerMock = new Mock<ISelectedComponentController>();
         _sectionSelectionControllerMock = new Mock<ISelectedSectionController>();
         _historyMock = new Mock<IAppStateHistory>();
         _historyMock.Setup(h => h.AddClient(It.IsAny<ISubscriptionClient>(), 0)).Returns(_historyMock.Object);
         _vmReportBoundarySectionFactory = new VMReportBoundarySectionFactory(_historyMock.Object, _reportBoundarySectionUpdateReceiverMock.Object, _imageControllerMock.Object, _textboxControllerMock.Object, _headerFormControllerMock.Object, _selectedComponentControllerMock.Object, _sectionSelectionControllerMock.Object);
          var reportElementListItems = new List<IVMReportComponent>().ToImmutableList();
         _reportElementListMock = new Mock<IVMReportElementList>();
         _reportElementListMock.Setup(p => p.Elements).Returns(reportElementListItems);
         _vmAppStateTestManager = new VMAppStateTestManager();
         _disabledSpaces = new Mock<IDisabledSpaceDataCollection>();
         _disabledSpaces.Setup(d => d.Items).Returns(new List<DisabledSpaceData>());
         _reportModeProperty = new ReportModeProperty(_historyMock.Object);
         _actionCallerMock = new Mock<IActionCaller>();
      }

      [Test]
      public void VMPageLayout_AppstateCommonPageLayoutChange()
      {
         // Arrange
         var multiPageElementManagerMock = new Mock<IMultiPageElementManager>();
         var commonPageLayout = _vmAppStateTestManager.CurrentSnapShot.GetItems<CommonPageLayout>().Single();
         var renderContent = new RenderedData(_disabledSpaces.Object, new VMPages(_vmReportBoundarySectionFactory, _reportBoundarySectionUpdateReceiverMock.Object, _reportModeProperty));
         var renderCalculator = new PagesRenderer(renderContent, _reportElementListMock.Object, new PageLayoutCalculator(multiPageElementManagerMock.Object), _vmAppStateTestManager.History, _actionCallerMock.Object);

         // Act
         _vmAppStateTestManager.UpdateSnapshot(commonPageLayout.With(new PageSizeInfo(System.Drawing.Printing.PaperKind.Letter, 500, 700)));

         Assert.AreEqual(renderCalculator.ActualPageSettings.PageSize.Width, 700);
         Assert.AreEqual(renderCalculator.ActualPageSettings.PageSize.Height, 500);
      }
   }
}
