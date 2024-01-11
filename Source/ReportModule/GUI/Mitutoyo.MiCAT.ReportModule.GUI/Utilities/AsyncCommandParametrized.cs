﻿// <copyright file="AsyncCommandParametrized.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using System.Threading.Tasks;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Utilities
{
   public class AsyncCommand<TParameter> : AsyncCommandBase
   {
      private readonly Func<TParameter, Task> _command;

      public AsyncCommand(Func<TParameter, Task> command) : this(command, null)
      {
      }

      public AsyncCommand(Func<TParameter, Task> command, Func<bool> canExecuteMethod) : base(canExecuteMethod)
      {
         _command = command;
      }

      protected override async Task ExecuteCommand(object parameter)
      {
         await _command((TParameter)parameter);
      }
   }
}
