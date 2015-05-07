using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Diagnostics;
using TheWayPOS.DAL;
using TheWayPOS.DAL.GenericInterfaces;
using MyModel = TheWayPOS.Entities;

namespace TheWayPOS.BL
{
    public class ProductCategoryManager : BusinessLogicManagerBase
    {
        #region Constructors

        /// <summary>
        /// Set true to ContextAtomic Parameter if you want to instantly dispose the Context Vairable
        /// Set false to ContextAtomic Parameter if you want to keep your Context Alive throughout the class Life
        /// </summary>
        /// <param name="contextAtomic"></param>
        public ProductCategoryManager(bool contextAtomic = false)
            : base()
        {
        }

        #endregion

        #region CRUD Operations Set
        public List<MyModel.ProductCategory> ProductCategoryList()
        {
            try
            {
                _dataManager = FactoryManager.Instance().GetRepositoryManager();
                var categories = _dataManager.ProductCategoryRepo.All().ToList();

                if (_contextAtomic)
                    _dataManager.Dispose();

                // return product list
                return categories;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public List<MyModel.ProductCategory> ProductCategoryFullInfoList()
        {
            try
            {
                _dataManager = FactoryManager.Instance().GetRepositoryManager();
                var categories = _dataManager.ProductCategoryRepo.getProductCategoryFullInfoList().ToList();

                if (_contextAtomic)
                    _dataManager.Dispose();

                // return product list
                return categories;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public void Add(MyModel.ProductCategory pc)
        {
            try
            {
                _dataManager = FactoryManager.Instance().GetRepositoryManager();

                // SaveChanges
                _dataManager.ProductCategoryRepo.Add(pc);

                if (_contextAtomic)
                    _dataManager.Dispose();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public void Update(MyModel.ProductCategory pc)
        {
            try
            {
                _dataManager = FactoryManager.Instance().GetRepositoryManager();

                // SaveChanges
                _dataManager.ProductCategoryRepo.Save(pc);

                if (_contextAtomic)
                    _dataManager.Dispose();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public void Deactivate(MyModel.ProductCategory p)
        {
            try
            {
                _dataManager = FactoryManager.Instance().GetRepositoryManager();

                // SaveChanges
                p.status = "Deactive";
                _dataManager.ProductCategoryRepo.Save(p);

                if (_contextAtomic)
                    _dataManager.Dispose();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
