using OnlineShop.Commons;
using OnlineShop.DAO;
using OnlineShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineShop.Areas.Admin.Controllers
{
    public class LoginController : Controller
    {
        // GET: Admin/Login
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Login(LoginModel model) 
        {
            if (ModelState.IsValid)
            {
                var dao = new UserDao();
                var rs = dao.Login(model.Username, EncryptorMD5.MD5Hash(model.Password), true);
                if (rs == 1)
                {
                    var user = dao.GetById(model.Username);
                    var userSession = new UserLogin();
                    userSession.UserName = user.Username;
                    userSession.UserID = user.ID;
                    userSession.GroupID = user.GroupID;
                    var listCredential = dao.GetListCredential(model.Username);
                    Session.Add(CommonConstants.SESSION_CREDENTIALS, listCredential);
                    Session.Add(CommonConstants.USER_SESSION, userSession);
                    return RedirectToAction("Index", "Home");
                }
                else if(rs == 0)
                {
                    ModelState.AddModelError("", "Tài khoản không tồn tại!");
                }
                else if(rs == -1) {
                    ModelState.AddModelError("", "Tài khoản đang bị khóa!");
                }
                else if(rs == -2)
                {
                    ModelState.AddModelError("", "Mật khẩu không hợp lệ!");
                }
                else if (rs == -3)
                {
                    ModelState.AddModelError("", "Tài khoản của bạn không có quyền đăng nhập!");
                }
            }
            return View("Index");
        }
    }
}