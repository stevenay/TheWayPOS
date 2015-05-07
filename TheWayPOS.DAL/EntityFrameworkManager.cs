namespace TheWayPOS.DAL
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using DomainInterfaces;
    using GenericInterfaces;

    // it's actually Unit of Work
    public class EntityFrameworkManager : IDataManager, IDisposable
    {
        private TWPDbContext _context;
        private bool disposed = false;

        public bool IsAlreadyDisposed
        {
            get
            {
                return disposed;
            }
        }

        public EntityFrameworkManager()
        {
            _context = new TWPDbContext();
        }
        public void InitializeDbContext()
        {
            if (disposed)
            {
                _context = new TWPDbContext();
            }
        }
        public IProductManager ProductRepo
        {
            get
            {
                return new EfDomainManagers.ProductManager(_context);
            }
        }
        public ISupplierManager SupplierRepo
        {
            get 
            {
                return new EfDomainManagers.SupplierManager(_context);
            }
        }
        public IUmManager UmRepo
        {
            get 
            {
                return new EfDomainManagers.UmManager(_context);
            }
           
        }
        public IProducts_UmManager Products_UmRepo
        {
            get
            {
                return new EfDomainManagers.Products_UmManager(_context);
            }
        }
        public IProductCategoryManager ProductCategoryRepo
        {
            get
            {
                return new EfDomainManagers.ProductCategoryManager(_context);
            }
        }
        public ISupplierCategoryManager SupplierCategoryRepo
        {
            get
            {
                return new EfDomainManagers.SupplierCategoryManager(_context);
            }
        }
        public IPurchaseOrderHeaderManager PurchaseOrderHeaderRepo
        {
            get
            {
                return new EfDomainManagers.PurchaseOrderHeaderManager(_context);
            }
        }
        public IPurchaseOrderDetailManager PurchaseOrderDetailRepo
        {
            get
            {
                return new EfDomainManagers.PurchaseOrderDetailManager(_context);
            }
        }
        public IPurchaseOrderDetailUmManager PurchaseOrderDetailUmRepo
        {
            get
            {
                return new EfDomainManagers.PurchaseOrderDetailUmManager(_context);
            }
        }
        public IProductArrivalManager ProductArrivalRepo
        {
            get
            {
                return new EfDomainManagers.ProductArrivalManager(_context);
            }
        }

        /// <summary>
        /// it's just like transaction.commit() in SqlClient Data Access Layer
        /// </summary>
        /// <returns> number of records modified </returns>
        public int SaveChanges()
        {
            return _context.SaveChanges();
        }

        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }
        private void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    if (_context != null)
                    {
                        _context.Dispose();
                    }
                }
            }

            disposed = true;
        }

        #endregion
    }
}
