using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.ShellModule;
using Mitutoyo.MiCAT.ShellModule.Regions;
using Prism.Regions;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Mitutoyo.Micat.PdfExportDebugModule.Module
{
   [ExcludeFromCodeCoverage]
   public class PdfExportDebugModuleNavigation : IPdfExportDebugModuleNavigation
   {
      private readonly IAppStateHistory _appStateHistory;
      private readonly IRegionManager _regionManager;

      public PdfExportDebugModuleNavigation(IAppStateHistory appStateHistory, IRegionManager regionManager)
      {
         _appStateHistory = appStateHistory;
         _regionManager = regionManager;
      }

      public UniqueValue Id { get; } = new UniqueValue(Guid.NewGuid());

      public void Initialize()
      {
         NavigationModuleInfo navigationModuleInfo = CreateNavigationModuleInfo();
         _appStateHistory.Run(snapShot => AddNavigationModuleInfoToSnapshot(snapShot, navigationModuleInfo));
      }

      public void Navigate()
      {
         _regionManager.RequestNavigate(RegionNames.WorkspaceRegion, nameof(PdfExportDebugView));
      }

      private ISnapShot AddNavigationModuleInfoToSnapshot(ISnapShot snapShot, NavigationModuleInfo navigationModuleInfo)
      {
         var navigationList = snapShot.GetItems<NavigationList>().Single();
         var navigationListToModule = new NavigationListToModule(
            _appStateHistory.UniqueValueFactory.UniqueValue(),
            navigationList.Id,
            navigationModuleInfo.Id,
            2);

         return snapShot
            .AddItem(navigationModuleInfo)
            .AddItem(navigationListToModule);
      }

      private NavigationModuleInfo CreateNavigationModuleInfo()
      {
         return new NavigationModuleInfo(
            new Id<NavigationModuleInfo>(Id),
            "PDF Export",
            "Module/ModuleIcon.xaml",
            "ModuleIcon",
            GetType().Assembly.FullName,
            "T");
      }
   }
}
