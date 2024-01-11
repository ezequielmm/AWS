// <copyright file="DisabledSpaceDataTest.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using System.Diagnostics.CodeAnalysis;
using System.Drawing.Printing;
using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.ReportModule.Domain;
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
   public class DisabledSpaceDataTest
   {
      private CommonPageLayout _pageLayoutForTest;
      private VMReportBoundarySection _VMReportFooter;
      private VMReportBoundarySection _VMReportHeader;
      private Mock<ISelectedSectionController> _sectionSelectionControllerMock;
      private Mock<IImageController> _imageControllerMock;
      private Mock<ITextBoxController> _textboxControllerMock;
      private Mock<IHeaderFormController> _headerFormControllerMock;
      private Mock<ISelectedComponentController> _selectedComponentControllerMock;
      private Mock<IReportModeProperty> _reportModeProperty;

      [SetUp]
      public void Setup()
      {
         _pageLayoutForTest = new CommonPageLayout(new PageSizeInfo(PaperKind.Custom, 1000, 300), new Margin(MarginKind.Narrow, 0, 48, 0, 48));
         _textboxControllerMock = new Mock<ITextBoxController>();
         _headerFormControllerMock = new Mock<IHeaderFormController>();
         _sectionSelectionControllerMock = new Mock<ISelectedSectionController>();
         _imageControllerMock = new Mock<IImageController>();
         _selectedComponentControllerMock = new Mock<ISelectedComponentController>();
         _reportModeProperty = new Mock<IReportModeProperty>();

         _VMReportFooter = new VMReportBoundarySection(new Id<ReportHeader>(Guid.NewGuid()), 5, 1, _imageControllerMock.Object, _textboxControllerMock.Object, _headerFormControllerMock.Object, _selectedComponentControllerMock.Object, _sectionSelectionControllerMock.Object);
         _VMReportHeader = new VMReportBoundarySection(new Id<ReportHeader>(Guid.NewGuid()), 5, 1, _imageControllerMock.Object, _textboxControllerMock.Object, _headerFormControllerMock.Object, _selectedComponentControllerMock.Object, _sectionSelectionControllerMock.Object);
      }
      [Test]
      public void ConstructorShouldSetPorpertiesProperly()
      {
         //Arrange
         var startPage = new VMPage(_pageLayoutForTest, _reportModeProperty.Object, _VMReportHeader, _VMReportFooter);
         var endPage = new VMPage(_pageLayoutForTest, _reportModeProperty.Object, _VMReportHeader, _VMReportFooter);

         //Act
         var disabledData = new DisabledSpaceData(100, startPage, 140, endPage, 700, 60);

         //Assert
         Assert.AreEqual(100, disabledData.StartDomainY);
         Assert.AreEqual(startPage, disabledData.StartPage);
         Assert.AreEqual(140, disabledData.StartVisualY);
         Assert.AreEqual(endPage, disabledData.EndPage);
         Assert.AreEqual(700, disabledData.EndVisualY);
         Assert.AreEqual(60, disabledData.UsableSpaceTaken);
      }

      [Test]
      public void TestShouldBeAffectedByDisabledSpace()
      {
         //Arrange
         var startEndPage = new VMPage(_pageLayoutForTest, _reportModeProperty.Object, _VMReportHeader, _VMReportFooter);
         var disabledData = new DisabledSpaceData(100, startEndPage, 140, startEndPage, 700, 60);

         //Act
         var isPushed = disabledData.IsAffectedByDisabledSpace(800);

         //Assert
         Assert.IsTrue(isPushed);
      }

      [Test]
      public void TestShouldNotBeAffectedByDisabledSpace()
      {
         //Arrange
         var startEndPage = new VMPage(_pageLayoutForTest, _reportModeProperty.Object, _VMReportHeader, _VMReportFooter);
         var disabledData = new DisabledSpaceData(100, startEndPage, 140, startEndPage, 700, 60);

         //Act
         var isPushed = disabledData.IsAffectedByDisabledSpace(90);

         //Assert
         Assert.IsFalse(isPushed);
      }
   }
}
