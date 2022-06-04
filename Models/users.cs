namespace KeyMax.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class users
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public users()
        {
            invoices = new HashSet<invoices>();
            carts = new HashSet<carts>();
        }

        [Key]
        public int user_id { get; set; }

        [Required]
        [StringLength(50)]
        public string user_fullname { get; set; }

        [Required]
        [StringLength(50)]
        public string user_email { get; set; }

        [Required]
        [StringLength(50)]
        public string user_password { get; set; }

        [StringLength(15)]
        public string user_phone_number { get; set; }

        [Column(TypeName = "text")]
        public string user_address { get; set; }

        public DateTime? user_created_at { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<invoices> invoices { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<carts> carts { get; set; }
    }
}
