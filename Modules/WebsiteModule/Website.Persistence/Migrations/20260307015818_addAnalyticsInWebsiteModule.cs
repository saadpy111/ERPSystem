using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Website.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class addAnalyticsInWebsiteModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WebsiteAnalyticsDaily",
                schema: "Website",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Visitors = table.Column<int>(type: "int", nullable: false),
                    UniqueVisitors = table.Column<int>(type: "int", nullable: false),
                    ActiveUsers = table.Column<int>(type: "int", nullable: false),
                    AddToCart = table.Column<int>(type: "int", nullable: false),
                    CheckoutStarted = table.Column<int>(type: "int", nullable: false),
                    OrdersCompleted = table.Column<int>(type: "int", nullable: false),
                    Revenue = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    TenantId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WebsiteAnalyticsDaily", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WebsiteVisitorSessions",
                schema: "Website",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SessionId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IpAddress = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    UserAgent = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    StartedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastSeenAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TenantId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WebsiteVisitorSessions", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WebsiteAnalyticsDaily_TenantId",
                schema: "Website",
                table: "WebsiteAnalyticsDaily",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_WebsiteAnalyticsDaily_TenantId_Date",
                schema: "Website",
                table: "WebsiteAnalyticsDaily",
                columns: new[] { "TenantId", "Date" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WebsiteVisitorSessions_SessionId",
                schema: "Website",
                table: "WebsiteVisitorSessions",
                column: "SessionId");

            migrationBuilder.CreateIndex(
                name: "IX_WebsiteVisitorSessions_TenantId",
                schema: "Website",
                table: "WebsiteVisitorSessions",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_WebsiteVisitorSessions_TenantId_SessionId",
                schema: "Website",
                table: "WebsiteVisitorSessions",
                columns: new[] { "TenantId", "SessionId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WebsiteAnalyticsDaily",
                schema: "Website");

            migrationBuilder.DropTable(
                name: "WebsiteVisitorSessions",
                schema: "Website");
        }
    }
}
