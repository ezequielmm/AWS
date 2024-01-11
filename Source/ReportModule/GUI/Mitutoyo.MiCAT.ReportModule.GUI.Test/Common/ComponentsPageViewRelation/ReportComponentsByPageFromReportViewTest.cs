// <copyright file="ReportComponentsByPageFromReportViewTest.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using Mitutoyo.MiCAT.ReportModule.GUI.Common.ComponentsPageViewRelation;
using Mitutoyo.MiCAT.ReportModule.GUI.MainWindow;
using Mitutoyo.MiCAT.ReportModule.GUI.PageLayout;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.Containers.InteractiveControlContainer;
using Moq;
using NUnit.Framework;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Test.Common.ComponentsPageViewRelation
{
   [ExcludeFromCodeCoverage]
   [TestFixture]
   public class ReportComponentsByPageFromReportViewTest
   {
      private Mock<IReportComponentsByPageArranger> _reportComponentsByPageArrangerMock;
      private Mock<IReportComponentsFromReportView> _reportComponentsGetterMock;
      private Mock<IPageViewsFromReportView> _pageViewsGetterMock;
      private ReportView _reportView;
      private List<PageView> _pages;
      private List<InteractiveControlContainer> _controls;

      [SetUp]
      public void Setup()
      {
         _reportComponentsByPageArrangerMock = new Mock<IReportComponentsByPageArranger>();
         _reportComponentsGetterMock = new Mock<IReportComponentsFromReportView>();
         _pageViewsGetterMock = new Mock<IPageViewsFromReportView>();
         _reportView = new ReportView();
         _pages = new List<PageView>();
         _controls = new List<InteractiveControlContainer>();

         _reportComponentsGetterMock.Setup(c => c.GetReportComponents(_reportView)).Returns(_controls);
         _pageViewsGetterMock.Setup(p => p.GetPageViews(_reportView)).Returns(_pages);
      }

      [Test]
      [Apartment(ApartmentState.STA)]
      public void GetReportComponentsByPageTest()
      {
         //Arrange

         ReportComponentsByPageFromReportView sut = new ReportComponentsByPageFromReportView(
                                                            _reportComponentsByPageArrangerMock.Object,
                                                            _reportComponentsGetterMock.Object,
                                                            _pageViewsGetterMock.Object);

         //Act
         sut.GetReportComponentsByPage(_reportView);

         //Assert
         _reportComponentsGetterMock.Verify(x => x.GetReportComponents(_reportView), Times.Once);
         _pageViewsGetterMock.Verify(x => x.GetPageViews(_reportView), Times.Once);
         _reportComponentsByPageArrangerMock.Verify(x => x.ArrangeReportComponentsByPage(_pages, _controls), Times.Once);
      }
   }
}
