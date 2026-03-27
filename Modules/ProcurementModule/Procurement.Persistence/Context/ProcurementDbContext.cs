using MediatR;
using Microsoft.EntityFrameworkCore;
using Procurement.Domain;
using Procurement.Domain.Entities;
using SharedKernel.Multitenancy;
using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Procurement.Persistence.Context
{
    public class ProcurementDbContext : DbContext
    {
        private readonly IMediator _mediator;
        private readonly ITenantProvider _tenantProvider;

        private string TenantId => _tenantProvider.GetTenantId()!;

        public ProcurementDbContext(
            DbContextOptions<ProcurementDbContext> options,
            IMediator mediator,
            ITenantProvider tenantProvider) : base(options)
        {
            _mediator = mediator;
            _tenantProvider = tenantProvider;
        }

        #region DbSets

        public DbSet<Vendor> Vendors { get; set; }
        public DbSet<PurchaseOrder> PurchaseOrders { get; set; }
        public DbSet<PurchaseOrderItem> PurchaseOrderItems { get; set; }
        public DbSet<GoodsReceipt> GoodsReceipts { get; set; }
        public DbSet<GoodsReceiptItem> GoodsReceiptItems { get; set; }
        public DbSet<PurchaseInvoice> PurchaseInvoices { get; set; }
        public DbSet<PurchaseRequisition> PurchaseRequisitions { get; set; }
        public DbSet<ProcurementAttachment> ProcurementAttachments { get; set; }

        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("Procurement");
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder);

            ApplyGlobalTenantFilter(modelBuilder);
        }

        #region Global Filter

        private void ApplyGlobalTenantFilter(ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
                {
                    var method = typeof(ProcurementDbContext)
                        .GetMethod(nameof(SetTenantFilter), BindingFlags.NonPublic | BindingFlags.Instance)!
                        .MakeGenericMethod(entityType.ClrType);

                    method.Invoke(this, new object[] { modelBuilder });
                }
            }
        }

        private void SetTenantFilter<TEntity>(ModelBuilder modelBuilder)
            where TEntity : BaseEntity
        {
            modelBuilder.Entity<TEntity>()
                .HasQueryFilter(e => e.TenantId == TenantId);
        }

        #endregion

        #region SaveChanges

        public override int SaveChanges()
        {
            ApplyTenantAndAudit();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ApplyTenantAndAudit();
            return await base.SaveChangesAsync(cancellationToken);
        }

        private void ApplyTenantAndAudit()
        {
            var tenantId = _tenantProvider.GetTenantId();

            if (string.IsNullOrEmpty(tenantId))
                throw new Exception("TenantId is not set!");

            foreach (var entry in ChangeTracker.Entries<BaseEntity>())
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.TenantId = tenantId;
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                }
                else if (entry.State == EntityState.Modified)
                {
                    if (entry.OriginalValues["TenantId"]?.ToString() != tenantId)
                        throw new Exception("Cross-tenant update is not allowed!");

                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                }
                else if (entry.State == EntityState.Deleted)
                {
                    if (entry.OriginalValues["TenantId"]?.ToString() != tenantId)
                        throw new Exception("Cross-tenant delete is not allowed!");
                }
            }
        }

        #endregion

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