// <copyright file="ActionCallerFake.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Mitutoyo.MiCAT.ReportModuleApp.Utilities;

namespace Mitutoyo.MiCAT.ReportModule.Setup.Export.Fakes
{
   [ExcludeFromCodeCoverage]
   public class ActionCallerFake : IActionCaller
   {
      public void RunUIThreadAction(Action func)
      {
         func.Invoke();
      }

      public Task RunUserActionAsync(Func<Task> func)
      {
         return func.Invoke();
      }

      public void RunUserAction(Action func)
      {
         func.Invoke();
      }
   }
}
