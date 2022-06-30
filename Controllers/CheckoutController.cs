using KeyMax.DataQuery;
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
        public void RemoveCart()
        {
            List<Cart> listCart = (List<Cart>)Session["listCart"];
            if (listCart != null)
            {
                listCart.Clear();
            }
        }
        public string Pay(invoices invoice)
        {
            OrderInfo order = new OrderInfo();
            order.OrderId = invoice.invoice_id;
            order.Amount = invoice.invoice_subtotal;
            order.OrderDesc = "Thanh toan truc tuyen KeyMax: " + invoice.invoice_id;
            VNPAY vnpay = new VNPAY();
            return vnpay.Pay(order);
        }

        // GET: Checkout
        public ActionResult Index()
        {
            if (Session["user_id"] == null)
            {
                return RedirectToAction("Login", "User");
            }
            List<Cart> listCart = GetCart();
            if (listCart.Count == 0) return RedirectToAction("Index", "Shop");
            int count = listCart.Count;
            if (count == 0) RedirectToAction("Index", "Shop");
            ViewData["listCart"] = listCart;
            ViewData["user"] = QD.GetUser((int)Session["user_id"]);
            return View();
        }
        [HttpPost]
        public ActionResult Index(invoices inv)
        {
            int id = QD.PostInvoice(inv, (int)Session["user_id"], GetCart(), out string err);
            if (id > 0)
            {
                RemoveCart();
                invoices invoice = QD.GetInvoice(id);
                if (invoice.invoice_prepaid == 1)
                {
                    return Redirect(Pay(invoice));
                }
                return RedirectToAction("Success", "Checkout", new { id });
            }

            ViewData["msg"] = err;
            List<Cart> listCart = GetCart();
            int count = listCart.Count;
            if (count == 0) RedirectToAction("Index", "Shop");
            ViewData["listCart"] = listCart;
            ViewData["user"] = QD.GetUser((int)Session["user_id"]);
            return View();
        }
        public ActionResult Success(int? id)
        {
            if (id == null) return HttpNotFound();
            List<invoice_details> listInvd = QD.GetInvoiceDetails((int)id);
            if (listInvd == null) return HttpNotFound();
            invoices invoice = QD.GetInvoice((int)id);
            string msg = "";
            string vnp_transaction_id = invoice.invoice_vnp_transaction_id;
            if (invoice.invoice_prepaid > 0 && string.IsNullOrEmpty(invoice.invoice_vnp_transaction_id) && Request.QueryString.Count > 0)
            {
                string vnp_HashSecret = Func.vnp_HashSecret; //Chuoi bi mat
                var vnpayData = Request.QueryString;
                VnPayLibrary vnpay = new VnPayLibrary();

                foreach (string s in vnpayData)
                {
                    //get all querystring data
                    if (!string.IsNullOrEmpty(s) && s.StartsWith("vnp_"))
                    {
                        vnpay.AddResponseData(s, vnpayData[s]);
                    }
                }
                //vnp_TxnRef: Ma don hang merchant gui VNPAY tai command=pay    
                //vnp_TransactionNo: Ma GD tai he thong VNPAY
                //vnp_ResponseCode:Response code from VNPAY: 00: Thanh cong, Khac 00: Xem tai lieu
                //vnp_SecureHash: HmacSHA512 cua du lieu tra ve

                long orderId = Convert.ToInt64(vnpay.GetResponseData("vnp_TxnRef"));
                long vnpayTranId = Convert.ToInt64(vnpay.GetResponseData("vnp_TransactionNo"));
                string vnp_ResponseCode = vnpay.GetResponseData("vnp_ResponseCode");
                string vnp_TransactionStatus = vnpay.GetResponseData("vnp_TransactionStatus");
                string vnp_SecureHash = Request.QueryString["vnp_SecureHash"];
                string TerminalID = Request.QueryString["vnp_TmnCode"];
                long vnp_Amount = Convert.ToInt64(vnpay.GetResponseData("vnp_Amount")) / 100;
                string bankCode = Request.QueryString["vnp_BankCode"];

                bool checkSignature = vnpay.ValidateSignature(vnp_SecureHash, vnp_HashSecret);
                if (checkSignature)
                {
                    if (vnp_ResponseCode == "00" && vnp_TransactionStatus == "00")
                    {
                        //Thanh toan thanh cong
                        //displayMsg.InnerText = "Giao dịch được thực hiện thành công. Cảm ơn quý khách đã sử dụng dịch vụ";
                        //log.InfoFormat("Thanh toan thanh cong, OrderId={0}, VNPAY TranId={1}", orderId, vnpayTranId);
                        invoice.invoice_vnp_transaction_id = vnpayTranId.ToString();
                        invoice.invoice_status_id = 2;
                        QD.UpdateInvoice(invoice);
                        vnp_transaction_id = vnpayTranId.ToString();
                        msg = "Giao dịch được thực hiện thành công.";
                    }
                    else
                    {
                        //Thanh toan khong thanh cong. Ma loi: vnp_ResponseCode
                        //displayMsg.InnerText = "Có lỗi xảy ra trong quá trình xử lý.Mã lỗi: " + vnp_ResponseCode;
                        //log.InfoFormat("Thanh toan loi, OrderId={0}, VNPAY TranId={1},ResponseCode={2}", orderId, vnpayTranId, vnp_ResponseCode);
                        msg = "Có lỗi xảy ra trong quá trình xử lý, đơn hàng được lưu lại. Mã lỗi: " + vnp_ResponseCode;
                    }
                    //displayTmnCode.InnerText = "Mã Website (Terminal ID):" + TerminalID;
                    //displayTxnRef.InnerText = "Mã giao dịch thanh toán:" + orderId.ToString();
                    //displayVnpayTranNo.InnerText = "Mã giao dịch tại VNPAY:" + vnpayTranId.ToString();
                    //displayAmount.InnerText = "Số tiền thanh toán (VND):" + vnp_Amount.ToString();
                    //displayBankCode.InnerText = "Ngân hàng thanh toán:" + bankCode;
                }
                else
                {
                    //log.InfoFormat("Invalid signature, InputData={0}", Request.RawUrl);
                    //displayMsg.InnerText = "Có lỗi xảy ra trong quá trình xử lý";
                    msg = "Có lỗi xảy ra trong quá trình xử lý.";
                }
            }
            else msg = "Đơn đặt hàng của bạn đang được tiến hành.";

            ViewData["vnp_transaction_id"] = vnp_transaction_id;
            ViewData["content"] = msg;
            ViewData["invoice"] = invoice;
            ViewData["listInvd"] = listInvd;
            return View();
        }
        public ActionResult Purchase(int? id)
        {
            if (id == null) return HttpNotFound();
            invoices invoice = QD.GetInvoice((int)id);
            if (invoice == null) return HttpNotFound();
            if (invoice.invoice_prepaid > 0 && !string.IsNullOrEmpty(invoice.invoice_vnp_transaction_id)) return RedirectToAction("Index", "Home");
            return Redirect(Pay(invoice));
        }
    }
}