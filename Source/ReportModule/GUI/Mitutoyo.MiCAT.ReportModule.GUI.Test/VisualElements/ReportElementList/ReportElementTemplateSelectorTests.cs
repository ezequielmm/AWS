// <copyright file="ReportElementTemplateSelectorTests.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.ReportModule.App.AppState;
using Mitutoyo.MiCAT.ReportModule.Domain.Components;
using Mitutoyo.MiCAT.ReportModule.Domain.Components.ReportSections;
using Mitutoyo.MiCAT.ReportModule.Domain.Data;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.Pieces.Table.ViewModels;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElementList.Views;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements.HeaderForm.ViewModels;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements.Image.ViewModels;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements.Table.ViewModels;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements.TesselationView.ViewModels;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements.TextBox.ViewModels;
using Mitutoyo.MiCAT.ReportModuleApp.AppState;
using Mitutoyo.MiCAT.ReportModuleApp.Controllers;
using Mitutoyo.MiCAT.ReportModuleApp.Utilities;
using Moq;
using NUnit.Framework;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Test.VisualElements.ReportElementList
{
   [ExcludeFromCodeCoverage]
   public class ReportElementTemplateSelectorTests
   {
      private readonly ReportElementTemplateSelector _selector;

      private readonly DataTemplate _textBoxDataTemplate;
      private readonly DataTemplate _tableDataTemplate;
      private readonly DataTemplate _tablePieceDataTemplate;
      private readonly DataTemplate _tessellationDataTemplate;
      private readonly DataTemplate _imageDataTemplate;
      private readonly DataTemplate _headerFormDataTemplate;
      private Mock<IAppStateHistory> _historyMock;

      public ReportElementTemplateSelectorTests()
      {
         _textBoxDataTemplate = new DataTemplate("TextBox");
         _tableDataTemplate = new DataTemplate("Table");
         _tessellationDataTemplate = new DataTemplate("Tessellation");
         _imageDataTemplate = new DataTemplate("Image");
         _headerFormDataTemplate = new DataTemplate("HeaderForm");
         _tablePieceDataTemplate = new DataTemplate("TablePiece");

         _selector = new ReportElementTemplateSelector
         {
            TextBoxTemplate = _textBoxDataTemplate,
            TableViewTemplate = _tableDataTemplate,
            TessellationViewTemplate = _tessellationDataTemplate,
            ImageTemplate = _imageDataTemplate,
            HeaderFormTemplate = _headerFormDataTemplate,
            TableViewPieceTemplate = _tablePieceDataTemplate,
         };
      }

      [SetUp]
      public void Setup()
      {
         _historyMock = new Mock<IAppStateHistory>();
         _historyMock.Setup(h => h.AddClient(It.IsAny<ISubscriptionClient>(), 0)).Returns(_historyMock.Object);
      }

      [Test]
      public void SelectTemplateForVMTextBox_ShouldReturnTextBoxTemplate()
      {
         var component = new ReportTextBox(new ReportComponentPlacement(new Id<ReportBody>(Guid.NewGuid()), 10, 10, 100, 100));
         var snapShot = Mock.Of<ISnapShot>(x =>
               x.GetChanges() == new Changes() &&
               x.GetItems<ReportModeState>() == new[] { new ReportModeState(), } &&
               x.GetItems<ReportComponentSelection>() == new[] { new ReportComponentSelection(), } &&
               x.GetItem(component.Id as IItemId<IReportComponent>) == (IReportComponent)component);

         _historyMock.Setup(h => h.CurrentSnapShot).Returns(snapShot);

         var item = new VMTextBox(
            _historyMock.Object,
            component.Id,
            Mock.Of<IVMReportComponentPlacement>(),
            Mock.Of<IDeleteComponentController>(),
            Mock.Of<ITextBoxController>());

         var result = _selector.SelectTemplate(item, null);

         Assert.IsNotNull(result);
         Assert.AreEqual(_textBoxDataTemplate, result);
      }

      [Test]
      public void SelectTemplateForVMTable_ShouldReturnTableTemplate()
      {
         var table = new ReportTableView(new ReportComponentPlacement(new ReportBody().Id, 0, 0, 100, 100)).WithDefaultColumns();

         var snapShot = Mock.Of<ISnapShot>(x =>
               x.GetChanges() == new Changes(new[] { new AddItemChange(table.Id), }.ToImmutableList<IItemChange>()) &&
               x.GetItems<ReportModeState>() == new[] { new ReportModeState(), } &&
               x.GetItems<RunSelection>() == new[] { new RunSelection(), } &&
               x.GetItems<ReportComponentSelection>() == new[] { new ReportComponentSelection(), } &&
               x.GetItems<AllCharacteristicTypes>() == new[]
               {
                  new AllCharacteristicTypes(ImmutableList.Create<string>()),
               }
               &&
               x.GetItems<AllCharacteristicDetails>() == new[]
               {
                  new AllCharacteristicDetails(ImmutableList.Create<string>()),
               }
               &&
               x.GetItem(table.Id as IItemId<IReportComponent>) == (IReportComponent) table);

         _historyMock.Setup(h => h.CurrentSnapShot).Returns(snapShot);

         var item = new VMTable(
            _historyMock.Object,
            table.Id,
            Mock.Of<IVMDisableSpacePlacement>(),
            Mock.Of<IDeleteComponentController>(),
            Mock.Of<ITableViewController>());

         var result = _selector.SelectTemplate(item, null);

         Assert.IsNotNull(result);
         Assert.AreEqual(_tableDataTemplate, result);
      }

      [Test]
      public void SelectTemplateForVMTablePiece_ShouldReturnTablePieceTemplate()
      {
         var item = new VMTablePiece(Mock.Of<IVMVisualPlacement>(), Mock.Of<IVMReportComponent>());

         var result = _selector.SelectTemplate(item, null);

         Assert.IsNotNull(result);
         Assert.AreEqual(_tablePieceDataTemplate, result);
      }

      [Test]
      [Ignore("We should considere to remove all tessllation classes to avoid spending time to fix these things.")]
      public void SelectTemplateForVMTessellation_ShouldReturnTessellationTemplate()
      {
         var tessellation = new ReportTessellationView(new ReportComponentPlacement(new ReportBody().Id, 0, 0, 100, 100));

         var snapShot = Mock.Of<ISnapShot>(x =>
               x.GetChanges() == new Changes() &&
               x.GetItems<ReportModeState>() == new[] { new ReportModeState(), } &&
               x.GetItems<ReportComponentSelection>() == new[] { new ReportComponentSelection(), } &&
               x.GetItem<IReportComponent>(tessellation.Id as IItemId) == (IReportComponent) tessellation &&
               x.GetItem<ReportTessellationView>(tessellation.Id as IItemId) == tessellation);

         _historyMock.Setup(h => h.CurrentSnapShot).Returns(snapShot);

         var item = new VMTessellationView(
            _historyMock.Object,
            new Id<ReportTessellationView>(Guid.NewGuid()),
            Mock.Of<IVMReportBodyPlacement>(),
            Mock.Of<IDeleteComponentController>(),
            Mock.Of<ITessellationViewController>());

         var result = _selector.SelectTemplate(item, null);

         Assert.IsNotNull(result);
         Assert.AreEqual(_tessellationDataTemplate, result);
      }

      [Test]
      public void SelectTemplateForVMImage_ShouldReturnImageTemplate()
      {
         ReportImage entity = new ReportImage(new ReportComponentPlacement(new ReportBody().Id, 0, 0, 100, 100));
         var snapShot = Mock.Of<ISnapShot>(x =>
               x.GetChanges() == new Changes() &&
               x.GetItems<ReportModeState>() == new[] { new ReportModeState(), } &&
               x.GetItems<ReportComponentSelection>() == new[] { new ReportComponentSelection(), } &&
               x.GetItem(entity.Id as IItemId<IReportComponent>) == (IReportComponent) entity);

         _historyMock.Setup(h => h.CurrentSnapShot).Returns(snapShot);

         var item = new VMImage(
            _historyMock.Object,
            entity.Id,
            Mock.Of<IVMReportComponentPlacement>(),
            Mock.Of<IDeleteComponentController>(),
            Mock.Of<IImageController>(),
            Mock.Of<IActionCaller>());

         var result = _selector.SelectTemplate(item, null);

         Assert.IsNotNull(result);
         Assert.AreEqual(_imageDataTemplate, result);
      }

      [Test]
      public void SelectTemplateForVMHeaderForm_ShouldReturnHeaderFormTemplate()
      {
         ReportHeaderForm entity = new ReportHeaderForm (new ReportComponentPlacement(new ReportBody().Id, 0, 0, 100, 100), ImmutableList<Id<ReportHeaderFormRow>>.Empty);
         var snapShot = Mock.Of<ISnapShot>(x =>
               x.GetChanges() == new Changes() &&
               x.GetItems<ReportModeState>() == new[] { new ReportModeState(), } &&
               x.GetItems<ReportComponentSelection>() == new[] { new ReportComponentSelection(), } &&
               x.GetItem(entity.Id as IItemId<IReportComponent>) == (IReportComponent)entity &&
               x.GetItem<ReportHeaderForm>(entity.Id as IItemId<IReportComponent>) == (ReportHeaderForm)entity &&
               x.GetItems<DynamicPropertiesState>() == new[]
               {
                  new DynamicPropertiesState(new  Id<DynamicPropertiesState>(new UniqueValueFactory())),
               });

         _historyMock.Setup(h => h.CurrentSnapShot).Returns(snapShot);

         var item = new VMHeaderForm(
            _historyMock.Object,
            entity.Id,
            Mock.Of<IVMReportComponentPlacement>(),
            Mock.Of<IDeleteComponentController>(),
            Mock.Of<IHeaderFormController>(),
            Mock.Of<IHeaderFormFieldController>());

         var result = _selector.SelectTemplate(item, null);

         Assert.IsNotNull(result);
         Assert.AreEqual(_headerFormDataTemplate, result);
      }
   }
}
