namespace TheWayPOS.DAL.EfDomainManagers
{
    using System;
    using System.Data.Entity;
    using System.Collections.Generic;
    using System.Linq;
    using DomainInterfaces;

    // actually it's Purchase Order Header Repository
    public class PurchaseOrderHeaderManager : DomainBase<DbContext, Entities.PurchaseOrderHeader, int>, IPurchaseOrderHeaderManager
    {
        private readonly DbContext _context;

        public PurchaseOrderHeaderManager(DbContext context) : base(context)
        {
            _context = context; 
        }
    }
}
