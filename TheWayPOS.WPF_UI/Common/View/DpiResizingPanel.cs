using System.Windows;
using System.Windows.Controls;
using System.Reflection;
using System.Windows.Media;

namespace TheWayPOS.WPF_UI.Common.View
{
    public class DpiResizingPanel : ContentControl
    {
        const double defaultDpi = 96d;
        public DpiResizingPanel()
        {
            ResizeByDpi();
        }

        static double GetDpiXFactor() { return GetDpiFactor("DpiX"); }

        static double GetDpiYFactor() { return GetDpiFactor("Dpi"); }

        static double GetDpiFactor(string propName)
        {
            var dpiProperty = typeof(SystemParameters).GetProperty(propName, BindingFlags.NonPublic | BindingFlags.Static);
            var dpi = (int)dpiProperty.GetValue(null, null);
            return dpi / defaultDpi;
        }

        static double CorrectDpiFactor(double factor)
        {
            return factor > 1.5 ? 1.5 : factor;
        }

        void ResizeByDpi()
        {
            if (SystemParameters.PrimaryScreenHeight > 1500 && SystemParameters.PrimaryScreenWidth > 2000)
                return;
            var dpiXFactor = CorrectDpiFactor(GetDpiXFactor());
            var dpiYFactor = CorrectDpiFactor(GetDpiYFactor());
            LayoutTransform = new ScaleTransform(1 / dpiXFactor, 1 / dpiYFactor);
            float touchScaleFactor, fontSize;
            DevExpress.DevAV.Common.Utils.DeviceDetector.SuggestHybridDemoParameters(out touchScaleFactor, out fontSize);
            FontSize = 12 * dpiXFactor;
        }
    }
}
