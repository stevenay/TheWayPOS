using System;
using System.Diagnostics;
using System.Linq;
using TheWayPOS.Entities;
using TheWayPOS.DAL;
using TheWayPOS.DAL.GenericInterfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TheWayPOS.DALTest
{
    [TestClass]
    public class ProductTests
    {
        //public IDataManager dm;

        //[TestInitialize]
        //public void Initialise()
        //{
        //    dm = fm.GetRepositoryManager();
        //}

        //[TestMethod]
        //public void Query_AllProducts()
        //{
        //    var productList = dm.GetProductManager().Where(p => p.buying_price == 2);

        //    foreach (var item in productList)
        //    {
        //        Trace.TraceInformation("Product Name: {0}", item.product_code);
        //    }
        //}

        //[TestMethod]
        //public void Query_RelatedCategory()
        //{
        //    var cate = dm.GetProductManager().getProductCategory(6);

        //    // Trace.TraceInformation("Category Name: {0}", cate.category_name);
        //    Assert.IsNotNull(cate);
        //}

        //[TestMethod]
        //public void Test_AddNewProduct()
        //{
        //    var productManager = dm.GetProductManager();
        //    var umManager = dm.GetUmManager();
        //    var productsUmManager = dm.GetProducts_UmManager();

        //    // Create new product
        //    Entities.Product p = new Entities.Product()
        //    {
        //        product_name = "Test Product",
        //        product_category_code = 2,
        //        buying_price = 100,
        //        buying_price_um_code = 1,
        //        supplier_code = 2,
        //        discount_percentage = 3,
        //        retail_price = 110,
        //        wholesale_price = 105,
        //        stock_quantity = 100
        //    };

        //    // Get Um
        //    Entities.Um u = umManager.Get(1);
            
        //    // Create new Products_Ums
        //    Entities.Products_Ums pu = new Entities.Products_Ums();
        //    pu.Product = p;
        //    pu.um_code = u.um_code;

        //    int countBeforeAdd = productManager.Count();
        //    productsUmManager.Add(pu);
        //    dm.SaveChanges();
        //    int countAfterAdd = productManager.Count();

        //    Assert.AreEqual(countAfterAdd, (countBeforeAdd + 1));
        //}
    }
}
