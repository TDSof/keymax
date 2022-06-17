using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KeyMax.Models
{
    public class ProductWithType
    {
        public int product_id { get; set; }
        public string product_name { get; set; }
        public int? product_type_id { get; set; }
        public int product_price { get; set; }
        public string product_img { get; set; }
        public int? product_quantity { get; set; }
        public string product_description { get; set; }
        public string product_type_name { get; set; }
    }
}