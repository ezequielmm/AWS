// <copyright file="DragDeltaEventToDragCommandArgsValueConverter.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using System.Globalization;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements;

namespace Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.Containers.ReportElementContainer
{
   public class DragDeltaEventToDragCommandArgsValueConverter : IValueConverter
   {
      public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
      {
         var eventArgs = (DragDeltaEventArgs)value;

         return new DragCommandArgs
         {
            HorizontalDelta = (int)eventArgs.HorizontalChange,
            VerticalDelta = (int)eventArgs.VerticalChange,
         };
      }

      public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
      {
         throw new NotImplementedException();
      }
   }
}
