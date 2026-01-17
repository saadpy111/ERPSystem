using Microsoft.EntityFrameworkCore;
using Subscription.Domain.Entities;

namespace Subscription.Persistence.Context
{
    public class SubscriptionDbContext : DbContext
    {
        public SubscriptionDbContext(DbContextOptions<SubscriptionDbContext> options)
            : base(options)
        {
        }

        public DbSet<SubscriptionPlan> SubscriptionPlans => Set<SubscriptionPlan>();
        public DbSet<PlanPrice> PlanPrices => Set<PlanPrice>();
        public DbSet<PlanModule> PlanModules => Set<PlanModule>();
        public DbSet<TenantSubscription> TenantSubscriptions => Set<TenantSubscription>();
        public DbSet<UsageHistory> UsageHistory => Set<UsageHistory>();
        public DbSet<SubscriptionHistory> SubscriptionHistory => Set<SubscriptionHistory>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.HasDefaultSchema("Subscription");
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(SubscriptionDbContext).Assembly);
        }
    }
}
