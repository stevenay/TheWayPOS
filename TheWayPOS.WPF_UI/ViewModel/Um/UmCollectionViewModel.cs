using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using System.Diagnostics;
using DevExpress.Data.Filtering;
using TheWayPOS.WPF_UI.Common.ViewModel;
using TheWayPOS.WPF_UI.Service;
using TheWayPOS.BL;
using TheWayPOS.Entities;
using MyModel = TheWayPOS.Entities;
using DevExpress.Mvvm.POCO;
using DevMvvm = DevExpress.Mvvm;

namespace TheWayPOS.WPF_UI.ViewModel
{
    public class UmCollectionViewModel : CollectionViewModel<UmViewModel, MyModel.Um, int>, ISupportFiltering<MyModel.Um>
    {
        #region Fields

        private UmManager _businessLogic;

        public virtual ICustomFilterContainerService CustomFilterContainerService { get { return null; } }
        public FilterTreeViewModel<MyModel.Um> FilterTreeViewModel { get; set; }

        #endregion

        #region Constructors

		public UmCollectionViewModel()
		{
			_businessLogic = new UmManager();

			//FilterTreeViewModel = ViewModelSource.Create(() => new FilterTreeViewModel<MyModel.Um>(new UmsFilterModelSettings()));
			//FilterTreeViewModel.ChangeViewModel(this);

			// FilterTreeViewModel.ActiveFilterChanged += FilterTreeViewModel_ActiveFilterChanged;
		}
		public UmCollectionViewModel(DevMvvm.IDocumentManagerService dms) : this()
		{
			base.DocumentManagerService = dms;
		}
        public UmCollectionViewModel(DevMvvm.IDocumentManagerService dms, DevMvvm.INavigationService ns)
            : this(dms)
		{
			base.NavigationService = ns;
		}

		#endregion

        #region Properties

        // the selected product (for showing orders for that Product)
        private UmViewModel _selectedUm = null;
        public UmViewModel SelectedUm
        {
            get { return _selectedUm; }
            set
            {
                if (_selectedUm == value)
                {
                    return;
                }
                _selectedUm = value;
                OnPropertyChanged("SelectedUm");
            }
        }

        #endregion

        #region Methods for Commands

        public override void New(UmViewModel newVm)
        {
            UmViewModel umvm = new UmViewModel(new MyModel.Um(), this.Entities, NavigationService, DocumentManagerService, MyModel.Mode.Add);
            base.New(umvm);
        }

        #endregion

        #region CollectionViewModel Abstract Methods Implementation

        public override string entitiesName
        {
            get { return "Um"; }
        }
        protected override int GetPrimaryKey(UmViewModel vm)
        {
            return vm.UmCode;
        }
        protected override ObservableCollection<UmViewModel> GetEntities()
        {
            if (entities == null)
            {
                entities = new ObservableCollection<UmViewModel>();
                entities.Clear();

                var umList = _businessLogic.UmList();

                foreach (MyModel.Um item in umList)
                {
                    UmViewModel uvm = new UmViewModel(item, _businessLogic, base.NavigationService, base.DocumentManagerService);
                    uvm.Mode = Mode.Edit;

                    entities.Add(uvm);
                }
            }

            return entities;
        }

        #endregion

        #region InnerClass for Um Filtering (there's no filtering)

        #endregion

        #region ISupportFiltering Implementation

        public int GetCount(CriteriaOperator filterCriteria)
        {
            return base.GetCount(GetExpression(filterCriteria));
        }

        #endregion
    }
}
