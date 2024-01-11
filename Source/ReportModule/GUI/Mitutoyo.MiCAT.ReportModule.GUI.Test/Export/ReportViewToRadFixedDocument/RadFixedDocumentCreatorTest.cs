// <copyright file="RadFixedDocumentCreatorTest.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using Mitutoyo.MiCAT.ReportModule.GUI.Common;
using Mitutoyo.MiCAT.ReportModule.GUI.Common.ComponentsPageViewRelation;
using Mitutoyo.MiCAT.ReportModule.GUI.Common.LayoutCalculator;
using Mitutoyo.MiCAT.ReportModule.GUI.ContextMenu;
using Mitutoyo.MiCAT.ReportModule.GUI.Export.ReportViewToRadFixedDocument;
using Mitutoyo.MiCAT.ReportModule.GUI.MainWindow;
using Mitutoyo.MiCAT.ReportModule.PDFRenderer.Renderers;
using Mitutoyo.MiCAT.ReportModuleApp.Controllers;
using Moq;
using NUnit.Framework;
using Telerik.Windows.Documents.Fixed.Model;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Test.Export.ReportViewToRadFixedDocument
{
   [ExcludeFromCodeCoverage]
   [TestFixture]
   public class RadFixedDocumentCreatorTest
   {
      private RadFixedDocumentCreator _radFixedDocumentCreator;
      private Mock<IReportComponentsByPageFromReportView> _reportComponentByPageFromReporViewMock;
      [SetUp]
      public void Setup()
      {
         var pageRendererMock = new Mock<IPagesRenderer>();
         var reportModePropertyMock = new Mock<IReportModeProperty>();
         var rendererMock = new Mock<IRenderer>();
         var paletteContextMenu = new Mock<PaletteContextMenu>(MockBehavior.Strict, new object[]
         {
            new Mock<ITextBoxController>().Object,
            new Mock<IImageController>().Object,
            new Mock<ITessellationViewController>().Object,
            new Mock<IHeaderFormController>().Object,
            new Mock<ITableViewController>().Object,
            new Mock<IRenderedData>().Object
         });
         var vmReportView = new VMReportView(pageRendererMock.Object, reportModePropertyMock.Object, paletteContextMenu.Object);
         _reportComponentByPageFromReporViewMock = new Mock<IReportComponentsByPageFromReportView>();
         _reportComponentByPageFromReporViewMock.Setup(s => s.GetReportComponentsByPage(It.IsAny<ReportView>())).Returns(ImmutableList<ReportComponentsByPage>.Empty);
         _radFixedDocumentCreator = new RadFixedDocumentCreator(_reportComponentByPageFromReporViewMock.Object, vmReportView, rendererMock.Object);
      }
      [Test]
      [Apartment(ApartmentState.STA)]
      public void GenerateDocumentToExportAsyncShouldReturnRadFixDocument()
      {
         //Act
         var result = _radFixedDocumentCreator.GenerateDocumentToExportAsync();
         //Assert
         Assert.IsTrue(result is RadFixedDocument);
      }
   }
}
