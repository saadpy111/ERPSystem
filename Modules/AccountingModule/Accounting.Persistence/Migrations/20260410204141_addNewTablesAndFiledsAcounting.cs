using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Accounting.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class addNewTablesAndFiledsAcounting : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Amount",
                schema: "Accounting",
                table: "Vouchers",
                newName: "TotalAmount");

            migrationBuilder.RenameColumn(
                name: "Name",
                schema: "Accounting",
                table: "CostCenters",
                newName: "NameAr");

            migrationBuilder.AlterColumn<int>(
                name: "PaymentMethod",
                schema: "Accounting",
                table: "Vouchers",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "PartnerId",
                schema: "Accounting",
                table: "Vouchers",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "JournalEntryId",
                schema: "Accounting",
                table: "Vouchers",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                schema: "Accounting",
                table: "Vouchers",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Reference",
                schema: "Accounting",
                table: "Vouchers",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Code",
                schema: "Accounting",
                table: "CostCenters",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Level",
                schema: "Accounting",
                table: "CostCenters",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NameEn",
                schema: "Accounting",
                table: "CostCenters",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Budgets",
                schema: "Accounting",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    FiscalYearId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    EnforceBudgetControl = table.Column<bool>(type: "bit", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Budgets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Budgets_FiscalYears_FiscalYearId",
                        column: x => x.FiscalYearId,
                        principalSchema: "Accounting",
                        principalTable: "FiscalYears",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "VoucherLines",
                schema: "Accounting",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VoucherId = table.Column<int>(type: "int", nullable: false),
                    AccountId = table.Column<int>(type: "int", nullable: false),
                    Debit = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    Credit = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    CurrencyId = table.Column<int>(type: "int", nullable: false),
                    CostCenterId = table.Column<int>(type: "int", nullable: true),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VoucherLines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VoucherLines_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalSchema: "Accounting",
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VoucherLines_CostCenters_CostCenterId",
                        column: x => x.CostCenterId,
                        principalSchema: "Accounting",
                        principalTable: "CostCenters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VoucherLines_Currencies_CurrencyId",
                        column: x => x.CurrencyId,
                        principalSchema: "Accounting",
                        principalTable: "Currencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VoucherLines_Vouchers_VoucherId",
                        column: x => x.VoucherId,
                        principalSchema: "Accounting",
                        principalTable: "Vouchers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BudgetLines",
                schema: "Accounting",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BudgetId = table.Column<int>(type: "int", nullable: false),
                    AccountId = table.Column<int>(type: "int", nullable: false),
                    CostCenterId = table.Column<int>(type: "int", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PlannedAmount = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BudgetLines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BudgetLines_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalSchema: "Accounting",
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BudgetLines_Budgets_BudgetId",
                        column: x => x.BudgetId,
                        principalSchema: "Accounting",
                        principalTable: "Budgets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BudgetLines_CostCenters_CostCenterId",
                        column: x => x.CostCenterId,
                        principalSchema: "Accounting",
                        principalTable: "CostCenters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_JournalEntryLines_AccountId_CostCenterId",
                schema: "Accounting",
                table: "JournalEntryLines",
                columns: new[] { "AccountId", "CostCenterId" });

            migrationBuilder.CreateIndex(
                name: "IX_CostCenters_TenantId_Code",
                schema: "Accounting",
                table: "CostCenters",
                columns: new[] { "TenantId", "Code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BudgetLines_AccountId",
                schema: "Accounting",
                table: "BudgetLines",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetLines_BudgetId",
                schema: "Accounting",
                table: "BudgetLines",
                column: "BudgetId");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetLines_CostCenterId",
                schema: "Accounting",
                table: "BudgetLines",
                column: "CostCenterId");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetLines_TenantId",
                schema: "Accounting",
                table: "BudgetLines",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Budgets_FiscalYearId",
                schema: "Accounting",
                table: "Budgets",
                column: "FiscalYearId");

            migrationBuilder.CreateIndex(
                name: "IX_Budgets_TenantId",
                schema: "Accounting",
                table: "Budgets",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_VoucherLines_AccountId",
                schema: "Accounting",
                table: "VoucherLines",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_VoucherLines_CostCenterId",
                schema: "Accounting",
                table: "VoucherLines",
                column: "CostCenterId");

            migrationBuilder.CreateIndex(
                name: "IX_VoucherLines_CurrencyId",
                schema: "Accounting",
                table: "VoucherLines",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_VoucherLines_TenantId",
                schema: "Accounting",
                table: "VoucherLines",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_VoucherLines_TenantId_Id",
                schema: "Accounting",
                table: "VoucherLines",
                columns: new[] { "TenantId", "Id" });

            migrationBuilder.CreateIndex(
                name: "IX_VoucherLines_VoucherId",
                schema: "Accounting",
                table: "VoucherLines",
                column: "VoucherId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BudgetLines",
                schema: "Accounting");

            migrationBuilder.DropTable(
                name: "VoucherLines",
                schema: "Accounting");

            migrationBuilder.DropTable(
                name: "Budgets",
                schema: "Accounting");

            migrationBuilder.DropIndex(
                name: "IX_JournalEntryLines_AccountId_CostCenterId",
                schema: "Accounting",
                table: "JournalEntryLines");

            migrationBuilder.DropIndex(
                name: "IX_CostCenters_TenantId_Code",
                schema: "Accounting",
                table: "CostCenters");

            migrationBuilder.DropColumn(
                name: "Description",
                schema: "Accounting",
                table: "Vouchers");

            migrationBuilder.DropColumn(
                name: "Reference",
                schema: "Accounting",
                table: "Vouchers");

            migrationBuilder.DropColumn(
                name: "Code",
                schema: "Accounting",
                table: "CostCenters");

            migrationBuilder.DropColumn(
                name: "Level",
                schema: "Accounting",
                table: "CostCenters");

            migrationBuilder.DropColumn(
                name: "NameEn",
                schema: "Accounting",
                table: "CostCenters");

            migrationBuilder.RenameColumn(
                name: "TotalAmount",
                schema: "Accounting",
                table: "Vouchers",
                newName: "Amount");

            migrationBuilder.RenameColumn(
                name: "NameAr",
                schema: "Accounting",
                table: "CostCenters",
                newName: "Name");

            migrationBuilder.AlterColumn<int>(
                name: "PaymentMethod",
                schema: "Accounting",
                table: "Vouchers",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "PartnerId",
                schema: "Accounting",
                table: "Vouchers",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "JournalEntryId",
                schema: "Accounting",
                table: "Vouchers",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
