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
            if (inv.user_id != (int)Session["user_id"]) return RedirectToAction("Index", "Home");
            ViewData["listInvd"] = QD.GetInvoiceDetails(inv.invoice_id);
            return View();
        }
        public ActionResult Cancel(int id)
        {
            InvoiceWithStatus inv = QD.GetInvoice(id);
            inv.invoice_status_id = 3;
            if (inv != null) QD.UpdateInvoice(inv);
            return RedirectToAction("Index", "User");
        }
    }
}