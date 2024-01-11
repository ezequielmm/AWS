// <copyright file="BackgroundExporter.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Mitutoyo.MiCAT.ReportModule.Connectors
{
   [ExcludeFromCodeCoverage]
   public class BackgroundExporter : IDisposable
   {
      private Dispatcher _currentDispatcher;
      private ManualResetEvent dispatcherReadyEvent = new ManualResetEvent(false);

      public BackgroundExporter()
      {
         InitializeNewThread();
      }

      public async Task ExportToPDF(Guid templateId, Guid runId, FileInfo saveFileInfo)
      {
         await _currentDispatcher.Invoke(()=>LaunchExport(templateId, runId, saveFileInfo));
      }

      private void InitializeNewThread()
      {
         var _STAThread = new Thread(new ThreadStart(StartDispatcher));
         _STAThread.SetApartmentState(ApartmentState.STA);
         _STAThread.IsBackground = true;
         _STAThread.Start();

         dispatcherReadyEvent.WaitOne();
      }

      private void StartDispatcher()
      {
         _currentDispatcher = Dispatcher.CurrentDispatcher;

         SynchronizationContext.SetSynchronizationContext(new DispatcherSynchronizationContext(_currentDispatcher));

         dispatcherReadyEvent.Set();

         Dispatcher.Run();
      }

      private async Task LaunchExport(Guid templateId, Guid runId, FileInfo saveFileInfo)
      {
         using var pdfExporter = new PdfExporter(saveFileInfo);
         await pdfExporter.Export(templateId, runId);
      }

      public void Dispose()
      {
         _currentDispatcher.BeginInvokeShutdown(DispatcherPriority.Normal);
      }
   }
}
