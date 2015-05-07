using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using TheWayPOS.WPF_UI.Common.ViewModel;
using TheWayPOS.WPF_UI.Service;
using TheWayPOS.BL;
using MyModel = TheWayPOS.Entities;
using DevMvvm = DevExpress.Mvvm;

namespace TheWayPOS.WPF_UI.ViewModel
{
    public class PurchaseOrderDetailUmCollectionViewModel : CollectionViewModel<PurchaseOrderDetailUmViewModel, MyModel.PurchaseOrderDetail_Ums, int>
    {
        #region Fields

        private PurchaseOrderManager _businessLogic;

        public virtual ICustomFilterContainerService CustomFilterContainerService { get { return null; } }
        public FilterTreeViewModel<MyModel.PurchaseOrderDetail_Ums> FilterTreeViewModel { get; set; }

        private PurchaseOrderDetailViewModel _parentEntity;

        #endregion

        #region Events

        public event EventHandler ValueChanged;

        #endregion

        #region Constructors

		public PurchaseOrderDetailUmCollectionViewModel(PurchaseOrderDetailViewModel podvm)
		{
			_parentEntity = podvm;
			_businessLogic = new PurchaseOrderManager(false);
		}
		public PurchaseOrderDetailUmCollectionViewModel(DevMvvm.IDocumentManagerService dms, PurchaseOrderDetailViewModel podvm)
			: this(podvm)
		{
			base.DocumentManagerService = dms;
		}
        public PurchaseOrderDetailUmCollectionViewModel(DevMvvm.IDocumentManagerService dms, DevMvvm.INavigationService ns, DevMvvm.IDialogService ds, PurchaseOrderDetailViewModel podvm)
			: this(dms, podvm)
		{
			this.dialogService = ds;
			base.NavigationService = ns;
		}

		#endregion

        #region Properties

        // View Related Services
        protected DevMvvm.IDialogService dialogService { get; set; }

        // Business Related Properties
        private List<MyModel.Products_Ums> _productUms = null;
        public List<MyModel.Products_Ums> ProductUms
        {
            get { return GetProductUms(); }
            set
            {
                _productUms = value;
                OnPropertyChanged("ProductUms");
            }
        }

        // Selected Product Um
        private PurchaseOrderDetailUmViewModel _selectedPurchaseOrderDetailUm = null;
        public PurchaseOrderDetailUmViewModel SelectedPurchaseOrderDetailUm
        {
            get { return _selectedPurchaseOrderDetailUm; }
            set
            {
                if (_selectedPurchaseOrderDetailUm == value)
                {
                    return;
                }
                _selectedPurchaseOrderDetailUm = value;
                OnPropertyChanged("SelectedPurchaseOrderDetailUm");
            }
        }

        // Apply Count
        private int _purchaseOrderDetailUmApplyCount = 0;
        public int PurchaseOrderDetailUmApplyCount
        {
            get
            {
                return _purchaseOrderDetailUmApplyCount;
            }
            set
            {
                if (_purchaseOrderDetailUmApplyCount == value)
                {
                    return;
                }

                _purchaseOrderDetailUmApplyCount = value;
                OnPropertyChanged("PurchaseOrderDetailUmApplyCount");
            }
        }

        // Error Message
        private string _errorMessage = "";
        public string ErrorMessage
        {
            get
            {
                return _errorMessage;
            }
            set
            {
                if (_errorMessage == value)
                    return;

                _errorMessage = value;
                OnPropertyChanged("ErrorMessage");
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

        // for PurchaseOrderDetailUmViewModel Edit
        public override void Edit(PurchaseOrderDetailUmViewModel puvm)
        {
            dialogService.ShowDialog(
                dialogCommands: null,
                title: "",
                documentType: "WinUIDialogWindow",
                viewModel: puvm,
                parameter: null,
                parentViewModel: this);
        }

        #endregion

        #region CollectionViewModel Abstract Methods Implementation

        public override string entitiesName
        {
            get { return "PurchaseOrderDetailUm"; }
        }
        protected override int GetPrimaryKey(PurchaseOrderDetailUmViewModel vm)
        {
            return vm.PurchaseOrderDetailUmId;
        }
        protected override ObservableCollection<PurchaseOrderDetailUmViewModel> GetEntities()
        {
            if (entities == null)
            {
                entities = new ObservableCollection<PurchaseOrderDetailUmViewModel>();
                entities.Clear();

                var purchaseOrderManager = new PurchaseOrderManager();

                if (this._parentEntity.Mode == MyModel.Mode.Add)
                {
                    foreach (MyModel.Products_Ums item in ProductUms)
                    {
                        PurchaseOrderDetailUmViewModel poduvm = new PurchaseOrderDetailUmViewModel(item, _parentEntity.Entity, purchaseOrderManager, false);
                        //poduvm.PropertyChanged += this.OnChildObjectChanged;
                        //poduvm.ApplyPropertyChanged += this.OnApplyCountChanged;
                        entities.Add(poduvm);
                    }
                }
                else if (this._parentEntity.Mode == MyModel.Mode.Edit)
                {
                    List<MyModel.PurchaseOrderDetail_Ums> poduList = purchaseOrderManager.PurchaseOrderDetailUmListbyPurhcaesOrderDetailId(_parentEntity.PurchaseOrderDetailId);

                    foreach (MyModel.Products_Ums item in ProductUms)
                    {
                        var foundPodu = poduList.Where(pu => pu.products_ums_id == item.id).FirstOrDefault();

                        PurchaseOrderDetailUmViewModel poduvm;
                        if (foundPodu != null)
                        {
                            poduvm = new PurchaseOrderDetailUmViewModel(foundPodu, purchaseOrderManager, true); //it's already applied, in other words it is in Edit Mode
                            this.PurchaseOrderDetailUmApplyCount++;
                        }
                        else
                        {
                            poduvm = new PurchaseOrderDetailUmViewModel(item, _parentEntity.Entity, purchaseOrderManager, false); //it's new, in other words it is Add Mode
                        }

                        poduvm.PropertyChanged += this.OnChildObjectChanged;
                        poduvm.ApplyPropertyChanged += this.OnApplyPropertyChanged;
                        entities.Add(poduvm);
                    }
                }

                this.OnApplyPropertyChanged(null, null);

                // productUmManager.FinishBusinessTransaction();

            }

            return entities;
        }
        
        #endregion

        #region Helper Methods

        internal List<MyModel.Products_Ums> GetProductUms()
        {
            if (_productUms == null)
            {
                _productUms = new List<MyModel.Products_Ums>();
                _productUms.Clear();

                ProductUmManager umm = new ProductUmManager(true);

                var productUmList = umm.ProductUmListbyProductCode(this._parentEntity.ProductCode);
                _productUms = productUmList;
            }

            return _productUms;
        }

        #endregion

        #region Events Methods

        private void OnChildObjectChanged(object sender, PropertyChangedEventArgs e)
        {
            if (ValueChanged != null)
            {
                ValueChanged(sender, e);
            }
        }
        private void OnApplyPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.ErrorFlag = true;

            if (sender != null)
            {
                PurchaseOrderDetailUmViewModel puvm = (PurchaseOrderDetailUmViewModel)sender;
                if (puvm.IsApply == false && this.PurchaseOrderDetailUmApplyCount > 0)
                    this.PurchaseOrderDetailUmApplyCount--;
                else
                    this.PurchaseOrderDetailUmApplyCount++;
            }

            if (PurchaseOrderDetailUmApplyCount < 1)
            {
                this.ErrorMessage = " (ယူနစ်တစ်ခုခုကို အောက်ပါဇယားကွက်တွင် ရွေးချယ်ပေးပါ။) ";
            }
            else
            {
                this.ErrorMessage = " (အထက်တွင် ရွေးချယ်ထားသော ဝယ်ဈေးအခြေခံယူနစ်ကိုလည်း အောက်ပါ ဇယားကွက်တွင် ရွေးပေးပါ။) ";
                //foreach (PurchaseOrderDetailUmViewModel item in this.Entities)
                //{
                //    if (item.UmCode == this._parentEntity.BuyingPriceUmCode && item.IsApply)
                //    {
                //        this.ErrorFlag = false;
                //    }
                //}
            }
        }

        #endregion
    }
}
