using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Mitutoyo.Micat.PdfExportDebugModule
{
   [ExcludeFromCodeCoverage]
   public class BoolToColorIndicationConverter : IValueConverter
   {
      public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
      {
         var isTrue = (bool)value;

         return isTrue ? Brushes.Green : Brushes.Red;
      }

      public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
      {
         throw new NotImplementedException();
      }
   }
}
