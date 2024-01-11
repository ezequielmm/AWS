// <copyright file="VMReportBoundarySectionComponentFactoryTest.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.ApplicationState.Controllers;
using Mitutoyo.MiCAT.ReportModule.App.AppState;
using Mitutoyo.MiCAT.ReportModule.Domain.Components;
using Mitutoyo.MiCAT.ReportModule.Domain.Components.ReportSections;
using Mitutoyo.MiCAT.ReportModule.Domain.Data;
using Mitutoyo.MiCAT.ReportModule.GUI.Common.BusyIndicators;
using Mitutoyo.MiCAT.ReportModule.GUI.Test.AppStateHelper;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements.HeaderForm.ViewModels;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements.Image.ViewModels;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements.TextBox.ViewModels;
using Mitutoyo.MiCAT.ReportModuleApp.AppState;
using Mitutoyo.MiCAT.ReportModuleApp.Controllers;
using Mitutoyo.MiCAT.ReportModuleApp.Utilities;
using Moq;
using NUnit.Framework;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Test.VisualElements.ReportElements
{
   [ExcludeFromCodeCoverage]
   [TestFixture]
   public class VMReportBoundarySectionComponentFactoryTest : BaseAppStateTest
   {
      private Mock<IReportComponentPlacementController> _reportComponentPlacementControllerMock;
      private Mock<IImageController> _reportBoundarySectionImageControllerMock;
      private Mock<ITextBoxController> _reportBoundarySectionTextboxControllerMock;
      private Mock<IHeaderFormController> _reportBoundarySectionHeaderFormControllerMock;
      private Mock<IHeaderFormFieldController> _headerFormFieldControllerMock;
      private Mock<ISelectedComponentController> _selectedComponentControllerMock;
      private Mock<IDeleteComponentController> _deleteComponentControllerMock;
      private IActionCaller _actionCaller;
      private Mock<IBusyIndicator> _busyIndicatorMock;

      public static ISnapShot BuilderHelper(ISnapShot snapShot)
      {
         snapShot = snapShot.AddCollection<ReportHeader>(AppStateKinds.Undoable);
         snapShot = snapShot.AddCollection<ReportTableView>(AppStateKinds.Undoable);
         snapShot = snapShot.AddCollection<ReportImage>(AppStateKinds.Undoable);
         snapShot = snapShot.AddCollection<ReportTextBox>(AppStateKinds.Undoable);
         snapShot = snapShot.AddCollection<ReportHeaderForm>(AppStateKinds.Undoable);
         snapShot = snapShot.AddCollection<ReportHeaderFormRow>(AppStateKinds.Undoable);
         snapShot = snapShot.AddCollection<DynamicPropertiesState>(AppStateKinds.Undoable);
         snapShot = snapShot.AddCollection<ReportModeState>(AppStateKinds.Undoable);
         snapShot = snapShot.AddCollection<ReportComponentSelection>(AppStateKinds.Undoable);

         return snapShot;
      }

      [SetUp]
      public void SetUp()
      {
         _reportComponentPlacementControllerMock = new Mock<IReportComponentPlacementController>();
         _reportBoundarySectionImageControllerMock = new Mock<IImageController>();
         _reportBoundarySectionTextboxControllerMock = new Mock<ITextBoxController>();
         _reportBoundarySectionHeaderFormControllerMock = new Mock<IHeaderFormController>();
         _headerFormFieldControllerMock = new Mock<IHeaderFormFieldController>();
         _selectedComponentControllerMock = new Mock<ISelectedComponentController>();
         _deleteComponentControllerMock = new Mock<IDeleteComponentController>();

         _busyIndicatorMock = new Mock<IBusyIndicator>();
         _busyIndicatorMock.Setup(b => b.SetIsBusyTrueUntilUIIsIdle()).Callback(() => { });
         _actionCaller = new BusyIndicatorActionCaller(_busyIndicatorMock.Object);

         SetUpHelper(BuilderHelper);
      }

      [Test]
      public void VMReportBoundarySectionComponentFactoryTest_GetNewViewModel()
      {
         // Arrange
         var reportHeader = new ReportHeader();
         var headerImage = ReportImage.CreateDefaultForBoundarySection(reportHeader.Id, 10, 10);
         var headerFormRow = new ReportHeaderFormRow(new List<Id<ReportHeaderFormField>>());
         var headerHeaderForm = new ReportHeaderForm(reportHeader.Id, 20, 20, new Id<ReportHeaderFormRow>[] { headerFormRow.Id });
         var headerTextBox = new ReportTextBox(reportHeader.Id, 30, 30);
         var tableView = new ReportTableView(reportHeader.Id, 30, 30);

         var newSnapShot = _history.NextSnapShot(ControllerCall.Empty);
         newSnapShot = newSnapShot.AddItem(reportHeader);
         newSnapShot = newSnapShot.AddItem(headerImage);
         newSnapShot = newSnapShot.AddItem(headerHeaderForm);
         newSnapShot = newSnapShot.AddItem(headerFormRow);
         newSnapShot = newSnapShot.AddItem(headerTextBox);
         newSnapShot = newSnapShot.AddItem(tableView);
         newSnapShot = newSnapShot.AddItem(new DynamicPropertiesState(new Id<DynamicPropertiesState>(Guid.NewGuid())));
         newSnapShot = newSnapShot.AddItem(new ReportModeState(true));
         newSnapShot = newSnapShot.AddItem(new ReportComponentSelection().WithJustThisSelectedComponent(headerImage.Id));

         _history.AddSnapShot(newSnapShot);

         var sut = new VMReportBoundarySectionComponentFactory(
            _history,
            _reportComponentPlacementControllerMock.Object,
            _reportBoundarySectionImageControllerMock.Object,
            _reportBoundarySectionTextboxControllerMock.Object,
            _reportBoundarySectionHeaderFormControllerMock.Object,
            _headerFormFieldControllerMock.Object,
            _selectedComponentControllerMock.Object,
            _deleteComponentControllerMock.Object,
            _actionCaller
         );

         // Act
         var resultImage = sut.GetNewViewModel(headerImage.Id, _history.CurrentSnapShot);
         var resultHeaderForm = sut.GetNewViewModel(headerHeaderForm.Id, _history.CurrentSnapShot);
         var resultTextBox = sut.GetNewViewModel(headerTextBox.Id, _history.CurrentSnapShot);

         // Assert
         Assert.Throws(typeof(Exception), () => sut.GetNewViewModel(tableView.Id, _history.CurrentSnapShot));
         Assert.IsTrue(resultImage is VMImage);
         Assert.IsTrue(resultHeaderForm is VMHeaderForm);
         Assert.IsTrue(resultTextBox is VMTextBox);
      }
   }
}
