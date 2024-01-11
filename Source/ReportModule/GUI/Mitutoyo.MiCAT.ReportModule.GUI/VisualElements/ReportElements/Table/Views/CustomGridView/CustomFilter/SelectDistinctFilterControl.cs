// <copyright file="SelectDistinctFilterControl.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Windows;
using Telerik.Windows.Controls.GridView;

namespace Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements.Table.Views.CustomGridView.CustomFilter
{
   public partial class SelectDistinctFilterControl : IFilteringControl
   {
      /// <summary>
      /// Gets or sets a value indicating whether the filtering is active.
      /// </summary>
      public bool IsActive
      {
         get { return (bool)GetValue(IsActiveProperty); }
         set { SetValue(IsActiveProperty, value); }
      }

      /// <summary>
      /// Identifies the <see cref="IsActive"/> dependency property.
      /// </summary>
      public static readonly DependencyProperty IsActiveProperty =
         DependencyProperty.Register(
            "IsActive",
            typeof(bool),
            typeof(SelectDistinctFilterControl),
            new System.Windows.PropertyMetadata(false));

      public void Prepare(Telerik.Windows.Controls.GridViewColumn column)
      {
      }
   }
}
