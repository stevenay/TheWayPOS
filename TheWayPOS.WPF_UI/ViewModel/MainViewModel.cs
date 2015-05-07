using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using System.Collections.ObjectModel;
using TheWayPOS.WPF_UI.Common;
using MyModel = TheWayPOS.Entities;
using DeMvvm = DevExpress.Mvvm;

namespace TheWayPOS.WPF_UI.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        // Document Services
        // public virtual DeMvvm.IDocument Document { get; set; }
        protected virtual DeMvvm.IDocumentManagerService DocumentManagerService { get { return GetService<DeMvvm.IDocumentManagerService>(); } }

        // For TileBar, this Collection works as ItemSource
        public ObservableCollection<TileViewModel> TileCollection { get; set; }
        private DeMvvm.INavigationService NavigationService { get { return this.GetService<DeMvvm.INavigationService>(); } }

        #region Constructor

        /// <summary>
        /// Initialize a new instance of MainViewModel
        /// Also initialize ItemSource Object for TileBar
        /// </summary>
        public MainViewModel()
        {
        }

        #endregion

        #region Commands
        // for opening up the Add Product window
        private ICommand showAddNewProductCommand;
        public ICommand ShowAddNewProductCommand
        {
            get
            {
                if (showAddNewProductCommand == null)
                {
                    showAddNewProductCommand = new RelayCommand(this.ShowAddNewProduct, null);
                }

                return showAddNewProductCommand;
            }
        }

        // for opening up Product List Window
        private ICommand showProductListCommand;
        public ICommand ShowProductListCommand
        {
            get
            {
                if (showProductListCommand == null)
                {
                    showProductListCommand = new RelayCommand(this.ShowProductList, null);
                }

                return showProductListCommand;
            }
        }

        // for opening up the Add Supplier window
        private ICommand showAddNewSupplierCommand;
        public ICommand ShowAddNewSupplierCommand
        {
            get
            {
                if (showAddNewSupplierCommand == null)
                {
                    showAddNewSupplierCommand = new RelayCommand(this.ShowAddNewSupplier, null);
                }

                return showAddNewSupplierCommand;
            }
        }

        // for opening up Supplier List Window
        private ICommand showSupplierListCommand;
        public ICommand ShowSupplierListCommand
        {
            get
            {
                if (showSupplierListCommand == null)
                {
                    showSupplierListCommand = new RelayCommand(this.ShowSupplierList, null);
                }

                return showSupplierListCommand;
            }
        }

        // for opening up Product Category List Window
        private ICommand showProductCategoryListCommand;
        public ICommand ShowProductCategoryListCommand
        {
            get
            {
                if (showProductCategoryListCommand == null)
                {
                    showProductCategoryListCommand = new RelayCommand(this.ShowProductCategoryList, null);
                }

                return showProductCategoryListCommand;
            }
        }

        // for opening up Um List Window
        private ICommand showUmListCommand;
        public ICommand ShowUmListCommand
        {
            get
            {
                if (showUmListCommand == null)
                {
                    showUmListCommand = new RelayCommand(this.ShowUmList, null);
                }

                return showUmListCommand;
            }
        }
        
        // for opening up PurchaseInvoice Window
        private ICommand showPurchaseOrderCommand;
        public ICommand ShowPurchaseOrderCommand
        {
            get
            {
                if (showPurchaseOrderCommand == null)
                {
                    showPurchaseOrderCommand = new RelayCommand(this.ShowPurchaseOrder, null);
                }

                return showPurchaseOrderCommand;
            }
        }
        #endregion

        #region Methods For Commands
        public void ShowAddNewProduct()
        {
            ProductViewModel pvm = new ProductViewModel(new MyModel.Product(), NavigationService, DocumentManagerService, MyModel.Mode.Add);
            ShowDocument("ProductView", "Add New Product", pvm);
        }
        public void ShowProductList()
        {
            ShowDocument("ProductCollectionView", "Product Collection", new ProductCollectionViewModel(DocumentManagerService, NavigationService));
        }
        public void ShowAddNewSupplier()
        {
            SupplierViewModel svm = new SupplierViewModel(new MyModel.Supplier(), NavigationService, DocumentManagerService, MyModel.Mode.Add);
            ShowDocument("SupplierView", "Add New Supplier", svm);
        }
        public void ShowSupplierList()
        {
            ShowDocument("SupplierCollectionView", "Supplier Collection", new SupplierCollectionViewModel(DocumentManagerService, NavigationService));
        }
        public void ShowProductCategoryList()
        {
            ShowDocument("ProductCategoryCollectionView", "Product Category Collection", new ProductCategoryCollectionViewModel(DocumentManagerService, NavigationService));
        }
        public void ShowUmList()
        {
            ShowDocument("UmCollectionView", "Um Collection", new UmCollectionViewModel(DocumentManagerService, NavigationService));
        }
        public void ShowPurchaseOrder()
        {
            PurchaseOrderHeaderViewModel pohvm = new PurchaseOrderHeaderViewModel(new MyModel.PurchaseOrderHeader(), NavigationService, DocumentManagerService, MyModel.Mode.Add); 
            ShowDocument("PurchaseOrderView", "Purchase Order Collection", pohvm);
        }
        #endregion

        #region Helper Methods

        private void ShowDocument(string documentName, string documentTitle, object viewModel = null)
        {
            DeMvvm.IDocument document;
            if (FindEntityDocumentbyTitle(documentTitle))
                document = null;
            else
                document = CreateDocument(documentName, documentTitle, viewModel, "");

            if (document != null)
                document.Show();
        }
        protected virtual DeMvvm.IDocument CreateDocument(string documentName, string documentTitle, object vm, string parameter)
        {
            if (DocumentManagerService == null) return null;

            DeMvvm.IDocument document = DocumentManagerService.CreateDocument(documentName, vm, parameter, this);
            document = DocumentManagerService.CreateDocument(documentName, vm, null, this);
            document.Title = documentTitle;
            document.DestroyOnClose = true;

            return document;
        }
        protected bool FindEntityDocumentbyTitle(string key)
        {
            if (DocumentManagerService == null) return false;
            foreach (DeMvvm.IDocument document in DocumentManagerService.Documents)
            {
                if (string.Equals(key, (document.Title ?? "").ToString(), StringComparison.CurrentCultureIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }

        #endregion
    }
}
