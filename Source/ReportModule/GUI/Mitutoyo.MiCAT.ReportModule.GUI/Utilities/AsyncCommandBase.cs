// <copyright file="AsyncCommandBase.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Utilities
{
   public abstract class AsyncCommandBase : IAsyncCommand
   {
      private readonly Func<bool> _canExecuteMethod;

      protected AsyncCommandBase(Func<bool> canExecuteMethod)
      {
         _canExecuteMethod = canExecuteMethod;
      }

      public bool CanExecute(object parameter)
      {
         return _canExecuteMethod == null || _canExecuteMethod();
      }

      public async void Execute(object parameter)
      {
         await ExecuteAsync(parameter);
      }

      public async Task ExecuteAsync(object parameter)
      {
         RaiseCanExecuteChanged();
         await ExecuteCommand(parameter);
         RaiseCanExecuteChanged();
      }

      public event EventHandler CanExecuteChanged
      {
         add { CommandManager.RequerySuggested += value; }
         remove { CommandManager.RequerySuggested -= value; }
      }

      private void RaiseCanExecuteChanged()
      {
         CommandManager.InvalidateRequerySuggested();
      }

      protected abstract Task ExecuteCommand(object parameter);
   }
}
