// <copyright file="IReportModuleNavigation.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.ShellModule;
using Unity;

namespace Mitutoyo.MiCAT.ReportModule
{
   public interface IReportModuleNavigation
   {
      IUnityContainer GetContainer(Id key);

      IUnityContainer CreateContainerForWorkspace(Id<NavigationModuleChildInfo> childId);

      void AddNewInstanceToRegion(Id containerId);

      void Navigate<TView>(string regionName, string viewName, IUnityContainer container);

      void Navigate(string regionName, string source);

      void DeActivateViews(string regionName);

      void RemoveContainer(Id key);
   }
}
