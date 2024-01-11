// <copyright file="CloseController.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using Mitutoyo.MiCAT.ReportModuleApp.Utilities;

namespace Mitutoyo.MiCAT.ReportModuleApp.Controllers
{
   public class CloseController : ICloseController
   {
      private readonly ICloseService _closeService;
      public CloseController(ICloseService closeService)
      {
         _closeService = closeService;
      }
      public void CloseWorkspace()
      {
         _closeService.CloseInstance();
      }
   }
}
