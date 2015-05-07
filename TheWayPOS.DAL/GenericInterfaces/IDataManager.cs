namespace TheWayPOS.DAL.GenericInterfaces
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using DomainInterfaces;

    public interface IDataManager
    {
        IProductManager ProductRepo{ get; }
        ISupplierManager SupplierRepo{ get; }
        ISupplierCategoryManager SupplierCategoryRepo { get; }
        IUmManager UmRepo{ get; }
        IProducts_UmManager Products_UmRepo{ get; }
        IProductCategoryManager ProductCategoryRepo{ get; }
        IPurchaseOrderHeaderManager PurchaseOrderHeaderRepo { get; }
        IPurchaseOrderDetailManager PurchaseOrderDetailRepo { get; }
        IPurchaseOrderDetailUmManager PurchaseOrderDetailUmRepo { get; }
        IProductArrivalManager ProductArrivalRepo { get; }

        int SaveChanges();
        void Dispose();

        bool IsAlreadyDisposed{ get; }
    }
}
