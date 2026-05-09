using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Accounting.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddCurrencyForPayableAndReciInAccounting : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JournalEntries_JournalEntries_ReversedEntryId",
                schema: "Accounting",
                table: "JournalEntries");

            migrationBuilder.DropColumn(
                name: "ReversalEntryId",
                schema: "Accounting",
                table: "JournalEntries");

            migrationBuilder.RenameColumn(
                name: "ReversedEntryId",
                schema: "Accounting",
                table: "JournalEntries",
                newName: "ReversedSourceJournalId");

            migrationBuilder.RenameIndex(
                name: "IX_JournalEntries_ReversedEntryId",
                schema: "Accounting",
                table: "JournalEntries",
                newName: "IX_JournalEntries_ReversedSourceJournalId");

            migrationBuilder.AddColumn<decimal>(
                name: "BaseAmount",
                schema: "Accounting",
                table: "VoucherLines",
                type: "decimal(18,6)",
                precision: 18,
                scale: 6,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ExchangeRate",
                schema: "Accounting",
                table: "VoucherLines",
                type: "decimal(18,6)",
                precision: 18,
                scale: 6,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ForeignAmount",
                schema: "Accounting",
                table: "VoucherLines",
                type: "decimal(18,6)",
                precision: 18,
                scale: 6,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "BaseAmount",
                schema: "Accounting",
                table: "ReceivablePayments",
                type: "decimal(18,6)",
                precision: 18,
                scale: 6,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "CurrencyId",
                schema: "Accounting",
                table: "ReceivablePayments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "ExchangeRate",
                schema: "Accounting",
                table: "ReceivablePayments",
                type: "decimal(18,6)",
                precision: 18,
                scale: 6,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "ForeignAmount",
                schema: "Accounting",
                table: "ReceivablePayments",
                type: "decimal(18,6)",
                precision: 18,
                scale: 6,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "BaseAmount",
                schema: "Accounting",
                table: "PayablePayments",
                type: "decimal(18,6)",
                precision: 18,
                scale: 6,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "CurrencyId",
                schema: "Accounting",
                table: "PayablePayments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "ExchangeRate",
                schema: "Accounting",
                table: "PayablePayments",
                type: "decimal(18,6)",
                precision: 18,
                scale: 6,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "ForeignAmount",
                schema: "Accounting",
                table: "PayablePayments",
                type: "decimal(18,6)",
                precision: 18,
                scale: 6,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "ApprovedBy",
                schema: "Accounting",
                table: "JournalEntries",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PostedBy",
                schema: "Accounting",
                table: "JournalEntries",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "AllowNegativeBalance",
                schema: "Accounting",
                table: "CashAccounts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_JournalEntries_JournalEntries_ReversedSourceJournalId",
                schema: "Accounting",
                table: "JournalEntries",
                column: "ReversedSourceJournalId",
                principalSchema: "Accounting",
                principalTable: "JournalEntries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JournalEntries_JournalEntries_ReversedSourceJournalId",
                schema: "Accounting",
                table: "JournalEntries");

            migrationBuilder.DropColumn(
                name: "BaseAmount",
                schema: "Accounting",
                table: "VoucherLines");

            migrationBuilder.DropColumn(
                name: "ExchangeRate",
                schema: "Accounting",
                table: "VoucherLines");

            migrationBuilder.DropColumn(
                name: "ForeignAmount",
                schema: "Accounting",
                table: "VoucherLines");

            migrationBuilder.DropColumn(
                name: "BaseAmount",
                schema: "Accounting",
                table: "ReceivablePayments");

            migrationBuilder.DropColumn(
                name: "CurrencyId",
                schema: "Accounting",
                table: "ReceivablePayments");

            migrationBuilder.DropColumn(
                name: "ExchangeRate",
                schema: "Accounting",
                table: "ReceivablePayments");

            migrationBuilder.DropColumn(
                name: "ForeignAmount",
                schema: "Accounting",
                table: "ReceivablePayments");

            migrationBuilder.DropColumn(
                name: "BaseAmount",
                schema: "Accounting",
                table: "PayablePayments");

            migrationBuilder.DropColumn(
                name: "CurrencyId",
                schema: "Accounting",
                table: "PayablePayments");

            migrationBuilder.DropColumn(
                name: "ExchangeRate",
                schema: "Accounting",
                table: "PayablePayments");

            migrationBuilder.DropColumn(
                name: "ForeignAmount",
                schema: "Accounting",
                table: "PayablePayments");

            migrationBuilder.DropColumn(
                name: "ApprovedBy",
                schema: "Accounting",
                table: "JournalEntries");

            migrationBuilder.DropColumn(
                name: "PostedBy",
                schema: "Accounting",
                table: "JournalEntries");

            migrationBuilder.DropColumn(
                name: "AllowNegativeBalance",
                schema: "Accounting",
                table: "CashAccounts");

            migrationBuilder.RenameColumn(
                name: "ReversedSourceJournalId",
                schema: "Accounting",
                table: "JournalEntries",
                newName: "ReversedEntryId");

            migrationBuilder.RenameIndex(
                name: "IX_JournalEntries_ReversedSourceJournalId",
                schema: "Accounting",
                table: "JournalEntries",
                newName: "IX_JournalEntries_ReversedEntryId");

            migrationBuilder.AddColumn<int>(
                name: "ReversalEntryId",
                schema: "Accounting",
                table: "JournalEntries",
                type: "int",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_JournalEntries_JournalEntries_ReversedEntryId",
                schema: "Accounting",
                table: "JournalEntries",
                column: "ReversedEntryId",
                principalSchema: "Accounting",
                principalTable: "JournalEntries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
