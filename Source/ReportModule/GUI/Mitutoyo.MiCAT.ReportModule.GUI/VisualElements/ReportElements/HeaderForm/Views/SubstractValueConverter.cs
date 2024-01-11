// <copyright file="SubstractValueConverter.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Windows.Data;

namespace Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements.HeaderForm.Views
{
   [ExcludeFromCodeCoverage]
   public class SubstractValueConverter : IValueConverter
   {
      public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
      {
         if (value == null)
            return 0;

         if (parameter == null)
            parameter = 0;

         double minuend;
         double substrahend;

         if (double.TryParse(value.ToString(), out minuend) && double.TryParse(parameter.ToString(), out substrahend))
         {
            return minuend - substrahend;
         }

         return 0;
      }

      public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
      {
         if (value == null)
            return 0;

         if (parameter == null)
            parameter = 0;

         double difference;
         double substrahend;

         if (double.TryParse(value.ToString(), out difference) && double.TryParse(parameter.ToString(), out substrahend))
         {
            return difference + substrahend;
         }

         return 0;
      }
   }
}
