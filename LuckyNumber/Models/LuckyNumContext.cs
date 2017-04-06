namespace LuckyNumber.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class LuckyNumContext : DbContext
    {
        public LuckyNumContext()
            : base("name=LuckyNumContext")
        {
        }

        public virtual DbSet<Admin> Admins { get; set; }
        public virtual DbSet<ChiTietCuocChoi> ChiTietCuocChois { get; set; }
        public virtual DbSet<ChiTietTrungThuong> ChiTietTrungThuongs { get; set; }
        public virtual DbSet<CuocChoi> CuocChois { get; set; }
        public virtual DbSet<DanhSachTrungThuong> DanhSachTrungThuongs { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CuocChoi>()
                .HasMany(e => e.ChiTietCuocChois)
                .WithRequired(e => e.CuocChoi)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CuocChoi>()
                .HasMany(e => e.DanhSachTrungThuongs)
                .WithRequired(e => e.CuocChoi)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<DanhSachTrungThuong>()
                .HasMany(e => e.ChiTietTrungThuongs)
                .WithRequired(e => e.DanhSachTrungThuong)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.ChiTietCuocChois)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.ChiTietTrungThuongs)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);
        }
    }
}
