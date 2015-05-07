namespace TheWayPOS.DAL.EfDomainManagers
{
    using System;
    using System.Data.Entity;
    using System.Collections.Generic;
    using System.Linq;
    using DomainInterfaces;

    // actually it's Product Category Repository
    class ProductCategoryManager : DomainBase<DbContext, Entities.ProductCategory, int>, IProductCategoryManager
    {
        private readonly DbContext _context;

        public ProductCategoryManager(DbContext context) : base(context)
        {
            _context = context; 
        }

        public IEnumerable<Entities.ProductCategory> getProductCategoryFullInfoList()
        {
            return _context.Set<Entities.ProductCategory>().Include(p => p.Products).ToList();
        }
    }
}
