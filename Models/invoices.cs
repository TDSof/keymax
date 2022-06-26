namespace KeyMax.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class invoices
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public invoices()
        {
            invoice_details = new HashSet<invoice_details>();
        }

        [Key]
        public int invoice_id { get; set; }

        public int user_id { get; set; }

        [Required]
        [StringLength(50)]
        public string invoice_user_fullname { get; set; }

        [Required]
        [StringLength(30)]
        public string invoice_user_phone_number { get; set; }

        [Required]
        [StringLength(50)]
        public string invoice_user_email { get; set; }

        [Column(TypeName = "ntext")]
        [Required]
        public string invoice_user_address { get; set; }

        [Column(TypeName = "ntext")]
        public string invoice_note { get; set; }

        [Column(TypeName = "ntext")]
        public string invoice_note_admin { get; set; }

        public int invoice_subtotal { get; set; }

        public int invoice_fee_transport { get; set; }

        public int? invoice_status_id { get; set; }

        public short? invoice_prepaid { get; set; }

        [StringLength(20)]
        public string invoice_vnp_transaction_id { get; set; }

        public DateTime invoice_created_at { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<invoice_details> invoice_details { get; set; }

        public virtual invoice_status invoice_status { get; set; }

        public virtual users user { get; set; }
    }
}
