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
	public class ProductCategoryViewModel : SingleObjectViewModelBase<ProductCategory, int>, IDataErrorInfo
	{
		#region Fields
		private ProductCategory _entity;
		private ProductCategoryManager _businessLogic;
		private IValidator<ProductCategory> _businessValidator;

        private ObservableCollection<ProductCategoryViewModel> _pcvmCollection;
		// private bool childViewModelChangedData;
		#endregion

		#region Constructors
		private ProductCategoryViewModel()
		{
			EntityName = "ProductCategory";
		}

		// BusinessLogic Constructor Version
		public ProductCategoryViewModel(Entities.ProductCategory p, ProductCategoryManager ProdMngr) : this()
		{
			this._entity = p;
			base.Entity = _entity;
			this._businessLogic = ProdMngr;

            // this should be controlled Dependency Injection in the future
            this._businessValidator = new ProductCategoryValidator(_entity);

			// copy the current value so in case cancel you can undo
			// Now it is not needed anymore because I use INotifyDirtyData Interface
			// this.originalValue = (ProductViewModel)this.MemberwiseClone();
		}
		public ProductCategoryViewModel(Entities.ProductCategory pc, ProductCategoryManager pm, DevMvvm.INavigationService ns)
			: this (pc, pm)
		{
			base.NavigationService = ns;
		}
		public ProductCategoryViewModel(Entities.ProductCategory pc, ProductCategoryManager pm, DevMvvm.INavigationService ns, DevMvvm.IDocumentManagerService ds)
			: this(pc, pm, ns)
		{
			base.DocumentManagerService = ds;
		}

		// Without BusinessLogic Constructor Version
		public ProductCategoryViewModel(Entities.ProductCategory pc)
			: this()
		{
			this._entity = pc;
			base.Entity = _entity;
			this._businessLogic = new ProductCategoryManager(false);

            // this should be controlled Dependency Injection in the future
            this._businessValidator = new ProductCategoryValidator(_entity);

			// copy the current value so in case cancel you can undo
			// Now it is not needed anymore because I use INotifyDirtyData Interface
			// this.originalValue = (ProductViewModel)this.MemberwiseClone();
		}
		public ProductCategoryViewModel(Entities.ProductCategory pc, DevMvvm.INavigationService ns)
			: this(pc)
		{
			base.NavigationService = ns;
		}
		public ProductCategoryViewModel(Entities.ProductCategory pc, DevMvvm.INavigationService ns, DevMvvm.IDocumentManagerService ds)
			: this(pc, ns)
		{
			base.DocumentManagerService = ds;
		}
		public ProductCategoryViewModel(Entities.ProductCategory pc, ObservableCollection<ProductCategoryViewModel> pcvmCollection, DevMvvm.INavigationService ns, DevMvvm.IDocumentManagerService ds, Entities.Mode mode)
			: this(pc, ns)
		{
            _pcvmCollection = pcvmCollection;
			this.Mode = mode;
			base.DocumentManagerService = ds;
		}
		#endregion

		#region Properties
		// View Related Services
		[ServiceProperty(SearchMode = DevMvvm.ServiceSearchMode.PreferLocal)]
		protected virtual DevMvvm.IDialogService dialogService { get { return GetService<DevMvvm.IDialogService>(); } set { } }

		// Business Domain Properties
		public int ProductCategoryCode
		{
			get { return _entity.category_code; }
			set
			{
				if (value == _entity.category_code)
					return;

				base.CheckDataChange(value);
				_entity.category_code = value;
				base.OnPropertyChanged();
			}
		}
		public string CategoryName
		{
			get { return _entity.category_name; }
			set
			{
				if (_entity.category_name == value)
					return;

				if (this.Mode == Mode.Edit)
					base.CheckDataChange(value);

				_entity.category_name = value;
				base.OnPropertyChanged();
			}
		}
		public int ProductCount
		{
			get { return _entity.Products.Count; }
		}
		#endregion

		#region Commands

		#endregion

		#region FunctionsforCommands

		// Functions for Commands
		// Override Functions from SingleObjectViewModelBase
		public override void Update()
		{
			if (this.Mode == Mode.Add) // if adding a ProductCategory
			{
				_businessLogic.Add(_entity);
				_businessLogic.FinishBusinessTransaction();

				this.Mode = Mode.Edit;

                if (_pcvmCollection != null)
                    _pcvmCollection.Add(this);

                ShowMessageBox("New ProductCategory Successfully Added.");
			}
			else if (this.Mode == Mode.Edit) // if editing a ProductCategory
			{
				if (!base.HasChangedData)
					return;

				if (base.HasChangedData)
					_businessLogic.Update(_entity);

				_businessLogic.FinishBusinessTransaction();
				base.ClearChangedData();

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
			_businessLogic.Deactivate(this._entity);
			base.ClearChangedData();
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
			return _entity.category_code;
		}

		#endregion

		#region IDataErrorInfoImplementation (or) ValidationRules
		string IDataErrorInfo.this[string columnName]
		{
			get
			{
				_error = null;

				if (columnName == "CategoryName")
				{
					_error = _businessValidator.ValidateProperty(columnName, this.CategoryName);
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
		#endregion
	}
}
