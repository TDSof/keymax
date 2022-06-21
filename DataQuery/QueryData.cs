using KeyMax.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;

namespace KeyMax.DataQuery
{
    public class TokenGoogle
    {
        //[JsonProperty(PropertyName = "error_description")]
        public string error_description { get; set; }
        //[JsonProperty(PropertyName = "name")]
        public string name { get; set; }
        //[JsonProperty(PropertyName = "email")]
        public string email { get; set; }
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
        public bool LoginAdmin(string password)
        {
            if (password.Equals("keymax@2022")) return true;
            return false;
        }
        public TokenGoogle VerifyTokenGoogle(string token)
        {
            TokenGoogle json = new TokenGoogle();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://www.googleapis.com/oauth2/v3/tokeninfo");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //GET Method
                HttpResponseMessage response = client.GetAsync("?id_token=" + token).Result;
                if (response.IsSuccessStatusCode)
                {
                    var t = JsonConvert.DeserializeObject<TokenGoogle>(response.Content.ReadAsStringAsync().Result);
                    if (t.error_description == null)
                    {
                        json.name = t.name;
                        json.email = t.email;
                    }
                    else json.error_description = t.error_description;
                }
            }
            return json;
        }
        public int LoginWith(string token, string from, out users user)
        {
            user = null;
            if (from.ToLower().Equals("google"))
            {
                TokenGoogle data = VerifyTokenGoogle(token);
                if (data.error_description == null)
                {
                    using (var dbContext = new DBContext())
                    {
                        users x = dbContext.users.SingleOrDefault(s => s.user_email.Equals(data.email));
                        if (x == null)
                        {
                            x = new users();
                            x.user_fullname = data.name;
                            x.user_email = data.email.ToLower();
                            x.user_created_at = DateTime.Now;
                            dbContext.users.Add(x);
                            dbContext.SaveChanges();
                        }
                        user = x;
                        return x.user_id;
                    }
                }
            }
            else if (from.ToLower().Equals("facebook"))
            {

            }
            return 0;
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
        public bool UpdateUser(users user)
        {
            try
            {
                using (var dbContext = new DBContext())
                {
                    var userUpdate = dbContext.users.SingleOrDefault(s => s.user_id == user.user_id);
                    if (userUpdate == null)
                    {
                        return false;
                    }
                    if (userUpdate.user_fullname != user.user_fullname) userUpdate.user_fullname = user.user_fullname;
                    //if (userUpdate.user_email != user.user_email) userUpdate.user_email = user.user_email;
                    if (userUpdate.user_phone_number != user.user_phone_number) userUpdate.user_phone_number = user.user_phone_number;
                    if (userUpdate.user_address != user.user_address) userUpdate.user_address = user.user_address;
                    dbContext.SaveChanges();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        //public List<products> GetProducts(string search = "")
        //{
        //    using (var dbContext = new DBContext())
        //    {
        //        if (string.IsNullOrEmpty(search)) return dbContext.products.ToList();
        //        else return dbContext.products.Where(oh => oh.product_name.ToLower().Contains(search.ToLower())).ToList();
        //    }
        //}
        public List<ProductWithType> GetProductsWithType(string search = "", int order_by = 1, int product_type_id = 0, int page = 1, int limit = 20, int product_published = 1)
        {
            using (var dbContext = new DBContext())
            {
                IQueryable<ProductWithType> list = (from p in dbContext.products
                                                    join pt in dbContext.product_types on p.product_type_id equals pt.product_type_id into product_with_type
                                                    from pwt in product_with_type.DefaultIfEmpty()
                                                    select new ProductWithType
                                                    {
                                                        product_id = p.product_id,
                                                        product_name = p.product_name,
                                                        product_type_id = p.product_type_id,
                                                        product_price = p.product_price,
                                                        product_img = p.product_img,
                                                        product_quantity = p.product_quantity,
                                                        product_description = p.product_description,
                                                        product_type_name = pwt.product_type_name,
                                                        product_published = p.product_published
                                                    });

                if (!string.IsNullOrEmpty(search)) list = list.Where(w => w.product_name.ToLower().Contains(search.ToLower()));
                if (product_type_id > 0) list = list.Where(w => w.product_type_id == product_type_id);
                if (product_published == 0 || product_published == 1) list = list.Where(w => w.product_published == product_published);
                switch (order_by)
                {
                    case 2:
                        list = list.OrderBy(o => o.product_price);
                        break;
                    case 3:
                        list = list.OrderByDescending(o => o.product_price);
                        break;
                    default:
                        list = list.OrderByDescending(o => o.product_id);
                        break;
                }

                if (limit == 0) return list.ToList();
                else return list.Skip((page - 1) *  limit).Take(limit).ToList();
            }
        }
        public int GetCountProducts()
        {
            using (var dbContext = new DBContext())
            {
                return dbContext.products.Count();
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
                            product_id = p.product_id,
                            product_name = p.product_name,
                            product_type_id = p.product_type_id,
                            product_price = p.product_price,
                            product_img = p.product_img,
                            product_quantity = p.product_quantity,
                            product_description = p.product_description,
                            product_published = p.product_published,
                            product_type_name = pwt.product_type_name
                        }).FirstOrDefault();
            }
            //Product = new products
            //{
            //    product_id = p.product_id,
            //    product_name = p.product_name,
            //    product_type_id = p.product_type_id,
            //    product_price = p.product_price,
            //    product_img = p.product_img,
            //    product_quantity = p.product_quantity,
            //    product_description = p.product_description
            //},
        }
        public List<product_types> GetProductTypes()
        {
            using (var dbContext = new DBContext())
            {
                return dbContext.product_types.ToList();
            }
        }

        public List<Cart> GetCart(List<Cart> listCart)
        {
            using (var dbContext = new DBContext())
            {
                return (from c in listCart
                        join p1 in dbContext.products on c.product.product_id equals p1.product_id into products
                        from p in products
                        join pt in dbContext.product_types on p.product_type_id equals pt.product_type_id into product_with_type
                        from pwt in product_with_type.DefaultIfEmpty()
                        select new Cart
                        {
                            product = new ProductWithType
                            {
                                product_id = p.product_id,
                                product_name = p.product_name,
                                product_type_id = p.product_type_id,
                                product_price = p.product_price,
                                product_img = p.product_img,
                                product_quantity = p.product_quantity,
                                product_description = p.product_description,
                                product_type_name = pwt.product_type_name
                            },
                            cart_product_quantity = c.cart_product_quantity
                        }).ToList();
            }
        }
        public int PostProduct(products product, out string err)
        {
            err = string.Empty;
            try
            {
                using (var dbContext = new DBContext())
                {
                    dbContext.products.Add(product);
                    dbContext.SaveChanges();
                    return product.product_id;
                }
            }
            catch (Exception e)
            {
                err = e.Message;
                return 0;
            }
        }
        public bool UpdateProduct(products product, out string err)
        {
            err = string.Empty;
            try
            {
                using (var dbContext = new DBContext())
                {
                    var productUpdate = dbContext.products.SingleOrDefault(s => s.product_id == product.product_id);
                    if (productUpdate == null)
                    {
                        err = "Sản phẩm không tìm thấy!";
                        return false;
                    }
                    if (productUpdate.product_name != product.product_name) productUpdate.product_name = product.product_name;
                    if (productUpdate.product_type_id != product.product_type_id) productUpdate.product_type_id = product.product_type_id;
                    if (productUpdate.product_price != product.product_price) productUpdate.product_price = product.product_price;
                    if (productUpdate.product_img != product.product_img) productUpdate.product_img = product.product_img;
                    if (productUpdate.product_quantity != product.product_quantity) productUpdate.product_quantity = product.product_quantity;
                    if (productUpdate.product_description != product.product_description) productUpdate.product_description = product.product_description;
                    if (productUpdate.product_published != product.product_published) productUpdate.product_published = product.product_published;
                    dbContext.SaveChanges();
                    return true;
                }
            }
            catch (Exception e)
            {
                err = e.Message;
                return false;
            }
        }

        public invoices GetInvoice(int invoice_id)
        {
            using (var dbContext = new DBContext())
            {
                return dbContext.invoices.SingleOrDefault(s => s.invoice_id == invoice_id);
            }
        }
        public int PostInvoice(invoices invoice, int user_id, List<Cart> carts, out string err)
        {
            err = String.Empty;
            using (var dbContext = new DBContext())
            using (var dbContextTransaction = dbContext.Database.BeginTransaction())
            {
                try
                {
                    invoices inv = new invoices();
                    inv.user_id = user_id;
                    inv.invoice_user_fullname = invoice.invoice_user_fullname;
                    inv.invoice_user_phone_number = invoice.invoice_user_phone_number;
                    inv.invoice_user_email = invoice.invoice_user_email;
                    inv.invoice_user_address = invoice.invoice_user_address;
                    inv.invoice_subtotal = carts.Sum(s => s.cart_product_quantity * s.product.product_price);
                    inv.invoice_fee_transport = 0;
                    inv.invoice_created_at = DateTime.Now;
                    dbContext.invoices.Add(inv);
                    dbContext.SaveChanges();
                    foreach(var item in carts)
                    {
                        dbContext.invoice_details.Add(new invoice_details {
                            invoice_id = inv.invoice_id,
                            product_id = item.product.product_id,
                            invd_product_quantity = item.cart_product_quantity,
                            invd_product_price = item.product.product_price
                        });
                        dbContext.SaveChanges();
                    }
                    dbContextTransaction.Commit();
                    return inv.invoice_id;
                }
                catch (Exception e)
                {
                    err = e.Message;
                    dbContextTransaction.Rollback();
                    return 0;
                }
            }
        }
        public List<ProductWithType> GetInvoiceDetails(int invoice_id)
        {
            using (var dbContext = new DBContext())
            {
                return (from invd in dbContext.invoice_details
                        join p in dbContext.products
                        on invd.product_id equals p.product_id into product_with_type
                        from pwt in product_with_type.DefaultIfEmpty()
                        where invd.invoice_id == invoice_id
                        select new ProductWithType
                        {
                            product_id = pwt.product_id,
                            product_name = pwt.product_name,
                            product_price = pwt.product_price,
                            product_img = pwt.product_img,
                            product_quantity = invd.invd_product_quantity
                        }).ToList();
            }
        }
    }
}