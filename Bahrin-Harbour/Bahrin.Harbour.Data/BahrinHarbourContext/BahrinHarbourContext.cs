using Bahrin.Harbour.Data.DBCollections;
using Bahrin.Harbour.Model.ClientModel;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Bahrin.Harbour.Data.DataContext
{
    public class BahrinHarbourContext : IdentityDbContext<ApplicationUser>
    {
        public BahrinHarbourContext(DbContextOptions<BahrinHarbourContext> options) : base(options) { }

        public DbSet<Client> Clients { get; set; }
        public DbSet<Outlet> Outlets { get; set; }
        public DbSet<Property> Properties { get; set; }
        public DbSet<VisitHistory> VisitHistory { get; set; }
        public DbSet<LoyaltyCard> LoyaltyCards { get; set; }
        public DbSet<UserOutletRelation> UserOutletRelations { get; set; }
        public DbSet<Email> Email { get; set; }
        public DbSet<City> Cities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            /* modelBuilder.Entity<Property>()
                 .Property(c => c.)
                 .HasColumnType("decimal(18,2)");

             modelBuilder.Entity<Client>()
                 .Property(c => c.AvailedDiscount)
                 .HasColumnType("decimal(18,2)");*/

            modelBuilder.Entity<UserOutletRelation>()
         .HasKey(uo => new { uo.UserId, uo.OutletId }); 

            modelBuilder.Entity<UserOutletRelation>()
                .HasOne(uo => uo.User)
                .WithMany(u => u.UserOutlets)
                .HasForeignKey(uo => uo.UserId)
                .HasPrincipalKey(u => u.UserGuid); 

            modelBuilder.Entity<UserOutletRelation>()
                .HasOne(uo => uo.Outlet)
                .WithMany(o => o.UserOutlets)
                .HasForeignKey(uo => uo.OutletId);

            modelBuilder.Entity<Outlet>()
                .Property(o => o.DiscountPercentage)
                .HasColumnType("decimal(5, 2)");
        }
    }
}
/*
 * 
 * Migration Run Commands
 * 
 *  Add-Migration <<MigrationName>> -Project Bahrin.Harbour.Data -StartupProject AdminBahrin.Harbour
    Update-Database -Project Bahrin.Harbour.Data -StartupProject AdminBahrin.Harbour

    Add-Migration sdsas -Project Bahrin.Harbour.Data -StartupProject AdminBahrin.Harbour
    Update-Database -Project Bahrin.Harbour.Data -StartupProject AdminBahrin.Harbour

    Remove-Migration -Project Bahrin.Harbour.Data -StartupProject AdminBahrin.Harbour
    Update-Database -Project Bahrin.Harbour.Data -StartupProject AdminBahrin.Harbour
 */ 