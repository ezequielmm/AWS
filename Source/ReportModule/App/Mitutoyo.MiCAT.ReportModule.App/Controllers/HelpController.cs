// <copyright file="HelpController.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using Mitutoyo.MiCAT.Common.Help;
using Mitutoyo.MiCAT.ReportModuleApp.Utilities;

namespace Mitutoyo.MiCAT.ReportModuleApp.Controllers
{
   public class HelpController : IHelpController
   {
      private readonly IHelpService _helpService;

      public HelpController(IHelpService helpService)
      {
         _helpService = helpService;
      }

      public void OpenHelp(HelpTopicPath path)
      {
         _helpService.OpenHelp(path);
      }
   }
}
