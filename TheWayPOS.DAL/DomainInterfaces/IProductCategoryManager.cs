namespace TheWayPOS.DAL.DomainInterfaces
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using TheWayPOS.Entities;

    public interface IProductCategoryManager : GenericInterfaces.IDomainManager<ProductCategory, int>
    {
        IEnumerable<Entities.ProductCategory> getProductCategoryFullInfoList();
    }
}
