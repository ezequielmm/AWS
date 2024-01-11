// <copyright file="IDialogService.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Dialog
{
   public interface IDialogService
   {
      void ShowError(Exception ex);
      void ShowInfo(string message);
      void ShowDialog<TViewModel>(TViewModel vm, DialogType type) where TViewModel : IDialogRequestClose;
   }
}