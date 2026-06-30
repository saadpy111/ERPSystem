using Microsoft.EntityFrameworkCore;
using SharedKernel.Enums;
using Subscription.Domain.Entities;
using Subscription.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Subscription.Persistence.Seeders
{
    public class ModuleSeeder
    {
        private readonly SubscriptionDbContext _context;

        public ModuleSeeder(SubscriptionDbContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            var modules = GetDefaultModules();
            var existingCodes = await _context.Modules.Select(m => m.Code).ToListAsync();

            foreach (var module in modules)
            {
                if (!existingCodes.Contains(module.Code))
                {
                    await _context.Modules.AddAsync(module);
                }
            }

            await _context.SaveChangesAsync();

            await SeedModulePricesAsync();
        }

        private static List<Module> GetDefaultModules()
        {
            var now = DateTime.UtcNow;
            return new List<Module>
            {
                new Module
                {
                    Id = Guid.NewGuid().ToString(),
                    Code = "ACCOUNTING",
                    Name = "Accounting",
                    DisplayName = "Accounting Module",
                    Description = "Manage accounts, journal entries, vouchers, and budgets",
                    IsActive = true,
                    CreatedAt = now
                },
                new Module
                {
                    Id = Guid.NewGuid().ToString(),
                    Code = "INVENTORY",
                    Name = "Inventory",
                    DisplayName = "Inventory Module",
                    Description = "Manage products, categories, warehouses, and stock movements",
                    IsActive = true,
                    CreatedAt = now
                },
                new Module
                {
                    Id = Guid.NewGuid().ToString(),
                    Code = "HR",
                    Name = "HR",
                    DisplayName = "HR Module",
                    Description = "Manage employees, departments, payroll, and recruitment",
                    IsActive = true,
                    CreatedAt = now
                },
                new Module
                {
                    Id = Guid.NewGuid().ToString(),
                    Code = "CRM",
                    Name = "CRM",
                    DisplayName = "CRM Module",
                    Description = "Manage customer relationships and sales pipeline",
                    IsActive = true,
                    CreatedAt = now
                },
                new Module
                {
                    Id = Guid.NewGuid().ToString(),
                    Code = "PROCUREMENT",
                    Name = "Procurement",
                    DisplayName = "Procurement Module",
                    Description = "Manage vendors, purchase orders, and invoices",
                    IsActive = true,
                    CreatedAt = now
                },
                new Module
                {
                    Id = Guid.NewGuid().ToString(),
                    Code = "REPORT",
                    Name = "Report",
                    DisplayName = "Report Module",
                    Description = "Generate and manage reports across all modules",
                    IsActive = true,
                    CreatedAt = now
                },
                new Module
                {
                    Id = Guid.NewGuid().ToString(),
                    Code = "WEBSITE",
                    Name = "Website",
                    DisplayName = "Website Module",
                    Description = "Manage website configuration, themes, and ecommerce",
                    IsActive = true,
                    CreatedAt = now
                }
            };
        }

        private async Task SeedModulePricesAsync()
        {
            var modules = await _context.Modules.ToListAsync();
            var now = DateTime.UtcNow;

            foreach (var module in modules)
            {
                var existingPrices = await _context.ModulePrices
                    .Where(mp => mp.ModuleId == module.Id)
                    .ToListAsync();

                if (!existingPrices.Any())
                {
                    var prices = new List<ModulePrice>
                    {
                        new ModulePrice
                        {
                            ModuleId = module.Id,
                            CurrencyCode = "USD",
                            UnitPrice = 29.00m,
                            Interval = BillingInterval.Monthly,
                            EffectiveFrom = now,
                            IsActive = true,
                            CreatedAt = now
                        },
                        new ModulePrice
                        {
                            ModuleId = module.Id,
                            CurrencyCode = "USD",
                            UnitPrice = 290.00m,
                            Interval = BillingInterval.Yearly,
                            EffectiveFrom = now,
                            IsActive = true,
                            CreatedAt = now
                        },
                        new ModulePrice
                        {
                            ModuleId = module.Id,
                            CurrencyCode = "EGP",
                            UnitPrice = 890.00m,
                            Interval = BillingInterval.Monthly,
                            EffectiveFrom = now,
                            IsActive = true,
                            CreatedAt = now
                        },
                        new ModulePrice
                        {
                            ModuleId = module.Id,
                            CurrencyCode = "EGP",
                            UnitPrice = 8900.00m,
                            Interval = BillingInterval.Yearly,
                            EffectiveFrom = now,
                            IsActive = true,
                            CreatedAt = now
                        }
                    };

                    await _context.ModulePrices.AddRangeAsync(prices);
                }
            }

            await _context.SaveChangesAsync();
        }
    }
}
