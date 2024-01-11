// <copyright file="ActiveWindowProvider.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Windows;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Providers
{
   [ExcludeFromCodeCoverage]
   public class ActiveWindowProvider : IActiveWindowProvider
   {
      public Window GetActiveWindow()
      {
         var activeWindow = Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);

         if (activeWindow != null) return activeWindow;

         return Application.Current.MainWindow;
      }
   }
}
