// <copyright file="RadFixedDocumentCreator.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using System.Windows;
using System.Windows.Threading;
using Mitutoyo.MiCAT.ReportModule.GUI.Common.ComponentsPageViewRelation;
using Mitutoyo.MiCAT.ReportModule.GUI.MainWindow;
using Mitutoyo.MiCAT.ReportModule.PDFRenderer.Renderers;
using Telerik.Windows.Documents.Fixed.Model;
using Telerik.Windows.Documents.Fixed.Model.Editing;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Export.ReportViewToRadFixedDocument
{
   public class RadFixedDocumentCreator : IRadFixedDocumentGenerator
   {
      private IReportComponentsByPageFromReportView _reportComponentsByPageGetter;
      private VMReportView _VMreportView;
      private IRenderer _renderer;

      public RadFixedDocumentCreator(
         IReportComponentsByPageFromReportView reportComponentsByPageGetter,
         VMReportView vmReport, IRenderer renderer)
      {
         _reportComponentsByPageGetter = reportComponentsByPageGetter;
         _VMreportView = vmReport;

         _renderer = renderer;
      }

      public RadFixedDocument GenerateDocumentToExportAsync()
      {
         RadFixedDocument resultDocument;

         var report = new ReportView();
         report.DataContext = _VMreportView;

         RenderReportViewOnMemory(report);
         resultDocument = CreateDocument(report);

         report.DataContext = null;
         return resultDocument;
      }

      private void RenderReportViewOnMemory(ReportView reportView)
      {
         reportView.Measure(new Size(Double.PositiveInfinity, Double.PositiveInfinity));
         reportView.Arrange(new Rect(reportView.DesiredSize));

         WaitUntilRenderEnds(reportView);
      }

      private void WaitUntilRenderEnds(ReportView reportView)
      {
         reportView.Dispatcher?.Invoke(new Action(() => { }), DispatcherPriority.ApplicationIdle);
      }

      private RadFixedDocument CreateDocument(ReportView reportView)
      {
         var resultDocument = new RadFixedDocument();

         foreach (var componentsByPage in _reportComponentsByPageGetter.GetReportComponentsByPage(reportView))
            resultDocument.Pages.Add(CreatePage(componentsByPage));

         return resultDocument;
      }
      private RadFixedPage CreatePage(ReportComponentsByPage componentsByPage)
      {
         var radPage = new RadFixedPage { Size = new Size(componentsByPage.Page.ActualWidth, componentsByPage.Page.ActualHeight) };
         var editor = new FixedContentEditor(radPage, Telerik.Windows.Documents.Fixed.Model.Data.MatrixPosition.Default);

         foreach (var component in componentsByPage.Components)
         {
            _renderer.Render(component, componentsByPage.Page, editor);
         }

         return radPage;
      }
   }
}
