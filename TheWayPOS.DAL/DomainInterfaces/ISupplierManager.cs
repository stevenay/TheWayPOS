namespace TheWayPOS.DAL.DomainInterfaces
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using TheWayPOS.Entities;

    // ISupplierManager is just like SupplierRepositoryInterface
    public interface ISupplierManager : GenericInterfaces.IDomainManager<Supplier, int>
    {
        // void Update(int id, Product product);

    }
}
