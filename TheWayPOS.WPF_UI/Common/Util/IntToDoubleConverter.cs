using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Data;

namespace TheWayPOS.WPF_UI.Common.Util
{
    public class IntToDoubleConverter : IValueConverter
    {
        public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return System.Convert.ChangeType(value, typeof(double));
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new System.NotImplementedException();
        }
    }
}
