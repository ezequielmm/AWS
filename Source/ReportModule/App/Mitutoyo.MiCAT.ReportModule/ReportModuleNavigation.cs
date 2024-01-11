// <copyright file="ReportModuleNavigation.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Collections.Generic;
using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.Core.StdLib.Extensions;
using Mitutoyo.MiCAT.ReportModule.GUI;
using Mitutoyo.MiCAT.ReportModule.GUI.Common.Filebar;
using Mitutoyo.MiCAT.ShellModule;
using Mitutoyo.MiCAT.ShellModule.Regions;
using Prism.Regions;
using Unity;

namespace Mitutoyo.MiCAT.ReportModule
{
   public class ReportModuleNavigation : IReportModuleNavigation
   {
      private readonly IRegionManager _regionManager;
      private readonly IReportModule _reportModule;
      private readonly IDictionary<Id, IUnityContainer> _moduleContainers;

      public ReportModuleNavigation(IRegionManager regionManager, IReportModule reportModule)
      {
         _regionManager = regionManager;
         _reportModule = reportModule;
         _moduleContainers = new Dictionary<Id, IUnityContainer>();
      }

      public void AddNewInstanceToRegion(Id containerId)
      {
         IUnityContainer container = GetContainer(containerId);
         var viewId = containerId.ToString();

         AddToRegion<ReportFilebar>(RegionNames.ToolBarRegion, viewId, container);
         AddToRegion<ReportViewWorkspace>(RegionNames.WorkspaceRegion, viewId, container);

         DeActivateViews(RegionNames.ToolBarRegion);
         DeActivateViews(RegionNames.WorkspaceRegion);
      }

      public void RemoveContainer(Id key)
      {
         if (_moduleContainers.TryGetValue(key, out var container))
         {
            container.Dispose();
            _moduleContainers.Remove(key);
         }
      }

      public IUnityContainer GetContainer(Id key)
      {
         return _moduleContainers[key];
      }

      public void Navigate<TView>(string regionName, string viewName, IUnityContainer container)
      {
         AddToRegion<TView>(regionName, viewName, container);

         ActivateView(regionName, viewName);
      }

      public void Navigate(string regionName, string source)
      {
         _regionManager.RequestNavigate(regionName, source);
      }

      public void ActivateView(string regionName, string viewName)
      {
         var view = _regionManager.Regions[regionName].GetView(viewName);
         _regionManager.Regions[regionName].Activate(view);
      }

      public void DeActivateViews(string regionName)
      {
         _regionManager.Regions[regionName].ActiveViews.ForEach(v =>
         {
            _regionManager.Regions[regionName].Deactivate(v);
         });
      }

      public IUnityContainer CreateContainerForWorkspace(Id<NavigationModuleChildInfo> childId)
      {
         var unityContainerReportModule = new UnityContainer();
         _reportModule.InitializeWorkspace(childId, unityContainerReportModule);
         _moduleContainers.Add(childId, unityContainerReportModule);
         return unityContainerReportModule;
      }

      private void AddToRegion<TView>(string regionName, string viewName, IUnityContainer container)
      {
         var view = _regionManager.Regions[regionName].GetView(viewName);

         if (view == null)
         {
            _regionManager.Regions[regionName].Add(container.Resolve(typeof(TView)), viewName);
         }
      }
   }
}
