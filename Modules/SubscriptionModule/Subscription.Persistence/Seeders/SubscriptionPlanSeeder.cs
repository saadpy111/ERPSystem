using Microsoft.EntityFrameworkCore;
using Subscription.Domain.Entities;
using Subscription.Domain.Enums;
using SharedKernel.Enums;
using Subscription.Persistence.Context;

namespace Subscription.Persistence.Seeders
{
    /// <summary>
    /// Seeds subscription plans based on existing ERP modules.
    /// Idempotent: safe to run multiple times.
    /// </summary>
    public class SubscriptionPlanSeeder
    {
        private readonly SubscriptionDbContext _context;

        public SubscriptionPlanSeeder(SubscriptionDbContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            // Seed Starter Plan
            await SeedStarterPlanAsync();

            // Seed Business Plan
            await SeedBusinessPlanAsync();

            // Seed Enterprise Plan
            await SeedEnterprisePlanAsync();

            await _context.SaveChangesAsync();
        }

        private async Task SeedStarterPlanAsync()
        {
            var planCode = "STARTER";
            var existingPlan = await _context.SubscriptionPlans
                .Include(p => p.PlanModules)
                .Include(p => p.Prices)
                .FirstOrDefaultAsync(p => p.Code == planCode);

            if (existingPlan == null)
            {
                var plan = new SubscriptionPlan
                {
                    Id = Guid.NewGuid().ToString(),
                    Code = planCode,
                    Name = "Starter",
                    DisplayName = "Starter Plan",
                    Description = "Perfect for small teams getting started",
                    IsTrial = true,
                    TrialDays = 14,
                    IsVisible = true,
                    SortOrder = 1,
                    IsActive = true,
                    MaxUsers = 5,
                    MaxStorageBytes = 1073741824, // 1 GB
                    MaxProducts = 100,
                    MaxMonthlyTransactions = 500,
                    MaxMonthlyApiCalls = 10000,
                    CreatedAt = DateTime.UtcNow
                };

                await _context.SubscriptionPlans.AddAsync(plan);

                // Add modules
                await _context.PlanModules.AddRangeAsync(
                    new PlanModule { PlanId = plan.Id, ModuleName = "HR", IsEnabled = true },
                    new PlanModule { PlanId = plan.Id, ModuleName = "Report", IsEnabled = true },
                    new PlanModule { PlanId = plan.Id, ModuleName = "Website", IsEnabled = true }

                );

                // Add pricing (Free tier)
                await _context.PlanPrices.AddRangeAsync(
                    new PlanPrice { PlanId = plan.Id, CurrencyCode = "USD", Amount = 0, Interval = BillingInterval.Monthly, IsActive = true, CreatedAt = DateTime.UtcNow },
                    new PlanPrice { PlanId = plan.Id, CurrencyCode = "EGP", Amount = 0, Interval = BillingInterval.Monthly, IsActive = true, CreatedAt = DateTime.UtcNow }
                );
            }
            else
            {
                // Update existing plan (idempotency)
                existingPlan.Name = "Starter";
                existingPlan.DisplayName = "Starter Plan";
                existingPlan.Description = "Perfect for small teams getting started";
                existingPlan.IsActive = true;
            }
        }

        private async Task SeedBusinessPlanAsync()
        {
            var planCode = "BUSINESS";
            var existingPlan = await _context.SubscriptionPlans
                .Include(p => p.PlanModules)
                .Include(p => p.Prices)
                .FirstOrDefaultAsync(p => p.Code == planCode);

            if (existingPlan == null)
            {
                var plan = new SubscriptionPlan
                {
                    Id = Guid.NewGuid().ToString(),
                    Code = planCode,
                    Name = "Business",
                    DisplayName = "Business Plan",
                    Description = "Full ERP suite for growing companies",
                    IsTrial = true,
                    TrialDays = 14,
                    IsVisible = true,
                    SortOrder = 2,
                    IsActive = true,
                    MaxUsers = 25,
                    MaxStorageBytes = 10737418240, // 10 GB
                    MaxProducts = 5000,
                    MaxMonthlyTransactions = 10000,
                    MaxMonthlyApiCalls = 100000,
                    CreatedAt = DateTime.UtcNow
                };

                await _context.SubscriptionPlans.AddAsync(plan);

                // Add all modules
                await _context.PlanModules.AddRangeAsync(
                    new PlanModule { PlanId = plan.Id, ModuleName = "HR", IsEnabled = true },
                    new PlanModule { PlanId = plan.Id, ModuleName = "Inventory", IsEnabled = true },
                    new PlanModule { PlanId = plan.Id, ModuleName = "Procurement", IsEnabled = true },
                    new PlanModule { PlanId = plan.Id, ModuleName = "Report", IsEnabled = true },
                    new PlanModule { PlanId = plan.Id, ModuleName = "Website", IsEnabled = true }
                );

                // Add pricing
                await _context.PlanPrices.AddRangeAsync(
                    new PlanPrice { PlanId = plan.Id, CurrencyCode = "USD", Amount = 99.00m, Interval = BillingInterval.Monthly, IsActive = true, CreatedAt = DateTime.UtcNow },
                    new PlanPrice { PlanId = plan.Id, CurrencyCode = "USD", Amount = 990.00m, Interval = BillingInterval.Yearly, IsActive = true, CreatedAt = DateTime.UtcNow },
                    new PlanPrice { PlanId = plan.Id, CurrencyCode = "EGP", Amount = 3000.00m, Interval = BillingInterval.Monthly, IsActive = true, CreatedAt = DateTime.UtcNow },
                    new PlanPrice { PlanId = plan.Id, CurrencyCode = "EGP", Amount = 30000.00m, Interval = BillingInterval.Yearly, IsActive = true, CreatedAt = DateTime.UtcNow }
                );
            }
            else
            {
                existingPlan.Name = "Business";
                existingPlan.DisplayName = "Business Plan";
                existingPlan.Description = "Full ERP suite for growing companies";
                existingPlan.IsActive = true;
            }
        }

        private async Task SeedEnterprisePlanAsync()
        {
            var planCode = "ENTERPRISE";
            var existingPlan = await _context.SubscriptionPlans
                .Include(p => p.PlanModules)
                .Include(p => p.Prices)
                .FirstOrDefaultAsync(p => p.Code == planCode);

            if (existingPlan == null)
            {
                var plan = new SubscriptionPlan
                {
                    Id = Guid.NewGuid().ToString(),
                    Code = planCode,
                    Name = "Enterprise",
                    DisplayName = "Enterprise Plan",
                    Description = "Unlimited power for large organizations",
                    IsTrial = false, // No trial
                    TrialDays = 0,
                    IsVisible = true,
                    SortOrder = 3,
                    IsActive = true,
                    MaxUsers = -1, // Unlimited
                    MaxStorageBytes = -1,
                    MaxProducts = -1,
                    MaxMonthlyTransactions = -1,
                    MaxMonthlyApiCalls = -1,
                    CreatedAt = DateTime.UtcNow
                };

                await _context.SubscriptionPlans.AddAsync(plan);

                // Add all modules
                await _context.PlanModules.AddRangeAsync(
                    new PlanModule { PlanId = plan.Id, ModuleName = "HR", IsEnabled = true },
                    new PlanModule { PlanId = plan.Id, ModuleName = "Inventory", IsEnabled = true },
                    new PlanModule { PlanId = plan.Id, ModuleName = "Procurement", IsEnabled = true },
                    new PlanModule { PlanId = plan.Id, ModuleName = "Report", IsEnabled = true },
                    new PlanModule { PlanId = plan.Id, ModuleName = "Website", IsEnabled = true }

                );

                // Add pricing
                await _context.PlanPrices.AddRangeAsync(
                    new PlanPrice { PlanId = plan.Id, CurrencyCode = "USD", Amount = 499.00m, Interval = BillingInterval.Monthly, IsActive = true, CreatedAt = DateTime.UtcNow },
                    new PlanPrice { PlanId = plan.Id, CurrencyCode = "USD", Amount = 4990.00m, Interval = BillingInterval.Yearly, IsActive = true, CreatedAt = DateTime.UtcNow },
                    new PlanPrice { PlanId = plan.Id, CurrencyCode = "EGP", Amount = 15000.00m, Interval = BillingInterval.Monthly, IsActive = true, CreatedAt = DateTime.UtcNow },
                    new PlanPrice { PlanId = plan.Id, CurrencyCode = "EGP", Amount = 150000.00m, Interval = BillingInterval.Yearly, IsActive = true, CreatedAt = DateTime.UtcNow }
                );
            }
            else
            {
                existingPlan.Name = "Enterprise";
                existingPlan.DisplayName = "Enterprise Plan";
                existingPlan.Description = "Unlimited power for large organizations";
                existingPlan.IsActive = true;
            }
        }
    }
}
