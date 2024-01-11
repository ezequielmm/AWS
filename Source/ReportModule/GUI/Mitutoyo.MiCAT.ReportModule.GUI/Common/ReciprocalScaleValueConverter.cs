// <copyright file="ReciprocalScaleValueConverter.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using System.Globalization;
using System.Windows.Data;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Common
{
   public class ReciprocalScaleValueConverter : IValueConverter
   {
      private const int NumDecimalPlacesToRound = 3;
      public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
      {
         return Inverse(value);
      }

      public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
      {
         throw new NotImplementedException();
      }

      private object Inverse(object value)
      {
         return Math.Round(1 / (double)value, NumDecimalPlacesToRound, MidpointRounding.ToEven);
      }
   }
}