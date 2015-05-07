using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using TheWayPOS.DAL;
using MyModel = TheWayPOS.Entities;

namespace TheWayPOS.BL
{
    public class PurchaseOrderManager : BusinessLogicManagerBase
    {
        #region Constructors

        /// <summary>
        /// Set true to ContextAtomic Parameter if you want to instantly dispose the Context Vairable
        /// Set false to ContextAtomic Parameter if you want to keep your Context Alive throughout the class Life
        /// </summary>
        /// <param name="contextAtomic"></param>
        public PurchaseOrderManager(bool contextAtomic = false)
            : base()
        {
        }

        #endregion

        #region CRUD Operations Set
        public void Add(MyModel.PurchaseOrderHeader poh)
        {
            try
            {
                _dataManager = FactoryManager.Instance().GetRepositoryManager();

                // SaveChanges
                _dataManager.PurchaseOrderHeaderRepo.Add(poh);

                if (_contextAtomic)
                    _dataManager.Dispose();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public void AddOrderDetail(MyModel.PurchaseOrderDetail pod)
        {
            try
            {
                _dataManager = FactoryManager.Instance().GetRepositoryManager();

                // SaveChanges
                _dataManager.PurchaseOrderDetailRepo.Add(pod);

                if (_contextAtomic)
                    _dataManager.Dispose();

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void Update(MyModel.PurchaseOrderHeader poh)
        {
            try
            {
                _dataManager = FactoryManager.Instance().GetRepositoryManager();

                // SaveChanges
                _dataManager.PurchaseOrderHeaderRepo.Save(poh);

                if (_contextAtomic)
                    _dataManager.Dispose();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public void UpdateOrderDetail(MyModel.PurchaseOrderDetail pod)
        {
            try
            {
                _dataManager = FactoryManager.Instance().GetRepositoryManager();

                // SaveChanges
                _dataManager.PurchaseOrderDetailRepo.Save(pod);

                if (_contextAtomic)
                    _dataManager.Dispose();

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void Delete(MyModel.PurchaseOrderHeader poh)
        {
            try
            {
                _dataManager = FactoryManager.Instance().GetRepositoryManager();

                // Delete PurchaseOrderHeader
                _dataManager.PurchaseOrderHeaderRepo.Delete(poh);

                if (_contextAtomic)
                    _dataManager.Dispose();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public void DeleteOrderDetail(MyModel.PurchaseOrderDetail pod)
        {
            try
            {
                _dataManager = FactoryManager.Instance().GetRepositoryManager();

                // Delete 
                _dataManager.PurchaseOrderDetailRepo.Delete(pod);

                if (_contextAtomic)
                    _dataManager.Dispose();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<MyModel.PurchaseOrderDetail_Ums> PurchaseOrderDetailUmListbyPurhcaesOrderDetailId(int purchaseOrderDetailId)
        {
            try
            {
                _dataManager = FactoryManager.Instance().GetRepositoryManager();

                // get products list into variable so that
                // we can destroy dm before return
                var podums = _dataManager.PurchaseOrderDetailUmRepo.Where(podu => podu.purchase_order_detail_id == purchaseOrderDetailId).ToList();

                if (_contextAtomic)
                    _dataManager.Dispose();

                // return product list
                return podums;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public List<MyModel.PurchaseOrderDetail> PurchaseOrderDetailListbyPurhcaesOrderHeaderId(int purchaseOrderCode)
        {
            try
            {
                _dataManager = FactoryManager.Instance().GetRepositoryManager();

                // get products list into variable so that
                // we can destroy dm before return
                var pods = _dataManager.PurchaseOrderDetailRepo.Where(pod => pod.purchase_order_code == purchaseOrderCode).ToList();

                if (_contextAtomic)
                    _dataManager.Dispose();

                // return product list
                return pods;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        #endregion

    }
}
