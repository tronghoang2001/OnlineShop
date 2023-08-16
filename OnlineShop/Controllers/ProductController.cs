using OnlineShop.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace OnlineShop.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        public ActionResult Index()
        {
            return View();
        }
        public PartialViewResult ProductCategory()
        {
            var model = new ProductCategoryDao().ListAll();
            return PartialView(model);
        }

        [OutputCache(CacheProfile = "Cache1DayForProduct")]
        public ActionResult Details(long productId)
        {
            var product = new ProductDao().ViewDetail(productId);
            ViewBag.CategoryName = new ProductCategoryDao().CategoryName(product.CategoryID.Value);
            ViewBag.RelatedProduct = new ProductDao().RelatedProduct(productId);
            return View(product);
        }

        public ActionResult ProductByCate(long cateId, int page = 1, int pageSize = 3)
        {
            var product = new ProductDao().ProductByCate(cateId, page, pageSize);
            ViewBag.CategoryName = new ProductCategoryDao().CategoryName(cateId);
            return View(product);
        }
    }
}