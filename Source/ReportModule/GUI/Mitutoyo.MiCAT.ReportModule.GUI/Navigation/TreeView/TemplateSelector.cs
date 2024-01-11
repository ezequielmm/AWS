// <copyright file="TemplateSelector.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Windows;
using System.Windows.Controls;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Navigation.TreeView
{
   public class TemplateSelector : DataTemplateSelector
   {
      public HierarchicalDataTemplate PartTempalte { get; set; }
      public DataTemplate PlanTemplate { get; set; }
      public DataTemplate RunsTemplate { get; set; }

      public override DataTemplate SelectTemplate(object item, DependencyObject container)
      {
         base.SelectTemplate(item, container);

         if (item is VmPartTreeViewItem) return PartTempalte;
         if (item is VmPlanTreeViewItem) return PlanTemplate;
         if (item is VmRunTreeViewItem) return RunsTemplate;

         return null;
      }
   }
}
