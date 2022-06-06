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
            string orderBy = Request.QueryString["order_by"];
            string productTypeId = Request.QueryString["product_type_id"];
            int order_by = 1, product_type_id = 0;
            if (keyword == null) keyword = "";
            if (orderBy != null) int.TryParse(orderBy, out order_by);
            if (productTypeId != null) int.TryParse(productTypeId, out product_type_id);
            switch (order_by)
            {
                case 2:
                    ViewData["orderBy"] = "Giá tăng dần";
                    break;
                case 3:
                    ViewData["orderBy"] = "Giá giảm dần";
                    break;
                default:
                    ViewData["orderBy"] = "Mới cập nhật";
                    break;
            }
            ViewData["listProducts"] = QD.GetProductsWithType(keyword, order_by, product_type_id, 0, 0);
            ViewData["listPT"] = QD.GetProductTypes();
            ViewBag.productTypeId = product_type_id;
            return View();
        }
        public ActionResult Detail(int? id)
        {
            if (id == null) return HttpNotFound();
            ProductWithType p = QD.GetProductWithType((int)id);
            if (p == null) return HttpNotFound();
            ViewData["product"] = p;
            ViewData["productRelated"] = QD.GetProductsWithType("", 1, (int)p.Product.product_type_id, 1, 8);
            return View();
        }
    }
}