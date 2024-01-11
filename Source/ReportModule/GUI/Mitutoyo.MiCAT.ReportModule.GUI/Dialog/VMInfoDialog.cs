// <copyright file="VMInfoDialog.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using System.Windows.Input;
using Mitutoyo.MiCAT.ReportModule.GUI.Strings;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Dialog
{
   public class VMInfoDialog : IDialogRequestClose
   {
      public VMInfoDialog(string title, string message)
      {
         Title = title;
         Message = message;
         CloseCommand = new ActionCommand(p => CloseRequested?.Invoke(this, new DialogEventArgs(false)));
      }

      public event EventHandler<DialogEventArgs> CloseRequested;
      public string Message { get; }
      public string Title { get; }
      public ICommand CloseCommand { get; }

      public static explicit operator VMInfoDialog(string messageInfo)
      {
         return new VMInfoDialog(StringFinder.FindLocalizedString(messageInfo + "Title"), StringFinder.FindLocalizedString(messageInfo + "Message"));
      }
   }
}
