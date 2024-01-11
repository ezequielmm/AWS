// <copyright file="VMReportBoundarySectionTest.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using System.Diagnostics.CodeAnalysis;
using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.ReportModule.Domain.Components.ReportSections;
using Mitutoyo.MiCAT.ReportModule.GUI.ReportBoundarySection;
using Mitutoyo.MiCAT.ReportModule.GUI.ReportBoundarySection.ValueConverters;
using Mitutoyo.MiCAT.ReportModuleApp.Controllers;
using Moq;
using NUnit.Framework;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Test.ReportBoundarySection
{
   [ExcludeFromCodeCoverage]
   [TestFixture]
   public class VMReportBoundarySectionTest
   {
      [Test]
      public void OnAddImageShouldCallController()
      {
         // Arrange
         var imageControllerMock = new Mock<IImageController>();
         var textboxControllerMock = new Mock<ITextBoxController>();
         var headerFormControllerMock = new Mock<IHeaderFormController>();
         var sectionSelectionControllerMock = new Mock<ISelectedSectionController>();
         var selectionControllerMock = new Mock<ISelectedComponentController>();
         var sectionId = new Id<ReportFooter>(Guid.NewGuid());

         var viewModel = new VMReportBoundarySection(sectionId, 100, 0, imageControllerMock.Object, textboxControllerMock.Object, headerFormControllerMock.Object, selectionControllerMock.Object, sectionSelectionControllerMock.Object);

         // Act
         viewModel.AddImageCommand.Execute(new ContextMenuCommandArgs() { Position = new System.Windows.Point(1, 2) });

         // Assert
         imageControllerMock.Verify(c => c.AddImageToBoundarySection(sectionId, 1, 2), Times.Once);
      }
   }
}
