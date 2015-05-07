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
    public class PurchaseOrderDetailViewModel : SingleObjectViewModelBase<PurchaseOrderDetail, int>, IDataErrorInfo
    {
        #region Fields

        private PurchaseOrderDetail _entity;
        private PurchaseOrderManager _businessLogic;
        private IValidator<PurchaseOrderDetail> _businessValidator;

        private ObservableCollection<PurchaseOrderDetailViewModel> _podvmCollection;
        private bool childViewModelChangedData;

        #endregion

        #region Constructors
		private PurchaseOrderDetailViewModel()
		{
			EntityName = "ProductCategory";
            this.ReMatchProductConflict = true;
		}
        private void InitializeViewModel(PurchaseOrderDetail pod, PurchaseOrderManager poManager)
        {
            this._entity = pod;
            base.Entity = _entity;
            this._businessLogic = poManager;

            this._businessValidator = new PurchaseOrderDetailValidator(_entity);
        }

		// BusinessLogic Constructor Version
		public PurchaseOrderDetailViewModel(Entities.PurchaseOrderDetail pod, PurchaseOrderManager poManager) : this()
		{
			this.InitializeViewModel(pod, poManager);
		}
		public PurchaseOrderDetailViewModel(Entities.PurchaseOrderDetail pod, PurchaseOrderManager poManager, DevMvvm.INavigationService ns)
			: this (pod, poManager)
		{
			base.NavigationService = ns;
		}
		public PurchaseOrderDetailViewModel(Entities.PurchaseOrderDetail pod, PurchaseOrderManager poManager, DevMvvm.INavigationService ns, DevMvvm.IDocumentManagerService ds)
			: this(pod, poManager, ns)
		{
			base.DocumentManagerService = ds;
		}
        public PurchaseOrderDetailViewModel(Entities.PurchaseOrderDetail pod, ObservableCollection<PurchaseOrderDetailViewModel> podvmCollection, PurchaseOrderManager poManager, DevMvvm.INavigationService ns, DevMvvm.IDocumentManagerService ds, Entities.Mode mode)
            : this(pod, poManager, ns)
        {
            _podvmCollection = podvmCollection;
            this.Mode = mode;
            base.DocumentManagerService = ds;
        }

		// Without BusinessLogic Constructor Version
		public PurchaseOrderDetailViewModel(Entities.PurchaseOrderDetail pod)
			: this()
		{
			this._entity = pod;
			base.Entity = _entity;
			this._businessLogic = new PurchaseOrderManager(false);

            // this should be controlled Dependency Injection in the future
            this._businessValidator = new PurchaseOrderDetailValidator(_entity);

			// copy the current value so in case cancel you can undo
			// Now it is not needed anymore because I use INotifyDirtyData Interface
			// this.originalValue = (ProductViewModel)this.MemberwiseClone();
		}
		public PurchaseOrderDetailViewModel(Entities.PurchaseOrderDetail pod, DevMvvm.INavigationService ns)
			: this(pod)
		{
			base.NavigationService = ns;
		}
		public PurchaseOrderDetailViewModel(Entities.PurchaseOrderDetail pod, DevMvvm.INavigationService ns, DevMvvm.IDocumentManagerService ds)
			: this(pod, ns)
		{
			base.DocumentManagerService = ds;
		}
        public PurchaseOrderDetailViewModel(Entities.PurchaseOrderDetail pod, ObservableCollection<PurchaseOrderDetailViewModel> podvmCollection, DevMvvm.INavigationService ns, DevMvvm.IDocumentManagerService ds, Entities.Mode mode)
			: this(pod, ns)
		{
            _podvmCollection = podvmCollection;
			this.Mode = mode;
			base.DocumentManagerService = ds;
		}
		#endregion

        #region System Related Properties

        // View Related Services
        [ServiceProperty(SearchMode = DevMvvm.ServiceSearchMode.PreferLocal)]
        protected virtual DevMvvm.IDialogService dialogService { get { return GetService<DevMvvm.IDialogService>(); } set { } }

        #endregion

        #region Properties
        
        // Business Domain Properties
        public int PurchaseOrderDetailId
        {
            get { return _entity.purchase_order_detail_id; }
            set
            {
                if (value == _entity.purchase_order_detail_id)
                    return;

                base.CheckDataChange(value);
                _entity.purchase_order_detail_id = value;
                base.OnPropertyChanged();
            }
        }
        public int PurchaseOrderCode
        {
            get { return _entity.purchase_order_code; }
            set
            {
                if (_entity.purchase_order_code == value)
                    return;

                base.CheckDataChange(value);
                _entity.purchase_order_code = value;
                base.OnPropertyChanged();
            }
        }
        public int ProductCode
        {
            get { return _entity.product_code; }
            set
            {
                if (_entity.product_code == value)
                    return;

                base.CheckDataChange(value);

                _entity.product_code = value;

                _purchaseOrderDetailUmCollection = null;
                PurchaseOrderDetailUmCollection = GetPurchaseOrderDetailUmCollection();
                
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
            }
        }
        public int BuyingPrice
        {
            get { return _entity.buying_price; }
            set
            {
                if (_entity.buying_price == value)
                    return;

                base.CheckDataChange(value);
                _entity.buying_price = value;
                base.OnPropertyChanged();
            }
        }
        public int Quantity
        {
            get { return _entity.quantity; }
            set
            {
                if (_entity.quantity == value)
                    return;

                base.CheckDataChange(value);
                _entity.quantity = value;
                base.OnPropertyChanged();
            }
        }
        public int? LineTotal
        {
            get { return _entity.line_total ?? 0; }
            set
            {
                if (_entity.line_total == value)
                    return;

                base.CheckDataChange(value);
                _entity.line_total = value;
                base.OnPropertyChanged();
            }
        }
        public decimal? DiscountPercentage
        {
            get { return _entity.discount_percentage ?? 0; }
            set
            {
                if (_entity.discount_percentage == value)
                    return;

                base.CheckDataChange(value);
                _entity.discount_percentage = value;
                base.OnPropertyChanged();
            }
        }

        public bool ReMatchProductConflict { get; set; }
        public string ReMatchProductConflictErrorMessage
        {
            get
            {
                return "ရွေးချယ်ထားသော Company တွင် ထို Product မရှိပါ။";
            }
        }

        private PurchaseOrderDetailUmCollectionViewModel _purchaseOrderDetailUmCollection;
        public PurchaseOrderDetailUmCollectionViewModel PurchaseOrderDetailUmCollection
        {
            get { return GetPurchaseOrderDetailUmCollection(); }
            set 
            {
                _purchaseOrderDetailUmCollection = value;
                OnPropertyChanged("PurchaseOrderDetailUmCollection");
            }
        }
        
        #endregion

        #region SingleObjectViewModel Implementation
        protected override int GetPrimaryKey()
        {
            return this.PurchaseOrderDetailId;
        }
        #endregion 

        #region Commands



        #endregion

        #region FunctionsforCommands
        // Functions for Commands
        // Override Functions from SingleObjectViewModelBase
        public override void Update()
        {
            if (this.Mode == Mode.Add) // if adding a PurchaseOrderDetail
            {
                _businessLogic.AddOrderDetail(_entity);

                this.Mode = Mode.Edit;

                if (_podvmCollection != null)
                    _podvmCollection.Add(this);
            }
            else if (this.Mode == Mode.Edit) // if editing a PurchaseOrderDetail
            {
                if (!base.HasChangedData)
                    return;

                if (base.HasChangedData)
                    _businessLogic.UpdateOrderDetail(_entity);
            }
        }

        protected override bool CanUpdate()
        {
            if (this.Mode == Mode.Add)
                return String.IsNullOrEmpty(_error);
            else // if (this.Mode == Mode.Edit)
                return String.IsNullOrEmpty(_error) && (base.HasChangedData);
        }

        // Actually it's not the Delete Method
        // It's just Deactivate method
        public override void Delete()
        {
            // this logic is the same as PurchaseOrder Delete Logic
            _businessLogic.DeleteOrderDetail(this._entity);
            base.ClearChangedData();
        }

        public override void Cancel()
        {
            this.ChildEntitiesUndo();
            base.Cancel();
        }

        public override bool CanUndo()
        {
            return base.HasChangedData;
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
            //    foreach (var item in this.PurchaseOrderDetailUmCollection.Entities)
            //    {
            //        item.Undo();
            //    }
            //}

            // this.Mode is add
            this.PurchaseOrderDetailUmCollection.Entities.Clear();
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

        #region HelperMethods

        internal PurchaseOrderDetailUmCollectionViewModel GetPurchaseOrderDetailUmCollection()
        {
            if (_purchaseOrderDetailUmCollection == null)
                _purchaseOrderDetailUmCollection = new PurchaseOrderDetailUmCollectionViewModel(base.DocumentManagerService, base.NavigationService, dialogService, this);

            return _purchaseOrderDetailUmCollection;
        }

        #endregion
    }
}
