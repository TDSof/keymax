using KeyMax.DataQuery;
using KeyMax.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KeyMax.Areas.Admin.Controllers
{
    public class InvoicesController : Controller
    {
        QueryData QD = new QueryData();
        // GET: Admin/Invoices
        public ActionResult Index()
        {
            ViewData["listInv"] = QD.GetInvoices();
            return View();
        }
        public ActionResult Detail(int invoice_id)
        {
            InvoiceWithStatus inv = QD.GetInvoice(invoice_id);
            if (inv == null) return RedirectToAction("Index");
            List<ProductWithType> listPwt = QD.GetInvoiceDetails(invoice_id);
            ViewData["invoice"] = inv;
            ViewData["listProducts"] = listPwt;
            ViewData["listINVS"] = QD.GetInvoiceStatus();
            return View();
        }
        [HttpPost]
        public ActionResult Detail(int invoice_id, int? invoice_status_id)
        {
            InvoiceWithStatus inv = QD.GetInvoice(invoice_id);
            if (inv == null) return RedirectToAction("Index");
            List<ProductWithType> listPwt = QD.GetInvoiceDetails(invoice_id);
            ViewData["invoice"] = inv;
            ViewData["listProducts"] = listPwt;
            return View();
        }
        public ActionResult Update(int invoice_id, int invoice_status_id)
        {
            InvoiceWithStatus inv = QD.GetInvoice(invoice_id);
            if (inv == null) return RedirectToAction("Index");
            inv.invoice_status_id = invoice_status_id;
            QD.UpdateInvoice(inv);
            return Redirect(Request.UrlReferrer.ToString());
        }
        public ActionResult Print(int invoice_id)
        {
            InvoiceWithStatus inv = QD.GetInvoice(invoice_id);
            if (inv == null) return RedirectToAction("Index");
            List<ProductWithType> listPwt = QD.GetInvoiceDetails(invoice_id);
            ViewData["invoice"] = inv;
            ViewData["listProducts"] = listPwt;
            return View();
        }
    }
}