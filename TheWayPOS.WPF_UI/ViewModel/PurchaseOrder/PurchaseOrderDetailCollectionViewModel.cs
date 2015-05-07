using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using System.Collections.ObjectModel;
using TheWayPOS.WPF_UI.Common.ViewModel;
using TheWayPOS.WPF_UI.Service;
using TheWayPOS.BL;
using TheWayPOS.Entities;
using MyModel = TheWayPOS.Entities;
using DevMvvm = DevExpress.Mvvm;
using TheWayPOS.WPF_UI.Common;


namespace TheWayPOS.WPF_UI.ViewModel
{
	public class PurchaseOrderDetailCollectionViewModel : CollectionViewModel<PurchaseOrderDetailViewModel, MyModel.PurchaseOrderDetail, int>
	{
		#region Fields

		private PurchaseOrderManager _businessLogic;

		public virtual ICustomFilterContainerService CustomFilterContainerService { get { return null; } }
		public FilterTreeViewModel<MyModel.PurchaseOrderDetail> FilterTreeViewModel { get; set; }

        private PurchaseOrderHeaderViewModel _parentEntity;

		#endregion

		#region Constructors

        public PurchaseOrderDetailCollectionViewModel(PurchaseOrderHeaderViewModel pohvm)
		{
			_parentEntity = pohvm;
			_businessLogic = new PurchaseOrderManager(false);
		}
		public PurchaseOrderDetailCollectionViewModel(DevMvvm.IDocumentManagerService dms, PurchaseOrderHeaderViewModel pohvm)
			: this(pohvm)
		{
            base.DocumentManagerService = dms;
		}

        public PurchaseOrderDetailCollectionViewModel(DevMvvm.IDocumentManagerService dms, DevMvvm.INavigationService ns, DevMvvm.IDialogService ds, PurchaseOrderHeaderViewModel pohvm)
			: this(dms, pohvm)
		{
			// this.dialogService = ds;
			base.NavigationService = ns;
		}

		#endregion

		#region Properties

		// the selected product (for showing orders for that Product)
		private PurchaseOrderDetailViewModel _selectedPurchaseOrderDetail = null;
		public PurchaseOrderDetailViewModel SelectedPurchaseOrderDetail
		{
            get { return _selectedPurchaseOrderDetail; }
			set
			{
                if (_selectedPurchaseOrderDetail == value)
				{
					return;
				}
                _selectedPurchaseOrderDetail = value;
				OnPropertyChanged("SelectedPurchaseOrderDetail");
			}
		}

        // Error Flag
        private bool _errorFlag = false;
        public bool ErrorFlag
        {
            get
            {
                return _errorFlag;
            }
            set
            {
                if (_errorFlag == value)
                    return;

                _errorFlag = value;
                OnPropertyChanged("ErrorFlag");
            }
        }

		#endregion

        #region Commands

        #endregion

        #region Methods for Commands

        public override void New(PurchaseOrderDetailViewModel newVm)
        {
            PurchaseOrderDetailViewModel pvm = new PurchaseOrderDetailViewModel(new MyModel.PurchaseOrderDetail(), this.Entities, NavigationService, DocumentManagerService, MyModel.Mode.Add);
            base.New(pvm);
        }

        // AddNewOrderDetail Method
        public PurchaseOrderDetailViewModel AddNewOrderDetail()
        {
            var selectedSupplier = this._parentEntity.Suppliers.Where(s => s.supplier_code == this._parentEntity.SupplierCode).SingleOrDefault();
            
            // Check Supplier Code is Selected
            if (selectedSupplier != null)
            {
                var purchaseOrderHeader = this._parentEntity.Entity;
                PurchaseOrderDetail purchaseOrderDetail = new PurchaseOrderDetail() { purchase_order_code = purchaseOrderHeader.purchase_order_code, PurchaseOrderHeader = purchaseOrderHeader };

                if (selectedSupplier.SupplierCategory.supplier_category_name == "ကုန်ထုတ်လုပ်သူ")
                {
                    purchaseOrderDetail.supplier_code = purchaseOrderHeader.supplier_code;   
                }

                PurchaseOrderDetailViewModel podvm = new PurchaseOrderDetailViewModel(purchaseOrderDetail, this._businessLogic, this.NavigationService, this.DocumentManagerService);
                this.Entities.Add(podvm);

                return podvm;
            }

            return null;
        }

        // Change product supplier if selected supplier is changed
        public void ReMatchProductsbySupplier()
        {
            var selectedSupplier = this._parentEntity.Suppliers.Where(s => s.supplier_code == this._parentEntity.SupplierCode).SingleOrDefault();

            if (selectedSupplier.SupplierCategory.supplier_category_name == "ကုန်ထုတ်လုပ်သူ")
            {
                foreach (PurchaseOrderDetailViewModel item in this.entities)
                {
                    if (item.ProductCode <= 0)
                        continue;

                    // Search the same named product although different supplier
                    var pm = new ProductManager();
                    var sameNameProduct = pm.GetSameNameProductbySupplierCode(item.ProductCode, this._parentEntity.SupplierCode);

                    if (sameNameProduct != null)
                    {
                        item.ProductCode = sameNameProduct.product_code;
                    }
                    else
                    {
                        item.ReMatchProductConflict = true;
                    }
                }
            }
        }

        #endregion

        #region CollectionViewModel Abstract Methods Implementation
        public override string entitiesName
        {
            get { return "PurchaseOrderDetail"; }
        }
        protected override int GetPrimaryKey(PurchaseOrderDetailViewModel vm)
        {
            return vm.PurchaseOrderDetailId;
        }
        protected override ObservableCollection<PurchaseOrderDetailViewModel> GetEntities()
        {
            if (entities == null)
            {
                entities = new ObservableCollection<PurchaseOrderDetailViewModel>();
                entities.Clear();

                var purchaseOrderManager = new PurchaseOrderManager();

                if (this._parentEntity.Mode == MyModel.Mode.Edit)
                {
                    var poduList = purchaseOrderManager.PurchaseOrderDetailListbyPurhcaesOrderHeaderId(_parentEntity.PurchaseOrderCode);

                    foreach (MyModel.PurchaseOrderDetail item in poduList)
                    {
                        PurchaseOrderDetailViewModel podvm = new PurchaseOrderDetailViewModel(item, this.entities, _businessLogic, base.NavigationService, base.DocumentManagerService, Mode.Edit);

                        entities.Add(podvm);
                    }
                }

                // productUmManager.FinishBusinessTransaction();

            }

            return entities;
        }
        #endregion
    }
}
