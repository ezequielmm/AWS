using Mitutoyo.MiCAT.ApplicationState;

namespace Mitutoyo.Micat.PdfExportDebugModule.Module
{
   public interface IPdfExportDebugModuleNavigation
   {
      UniqueValue Id { get; }
      void Initialize();
      void Navigate();
   }
}
