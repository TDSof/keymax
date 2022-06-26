using KeyMax.DataQuery;
using KeyMax.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KeyMax.Controllers
{
    public class HomeController : Controller
    {
        Func f = new Func();
        QueryData QD = new QueryData();

        public ActionResult Index()
        {
            ViewData["listProducts"] = QD.GetProductsWithType("", 1, 0, 1, 8);
            return View();
        }

        public ActionResult About()
        {
            return View();
        }
        [HttpPost]
        public ActionResult About(reports r)
        {
            string msg;
            if (QD.VerifyRecaptcha(Request.Form["g-recaptcha-response"]))
            {
                QD.PostReport(r, out msg);
                if (string.IsNullOrEmpty(msg)) msg = "Gửi thông tin thành công!";
            }
            else msg = "Xác minh không thành công. Vui lòng thử lại!";
            ViewData["msg"] = msg;
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}