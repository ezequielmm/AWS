// <copyright file="VMErrorDialog.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using System.Windows.Input;
using Mitutoyo.MiCAT.ReportModule.Domain.DataResult;
using Mitutoyo.MiCAT.ReportModule.GUI.Properties;
using Mitutoyo.MiCAT.ReportModule.GUI.Strings;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Dialog
{
   public class VMErrorDialog : IDialogRequestClose
   {
      public VMErrorDialog(string title, string message)
      {
         Title = title;
         Message = message;
         CloseCommand = new ActionCommand(p => CloseRequested?.Invoke(this, new DialogEventArgs(false)));
         CloseButtonLabel = Resources.ResourceManager.GetString("OkButtonLabel");
      }

      public event EventHandler<DialogEventArgs> CloseRequested;
      public string Message { get; }
      public string Title { get; }
      public ICommand CloseCommand { get; }
      public string CloseButtonLabel { get; }

      public static explicit operator VMErrorDialog(ResultException ex)
      {
         return new VMErrorDialog(StringFinder.FindLocalizedString(ex.Key + "Title"), StringFinder.FindLocalizedString(ex.Key + "Message"));
      }
   }
}