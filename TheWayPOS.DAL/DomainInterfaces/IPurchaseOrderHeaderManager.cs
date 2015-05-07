namespace TheWayPOS.DAL.DomainInterfaces
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using TheWayPOS.Entities;

    // IPurchaseOrderHeaderManager is just like PurchaseOrderHeaderRepositoryInterface
    public interface IPurchaseOrderHeaderManager : GenericInterfaces.IDomainManager<PurchaseOrderHeader, int>
    {
    }
}
