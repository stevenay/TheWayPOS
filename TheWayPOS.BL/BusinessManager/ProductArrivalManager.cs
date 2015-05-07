using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using TheWayPOS.DAL;
using MyModel = TheWayPOS.Entities;

namespace TheWayPOS.BL
{
    public class ProductArrivalManager : BusinessLogicManagerBase
    {
        #region Constructors

        /// <summary>
        /// Set true to ContextAtomic Parameter if you want to instantly dispose the Context Vairable
        /// Set false to ContextAtomic Parameter if you want to keep your Context Alive throughout the class Life
        /// </summary>
        /// <param name="contextAtomic"></param>
        public ProductArrivalManager(bool contextAtomic = false)
            : base()
        {
        }

        #endregion

        #region CRUD Operations Set
        public List<MyModel.ProductArrival> ProductArrivalList()
        {
            try
            {
                _dataManager = FactoryManager.Instance().GetRepositoryManager();

                // get products list into variable so that
                // we can destroy dm before return
                var ums = _dataManager.ProductArrivalRepo.All().ToList();

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
        #endregion
    }
}
