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
using DevExpress.Xpf.Editors.Helpers;

namespace TheWayPOS.WPF_UI.ViewModel
{
	public class UmViewModel : SingleObjectViewModelBase<Um, int>, IDataErrorInfo
	{
		#region Fields
		
		public Um _entity;
		private UmManager _businessLogic;
		private IValidator<Um> _businessValidator;

		private ObservableCollection<UmViewModel> _umvmCollection;

		#endregion

		#region Constructors

		internal UmViewModel()
		{
			Entity = _entity;
			EntityName = "Um";
		}

		// BusinessLogic Constructor Version
		public UmViewModel(Entities.Um u, UmManager UmMngr)
			: this()
		{
			this._entity = u;
			base.Entity = _entity;
            this._businessLogic = UmMngr;

			// this should be controlled Dependency Injection in the future
			this._businessValidator = new UmValidator(_entity);

			// copy the current value so in case cancel you can undo
			// Now it is not needed anymore because I use INotifyDirtyData Interface
			// this.originalValue = (ProductViewModel)this.MemberwiseClone();
		}
        public UmViewModel(Entities.Um u, UmManager umMngr, DevMvvm.INavigationService ns)
			: this (u, umMngr)
		{
			base.NavigationService = ns;
		}
		public UmViewModel(Entities.Um u, UmManager umMngr, DevMvvm.INavigationService ns, DevMvvm.IDocumentManagerService ds)
			: this(u, umMngr, ns)
		{
			base.DocumentManagerService = ds;
		}
        public UmViewModel(Entities.Um u, UmManager umMngr, ObservableCollection<UmViewModel> umvmCollection, DevMvvm.INavigationService ns, DevMvvm.IDocumentManagerService ds)
			: this(u, umMngr, ns)
		{
            _umvmCollection = umvmCollection; 
			base.DocumentManagerService = ds;
		}

		public UmViewModel(Entities.Um u)
			: this()
		{
			this._entity = u;
			base.Entity = _entity;
			this._businessLogic = new UmManager(false);

			// this should be controlled Dependency Injection in the future
			this._businessValidator = new UmValidator(this._entity);

			// copy the current value so in case cancel you can undo
			// Now it is not needed anymore because I use INotifyDirtyData Interface
			// this.originalValue = (ProductViewModel)this.MemberwiseClone();
		}
		public UmViewModel(Entities.Um u, DevMvvm.INavigationService ns)
			: this(u)
		{
			base.NavigationService = ns;
		}
		public UmViewModel(Entities.Um u, DevMvvm.INavigationService ns, DevMvvm.IDocumentManagerService ds)
			: this(u, ns)
		{
			base.DocumentManagerService = ds;
		}
		public UmViewModel(Entities.Um u, DevMvvm.INavigationService ns, DevMvvm.IDocumentManagerService ds, Entities.Mode mode)
			: this(u, ns)
		{
			this.Mode = mode;
			base.DocumentManagerService = ds;
		}
		public UmViewModel(Entities.Um u, ObservableCollection<UmViewModel> umvmCollection, DevMvvm.INavigationService ns, DevMvvm.IDocumentManagerService ds, Entities.Mode mode)
			: this(u, ns)
		{
			_umvmCollection = umvmCollection;
			this.Mode = mode;
			base.DocumentManagerService = ds;
		}
		#endregion

		#region Properties

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
			get { return _entity.um_shortname; }
			set
			{
				if (_entity.um_shortname == value)
					return;

				base.CheckDataChange(value);
				_entity.um_shortname = value;
				base.OnPropertyChanged();
			}
		}
		public string UmFullName
		{
			get { return _entity.um_fullname; }
			set
			{
				if (_entity.um_fullname == value)
					return;

				base.CheckDataChange(value);
				_entity.um_fullname = value;
				OnPropertyChanged();
			}
		}
		public bool Disposable
		{
			get { return _entity.disposable; }
			set
			{
				if (_entity.disposable == value)
					return;

				base.CheckDataChange(value);
				_entity.disposable = value;
				OnPropertyChanged();
			}
		}
		public int? DisposedUmCode
		{
			get { return _entity.disposed_um_code; }
			set
			{
				if (_entity.disposed_um_code == value)
					return;

				base.CheckDataChange(value);
				_entity.disposed_um_code = value;
				OnPropertyChanged();
			}
		}
		public short? DisposedUmQuantity
		{
			get { return _entity.disposed_um_qty; }
			set
			{
				if (_entity.disposed_um_qty == value)
					return;

				base.CheckDataChange(value);
				_entity.disposed_um_qty = value;
				OnPropertyChanged();
			}
		}
		public string DisposedUmName
		{
			get { return (_entity.Disposed_Um != null) ? _entity.Disposed_Um.um_shortname : String.Empty; }
		}
        // Lookup (Linked) Entities
        private List<Um> _disposedUms = null;
        public List<Um> DisposedUms
        {
            get { return GetDisposedUms(); }
            set
            {
                _disposedUms = value;
                OnPropertyChanged("DisposedUms");
            }
        }
		
        #endregion

        #region FunctionsforCommands

        // Functions for Commands
        // Override Functions from SingleObjectViewModelBase
        public override void Update()
        {
            if (this.Mode == Mode.Add) // if adding a Um
            {
                _businessLogic.Add(_entity);
                
                _businessLogic.FinishBusinessTransaction();
                base.ClearChangedData();

                this.Mode = Mode.Edit;

                if (_umvmCollection != null)
                    _umvmCollection.Add(this);

                ShowMessageBox("New Um Successfully Added.");
            }
            else if (this.Mode == Mode.Edit) // if editing a Um
            {
                if (!this.Disposable)
                {
                    this.DisposedUmCode = null;
                    this.DisposedUmQuantity = null;
                }

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
        //public override void Delete()
        //{
        //    _businessLogic.Deactivate(this._entity);
        //    base.ClearChangedData();
        //}
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

		#region Implement_SingleObjectViewModelBase

		protected override int GetPrimaryKey()
		{
			return UmCode;
		}

		#endregion

        #region IDataErrorInfoImplementation (or) ValidationRules
        string IDataErrorInfo.this[string columnName]
        {
            get
            {
                _error = null;

                if (columnName == "Disposable")
                {
                    if ((bool)this.Disposable)  // if check on Disposable, both DisposedUmCode and DisposedUmQuantity must be existed.
                    {
                        OnPropertyChanged("DisposedUmCode");
                        OnPropertyChanged("DisposedUmQuantity");
                    }
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

        #region Helper Methods

        internal List<Um> GetDisposedUms()
        {
            if (_disposedUms == null)
            {
                _disposedUms = new List<Um>();
                _disposedUms.Clear();

                var disposedUmList = this._businessLogic.UmList();
                _disposedUms = disposedUmList;
                _disposedUms.Remove(this._entity);
            }

            return _disposedUms;
        }

        #endregion
    }
}
