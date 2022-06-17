using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KeyMax.Models
{
    public class Cart
    {
        public int? user_id { get; set; }
        public ProductWithType product { get; set; }
        public int cart_product_quantity { get; set; }
        public Cart() { }
        public Cart(int product_id)
        {
            product = new ProductWithType();
            product.product_id = product_id;
            cart_product_quantity = 0;
        }
        public Cart(int product_id, int cart_product_quantity)
        {
            product = new ProductWithType();
            product.product_id = product_id;
            this.cart_product_quantity = cart_product_quantity > 0 ? cart_product_quantity : 1;
        }
    }
}