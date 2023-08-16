using Microsoft.Ajax.Utilities;
using OnlineShop.Models.EF;
using OnlineShop.ViewModel;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace OnlineShop.DAO
{
    public class ProductDao
    {
        OnlineShopDbContext db = null;
        public ProductDao()
        {
            db = new OnlineShopDbContext();
        }

        public List<Product> ListNewProduct(int top)
        {
            return db.Products.Where(x=>x.Status==true).OrderByDescending(x=>x.CreatedDate).Take(top).ToList();  
        }

        public List<Product> ListFeatureProduct(int top)
        {
            return db.Products.Where(x => x.TopHot != null && x.TopHot > DateTime.Now && x.Status == true).OrderByDescending(x => x.CreatedDate).Take(top).ToList();
        }

        public List<ProductViewModel> ListTopViewProduct(int top)
        {
            var model = from a in db.Products
                        join b in db.ProductCategories
                        on a.CategoryID equals b.ID
                        where a.Status == true
                        select new ProductViewModel()
                        {
                            CateMetatitle = b.MetaTitle,
                            CateName = b.Name,
                            CreatedDate = a.CreatedDate,
                            ID = a.ID,
                            Image = a.Image,
                            Name = a.Name,
                            MetaTitle = a.MetaTitle,
                            Price = a.Price
                        };
            return model.Take(top).ToList();
        }
        public IEnumerable<Product> ProductByCate(long cateId, int page, int pageSize)
        {
            IQueryable<Product> model = db.Products;
            return model.Where(x => x.Status == true && x.CategoryID == cateId).OrderByDescending(x => x.CreatedDate).ToPagedList(page, pageSize);
        }
        public List<Product> RelatedProduct(long productId)
        {
            var product = db.Products.Find(productId);
            return db.Products.Where(x => x.Status == true && x.CategoryID == product.CategoryID && x.ID != productId).ToList();
        }

        public Product ViewDetail(long id)
        {
            return db.Products.Find(id);
        }
    }
}