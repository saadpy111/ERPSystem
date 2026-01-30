using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inventory.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class addImageFroProductCategoryInInventoryModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                schema: "Inventory",
                table: "ProductCategories",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagePath",
                schema: "Inventory",
                table: "ProductCategories");
        }
    }
}
