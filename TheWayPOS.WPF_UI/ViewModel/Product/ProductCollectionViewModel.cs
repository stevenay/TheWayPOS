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
	public class ProductCollectionViewModel : CollectionViewModel<ProductViewModel, MyModel.Product, int>, ISupportFiltering<MyModel.Product>
	{
		#region Fields

		private ProductManager _businessLogic;

		public virtual ICustomFilterContainerService CustomFilterContainerService { get { return null; } }
		public FilterTreeViewModel<MyModel.Product> FilterTreeViewModel { get; set; }

		#endregion
		
		#region Constructors

		public ProductCollectionViewModel()
		{
			_businessLogic = new ProductManager();

			FilterTreeViewModel = ViewModelSource.Create(() => new FilterTreeViewModel<MyModel.Product>(new ProductsFilterModelSettings()));
			FilterTreeViewModel.ChangeViewModel(this);
		}

		public ProductCollectionViewModel(DevMvvm.IDocumentManagerService dms) : this()
		{
			base.DocumentManagerService = dms;
		}
		public ProductCollectionViewModel(DevMvvm.IDocumentManagerService dms, DevMvvm.INavigationService ns) : this(dms)
		{
			base.NavigationService = ns;
		}

		#endregion

		#region Properties

		// the selected product (for showing orders for that Product)
		private ProductViewModel _selectedProduct = null;
		public ProductViewModel SelectedProduct
		{
            get { return _selectedProduct; }
			set
			{
                if (_selectedProduct == value)
				{
					return;
				}
                _selectedProduct = value;
				OnPropertyChanged("SelectedProduct");
			}
		}

		#endregion

        #region Methods for Commands

        public override void New(ProductViewModel newVm)
        {
            ProductViewModel pvm = new ProductViewModel(new MyModel.Product(), this.Entities, NavigationService, DocumentManagerService, MyModel.Mode.Add);
            base.New(pvm);
        }

        #endregion

        #region CollectionViewModel Abstract Methods Implementation

        public override string entitiesName
		{
			get { return "Product"; }
		}
		protected override int GetPrimaryKey(ProductViewModel vm)
		{
			return vm.ProductCode;
		}
		protected override ObservableCollection<ProductViewModel> GetEntities()
		{
			if (entities == null)
			{
				entities = new ObservableCollection<ProductViewModel>();
				entities.Clear();

				var productList = _businessLogic.ProductFullInfoList();
				Stopwatch watch = Stopwatch.StartNew();

				SupplierManager sm = new SupplierManager();
				var supplierList = sm.SupplierList();

				UmManager um = new UmManager();
				var umList = um.UmList();

				ProductCategoryManager pcm = new ProductCategoryManager();
				var pcList = pcm.ProductCategoryList();

				foreach (MyModel.Product item in productList)
				{
					ProductViewModel pvm = new ProductViewModel(item, _businessLogic, base.NavigationService, base.DocumentManagerService);
					pvm.Mode = Mode.Edit;
					pvm.Suppliers = supplierList;
					pvm.Ums = umList;
					pvm.ProductCategories = pcList;

					entities.Add(pvm);
				}

				// _businessLogic.FinishBusinessTransaction();

				watch.Stop();
				EllapsedTime = (int)watch.Elapsed.TotalSeconds;
			}
			
			return entities;
		}

		#endregion

		#region InnerClass for Product Category

		private class ProductsFilterModelSettings : IFilterTreeModelPageSpecificSettings {

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

			public virtual IEnumerable<FilterItemBase> CreateInitialCustomFilters(FilterTreeViewModelBase creator) {
				// FilterItemBase, I'm not sure it is the right way to do it
				return new FilterItemBase[] {};
			}
			public IEnumerable<FilterItemBase> CreateStaticFilters(FilterTreeViewModelBase creator) {
				return new FilterItemBase[] {
					creator.CreateStaticFilterItem("All", null).SetIcon("Resources/Products/All.png"),
					creator.CreateStaticFilterItem("Popular", new OperandProperty("SupplierName") == "Popular").SetIcon("Resources/Products/VideoPlayers.png"),
					//creator.CreateStaticFilterItem("Automation", new OperandProperty("Category") == new ConstantValue(ProductCategory.Automation)).SetIcon("Resources/Products/Automation.png"),
					//creator.CreateStaticFilterItem("Monitors", new OperandProperty("Category") == new ConstantValue(ProductCategory.Monitors)).SetIcon("Resources/Products/Monitors.png"),
					//creator.CreateStaticFilterItem("Projectors", new OperandProperty("Category") == new ConstantValue(ProductCategory.Projectors)).SetIcon("Resources/Products/Projectors.png"),
					//creator.CreateStaticFilterItem("Televisions", new OperandProperty("Category") == new ConstantValue(ProductCategory.Televisions)).SetIcon("Resources/Products/TVs.png"),
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
