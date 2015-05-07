namespace TheWayPOS.DAL.EfDomainManagers
{
    using System;
    using System.Data.Entity;
    using System.Collections.Generic;
    using System.Linq;
    using DomainInterfaces;

    public class PurchaseOrderDetailUmManager : DomainBase<DbContext, Entities.PurchaseOrderDetail_Ums, int>, IPurchaseOrderDetailUmManager
    {
        private readonly DbContext _context;

        public PurchaseOrderDetailUmManager(DbContext context)
            : base(context)
        {
            _context = context; 
        }
    }
}
