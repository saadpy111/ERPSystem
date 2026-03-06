using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Website.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class addCustomerAnalyticsInWebsiteModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CustomerAnalytics",
                schema: "Website",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    OrdersCount = table.Column<int>(type: "int", nullable: false),
                    TotalSpent = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    AverageOrderValue = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    LastOrderDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AverageDaysBetweenOrders = table.Column<double>(type: "float", nullable: false),
                    FavoritePurchaseDay = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    TotalItemsPurchased = table.Column<int>(type: "int", nullable: false),
                    ReturnedItems = table.Column<int>(type: "int", nullable: false),
                    ReturnRate = table.Column<double>(type: "float", nullable: false),
                    FirstOrderDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MostPurchasedCategory = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    TenantId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerAnalytics", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustomerAnalytics_TenantId",
                schema: "Website",
                table: "CustomerAnalytics",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerAnalytics_TenantId_UserId",
                schema: "Website",
                table: "CustomerAnalytics",
                columns: new[] { "TenantId", "UserId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomerAnalytics",
                schema: "Website");
        }
    }
}
