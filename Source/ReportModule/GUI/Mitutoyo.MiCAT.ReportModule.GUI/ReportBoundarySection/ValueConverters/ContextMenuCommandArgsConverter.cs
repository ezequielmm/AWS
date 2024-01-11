// <copyright file="ContextMenuCommandArgsConverter.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using Telerik.Windows.Controls;

namespace Mitutoyo.MiCAT.ReportModule.GUI.ReportBoundarySection.ValueConverters
{
   public class ContextMenuCommandArgsConverter : IValueConverter
   {
      public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
      {
         var result = new ContextMenuCommandArgs();

         var eventArgs = value as RoutedEventArgs;
         var menuItem = eventArgs.Source as RadMenuItem;
         var contextMenu = menuItem.Menu as RadContextMenu;

         result.Position = new Point((int)contextMenu.MousePosition.X, (int)contextMenu.MousePosition.Y);

         return result;
      }

      public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
      {
         throw new NotImplementedException();
      }
   }
}