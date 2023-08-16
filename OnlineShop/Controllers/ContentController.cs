using OnlineShop.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineShop.Controllers
{
    public class ContentController : Controller
    {
        // GET: Content
        public ActionResult Index(int pageIndex = 1, int pageSize = 8)
        {
            var model = new ContentDao().ListAllPaging(pageIndex, pageSize);
            return View(model);
        }
        public ActionResult Detail(long id)
        {
            var model = new ContentDao().GetByID(id);
            ViewBag.Tags = new ContentDao().ListTag(id);
            return View(model);
        }
        public ActionResult ContentByTag(string tagId, int pageIndex = 1, int pageSize = 8)
        {
            var model = new ContentDao().ListAllByTag(tagId, pageIndex, pageSize);
            ViewBag.Tag = new ContentDao().GetTag(tagId);
            return View(model);
        }    
    }
}