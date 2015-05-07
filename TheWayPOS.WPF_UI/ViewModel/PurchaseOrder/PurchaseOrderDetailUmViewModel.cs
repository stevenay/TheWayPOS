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
    public class PurchaseOrderDetailUmViewModel : SingleObjectViewModelBase<PurchaseOrderDetail_Ums, int>, IDataErrorInfo
    {
        #region Fields
        private PurchaseOrderDetail_Ums _entity;
        private PurchaseOrderManager _businessLogic;
        private IValidator<PurchaseOrderDetail_Ums> _businessValidator;

        private bool childViewModelChangedData;
        private ObservableCollection<PurchaseOrderDetailUmViewModel> _pohvmCollection;
        #endregion

        #region Constructors
		internal PurchaseOrderDetailUmViewModel()
		{
			EntityName = "ProductUm";

			// this should be controlled Dependency Injection in the future
			this._businessValidator = new PurchaseOrderDetailUmValidator(_entity);
		}
		internal void InitializeViewModel(Entities.PurchaseOrderDetail_Ums podu, PurchaseOrderManager PoManager, bool IsApplyAlready)
		{
			this._entity = podu;
			base.Entity  = _entity;
			this.IsApply = IsApplyAlready;
			this._businessLogic = PoManager;
		}

		public PurchaseOrderDetailUmViewModel(Entities.Products_Ums pu, Entities.PurchaseOrderDetail pod, PurchaseOrderManager PoManager, bool IsApplyAlready)
			: this()
		{
			Entities.PurchaseOrderDetail_Ums podu = new PurchaseOrderDetail_Ums() { Products_Ums = pu, PurchaseOrderDetail = pod, products_ums_id = pu.id, purchase_order_detail_id = pod.purchase_order_detail_id };
			
			InitializeViewModel(podu, PoManager, IsApplyAlready);
			this.Mode = Mode.Add;
		}
        public PurchaseOrderDetailUmViewModel(Entities.PurchaseOrderDetail_Ums podu, PurchaseOrderManager poManager, bool IsApplyAlready)
			: this()
		{
			InitializeViewModel(podu, poManager, IsApplyAlready);
			this.Mode = Mode.Edit;
		}
		#endregion

        #region System Related Properties

        // View Related Services
        [ServiceProperty(SearchMode = DevMvvm.ServiceSearchMode.PreferLocal)]
        protected virtual DevMvvm.IDialogService dialogService { get { return GetService<DevMvvm.IDialogService>(); } set { } }

        #endregion

        #region Properties
        
        // ApplyCount EventHandler
        public event PropertyChangedEventHandler ApplyPropertyChanged;

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
        public bool IsApply
        {
            get { return _entity.isApply; }
            set
            {
                if (value == _entity.isApply)
                    return;

                base.CheckDataChange(value);
                _entity.isApply = value;
                base.OnPropertyChanged();
                this.OnIsApplyPropertyChanged("IsApply");
            }
        }
        public int PurchaseOrderDetailUmId
        {
            get { return _entity.purchase_order_detail_um_id; }
        }
        public int PurchaseOrderDetailId
        {
            get { return _entity.purchase_order_detail_id; }
        }
        public int ProductUmId
        {
            get { return _entity.products_ums_id; }
            set
            {
                if (_entity.products_ums_id == value)
                    return;

                if (this.Mode == Mode.Edit)
                    base.CheckDataChange(value);

                _entity.products_ums_id = value;
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
        
        public string UmShortName
        {
            get { return _entity.Products_Ums.Um.um_shortname; }
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
                ShowMessageBox("New Purchase Order Item Successfully Added.");
            }
            else if (this.Mode == Mode.Edit) // if editing a Product
            {
                ShowMessageBox("Data Successfully Saved.");
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
            base.Cancel();
        }
        public override bool CanUndo()
        {
            return base.HasChangedData || this.childViewModelChangedData;
        }

        #endregion

        #region SingleObjectViewModelBase Implementation

        protected override int GetPrimaryKey()
        {
            return _entity.purchase_order_detail_um_id;
        }

        #endregion

        #region IDataErrorInfo_Impln (or) Validation Rules

        string IDataErrorInfo.this[string columnName]
        {
            get
            {
                _error = _businessValidator.ValidateProperty(columnName, base.GetPropertyValue(columnName));

                // Dirty the commands registered with CommandManager, such as our Save command, so that they are queried
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

        protected virtual void OnIsApplyPropertyChanged(string propertyName = "")
        {
            base.VerifyPropertyName(propertyName);
            // to make sure the Handle is thread-safe
            PropertyChangedEventHandler handler = this.ApplyPropertyChanged;
            if (this.ApplyPropertyChanged != null)
                this.ApplyPropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
