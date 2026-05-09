using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Accounting.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class removeIdempFromJournalInAccounting : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_JournalEntries_TenantId_SourceType_SourceId",
                schema: "Accounting",
                table: "JournalEntries");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_JournalEntries_TenantId_SourceType_SourceId",
                schema: "Accounting",
                table: "JournalEntries",
                columns: new[] { "TenantId", "SourceType", "SourceId" },
                unique: true);
        }
    }
}
