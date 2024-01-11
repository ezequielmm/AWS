// <copyright file="CustomFilterInfoEventArgs.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Windows;

namespace Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements.Table.Views.CustomGridView
{
   public class CustomFilterInfoEventArgs : RoutedEventArgs
   {
      public string ColumnName { get; set; }
      public string[] SelectedValues { get; set; }

      public CustomFilterInfoEventArgs(RoutedEvent e, string columnName, string[] selectedValues) : base(e)
      {
         ColumnName = columnName;
         SelectedValues = selectedValues;
      }
   }
}
