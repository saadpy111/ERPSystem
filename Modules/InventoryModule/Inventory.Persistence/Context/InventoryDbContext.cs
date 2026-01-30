using Inventory.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Multitenancy;
using System;
using System.Reflection;

namespace Inventory.Persistence.Context
{
    /// <summary>
    /// DbContext for the Inventory module with multi-tenant query filtering.
    /// All queries are automatically filtered by CurrentTenantId when set.
    /// Use IgnoreQueryFilters() for admin/system scenarios.
    /// </summary>
    public class InventoryDbContext : DbContext
    {
        private readonly ITenantProvider? _tenantProvider;

        /// <summary>
        /// Current tenant ID resolved from the request context.
        /// Used for global query filters.
        /// </summary>
        public string? CurrentTenantId => _tenantProvider?.GetTenantId();

        public InventoryDbContext(
            DbContextOptions<InventoryDbContext> options,
            ITenantProvider? tenantProvider = null) : base(options)
        {
            _tenantProvider = tenantProvider;
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<StockQuant> StockQuants { get; set; }
        public DbSet<StockMove> StockMoves { get; set; }
        public DbSet<StockAdjustment> StockAdjustments { get; set; }

        public DbSet<SerialOrBatchNumber> SerialOrBatchNumbers { get; set; }
        public DbSet<ProductCostHistory> ProductCostHistories { get; set; }
        public DbSet<ProductBarcode> ProductBarcodes { get; set; }
        public DbSet<InventoryQuarantine> InventoryQuarantines { get; set; }

        public DbSet<ProductAttribute> ProductAttributes { get; set; }
        public DbSet<ProductImage> productImages { get; set; }
        public DbSet<ProductAttributeValue> ProductAttributeValues { get; set; }
        public DbSet<Attachment> Attachments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            modelBuilder.HasDefaultSchema("Inventory");

            ApplyGlobalQueryFilters(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }

        /// <summary>
        /// Applies global query filters to all tenant-scoped entities.
        /// Filters only apply when CurrentTenantId is set.
        /// Use IgnoreQueryFilters() to bypass for admin/system scenarios.
        /// </summary>
        private void ApplyGlobalQueryFilters(ModelBuilder modelBuilder)
        {
            if (!string.IsNullOrEmpty(CurrentTenantId))
            {
                // Core entities
                modelBuilder.Entity<Product>()
                    .HasQueryFilter(e => e.TenantId == CurrentTenantId);
                modelBuilder.Entity<ProductCategory>()
                    .HasQueryFilter(e => e.TenantId == CurrentTenantId);
                modelBuilder.Entity<Warehouse>()
                    .HasQueryFilter(e => e.TenantId == CurrentTenantId);
                modelBuilder.Entity<Location>()
                    .HasQueryFilter(e => e.TenantId == CurrentTenantId);

                // Stock entities
                modelBuilder.Entity<StockQuant>()
                    .HasQueryFilter(e => e.TenantId == CurrentTenantId);
                modelBuilder.Entity<StockMove>()
                    .HasQueryFilter(e => e.TenantId == CurrentTenantId);
                modelBuilder.Entity<StockAdjustment>()
                    .HasQueryFilter(e => e.TenantId == CurrentTenantId);

                // Product-related entities
                modelBuilder.Entity<SerialOrBatchNumber>()
                    .HasQueryFilter(e => e.TenantId == CurrentTenantId);
                modelBuilder.Entity<ProductCostHistory>()
                    .HasQueryFilter(e => e.TenantId == CurrentTenantId);
                modelBuilder.Entity<ProductBarcode>()
                    .HasQueryFilter(e => e.TenantId == CurrentTenantId);
                modelBuilder.Entity<InventoryQuarantine>()
                    .HasQueryFilter(e => e.TenantId == CurrentTenantId);

                // Attribute entities
                modelBuilder.Entity<ProductAttribute>()
                    .HasQueryFilter(e => e.TenantId == CurrentTenantId);
                modelBuilder.Entity<ProductImage>()
                    .HasQueryFilter(e => e.TenantId == CurrentTenantId);
                modelBuilder.Entity<ProductAttributeValue>()
                    .HasQueryFilter(e => e.TenantId == CurrentTenantId);

                // Attachments
                modelBuilder.Entity<Attachment>()
                    .HasQueryFilter(e => e.TenantId == CurrentTenantId);
            }
        }

        /// <summary>
        /// Configures indexes on TenantId columns for query performance.
        /// </summary>

        public override Task<int> SaveChangesAsync(
                      CancellationToken cancellationToken = default)
        {
            EnforceTenantOnInsert();
            return base.SaveChangesAsync(cancellationToken);
        }
        private void EnforceTenantOnInsert()
        {
            var tenantId = CurrentTenantId;
            if (string.IsNullOrEmpty(tenantId))
                throw new UnauthorizedAccessException("TenantId not resolved");

            var entries = ChangeTracker
                .Entries<ITenantEntity>()
                .Where(e => e.State == EntityState.Added);

            foreach (var entry in entries)
            {
                entry.Entity.TenantId = tenantId;
            }
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
