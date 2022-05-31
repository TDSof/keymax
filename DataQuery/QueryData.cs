using KeyMax.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KeyMax.DataQuery
{
    public class ProductWithType {
        public products Product { get; set; }
        public string ProductTypeName { get; set; }
    }
    public class QueryData
    {
        Func f = new Func();

        public bool TestConnect()
        {
            using (var dbContext = new DBContext())
            {
                return dbContext.Database.Exists();
            }
        }

        public List<products> GetProducts(string search = "")
        {
            using (var dbContext = new DBContext())
            {
                if (string.IsNullOrEmpty(search)) return dbContext.products.ToList();
                else return dbContext.products.Where(oh => oh.product_name.ToLower().Contains(search.ToLower())).ToList();
            }
        }
        public List<ProductWithType> GetProductsWithType(string search = "", int page = 1, int limit = 20)
        {
            using (var dbContext = new DBContext())
            {
                if (string.IsNullOrEmpty(search))
                {
                    return (from p in dbContext.products
                             join pt in dbContext.product_types on p.product_type_id equals pt.product_type_id into product_with_type
                             from pwt in product_with_type.DefaultIfEmpty()
                             orderby p.product_id descending
                             select new ProductWithType
                             {
                                 Product = p,
                                 ProductTypeName = pwt.product_type_name
                             }).Take(limit).ToList();
                }
                else
                {
                    return (from p in dbContext.products
                             join pt in dbContext.product_types on p.product_type_id equals pt.product_type_id into product_with_type
                             from pwt in product_with_type.DefaultIfEmpty()
                             where p.product_name.ToLower().Contains(search.ToLower())
                             orderby p.product_id descending
                             select new ProductWithType
                             {
                                 Product = p,
                                 ProductTypeName = pwt.product_type_name
                             }).Take(limit).ToList();
                }
            }
        }
        public products GetProduct(int product_id)
        {
            try
            {
                using (var dbContext = new DBContext())
                {
                    return dbContext.products.SingleOrDefault(s => s.product_id == product_id);
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public ProductWithType GetProductWithType(int product_id)
        {
            try
            {
                using (var dbContext = new DBContext())
                {
                    return (from p in dbContext.products
                            join pt in dbContext.product_types on p.product_type_id equals pt.product_type_id into product_with_type
                            from pwt in product_with_type.DefaultIfEmpty()
                            where p.product_id == product_id
                            select new ProductWithType
                            {
                                Product = p,
                                ProductTypeName = pwt.product_type_name
                            }).FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public List<ProductWithType> GetProductsWithTypeByProductTypeId(int product_type_id, int page = 1, int limit = 10)
        {
            using (var dbContext = new DBContext())
            {
                return (from p in dbContext.products
                    join pt in dbContext.product_types on p.product_type_id equals pt.product_type_id into product_with_type
                    from pwt in product_with_type.DefaultIfEmpty()
                    where p.product_type_id == product_type_id
                    orderby p.product_id descending
                    select new ProductWithType
                    {
                        Product = p,
                        ProductTypeName = pwt.product_type_name
                    }).Take(limit).ToList();
            }
        }
    }
}