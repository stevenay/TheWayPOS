using System.Collections.Generic;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Editors.Filtering;
using DevExpress.Xpf.Grid;
using TheWayPOS.WPF_UI.Common.ViewModel;

namespace TheWayPOS.WPF_UI.Service {
    public interface ICustomFilterContainerService {
        void UpdateFilterColumns(CustomFilterViewModel viewModel);
    }
    public class CustomFilterContainerService : ServiceBase, ICustomFilterContainerService {
        GridControl Grid { get { return (GridControl)AssociatedObject; } }
        public void UpdateFilterColumns(CustomFilterViewModel viewModel) {
            viewModel.FilterColumns = Grid.FilteredComponent.CreateFilterColumnCollection();
        }
    }
}
