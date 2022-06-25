using KeyMax.DataQuery;
using KeyMax.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KeyMax.Controllers
{
    public class InvoiceController : Controller
    {
        QueryData QD = new QueryData();
        // GET: Invoice
        public ActionResult Detail(int? invoice_id)
        {
            if (invoice_id == null) return RedirectToAction("Index", "Home");
            InvoiceWithStatus inv = QD.GetInvoice((int)invoice_id);
            if (inv == null) return RedirectToAction("Index", "Home");
            ViewData["invoice"] = inv;
            ViewData["listInvd"] = QD.GetInvoiceDetails(inv.invoice_id);
            return View();
        }
    }
}