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
    public class ProductCategoryCollectionViewModel : CollectionViewModel<ProductCategoryViewModel, MyModel.ProductCategory, int>, ISupportFiltering<Entities.ProductCategory>
    {
        #region Fields

        private ProductCategoryManager _businessLogic;

        public virtual ICustomFilterContainerService CustomFilterContainerService { get { return null; } }
        public FilterTreeViewModel<MyModel.ProductCategory> FilterTreeViewModel { get; set; }

        #endregion

        #region Constructors

		public ProductCategoryCollectionViewModel()
		{
			_businessLogic = new ProductCategoryManager();

            // I think this feature isn't needed for this entity.
            //FilterTreeViewModel = ViewModelSource.Create(() => new FilterTreeViewModel<MyModel.Product>(new ProductsFilterModelSettings()));
            //FilterTreeViewModel.ChangeViewModel(this);

			// FilterTreeViewModel.ActiveFilterChanged += FilterTreeViewModel_ActiveFilterChanged;
		}

		public ProductCategoryCollectionViewModel(DevMvvm.IDocumentManagerService dms) : this()
		{
			base.DocumentManagerService = dms;
		}
        public ProductCategoryCollectionViewModel(DevMvvm.IDocumentManagerService dms, DevMvvm.INavigationService ns)
            : this(dms)
		{
			base.NavigationService = ns;
		}

		#endregion

        #region Properties

        // the selected product (for showing orders for that Product)
        private ProductCategoryViewModel _selectedProduct = null;
        public ProductCategoryViewModel SelectedProduct
        {
            get { return _selectedProduct; }
            set
            {
                if (_selectedProduct == value)
                {
                    return;
                }
                _selectedProduct = value;
                OnPropertyChanged("SelectedProductCategory");
            }
        }

        #endregion

        #region Methods for Commands

        public override void New(ProductCategoryViewModel newVm)
        {
            ProductCategoryViewModel pvm = new ProductCategoryViewModel(new MyModel.ProductCategory(), this.Entities, NavigationService, DocumentManagerService, MyModel.Mode.Add);
            base.New(pvm);
        }

        #endregion

        #region CollectionViewModel Abstract Methods Implementation

        public override string entitiesName
        {
            get { return "ProductCategory"; }
        }
        protected override int GetPrimaryKey(ProductCategoryViewModel vm)
        {
            return vm.ProductCategoryCode;
        }
        protected override ObservableCollection<ProductCategoryViewModel> GetEntities()
        {
            if (entities == null)
            {
                entities = new ObservableCollection<ProductCategoryViewModel>();
                entities.Clear();

                var pcList = _businessLogic.ProductCategoryList();

                foreach (MyModel.ProductCategory item in pcList)
                {
                    ProductCategoryViewModel pvm = new ProductCategoryViewModel(item, _businessLogic, base.NavigationService, base.DocumentManagerService);
                    pvm.Mode = Mode.Edit;

                    entities.Add(pvm);
                }
            }

            return entities;
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
