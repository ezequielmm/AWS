using Mitutoyo.MiCAT.ReportModuleConnector.ExportConnector;

namespace Mitutoyo.Micat.PdfExportDebugModule.Module
{
   public interface IPdfExportDebugModuleRegister
   {
      void Initialize(IReportExporter reportExporter);
   }
}
