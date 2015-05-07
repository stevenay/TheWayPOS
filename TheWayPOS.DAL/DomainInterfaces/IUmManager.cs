namespace TheWayPOS.DAL.DomainInterfaces
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using TheWayPOS.Entities;

    // IProductManager is just like ProductRepositoryInterface
    public interface IUmManager : GenericInterfaces.IDomainManager<Um, int>
    {
        // void Update(int id, Product product);
    }
}

