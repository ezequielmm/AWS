// <copyright file="ShellHelpService.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.Common.Help;
using Mitutoyo.MiCAT.ReportModuleApp.Utilities;

namespace Mitutoyo.MiCAT.ReportModule.Services
{
   public class ShellHelpService : IHelpService
   {
      private readonly IAppStateHistory _appStateHistory;
      private readonly IHelpTopicController _shellHelpTopicController;
      public ShellHelpService(
         IAppStateHistory appStateHistory,
         IHelpTopicController shellHelpTopicController)
      {
         _appStateHistory = appStateHistory;
         _shellHelpTopicController = shellHelpTopicController;
      }
      public void OpenHelp(HelpTopicPath path)
      {
         _appStateHistory.RunAsync((snapShot) => _shellHelpTopicController.AddHelpTopic(snapShot, HelpTopicPath.Home));
      }
   }
}
