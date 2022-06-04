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
        [Column(Order = 0)]
        public int report_id { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(50)]
        public string report_fullname { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(50)]
        public string report_email { get; set; }

        [Key]
        [Column(Order = 3, TypeName = "ntext")]
        public string report_content { get; set; }
    }
}
