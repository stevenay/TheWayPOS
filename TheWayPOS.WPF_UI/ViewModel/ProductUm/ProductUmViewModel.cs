using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using TheWayPOS.BL;
using TheWayPOS.BL.Validator;
using TheWayPOS.Entities;
using TheWayPOS.WPF_UI.Common;
using TheWayPOS.WPF_UI.Common.ViewModel;
using DevMvvm = DevExpress.Mvvm;

namespace TheWayPOS.WPF_UI.ViewModel
{
	public class ProductUmViewModel : SingleObjectViewModelBase<Products_Ums, int>, IDataErrorInfo
	{
		#region Fields
		public Products_Ums _entity;
		private ProductUmManager _businessLogic;
		private IValidator<Products_Ums> _businessValidator;
		#endregion

		#region Constructors
		internal ProductUmViewModel()
		{
			EntityName = "ProductUm";

			// this should be controlled Dependency Injection in the future
			this._businessValidator = new ProductUmValidator();
		}
		internal void InitializeViewModel(Entities.Products_Ums pu, ProductUmManager ProdMngr, bool IsApplyAlready)
		{
			this._entity = pu;
			base.Entity = _entity;
			this.IsApply = IsApplyAlready;
			this._businessLogic = ProdMngr;
		}

		public ProductUmViewModel(Entities.Um u, Entities.Product p, ProductUmManager ProdMngr, bool IsApplyAlready)
			: this()
		{
			Entities.Products_Ums pu = new Products_Ums() { Product = p, product_code = p.product_code, Um = u, um_code = u.um_code, Disposed_ProductUm = u.Disposed_Um };

            if (u.Disposed_Um != null)
            {
                pu.disposed_um_code = u.Disposed_Um.um_code;
                pu.disposed_um_qty = u.disposed_um_qty;
            }
			
			InitializeViewModel(pu, ProdMngr, IsApplyAlready);
			this.Mode = Mode.Add;
		}
		public ProductUmViewModel(Entities.Products_Ums pu, ProductUmManager ProdMngr, bool IsApplyAlready)
			: this()
		{
			
			InitializeViewModel(pu, ProdMngr, IsApplyAlready);
			this.Mode = Mode.Edit;

		}
		#endregion

		#region Properties
        // ApplyCount EventHandler
        public event PropertyChangedEventHandler ApplyCountPropertyChanged;

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
		public override Mode Mode
		{
			get { return _entity.mode; }
			set
			{
				if (value == _entity.mode)
					return;

				base.CheckDataChange(value);
				_entity.mode = value;
				base.OnPropertyChanged();
			}
		}
		public int ID
		{
			get { return _entity.id; }
		}
		public int UmCode
		{
			get { return _entity.um_code; }
			set
			{
				if (value == _entity.um_code)
					return;

				base.CheckDataChange(value);
				_entity.um_code = value;
				base.OnPropertyChanged();
			}
		}
		public string UmShortName
		{
			get { return _entity.Um.um_shortname; }
		}
		public int ProductCode
		{
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
			get { return _entity.Product.product_name; }
		}
		public bool Disposable
		{
			get { return _entity.Um.disposable; }
		}
		public int? DisposedUmCode
		{
			get { return _entity.disposed_um_code; }
			set
			{
				if (value == _entity.disposed_um_code)
					return;

				base.CheckDataChange(value);
				_entity.disposed_um_code = value;
				base.OnPropertyChanged();
			}
		}
		public short? DisposedUmQuantity
		{
			get { return _entity.disposed_um_qty; }
			set
			{
				if (value == _entity.disposed_um_qty)
					return;

				base.CheckDataChange(value);
				_entity.disposed_um_qty = value;
				base.OnPropertyChanged();
			}
		}
		public string DisposedUmShortName
		{
			get { return (_entity.Disposed_ProductUm != null) ? _entity.Disposed_ProductUm.um_shortname : String.Empty; }
		}

		private List<Um> _lookUpUms;
		public List<Um> LookUpUms
		{
			get { return GetUms(); }
			set
			{
				_lookUpUms = value;
				OnPropertyChanged("Ums");
			}
		}

		#endregion

		#region SingleObjectViewModel Implementation
		protected override int GetPrimaryKey()
		{
			return this.ID;
		}
		#endregion      

		#region Commands

		//for adding/saving Product Information
		private ICommand confirmCommand;
		public ICommand ConfirmCommand
		{
			get
			{
				if (confirmCommand == null)
				{
					confirmCommand = new RelayCommand(this.Confirm, null);
				}
				return confirmCommand;
			}
		}

		#endregion

		#region FunctionsforCommands

		public override void Update()
		{
			if (!base.HasChangedData)
				return;

			if (this.Mode == Mode.Edit)
			{
				if (this.IsApply == true)
					_businessLogic.Update(_entity);
				else
					_businessLogic.Delete(_entity);
			}

			base.ClearChangedData();
		}
        public void Add(int productCode = 0)
        {
            _entity.product_code = productCode;

            if (this.Mode == Mode.Add)
            {
                if (this.IsApply == true)
                {
                    _businessLogic.Add(_entity);
                    this.Mode = Mode.Edit;
                }
            }
        }
		public void Confirm()
		{
			// this exit call will not do the Undo
			base.Exit(false);
		}

		#endregion

		#region HelperMethods
		internal List<Um> GetUms()
		{
			if (_lookUpUms == null)
				_lookUpUms = new List<Um>();

			_lookUpUms.Clear();

			UmManager umm = new UmManager();

			var umList = umm.UmList();
			umList.RemoveAll(x => x.um_code == UmCode);
			return umList;
		}
		#endregion

		#region IDataErrorInfo_Impln (or) Validation Rules

		string IDataErrorInfo.this[string columnName]
		{
			get
			{
				_error = null;

				if (columnName == "ProductName")
				{
					if (ProductName == null)  //must have an product name
						_error = "Please name cannot null.";
					else
						_error = _businessValidator.ValidateProperty(columnName, ProductName);
				}
				else if (columnName == "DisposedUmQuantity")
				{
					if (this.Disposable == true)
					{
						if (this.DisposedUmQuantity < 1)  //if not integer
							_error = "အခွဲ UM ပမာဏ သည် အနည်းဆုံး ၁ ထက် ကြီးရပါမည်။";
						else if (this.DisposedUmQuantity == null)
							_error = "အခွဲ UM ပမာဏ သည် ၁ ထက်ကြီးသော ဂဏန်း တစ်ခုခု ဖြစ်ရပါမည်။";
						else
							_error = _businessValidator.ValidateProperty(columnName, this.DisposedUmQuantity);
					}
				}
				else
				{
					_error = _businessValidator.ValidateProperty(columnName, base.GetPropertyValue(columnName));
				}

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
            PropertyChangedEventHandler handler = this.ApplyCountPropertyChanged;
            if (this.ApplyCountPropertyChanged != null)
                this.ApplyCountPropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
