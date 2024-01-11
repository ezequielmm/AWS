// <copyright file="CommonPageLayoutControllerIntegrationTest.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using System.Drawing.Printing;
using System.Linq;
using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.ApplicationState.Controllers;
using Mitutoyo.MiCAT.ReportModule.ControllerIntegration.Test.ControllerTests;
using Mitutoyo.MiCAT.ReportModule.Domain;
using Mitutoyo.MiCAT.ReportModule.Domain.Data;
using Mitutoyo.MiCAT.ReportModuleApp.Controllers;
using NUnit.Framework;

namespace Mitutoyo.MiCAT.ReportModule.ControllerIntegration.Test
{
   [ExcludeFromCodeCoverage]
   [TestFixture]
   public class CommonPageLayoutControllerIntegrationTest : BaseAppStateTest
   {
      private ICommonPageLayoutController _controller;
      private PageSizeList _pageSizeList = new PageSizeList();

      public static ISnapShot BuildHelper(ISnapShot snapShot)
      {
         snapShot = snapShot.AddCollection<CommonPageLayout>(AppStateKinds.Undoable);
         return snapShot;
      }

      [SetUp]
      public void Setup()
      {
         SetUpHelper(BuildHelper);

         _controller = new CommonPageLayoutController(_history, _pageSizeList);
      }

      [Test]
      public void PageSizeChange()
      {
         // Arrange
         var newPageSizeInfo = new PageSizeInfo(PaperKind.A4, 100, 500);
         CommonPageLayout commonPageLayout = new CommonPageLayout(new PageSizeInfo(PaperKind.Letter, 0, 0), new Margin(), new HeaderData(0), new FooterData(0));

         var snapshot = _history.NextSnapShot(ControllerCall.Empty);
         snapshot = snapshot.AddItem(commonPageLayout);
         _history.AddSnapShot(snapshot);

         // Act
         _controller.SetPageSize(newPageSizeInfo);

         // Assert
         var commonPageLayoutToTest = _history.CurrentSnapShot.GetItems<CommonPageLayout>().Single();
         Assert.AreEqual(commonPageLayoutToTest.PageSize.PaperKind, newPageSizeInfo.PaperKind);
         Assert.AreEqual(commonPageLayoutToTest.PageSize.Width, newPageSizeInfo.Width);
         Assert.AreEqual(commonPageLayoutToTest.PageSize.Height, newPageSizeInfo.Height);
      }

      [Test]
      public void GetCurrentCommonPageLayout()
      {
         // Arrange
         CommonPageLayout commonPageLayout = new CommonPageLayout(new PageSizeInfo(PaperKind.Letter, 0, 0), new Margin(), new HeaderData(0), new FooterData(0));

         var snapshot = _history.NextSnapShot(ControllerCall.Empty);
         snapshot = snapshot.AddItem(commonPageLayout);
         _history.AddSnapShot(snapshot);

         // Act
         var currentCommonPageLayout = _controller.GetCurrentCommonPageLayout();

         // Assert
         Assert.AreEqual(currentCommonPageLayout.PageSize.PaperKind, commonPageLayout.PageSize.PaperKind);
         Assert.AreEqual(currentCommonPageLayout.PageSize.Width, commonPageLayout.PageSize.Width);
         Assert.AreEqual(currentCommonPageLayout.PageSize.Height, commonPageLayout.PageSize.Height);
      }

      [Test]
      public void GetSupportedPageSizeListShouldBeTheSameThatPageSizeList()
      {
         var supportedPageSizeList = _controller.GetSupportedPageSizeList();
         Assert.AreEqual(supportedPageSizeList, _pageSizeList);
      }

      // Controller Integration tests can take time to run so they may test many different things
      // and may not even have "Should" in the name
      [Test]
      public void SetPageSizeA4_ShouldSetAppStateAndNotifyApplicationStateClient()
      {
         // Arrange
         var pageSizeList = new PageSizeList();

         _history.Run(snapshot=>snapshot.AddItem(
            new CommonPageLayout(new PageSizeInfo(PaperKind.Letter, 10, 20),
               new Margin(MarginKind.Narrow, 0, 48, 0, 48), new HeaderData(100), new FooterData(100))));

         var commonPageLayoutController = new CommonPageLayoutController(_history, pageSizeList);
         var pageSizeInfo = pageSizeList.FindPageSize(PaperKind.A4);

         // act
         commonPageLayoutController.SetPageSize(pageSizeInfo);

         // assert
         Assert.AreEqual(PaperKind.A4, _history.CurrentSnapShot.GetItems<CommonPageLayout>().SingleOrDefault().PageSize.PaperKind);
      }

      [Test]
      public void WhenMarginSizeChangeTheChangesShouldBeNotifier()
      {
         // Arrange
         var newMarginSizeInfo = new Margin(MarginKind.Narrow, 0, 48, 0, 48);
         CommonPageLayout commonPageLayout = new CommonPageLayout(new PageSizeInfo(PaperKind.Letter, 0, 0), new Margin(), new HeaderData(0), new FooterData(0));

         var snapshot = _history.NextSnapShot(ControllerCall.Empty);
         snapshot = snapshot.AddItem(commonPageLayout);
         _history.AddSnapShot(snapshot);

         // Act
         _controller.SetMarginSize(newMarginSizeInfo);

         // Assert
         var commonPageLayoutToTest = _history.CurrentSnapShot.GetItems<CommonPageLayout>().Single();
         Assert.AreEqual(commonPageLayoutToTest.CanvasMargin.MarginKind, newMarginSizeInfo.MarginKind);
         Assert.AreEqual(commonPageLayoutToTest.CanvasMargin.Top, newMarginSizeInfo.Top);
         Assert.AreEqual(commonPageLayoutToTest.CanvasMargin.Bottom, newMarginSizeInfo.Bottom);
      }

      [Test]
      public void PageLayoutController_ShouldGetSamePageSizeListAfterConstructorExecuted()
      {
         // arrange
         var pageSizeList = new PageSizeList();

         // act
         var commonPageLayoutController = new CommonPageLayoutController(_history, pageSizeList);

         // assert
         Assert.AreSame(pageSizeList, commonPageLayoutController.GetSupportedPageSizeList());
      }
   }
}
