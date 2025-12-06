using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Report.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class removeConFromDatasource : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConnectionString",
                schema: "Report",
                table: "ReportDataSources");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ConnectionString",
                schema: "Report",
                table: "ReportDataSources",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");
        }
    }
}
