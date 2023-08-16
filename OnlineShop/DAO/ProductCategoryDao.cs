using OnlineShop.Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineShop.DAO
{
    public class ProductCategoryDao
    {
        OnlineShopDbContext db = null;
        public ProductCategoryDao()
        {
            db = new OnlineShopDbContext();
        }

        public List<ProductCategory> ListAll()
        {
            return db.ProductCategories.Where(x=>x.Status == true).OrderBy(x=>x.DisplayOrder).ToList();
        }
        public ProductCategory CategoryName(long cateId)
        {
            return db.ProductCategories.Find(cateId);
        }
    }
}