// <copyright file="RelayCommand.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using System.Windows.Input;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Utilities
{
   public class RelayCommand<T> : ICommand
   {
      public Action<T> ExecuteHandler { get; set; }
      public Func<T, bool> CanExecuteHandler { get; set; }

      public event EventHandler CanExecuteChanged;

      public RelayCommand(Action<T> executeMethod)
      {
         ExecuteHandler = executeMethod;
      }

      public RelayCommand(Action<T> executeMethod, Func<T, bool> canExecuteMethod)
      {
         ExecuteHandler = executeMethod;
         CanExecuteHandler = canExecuteMethod;
      }

      public bool CanExecute(object parameter)
      {
         return (this.CanExecuteHandler == null ? true : this.CanExecuteHandler((T)parameter));
      }

      public void Execute(object parameter)
      {
         this.ExecuteHandler((T)parameter);
      }

      public void RaiseCanExecuteChanged()
      {
         CanExecuteChanged?.Invoke(this, System.EventArgs.Empty);
      }
   }
}
