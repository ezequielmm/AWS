// <copyright file="WorskpaceContainerResolver.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.ReportModuleApp.Manager;
using Unity;

namespace Mitutoyo.MiCAT.ReportModule.ReportModuleManager
{
   public class WorskpaceContainerResolver : IWorskpaceContainerResolver
   {
      private readonly IReportModuleNavigation _navigation;
      public WorskpaceContainerResolver(IReportModuleNavigation navigation)
      {
         _navigation = navigation;
      }
      private IUnityContainer GetWorkspaceInstanceContainer(Id id)
      {
         return _navigation.GetContainer(id);
      }
      public IWorkspaceCloseManager GetWorkspaceController(Id id)
      {
         return GetWorkspaceInstanceContainer(id).Resolve<IWorkspaceCloseManager>();
      }
   }

   public interface IWorskpaceContainerResolver
   {
      IWorkspaceCloseManager GetWorkspaceController(Id id);
   }
}
