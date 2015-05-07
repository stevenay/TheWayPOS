using DevExpress.Mvvm.UI.Interactivity;
using DevExpress.Xpf.WindowsUI;
using DevExpress.Xpf.Core.Native;
using System.Windows.Shapes;
using System.Windows;

namespace TheWayPOS.WPF_UI.Common.View
{
    public class AppBarCancelClosingBehavior: Behavior<AppBar> {
        protected override void OnAttached() {
            base.OnAttached();
            AssociatedObject.Closing += OnAssociatedObjectClosing;
            AssociatedObject.Loaded += AssociatedObject_Loaded;
        }

        void AssociatedObject_Loaded(object sender, RoutedEventArgs e) {
            //foreach(var item in AssociatedObject.Items) {
            //    if(item is AppBarSeparator) {
            //        var separator = item as AppBarSeparator;
            //        LayoutHelper.FindElementByType<Rectangle>(separator).Height = AssociatedObject.IsCompact ? 40 : 60;
            //    }
            //}
        }

        protected override void OnDetaching() {
            base.OnDetaching();
            AssociatedObject.Closing -= OnAssociatedObjectClosing;
            AssociatedObject.Loaded -= AssociatedObject_Loaded;
        }

        void OnAssociatedObjectClosing(object sender, AppBarEventArgs e) {
            e.Cancel = true;
        }
    }
}
