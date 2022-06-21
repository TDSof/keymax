using KeyMax.DataQuery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KeyMax.Areas.Admin.Controllers
{
    public class LoginController : Controller
    {
        QueryData QD = new QueryData();
        // GET: Admin/Login
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(Object obj)
        {
            if (QD.LoginAdmin(HttpContext.Request.Form["password"]))
            {
                Session["admin"] = true;
                return RedirectToAction("Index", "Dashboard");
            }
            return View();
        }
    }
}