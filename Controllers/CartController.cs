using KeyMax.DataQuery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KeyMax.Controllers
{
    public class CartController : Controller
    {
        Func f = new Func();
        QueryData QD = new QueryData();

        public List<Cart> GetCart()
        {

            List<Cart> listCart = (List<Cart>)Session["listCart"];
            if (listCart == null)
            {
                listCart = new List<Cart>();
            }
            listCart = QD.GetCart(listCart);
            Session["listCart"] = listCart;
            return listCart;
        }
        // GET: Cart
        public ActionResult Index()
        {
            if (Session["user_id"] == null)
            {
                return RedirectToAction("Login", "User");
            }
            ViewData["listCart"] = GetCart();
            return View();
        }
    }
}