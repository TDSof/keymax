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

        // Hàm Login trả về: -1: Tài khoản không có / 0: Sai mật khẩu / >=1: Đăng nhập thành công!
        public int Login(string email, string pass, out users user)
        {
            user = null;
            string pass_md5 = f.CreateMD5(pass);
            using (var dbContext = new DBContext())
            {
                users x = dbContext.users.SingleOrDefault(s => s.user_email == email);
                if (x == null) return -1;
                else
                {
                    if (x.user_password.Equals(pass_md5))
                    {
                        user = x;
                        return x.user_id;
                    }
                    else return 0;
                }
            }
        }
        public int Reg(string fullname, string email, string pass)
        {
            using (var dbContext = new DBContext())
            {
                users x = dbContext.users.SingleOrDefault(s => s.user_email == email);
                if (x == null)
                {
                    x = new users();
                    x.user_fullname = fullname;
                    x.user_email = email.ToLower();
                    x.user_password = f.CreateMD5(pass);
                    x.user_created_at = DateTime.Now;
                    dbContext.users.Add(x);
                    dbContext.SaveChanges();
                    return x.user_id;
                }
                else
                {
                    return 0;
                }
            }
        }
        public bool ChangePass(int user_id, string pass_old, string pass_new, out string err)
        {
            err = string.Empty;
            try
            {
                string pass_md5 = f.CreateMD5(pass_old);
                string pass_md5_ = f.CreateMD5(pass_new);
                using (var dbContext = new DBContext())
                {
                    users x = dbContext.users.SingleOrDefault(s => s.user_id == user_id);
                    if (x == null)
                    {
                        err = "Tài khoản không tìm thấy!";
                        return false;
                    }
                    else
                    {
                        if (x.user_password.Equals(pass_md5))
                        {
                            x.user_password = pass_md5_;
                            dbContext.SaveChanges();
                            return true;
                        }
                        else
                        {
                            err = "Mật khẩu cũ không chính xác!";
                            return false;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                err = e.Message;
                return false;
            }
        }
        public users GetUser(int user_id)
        {
            using (var dbContext = new DBContext())
            {
                users x = dbContext.users.SingleOrDefault(s => s.user_id == user_id);
                return x;
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
                IQueryable<ProductWithType> list = null;
                if (string.IsNullOrEmpty(search))
                {
                    list = (from p in dbContext.products
                            join pt in dbContext.product_types on p.product_type_id equals pt.product_type_id into product_with_type
                            from pwt in product_with_type.DefaultIfEmpty()
                            orderby p.product_id descending
                            select new ProductWithType
                            {
                                Product = p,
                                ProductTypeName = pwt.product_type_name
                            });
                }
                else
                {
                    list = (from p in dbContext.products
                            join pt in dbContext.product_types on p.product_type_id equals pt.product_type_id into product_with_type
                            from pwt in product_with_type.DefaultIfEmpty()
                            where p.product_name.ToLower().Contains(search.ToLower())
                            orderby p.product_id descending
                            select new ProductWithType
                            {
                                Product = p,
                                ProductTypeName = pwt.product_type_name
                            });
                }
                if (limit == 0) return list.ToList();
                else return list.Skip((page - 1) *  limit).Take(limit).ToList();
            }
        }
        public products GetProduct(int product_id)
        {
            using (var dbContext = new DBContext())
            {
                return dbContext.products.SingleOrDefault(s => s.product_id == product_id);
            }
        }
        public ProductWithType GetProductWithType(int product_id)
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