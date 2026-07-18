using Microsoft.EntityFrameworkCore;
using Subscription.Domain.Entities;
using Subscription.Domain.Enums;
using SharedKernel.Enums;
using Subscription.Persistence.Context;

namespace Subscription.Persistence.Seeders
{
    public class SubscriptionPlanSeeder
    {
        private readonly SubscriptionDbContext _context;

        public SubscriptionPlanSeeder(SubscriptionDbContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();

            var lockResultParam = new Microsoft.Data.SqlClient.SqlParameter
            {
                ParameterName = "@LockResult",
                SqlDbType = System.Data.SqlDbType.Int,
                Direction = System.Data.ParameterDirection.Output
            };

            await _context.Database.ExecuteSqlRawAsync(
                @"EXEC @LockResult = sp_getapplock
                    @Resource = 'SubscriptionPlanSeeder_Lock',
                    @LockMode = 'Exclusive',
                    @LockOwner = 'Transaction',
                    @LockTimeout = 30000;",
                lockResultParam);

            var lockResult = (int)lockResultParam.Value;
            if (lockResult < 0)
            {
                await transaction.RollbackAsync();
                throw new InvalidOperationException(
                    $"لم يتمكن SubscriptionPlanSeeder من الحصول على القفل خلال المدة المحددة (النتيجة: {lockResult}). على الأرجح هناك عملية seeding أخرى تعمل بالتوازي.");
            }

            await SeedNoModulesPlanAsync();

            await SeedTwoModulesPlanAsync();

            await SeedThreeModulesPlanAsync();

            await SeedFourModulesPlanAsync();

            await SeedFullModulesPlanAsync();

            await _context.SaveChangesAsync();

            await transaction.CommitAsync();
        }

        private async Task SeedNoModulesPlanAsync()
        {
            var planCode = "BASIC_NOMODULES";
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
                    Name = "الخطة الأساسية",
                    DisplayName = "الخطة الأساسية",
                    Description = "خطة تمهيدية مناسبة لمن يريد تجربة النظام الأساسي قبل تفعيل أي إمكانيات إضافية",
                    IsTrial = true,
                    TrialDays = 7,
                    IsVisible = true,
                    SortOrder = 1,
                    IsActive = true,
                    MaxUsers = 2,
                    MaxStorageBytes = 268435456,
                    MaxProducts = 20,
                    MaxMonthlyTransactions = 50,
                    MaxMonthlyApiCalls = 1000,
                    CreatedAt = DateTime.UtcNow
                };

                await _context.SubscriptionPlans.AddAsync(plan);

                await _context.PlanPrices.AddRangeAsync(
                    new PlanPrice { PlanId = plan.Id, CurrencyCode = "EGP", Amount = 100.00m, Interval = BillingInterval.Monthly, IsActive = true, CreatedAt = DateTime.UtcNow },
                    new PlanPrice { PlanId = plan.Id, CurrencyCode = "EGP", Amount = 1000.00m, Interval = BillingInterval.Yearly, IsActive = true, CreatedAt = DateTime.UtcNow }
                );
            }
            else
            {
                existingPlan.Name = "الخطة الأساسية";
                existingPlan.DisplayName = "الخطة الأساسية";
                existingPlan.Description = "خطة تمهيدية مناسبة لمن يريد تجربة النظام الأساسي قبل تفعيل أي إمكانيات إضافية";
                existingPlan.IsActive = true;
            }
        }

        private async Task SeedTwoModulesPlanAsync()
        {
            var planCode = "STARTER_2MOD";
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
                    Name = "خطة الانطلاق",
                    DisplayName = "خطة الانطلاق",
                    Description = "مناسبة للفرق الصغيرة التي تحتاج أساسيات الموارد البشرية والتقارير للبدء في تنظيم عملها",
                    IsTrial = true,
                    TrialDays = 14,
                    IsVisible = true,
                    SortOrder = 2,
                    IsActive = true,
                    MaxUsers = 5,
                    MaxStorageBytes = 1073741824,
                    MaxProducts = 100,
                    MaxMonthlyTransactions = 500,
                    MaxMonthlyApiCalls = 10000,
                    CreatedAt = DateTime.UtcNow
                };

                await _context.SubscriptionPlans.AddAsync(plan);

                await _context.PlanModules.AddRangeAsync(
                    new PlanModule { PlanId = plan.Id, ModuleId = await GetModuleIdAsync("HR"), ModuleName = "HR", IsEnabled = true },
                    new PlanModule { PlanId = plan.Id, ModuleId = await GetModuleIdAsync("REPORT"), ModuleName = "Report", IsEnabled = true }
                );

                await _context.PlanPrices.AddRangeAsync(
                    new PlanPrice { PlanId = plan.Id, CurrencyCode = "EGP", Amount = 250.00m, Interval = BillingInterval.Monthly, IsActive = true, CreatedAt = DateTime.UtcNow },
                    new PlanPrice { PlanId = plan.Id, CurrencyCode = "EGP", Amount = 2500.00m, Interval = BillingInterval.Yearly, IsActive = true, CreatedAt = DateTime.UtcNow }
                );
            }
            else
            {
                existingPlan.Name = "خطة الانطلاق";
                existingPlan.DisplayName = "خطة الانطلاق";
                existingPlan.Description = "مناسبة للفرق الصغيرة التي تحتاج أساسيات الموارد البشرية والتقارير للبدء في تنظيم عملها";
                existingPlan.IsActive = true;
            }
        }

        private async Task SeedThreeModulesPlanAsync()
        {
            var planCode = "GROWTH_3MOD";
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
                    Name = "خطة النمو",
                    DisplayName = "خطة النمو",
                    Description = "مثالية للشركات المتوسطة التي بدأت تحتاج إدارة المخزون والموقع الإلكتروني بجانب الموارد البشرية",
                    IsTrial = true,
                    TrialDays = 14,
                    IsVisible = true,
                    SortOrder = 3,
                    IsActive = true,
                    MaxUsers = 15,
                    MaxStorageBytes = 5368709120,
                    MaxProducts = 2000,
                    MaxMonthlyTransactions = 5000,
                    MaxMonthlyApiCalls = 50000,
                    CreatedAt = DateTime.UtcNow
                };

                await _context.SubscriptionPlans.AddAsync(plan);

                await _context.PlanModules.AddRangeAsync(
                    new PlanModule { PlanId = plan.Id, ModuleId = await GetModuleIdAsync("HR"), ModuleName = "HR", IsEnabled = true },
                    new PlanModule { PlanId = plan.Id, ModuleId = await GetModuleIdAsync("INVENTORY"), ModuleName = "Inventory", IsEnabled = true },
                    new PlanModule { PlanId = plan.Id, ModuleId = await GetModuleIdAsync("WEBSITE"), ModuleName = "Website", IsEnabled = true }
                );

                await _context.PlanPrices.AddRangeAsync(
                    new PlanPrice { PlanId = plan.Id, CurrencyCode = "EGP", Amount = 500.00m, Interval = BillingInterval.Monthly, IsActive = true, CreatedAt = DateTime.UtcNow },
                    new PlanPrice { PlanId = plan.Id, CurrencyCode = "EGP", Amount = 5000.00m, Interval = BillingInterval.Yearly, IsActive = true, CreatedAt = DateTime.UtcNow }
                );
            }
            else
            {
                existingPlan.Name = "خطة النمو";
                existingPlan.DisplayName = "خطة النمو";
                existingPlan.Description = "مثالية للشركات المتوسطة التي بدأت تحتاج إدارة المخزون والموقع الإلكتروني بجانب الموارد البشرية";
                existingPlan.IsActive = true;
            }
        }

        private async Task SeedFourModulesPlanAsync()
        {
            var planCode = "BUSINESS_4MOD";
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
                    Name = "خطة الأعمال",
                    DisplayName = "خطة الأعمال",
                    Description = "تغطي جميع احتياجات الشركات الكبيرة نسبيًا من موارد بشرية ومخزون ومشتريات وتقارير متقدمة",
                    IsTrial = true,
                    TrialDays = 14,
                    IsVisible = true,
                    SortOrder = 4,
                    IsActive = true,
                    MaxUsers = 25,
                    MaxStorageBytes = 10737418240,
                    MaxProducts = 5000,
                    MaxMonthlyTransactions = 10000,
                    MaxMonthlyApiCalls = 100000,
                    CreatedAt = DateTime.UtcNow
                };

                await _context.SubscriptionPlans.AddAsync(plan);

                await _context.PlanModules.AddRangeAsync(
                    new PlanModule { PlanId = plan.Id, ModuleId = await GetModuleIdAsync("HR"), ModuleName = "HR", IsEnabled = true },
                    new PlanModule { PlanId = plan.Id, ModuleId = await GetModuleIdAsync("INVENTORY"), ModuleName = "Inventory", IsEnabled = true },
                    new PlanModule { PlanId = plan.Id, ModuleId = await GetModuleIdAsync("PROCUREMENT"), ModuleName = "Procurement", IsEnabled = true },
                    new PlanModule { PlanId = plan.Id, ModuleId = await GetModuleIdAsync("REPORT"), ModuleName = "Report", IsEnabled = true }
                );

                await _context.PlanPrices.AddRangeAsync(
                    new PlanPrice { PlanId = plan.Id, CurrencyCode = "EGP", Amount = 900.00m, Interval = BillingInterval.Monthly, IsActive = true, CreatedAt = DateTime.UtcNow },
                    new PlanPrice { PlanId = plan.Id, CurrencyCode = "EGP", Amount = 9000.00m, Interval = BillingInterval.Yearly, IsActive = true, CreatedAt = DateTime.UtcNow }
                );
            }
            else
            {
                existingPlan.Name = "خطة الأعمال";
                existingPlan.DisplayName = "خطة الأعمال";
                existingPlan.Description = "تغطي جميع احتياجات الشركات الكبيرة نسبيًا من موارد بشرية ومخزون ومشتريات وتقارير متقدمة";
                existingPlan.IsActive = true;
            }
        }

        private async Task SeedFullModulesPlanAsync()
        {
            var planCode = "ENTERPRISE_FULL";
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
                    Name = "الخطة المؤسسية",
                    DisplayName = "الخطة المؤسسية",
                    Description = "الخطة الأشمل والأقوى، تفتح جميع إمكانيات النظام بدون أي قيود، مناسبة للمؤسسات الكبرى",
                    IsTrial = false,
                    TrialDays = 0,
                    IsVisible = true,
                    SortOrder = 5,
                    IsActive = true,
                    MaxUsers = -1,
                    MaxStorageBytes = -1,
                    MaxProducts = -1,
                    MaxMonthlyTransactions = -1,
                    MaxMonthlyApiCalls = -1,
                    CreatedAt = DateTime.UtcNow
                };

                await _context.SubscriptionPlans.AddAsync(plan);

                await _context.PlanModules.AddRangeAsync(
                    new PlanModule { PlanId = plan.Id, ModuleId = await GetModuleIdAsync("HR"), ModuleName = "HR", IsEnabled = true },
                    new PlanModule { PlanId = plan.Id, ModuleId = await GetModuleIdAsync("INVENTORY"), ModuleName = "Inventory", IsEnabled = true },
                    new PlanModule { PlanId = plan.Id, ModuleId = await GetModuleIdAsync("PROCUREMENT"), ModuleName = "Procurement", IsEnabled = true },
                    new PlanModule { PlanId = plan.Id, ModuleId = await GetModuleIdAsync("REPORT"), ModuleName = "Report", IsEnabled = true },
                    new PlanModule { PlanId = plan.Id, ModuleId = await GetModuleIdAsync("WEBSITE"), ModuleName = "Website", IsEnabled = true }
                );

                await _context.PlanPrices.AddRangeAsync(
                    new PlanPrice { PlanId = plan.Id, CurrencyCode = "EGP", Amount = 1500.00m, Interval = BillingInterval.Monthly, IsActive = true, CreatedAt = DateTime.UtcNow },
                    new PlanPrice { PlanId = plan.Id, CurrencyCode = "EGP", Amount = 15000.00m, Interval = BillingInterval.Yearly, IsActive = true, CreatedAt = DateTime.UtcNow }
                );
            }
            else
            {
                existingPlan.Name = "الخطة المؤسسية";
                existingPlan.DisplayName = "الخطة المؤسسية";
                existingPlan.Description = "الخطة الأشمل والأقوى، تفتح جميع إمكانيات النظام بدون أي قيود، مناسبة للمؤسسات الكبرى";
                existingPlan.IsActive = true;
            }
        }

        private async Task<string> GetModuleIdAsync(string code)
        {
            var module = await _context.Modules.FirstOrDefaultAsync(m => m.Code == code);
            return module?.Id ?? string.Empty;
        }
    }
}