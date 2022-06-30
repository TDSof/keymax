using KeyMax.DataQuery;
using KeyMax.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KeyMax.Areas.Admin.Controllers
{
    public class ProductsController : Controller
    {
        QueryData QD = new QueryData();

        // GET: Admin/Product
        public ActionResult Index()
        {
            ViewData["listProducts"] = QD.GetProductsWithType("", 1, 0, 0, 0, -1);
            return View();
        }
        public ActionResult Add()
        {
            ViewData["listPT"] = QD.GetProductTypes();
            return View();
        }
        [HttpPost]
        public ActionResult Add(products p)
        {
            QD.PostProduct(p, out string msg);
            if (string.IsNullOrEmpty(msg)) msg = "Thêm thành công!";
            ViewData["msg"] = msg;
            //ViewData["listPT"] = QD.GetProductTypes();
            //return View();
            return RedirectToAction("Index");
        }
        public ActionResult Edit(int? id)
        {
            if (id == null) return RedirectToAction("Index");
            products p = QD.GetProductWithType((int)id);
            if (p == null) return RedirectToAction("Index");
            ViewData["listPT"] = QD.GetProductTypes();
            ViewData["product"] = p;
            return View();
        }
        [HttpPost]
        public ActionResult Edit(int? id, products prod)
        {
            if (id == null) return RedirectToAction("Index");
            products p = QD.GetProductWithType((int)id);
            if (p == null) return RedirectToAction("Index");
            QD.UpdateProduct(prod, out string msg);
            if(String.IsNullOrEmpty(msg))
            {
                p = QD.GetProductWithType((int)id);
                msg = "Sửa thành công!";
            }
            ViewData["msg"] = msg;
            ViewData["listPT"] = QD.GetProductTypes();
            ViewData["product"] = p;
            return View();
        }
    }
}