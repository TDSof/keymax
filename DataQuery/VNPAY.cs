using KeyMax.Models;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KeyMax.DataQuery
{
    public class VNPAY
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public string Pay(OrderInfo order)
        {
            //Get Config Info
            string vnp_Returnurl = "http://localhost:53593/Checkout/Success/" + order.OrderId; //URL nhan ket qua tra ve 
            string vnp_Url = "https://sandbox.vnpayment.vn/paymentv2/vpcpay.html"; //URL thanh toan cua VNPAY 
            string vnp_TmnCode = Func.vnp_TmnCode; //Ma website
            string vnp_HashSecret = Func.vnp_HashSecret; //Chuoi bi mat

            //Get payment input
            //OrderInfo order = new OrderInfo();
            //Save order to db
            //order.OrderId = DateTime.Now.Ticks; // Giả lập mã giao dịch hệ thống merchant gửi sang VNPAY
            //order.Amount = 100000; // Giả lập số tiền thanh toán hệ thống merchant gửi sang VNPAY 100,000 VND
            order.Status = "0"; //0: Trạng thái thanh toán "chờ thanh toán" hoặc "Pending"
            //order.OrderDesc = "Thanh toan hoa don truc tuyen";
            order.CreatedDate = DateTime.Now;

            //Build URL for VNPAY
            VnPayLibrary vnpay = new VnPayLibrary();
            vnpay.AddRequestData("vnp_Version", VnPayLibrary.VERSION);
            vnpay.AddRequestData("vnp_Command", "pay");
            vnpay.AddRequestData("vnp_TmnCode", vnp_TmnCode);
            vnpay.AddRequestData("vnp_Amount", (order.Amount * 100).ToString()); //Số tiền thanh toán. Số tiền không mang các ký tự phân tách thập phân, phần nghìn, ký tự tiền tệ. Để gửi số tiền thanh toán là 100,000 VND (một trăm nghìn VNĐ) thì merchant cần nhân thêm 100 lần (khử phần thập phân), sau đó gửi sang VNPAY là: 10000000
            //vnpay.AddRequestData("vnp_BankCode", cboBankCode.SelectedItem.Value);
            vnpay.AddRequestData("vnp_CreateDate", order.CreatedDate.ToString("yyyyMMddHHmmss"));
            vnpay.AddRequestData("vnp_CurrCode", "VND");
            vnpay.AddRequestData("vnp_IpAddr", Utils.GetIpAddress());
            vnpay.AddRequestData("vnp_Locale", "vn");

            vnpay.AddRequestData("vnp_OrderInfo", "Thanh toan don hang:" + order.OrderId);
            vnpay.AddRequestData("vnp_OrderType", "140004"); //default value: other
            vnpay.AddRequestData("vnp_ReturnUrl", vnp_Returnurl);
            vnpay.AddRequestData("vnp_TxnRef", order.OrderId.ToString()); // Mã tham chiếu của giao dịch tại hệ thống của merchant. Mã này là duy nhất dùng để phân biệt các đơn hàng gửi sang VNPAY. Không được trùng lặp trong ngày
            //Add Params of 2.1.0 Version
            vnpay.AddRequestData("vnp_ExpireDate", DateTime.Now.AddMinutes(15).ToString("yyyyMMddHHmmss"));

            //Billing
            //vnpay.AddRequestData("vnp_Bill_Mobile", txt_billing_mobile.Text.Trim());
            //vnpay.AddRequestData("vnp_Bill_Email", txt_billing_email.Text.Trim());
            //var fullName = txt_billing_fullname.Text.Trim();
            //if (!String.IsNullOrEmpty(fullName))
            //{
            //    var indexof = fullName.IndexOf(' ');
            //    vnpay.AddRequestData("vnp_Bill_FirstName", fullName.Substring(0, indexof));
            //    vnpay.AddRequestData("vnp_Bill_LastName", fullName.Substring(indexof + 1, fullName.Length - indexof - 1));
            //}
            //vnpay.AddRequestData("vnp_Bill_Address", txt_inv_addr1.Text.Trim());
            //vnpay.AddRequestData("vnp_Bill_City", txt_bill_city.Text.Trim());
            //vnpay.AddRequestData("vnp_Bill_Country", txt_bill_country.Text.Trim());
            //vnpay.AddRequestData("vnp_Bill_State", "");
            // Invoice
            //vnpay.AddRequestData("vnp_Inv_Phone", txt_inv_mobile.Text.Trim());
            //vnpay.AddRequestData("vnp_Inv_Email", txt_inv_email.Text.Trim());
            //vnpay.AddRequestData("vnp_Inv_Customer", txt_inv_customer.Text.Trim());
            //vnpay.AddRequestData("vnp_Inv_Address", txt_inv_addr1.Text.Trim());
            //vnpay.AddRequestData("vnp_Inv_Company", txt_inv_company.Text);
            //vnpay.AddRequestData("vnp_Inv_Taxcode", txt_inv_taxcode.Text);
            //vnpay.AddRequestData("vnp_Inv_Type", cbo_inv_type.SelectedItem.Value);

            string paymentUrl = vnpay.CreateRequestUrl(vnp_Url, vnp_HashSecret);
            log.InfoFormat("VNPAY URL: {0}", paymentUrl);
            return paymentUrl;
        }
    }
}