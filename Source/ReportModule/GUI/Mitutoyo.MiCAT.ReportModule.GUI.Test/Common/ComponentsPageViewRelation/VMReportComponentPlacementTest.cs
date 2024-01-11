// <copyright file="VMReportComponentPlacementTest.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.ReportModule.App.AppState;
using Mitutoyo.MiCAT.ReportModule.Domain.Components;
using Mitutoyo.MiCAT.ReportModule.Domain.Components.ReportSections;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements;
using Mitutoyo.MiCAT.ReportModuleApp.Controllers;
using Moq;
using NUnit.Framework;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Test.Common.ComponentsPageViewRelation
{
   [TestFixture]
   [ExcludeFromCodeCoverage]
   public class VMReportComponentPlacementTest
   {
      private Mock<IAppStateHistory> _historyMock;
      private Mock<IReportComponentPlacementController> _placementMock;
      private Mock<ISelectedComponentController> _selectedComponentMock;
      private Mock<ISnapShot> _snapShotMock;

      [SetUp]
      public void Setup()
      {
         _snapShotMock = new Mock<ISnapShot>();

         _snapShotMock.Setup(s => s.GetItems<ReportModeState>()).Returns(new List<ReportModeState>() { new ReportModeState(false) });

         _historyMock = new Mock<IAppStateHistory>();
         _historyMock.Setup(h => h.CurrentSnapShot).Returns(_snapShotMock.Object);
         _historyMock.Setup(a => a.AddClient(It.IsAny<object>(), It.IsAny<int>())).Returns(_historyMock.Object);

         _selectedComponentMock = new Mock<ISelectedComponentController>();

         _placementMock = new Mock<IReportComponentPlacementController>();
      }

      [Test]
      [TestCase(1, 30, 100, 50)]
      [TestCase(57, 1, 30, 200)]
      [TestCase(100, 12, 10, 10)]
      public void ResetVisuals_ShouldReset_Visuals(int x, int y, int width, int height)
      {
         //Arrange
         int delta = 10;

         var placement = new ReportComponentPlacement(new ReportBody().Id, x, y, width, height);

         var sut = new VMReportElementWithPublicSetters(new ReportImage(placement).Id, placement, _placementMock.Object, _selectedComponentMock.Object)
         {
            DomainX = x,
            DomainY = y,
            DomainHeight = height,
            DomainWidth = width,
            VisualX = x + delta,
            VisualY = y + delta,
            VisualWidth = width + delta,
            VisualHeight = height + delta,
         };

         //Act
         sut.CallToResetVisuals();

         Assert.AreEqual(x, sut.DomainX);
         Assert.AreEqual(y, sut.DomainY);
         Assert.AreEqual(width, sut.DomainWidth);
         Assert.AreEqual(height, sut.DomainHeight);
         Assert.AreEqual(sut.DomainX, sut.VisualX);
         Assert.AreEqual(sut.DomainY, sut.VisualY);
         Assert.AreEqual(sut.DomainWidth, sut.VisualWidth);
         Assert.AreEqual(sut.DomainHeight, sut.VisualHeight);
      }

      private class VMReportElementWithPublicSetters : VMReportComponentPlacement
      {
         public VMReportElementWithPublicSetters(IItemId<IReportComponent> reportComponentId, ReportComponentPlacement reportPlacement, IReportComponentPlacementController placementController, ISelectedComponentController selectedComponentController)
            : base(reportComponentId, reportPlacement, placementController, selectedComponentController)
         {
         }

         public new int DomainX
         {
            get => base.DomainX;
            set => base.DomainX = value;
         }

         public new int DomainY
         {
            get => base.DomainY;
            set => base.DomainY = value;
         }

         public new int DomainWidth
         {
            get => base.DomainWidth;
            set => base.DomainWidth = value;
         }

         public new int DomainHeight
         {
            get => base.DomainHeight;
            set => base.DomainHeight = value;
         }

         public new int VisualX
         {
            get => base.VisualX;
            set => base.VisualX = value;
         }

         public new int VisualY
         {
            get => base.VisualY;
            set => base.VisualY = value;
         }

         public new int VisualWidth
         {
            get => base.VisualWidth;
            set => base.VisualWidth = value;
         }

         public new int VisualHeight
         {
            get => base.VisualHeight;
            set => base.VisualHeight = value;
         }

         public void CallToResetVisuals()
         {
            ResetVisuals();
         }
      }
   }
}
