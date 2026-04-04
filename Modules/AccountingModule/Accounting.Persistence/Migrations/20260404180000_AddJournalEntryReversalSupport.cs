using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Accounting.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddJournalEntryReversalSupport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // ── 1. Add reversal tracking scalar columns ────────────────────────
            migrationBuilder.AddColumn<bool>(
                name: "IsReversed",
                schema: "Accounting",
                table: "JournalEntries",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "ReversedAt",
                schema: "Accounting",
                table: "JournalEntries",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReversedBy",
                schema: "Accounting",
                table: "JournalEntries",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReversalReason",
                schema: "Accounting",
                table: "JournalEntries",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            // ── 2. Add self-referencing FK column ─────────────────────────────
            migrationBuilder.AddColumn<int>(
                name: "ReversedEntryId",
                schema: "Accounting",
                table: "JournalEntries",
                type: "int",
                nullable: true);

            // ── 3. Add self-referencing FK constraint ─────────────────────────
            migrationBuilder.AddForeignKey(
                name: "FK_JournalEntries_JournalEntries_ReversedEntryId",
                schema: "Accounting",
                table: "JournalEntries",
                column: "ReversedEntryId",
                principalSchema: "Accounting",
                principalTable: "JournalEntries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            // ── 4. Index for FK look-ups on reversal chains ────────────────────
            migrationBuilder.CreateIndex(
                name: "IX_JournalEntries_ReversedEntryId",
                schema: "Accounting",
                table: "JournalEntries",
                column: "ReversedEntryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_JournalEntries_ReversedEntryId",
                schema: "Accounting",
                table: "JournalEntries");

            migrationBuilder.DropForeignKey(
                name: "FK_JournalEntries_JournalEntries_ReversedEntryId",
                schema: "Accounting",
                table: "JournalEntries");

            migrationBuilder.DropColumn(
                name: "IsReversed",
                schema: "Accounting",
                table: "JournalEntries");

            migrationBuilder.DropColumn(
                name: "ReversedAt",
                schema: "Accounting",
                table: "JournalEntries");

            migrationBuilder.DropColumn(
                name: "ReversedBy",
                schema: "Accounting",
                table: "JournalEntries");

            migrationBuilder.DropColumn(
                name: "ReversalReason",
                schema: "Accounting",
                table: "JournalEntries");

            migrationBuilder.DropColumn(
                name: "ReversedEntryId",
                schema: "Accounting",
                table: "JournalEntries");
        }
    }
}
