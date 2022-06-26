using KeyMax.DataQuery;
using KeyMax.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KeyMax.Areas.Admin.Controllers
{
    public class ReportsController : Controller
    {
        QueryData QD = new QueryData();
        // GET: Admin/Reports
        public ActionResult Index()
        {
            ViewData["listReports"] = QD.GetReports();
            return View();
        }
        public ActionResult Detail(int report_id)
        {
            reports r = QD.GetReport(report_id);
            if (r == null) return RedirectToAction("Index");
            ViewData["report"] = r;
            return View();
        }
    }
}