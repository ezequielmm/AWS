// <copyright file="StateChangeEventArgsExt.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Collections.Generic;
using System.Linq;
using Mitutoyo.MiCAT.ReportModule.Domain.Visibility;
using Telerik.Windows.Controls.Docking;

namespace Mitutoyo.MiCAT.ReportModule.GUI.MainWindow
{
   public static class StateChangeEventArgsExt
   {
      private static IDictionary<string, ViewElement> _views = new Dictionary<string, ViewElement>
      {
         { "PlansViewPane", ViewElement.Plans },
         { "RunsViewPane", ViewElement.Runs }
      };

      public static IEnumerable<ViewElement> GetHiddenViews(this StateChangeEventArgs args)
      {
         return args
            .Panes
            .Where(p => p.IsHidden)
            .Select(p => _views[p.Name]);
      }
   }
}
