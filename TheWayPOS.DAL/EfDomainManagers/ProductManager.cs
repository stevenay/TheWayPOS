namespace TheWayPOS.DAL.EfDomainManagers
{
    using System;
    using System.Diagnostics;
    using System.Data.Entity;
    using System.Collections.Generic;
    using System.Linq;
    using DomainInterfaces;

    // actually it's Product Repository
    class ProductManager : DomainBase<DbContext, Entities.Product , int>, IProductManager
    {  
        private readonly DbContext _context;

        public ProductManager(DbContext context) : base(context)
        {
            _context = context; 
        }

        public Entities.ProductCategory getProductCategory(int productCode)
        {
            var cate = from pc in _context.Set<Entities.ProductCategory>()
                       from p in _context.Set<Entities.Product>()
                       where p.product_code == productCode
                       where pc.category_code == p.product_category_code
                       select pc;

            return cate.SingleOrDefault<Entities.ProductCategory>();
        }

        public IEnumerable<Entities.Product> getProductFullInfoList()
        {
            return _context.Set<Entities.Product>().Include(p => p.Supplier).ToList();
        }

        public Entities.Product getSameNameProductbySupplierCode(int productCode, int supplierCode)
        {
            var prod = from product1 in _context.Set<Entities.Product>().Where(p => p.supplier_code == supplierCode)
                       from product2 in _context.Set<Entities.Product>().Where(p => p.product_code == productCode)
                       where product1.product_name == product2.product_name
                       select product1;

            return prod.SingleOrDefault<Entities.Product>();
        }
    }
}
