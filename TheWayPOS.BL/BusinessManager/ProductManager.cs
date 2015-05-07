using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using TheWayPOS.DAL;
using MyModel = TheWayPOS.Entities;

namespace TheWayPOS.BL
{
    public class ProductManager : BusinessLogicManagerBase
    {
        #region Singleton_Implementation

        //private static ProductManager _instance = null;
        //private static object lockObject = new object();

        //// Singleton Service Provider
        //public static ProductManager Instance()
        //{
        //    if (_instance == null) //lazy loading
        //    {
        //        // lock so that another thread cannot enter(access) 
        //        // the following code block 
        //        // while it's executing in current thread
        //        lock (lockObject) 
        //        {
        //            if (_instance == null)
        //            {
        //                _instance = new ProductManager();
        //            }
        //        }
        //    }
        //    return _instance;
        //}

        #endregion

        #region Constructors

        /// <summary>
        /// Set true to ContextAtomic Parameter if you want to instantly dispose the Context Vairable
        /// Set false to ContextAtomic Parameter if you want to keep your Context Alive throughout the class Life
        /// </summary>
        /// <param name="contextAtomic"></param>
        public ProductManager(bool contextAtomic = false)
            : base()
        {
        }

        #endregion

        #region CRUD Operations Set
        public MyModel.Product GetProduct(int productId)
        {
            try
            {
                _dataManager = FactoryManager.Instance().GetRepositoryManager();

                // Get Um
                MyModel.Product p = _dataManager.ProductRepo.Get(productId);

                // Destroy the DataManager
                if (_contextAtomic)
                    _dataManager.Dispose();

                // return product
                return p;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public MyModel.Product GetSameNameProductbySupplierCode(int productCode, int supplierCode)
        {
            try
            {
                _dataManager = FactoryManager.Instance().GetRepositoryManager();

                MyModel.Product product = _dataManager.ProductRepo.getSameNameProductbySupplierCode(productCode, supplierCode);

                return product;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public List<MyModel.Product> ProductList()
        {
            try
            {
                _dataManager = FactoryManager.Instance().GetRepositoryManager();

                // get products list into variable so that
                // we can destroy dm before return
                var products = _dataManager.ProductRepo.All().ToList();

                if (_contextAtomic)
                    _dataManager.Dispose();

                // return product list
                return products;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public List<MyModel.Product> ProductFullInfoList()
        {
            try
            {
                _dataManager = FactoryManager.Instance().GetRepositoryManager();

                // get products list into variable so that
                // we can destroy dm before return
                var products = _dataManager.ProductRepo.getProductFullInfoList().ToList();

                if (_contextAtomic)
                    _dataManager.Dispose();

                // return product list
                return products;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public List<MyModel.Product> ProductListbySupplierCode(int supplierCode)
        {
            try
            {
                _dataManager = FactoryManager.Instance().GetRepositoryManager();

                // get products list into variable so that
                // we can destroy dm before return
                var products = _dataManager.ProductRepo.Where(p => p.supplier_code == supplierCode).ToList();

                if (_contextAtomic)
                    _dataManager.Dispose();

                // return product list
                return products;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void Add(MyModel.Product p)
        {
            try
            {
                 _dataManager = FactoryManager.Instance().GetRepositoryManager();

                // SaveChanges
                _dataManager.ProductRepo.Add(p);

                if (_contextAtomic)
                    _dataManager.Dispose();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public void Delete(MyModel.Product p)
        {
            try
            {
                _dataManager = FactoryManager.Instance().GetRepositoryManager();

                // SaveChanges
                _dataManager.ProductRepo.Delete(p);

                if (_contextAtomic)
                    _dataManager.Dispose();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public void Deactivate(MyModel.Product p)
        {
            try
            {
                _dataManager = FactoryManager.Instance().GetRepositoryManager();

                // SaveChanges
                p.status = "Deactive";
                _dataManager.ProductRepo.Save(p);

                if (_contextAtomic)
                    _dataManager.Dispose();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void Update(MyModel.Product p)
        {
            try
            {
                _dataManager = FactoryManager.Instance().GetRepositoryManager();

                // SaveChanges
                _dataManager.ProductRepo.Save(p);

                if (_contextAtomic)
                    _dataManager.Dispose();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public void Update(MyModel.Product p, IEnumerable<MyModel.Products_Ums> puList)
        {
            try
            {
                _dataManager = FactoryManager.Instance().GetRepositoryManager();

                // Save Product
                _dataManager.ProductRepo.Save(p);

                // Save Product Um List
                foreach (var item in puList)
                {
                    if (item.mode == MyModel.Mode.Add)
                        _dataManager.Products_UmRepo.Add(item);
                    else if (item.mode == MyModel.Mode.Edit)
                    {
                        if (item.isApply == true)
                            _dataManager.Products_UmRepo.Save(item);
                        else // item.isApply == false
                            _dataManager.Products_UmRepo.Delete(item);
                    }
                }

                if (_contextAtomic)
                    _dataManager.Dispose();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public int GetCount(Expression<Func<MyModel.Product, bool>> filter)
        {
            try
            {
                _dataManager = FactoryManager.Instance().GetRepositoryManager();

                int productsCount = _dataManager.ProductRepo.Count(filter);

                if (_contextAtomic)
                    _dataManager.Dispose();

                // return product count
                return productsCount;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        #endregion 
  
        #region BusinessRelatedMethods

        public bool ChangeSupplier(MyModel.Product p, int supplierId)
        {
            var supplier = _dataManager.SupplierRepo.Get(supplierId);
            if (supplier != null)
            {
                p.Supplier = supplier;
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion
    }
}
