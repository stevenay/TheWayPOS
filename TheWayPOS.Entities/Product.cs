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
    
    public partial class Product
    {
        public Product()
        {
            this.Products_Ums = new HashSet<Products_Ums>();
            this.PurchaseOrderDetails = new HashSet<PurchaseOrderDetail>();
        }
    
        public int product_code { get; set; }
        public string product_name { get; set; }
        public Nullable<int> product_category_code { get; set; }
        public int buying_price { get; set; }
        public Nullable<int> buying_price_um_code { get; set; }
        public Nullable<decimal> discount_percentage { get; set; }
        public Nullable<int> retail_price { get; set; }
        public Nullable<int> wholesale_price { get; set; }
        public Nullable<int> supplier_code { get; set; }
        public Nullable<int> stock_quantity { get; set; }
        public string status { get; set; }
        public Nullable<System.DateTime> created_at { get; set; }
        public Nullable<System.DateTime> updated_at { get; set; }
    
        public virtual ProductCategory ProductCategory { get; set; }
        public virtual Supplier Supplier { get; set; }
        public virtual Um Um { get; set; }
        public virtual ICollection<Products_Ums> Products_Ums { get; set; }
        public virtual ICollection<PurchaseOrderDetail> PurchaseOrderDetails { get; set; }
    }
}
