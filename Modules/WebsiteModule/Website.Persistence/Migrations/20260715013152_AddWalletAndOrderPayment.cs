using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Website.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddWalletAndOrderPayment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                schema: "Website",
                table: "Orders",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "TenantId",
                schema: "Website",
                table: "Orders",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "OrderNumber",
                schema: "Website",
                table: "Orders",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                schema: "Website",
                table: "Orders",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CustomerPhone",
                schema: "Website",
                table: "Orders",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "CustomerName",
                schema: "Website",
                table: "Orders",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<DateTime>(
                name: "PaidAt",
                schema: "Website",
                table: "Orders",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PaymentId",
                schema: "Website",
                table: "Orders",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Wallets",
                schema: "Website",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CurrentBalance = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    TenantId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wallets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WalletTransactions",
                schema: "Website",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WalletId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    BalanceBefore = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    BalanceAfter = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Reference = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    TenantId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WalletTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WalletTransactions_Wallets_WalletId",
                        column: x => x.WalletId,
                        principalSchema: "Website",
                        principalTable: "Wallets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WithdrawalRequests",
                schema: "Website",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WalletId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    RequestedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReviewedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReviewedBy = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    TenantId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WithdrawalRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WithdrawalRequests_Wallets_WalletId",
                        column: x => x.WalletId,
                        principalSchema: "Website",
                        principalTable: "Wallets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_OrderNumber",
                schema: "Website",
                table: "Orders",
                column: "OrderNumber");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_PaymentId",
                schema: "Website",
                table: "Orders",
                column: "PaymentId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_UserId",
                schema: "Website",
                table: "Orders",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Wallets_TenantId",
                schema: "Website",
                table: "Wallets",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_WalletTransactions_TenantId",
                schema: "Website",
                table: "WalletTransactions",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_WalletTransactions_WalletId_CreatedAt",
                schema: "Website",
                table: "WalletTransactions",
                columns: new[] { "WalletId", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_WithdrawalRequests_Status",
                schema: "Website",
                table: "WithdrawalRequests",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_WithdrawalRequests_TenantId",
                schema: "Website",
                table: "WithdrawalRequests",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_WithdrawalRequests_WalletId_Status",
                schema: "Website",
                table: "WithdrawalRequests",
                columns: new[] { "WalletId", "Status" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WalletTransactions",
                schema: "Website");

            migrationBuilder.DropTable(
                name: "WithdrawalRequests",
                schema: "Website");

            migrationBuilder.DropTable(
                name: "Wallets",
                schema: "Website");

            migrationBuilder.DropIndex(
                name: "IX_Orders_OrderNumber",
                schema: "Website",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_PaymentId",
                schema: "Website",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_UserId",
                schema: "Website",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "PaidAt",
                schema: "Website",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "PaymentId",
                schema: "Website",
                table: "Orders");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                schema: "Website",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldMaxLength: 450);

            migrationBuilder.AlterColumn<string>(
                name: "TenantId",
                schema: "Website",
                table: "Orders",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "OrderNumber",
                schema: "Website",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                schema: "Website",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CustomerPhone",
                schema: "Website",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "CustomerName",
                schema: "Website",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);
        }
    }
}
