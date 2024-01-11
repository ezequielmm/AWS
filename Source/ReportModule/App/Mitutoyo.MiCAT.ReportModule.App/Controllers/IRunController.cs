// <copyright file="IRunController.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using System.Threading.Tasks;
using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.ReportModule.Domain.Data;

namespace Mitutoyo.MiCAT.ReportModuleApp.Controllers
{
   public interface IRunController
   {
      Task DeleteRun(Guid runId);
      ISnapShot CompleteRunSelectionOnAppState(ISnapShot snapShot, Guid runIdRequested, RunData runData);
   }
}
