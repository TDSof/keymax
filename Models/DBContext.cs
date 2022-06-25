using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace KeyMax.Models
{
    public partial class DBContext : DbContext
    {
        public DBContext()
            : base("name=DBContext")
        {
        }

        public virtual DbSet<invoice_details> invoice_details { get; set; }
        public virtual DbSet<invoice_status> invoice_status { get; set; }
        public virtual DbSet<invoices> invoices { get; set; }
        public virtual DbSet<product_types> product_types { get; set; }
        public virtual DbSet<products> products { get; set; }
        public virtual DbSet<reports> reports { get; set; }
        public virtual DbSet<users> users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<invoices>()
                .HasMany(e => e.invoice_details)
                .WithRequired(e => e.invoice)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<products>()
                .HasMany(e => e.invoice_details)
                .WithRequired(e => e.product)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<users>()
                .Property(e => e.user_address)
                .IsUnicode(false);

            modelBuilder.Entity<users>()
                .HasMany(e => e.invoices)
                .WithRequired(e => e.user)
                .WillCascadeOnDelete(false);
        }
    }
}
