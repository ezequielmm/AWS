// <copyright file="WindowExtensions.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using System.Windows;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Extensions
{
   [ExcludeFromCodeCoverage]
   public static class WindowExtensions
   {
      public static bool? ShowDialogCenteredOnOwnerOrScreen(this Window window)
      {
         var hasOwner = window.Owner != null;
         window.WindowStartupLocation = hasOwner ?
            WindowStartupLocation.CenterOwner :
            WindowStartupLocation.CenterScreen;

         return window.ShowDialog();
      }
   }
}
