// <copyright file="IReportModule.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.ShellModule;
using Prism.Modularity;
using Unity;

namespace Mitutoyo.MiCAT.ReportModule
{
   public interface IReportModule : IModule
   {
      void InitializeWorkspace(Id<NavigationModuleChildInfo> containerId, IUnityContainer reportViewUnityContainer);
   }
}
