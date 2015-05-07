using System;
using System.Windows.Data;

namespace TheWayPOS.WPF_UI.Common.Util
{
    public class ProductUmApplyCountConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return false;

            int applyCount;
            if (int.TryParse(value.ToString(), out applyCount))
            {
                if (applyCount > 0)
                    return true;
                else
                    return false;
            }

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
