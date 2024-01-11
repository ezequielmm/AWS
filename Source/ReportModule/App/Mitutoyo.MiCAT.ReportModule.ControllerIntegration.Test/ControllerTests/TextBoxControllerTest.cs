// <copyright file="TextBoxControllerTest.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.ApplicationState.Controllers;
using Mitutoyo.MiCAT.ReportModule.ControllerIntegration.Test.ControllerTests.TestHelper;
using Mitutoyo.MiCAT.ReportModule.Domain.Components;
using Mitutoyo.MiCAT.ReportModule.Domain.Data;
using Mitutoyo.MiCAT.ReportModuleApp.Controllers;
using Mitutoyo.MiCAT.ReportModuleApp.Services.ReportComponents;
using NUnit.Framework;

namespace Mitutoyo.MiCAT.ReportModule.ControllerIntegration.Test.ControllerTests
{
   [ExcludeFromCodeCoverage]
   [TestFixture]
   public class TextBoxControllerTest : BaseAppStateTest
   {
      private TextBoxController _controller;

      public static ISnapShot BuildHelper(ISnapShot snapShot)
      {
         snapShot = AppStateReportComponentTestHelper.InitializeSnapShot(snapShot);
         snapShot = snapShot.AddCollection<ReportTextBox>(AppStateKinds.Undoable);

         return snapShot;
      }

      [SetUp]
      public virtual void Setup()
      {
         SetUpHelper(BuildHelper);

         var reportComponentService = new ReportComponentService(new DomainSpaceService());
         _controller = new TextBoxController(_history, reportComponentService);
      }

      [Test]
      public void TextBoxController_ModifyTextToTextBox()
      {
         // Arrange
         var placement = new ReportComponentPlacement(_history.CurrentSnapShot.GetReportBodyId(), 1, 2, 3, 4);
         var textbox = new ReportTextBox(placement, "text");

         var snapshot = _history.NextSnapShot(ControllerCall.Empty);
         snapshot = snapshot.AddItem(textbox);

         _history.AddSnapShot(snapshot);

         // Act
         var newText = "New Text";
         _controller.ModifyText(textbox.Id, newText);

         // Assert
         var actualTextbox = _history.CurrentSnapShot.GetItems<ReportTextBox>().Single();

         Assert.AreEqual(_history.CurrentSnapShot.GetItems<ReportTextBox>().Count(), 1);
         Assert.AreEqual(newText, actualTextbox.Text);
      }

      [Test]
      public void TextBoxController_AddANewTextBoxToBody()
      {
         // Arrange
         var oldTextbox = ArrangeOldTextbox(_history.CurrentSnapShot.GetReportHeaderId());
         var sectionIdToAdd = _history.CurrentSnapShot.GetReportBodyId();

         // Act
         _controller.AddTextboxToBody(10, 20);

         // Assert
         AssertNewTextBox(oldTextbox.Id, new ReportComponentPlacement(sectionIdToAdd, 10, 20, 100, 25), string.Empty);
      }

      [Test]
      public void TextBoxController_AddNewTextboxToHeader()
      {
         // Arrange
         var oldTextbox = ArrangeOldTextbox(_history.CurrentSnapShot.GetReportFooterId());
         var sectionIdToAdd = _history.CurrentSnapShot.GetReportHeaderId();

         // Act
         _controller.AddTextboxToSection(sectionIdToAdd, 10, 20);

         // Assert
         AssertNewTextBox(oldTextbox.Id, new ReportComponentPlacement(sectionIdToAdd, 10, 20, 100, 25), string.Empty);
      }

      [Test]
      public void TextBoxController_AddNewTextboxToFooter()
      {
         // Arrange
         var oldTextbox = ArrangeOldTextbox(_history.CurrentSnapShot.GetReportBodyId());
         var sectionIdToAdd = _history.CurrentSnapShot.GetReportFooterId();

         // Act
         _controller.AddTextboxToSection(sectionIdToAdd, 10, 20);

         // Assert
         AssertNewTextBox(oldTextbox.Id, new ReportComponentPlacement(sectionIdToAdd, 10, 20, 100, 25), string.Empty);
      }

      private ReportTextBox ArrangeOldTextbox(IItemId reportSection)
      {
         var oldPlacement = new ReportComponentPlacement(reportSection, 1, 2, 3, 4);
         var oldTextbox = new ReportTextBox(oldPlacement);

         var snapshot = _history.NextSnapShot(ControllerCall.Empty);
         snapshot = snapshot.AddItem(oldTextbox);

         _history.AddSnapShot(snapshot);

         return oldTextbox;
      }

      private void AssertNewTextBox(Id<ReportTextBox> oldTextBoxId, ReportComponentPlacement expectedPlacement, string expectedText)
      {
         var textboxes = _history.CurrentSnapShot.GetItems<ReportTextBox>();
         var newTextbox = textboxes.Single(t => t.Id != oldTextBoxId);
         var actualPlacement = _history.CurrentSnapShot.GetItem(newTextbox.Id).Placement;
         var actualSelectedComponent = _history.CurrentSnapShot.GetItems<ReportComponentSelection>().Single();

         Assert.AreEqual(2, textboxes.Count());
         Assert.AreEqual(expectedText, newTextbox.Text);
         Assert.AreEqual(newTextbox.Id, actualSelectedComponent.SelectedReportComponentIds.Single());
         Assert.AreEqual(expectedPlacement.ReportSectionId, actualPlacement.ReportSectionId);
         Assert.AreEqual(expectedPlacement.X, actualPlacement.X);
         Assert.AreEqual(expectedPlacement.Y, actualPlacement.Y);
         Assert.AreEqual(expectedPlacement.Width, actualPlacement.Width);
         Assert.AreEqual(expectedPlacement.Height, actualPlacement.Height);
      }
   }
}