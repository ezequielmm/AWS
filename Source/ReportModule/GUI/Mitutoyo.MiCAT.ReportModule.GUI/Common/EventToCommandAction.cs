// <copyright file="EventToCommandAction.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Common
{
   public class EventToCommandAction : TriggerAction<DependencyObject>
   {
      public static readonly DependencyProperty CommandProperty;
      public static readonly DependencyProperty EventArgsValueConverterProperty;

      static EventToCommandAction()
      {
         CommandProperty = DependencyProperty.Register(
            "Command",
            typeof(ICommand),
            typeof(EventToCommandAction),
            new PropertyMetadata(null));

         EventArgsValueConverterProperty = DependencyProperty.Register(
            "EventArgsValueConverter",
            typeof(IValueConverter),
            typeof(EventToCommandAction),
            new PropertyMetadata(null));
      }

      public ICommand Command
      {
         get => (ICommand)GetValue(CommandProperty);
         set => SetValue(CommandProperty, value);
      }

      public IValueConverter EventArgsValueConverter
      {
         get => (IValueConverter)GetValue(EventArgsValueConverterProperty);
         set => SetValue(EventArgsValueConverterProperty, value);
      }

      protected override void Invoke(object parameter)
      {
         if (Command is null) return;

         var commandArgs = GetCommandArgs(parameter);

         if (Command.CanExecute(commandArgs))
            Command.Execute(commandArgs);
      }

      private object GetCommandArgs(object parameter)
      {
         if (EventArgsValueConverter is null)
            return parameter;

         return EventArgsValueConverter.Convert(parameter, typeof(object), null, CultureInfo.CurrentCulture);
      }
   }
}
