// <copyright file="NegateBoolConverter.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Common
{
   public class NegateBoolConverter : IValueConverter
   {
      public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
      {
         return NegateValue(value);
      }

      public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
      {
         return NegateValue(value);
      }

      private object NegateValue(object value)
      {
         return (value is bool || value is bool?) ? !(bool)value : DependencyProperty.UnsetValue;
      }
   }
}
