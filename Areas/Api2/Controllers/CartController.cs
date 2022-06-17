using KeyMax.DataQuery;
using KeyMax.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KeyMax.Areas.Api2.Controllers
{
    public class CartController : Controller
    {
        Func f = new Func();
        QueryData QD = new QueryData();

        public List<Cart> GetCart()
        {

            List<Cart> listCart = (List<Cart>)Session["listCart"];
            if (listCart == null) {
                listCart = new List<Cart>();
            }
            listCart = QD.GetCart(listCart);
            Session["listCart"] = listCart;
            return listCart;
        }
        // GET: Api2/Cart
        public ActionResult Index()
        {
            List<Cart> listCart = GetCart();
            ReturnApi msg = new ReturnApi { success = true, data = listCart };
            ViewData["listCart"] = JsonConvert.SerializeObject(msg);
            //ViewData["listCart"] = JsonConvert.SerializeObject(QD.GetProductsWithType(), f.JsonSetting());
            Response.ContentType = "text/plain";
            return View();
        }
        [HttpPost]
        public ActionResult Add(int? id)
        {
            ReturnApi msg = new ReturnApi { success = false };
            if (id != null)
            {
                int.TryParse(Request.QueryString["cart_product_quantity"], out int quantity);

                List<Cart> listCart = GetCart();
                Cart sp = listCart.Find(f => f.product.product_id == id);
                if (sp == null) listCart.Add(new Cart((int)id, quantity));
                else
                {
                    if (quantity > 0) sp.cart_product_quantity += quantity;
                    else sp.cart_product_quantity++;
                }
                msg.success = true;
            }
            ViewData["listCart"] = JsonConvert.SerializeObject(msg);
            Response.ContentType = "application/json";
            return View();
        }
        [HttpPost]
        public ActionResult Update(int? id)
        {
            ReturnApi msg = new ReturnApi { success = false };
            if (id != null)
            {
                List<Cart> listCart = GetCart();
                Cart sp = listCart.Find(f => f.product.product_id == id);
                if (sp != null) {
                    if (int.TryParse(Request.QueryString["cart_product_quantity"], out int quantity))
                    {
                        if (quantity > 0) sp.cart_product_quantity = quantity;
                        else listCart.Remove(sp);
                    }
                }
                msg.success = true;
            }
            ViewData["listCart"] = JsonConvert.SerializeObject(msg);
            Response.ContentType = "application/json";
            return View();
        }
        [HttpPost]
        public ActionResult Remove(int? id)
        {
            ReturnApi msg = new ReturnApi { success = false };
            if (id != null)
            {
                List<Cart> listCart = GetCart();
                Cart sp = listCart.Find(f => f.product.product_id == id);
                if (sp != null)
                {
                    listCart.Remove(sp);
                    msg.success = true;
                }
            }
            ViewData["listCart"] = JsonConvert.SerializeObject(msg);
            Response.ContentType = "application/json";
            return View();
        }
    }
}