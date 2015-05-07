namespace TheWayPOS.DAL.EfDomainManagers
{
    using System;
    using System.Diagnostics;
    using System.Data.Entity;
    using System.Collections.Generic;
    using System.Linq;
    using DomainInterfaces;

    public class SupplierCategoryManager : DomainBase<DbContext, Entities.SupplierCategory, int>, ISupplierCategoryManager
    {
        private readonly DbContext _context;

        public SupplierCategoryManager(DbContext context)
            : base(context)
        {
            _context = context; 
        }
    }
}
