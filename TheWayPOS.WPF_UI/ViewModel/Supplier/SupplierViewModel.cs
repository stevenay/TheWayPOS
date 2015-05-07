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
	public class SupplierViewModel : SingleObjectViewModelBase<Supplier, int>, IDataErrorInfo
	{
		#region Fields
		private Supplier _entity;
		private SupplierManager _businessLogic;
		private IValidator<Supplier> _businessValidator;
        private ObservableCollection<SupplierViewModel> _svmCollection;
		#endregion

		#region Constructors
		private SupplierViewModel()
		{
			EntityName = "Supplier";
		}

		// BusinessLogic Constructor Version
		public SupplierViewModel(Entities.Supplier s, SupplierManager SupMngr) : this()
		{
			this._entity = s;
			base.Entity = _entity;
			this._businessLogic = SupMngr;

            // this should be controlled Dependency Injection in the future
            this._businessValidator = new SupplierValidator(_entity);

			// copy the current value so in case cancel you can undo
			// Now it is not needed anymore because I use INotifyDirtyData Interface
			// this.originalValue = (ProductViewModel)this.MemberwiseClone();
		}
		public SupplierViewModel(Entities.Supplier s, SupplierManager sm, DevMvvm.INavigationService ns)
			: this (s, sm)
		{
			base.NavigationService = ns;
		}
		public SupplierViewModel(Entities.Supplier s, SupplierManager sm, DevMvvm.INavigationService ns, DevMvvm.IDocumentManagerService ds)
			: this(s, sm, ns)
		{
			base.DocumentManagerService = ds;
		}
        public SupplierViewModel(Entities.Supplier s, SupplierManager sm, ObservableCollection<SupplierViewModel> svmCollection, DevMvvm.INavigationService ns, DevMvvm.IDocumentManagerService ds)
            : this(s, sm, ns)
        {
            _svmCollection = svmCollection; 
            base.DocumentManagerService = ds;
        }

		// Without BusinessLogic Constructor Version
		public SupplierViewModel(Entities.Supplier s)
			: this()
		{
			this._entity = s;
			base.Entity = _entity;
			this._businessLogic = new SupplierManager(false);

            // this should be controlled Dependency Injection in the future
            this._businessValidator = new SupplierValidator(this._entity);

			// copy the current value so in case cancel you can undo
			// Now it is not needed anymore because I use INotifyDirtyData Interface
			// this.originalValue = (ProductViewModel)this.MemberwiseClone();
		}
		public SupplierViewModel(Entities.Supplier s, DevMvvm.INavigationService ns)
			: this(s)
		{
			base.NavigationService = ns;
		}
		public SupplierViewModel(Entities.Supplier s, DevMvvm.INavigationService ns, DevMvvm.IDocumentManagerService ds)
			: this(s, ns)
		{
			base.DocumentManagerService = ds;
		}
		public SupplierViewModel(Entities.Supplier s, DevMvvm.INavigationService ns, DevMvvm.IDocumentManagerService ds, Entities.Mode mode)
			: this(s, ns)
		{
			this.Mode = mode;
			base.DocumentManagerService = ds;
		}
        public SupplierViewModel(Entities.Supplier s, ObservableCollection<SupplierViewModel> svmCollection, DevMvvm.INavigationService ns, DevMvvm.IDocumentManagerService ds, Entities.Mode mode)
            : this(s, ns)
        {
            _svmCollection = svmCollection;
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
		public int SupplierCode
		{
			get { return _entity.supplier_code; }
			set
			{
				if (value == _entity.supplier_code)
					return;

				base.CheckDataChange(value);
				_entity.supplier_code = value;
				base.OnPropertyChanged();
			}
		}
		public string SupplierName
		{
			get { return _entity.supplier_name; }
			set
			{
				if (_entity.supplier_name == value)
					return;

				if (this.Mode == Mode.Edit)
					base.CheckDataChange(value);

				_entity.supplier_name = value;
				base.OnPropertyChanged();
			}
		}
		public string SupplierAddress
		{
			get { return _entity.address; }
			set
			{
				if (_entity.address == value)
					return;

				if (this.Mode == Mode.Edit)
					base.CheckDataChange(value);

				_entity.address = value;
				OnPropertyChanged();
			}
		}
		public int SupplierCategoryCode
		{
			get { return _entity.supplier_category_code ?? 0; }
			set
			{
				if (_entity.supplier_category_code == value)
					return;

				if (this.Mode == Mode.Edit)
					base.CheckDataChange(value);

				_entity.supplier_category_code = value;
				base.OnPropertyChanged();
			}
		}
        public string SupplierCategoryName
        {
            get
            {
                return _entity.SupplierCategory.supplier_category_name;
            }
        }

		// Lookup (Linked) Entities
		private List<SupplierCategory> _supplierCategories = null;
		public List<SupplierCategory> SupplierCategories
		{
			get { return GetSupplierCategories(); }
			set
			{
				_supplierCategories = value;
				OnPropertyChanged("SupplierCategories");
			}
		}

		#endregion

		#region Commands
		#endregion

		#region FunctionsforCommands

		// Functions for Commands
		// Override Functions from SingleObjectViewModelBase
		public override void Update()
		{
			if (this.Mode == Mode.Add) // if adding a Produc
			{
				_businessLogic.Add(_entity);
                _businessLogic.FinishBusinessTransaction();

				this.Mode = Mode.Edit;

                if (_svmCollection != null)
                    _svmCollection.Add(this);

                ShowMessageBox("New Supplier Successfully Added.");
			}
			else if (this.Mode == Mode.Edit) // if editing a Product
			{
				if (!base.HasChangedData)
					return;

				_businessLogic.Update(_entity);
                _businessLogic.FinishBusinessTransaction();
				base.ClearChangedData();
				// _businessLogic.FinishBusinessTransaction();

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

		public override void Cancel()
		{
			base.Cancel();
		}
		public override bool CanUndo()
		{
			return base.HasChangedData;
		}

		public override void Back()
		{
			base.Back();
		}
		#endregion

		#region SingleObjectViewModelBase Implementation

		protected override int GetPrimaryKey()
		{
			return _entity.supplier_code;
		}

		#endregion

		#region IDataErrorInfoImplementation (or) ValidationRules

		string IDataErrorInfo.this[string columnName]
		{
			get
			{
				_error = null;

				if (columnName == "SupplierName")
				{
					if ( String.IsNullOrEmpty(SupplierName) )  // must have a supplier name
						_error = "Please enter a Supplier Name";
					else
						_error = _businessValidator.ValidateProperty(columnName, SupplierName);
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
		internal List<SupplierCategory> GetSupplierCategories()
		{
			if (_supplierCategories == null)
			{
				_supplierCategories = new List<SupplierCategory>();
				_supplierCategories.Clear();

				SupplierCategoryManager sm = new SupplierCategoryManager();
				var supCateList = sm.SupplierCategoryList();
				_supplierCategories = supCateList;
			}

			return _supplierCategories;
		}
		#endregion
	}
}
