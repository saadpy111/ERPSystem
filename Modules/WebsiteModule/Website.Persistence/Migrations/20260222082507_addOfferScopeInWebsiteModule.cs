using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Website.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class addOfferScopeInWebsiteModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ScopeType",
                schema: "Website",
                table: "Offers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ScopeType",
                schema: "Website",
                table: "Offers");
        }
    }
}
