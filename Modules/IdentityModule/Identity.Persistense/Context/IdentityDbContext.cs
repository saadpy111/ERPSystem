
using Identity.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace Identity.Persistense.Context
{
    public class IdentityDbContext : IdentityDbContext<ApplicationUser , ApplicationRole , string>
    {
        public IdentityDbContext(DbContextOptions<IdentityDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("Identity");
            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                string con = "Server=DESKTOP-VGEBCK1\\SQLEXPRESS;Database=InventoryMicro;Trusted_Connection=True;MultipleActiveResultSets=true;Encrypt=False;TrustServerCertificate=True;";
                optionsBuilder.UseSqlServer(con);
            }
            base.OnConfiguring(optionsBuilder);
        }
    }
}

