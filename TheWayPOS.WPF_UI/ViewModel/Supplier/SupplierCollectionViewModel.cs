using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
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
	public class SupplierCollectionViewModel : CollectionViewModel<SupplierViewModel, MyModel.Supplier, int>, ISupportFiltering<MyModel.Supplier>
	{
		#region Fields

		private SupplierManager _businessLogic;

		public virtual ICustomFilterContainerService CustomFilterContainerService { get { return null; } }
		public FilterTreeViewModel<MyModel.Supplier> FilterTreeViewModel { get; set; }

		#endregion

		#region Constructors

		public SupplierCollectionViewModel()
		{
			_businessLogic = new SupplierManager();

            FilterTreeViewModel = ViewModelSource.Create(() => new FilterTreeViewModel<MyModel.Supplier>(new SuppliersFilterModelSettings()));
            FilterTreeViewModel.ChangeViewModel(this);

			// FilterTreeViewModel.ActiveFilterChanged += FilterTreeViewModel_ActiveFilterChanged;
		}
		public SupplierCollectionViewModel(DevMvvm.IDocumentManagerService dms) : this()
		{
			base.DocumentManagerService = dms;
		}
        public SupplierCollectionViewModel(DevMvvm.IDocumentManagerService dms, DevMvvm.INavigationService ns)
            : this(dms)
		{
			base.NavigationService = ns;
		}

		#endregion

        #region Properties

        // the selected product (for showing orders for that Product)
        private SupplierViewModel _selectedSupplier = null;
        public SupplierViewModel SelectedSupplier
        {
            get { return _selectedSupplier; }
            set
            {
                if (_selectedSupplier == value)
                {
                    return;
                }
                _selectedSupplier = value;
                OnPropertyChanged("SelectedSupplier");
            }
        }

        #endregion

        #region Methods for Commands

        public override void New(SupplierViewModel newVm)
        {
            SupplierViewModel svm = new SupplierViewModel(new MyModel.Supplier(), this.entities, NavigationService, DocumentManagerService, MyModel.Mode.Add);
            base.New(svm);
        }

        #endregion

        #region CollectionViewModel Abstract Methods Implementation

        public override string entitiesName
        {
            get { return "Supplier"; }
        }
        protected override int GetPrimaryKey(SupplierViewModel vm)
        {
            return vm.SupplierCode;
        }
        protected override ObservableCollection<SupplierViewModel> GetEntities()
        {
            if (entities == null)
            {
                entities = new ObservableCollection<SupplierViewModel>();
                entities.Clear();

                var supList = _businessLogic.SupplierList();

                SupplierCategoryManager scm = new SupplierCategoryManager();
                var supCateList = scm.SupplierCategoryList();

                foreach (MyModel.Supplier item in supList)
                {
                    SupplierViewModel svm = new SupplierViewModel(item, _businessLogic, base.NavigationService, base.DocumentManagerService);
                    svm.Mode = Mode.Edit;
                    svm.SupplierCategories = supCateList;

                    entities.Add(svm);
                }
            }

            return entities;
        }

        #endregion

        #region FilterTreeModelPage InnerClass for Supplier Category

        private class SuppliersFilterModelSettings : IFilterTreeModelPageSpecificSettings
        {

            public string CustomFiltersSetting
            {
                get;
                set;
            }

            public string GroupFiltersSetting
            {
                get;
                set;
            }

            public virtual IEnumerable<FilterItemBase> CreateInitialCustomFilters(FilterTreeViewModelBase creator)
            {
                // FilterItemBase, I'm not sure it is the right way to do it
                return new FilterItemBase[] { };
            }
            public IEnumerable<FilterItemBase> CreateStaticFilters(FilterTreeViewModelBase creator)
            {
                return new FilterItemBase[] {
					creator.CreateStaticFilterItem("All", null).SetIcon("Resources/Products/All.png"),
					creator.CreateStaticFilterItem("တစ်ဆင့်ပြန်ရောင်းသူ", new OperandProperty("SupplierCategoryName") == "တစ်ဆင့်ပြန်ရောင်းသူ").SetIcon("Resources/Products/VideoPlayers.png"),
                    creator.CreateStaticFilterItem("ကုမ္ပဏီများ", new OperandProperty("SupplierCategoryName") == "ကုန်ထုတ်လုပ်သူ").SetIcon("Resources/Products/Automation.png")
				};
            }
        }

        #endregion

        #region ISupportFiltering Implementation

        public int GetCount(CriteriaOperator filterCriteria)
        {
            return base.GetCount(GetExpression(filterCriteria));
        }

        #endregion
    }
}
