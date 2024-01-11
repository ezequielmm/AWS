// <copyright file="ReportComponentsByPageArrangerTest.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Drawing.Printing;
using System.Threading;
using Mitutoyo.MiCAT.ReportModule.Domain;
using Mitutoyo.MiCAT.ReportModule.Domain.Components.ReportSections;
using Mitutoyo.MiCAT.ReportModule.Domain.Data;
using Mitutoyo.MiCAT.ReportModule.GUI.Common;
using Mitutoyo.MiCAT.ReportModule.GUI.Common.ComponentsPageViewRelation;
using Mitutoyo.MiCAT.ReportModule.GUI.PageLayout;
using Mitutoyo.MiCAT.ReportModule.GUI.ReportBoundarySection;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.Containers.InteractiveControlContainer;
using Mitutoyo.MiCAT.ReportModuleApp.Controllers;
using Moq;
using NUnit.Framework;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Test.Common.ComponentsPageViewRelation
{
   [ExcludeFromCodeCoverage]
   [TestFixture]
   public class ReportComponentsByPageArrangerTest
   {
      [Test]
      public void ShouldReturnEmptyList()
      {
         var sut = new ReportComponentsByPageArranger();

         var result = sut.ArrangeReportComponentsByPage(new List<PageView>(), new List<InteractiveControlContainer>());

         Assert.AreEqual(0, result.Count);
      }

      [Test]
      [Apartment(ApartmentState.STA)]
      public void ShouldReturnTwoPagesWithTwoComponents()
      {
         //Arrange
         var page1 = CreatePage(10);
         var page2 = CreatePage(210);

         var reportComponent1 = CreateInteractiveControlContainer(20, 10, 30);
         var reportComponent2 = CreateInteractiveControlContainer(120, 90, 30);
         var reportComponent3 = CreateInteractiveControlContainer(220, 20, 40);
         var reportComponent4 = CreateInteractiveControlContainer(320, 70, 40);

         var sut = new ReportComponentsByPageArranger();

         //Act
         var result = sut.ArrangeReportComponentsByPage(new[] { page1, page2 }, new[] { reportComponent1, reportComponent2, reportComponent3, reportComponent4 });

         //Assert
         Assert.AreEqual(2, result.Count);

         Assert.AreEqual(2, result[0].Components.Count);
         Assert.AreEqual(2, result[1].Components.Count);

         Assert.AreEqual(result[0].Page, page1);
         Assert.AreEqual(result[1].Page, page2);

         Assert.AreEqual(result[0].Components[0], reportComponent1);
         Assert.AreEqual(result[0].Components[1], reportComponent2);

         Assert.AreEqual(result[1].Components[0], reportComponent3);
         Assert.AreEqual(result[1].Components[1], reportComponent4);
      }

      private InteractiveControlContainer CreateInteractiveControlContainer(int visualY, int visualX, int visualWidth)
      {
         var vm = CreateVmVisualElement(visualY, visualX, visualWidth);
         return new InteractiveControlContainer { DataContext = vm };
      }

      private IVMVisualElement CreateVmVisualElement(int visualY, int visualX, int visualWidth)
      {
         var vmVisualElementMock = new Mock<IVMVisualElement>();
         vmVisualElementMock.SetupGet(e => e.VMPlacement.VisualY).Returns(visualY);
         vmVisualElementMock.SetupGet(e => e.VMPlacement.VisualX).Returns(visualX);
         vmVisualElementMock.SetupGet(e => e.VMPlacement.VisualWidth).Returns(visualWidth);

         return vmVisualElementMock.Object;
      }

      private PageView CreatePage(int startVisualY)
      {
         var vmPage = CreateVMPage(startVisualY);
         return new PageView { DataContext = vmPage };
      }

      private VMPage CreateVMPage(int startVisualY)
      {
         var imageControllerMock = new Mock<IImageController>();
         var textboxControllerMock = new Mock<ITextBoxController>();
         var headerFormControllerMock = new Mock<IHeaderFormController>();
         var selectedComponentControllerMock = new Mock<ISelectedComponentController>();
         var selectedSectionControllerMock = new Mock<ISelectedSectionController>();
         var reportModePropertyMock = new Mock<IReportModeProperty>();
         var currentPageLayout = new CommonPageLayout(new PageSizeInfo(PaperKind.Custom, 200, 100), new Margin(MarginKind.Normal, 10, 11, 12, 13));

         var vmReportHeader = new VMReportBoundarySection(new ReportHeader().Id, 10, 20, imageControllerMock.Object, textboxControllerMock.Object, headerFormControllerMock.Object, selectedComponentControllerMock.Object, selectedSectionControllerMock.Object);
         var vmReportFooter = new VMReportBoundarySection(new ReportFooter().Id, 30, 40, imageControllerMock.Object, textboxControllerMock.Object, headerFormControllerMock.Object, selectedComponentControllerMock.Object, selectedSectionControllerMock.Object);

         return new VMPage(currentPageLayout, reportModePropertyMock.Object, vmReportHeader, vmReportFooter)
         {
            StartVisualY = startVisualY
         };
      }
   }
}
