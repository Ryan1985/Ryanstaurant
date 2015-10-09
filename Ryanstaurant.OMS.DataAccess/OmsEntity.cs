namespace Ryanstaurant.OMS.DataAccess
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class OmsEntity : DbContext
    {
        public OmsEntity()
            : base("name=Oms")
        {
        }

        public virtual DbSet<OMS_Derates> OMS_Derates { get; set; }
        public virtual DbSet<OMS_ItemDerates> OMS_ItemDerates { get; set; }
        public virtual DbSet<OMS_Items> OMS_Items { get; set; }
        public virtual DbSet<OMS_Menu> OMS_Menu { get; set; }
        public virtual DbSet<OMS_MenuDetail> OMS_MenuDetail { get; set; }
        public virtual DbSet<OMS_OrderDetail> OMS_OrderDetail { get; set; }
        public virtual DbSet<OMS_Orders> OMS_Orders { get; set; }
        public virtual DbSet<OMS_SpecialRequests> OMS_SpecialRequests { get; set; }
        public virtual DbSet<OMS_TableRelations> OMS_TableRelations { get; set; }
        public virtual DbSet<OMS_Tables> OMS_Tables { get; set; }
        public virtual DbSet<OMS_Token> OMS_Token { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OMS_Derates>()
                .Property(e => e.Amount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<OMS_Derates>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<OMS_Derates>()
                .Property(e => e.DerateLine)
                .HasPrecision(19, 4);

            modelBuilder.Entity<OMS_Items>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<OMS_Items>()
                .Property(e => e.Price)
                .HasPrecision(19, 4);

            modelBuilder.Entity<OMS_Items>()
                .Property(e => e.Cost)
                .HasPrecision(19, 4);

            modelBuilder.Entity<OMS_Items>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<OMS_Items>()
                .Property(e => e.ChildIdList)
                .IsUnicode(false);

            modelBuilder.Entity<OMS_Menu>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<OMS_Menu>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<OMS_Menu>()
                .Property(e => e.SubDescription)
                .IsUnicode(false);

            modelBuilder.Entity<OMS_OrderDetail>()
                .Property(e => e.OraginPrice)
                .HasPrecision(19, 4);

            modelBuilder.Entity<OMS_OrderDetail>()
                .Property(e => e.Price)
                .HasPrecision(19, 4);

            modelBuilder.Entity<OMS_Orders>()
                .Property(e => e.SubTotal)
                .HasPrecision(19, 4);

            modelBuilder.Entity<OMS_Orders>()
                .Property(e => e.Total)
                .HasPrecision(19, 4);

            modelBuilder.Entity<OMS_Orders>()
                .Property(e => e.PendingReason)
                .IsUnicode(false);

            modelBuilder.Entity<OMS_Orders>()
                .Property(e => e.RevokeReason)
                .IsUnicode(false);

            modelBuilder.Entity<OMS_Orders>()
                .Property(e => e.CancelReason)
                .IsUnicode(false);

            modelBuilder.Entity<OMS_SpecialRequests>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<OMS_SpecialRequests>()
                .Property(e => e.RequestGroup)
                .IsUnicode(false);

            modelBuilder.Entity<OMS_Tables>()
                .Property(e => e.DisplayNo)
                .IsUnicode(false);
        }
    }
}