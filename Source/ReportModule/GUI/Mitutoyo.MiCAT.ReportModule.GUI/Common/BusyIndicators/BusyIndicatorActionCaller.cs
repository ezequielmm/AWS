// <copyright file="BusyIndicatorActionCaller.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using System.Threading.Tasks;
using Mitutoyo.MiCAT.ReportModuleApp.Utilities;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Common.BusyIndicators
{
   public class BusyIndicatorActionCaller : IActionCaller
   {
      private readonly IBusyIndicator _busyIndicator;

      public BusyIndicatorActionCaller(IBusyIndicator busyIndicator)
      {
         _busyIndicator = busyIndicator;
      }

      public void RunUserAction(Action func)
      {
         _busyIndicator.SetIsBusyTrueAndWaitForWPFRender();
         func();
         _busyIndicator.SetIsBusyFalse();
      }

      public async Task RunUserActionAsync(Func<Task> func)
      {
         _busyIndicator.SetIsBusyTrueAndWaitForWPFRender();
         await func();
         _busyIndicator.SetIsBusyFalse();
      }

      public void RunUIThreadAction(Action func)
      {
         _busyIndicator.SetIsBusyTrueUntilUIIsIdle();
         func();
      }
   }
}
