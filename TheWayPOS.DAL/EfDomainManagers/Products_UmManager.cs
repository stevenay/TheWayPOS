namespace TheWayPOS.DAL.EfDomainManagers
{
    using System;
    using System.Data.Entity;
    using System.Collections.Generic;
    using System.Linq;
    using DomainInterfaces;

    // actually it's Product Repository
    class Products_UmManager : DomainBase<DbContext, Entities.Products_Ums, int>, IProducts_UmManager
    {
        private readonly DbContext _context;

        public Products_UmManager(DbContext context)
            : base(context)
        {
            _context = context;
        }
    }
}
