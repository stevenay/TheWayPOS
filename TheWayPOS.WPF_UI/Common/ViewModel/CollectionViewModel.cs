using System;
using System.Diagnostics;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.ObjectModel;
using DeMvvm = DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Data.Filtering;
using System.Windows.Input;
using TheWayPOS.WPF_UI.Common.Util;
using TheWayPOS.WPF_UI.Interface;
using MyModel = TheWayPOS.Entities;
using MessageBoxResult = System.Windows.MessageBoxResult;
using MessageBoxImage = System.Windows.MessageBoxImage;
using MessageBoxButton = System.Windows.MessageBoxButton;

namespace TheWayPOS.WPF_UI.Common.ViewModel
{
    public abstract class CollectionViewModel<TViewModel, TEntity, TPrimaryKey> : ViewModelBase
        where TEntity : class
        where TViewModel : ISingleObjectViewModel<TEntity, TPrimaryKey>
    {
        #region Properties

        // MessageBoxService
        [Required]
        protected virtual DeMvvm.IMessageBoxService MessageBoxService { get { return GetService<DeMvvm.IMessageBoxService>(); } }

        // DocumentManagerService
        [ServiceProperty(DeMvvm.ServiceSearchMode.PreferParents)]
        protected virtual DeMvvm.IDocumentManagerService DocumentManagerService { get; set; }
        protected virtual DeMvvm.INavigationService NavigationService { get; set; }

        /// <summary>
        /// The selected enity.
        /// Since ReadOnlyCollectionViewModelBase is a POCO view model, this property will raise INotifyPropertyChanged.PropertyEvent when modified so it can be used as a binding source in views.
        /// </summary>
        public virtual TViewModel SelectedEntity { get; set; }

        public abstract string entitiesName { get; }

        /// <summary>
        /// Product Entities List loaded from the Business Logic Layer (BLL)
        /// It's same as Entities in DevExpress Project
        /// </summary>
        protected ObservableCollection<TViewModel> entities = null;
        public ObservableCollection<TViewModel> Entities
        {
            get { return GetEntities(); }
            set
            {
                entities = value;
                OnPropertyChanged(entitiesName);
            }
        }

        #endregion

        #region Commands

        // for opening up the Add Product Window
        private ICommand newCommand;
        public ICommand NewCommand
        {
            get
            {
                if (newCommand == null)
                {
                    newCommand = new RelayCommand<TViewModel>(this.New, null);
                }
                return newCommand;
            }
        }

        /// <summary>
        /// Creates and shows a document containing a single object view model for new entity.
        /// Since CollectionViewModelBase is a POCO view model, an instance of this class will also expose the NewCommand property that can be used as a binding source in views.
        /// </summary>
        public virtual void New(TViewModel newVm)
        {
            // I still don't know how to figured it out.
            //IDocument document = CreateDocument(newEntityInitializerFactory != null ?newEntityInitializerFactory() : null);

            DeMvvm.IDocument document = CreateDocument(newVm, null);
            if (document != null)
                document.Show();
        }

        // for opening up the Edit Product Window
        // for opening up the Add Product window
        private ICommand editCommand;
        public ICommand EditCommand
        {
            get
            {
                if (editCommand == null)
                {
                    editCommand = new RelayCommand<TViewModel>(this.Edit, null);
                }
                return editCommand;
            }
        }

        public int EllapsedTime { get; set; }

        /// <summary>
        /// Creates and shows a document containing a single object view model for the existing entity.
        /// Since CollectionViewModelBase is a POCO view model, an instance of this class will also expose the EditCommand property that can be used as a binding source in views.
        /// </summary>
        /// <param name="entity">Entity to edit.</param>
        public virtual void Edit(TViewModel vm)
        {
            // Original Code => Repository.UnitOfWork.GetState(entity) == EntityState.Detached
            // But in my Collection, all objects are ViewModels Not Models
            // So I think this check is unnecessary
            if (vm == null)
            {
                return;
            }
            Stopwatch watch = Stopwatch.StartNew();

            ShowDocument(vm);
            
            watch.Stop();
            EllapsedTime = (int)watch.Elapsed.TotalSeconds;
        }

        /// <summary>
        /// Determines whether an entity can be edited.
        /// Since CollectionViewModelBase is a POCO view model, this method will be used as a CanExecute callback for EditCommand.
        /// </summary>
        /// <param name="entity">An entity to edit.</param>
        public bool CanEdit(TEntity entity)
        {
            return entity != null;
        }

        /// <summary>
        /// Deletes a given entity from the unit of work and saves changes if confirmed by a user.
        /// Since CollectionViewModelBase is a POCO view model, an instance of this class will also expose the DeleteCommand property that can be used as a binding source in views.
        /// </summary>
        /// <param name="entity">An entity to edit.</param>
        public virtual void Delete(TEntity entity)
        {
            if (MessageBoxService.Show(string.Format(CommonResources.Confirmation_Delete, typeof(TEntity).Name), CommonResources.Confirmation_Caption, MessageBoxButton.YesNo, MessageBoxImage.Error, MessageBoxResult.Yes) != MessageBoxResult.Yes)
                return;
            try
            {
                // Delete the Entity

                //Entities.Remove(entity);
                //Repository.Remove(entity);
                //Repository.UnitOfWork.SaveChanges();
                //Messenger.Default.Send(new EntityMessage<TEntity>(entity, EntityMessageType.Deleted));
            }
            catch (Exception e)
            {
                Refresh();
                MessageBoxService.Show("Error occured", "Database Exception", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);

                //MessageBoxService.Show(e.ErrorMessage, e.ErrorCaption, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Determines whether an entity can be deleted.
        /// Since CollectionViewModelBase is a POCO view model, this method will be used as a CanExecute callback for DeleteCommand.
        /// </summary>
        /// <param name="entity">An entity to edit.</param>
        public virtual bool CanDelete(TEntity entity)
        {
            return entity != null;
        }

        /// <summary>
        /// Updates a given entity state and saves changes.
        /// Since CollectionViewModelBase is a POCO view model, instance of this class will also expose the SaveCommand property that can be used as a binding source in views.
        /// </summary>
        /// <param name="entity">Entity to update and save.</param>
        [Display(AutoGenerateField = false)]
        public virtual void Save(TEntity entity)
        {
            //try
            //{
            //    // Save the Entity
            //    //Repository.UnitOfWork.Update(entity);
            //    //Repository.UnitOfWork.SaveChanges();
            //    //Messenger.Default.Send(new EntityMessage<TEntity>(entity, EntityMessageType.Changed));
            //}
            //catch (DbException e)
            //{
            //    MessageBoxService.Show(e.ErrorMessage, e.ErrorCaption, MessageBoxButton.OK, MessageBoxImage.Error);
            //}
        }

        /// <summary>
        /// Determines whether entity local changes can be saved.
        /// Since CollectionViewModelBase is a POCO view model, this method will be used as a CanExecute callback for SaveCommand.
        /// </summary>
        /// <param name="entity">Entity to edit.</param>
        public virtual bool CanSave(TEntity entity)
        {
            return entity != null;
        }
        #endregion

        #region AbstractMethods

        /// <summary>
        /// Get Product Entities method for Products property
        /// It's the sam GetEntities() method in DevExpress Project
        /// </summary>
        /// <returns></returns>
        protected abstract ObservableCollection<TViewModel> GetEntities();
        protected abstract TPrimaryKey GetPrimaryKey(TViewModel vm);

        #endregion

        #region HelperMethods

        //internal ObservableCollection<ProductViewModel> GetProducts()
        //{
        //    if (products == null)
        //        products = new ObservableCollection<ProductViewModel>();

        //    products.Clear();

        //    var productList = _businessLogic.ProductList();
        //    foreach (var item in productList)
        //    {
        //        ProductViewModel pvm = new ProductViewModel(item, _businessLogic);
        //        products.Add(pvm);
        //    }

        //    return products;
        //}

        //public int GetCount(Data.Filtering.CriteriaOperator filterCriteria)
        //{
        //    return GetCount(GetExpression(filterCriteria));
        //}

        //public Data.Filtering.CriteriaOperator GetInOperator(IEnumerable<TEntity> entities)
        //{
        //    string keyName = ((MemberExpression)Repository.GetPrimaryKeyExpression.Body).Member.Name;
        //    return new Data.Filtering.InOperator(keyName, entities.Select(e => GetPrimaryKey(e)));
        //}

        void ShowDocument(TViewModel vm)
        {
            string entitisedKey = entitiesName + GetPrimaryKey(vm);
            DeMvvm.IDocument document = FindEntityDocument(entitisedKey) ?? CreateDocument(vm, entitisedKey);
            if (document != null)
                document.Show();
        }

        protected virtual DeMvvm.IDocument CreateDocument(TViewModel vm, object parameter)
        {
            if (DocumentManagerService == null) return null;
            return DocumentManagerService.CreateDocument(typeof(TEntity).Name + "View", vm, parameter, this);
        }

        protected void DestroyDocument(DeMvvm.IDocument document)
        {
            if (document != null)
                document.Close();
        }

        protected DeMvvm.IDocument FindEntityDocument(string key)
        {
            if (DocumentManagerService == null) return null;
            foreach (DeMvvm.IDocument document in DocumentManagerService.Documents)
            {
                ISingleObjectViewModel<TEntity, TPrimaryKey> entityViewModel = document.Content as ISingleObjectViewModel<TEntity, TPrimaryKey>;
                if (entityViewModel != null && String.Equals(entityViewModel.EntityName + entityViewModel.PrimaryKey.ToString(), key))
                    return document;
            }
            return null;
        }

        public void Refresh()
        {
            //TEntity entity = SelectedEntity;
            //base.Refresh();
            //if (entity != null && Repository.HasPrimaryKey(entity))
            //    SelectedEntity = FindNewEntity(GetPrimaryKey(entity));
        }

        protected int GetCount(Expression<Func<TViewModel, bool>> filter)
        {
            int count = (filter != null) ? entities.Where(filter.Compile()).Count() : entities.Count;
            return count;
        }

        public Expression<Func<TViewModel, bool>> GetExpression(CriteriaOperator filterCriteria)
        {
            return new CriteriaOperatorToExpressionConverter<TViewModel>().Convert(filterCriteria);
        }

        
        #endregion

        #region Events

        protected void OnSelectedEntityChanged()
        {
            // base.OnSelectedEntityChanged();
            // this.RaiseCanExecuteChanged(x => x.Edit(SelectedEntity));
            // this.RaiseCanExecuteChanged(x => x.Delete(SelectedEntity));
            // this.RaiseCanExecuteChanged(x => x.Save(SelectedEntity));
        }

        #endregion
    }
}
