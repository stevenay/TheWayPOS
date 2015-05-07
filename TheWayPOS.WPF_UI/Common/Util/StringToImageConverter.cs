using System;
using System.Linq;
using System.Windows.Data;
using System.Windows.Markup;
using System.Collections.Generic;
using System.Windows.Media.Imaging;

namespace TheWayPOS.WPF_UI.Common.Util {
    public class StringToImageConverter : MarkupExtension, IValueConverter {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            return new BitmapImage(new Uri(@"/TheWayPOS.WPF_UI;component/Images/" + value.ToString(), UriKind.Relative));
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            return null;
        }

        public override object ProvideValue(IServiceProvider serviceProvider) {
            return this;
        }
    }
}
