using System;
using System.Windows.Data;

namespace TheWayPOS.WPF_UI.Common.Util
{
    public class NullStringToMessage : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null || String.IsNullOrWhiteSpace(value.ToString()))
                return "အချက်အလက် ထည့်သွင်းထားခြင်း မရှိပါ";
            else
                return value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }
    }
}
