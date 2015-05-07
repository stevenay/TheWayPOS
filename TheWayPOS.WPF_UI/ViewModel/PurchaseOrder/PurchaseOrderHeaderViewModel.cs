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
    public class PurchaseOrderHeaderViewModel : SingleObjectViewModelBase<PurchaseOrderHeader, int>, IDataErrorInfo
    {
        #region Fields
        private PurchaseOrderHeader _entity;
        private PurchaseOrderManager _businessLogic;
        private IValidator<PurchaseOrderHeader> _businessValidator;

        private bool childViewModelChangedData;
        private ObservableCollection<PurchaseOrderHeaderViewModel> _pohvmCollection;
        #endregion

        #region Constructors
		private PurchaseOrderHeaderViewModel()
		{
			EntityName = "Product";
		}

		// BusinessLogic Constructor Version
		public PurchaseOrderHeaderViewModel(Entities.PurchaseOrderHeader poh, PurchaseOrderManager poMngr) : this()
		{
			this._entity = poh;
			base.Entity = _entity;
			this._businessLogic = poMngr;

            // this should be controlled Dependency Injection in the future
            this._businessValidator = new PurchaseOrderValidator(_entity);

			// copy the current value so in case cancel you can undo
			// Now it is not needed anymore because I use INotifyDirtyData Interface
			// this.originalValue = (ProductViewModel)this.MemberwiseClone();
		}
        public PurchaseOrderHeaderViewModel(Entities.PurchaseOrderHeader poh, PurchaseOrderManager poManager, DevMvvm.INavigationService ns)
			: this (poh, poManager)
		{
			base.NavigationService = ns;
		}
		public PurchaseOrderHeaderViewModel(Entities.PurchaseOrderHeader poh, PurchaseOrderManager poManager, DevMvvm.INavigationService ns, DevMvvm.IDocumentManagerService ds)
			: this(poh, poManager, ns)
		{
			base.DocumentManagerService = ds;
		}

		// Without BusinessLogic Constructor Version
		public PurchaseOrderHeaderViewModel(Entities.PurchaseOrderHeader poh)
			: this()
		{
			this._entity = poh;
			base.Entity = _entity;
			this._businessLogic = new PurchaseOrderManager();

            // this should be controlled Dependency Injection in the future
            this._businessValidator = new PurchaseOrderValidator(this._entity);

			// copy the current value so in case cancel you can undo
			// Now it is not needed anymore because I use INotifyDirtyData Interface
			// this.originalValue = (ProductViewModel)this.MemberwiseClone();
		}
		public PurchaseOrderHeaderViewModel(Entities.PurchaseOrderHeader poh, DevMvvm.INavigationService ns)
			: this(poh)
		{
			base.NavigationService = ns;
		}
		public PurchaseOrderHeaderViewModel(Entities.PurchaseOrderHeader poh, DevMvvm.INavigationService ns, DevMvvm.IDocumentManagerService ds)
			: this(poh, ns)
		{
			base.DocumentManagerService = ds;
		}
		public PurchaseOrderHeaderViewModel(Entities.PurchaseOrderHeader poh, DevMvvm.INavigationService ns, DevMvvm.IDocumentManagerService ds, Entities.Mode mode)
			: this(poh, ns)
		{
			this.Mode = mode;
			base.DocumentManagerService = ds;
		}
        public PurchaseOrderHeaderViewModel(Entities.PurchaseOrderHeader poh, ObservableCollection<PurchaseOrderHeaderViewModel> pohvmCollection, DevMvvm.INavigationService ns, DevMvvm.IDocumentManagerService ds, Entities.Mode mode)
            : this(poh, ns)
        {
            _pohvmCollection = pohvmCollection;
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
        public int PurchaseOrderCode
        {
            get { return _entity.purchase_order_code; }
            set
            {
                if (value == _entity.purchase_order_code)
                    return;

                base.CheckDataChange(value);
                _entity.purchase_order_code = value;
                base.OnPropertyChanged();
            }
        }
        public int SupplierCode
        {
            get { return _entity.supplier_code; }
            set
            {
                if (_entity.supplier_code == value)
                    return;

                base.CheckDataChange(value);
                _entity.supplier_code = value;
                base.OnPropertyChanged();

                // Load products after choose the Supplier
                this.SuppliedProducts = GetSuppliedProducts(_entity.supplier_code);
            }
        }
        public int ProductArrivalCode
        {
            get { return _entity.product_arrival_code; }
            set
            {
                if (_entity.product_arrival_code == value)
                    return;

                _entity.product_arrival_code = value;
                base.OnPropertyChanged();
            }
        }

        // Product Arrival Options
        public List<ProductArrival> _productArrivals = null;
        public List<ProductArrival> ProductArrivals
        {
            get { return GetProductArrivals(); }
            set
            {
                _productArrivals = value;
                OnPropertyChanged("ProductArrivals");
            }
        }

        // Lookup (Linked) Entities
        private List<Product> _suppliedProducts = null;
        public List<Product> SuppliedProducts
        {
            get { return _suppliedProducts; }
            set
            {
                if (_suppliedProducts == value)
                    return;

                _suppliedProducts = value;
                OnPropertyChanged("SuppliedProducts");
            }
        }

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

        private List<Supplier> _manufacturers = null;
        public List<Supplier> Manufacturers
        {
            get { return GetManufacturers(); }
            set
            {
                _manufacturers = value;
                OnPropertyChanged("Manufacturers");
            }
        }

        private PurchaseOrderDetailCollectionViewModel _purchaseOrderDetailCollection = null;
        public PurchaseOrderDetailCollectionViewModel PurchaseOrderDetailCollection
        {
            get { return GetPurchaseOrderDetailCollection(); }
            set
            {
                _purchaseOrderDetailCollection = value;
                OnPropertyChanged("PurchaseOrderDetails");
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
                ShowMessageBox("New Purchase Order Successfully Added.");
            }
            else if (this.Mode == Mode.Edit) // if editing a Product
            {
                ShowMessageBox("Data Successfully Saved.");
            }
        }
        protected override bool CanUpdate()
        {
            if (this.Mode == Mode.Add)
                return String.IsNullOrEmpty(_error) && (_purchaseOrderDetailCollection.Entities.Count > 0);
            else // if (this.Mode == Mode.Edit)
                return String.IsNullOrEmpty(_error) && (base.HasChangedData || this.childViewModelChangedData) && (!_purchaseOrderDetailCollection.ErrorFlag);
        }

        // Actually it's not the Delete Method
        // It's just Deactivate method
        public override void Delete()
        {
            // there is no Delete Metohd in Sale Invoice
            // to delete "Purchase Order"

            // first, we need to check "TransactionLog"
            // After this "Purchase Order", check it already happen "Sale"
            // if already happen "Sale", check the quantity become minus(-) if this Purchase Order Quantity is deleted
            // _businessLogic.Deactivate(this._entity);
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
            //if (this.childViewModelChangedData)
            //{
            //    foreach (var item in PurchaseOrderDetailCollection.Entities)
            //    {
            //        item.Undo();
            //    }
            //}

            // this.Mode is add
            if (this.Mode == Mode.Add)
            {
                this.PurchaseOrderDetailCollection.Entities.Clear();
            }
        }
        #endregion

        #region SingleObjectViewModelBase Implementation

        protected override int GetPrimaryKey()
        {
            return _entity.purchase_order_code;
        }

        #endregion

        #region IDataErrorInfoImplementation (or) ValidationRules
        string IDataErrorInfo.this[string columnName]
        {
            get
            {
                _error = null;
                _error = _businessValidator.ValidateProperty(columnName, base.GetPropertyValue(columnName));
                
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

        #region EventRelatedMethods
        private void ChildViewModelChanged(object sender, EventArgs e)
        {
            //if (this.Mode == Mode.Edit)
            //{
            //    var puvm = (ProductUmViewModel)sender;
            //    this.childViewModelChangedData = puvm.HasChangedData;
            //}
        }
        #endregion

        #region HelperMethods

        internal List<Product> GetSuppliedProducts(int supplierCode)
        {
            if (_suppliedProducts == null)
                _suppliedProducts = new List<Product>();

            ProductManager pm = new ProductManager();
            var productList = pm.ProductListbySupplierCode(supplierCode);
            return productList;
        }
        internal List<Product> GetSuppliedProducts()
        {
            if (_suppliedProducts == null)
            {
                _suppliedProducts = new List<Product>();
                _suppliedProducts.Clear();

                ProductManager pm = new ProductManager();
                var productList = pm.ProductList();
                _suppliedProducts = productList;
            }

            return _suppliedProducts;
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
        internal List<Supplier> GetManufacturers()
        {
            GetSuppliers();

            if (_manufacturers == null)
            {
                _manufacturers = new List<Supplier>();
                _manufacturers.Clear();

                var supplierList = this._suppliers.Where(s => s.SupplierCategory.supplier_category_name == "ကုန်ထုတ်လုပ်သူ").ToList();
                _manufacturers = supplierList;
            }

            return _manufacturers;
        }
        internal PurchaseOrderDetailCollectionViewModel GetPurchaseOrderDetailCollection()
        {
            if (_purchaseOrderDetailCollection == null)
            {
                _purchaseOrderDetailCollection = new PurchaseOrderDetailCollectionViewModel(base.DocumentManagerService, base.NavigationService, dialogService, this);

                // Initialize and pass some data to child object
                //_productUmViewModelCollection.Ums = this.Ums;
                //_productUmViewModelCollection.ValueChanged += ChildViewModelChanged;
            }

            return _purchaseOrderDetailCollection;
        }
        internal List<ProductArrival> GetProductArrivals()
        {
            if (_productArrivals == null)
            {
                _productArrivals = new List<ProductArrival>();
                _productArrivals.Clear();

                ProductArrivalManager pom = new ProductArrivalManager();
                var productArrivalList = pom.ProductArrivalList();
                _productArrivals = productArrivalList;
            }

            return _productArrivals;
        }

        #endregion
    }
}
