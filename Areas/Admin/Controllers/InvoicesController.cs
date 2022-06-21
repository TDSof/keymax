using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KeyMax.Areas.Admin.Controllers
{
    public class InvoicesController : Controller
    {
        // GET: Admin/Invoices
        public ActionResult Index()
        {
            return View();
        }
    }
}