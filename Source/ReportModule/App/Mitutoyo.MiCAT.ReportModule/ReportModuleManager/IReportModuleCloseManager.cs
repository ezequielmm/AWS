// <copyright file="IReportModuleCloseManager.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Threading.Tasks;
using Mitutoyo.MiCAT.ApplicationState;

namespace Mitutoyo.MiCAT.ReportModule.ReportModuleManager
{
   public interface IReportModuleCloseManager
   {
      Task CloseWorkspaceInstanceById(Id workspaceId);
      Task CloseAllWorkspaceInstances();
      bool AnyUnsavedChangeOnApplication();
   }
}
