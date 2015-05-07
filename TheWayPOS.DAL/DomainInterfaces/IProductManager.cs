namespace TheWayPOS.DAL.DomainInterfaces
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using TheWayPOS.Entities;

    // IProductManager is just like ProductRepositoryInterface
    public interface IProductManager : GenericInterfaces.IDomainManager<Product, int>
    {
        // void Update(int id, Product product);
        Entities.ProductCategory getProductCategory(int productCode);
        IEnumerable<Entities.Product> getProductFullInfoList();
        Entities.Product getSameNameProductbySupplierCode(int productCode, int supplierCode);
    }
}
