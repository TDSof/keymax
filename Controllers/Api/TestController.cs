using KeyMax.DataQuery;
using KeyMax.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace KeyMax.Controllers.Api
{
    public class TestController : ApiController
    {
        Func f = new Func();
        QueryData QD = new QueryData();
        public List<ProductWithType> Get()
        {
            var a = QD.GetProductsWithType();
            return a;
        }
        public ProductWithType Get(int id)
        {
            var a = QD.GetProductWithType(id);
            return a;
        }
    }
}
