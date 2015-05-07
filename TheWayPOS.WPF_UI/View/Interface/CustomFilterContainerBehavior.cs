using System.Windows;
using System.Windows.Input;
using DevExpress.Mvvm.UI.Interactivity;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.PivotGrid;

namespace TheWayPOS.WPF_UI.Common.Interface {
    public class CustomFilterContainerBehavior : Behavior<DataViewBase> {
        public ICommand ShowFilterCommand {
            get { return (ICommand)GetValue(ShowFilterCommandProperty); }
            set { SetValue(ShowFilterCommandProperty, value); }
        }
        public static readonly DependencyProperty ShowFilterCommandProperty =
            DependencyProperty.Register("ShowFilterCommand", typeof(ICommand), typeof(CustomFilterContainerBehavior), new PropertyMetadata(null));

        protected override void OnAttached() {
            base.OnAttached();
            AssociatedObject.FilterEditorCreated += View_FilterEditorCreated;
        }
        protected override void OnDetaching() {
            AssociatedObject.FilterEditorCreated -= View_FilterEditorCreated;
        }
        void View_FilterEditorCreated(object sender, FilterEditorEventArgs e) {
            e.Handled = true;
            if(ShowFilterCommand != null) {
                ShowFilterCommand.Execute(null);
            }
        }
    }

    //public class PivotCustomFilterContainerBehavior : Behavior<CustomPivotGridControl> {

    //    public static readonly DependencyProperty ShowFilterCommandProperty =
    //        DependencyProperty.Register("ShowFilterCommand", typeof(ICommand), typeof(PivotCustomFilterContainerBehavior), new PropertyMetadata(null));
    //    public static readonly DependencyProperty PrefilterForceClosedProperty =
    //        DependencyProperty.Register("PrefilterForceClosed", typeof(bool), typeof(PivotCustomFilterContainerBehavior), new PropertyMetadata(false,
    //            (d, e) => ((PivotCustomFilterContainerBehavior)d).OnPrefilterForceClosedChanged(e)));

    //    public ICommand ShowFilterCommand {
    //        get { return (ICommand)GetValue(ShowFilterCommandProperty); }
    //        set { SetValue(ShowFilterCommandProperty, value); }
    //    }
    //    public bool PrefilterForceClosed {
    //        get { return (bool)GetValue(PrefilterForceClosedProperty); }
    //        set { SetValue(PrefilterForceClosedProperty, value); }
    //    }
    //    protected override void OnAttached() {
    //        base.OnAttached();
    //        AssociatedObject.PrefilterEditorCreated += View_FilterEditorCreated;
    //    }
    //    protected override void OnDetaching() {
    //        base.OnDetaching();
    //        AssociatedObject.PrefilterEditorCreated -= View_FilterEditorCreated;
    //    }
    //    void View_FilterEditorCreated(object sender, PivotFilterEditorEventArgs e) {
    //        e.Handled = true;
    //        if(ShowFilterCommand != null) {
    //            ShowFilterCommand.Execute(null);
    //        }
    //    }
    //    protected virtual void OnPrefilterForceClosedChanged(DependencyPropertyChangedEventArgs e) {
    //        if(!(bool)e.OldValue && (bool)e.NewValue)
    //            AssociatedObject.PrefilterClosed();
    //    }
    //}
}
