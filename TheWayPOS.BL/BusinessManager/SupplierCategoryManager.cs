using System;
using System.Collections.Generic;
using System.Linq;
using TheWayPOS.DAL;
using TheWayPOS.DAL.GenericInterfaces;
using MyModel = TheWayPOS.Entities;

namespace TheWayPOS.BL
{
    public class SupplierCategoryManager
    {
        #region Properties

        private bool _contextAtomic;
        private IDataManager _dataManager;

        #endregion

        #region Constructors

        private SupplierCategoryManager()
        {
        }

        /// <summary>
        /// Set true to ContextAtomic Parameter if you want to instantly dispose the Context Vairable
        /// Set false to ContextAtomic Parameter if you want to keep your Context Alive throughout the class Life
        /// </summary>
        /// <param name="contextAtomic"></param>
        public SupplierCategoryManager(bool contextAtomic = false)
            : this()
        {
            _contextAtomic = contextAtomic;
            if (!_contextAtomic)
            {
                _dataManager = FactoryManager.Instance().GetRepositoryManager();
            }
        }

        #endregion

        #region CRUD Operations Set

        public MyModel.SupplierCategory GetSupplierCategory(int supplierCategoryId)
        {
            try
            {
                _dataManager = FactoryManager.Instance().GetRepositoryManager();

                MyModel.SupplierCategory s = _dataManager.SupplierCategoryRepo.Get(supplierCategoryId);

                if (_contextAtomic)
                    _dataManager.Dispose();

                return s;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public List<MyModel.SupplierCategory> SupplierCategoryList()
        {
            try
            {
                _dataManager = FactoryManager.Instance().GetRepositoryManager();

                // get products list into variable so that
                // we can destroy dm before return
                var supCategories = _dataManager.SupplierCategoryRepo.All().ToList();

                if (_contextAtomic)
                    FactoryManager.Instance().Dispose();

                // return product list
                return supCategories;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        

        #endregion
    }
}
