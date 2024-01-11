// <copyright file="IPlanController.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using System.Threading.Tasks;

namespace Mitutoyo.MiCAT.ReportModuleApp.Controllers
{
   public interface IPlanController
   {
      Task SelectPlan(Guid planId);
      Task SelectPart(Guid partId);
      Task DeletePlan(Guid planId);
      Task DeletePart(Guid partId);
      Task RefreshMessurementDataSourceList();
   }
}
