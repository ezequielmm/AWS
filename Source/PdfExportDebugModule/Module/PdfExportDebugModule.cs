using Mitutoyo.MiCAT.ReportModuleConnector.ExportConnector;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Unity;
using System.Diagnostics.CodeAnalysis;
using Unity;

namespace Mitutoyo.Micat.PdfExportDebugModule.Module
{
   [ExcludeFromCodeCoverage]
   public class PdfExportDebugModule : IModule
   {
      public void OnInitialized(IContainerProvider containerProvider)
      {
         var shellUnityContainer = containerProvider.GetContainer();

         shellUnityContainer.Resolve<PdfExportDebugModuleUpdateReceiver>();

         var moduleNavigation = shellUnityContainer.Resolve<IPdfExportDebugModuleNavigation>();
         moduleNavigation.Initialize();

         var moduleRegister = shellUnityContainer.Resolve<IPdfExportDebugModuleRegister>();
         moduleRegister.Initialize(shellUnityContainer.Resolve<IReportExporter>());
      }

      public void RegisterTypes(IContainerRegistry containerRegistry)
      {
         containerRegistry.RegisterSingleton<IUnityContainer, UnityContainer>();
         containerRegistry.RegisterSingleton<IPdfExportDebugModuleRegister, PdfExportDebugModuleRegister>();
         containerRegistry.RegisterSingleton<IPdfExportDebugModuleNavigation, PdfExportDebugModuleNavigation>();
         containerRegistry.RegisterForNavigation<PdfExportDebugView, VMPdfExportDebug>();
      }
   }
}
