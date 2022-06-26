namespace KeyMax.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class reports
    {
        [Key]
        public int report_id { get; set; }

        [Required]
        [StringLength(50)]
        public string report_fullname { get; set; }

        [Required]
        [StringLength(50)]
        public string report_email { get; set; }

        [Column(TypeName = "ntext")]
        [Required]
        public string report_content { get; set; }

        public DateTime report_created_at { get; set; }
    }
}
