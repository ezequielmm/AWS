// <copyright file="MultiBoolToVisibleValueConverter.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using System.Windows;
using System.Windows.Data;

namespace Mitutoyo.MiCAT.ReportModule.GUI
{
   public class MultiBoolToVisibleValueConverter : IMultiValueConverter
   {
      public object Convert(object[] values, Type targetType, object parameter,
         System.Globalization.CultureInfo culture)
      {
         bool cb1 = (bool)values[0];
         bool cb2 = (bool)values[1];
         var behaviorPattern = parameter.ToString().Split('|');
         bool pattern1 = behaviorPattern[0] == "true"?true:false;
         bool pattern2 = behaviorPattern[1] == "true"?true :false;

         if (cb1 == pattern1 && cb2 == pattern2)
            return Visibility.Visible;
         return Visibility.Collapsed;
      }
      public object[] ConvertBack(object value, Type[] targetTypes, object parameter,
         System.Globalization.CultureInfo culture)
      {
         throw new NotImplementedException();
      }
   }
}
