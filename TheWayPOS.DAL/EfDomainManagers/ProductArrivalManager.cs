namespace TheWayPOS.DAL.EfDomainManagers
{
    using System;
    using System.Data.Entity;
    using System.Collections.Generic;
    using System.Linq;
    using DomainInterfaces;

    public class ProductArrivalManager : DomainBase<DbContext, Entities.ProductArrival, int>, IProductArrivalManager
    {
        private readonly DbContext _context;

        public ProductArrivalManager(DbContext context)
            : base(context)
        {
            _context = context; 
        }
    }
}
