// <copyright file="StatusCellMergedStyleSelector.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Mitutoyo.MiCAT.ReportModule.GUI.Properties;
using Telerik.Windows.Controls.GridView;

namespace Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements.Table.Views.CustomGridView
{
   public class StatusCellMergedStyleSelector : StyleSelector
   {
      public override Style SelectStyle(object item, DependencyObject container)
      {
         var commonStyle = new Style() { TargetType = typeof(GridViewMergedCell) };
         commonStyle.Setters.Add(new Setter()
         { Property = GridViewMergedCell.BackgroundProperty, Value = new SolidColorBrush(Colors.White) });
         commonStyle.Setters.Add(new Setter()
         { Property = GridViewMergedCell.ForegroundProperty, Value = new SolidColorBrush(Colors.Black) });

         var passStyle = new Style() { TargetType = typeof(GridViewMergedCell) };
         passStyle.Setters.Add(new Setter()
         { Property = GridViewMergedCell.BackgroundProperty, Value = new SolidColorBrush(Color.FromRgb(202, 231, 218)) });

         var failStyle = new Style() { TargetType = typeof(GridViewMergedCell) };
         failStyle.Setters.Add(new Setter()
         { Property = GridViewMergedCell.BackgroundProperty, Value = new SolidColorBrush(Color.FromRgb(248, 210, 201)) });

         var invalidStyle = new Style() { TargetType = typeof(GridViewMergedCell) };
         invalidStyle.Setters.Add(new Setter()
         { Property = GridViewMergedCell.BackgroundProperty, Value = new SolidColorBrush(Color.FromRgb(248, 210, 201)) });

         var style = commonStyle;
         if (item is MergedCellInfo cell)
         {
            if (cell.Value.ToString() == Resources.Pass)
               style = passStyle;
            if (cell.Value.ToString() == Resources.Fail)
               style = failStyle;
            if (cell.Value.ToString() == Resources.Invalid)
               style = invalidStyle;
         }

         return style;
      }
   }
}