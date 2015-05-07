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
    public class ProductUmManager : BusinessLogicManagerBase
    {
        #region Constructors

        /// <summary>
        /// Set true to ContextAtomic Parameter if you want to instantly dispose the Context Vairable
        /// Set false to ContextAtomic Parameter if you want to keep your Context Alive throughout the class Life
        /// </summary>
        /// <param name="contextAtomic"></param>
        public ProductUmManager(bool contextAtomic = false)
            : base()
        {
        }

        #endregion

        #region CRUD Operations Set
        public List<MyModel.Products_Ums> ProductUmList()
        {
            try
            {
                if (_contextAtomic)
                    _dataManager = FactoryManager.Instance().GetRepositoryManager();

                // get products list into variable so that
                // we can destroy dm before return
                var pums = _dataManager.Products_UmRepo.All().ToList();

                if (_contextAtomic)
                    _dataManager.Dispose();

                // return product list
                return pums;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public List<MyModel.Products_Ums> ProductUmListbyProductCode(int productCode)
        {
            try
            {
                _dataManager = FactoryManager.Instance().GetRepositoryManager();

                // get products list into variable so that
                // we can destroy dm before return
                var pums = _dataManager.Products_UmRepo.Where(p => p.product_code == productCode).ToList();

                if (_contextAtomic)
                    _dataManager.Dispose();

                // return product list
                return pums;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public void Update(MyModel.Products_Ums pu)
        {
            _dataManager = FactoryManager.Instance().GetRepositoryManager();

            // SaveChanges
            _dataManager.Products_UmRepo.Save(pu);
            // _dataManager.SaveChanges();

            if (_contextAtomic)
                _dataManager.Dispose();
        }
        public void Add(MyModel.Products_Ums pu)
        {
            _dataManager = FactoryManager.Instance().GetRepositoryManager();

            // SaveChanges
            _dataManager.Products_UmRepo.Add(pu);
           //  _dataManager.SaveChanges();

            if (_contextAtomic)
                _dataManager.Dispose();
        }
        public void Delete(MyModel.Products_Ums pu)
        {
            _dataManager = FactoryManager.Instance().GetRepositoryManager();

            // SaveChanges
            _dataManager.Products_UmRepo.Delete(pu);
            // _dataManager.SaveChanges();

            if (_contextAtomic)
                _dataManager.Dispose();
        }
        #endregion
    }
}
