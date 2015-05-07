using System;
using System.Collections.Generic;
using System.Linq;
using TheWayPOS.DAL;
using TheWayPOS.DAL.GenericInterfaces;
using MyModel = TheWayPOS.Entities;

namespace TheWayPOS.BL
{
    public class SupplierManager : BusinessLogicManagerBase
    {

        #region Constructors

        private SupplierManager()
        {
        }

        /// <summary>
        /// Set true to ContextAtomic Parameter if you want to instantly dispose the Context Vairable
        /// Set false to ContextAtomic Parameter if you want to keep your Context Alive throughout the class Life
        /// </summary>
        /// <param name="contextAtomic"></param>
        public SupplierManager(bool contextAtomic = false)
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

        public MyModel.Supplier GetSupplier(int supplierId)
        {
            try
            {
                _dataManager = FactoryManager.Instance().GetRepositoryManager();

                MyModel.Supplier s = _dataManager.SupplierRepo.Get(supplierId);

                if (_contextAtomic)
                    _dataManager.Dispose();

                return s;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public List<MyModel.Supplier> SupplierList()
        {
            try
            {
                _dataManager = FactoryManager.Instance().GetRepositoryManager();

                // get products list into variable so that
                // we can destroy dm before return
                var suppliers = _dataManager.SupplierRepo.All().ToList();

                if (_contextAtomic)
                    FactoryManager.Instance().Dispose();

                // return product list
                return suppliers;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public void Add(MyModel.Supplier s)
        {
            try
            {
                _dataManager = FactoryManager.Instance().GetRepositoryManager();

                // SaveChanges
                _dataManager.SupplierRepo.Add(s);

                if (_contextAtomic)
                    _dataManager.Dispose();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public void Update(MyModel.Supplier s)
        {
            try
            {
                _dataManager = FactoryManager.Instance().GetRepositoryManager();

                // SaveChanges
                _dataManager.SupplierRepo.Save(s);

                if (_contextAtomic)
                    _dataManager.Dispose();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        #endregion
    }
}
