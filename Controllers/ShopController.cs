using KeyMax.DataQuery;
using KeyMax.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KeyMax.Controllers
{
    public class ShopController : Controller
    {
        QueryData QD = new QueryData();

        // GET: Shop
        public ActionResult Index()
        {
            string keyword = Request.QueryString["keyword"];
            if (keyword != null) ViewData["listProducts"] = QD.GetProductsWithType(keyword, 0, 0);
            else ViewData["listProducts"] = QD.GetProductsWithType("", 0, 0);
            return View();
        }
        public ActionResult Detail(int? id)
        {
            if (id == null) return HttpNotFound();
            ProductWithType p = QD.GetProductWithType((int)id);
            if (p == null) return HttpNotFound();
            ViewData["product"] = p;
            ViewData["productRelated"] = QD.GetProductsWithTypeByProductTypeId((int)p.Product.product_type_id);
            return View();
        }
    }
}