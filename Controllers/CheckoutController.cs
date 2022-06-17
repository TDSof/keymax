﻿using KeyMax.DataQuery;
using KeyMax.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KeyMax.Controllers
{
    public class CheckoutController : Controller
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
        // GET: Checkout
        public ActionResult Index()
        {
            if (Session["user_id"] == null)
            {
                return RedirectToAction("Login", "User");
            }

            List<Cart> listCart = GetCart();
            int count = listCart.Count;
            if (count == 0) RedirectToAction("Index", "Shop");
            ViewData["listCart"] = listCart;
            ViewData["user"] = QD.GetUser((int)Session["user_id"]);
            return View();
        }
    }
}