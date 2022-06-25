using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KeyMax.Models
{
    public class InvoiceWithStatus
    {
        public int invoice_id { get; set; }
        public int user_id { get; set; }
        public string invoice_user_fullname { get; set; }
        public string invoice_user_phone_number { get; set; }
        public string invoice_user_email { get; set; }
        public string invoice_user_address { get; set; }
        public string invoice_note { get; set; }
        public string invoice_note_admin { get; set; }
        public int invoice_subtotal { get; set; }
        public int invoice_fee_transport { get; set; }
        public int invoice_status_id { get; set; }
        public string invoice_status_name { get; set; }
        public short invoice_prepaid { get; set; }
        public DateTime invoice_created_at { get; set; }
    }
}