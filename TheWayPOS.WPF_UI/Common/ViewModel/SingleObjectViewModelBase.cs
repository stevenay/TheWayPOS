using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;
using System.Reflection;
using System.Linq;
using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using DevMvvm = DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using TheWayPOS.WPF_UI.Interface;
using TheWayPOS.Entities;

namespace TheWayPOS.WPF_UI.Common.ViewModel
{
    /// <summary>
    /// The base class for POCO view models exposing a single entity of a given type and CRUD operations against this entity. 
    /// It is not recommended to inherit directly from this class. Use the SingleObjectViewModel class instead.
    /// </summary>
    /// <typeparam name="TEntity">An entity type.</typeparam>
    /// <typeparam name="TPrimaryKey">A primary key value type.</typeparam>
    /// <typeparam name="TUnitOfWork">A unit of work type.</typeparam>
    [POCOViewModel]
    public abstract class SingleObjectViewModelBase<TEntity, TPrimaryKey> : ViewModelBase, ISingleObjectViewModel<TEntity, TPrimaryKey>, IDocumentContent, INotifyDirtyData
        where TEntity : class
    {
        #region Fields

        String title = "";
        
        // protected readonly Func<TUnitOfWork, IRepository<TEntity, TPrimaryKey>> getRepositoryFunc;
        protected readonly Func<TEntity, object> getEntityDisplayNameFunc;

        // protected IUnitOfWorkFactory<TUnitOfWork> UnitOfWorkFactory { get; private set; }
        // protected TUnitOfWork UnitOfWork { get; set; }

        protected String _error;
        
        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the SingleObjectViewModelBase class.
        /// </summary>
        /// <param name="unitOfWorkFactory">A factory used to create the unit of work instance.</param>
        /// <param name="getRepositoryFunc">A function that returns repository representing entities of a given type.</param>
        /// <param name="getEntityDisplayNameFunc">An optional parameter that provides a function to obtain the display text for a given entity. If ommited, the primary key value is used as a display text.</param>
        protected SingleObjectViewModelBase()
        {
            // UnitOfWorkFactory = unitOfWorkFactory;
            // this.getRepositoryFunc = getRepositoryFunc;
            // this.getEntityDisplayNameFunc = getEntityDisplayNameFunc;
            // UnitOfWork = UnitOfWorkFactory.CreateUnitOfWork();
        }

        #endregion

        #region Properties

        /// <summary>
        /// For determining if you are in Add Product or Edit Product Mode
        /// </summary>
        private Mode _mode;
        public virtual Mode Mode
        {
            get
            {
                return _mode;
            }
            set
            {
                if (_mode == value)
                    return;

                _mode = value;
                this.OnPropertyChanged();
            }
        }

        // Navigation Service
        protected virtual DevMvvm.INavigationService NavigationService { get; set; }

        // DocumentManager Service
        protected virtual DevMvvm.IDocumentManagerService DocumentManagerService { get; set; }

        // MessageBox Service
        [ServiceProperty(SearchMode = DevMvvm.ServiceSearchMode.PreferLocal)]
        protected virtual DevMvvm.IMessageBoxService messageBoxService { get { return GetService<DevMvvm.IMessageBoxService>(); } set { } }

        /// <summary>
        /// The display text for a given entity used as a title in the corresponding view.
        /// </summary>
        /// <returns></returns>
        public object Title { get { return title; } }

        /// <summary>
        /// An entity represented by this view model.
        /// Since SingleObjectViewModelBase is a POCO view model, this property will raise INotifyPropertyChanged.PropertyEvent when modified so it can be used as a binding source in views.
        /// </summary>
        /// <returns></returns>
        public virtual TEntity Entity { get; set; }

        /// <summary>
        /// Entity Name represented by this view model.
        /// </summary>
        public virtual String EntityName { get; set; }

        #endregion

        #region Commands

        // for Show Message Box
        private ICommand messageBoxCommand;
        public ICommand MessageBoxCommand
        {
            get
            {
                if (messageBoxCommand == null)
                {
                    messageBoxCommand = new RelayCommand<string>(this.ShowMessageBox, null);
                }
                return messageBoxCommand;
            }
        }   

        //for adding/saving Entity Information
        private ICommand updateCommand;
        public ICommand UpdateCommand
        {
            get
            {
                if (updateCommand == null)
                {
                    updateCommand = new RelayCommand(Update, CanUpdate);
                }
                return updateCommand;
            }
        }

        // for cancel an Edit
        private ICommand cancelCommand;
        public ICommand CancelCommand
        {
            get
            {
                if (cancelCommand == null)
                {
                    cancelCommand = new RelayCommand(this.Cancel, this.CanUndo);
                }
                return cancelCommand;
            }
        }

        // for delete Entity
        private ICommand deleteCommand;
        public ICommand DeleteCommand
        {
            get 
            {
                if (deleteCommand == null)
                {
                    deleteCommand = new RelayCommand(this.Delete, this.CanDelete);
                }
                return deleteCommand;
            }
        }

        // for back navigation
        private ICommand backCommand;
        public ICommand BackCommand
        {
            get
            {
                if (backCommand == null)
                {
                    backCommand = new RelayCommand(this.Back, this.CanBack);
                }
                return backCommand;
            }
        }

        // for exit the form
        private ICommand exitCommand;
        public ICommand ExitCommand
        {
            get
            {
                if (exitCommand == null)
                {
                    exitCommand = new RelayCommand<bool>(this.Exit, null);
                }
                return exitCommand;
            }
        }

        #endregion

        #region Methods for Commands
        // For Showing Message Box
        public void ShowMessageBox(string text)
        {
            messageBoxService.Show(text, "The Way POS System", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.None, System.Windows.MessageBoxResult.OK);
        }

        public virtual void Update()
        {
        }
        protected virtual bool CanUpdate()
        {
            return String.IsNullOrEmpty(_error) && base.HasChangedData == true;
        }

        public virtual void Delete()
        {
        }
        public virtual bool CanDelete()
        {
            return (Entity != null); 
        }

        public virtual void Cancel()
        {
            this.Undo();
        }
        
        // Undo is so detail to be abstract
        public virtual void Undo()
        {
            if (base.HasChangedData)
            {
                // clone _changes Dictionary from base class
                // because _changes Dictionary cannot be modified during foreach loop
                // When I change propertyInfo in foreach loop, 
                // it also call CheckDataChanged (which in turns delete PropertyName from _changes dictionary)
                var _changesDictTemp = base._changes.ToDictionary(entry => entry.Key, entry => entry.Value);

                foreach (KeyValuePair<String, Object> item in _changesDictTemp)
                {
                    Type valueType = this.GetType();
                    PropertyInfo propInfo = valueType.GetProperty(item.Key);

                    propInfo.SetValue(this, item.Value, null);
                }

                _changesDictTemp.Clear();
            }
        }
        public virtual bool CanUndo()
        {
            return base.HasChangedData;
        }

        public virtual void Back()
        {
            this.Undo();
            NavigationService.GoBack();
        }
        bool CanBack()
        {
            return NavigationService.CanGoBack;
        }

        public virtual void Exit(bool willUndo = true)
        {
            var currentWindow = ((CurrentWindowService)ServiceContainer.GetService<DevMvvm.ICurrentWindowService>()).Window;
            if (currentWindow == null)
                new Exception("Current Window Service is null!");
            else
            {
                if (willUndo)
                    this.Undo();
                ServiceContainer.GetService<DevMvvm.ICurrentWindowService>().Close();
            }
        }

        #endregion

        #region Abstract Methods

        protected abstract TPrimaryKey GetPrimaryKey();

        #endregion

        #region MethodsthatIdon'tknow

        protected virtual bool HasValidationErrors() { return false; }
        string GetTitle(bool entityModified)
        {
            if (entityModified)
                return GetTitle() + CommonResources.Entity_Changed;
            else
                return GetTitle();
        }
        protected virtual string GetTitleForNewEntity()
        {
            return typeof(TEntity).Name + CommonResources.Entity_New;
        }
        protected virtual string GetTitle()
        {
            return String.Format("{0} - {1}", typeof(TEntity).Name, Convert.ToString(getEntityDisplayNameFunc != null ? getEntityDisplayNameFunc(Entity) : this.GetPrimaryKey()));
        }
        protected void DestroyDocument(IDocument document)
        {
            if (document != null)
                document.Close();
        }

        #endregion       

        #region IDocumentContent
        object IDocumentContent.Title { get { return Title; } }

        void IDocumentContent.OnClose(CancelEventArgs e)
        {
            //e.Cancel = !TryClose();
        }

        void IDocumentContent.OnDestroy()
        {
            // OnDestroy();
        }

        IDocumentOwner IDocumentContent.DocumentOwner
        {
            get;
            set;
        }
        #endregion

        #region ISingleObjectViewModel
        TEntity ISingleObjectViewModel<TEntity, TPrimaryKey>.Entity { get { return Entity; } }

        TPrimaryKey ISingleObjectViewModel<TEntity, TPrimaryKey>.PrimaryKey { get { return this.GetPrimaryKey(); } }

        String ISingleObjectViewModel<TEntity, TPrimaryKey>.EntityName { get { return EntityName; } }
        #endregion

    }
}
