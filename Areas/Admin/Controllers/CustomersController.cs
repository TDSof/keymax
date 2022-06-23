using KeyMax.DataQuery;
using KeyMax.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KeyMax.Areas.Admin.Controllers
{
    public class CustomersController : Controller
    {
        QueryData QD = new QueryData();
        // GET: Admin/Customers
        public ActionResult Index()
        {
            ViewData["listCustomers"] = QD.GetUsers();
            return View();
        }
        public ActionResult Detail(int user_id)
        {
            users u = QD.GetUser(user_id);
            if (u != null)
            {
                ViewData["user"] = u;
                return View();
            }
            return RedirectToAction("Index");
        }
    }
}