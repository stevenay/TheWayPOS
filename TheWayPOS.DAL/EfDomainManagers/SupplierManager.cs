namespace TheWayPOS.DAL.EfDomainManagers
{
    using System;
    using System.Diagnostics;
    using System.Data.Entity;
    using System.Collections.Generic;
    using System.Linq;
    using DomainInterfaces;

    // actually it's Product Repository
    class SupplierManager : DomainBase<DbContext, Entities.Supplier, int>, ISupplierManager
    {
        private readonly DbContext _context;

        public SupplierManager(DbContext context)
            : base(context)
        {
            _context = context;
        }
    }
}