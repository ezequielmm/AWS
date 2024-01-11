// <copyright file="PdfExporter.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading.Tasks;
using Mitutoyo.MiCAT.ReportModule.Domain.Data;
using Mitutoyo.MiCAT.ReportModule.GUI.Utilities.Events;
using Mitutoyo.MiCAT.ReportModule.Setup;
using Mitutoyo.MiCAT.ReportModule.Setup.Export;
using Mitutoyo.MiCAT.ReportModule.Setup.ReportWorkspace;
using Mitutoyo.MiCAT.ReportModuleApp.Controllers;
using Mitutoyo.MiCAT.Utilities.IoC;
using Unity;

namespace Mitutoyo.MiCAT.ReportModule.Connectors
{
   [ExcludeFromCodeCoverage]
   internal class PdfExporter : IDisposable
   {
      private readonly IUnityContainer _container;

      public PdfExporter(FileInfo saveFileInfo)
      {
         _container = new UnityContainer();

         var serviceRegistrar = new ServiceRegistrar(_container);
         var serviceLocator = new ServiceLocator(_container);

         var commonRegistrar = new CommonRegistrar();
         var pdfExportRegistrar = new PdfExportRegistrar(commonRegistrar, saveFileInfo);

         pdfExportRegistrar.Register(serviceRegistrar);

         var appController = new ApplicationController(serviceLocator);
         appController.CreateAppClients();
         appController.ConfigureApplication();
      }

      internal async Task Export(Guid templateId, Guid runId)
      {
         var reportViewerStartUp = _container.Resolve<IReportWorkspaceStartUp>();
         var reportTemplateController = _container.Resolve<IReportTemplateController>();
         var runRequestController = _container.Resolve<IRunRequestController>();
         var pdfExportController = _container.Resolve<IPdfExportController>();

         var reportTemplate = await reportTemplateController.GetReportTemplateById(templateId);
         var selectedReportTemplateInfo = new SelectedReportTemplateInfo(reportTemplate, ReportMode.ViewMode);

         await reportViewerStartUp.Start(selectedReportTemplateInfo);
         await runRequestController.RequestRunAsync(runId);
         pdfExportController.Export();
      }

      public void Dispose()
      {
         _container.Dispose();
      }
   }
}
