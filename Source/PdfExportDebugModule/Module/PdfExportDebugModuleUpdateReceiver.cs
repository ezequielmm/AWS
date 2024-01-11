using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.ApplicationState.Clients;
using Mitutoyo.MiCAT.Core;
using Mitutoyo.MiCAT.ShellModule;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace Mitutoyo.Micat.PdfExportDebugModule.Module
{
   [ExcludeFromCodeCoverage]
   public class PdfExportDebugModuleUpdateReceiver : IUpdateClient
   {
      private readonly IPdfExportDebugModuleNavigation _moduleNavigation;

      public PdfExportDebugModuleUpdateReceiver(IAppStateHistory appStateHistory, IPdfExportDebugModuleNavigation moduleNavigation)
      {
         _moduleNavigation = moduleNavigation;
         appStateHistory.Subscribe(this);
      }

      public Name Name { get; } = "Pdf Export";

      public Task Update(ISnapShot snapShot)
      {
         var selectedItem = snapShot.GetItems<SelectedItem>().SingleOrDefault();

         if (selectedItem?.TargetId.UniqueValue == _moduleNavigation.Id)
            _moduleNavigation.Navigate();

         return Task.CompletedTask;
      }
   }
}
