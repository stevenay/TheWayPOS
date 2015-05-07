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
    public class UmManager : BusinessLogicManagerBase
    {
        #region Constructors

        /// <summary>
        /// Set true to ContextAtomic Parameter if you want to instantly dispose the Context Vairable
        /// Set false to ContextAtomic Parameter if you want to keep your Context Alive throughout the class Life
        /// </summary>
        /// <param name="contextAtomic"></param>
        public UmManager(bool contextAtomic = false)
            : base()
        {
        }

        #endregion

        #region CRUD Operations Set
        public List<MyModel.Um> UmList()
        {
            try
            {
                _dataManager = FactoryManager.Instance().GetRepositoryManager();

                // get products list into variable so that
                // we can destroy dm before return
                var ums = _dataManager.UmRepo.All().ToList();

                if (_contextAtomic)
                    _dataManager.Dispose();

                // return product list
                return ums;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public void Add(MyModel.Um u)
        {
            try
            {
                _dataManager = FactoryManager.Instance().GetRepositoryManager();

                // SaveChanges
                _dataManager.UmRepo.Add(u);

                if (_contextAtomic)
                    _dataManager.Dispose();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public void Update(MyModel.Um u)
        {
            try
            {
                _dataManager = FactoryManager.Instance().GetRepositoryManager();

                // SaveChanges
                _dataManager.UmRepo.Save(u);

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
