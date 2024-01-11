// <copyright file="VMMarginSizeSettingsTest.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.ApplicationState.Controllers;
using Mitutoyo.MiCAT.Core.StdLib.Extensions;
using Mitutoyo.MiCAT.ReportModule.App.AppState;
using Mitutoyo.MiCAT.ReportModule.Domain;
using Mitutoyo.MiCAT.ReportModule.Domain.Data;
using Mitutoyo.MiCAT.ReportModule.GUI.Common.Filebar.MarginsSettings;
using Mitutoyo.MiCAT.ReportModule.GUI.Test.AppStateHelper;
using Mitutoyo.MiCAT.ReportModuleApp.Controllers;
using Moq;
using NUnit.Framework;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Test.Common.Filebar.MarginsSettings
{
   [ExcludeFromCodeCoverage]
   [TestFixture]
   public class VMMarginSizeSettingsTest : BaseAppStateTest
   {
      private Mock<ICommonPageLayoutController> _commonPageLayoutControllerMock;
      private Mock<IMarginSizeList> _marginSizeListMock;
      private Mock<IAppStateHistory> _historyMock;
      private VMMarginSizeSettings _vmMarginSizeSettings;

      public static ISnapShot BuildHelper(ISnapShot snapShot)
      {
         snapShot = snapShot.AddCollection<CommonPageLayout>(AppStateKinds.Undoable);
         snapShot = snapShot.AddCollection<ReportModeState>(AppStateKinds.Undoable);

         return snapShot;
      }

      [SetUp]
      public void Setup()
      {
         SetUpHelper(BuildHelper);
         _commonPageLayoutControllerMock = new Mock<ICommonPageLayoutController>();
         _marginSizeListMock = new Mock<IMarginSizeList>();
         _historyMock = new Mock<IAppStateHistory>();
         _historyMock.Setup(h => h.AddClient(It.IsAny<VMMarginSizeSettings>(), 0)).Returns(_historyMock.Object);

         var marginSizeList = new List<Margin>() { new Margin(MarginKind.Normal, 0, 96, 0, 96), new Margin(MarginKind.Narrow, 0, 48, 0, 48) };
         _marginSizeListMock.Setup(m => m.MarginSizeInfoList).Returns(marginSizeList);
         _vmMarginSizeSettings = new VMMarginSizeSettings(_commonPageLayoutControllerMock.Object, _marginSizeListMock.Object, _historyMock.Object);

         var newSnapShot = _history.NextSnapShot(ControllerCall.Empty);
         var commonPageLayout = new CommonPageLayout(new PageSizeInfo(), new Margin(MarginKind.Narrow, 0, 48, 0, 48));
         newSnapShot = newSnapShot.AddItem(commonPageLayout);
         newSnapShot = newSnapShot.AddItem(new ReportModeState(false));
         _history.AddSnapShot(newSnapShot);
      }

      [Test]
      public void CallDisposeShouldCallUnsuscribe()
      {
         // Act
         _vmMarginSizeSettings.Dispose();

         // Assert
         _historyMock.Verify(c => c.RemoveClient(It.IsAny<ISubscriptionClient>()), Times.Once);
      }

      [Test]
      public void PopulateItemsShouldHaveDisplayNameAndDescription()
      {
         // Assert
         _vmMarginSizeSettings.MarginSizeItems.ForEach(i =>
         {
            Assert.IsNotNull(i.DisplayName);
            Assert.IsNotNull(i.MarginDescription);
         });
      }

      [Test]
      public void PopulateMarginSizeList()
      {
         // Assert
         Assert.AreEqual(2, _vmMarginSizeSettings.MarginSizeItems.Count);
      }

      [Test]
      public void MarginUpdateShouldRefreshMarginSizeList()
      {
         // Arrange
         var snapShot = _history.CurrentSnapShot;
         var nextSnapShot = _history.NextSnapShot(ControllerCall.Empty);
         var commonPageLayout = snapShot.GetItems<CommonPageLayout>().First();
         nextSnapShot = nextSnapShot.UpdateItem<CommonPageLayout>(commonPageLayout, commonPageLayout.With(new Margin(MarginKind.Normal, 0, 96, 0, 96)));
         _history.AddSnapShot(nextSnapShot);

         // Act
         _vmMarginSizeSettings.Update(nextSnapShot);

         // Assert
         var selected = _vmMarginSizeSettings.MarginSizeItems.Where(x => x.IsChecked);
         Assert.AreEqual(MarginKind.Normal, selected.First().Margin.MarginKind);
      }

      [Test]
      public void ReportModeUpdateShouldRefreshMarginSizeListEnabled()
      {
         // Arrange
         var snapShot = _history.CurrentSnapShot;
         var nextSnapShot = _history.NextSnapShot(ControllerCall.Empty);
         var reportModeState = snapShot.GetItems<ReportModeState>().First();
         nextSnapShot = nextSnapShot.UpdateItem<ReportModeState>(reportModeState, reportModeState.With(true));
         _history.AddSnapShot(nextSnapShot);
         var prevCount = _vmMarginSizeSettings.MarginSizeItems.Where(x => x.IsEnabled == false);

         // Act
         _vmMarginSizeSettings.Update(nextSnapShot);

         // Assert
         var enabled = _vmMarginSizeSettings.MarginSizeItems.Where(x => x.IsEnabled == true);
         Assert.AreNotEqual(prevCount.Count(), enabled.Count());
      }
   }
}
