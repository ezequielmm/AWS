// <copyright file="CustomHeadersFiltersChangeEventArgs.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Windows;
using Telerik.Windows.Controls;

namespace Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements.Table.Views.CustomGridView
{
   public class CustomHeadersFiltersChangeEventArgs : RoutedEventArgs
   {
      public GridViewColumn Column { get; }
      public InteractiveReportGridView.DescriptorOption Descriptor { get; }

      public CustomHeadersFiltersChangeEventArgs(RoutedEvent routedEvent, GridViewColumn column,
         InteractiveReportGridView.DescriptorOption descriptor) : base(routedEvent)
      {
         this.Column = column;
         this.Descriptor = descriptor;
      }
   }
}
