// <copyright file="VMPageSizeSettingsTest.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Drawing.Printing;
using System.Linq;
using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.ReportModule.App.AppState;
using Mitutoyo.MiCAT.ReportModule.Domain;
using Mitutoyo.MiCAT.ReportModule.Domain.Data;
using Mitutoyo.MiCAT.ReportModule.GUI.Common.BusyIndicators;
using Mitutoyo.MiCAT.ReportModule.GUI.Common.Filebar;
using Mitutoyo.MiCAT.ReportModuleApp.Controllers;
using Mitutoyo.MiCAT.ReportModuleApp.Utilities;
using Moq;
using NUnit.Framework;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Test.Common.Filebar
{
   [ExcludeFromCodeCoverage]
   public class VMPageSizeSettingsTest
   {
      private Mock<ICommonPageLayoutController> _commonPageLayoutControllerMock;
      private Mock<IPageSizeList> _pageSizeListMock;
      private Mock<IAppStateHistory> _historyMock;
      private Mock<IBusyIndicator> _busyIndicatorMock;
      private IActionCaller _actionCaller;

      private PageSizeInfo pageSizeInfoA4 = new PageSizeInfo(PaperKind.A4, 100, 200);
      private PageSizeInfo pageSizeInfoA3 = new PageSizeInfo(PaperKind.A3, 300, 400);
      private PageSizeInfo pageSizeInfoLetter = new PageSizeInfo(PaperKind.Letter, 500, 600);

      [SetUp]
      public void Setup()
      {
         _commonPageLayoutControllerMock = new Mock<ICommonPageLayoutController>();
         _pageSizeListMock = new Mock<IPageSizeList>();
         _historyMock = new Mock<IAppStateHistory>();
         _historyMock.Setup(h => h.AddClient(It.IsAny<VMPageSizeSettings>(), 0)).Returns(_historyMock.Object);
         _busyIndicatorMock = new Mock<IBusyIndicator>();

         _busyIndicatorMock.Setup(b => b.SetIsBusyTrueUntilUIIsIdle()).Callback(() => { });
         _actionCaller = new BusyIndicatorActionCaller(_busyIndicatorMock.Object);

         _pageSizeListMock
            .Setup(x => x.PageSizeInfoList)
            .Returns(new[] { pageSizeInfoA3, pageSizeInfoA4, pageSizeInfoLetter, });
      }

      [Test]
      public void Init_ShouldSubscribeToAppState()
      {
         // Arrange
         var vm = new VMPageSizeSettings(
            _commonPageLayoutControllerMock.Object,
            _pageSizeListMock.Object,
            _historyMock.Object);

         // Assert
         _historyMock.Verify(x => x.Subscribe(It.IsAny<Subscription>()), Times.AtLeastOnce);
         Assert.IsNotNull(vm.Name);
      }

      [Test]
      public void Init_ShouldPopulatePageSizeItems()
      {
         // Arrange
         var vm = new VMPageSizeSettings(
            _commonPageLayoutControllerMock.Object,
            _pageSizeListMock.Object,
            _historyMock.Object);

         // Assert
         Assert.IsNotNull(vm.PageSizeItems);
         Assert.IsNotEmpty(vm.PageSizeItems);
         Assert.AreEqual(3, vm.PageSizeItems.Count);
      }

      [Test]
      public void Dispose_ShouldUnsubscribeToAppState()
      {
         // Arrange
         var vm = new VMPageSizeSettings(
            _commonPageLayoutControllerMock.Object,
            _pageSizeListMock.Object,
            _historyMock.Object);

         // Assert
         vm.Dispose();

         // Assert
         _historyMock.Verify(x => x.RemoveClient(It.IsAny<ISubscriptionClient>()), Times.AtLeastOnce);
      }

      [Test]
      public void UpdateSelectedPageSizeFromSnapShot_ShouldUpdateSelectedPageSizeItem()
      {
         // Arrange
         var vm = new VMPageSizeSettings(
           _commonPageLayoutControllerMock.Object,
           _pageSizeListMock.Object,
           _historyMock.Object);

         var snapShotMock = new Mock<ISnapShot>();
         snapShotMock
            .Setup(x => x.GetChanges())
            .Returns(
               new Changes(new IItemChange[] { new UpdateItemChange(new Id<CommonPageLayout>(Guid.NewGuid())) }.ToImmutableList()));

         snapShotMock
            .Setup(x => x.GetItems<CommonPageLayout>())
            .Returns(new[] { new CommonPageLayout(pageSizeInfoLetter, new Margin(), new HeaderData(100), new FooterData(100)), });

         // Act
         vm.Update(snapShotMock.Object);

         var selectedItem = vm.PageSizeItems.FirstOrDefault(x => x.IsChecked);

         // Assert
         Assert.IsNotNull(selectedItem);
         Assert.AreEqual(pageSizeInfoLetter, selectedItem.PageSizeInfo);
      }

      [Test]
      public void UpdateIsEnabledFromSnapShot_ShouldDisableAllItemsWhenReportIsOnViewMode()
      {
         // Arrange
         var vm = new VMPageSizeSettings(
           _commonPageLayoutControllerMock.Object,
           _pageSizeListMock.Object,
           _historyMock.Object);

         var snapShotMock = new Mock<ISnapShot>();
         snapShotMock
            .Setup(x => x.GetChanges())
            .Returns(
               new Changes(new IItemChange[] { new UpdateItemChange(new Id<ReportModeState>(Guid.NewGuid())) }.ToImmutableList()));

         snapShotMock
            .Setup(x => x.GetItems<ReportModeState>())
            .Returns(new[] { new ReportModeState(false), });

         // Act
         vm.Update(snapShotMock.Object);

         // Assert
         Assert.IsTrue(vm.PageSizeItems.All(x => !x.IsEnabled));
      }

      [Test]
      public void UpdateIsEnabledFromSnapShot_ShouldEnableAllItemsWhenReportIsOnEditMode()
      {
         // Arrange
         var vm = new VMPageSizeSettings(
           _commonPageLayoutControllerMock.Object,
           _pageSizeListMock.Object,
           _historyMock.Object);

         var snapShotMock = new Mock<ISnapShot>();
         snapShotMock
            .Setup(x => x.GetChanges())
            .Returns(
               new Changes(new IItemChange[] { new UpdateItemChange(new Id<ReportModeState>(Guid.NewGuid())) }.ToImmutableList()));

         snapShotMock
            .Setup(x => x.GetItems<ReportModeState>())
            .Returns(new[] { new ReportModeState(true), });

         // Act
         vm.Update(snapShotMock.Object);

         // Assert
         Assert.IsTrue(vm.PageSizeItems.All(x => x.IsEnabled));
      }

      [Test]
      public void OnUpdatePageSizeCommand_ShouldCallController()
      {
         // Arrange
         var vm = new VMPageSizeSettings(
           _commonPageLayoutControllerMock.Object,
           _pageSizeListMock.Object,
           _historyMock.Object);

         var pageSizeInfo = new PageSizeInfo();

         // Act
         vm.UpdatePageSizeCommand.Execute(pageSizeInfo);

         // Assert
         _commonPageLayoutControllerMock
            .Verify(x => x.SetPageSize(It.Is<PageSizeInfo>(i => i == pageSizeInfo)), Times.Once);
      }
   }
}
