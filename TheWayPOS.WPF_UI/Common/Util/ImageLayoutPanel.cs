using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace TheWayPOS.WPF_UI.Common.Util
{
    public class ImageLayoutPanel : Panel
    {
        double childHeight;

        protected override Size MeasureOverride(Size availableSize)
        {
            double childWith = 0.0;
            foreach (UIElement child in Children)
            {
                child.Measure(new Size(availableSize.Width, childHeight));
                if (child.DesiredSize.Width > childWith)
                    childWith = child.DesiredSize.Width;
            }
            return new Size(childWith, 0.0);
        }
        protected override Size ArrangeOverride(Size finalSize)
        {
            foreach (UIElement child in Children)
                child.Arrange(new Rect(new Point(), finalSize));
            double newChildHeight = finalSize.Height;
            if (newChildHeight != childHeight)
            {
                childHeight = newChildHeight;
                InvalidateMeasure();
            }
            return finalSize;
        }
    }
}
