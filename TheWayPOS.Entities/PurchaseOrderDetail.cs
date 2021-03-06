//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TheWayPOS.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class PurchaseOrderDetail
    {
        public PurchaseOrderDetail()
        {
            this.PurchaseOrderDetail_Ums = new HashSet<PurchaseOrderDetail_Ums>();
        }
    
        public int purchase_order_detail_id { get; set; }
        public int purchase_order_code { get; set; }
        public int supplier_code { get; set; }
        public int product_code { get; set; }
        public int buying_price { get; set; }
        public int buying_price_um_code { get; set; }
        public Nullable<decimal> discount_percentage { get; set; }
        public int quantity { get; set; }
        public Nullable<int> line_total { get; set; }
        public Nullable<System.DateTime> created_at { get; set; }
        public Nullable<System.DateTime> updated_at { get; set; }
    
        public virtual Product Product { get; set; }
        public virtual ICollection<PurchaseOrderDetail_Ums> PurchaseOrderDetail_Ums { get; set; }
        public virtual PurchaseOrderHeader PurchaseOrderHeader { get; set; }
        public virtual Um Um { get; set; }
    }
}
