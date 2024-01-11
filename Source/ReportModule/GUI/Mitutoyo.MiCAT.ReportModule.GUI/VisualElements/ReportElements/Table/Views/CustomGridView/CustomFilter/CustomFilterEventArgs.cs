// <copyright file="CustomFilterEventArgs.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Collections.Generic;
using System.Windows;
using static Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements.Table.Views.CustomGridView.CustomFilter.VMSelectDistinctFilterControl;

namespace Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements.Table.Views.CustomGridView.CustomFilter
{
   public class CustomFilterEventArgs : RoutedEventArgs
   {
      public IEnumerable<SelectDistinctFilterOption> SelectedItems { get; }

      public CustomFilterEventArgs(IEnumerable<SelectDistinctFilterOption> selectedItems) : base()
      {
         SelectedItems = selectedItems;
      }
   }
}
