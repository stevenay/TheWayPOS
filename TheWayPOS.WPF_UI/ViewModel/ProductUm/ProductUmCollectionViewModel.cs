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
	public class ProductUmCollectionViewModel : CollectionViewModel<ProductUmViewModel, MyModel.Products_Ums, int>
	{
		#region Fields

		private ProductUmManager _businessLogic;

		public virtual ICustomFilterContainerService CustomFilterContainerService { get { return null; } }
		public FilterTreeViewModel<MyModel.Products_Ums> FilterTreeViewModel { get; set; }

		private ProductViewModel _parentEntity;

		#endregion

		#region Events

		public event EventHandler ValueChanged;

		#endregion

		#region Constructors

		public ProductUmCollectionViewModel(ProductViewModel pvm)
		{
			_parentEntity = pvm;
			_businessLogic = new ProductUmManager(false);
		}

		public ProductUmCollectionViewModel(DevMvvm.IDocumentManagerService dms, ProductViewModel pvm)
			: this(pvm)
		{
			base.DocumentManagerService = dms;
		}

		public ProductUmCollectionViewModel(DevMvvm.IDocumentManagerService dms, DevMvvm.INavigationService ns, DevMvvm.IDialogService ds, ProductViewModel pvm)
			: this(dms, pvm)
		{
			this.dialogService = ds;
			base.NavigationService = ns;
		}

		#endregion

		#region Properties

		// View Related Services
		protected DevMvvm.IDialogService dialogService { get; set; }

		// Business Related Properties
		private List<MyModel.Um> _ums = null;
		public List<MyModel.Um> Ums
		{
			get { return GetUms(); }
			set
			{
				_ums = value;
				OnPropertyChanged("Ums");
			}
		}

		// Selected Product Um
		private ProductUmViewModel _selectedProductUm = null;
		public ProductUmViewModel SelectedProductUm
		{
			get { return _selectedProductUm; }
			set
			{
				if (_selectedProductUm == value)
				{
					return;
				}
				_selectedProductUm = value;
				OnPropertyChanged("SelectedProductUm");
			}
		}

		// Apply Count
		private int _productUmApplyCount = 0;
		public int  ProductUmApplyCount
		{
			get
			{
				return _productUmApplyCount;
			}
			set
			{
				if (_productUmApplyCount == value)
				{
					return;
				}

				_productUmApplyCount = value;
				OnPropertyChanged("ProductUmApplyCount");
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

		// for Um Edit
		public override void Edit(ProductUmViewModel puvm)
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
			get { return "ProductUm"; }
		}
		protected override int GetPrimaryKey(ProductUmViewModel vm)
		{
			return vm.ProductCode;
		}
		protected override ObservableCollection<ProductUmViewModel> GetEntities()
		{
			if (entities == null)
			{
				entities = new ObservableCollection<ProductUmViewModel>();
				entities.Clear();

				var productUmManager = new ProductUmManager();

				if (this._parentEntity.Mode == MyModel.Mode.Add)
				{
					foreach (MyModel.Um item in Ums)
					{
						ProductUmViewModel puvm = new ProductUmViewModel(item, _parentEntity.Entity, productUmManager, false);
						puvm.PropertyChanged += this.OnChildObjectChanged;
						puvm.ApplyCountPropertyChanged += this.OnApplyCountChanged;
						entities.Add(puvm);
					}
				}
				else if (this._parentEntity.Mode == MyModel.Mode.Edit)
				{
					List<MyModel.Products_Ums> puList = productUmManager.ProductUmListbyProductCode(_parentEntity.ProductCode);

					foreach (MyModel.Um item in Ums)
					{
						var foundPu = puList.Where(pu => pu.um_code == item.um_code).FirstOrDefault();

						ProductUmViewModel puvm;
						if (foundPu != null) 
						{
							puvm = new ProductUmViewModel(foundPu, productUmManager, true); //it's already applied, in other words it is in Edit Mode
                            this.ProductUmApplyCount++;
						}
						else
						{
							puvm = new ProductUmViewModel(item, _parentEntity.Entity, productUmManager, false); //it's new, in other words it is Add Mode
						}
							
						puvm.PropertyChanged += this.OnChildObjectChanged;
						puvm.ApplyCountPropertyChanged += this.OnApplyCountChanged;
						entities.Add(puvm);
					}
				}

                this.OnApplyCountChanged(null, null);

				// productUmManager.FinishBusinessTransaction();
                
			}

			return entities;
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
		private void OnApplyCountChanged(object sender, PropertyChangedEventArgs e)
		{
            this.ErrorFlag = true;

            if (sender != null)
            {
                ProductUmViewModel puvm = (ProductUmViewModel)sender;
                if (puvm.IsApply == false && this.ProductUmApplyCount > 0)
                    this.ProductUmApplyCount--;
                else
                    this.ProductUmApplyCount++;
            }

            if (ProductUmApplyCount < 1)
            {
                this.ErrorMessage = " (ယူနစ်တစ်ခုခုကို အောက်ပါဇယားကွက်တွင် ရွေးချယ်ပေးပါ။) ";
            } 
            else
            {
                this.ErrorMessage = " (အထက်တွင် ရွေးချယ်ထားသော ဝယ်ဈေးအခြေခံယူနစ်ကိုလည်း အောက်ပါ ဇယားကွက်တွင် ရွေးပေးပါ။) ";
                foreach (ProductUmViewModel item in this.Entities)
                {
                    if (item.UmCode == this._parentEntity.BuyingPriceUmCode && item.IsApply)
                    {
                        this.ErrorFlag = false;
                    }
                }
            }
		}

		#endregion

		#region Helper Methods

		internal List<MyModel.Um> GetUms()
		{
			if (_ums == null)
			{
				_ums = new List<MyModel.Um>();
				_ums.Clear();

				UmManager umm = new UmManager(true);

				var umList = umm.UmList();
				_ums = umList;
			}

			return _ums;
		}

		#endregion
	}
}
