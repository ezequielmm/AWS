using Mitutoyo.Micat.PdfExportDebugModule.Descriptors;
using Mitutoyo.Micat.PdfExportDebugModule.Persistence;
using Mitutoyo.MiCAT.Common.GUI.Commands;
using Mitutoyo.MiCAT.ReportModuleConnector.ExportConnector;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using Unity;
using System.Linq;
using System.Collections.ObjectModel;

namespace Mitutoyo.Micat.PdfExportDebugModule
{
    [ExcludeFromCodeCoverage]
   public class VMPdfExportDebug : BindableBase
   {
      private IEnumerable<TemplateDescriptor> _templates;
      private TemplateDescriptor _selectedTemplate;

      private ObservableCollection<RunDescriptor> _runs;
      private RunDescriptor _selectedRun;

      private IReportExporter _reportExporter;
      private string _exportResultMessage;

      public VMPdfExportDebug(IUnityContainer moduleContainer)
      {
         _reportExporter = moduleContainer.Resolve<IReportExporter>();
         ExportCommand = new RelayCommand(OnExportCommand);

         Initialize(moduleContainer);
      }

      public IEnumerable<TemplateDescriptor> Templates
      {
         get => _templates;
         private set
         {
            if (_templates == value)
               return;

            _templates = value;
            RaisePropertyChanged();
         }
      }

      public TemplateDescriptor SelectedTemplate
      {
         get => _selectedTemplate;
         set
         {
            if (_selectedTemplate == value)
               return;

            _selectedTemplate = value;
            RaisePropertyChanged();
            RaisePropertyChanged(nameof(IsExportEnabled));
         }
      }

      public ObservableCollection<RunDescriptor> Runs
      {
         get => _runs;
         private set
         {
            if (_runs == value)
               return;

            _runs = value;
            RaisePropertyChanged();
         }
      }

      public RunDescriptor SelectedRun
      {
         get => _selectedRun;
         set
         {
            if (_selectedRun == value)
               return;

            _selectedRun = value;
            RaisePropertyChanged();
            RaisePropertyChanged(nameof(IsExportEnabled));
         }
      }

      public bool IsExportEnabled
      {
            get => SelectedTemplate != null && Runs.Any(r => r.IsSelected);
      }

      public string ExportResultMessage
      {
         get => _exportResultMessage;
         private set
         {
            if (_exportResultMessage == value)
               return;

            _exportResultMessage = value;
            RaisePropertyChanged();
         }
      }

      public ICommand ExportCommand { get; }

      private async void Initialize(IUnityContainer moduleContainer)
      {
         await FillData(moduleContainer.Resolve<IMainPersistence>());
      }

      private async Task FillData(IMainPersistence mainPersistence)
      {
         Templates = await mainPersistence.GetTemplates();
         Runs = new ObservableCollection<RunDescriptor>(await mainPersistence.GetRuns());
      }

      private async void LaunchExport(TemplateDescriptor template, RunDescriptor run)
      {
         Stopwatch stopWatch = new Stopwatch();
         stopWatch.Start();

         ExportResultMessage += $"{ DateTime.Now.ToLongTimeString() } Process Started     ({ template.Description  } / { run.Description })" + System.Environment.NewLine;

         var result = await _reportExporter.ExportToPdf(template.Id, run.Id, new FileInfo($"{Path.GetTempPath()}MicatReport{DateTime.Now.Ticks}.pdf"));

         stopWatch.Stop();

        if (result.IsSuccess)
            ExportResultMessage += $"{ DateTime.Now.ToLongTimeString() } Process Ended      ({ template.Description } / { run.Description })     Time: { stopWatch.Elapsed.ToString("hh\\:mm\\:ss\\.ff") }" + System.Environment.NewLine;
        else
            ExportResultMessage += $"{ DateTime.Now.ToLongTimeString() } Process Failed       ({ template.Description } / { run.Description })     Time: { stopWatch.Elapsed.ToString("hh\\:mm\\:ss\\.ff") }" + System.Environment.NewLine + result.Failure.OriginalException.StackTrace + System.Environment.NewLine;

      }

      private void OnExportCommand(object obj = null)
      {
         foreach (var run in Runs.Where(r => r.IsSelected))
         {
            LaunchExport(SelectedTemplate, run);
         }
      }
   }
}
