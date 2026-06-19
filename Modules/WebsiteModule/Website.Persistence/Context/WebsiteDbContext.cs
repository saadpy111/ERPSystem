using Microsoft.EntityFrameworkCore;
using SharedKernel.Multitenancy;
using Website.Domain.Entities;
using System.Reflection;

namespace Website.Persistence.Context
{
    /// <summary>
    /// Database context for Website module with multi-tenant query filtering.
    /// Uses "Website" schema for all tables.
    /// </summary>
    public class WebsiteDbContext : DbContext
    {
        private readonly ITenantProvider? _tenantProvider;

        /// <summary>
        /// Current tenant ID for query filtering.
        /// </summary>
        public string? CurrentTenantId => _tenantProvider?.GetTenantId();

        public WebsiteDbContext(
            DbContextOptions<WebsiteDbContext> options,
            ITenantProvider? tenantProvider = null)
            : base(options)
        {
            _tenantProvider = tenantProvider;
        }

        // Existing entities
        public DbSet<Theme> Themes => Set<Theme>();
        public DbSet<TenantWebsite> TenantWebsites => Set<TenantWebsite>();

        // E-commerce entities
        public DbSet<WebsiteProduct> WebsiteProducts => Set<WebsiteProduct>();
        public DbSet<WebsiteCategory> WebsiteCategories => Set<WebsiteCategory>();
        public DbSet<ProductCollection> ProductCollections => Set<ProductCollection>();
        public DbSet<ProductCollectionItem> ProductCollectionItems => Set<ProductCollectionItem>();
        public DbSet<Offer> Offers => Set<Offer>();
        public DbSet<OfferProduct> OfferProducts => Set<OfferProduct>();
        public DbSet<OfferCategory> OfferCategories => Set<OfferCategory>();
        public DbSet<Cart> Carts => Set<Cart>();
        public DbSet<CartItem> CartItems => Set<CartItem>();
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderItem> OrderItems => Set<OrderItem>();
        public DbSet<Coupon> Coupons => Set<Coupon>();
        public DbSet<CouponUsage> CouponUsages => Set<CouponUsage>();
        public DbSet<WebsiteProductImage> WebsiteProductImages => Set<WebsiteProductImage>();
        public DbSet<CustomerProfile> CustomerProfiles => Set<CustomerProfile>();
        public DbSet<CustomerAnalytics> CustomerAnalytics => Set<CustomerAnalytics>();
        public DbSet<WebsiteVisitorSession> WebsiteVisitorSessions => Set<WebsiteVisitorSession>();
        public DbSet<WebsiteAnalyticsDaily> WebsiteAnalyticsDaily => Set<WebsiteAnalyticsDaily>();
        public DbSet<Brand> Brands => Set<Brand>();
        public DbSet<Testimonial> Testimonials => Set<Testimonial>();
        public DbSet<NewsletterSubscriber> NewsletterSubscribers => Set<NewsletterSubscriber>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Apply all configurations from this assembly
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            // Set default schema
            modelBuilder.HasDefaultSchema("Website");

            // Apply tenant filters and indexes
            ApplyGlobalQueryFilters(modelBuilder);
            ConfigureTenantIdIndexes(modelBuilder);
            ConfigureValueObjects(modelBuilder);
        }

        /// <summary>
        /// Applies global query filters for tenant isolation.
        /// </summary>
        private void ApplyGlobalQueryFilters(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TenantWebsite>()
                .HasQueryFilter(e => e.TenantId == CurrentTenantId);

            modelBuilder.Entity<WebsiteProduct>()
                .HasQueryFilter(e => e.TenantId == CurrentTenantId);

            modelBuilder.Entity<WebsiteCategory>()
                .HasQueryFilter(e => e.TenantId == CurrentTenantId);

            modelBuilder.Entity<ProductCollection>()
                .HasQueryFilter(e => e.TenantId == CurrentTenantId);

            modelBuilder.Entity<ProductCollectionItem>()
                .HasQueryFilter(e => e.TenantId == CurrentTenantId);

            modelBuilder.Entity<Offer>()
                .HasQueryFilter(e => e.TenantId == CurrentTenantId);

            modelBuilder.Entity<OfferProduct>()
                .HasQueryFilter(e => e.TenantId == CurrentTenantId);

            modelBuilder.Entity<OfferCategory>()
                .HasQueryFilter(e => e.TenantId == CurrentTenantId);

            modelBuilder.Entity<Cart>()
                .HasQueryFilter(e => e.TenantId == CurrentTenantId);

            modelBuilder.Entity<CartItem>()
                .HasQueryFilter(e => e.TenantId == CurrentTenantId);

            modelBuilder.Entity<Order>()
                .HasQueryFilter(e => e.TenantId == CurrentTenantId);

            modelBuilder.Entity<OrderItem>()
                .HasQueryFilter(e => e.TenantId == CurrentTenantId);

            modelBuilder.Entity<Coupon>()
                .HasQueryFilter(e => e.TenantId == CurrentTenantId);

            modelBuilder.Entity<CouponUsage>()
                .HasQueryFilter(e => e.TenantId == CurrentTenantId);

            modelBuilder.Entity<WebsiteProductImage>()
                .HasQueryFilter(e => e.TenantId == CurrentTenantId);

            modelBuilder.Entity<CustomerProfile>()
                .HasQueryFilter(e => e.TenantId == CurrentTenantId);

            modelBuilder.Entity<CustomerAnalytics>()
                .HasQueryFilter(e => e.TenantId == CurrentTenantId);

            modelBuilder.Entity<WebsiteVisitorSession>()
                .HasQueryFilter(e => e.TenantId == CurrentTenantId);

            modelBuilder.Entity<WebsiteAnalyticsDaily>()
                .HasQueryFilter(e => e.TenantId == CurrentTenantId);

            modelBuilder.Entity<Brand>()
                .HasQueryFilter(e => e.TenantId == CurrentTenantId);

            modelBuilder.Entity<Testimonial>()
                .HasQueryFilter(e => e.TenantId == CurrentTenantId);

            modelBuilder.Entity<NewsletterSubscriber>()
                .HasQueryFilter(e => e.TenantId == CurrentTenantId);
        }


        /// <summary>
        /// Configures indexes on TenantId for query performance.
        /// </summary>
        private void ConfigureTenantIdIndexes(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TenantWebsite>().HasIndex(e => e.TenantId);
            modelBuilder.Entity<WebsiteProduct>().HasIndex(e => e.TenantId);
            modelBuilder.Entity<WebsiteCategory>().HasIndex(e => e.TenantId);
            modelBuilder.Entity<ProductCollection>().HasIndex(e => e.TenantId);
            modelBuilder.Entity<ProductCollectionItem>().HasIndex(e => e.TenantId);
            modelBuilder.Entity<Offer>().HasIndex(e => e.TenantId);
            modelBuilder.Entity<OfferProduct>().HasIndex(e => e.TenantId);
            modelBuilder.Entity<OfferCategory>().HasIndex(e => e.TenantId);
            modelBuilder.Entity<Cart>().HasIndex(e => e.TenantId);
            modelBuilder.Entity<CartItem>().HasIndex(e => e.TenantId);
            modelBuilder.Entity<Order>().HasIndex(e => e.TenantId);
            modelBuilder.Entity<OrderItem>().HasIndex(e => e.TenantId);
            modelBuilder.Entity<Coupon>().HasIndex(e => e.TenantId);
            modelBuilder.Entity<CouponUsage>().HasIndex(e => e.TenantId);
            modelBuilder.Entity<WebsiteProductImage>().HasIndex(e => e.TenantId);
            modelBuilder.Entity<CustomerProfile>().HasIndex(e => e.TenantId);
            modelBuilder.Entity<CustomerAnalytics>().HasIndex(e => e.TenantId);
            modelBuilder.Entity<WebsiteVisitorSession>().HasIndex(e => e.TenantId);
            modelBuilder.Entity<WebsiteAnalyticsDaily>().HasIndex(e => e.TenantId);
            modelBuilder.Entity<Brand>().HasIndex(e => e.TenantId);
            modelBuilder.Entity<NewsletterSubscriber>().HasIndex(e => e.TenantId);
        }

        /// <summary>
        /// Configures value objects as owned entities.
        /// </summary>
        private void ConfigureValueObjects(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>().OwnsOne(o => o.ShippingAddress, sa =>
            {
                sa.Property(a => a.RecipientName).HasColumnName("ShippingAddress_RecipientName").HasMaxLength(150);
                sa.Property(a => a.Phone).HasColumnName("ShippingAddress_Phone").HasMaxLength(20);
                sa.Property(a => a.Street).HasColumnName("ShippingAddress_Street").HasMaxLength(200);
                sa.Property(a => a.City).HasColumnName("ShippingAddress_City").HasMaxLength(100);
                sa.Property(a => a.State).HasColumnName("ShippingAddress_State").HasMaxLength(100);
                sa.Property(a => a.Country).HasColumnName("ShippingAddress_Country").HasMaxLength(100);
                sa.Property(a => a.ZipCode).HasColumnName("ShippingAddress_ZipCode").HasMaxLength(20);
            });
        }
    }
}

