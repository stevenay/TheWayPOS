namespace TheWayPOS.DAL.EfDomainManagers
{
    using System;
    using System.Data.Entity;
    using System.Collections.Generic;
    using System.Linq;
    using DomainInterfaces;

    public class PurchaseOrderDetailManager : DomainBase<DbContext, Entities.PurchaseOrderDetail, int>, IPurchaseOrderDetailManager
    {
        private readonly DbContext _context;

        public PurchaseOrderDetailManager(DbContext context)
            : base(context)
        {
            _context = context; 
        }
    }
}
