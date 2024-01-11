using Mitutoyo.Micat.PdfExportDebugModule.Persistence;
using Mitutoyo.MiCAT.DataServiceClient;
using Mitutoyo.MiCAT.ReportModuleConnector.ExportConnector;
using System.Diagnostics.CodeAnalysis;
using Unity;

namespace Mitutoyo.Micat.PdfExportDebugModule.Module
{
   [ExcludeFromCodeCoverage]
   public class PdfExportDebugModuleRegister : IPdfExportDebugModuleRegister
   {
      private readonly IUnityContainer _moduleContainer;

      public PdfExportDebugModuleRegister(IUnityContainer moduleContainer)
      {
         _moduleContainer = moduleContainer;
      }

      public void Initialize(IReportExporter reportExporter)
      {
         RegisterTypes(reportExporter);
         InitializeDataService();
      }

      private void RegisterTypes(IReportExporter reportExporter)
      {
         _moduleContainer.RegisterInstance(reportExporter);
         _moduleContainer.RegisterSingleton<IDataServiceClient, DataServiceClient>();
         _moduleContainer.RegisterSingleton<IDataServiceConfigurator, DataServiceConfigurator>();
         _moduleContainer.RegisterSingleton<IMainPersistence, MainPersistence>();
      }

      private void InitializeDataService()
      {
         var dataServiceConfigurator = _moduleContainer.Resolve<IDataServiceConfigurator>();
         dataServiceConfigurator.Configure();
      }
   }
}
