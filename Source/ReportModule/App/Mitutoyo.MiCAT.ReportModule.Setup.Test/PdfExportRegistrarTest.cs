// <copyright file="PdfExportRegistrarTest.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using Mitutoyo.MiCAT.Applications.Services.Core;
using Mitutoyo.MiCAT.ReportModule.GUI.Common;
using Mitutoyo.MiCAT.ReportModule.GUI.ContextMenu;
using Mitutoyo.MiCAT.ReportModule.Persistence;
using Mitutoyo.MiCAT.ReportModule.Setup.Export;
using Mitutoyo.MiCAT.ReportModule.Setup.Export.Fakes;
using Mitutoyo.MiCAT.ReportModuleApp.Controllers;
using Mitutoyo.MiCAT.ReportModuleApp.Providers;
using Mitutoyo.MiCAT.ReportModuleApp.Providers.Inputs;
using Mitutoyo.MiCAT.ReportModuleApp.Utilities;
using Mitutoyo.MiCAT.Utilities.IoC;
using Moq;
using NUnit.Framework;

namespace Mitutoyo.MiCAT.ReportModule.Setup.Test
{
   [ExcludeFromCodeCoverage]
   [TestFixture]
   public class PdfExportRegistrarTest
   {
      private Mock<IServiceRegistrar> serviceRegistrarMock;
      private PdfExportRegistrar sut;

      [SetUp]
      public void Setup()
      {
         // Arrange
         serviceRegistrarMock = new Mock<IServiceRegistrar> { DefaultValue = DefaultValue.Mock };
         var commonRegistrar = new Mock<ICommonRegistrar>();

         sut = new PdfExportRegistrar(commonRegistrar.Object, null);
      }

      [Test]
      public void PdfFakesShouldBeRegistered()
      {
         // Act
         sut.Register(serviceRegistrarMock.Object);

         // Assert
         serviceRegistrarMock.Verify(s => s.RegisterSingleton<IReportModeProperty, ReportModePropertyFake>(), Times.Once);
         serviceRegistrarMock.Verify(s => s.RegisterSingleton<IActionCaller, ActionCallerFake>(), Times.Once);
         serviceRegistrarMock.Verify(s => s.RegisterSingleton<IMainContextMenu, PaletteContextMenuFake>(), Times.Once);
         serviceRegistrarMock.Verify(s => s.RegisterSingleton<ITemplateNameResolver, TemplateNameResolverFake>(), Times.Once);
         serviceRegistrarMock.Verify(s => s.RegisterSingleton<IUnsavedChangesService, UnsavedChangesServiceFake>(), Times.Once);
         serviceRegistrarMock.Verify(s => s.RegisterSingleton<IReportTemplateDeleteConfirmationInput, ReportTemplateDeleteConfirmationInputFake>(), Times.Once);
         serviceRegistrarMock.Verify(s => s.RegisterSingleton<ITextBoxController, TextBoxControllerFake>(), Times.Once);
         serviceRegistrarMock.Verify(s => s.RegisterSingleton<ITableViewController, TableViewControllerFake>(), Times.Once);
         serviceRegistrarMock.Verify(s => s.RegisterSingleton<ITessellationViewController, TessellationViewControllerFake>(), Times.Once);
         serviceRegistrarMock.Verify(s => s.RegisterSingleton<IImageController, ImageControllerFake>(), Times.Once);
         serviceRegistrarMock.Verify(s => s.RegisterSingleton<IHeaderFormController, HeaderFormControllerFake>(), Times.Once);
         serviceRegistrarMock.Verify(s => s.RegisterSingleton<IHeaderFormFieldController, HeaderFormFieldControllerFake>(), Times.Once);
         serviceRegistrarMock.Verify(s => s.RegisterSingleton<ISelectedComponentController, SelectedComponentControllerFake>(), Times.Once);
         serviceRegistrarMock.Verify(s => s.RegisterSingleton<ISelectedSectionController, SelectedSectionControllerFake>(), Times.Once);
         serviceRegistrarMock.Verify(s => s.RegisterSingleton<IDeleteComponentController, DeleteComponentControllerFake>(), Times.Once);
         serviceRegistrarMock.Verify(s => s.RegisterSingleton<IPlanPersistence, PlanPersistenceFake>(), Times.Once);
         serviceRegistrarMock.Verify(s => s.RegisterSingleton<IPartPersistence, PartPersistenceFake>(), Times.Once);
      }
   }
}
