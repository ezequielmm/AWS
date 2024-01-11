// <copyright file="HeaderFormControllerTest.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.ReportModule.ControllerIntegration.Test.ControllerTests.TestHelper;
using Mitutoyo.MiCAT.ReportModule.Domain.Components;
using Mitutoyo.MiCAT.ReportModule.Domain.Data;
using Mitutoyo.MiCAT.ReportModuleApp.Controllers;
using Mitutoyo.MiCAT.ReportModuleApp.Services.ReportComponents;
using NUnit.Framework;

namespace Mitutoyo.MiCAT.ReportModule.ControllerIntegration.Test.ControllerTests
{
   [TestFixture]
   [ExcludeFromCodeCoverage]
   public class HeaderFormControllerTest : BaseAppStateTest
   {
      private HeaderFormController _controller;

      [SetUp]
      public virtual void Setup()
      {
         SetUpHelper(AppStateReportComponentTestHelper.InitializeSnapShot);

         var reportComponentService = new ReportComponentService(new DomainSpaceService());
         _controller = new HeaderFormController(_history, reportComponentService);
      }

      [Test]
      public void HeaderFormController_AddHeaderFormToBody()
      {
         TestAddNewHeaderFormToReportSection(_history.CurrentSnapShot.GetReportBodyId());
      }

      [Test]
      public void HeaderFormController_AddNewHeaderFormToHeader()
      {
         TestAddNewHeaderFormToReportSection(_history.CurrentSnapShot.GetReportHeaderId());
      }

      [Test]
      public void HeaderFormController_AddNewHeaderFormToFooter()
      {
         TestAddNewHeaderFormToReportSection(_history.CurrentSnapShot.GetReportFooterId());
      }

      [Test]
      public void HeaderFormController_AddHeaderFormOnFakeSpace()
      {
         // Arrange

         // Act
         _controller.AddHeaderFormOnFakeSpace(10, 510, 500, 200);
         var headerForm1 = _history.CurrentSnapShot.GetItems<ReportHeaderForm>().First();

         _controller.AddHeaderFormOnFakeSpace(10, 610, 500, 200);
         var headerForm2 = _history.CurrentSnapShot.GetItems<ReportHeaderForm>().Where(i => i.Id != headerForm1.Id).First();

         var placement1 = _history.CurrentSnapShot.GetItem(headerForm1.Id).Placement;
         var placement2 = _history.CurrentSnapShot.GetItem(headerForm2.Id).Placement;

         // Assert
         Assert.AreEqual(10, placement1.X);
         Assert.AreEqual(710, placement1.Y);

         Assert.AreEqual(10, placement2.X);
         Assert.AreEqual(610, placement2.Y);
      }

      private void TestAddNewHeaderFormToReportSection(IItemId reportSectionId)
      {
         // Arrange
         int x = 10;
         int y = 20;

         // Act
         _controller.AddHeaderFormToSection(reportSectionId, x, y);

         // Assert
         var headerFormToTest = _history.CurrentSnapShot.GetItems<ReportHeaderForm>().Single();
         var selectionComponentToTest = _history.CurrentSnapShot.GetItems<ReportComponentSelection>().Single();
         var placementToTest = headerFormToTest.Placement;

         Assert.AreEqual(selectionComponentToTest.SelectedReportComponentIds.Single(), headerFormToTest.Id);
         Assert.AreEqual(1, headerFormToTest.RowIds.Count());
         Assert.AreEqual(reportSectionId, placementToTest.ReportSectionId);
         Assert.AreEqual(x, placementToTest.X);
         Assert.AreEqual(y, placementToTest.Y);
         Assert.AreEqual(340, placementToTest.Width);
         Assert.AreEqual(32, placementToTest.Height);
      }
   }
}
