using KeyMax.DataQuery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KeyMax.Controllers
{
    public class HomeController : Controller
    {
        QueryData QD = new QueryData();

        public ActionResult Index()
        {
            ViewData["listProducts"] = QD.GetProductsWithType("", 1, 0, 1, 8);
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}