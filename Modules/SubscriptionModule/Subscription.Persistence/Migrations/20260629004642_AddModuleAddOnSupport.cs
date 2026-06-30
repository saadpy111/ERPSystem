using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Subscription.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddModuleAddOnSupport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ModuleId",
                schema: "Subscription",
                table: "PlanModules",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Modules",
                schema: "Subscription",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Modules", x => x.Id);
                });

            // Seed default modules
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT 1 FROM [Subscription].[Modules] WHERE [Code] = 'ACCOUNTING')
                    INSERT INTO [Subscription].[Modules] ([Id], [Code], [Name], [DisplayName], [Description], [IsActive], [CreatedAt])
                    VALUES (NEWID(), 'ACCOUNTING', 'Accounting', 'Accounting Module', 'Manage accounts, journal entries, vouchers, and budgets', 1, GETUTCDATE());
                IF NOT EXISTS (SELECT 1 FROM [Subscription].[Modules] WHERE [Code] = 'INVENTORY')
                    INSERT INTO [Subscription].[Modules] ([Id], [Code], [Name], [DisplayName], [Description], [IsActive], [CreatedAt])
                    VALUES (NEWID(), 'INVENTORY', 'Inventory', 'Inventory Module', 'Manage products, categories, warehouses, and stock movements', 1, GETUTCDATE());
                IF NOT EXISTS (SELECT 1 FROM [Subscription].[Modules] WHERE [Code] = 'HR')
                    INSERT INTO [Subscription].[Modules] ([Id], [Code], [Name], [DisplayName], [Description], [IsActive], [CreatedAt])
                    VALUES (NEWID(), 'HR', 'HR', 'HR Module', 'Manage employees, departments, payroll, and recruitment', 1, GETUTCDATE());
                IF NOT EXISTS (SELECT 1 FROM [Subscription].[Modules] WHERE [Code] = 'CRM')
                    INSERT INTO [Subscription].[Modules] ([Id], [Code], [Name], [DisplayName], [Description], [IsActive], [CreatedAt])
                    VALUES (NEWID(), 'CRM', 'CRM', 'CRM Module', 'Manage customer relationships and sales pipeline', 1, GETUTCDATE());
                IF NOT EXISTS (SELECT 1 FROM [Subscription].[Modules] WHERE [Code] = 'PROCUREMENT')
                    INSERT INTO [Subscription].[Modules] ([Id], [Code], [Name], [DisplayName], [Description], [IsActive], [CreatedAt])
                    VALUES (NEWID(), 'PROCUREMENT', 'Procurement', 'Procurement Module', 'Manage vendors, purchase orders, and invoices', 1, GETUTCDATE());
                IF NOT EXISTS (SELECT 1 FROM [Subscription].[Modules] WHERE [Code] = 'REPORT')
                    INSERT INTO [Subscription].[Modules] ([Id], [Code], [Name], [DisplayName], [Description], [IsActive], [CreatedAt])
                    VALUES (NEWID(), 'REPORT', 'Report', 'Report Module', 'Generate and manage reports across all modules', 1, GETUTCDATE());
                IF NOT EXISTS (SELECT 1 FROM [Subscription].[Modules] WHERE [Code] = 'WEBSITE')
                    INSERT INTO [Subscription].[Modules] ([Id], [Code], [Name], [DisplayName], [Description], [IsActive], [CreatedAt])
                    VALUES (NEWID(), 'WEBSITE', 'Website', 'Website Module', 'Manage website configuration, themes, and ecommerce', 1, GETUTCDATE());
            ");

            // Backfill ModuleId from ModuleName for existing PlanModule records
            migrationBuilder.Sql(@"
                UPDATE pm
                SET pm.[ModuleId] = m.[Id]
                FROM [Subscription].[PlanModules] pm
                INNER JOIN [Subscription].[Modules] m ON m.[Code] = pm.[ModuleName]
                WHERE pm.[ModuleId] IS NULL
            ");

            // Make ModuleId non-nullable
            migrationBuilder.AlterColumn<string>(
                name: "ModuleId",
                schema: "Subscription",
                table: "PlanModules",
                type: "nvarchar(450)",
                nullable: false,
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "ModulePrices",
                schema: "Subscription",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ModuleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CurrencyCode = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Interval = table.Column<int>(type: "int", nullable: false),
                    EffectiveFrom = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EffectiveTo = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModulePrices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ModulePrices_Modules_ModuleId",
                        column: x => x.ModuleId,
                        principalSchema: "Subscription",
                        principalTable: "Modules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TenantModuleSubscriptions",
                schema: "Subscription",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TenantId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ModuleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CurrencyCode = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    Interval = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AutoRenew = table.Column<bool>(type: "bit", nullable: false),
                    ExternalSubscriptionId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TenantModuleSubscriptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TenantModuleSubscriptions_Modules_ModuleId",
                        column: x => x.ModuleId,
                        principalSchema: "Subscription",
                        principalTable: "Modules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlanModules_ModuleId",
                schema: "Subscription",
                table: "PlanModules",
                column: "ModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_PlanModules_PlanId_ModuleId",
                schema: "Subscription",
                table: "PlanModules",
                columns: new[] { "PlanId", "ModuleId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ModulePrices_IsActive",
                schema: "Subscription",
                table: "ModulePrices",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_ModulePrices_ModuleId_CurrencyCode_Interval",
                schema: "Subscription",
                table: "ModulePrices",
                columns: new[] { "ModuleId", "CurrencyCode", "Interval" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Modules_Code",
                schema: "Subscription",
                table: "Modules",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Modules_IsActive",
                schema: "Subscription",
                table: "Modules",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_TenantModuleSubscriptions_ModuleId",
                schema: "Subscription",
                table: "TenantModuleSubscriptions",
                column: "ModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_TenantModuleSubscriptions_Status",
                schema: "Subscription",
                table: "TenantModuleSubscriptions",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_TenantModuleSubscriptions_TenantId",
                schema: "Subscription",
                table: "TenantModuleSubscriptions",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_TenantModuleSubscriptions_TenantId_Status",
                schema: "Subscription",
                table: "TenantModuleSubscriptions",
                columns: new[] { "TenantId", "Status" });

            migrationBuilder.AddForeignKey(
                name: "FK_PlanModules_Modules_ModuleId",
                schema: "Subscription",
                table: "PlanModules",
                column: "ModuleId",
                principalSchema: "Subscription",
                principalTable: "Modules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlanModules_Modules_ModuleId",
                schema: "Subscription",
                table: "PlanModules");

            migrationBuilder.DropTable(
                name: "ModulePrices",
                schema: "Subscription");

            migrationBuilder.DropTable(
                name: "TenantModuleSubscriptions",
                schema: "Subscription");

            migrationBuilder.DropTable(
                name: "Modules",
                schema: "Subscription");

            migrationBuilder.DropIndex(
                name: "IX_PlanModules_ModuleId",
                schema: "Subscription",
                table: "PlanModules");

            migrationBuilder.DropIndex(
                name: "IX_PlanModules_PlanId_ModuleId",
                schema: "Subscription",
                table: "PlanModules");

            migrationBuilder.DropColumn(
                name: "ModuleId",
                schema: "Subscription",
                table: "PlanModules");
        }
    }
}
