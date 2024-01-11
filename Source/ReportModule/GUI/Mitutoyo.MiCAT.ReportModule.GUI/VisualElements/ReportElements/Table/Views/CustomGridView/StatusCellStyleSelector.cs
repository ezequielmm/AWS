// <copyright file="StatusCellStyleSelector.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Mitutoyo.MiCAT.ReportModule.GUI.Properties;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements.Table.ViewModels;
using Telerik.Windows.Controls.GridView;

namespace Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements.Table.Views.CustomGridView
{
   public class StatusCellStyleSelector : StyleSelector
   {
      public override Style SelectStyle(object item, DependencyObject container)
      {
         var invalidStyle = new Style() { TargetType = typeof(GridViewCell) };
         invalidStyle.Setters.Add(new Setter()
         { Property = GridViewCell.BackgroundProperty, Value = new SolidColorBrush(Color.FromRgb(248, 210, 201)) });
         var commonStyle = new Style() { TargetType = typeof(GridViewCell) };
         commonStyle.Setters.Add(new Setter()
         { Property = GridViewCell.BackgroundProperty, Value = new SolidColorBrush(Colors.White) });
         var passStyle = new Style() { TargetType = typeof(GridViewCell) };
         passStyle.Setters.Add(new Setter()
         { Property = GridViewCell.BackgroundProperty, Value = new SolidColorBrush(Color.FromRgb(202, 231, 218)) });
         var failStyle = new Style() { TargetType = typeof(GridViewCell) };
         failStyle.Setters.Add(new Setter()
         { Property = GridViewCell.BackgroundProperty, Value = new SolidColorBrush(Color.FromRgb(248, 210, 201)) });
         var style = commonStyle;
         if (item is VMEvaluatedCharacteristic ec)
         {
            if (ec.Status == Resources.Pass)
               style = passStyle;
            if (ec.Status == Resources.Fail)
               style = failStyle;
            if (ec.Status == Resources.Invalid)
               style = invalidStyle;
         }
         return style;
      }
   }
}