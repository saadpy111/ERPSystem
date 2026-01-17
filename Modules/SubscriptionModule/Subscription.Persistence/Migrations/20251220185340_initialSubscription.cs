using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Subscription.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class initialSubscription : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Subscription");

            migrationBuilder.CreateTable(
                name: "SubscriptionPlans",
                schema: "Subscription",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IsTrial = table.Column<bool>(type: "bit", nullable: false),
                    TrialDays = table.Column<int>(type: "int", nullable: false),
                    IsVisible = table.Column<bool>(type: "bit", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    MaxUsers = table.Column<int>(type: "int", nullable: false),
                    MaxStorageBytes = table.Column<long>(type: "bigint", nullable: false),
                    MaxProducts = table.Column<int>(type: "int", nullable: false),
                    MaxMonthlyTransactions = table.Column<int>(type: "int", nullable: false),
                    MaxMonthlyApiCalls = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubscriptionPlans", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PlanModules",
                schema: "Subscription",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PlanId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ModuleName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsEnabled = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanModules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlanModules_SubscriptionPlans_PlanId",
                        column: x => x.PlanId,
                        principalSchema: "Subscription",
                        principalTable: "SubscriptionPlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlanPrices",
                schema: "Subscription",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PlanId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CurrencyCode = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Interval = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanPrices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlanPrices_SubscriptionPlans_PlanId",
                        column: x => x.PlanId,
                        principalSchema: "Subscription",
                        principalTable: "SubscriptionPlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TenantSubscriptions",
                schema: "Subscription",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TenantId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PlanId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TrialEndsAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CurrentPeriodStart = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CurrentPeriodEnd = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AutoRenew = table.Column<bool>(type: "bit", nullable: false),
                    BillingCycle = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    BillingAnchorDay = table.Column<int>(type: "int", nullable: false),
                    CurrencyCode = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    ExternalSubscriptionId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ExternalCustomerId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CurrentUsers = table.Column<int>(type: "int", nullable: false),
                    CurrentStorageBytes = table.Column<long>(type: "bigint", nullable: false),
                    CurrentProducts = table.Column<int>(type: "int", nullable: false),
                    CurrentMonthTransactions = table.Column<int>(type: "int", nullable: false),
                    CurrentMonthApiCalls = table.Column<int>(type: "int", nullable: false),
                    LastQuotaResetAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CanceledAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CancelReason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TenantSubscriptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TenantSubscriptions_SubscriptionPlans_PlanId",
                        column: x => x.PlanId,
                        principalSchema: "Subscription",
                        principalTable: "SubscriptionPlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SubscriptionHistory",
                schema: "Subscription",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TenantSubscriptionId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    EventType = table.Column<int>(type: "int", nullable: false),
                    FromPlanId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ToPlanId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    FromStatus = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ToStatus = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    PerformedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubscriptionHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubscriptionHistory_TenantSubscriptions_TenantSubscriptionId",
                        column: x => x.TenantSubscriptionId,
                        principalSchema: "Subscription",
                        principalTable: "TenantSubscriptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UsageHistory",
                schema: "Subscription",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TenantSubscriptionId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Users = table.Column<int>(type: "int", nullable: false),
                    StorageBytes = table.Column<long>(type: "bigint", nullable: false),
                    Products = table.Column<int>(type: "int", nullable: false),
                    Transactions = table.Column<int>(type: "int", nullable: false),
                    ApiCalls = table.Column<int>(type: "int", nullable: false),
                    PeriodStart = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PeriodEnd = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SnapshotAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HasOverage = table.Column<bool>(type: "bit", nullable: false),
                    OverageDetails = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsageHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UsageHistory_TenantSubscriptions_TenantSubscriptionId",
                        column: x => x.TenantSubscriptionId,
                        principalSchema: "Subscription",
                        principalTable: "TenantSubscriptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlanModules_PlanId_ModuleName",
                schema: "Subscription",
                table: "PlanModules",
                columns: new[] { "PlanId", "ModuleName" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlanPrices_IsActive",
                schema: "Subscription",
                table: "PlanPrices",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_PlanPrices_PlanId_CurrencyCode_Interval",
                schema: "Subscription",
                table: "PlanPrices",
                columns: new[] { "PlanId", "CurrencyCode", "Interval" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SubscriptionHistory_CreatedAt",
                schema: "Subscription",
                table: "SubscriptionHistory",
                column: "CreatedAt",
                descending: new bool[0]);

            migrationBuilder.CreateIndex(
                name: "IX_SubscriptionHistory_EventType",
                schema: "Subscription",
                table: "SubscriptionHistory",
                column: "EventType");

            migrationBuilder.CreateIndex(
                name: "IX_SubscriptionHistory_TenantSubscriptionId",
                schema: "Subscription",
                table: "SubscriptionHistory",
                column: "TenantSubscriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_SubscriptionPlans_Code",
                schema: "Subscription",
                table: "SubscriptionPlans",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SubscriptionPlans_IsActive",
                schema: "Subscription",
                table: "SubscriptionPlans",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_SubscriptionPlans_Name",
                schema: "Subscription",
                table: "SubscriptionPlans",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SubscriptionPlans_SortOrder",
                schema: "Subscription",
                table: "SubscriptionPlans",
                column: "SortOrder");

            migrationBuilder.CreateIndex(
                name: "IX_TenantSubscriptions_BillingAnchorDay",
                schema: "Subscription",
                table: "TenantSubscriptions",
                column: "BillingAnchorDay");

            migrationBuilder.CreateIndex(
                name: "IX_TenantSubscriptions_CurrentPeriodEnd",
                schema: "Subscription",
                table: "TenantSubscriptions",
                column: "CurrentPeriodEnd");

            migrationBuilder.CreateIndex(
                name: "IX_TenantSubscriptions_PlanId",
                schema: "Subscription",
                table: "TenantSubscriptions",
                column: "PlanId");

            migrationBuilder.CreateIndex(
                name: "IX_TenantSubscriptions_Status",
                schema: "Subscription",
                table: "TenantSubscriptions",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_TenantSubscriptions_TenantId",
                schema: "Subscription",
                table: "TenantSubscriptions",
                column: "TenantId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UsageHistory_HasOverage",
                schema: "Subscription",
                table: "UsageHistory",
                column: "HasOverage");

            migrationBuilder.CreateIndex(
                name: "IX_UsageHistory_PeriodEnd",
                schema: "Subscription",
                table: "UsageHistory",
                column: "PeriodEnd",
                descending: new bool[0]);

            migrationBuilder.CreateIndex(
                name: "IX_UsageHistory_TenantSubscriptionId",
                schema: "Subscription",
                table: "UsageHistory",
                column: "TenantSubscriptionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlanModules",
                schema: "Subscription");

            migrationBuilder.DropTable(
                name: "PlanPrices",
                schema: "Subscription");

            migrationBuilder.DropTable(
                name: "SubscriptionHistory",
                schema: "Subscription");

            migrationBuilder.DropTable(
                name: "UsageHistory",
                schema: "Subscription");

            migrationBuilder.DropTable(
                name: "TenantSubscriptions",
                schema: "Subscription");

            migrationBuilder.DropTable(
                name: "SubscriptionPlans",
                schema: "Subscription");
        }
    }
}
