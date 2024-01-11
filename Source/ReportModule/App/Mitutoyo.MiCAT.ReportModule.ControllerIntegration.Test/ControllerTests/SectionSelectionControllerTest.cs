// <copyright file="SectionSelectionControllerTest.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Mitutoyo.MiCAT.ReportModule.ControllerIntegration.Test.ControllerTests.TestHelper;
using Mitutoyo.MiCAT.ReportModule.Domain.Data;
using Mitutoyo.MiCAT.ReportModuleApp.Controllers;
using NUnit.Framework;

namespace Mitutoyo.MiCAT.ReportModule.ControllerIntegration.Test.ControllerTests
{
   [ExcludeFromCodeCoverage]
   [TestFixture]
   public class SectionSelectionControllerTest : BaseAppStateTest
   {
      private SelectedSectionController _controller;

      [SetUp]
      public virtual void Setup()
      {
         SetUpHelper(AppStateReportComponentTestHelper.InitializeSnapShot);

         _controller = new SelectedSectionController(_history);
      }

      [Test]
      public void SelectPageSectionTest()
      {
         // Act
         _controller.SelectPageBodySection();

         // Assert
         var newSectionSelectionState = GetReportSectionSelectionFromCurrentSnapShot();

         Assert.AreEqual(1, newSectionSelectionState.SelectedSectionIds.Count);
         Assert.AreEqual(_history.CurrentSnapShot.GetReportBodyId(), newSectionSelectionState.SelectedSectionIds[0]);
      }

      [Test]
      public void SelectReportBoundarySectionsTest()
      {
         // Act
         _controller.SelectReportBoundarySections();

         // Assert
         var newSectionSelectionState = GetReportSectionSelectionFromCurrentSnapShot();

         Assert.AreEqual(2, newSectionSelectionState.SelectedSectionIds.Count);

         CollectionAssert.Contains(newSectionSelectionState.SelectedSectionIds, _history.CurrentSnapShot.GetReportHeaderId());
         CollectionAssert.Contains(newSectionSelectionState.SelectedSectionIds, _history.CurrentSnapShot.GetReportFooterId());
      }

      private ReportSectionSelection GetReportSectionSelectionFromCurrentSnapShot()
      {
         return _history.CurrentSnapShot.GetItems<ReportSectionSelection>().Single();
      }
   }
}
