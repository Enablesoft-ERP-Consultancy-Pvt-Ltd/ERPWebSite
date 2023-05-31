using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace IExpro.Core.Entity
{
    public partial class IExproContext : DbContext
    {
        public IExproContext()
            : base("name=IExproContext")
        {
        }

        public virtual DbSet<FormName> FormNames { get; set; }
        public virtual DbSet<INVOICE> INVOICEs { get; set; }
        public virtual DbSet<tblDocumentType> tblDocumentTypes { get; set; }
        public virtual DbSet<tblXSLTClientMapping> tblXSLTClientMappings { get; set; }
        public virtual DbSet<tblXSLTDetail> tblXSLTDetails { get; set; }
        public virtual DbSet<NewUserDetail> NewUserDetails { get; set; }
        public virtual DbSet<UserRight> UserRights { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FormName>()
                .Property(e => e.NavigateURL)
                .IsUnicode(false);

            modelBuilder.Entity<FormName>()
                .Property(e => e.DisplayName)
                .IsUnicode(false);

            modelBuilder.Entity<FormName>()
                .Property(e => e.ToolTip)
                .IsUnicode(false);

            modelBuilder.Entity<FormName>()
                .HasMany(e => e.FormName1)
                .WithOptional(e => e.FormName2)
                .HasForeignKey(e => e.ParentId);

            modelBuilder.Entity<INVOICE>()
                .Property(e => e.TorderNo)
                .IsUnicode(false);

            modelBuilder.Entity<INVOICE>()
                .Property(e => e.BankSubmissionRefNo)
                .IsUnicode(false);

            modelBuilder.Entity<INVOICE>()
                .Property(e => e.Dtstamp)
                .IsUnicode(false);

            modelBuilder.Entity<INVOICE>()
                .Property(e => e.Delvwk)
                .IsUnicode(false);

            modelBuilder.Entity<INVOICE>()
                .Property(e => e.descriptionofgoods)
                .IsUnicode(false);

            modelBuilder.Entity<INVOICE>()
                .Property(e => e.countryoforigin)
                .IsUnicode(false);

            modelBuilder.Entity<INVOICE>()
                .Property(e => e.countryoffinaldest)
                .IsUnicode(false);

            modelBuilder.Entity<INVOICE>()
                .Property(e => e.shipmentid)
                .IsUnicode(false);

            modelBuilder.Entity<INVOICE>()
                .Property(e => e.truckno)
                .IsUnicode(false);

            modelBuilder.Entity<INVOICE>()
                .Property(e => e.Destcode)
                .IsUnicode(false);

            modelBuilder.Entity<INVOICE>()
                .Property(e => e.LUTARNNO)
                .IsUnicode(false);

            modelBuilder.Entity<INVOICE>()
                .Property(e => e.ShipToAddress)
                .IsUnicode(false);

            modelBuilder.Entity<INVOICE>()
                .Property(e => e.DiscountRemark)
                .IsUnicode(false);

            modelBuilder.Entity<INVOICE>()
                .Property(e => e.GstinType)
                .IsUnicode(false);

            modelBuilder.Entity<INVOICE>()
                .Property(e => e.EndUse)
                .IsUnicode(false);

            modelBuilder.Entity<INVOICE>()
                .Property(e => e.RawMaterialPile)
                .IsUnicode(false);

            modelBuilder.Entity<INVOICE>()
                .Property(e => e.RawMaterialBase)
                .IsUnicode(false);

            modelBuilder.Entity<INVOICE>()
                .Property(e => e.CountryOfFinalDestination)
                .IsUnicode(false);

            modelBuilder.Entity<INVOICE>()
                .Property(e => e.PlaceOfDelivery)
                .IsUnicode(false);

            modelBuilder.Entity<INVOICE>()
                .Property(e => e.WareHouseName)
                .IsUnicode(false);

            modelBuilder.Entity<INVOICE>()
                .Property(e => e.SUQty)
                .IsUnicode(false);

            modelBuilder.Entity<INVOICE>()
                .Property(e => e.PreferentialAgreement)
                .IsUnicode(false);

            modelBuilder.Entity<INVOICE>()
                .Property(e => e.FlightNo)
                .IsUnicode(false);

            modelBuilder.Entity<INVOICE>()
                .Property(e => e.Composition)
                .IsUnicode(false);

            modelBuilder.Entity<tblDocumentType>()
                .Property(e => e.DocumentType)
                .IsUnicode(false);

            modelBuilder.Entity<tblXSLTDetail>()
                .Property(e => e.XSLTText)
                .IsUnicode(false);

            modelBuilder.Entity<tblXSLTDetail>()
                .Property(e => e.XSLTSubject)
                .IsUnicode(false);

            modelBuilder.Entity<tblXSLTDetail>()
                .HasMany(e => e.tblXSLTClientMappings)
                .WithRequired(e => e.tblXSLTDetail)
                .WillCascadeOnDelete(false);
        }
    }
}
