using KeyMax.DataQuery;
using KeyMax.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KeyMax.Controllers
{
    public class UserController : Controller
    {
        QueryData QD = new QueryData();

        // GET: User
        public ActionResult Index()
        {
            if (Session["user_id"] == null)
            {
                return RedirectToAction("Login");
            }
            ViewData["user"] = QD.GetUser((int)Session["user_id"]);
            return View();
        }
        [HttpPost]
        public ActionResult Index(Object user)
        {
            string msg = string.Empty;
            string password = HttpContext.Request.Form["new_password"];
            if (password.Equals(HttpContext.Request.Form["re_new_password"]))
            {
                if (QD.ChangePass((int)Session["user_id"], HttpContext.Request.Form["password"], password, out msg))
                {
                    ViewData["msg"] = "Đổi mật khẩu thành công!";
                    return RedirectToAction("Login");
                }
                //else msg = "Đổi mật khẩu thất bại. Vui lòng thử lại!";
            }
            else msg = "Mật khẩu không trùng nhau. Vui lòng thử lại!";
            ViewData["msg"] = msg;
            return View();
        }

        public ActionResult Login()
        {
            if(Session["user_id"] != null)
            {
                return RedirectToAction("", "Home");
            }
            return View();
        }
        [HttpPost]
        public ActionResult Login(users user)
        {
            string msg = string.Empty;
            int success = QD.Login(user.user_email, user.user_password, out user);
            if (success == -1) msg = "Tài khoản không tồn tại.";
            else if (success == 0) msg = "Tài khoản không chính xác.";
            else
            {
                Session["user_id"] = user.user_id;
                Session["user_fullname"] = user.user_fullname;
                Session["user_email"] = user.user_email;
                return RedirectToAction("", "Home");
            }
            ViewData["msg"] = msg;
            return View();
        }

        public ActionResult Register()
        {
            if (Session["user_id"] != null)
            {
                return RedirectToAction("", "Home");
            }
            return View();
        }
        [HttpPost]
        public ActionResult Register(Object user)
        {
            string msg = string.Empty;
            string password = HttpContext.Request.Form["user_password"];
            if (password.Equals(HttpContext.Request.Form["user_repassword"]))
            {
                int user_id = QD.Reg(HttpContext.Request.Form["user_fullname"], HttpContext.Request.Form["user_email"], password);
                if (user_id > 0)
                {
                    ViewData["msg"] = "Đăng ký thành công. Vui lòng đăng nhập!";
                    return RedirectToAction("Login");
                }
                else msg = "Đăng ký thất bại. Vui lòng thử lại!";
            }
            else msg = "Mật khẩu không trùng nhau. Vui lòng thử lại!";
            ViewData["msg"] = msg;
            return View();
        }
    }
}