// <copyright file="ISelectedComponentController.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.ReportModule.Domain.Components;

namespace Mitutoyo.MiCAT.ReportModuleApp.Controllers
{
   public interface ISelectedComponentController
   {
      void SetSelected(IItemId<IReportComponent> reportComponentIdSelected);
      void UnSelectAll();
   }
}