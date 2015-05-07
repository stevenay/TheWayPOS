using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.ComponentModel;
using System.Linq;
using TheWayPOS.BL;
using TheWayPOS.BL.Validator;
using TheWayPOS.Entities;
using TheWayPOS.WPF_UI.Common.ViewModel;
using DevMvvm = DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;

namespace TheWayPOS.WPF_UI.ViewModel
{
	public class ProductViewModel : SingleObjectViewModelBase<Product, int>, IDataErrorInfo
	{
		#region Fields
		private Product _entity;
		private ProductManager _businessLogic;
		private IValidator<Product> _businessValidator;

		private bool childViewModelChangedData;
        private ObservableCollection<ProductViewModel> _pvmCollection;
		#endregion

		#region Constructors
		private ProductViewModel()
		{
			EntityName = "Product";
		}

		// BusinessLogic Constructor Version
		public ProductViewModel(Entities.Product p, ProductManager ProdMngr) : this()
		{
			this._entity = p;
			base.Entity = _entity;
			this._businessLogic = ProdMngr;

            // this should be controlled Dependency Injection in the future
            this._businessValidator = new ProductValidator(_entity);

			// copy the current value so in case cancel you can undo
			// Now it is not needed anymore because I use INotifyDirtyData Interface
			// this.originalValue = (ProductViewModel)this.MemberwiseClone();
		}
		public ProductViewModel(Entities.Product p, ProductManager pm, DevMvvm.INavigationService ns)
			: this (p, pm)
		{
			base.NavigationService = ns;
		}
		public ProductViewModel(Entities.Product p, ProductManager pm, DevMvvm.INavigationService ns, DevMvvm.IDocumentManagerService ds)
			: this(p, pm, ns)
		{
			base.DocumentManagerService = ds;
		}

		// Without BusinessLogic Constructor Version
		public ProductViewModel(Entities.Product p)
			: this()
		{
			this._entity = p;
			base.Entity = _entity;
			this._businessLogic = new ProductManager(false);

            // this should be controlled Dependency Injection in the future
            this._businessValidator = new ProductValidator(this._entity);

			// copy the current value so in case cancel you can undo
			// Now it is not needed anymore because I use INotifyDirtyData Interface
			// this.originalValue = (ProductViewModel)this.MemberwiseClone();
		}
		public ProductViewModel(Entities.Product p, DevMvvm.INavigationService ns)
			: this(p)
		{
			base.NavigationService = ns;
		}
		public ProductViewModel(Entities.Product p, DevMvvm.INavigationService ns, DevMvvm.IDocumentManagerService ds)
			: this(p, ns)
		{
			base.DocumentManagerService = ds;
		}
		public ProductViewModel(Entities.Product p, DevMvvm.INavigationService ns, DevMvvm.IDocumentManagerService ds, Entities.Mode mode)
			: this(p, ns)
		{
			this.Mode = mode;
			base.DocumentManagerService = ds;
		}
        public ProductViewModel(Product p, ObservableCollection<ProductViewModel> pvmCollection, DevMvvm.INavigationService ns, DevMvvm.IDocumentManagerService ds, Entities.Mode mode)
            : this(p, ns)
        {
            _pvmCollection = pvmCollection;
            this.Mode = mode;
            base.DocumentManagerService = ds;
        }
		#endregion

		#region Properties
		// View Related Services
		[ServiceProperty(SearchMode = DevMvvm.ServiceSearchMode.PreferLocal)]
		protected virtual DevMvvm.IDialogService dialogService { get { return GetService<DevMvvm.IDialogService>(); } set { } }

		// Business Domain Properties
        public override Mode Mode
        {
            get { return _entity.mode; }
            set
            {
                if (value == _entity.mode)
                    return;

                // base.CheckDataChange(value);
                _entity.mode = value;
                base.OnPropertyChanged();
            }
        }
		public int ProductCode{
			get { return _entity.product_code; }
			set
			{
				if (value == _entity.product_code)
					return;

				base.CheckDataChange(value);
				_entity.product_code = value;
				base.OnPropertyChanged();
			}
		}
		public string ProductName
		{
			get { return _entity.product_name; }
			set
			{
				if (_entity.product_name == value)
					return;

				if (this.Mode == Mode.Edit)
					base.CheckDataChange(value);

				_entity.product_name = value;
				base.OnPropertyChanged();
			}
		}
		public int BuyingPrice
		{
			get { return _entity.buying_price; }
			set {
				if (_entity.buying_price == value)
					return;

				if (this.Mode == Mode.Edit)
					base.CheckDataChange(value);

				_entity.buying_price = value;
				OnPropertyChanged();
			}
		}
		public decimal? DiscountPercentage
		{
			get { return _entity.discount_percentage ?? 0; }
			set
			{
				if (_entity.discount_percentage == value)
					return;

				if (this.Mode == Mode.Edit)
					base.CheckDataChange(value);

				_entity.discount_percentage = value;
				OnPropertyChanged();
			}
		}
		public int? RetailPrice
		{
			get { return _entity.retail_price; }
			set
			{
				if (_entity.retail_price == value)
					return;

				if (this.Mode == Mode.Edit)
					base.CheckDataChange(value);

				_entity.retail_price = value;
				OnPropertyChanged();
			}
		}

		private string _supplierName;
		public string SupplierName
		{
			get { return GetSupplierName(); }
			set
			{
				if (_supplierName == value)
					return;

				if (this.Mode == Mode.Edit)
					base.CheckDataChange(value);

				_supplierName = value;
				base.OnPropertyChanged();
			}
		}

		public int SupplierCode
		{
			get { return _entity.supplier_code ?? 0; }
			set
			{
				if (_entity.supplier_code == value)
					return;

				if (this.Mode == Mode.Edit)
					base.CheckDataChange(value);

				var selectedSupplier = Suppliers.Where(s => s.supplier_code == value).SingleOrDefault();
				if (selectedSupplier != null)
				{
					SupplierName = selectedSupplier.supplier_name;
					_entity.supplier_code = value;
				}

				base.OnPropertyChanged();
				
				//}
				//else
				//{
				//    throw new ArgumentException("လူကြီးမင်း ရွေးချယ်လိုက်သော Supplier သည် မှားယွင်းနေပါသည်။");
				//}
			}
		}
		public int ProductCategoryCode
		{
			get { return _entity.product_category_code ?? 0; }
			set
			{
				if (_entity.product_category_code == value)
					return;

				if (this.Mode == Mode.Edit)
					base.CheckDataChange(value);

				_entity.product_category_code = value;
				base.OnPropertyChanged();
			}
		}
		public int BuyingPriceUmCode
		{
			get { return _entity.buying_price_um_code ?? 0; }
			set
			{
				_entity.buying_price_um_code = value;
				OnPropertyChanged("BuyingPriceUmCode");
			}
		}

		// Lookup (Linked) Entities
		private List<Supplier> _suppliers = null;
		public List<Supplier> Suppliers
		{
			get { return GetSuppliers(); }
			set
			{
				_suppliers = value;
				OnPropertyChanged("Suppliers");
			}
		}

		private List<ProductCategory> _categories = null;
		public List<ProductCategory> ProductCategories
		{
			get { return GetCategories(); }
			set
			{
				_categories = value;
				OnPropertyChanged("ProductCategories");
			}
		}

		private List<Um> _ums = null;
		public List<Um> Ums
		{
			get { return GetUms(); }
			set
			{
				_ums = value;
				OnPropertyChanged("Ums");
			}
		}

		private ProductUmCollectionViewModel _productUmViewModelCollection;
		public ProductUmCollectionViewModel ProductUmViewModelCollection
		{
			get { return GetProductUmViewModelCollection(); }
			set
			{
				_productUmViewModelCollection = value;
				OnPropertyChanged("ProductUmViewModelCollection");
			}
		}
		#endregion

		#region Commands

		#endregion	

		#region FunctionsforCommands

		// For Showing Message Box
        // public void ShowMessageBox(string text)
        // {
        //     messageBoxService.Show(text, "The Way POS System", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.None, System.Windows.MessageBoxResult.OK);
        // }  
		
		// Functions for Commands
		// Override Functions from SingleObjectViewModelBase
		public override void Update()
		{
			if (this.Mode == Mode.Add) // if adding a Produc
			{
				_businessLogic.Add(_entity);

				foreach (var pu in _productUmViewModelCollection.Entities)
					pu.Add(_entity.product_code);

				_businessLogic.FinishBusinessTransaction();
				this.Mode = Mode.Edit;

                if (_pvmCollection != null)
                    _pvmCollection.Add(this);

                ShowMessageBox("New Product Successfully Added.");
			}
			else if (this.Mode == Mode.Edit) // if editing a Product
			{
				if (!base.HasChangedData && !this.childViewModelChangedData)
					return;

				if (base.HasChangedData)
					_businessLogic.Update(_entity);

				if (this.childViewModelChangedData)
					foreach (var pu in _productUmViewModelCollection.Entities)
						pu.Update();

				_businessLogic.FinishBusinessTransaction();
				base.ClearChangedData();
				// _businessLogic.FinishBusinessTransaction();

				ShowMessageBox("Data Successfully Saved.");
			}
		}
		protected override bool CanUpdate()
		{
			if (this.Mode == Mode.Add)
				return String.IsNullOrEmpty(_error) && (_productUmViewModelCollection.ProductUmApplyCount > 0);
			else // if (this.Mode == Mode.Edit)
				return String.IsNullOrEmpty(_error) && (base.HasChangedData || this.childViewModelChangedData) && (!_productUmViewModelCollection.ErrorFlag);
		}

		// Actually it's not the Delete Method
		// It's just Deactivate method
		public override void Delete()
		{
			_businessLogic.Deactivate(this._entity);
			base.ClearChangedData();
		}

		public override void Cancel()
		{
			this.ChildEntitiesUndo();
			base.Cancel();
		}

		public override bool CanUndo()
		{
			return base.HasChangedData || this.childViewModelChangedData;
		}

		public override void Back()
		{
			this.ChildEntitiesUndo();
			base.Back();
		}

		public void ChildEntitiesUndo()
		{
			// for Undo Child Objects
			if (this.childViewModelChangedData)
			{
				foreach (var item in ProductUmViewModelCollection.Entities)
				{
					item.Undo();
				}
			}
		}
		#endregion

		#region SingleObjectViewModelBase Implementation

		protected override int GetPrimaryKey()
		{
			return _entity.product_code;
		}

		#endregion

		#region IDataErrorInfoImplementation (or) ValidationRules
		string IDataErrorInfo.this[string columnName]
		{
			get
			{
				_error = null;

				if (columnName == "ProductName")
				{
					if (ProductName == null)  //must have an product name
						_error = "Please enter a Product Name";
					else
						_error = _businessValidator.ValidateProperty(columnName, ProductName);
				}
				else if (columnName == "BuyingPrice")
				{
					if (this.BuyingPrice < 1)  //if not integer
						_error = "Buying Price must be at least 1 (ကျပ်)";
					else
						_error = _businessValidator.ValidateProperty(columnName, this.BuyingPrice);
				}
				else if (columnName == "SupplierCode")
				{
					if (this.SupplierCode < 1)  //if null
						_error = "Please, choose one Supply Company.";
					else
						_error = _businessValidator.ValidateProperty(columnName, this.SupplierCode);
				}
				else if (columnName == "ProductCategoryCode")
				{
					if (this.ProductCategoryCode < 1)  //if null
						_error = "Please, choose one Product Category.";
					else
						_error = _businessValidator.ValidateProperty(columnName, this.ProductCategoryCode);
				}
				else if (columnName == "BuyingPriceUmCode")
				{
					if (this.BuyingPriceUmCode < 1)  //if null
						_error = "Please, choose one Price based Um.";
					else
						_error = _businessValidator.ValidateProperty(columnName, this.BuyingPriceUmCode);
				}
				else
				{
					_error = _businessValidator.ValidateProperty(columnName, base.GetPropertyValue(columnName));
				}

				// Dirty the commands registered with CommandManager,
				// such as our Save command, so that they are queried
				// to see if they can execute now.
				CommandManager.InvalidateRequerySuggested();

				return _error;
			}
		}
		string IDataErrorInfo.Error
		{
			get { return string.Empty; }
		}
		#endregion

		#region HelperMethods
		internal string GetSupplierName()
		{
			if (string.IsNullOrEmpty(_supplierName))
				_supplierName = _entity.Supplier.supplier_name;

			return _supplierName;
		}
		internal List<Supplier> GetSuppliers()
		{
			if (_suppliers == null)
			{
				_suppliers = new List<Supplier>();
				_suppliers.Clear();

				SupplierManager sm = new SupplierManager();
				var supplierList = sm.SupplierList();
				_suppliers = supplierList;
			}

			return _suppliers;
		}
		internal List<ProductCategory> GetCategories()
		{
			if (_categories == null)
			{
				_categories = new List<ProductCategory>();
				_categories.Clear();

				ProductCategoryManager pcm = new ProductCategoryManager();
				var cateList = pcm.ProductCategoryList();
				_categories = cateList;
			}

			return _categories;
		}
		internal List<Um> GetUms()
		{
			if (_ums == null)
			{
				_ums = new List<Um>();
				_ums.Clear();

				UmManager umm = new UmManager();

				var umList = umm.UmList();
				_ums = umList;
			}

			return _ums;
		}
		internal ProductUmCollectionViewModel GetProductUmViewModelCollection()
		{
			if (_productUmViewModelCollection == null) {
				_productUmViewModelCollection = new ProductUmCollectionViewModel(base.DocumentManagerService, base.NavigationService, dialogService, this);
				
				// Initialize and pass some data to child object
				_productUmViewModelCollection.Ums = this.Ums;
				_productUmViewModelCollection.ValueChanged += ChildViewModelChanged;
			}

			return _productUmViewModelCollection;
		}
		#endregion

		#region EventRelatedMethods
		private void ChildViewModelChanged(object sender, EventArgs e)
		{
			if (this.Mode == Mode.Edit)
			{
				var puvm = (ProductUmViewModel)sender;
				this.childViewModelChangedData = puvm.HasChangedData;
			}
		}
		#endregion

		#region ProductUmErrors
		
		#endregion
	}
}
