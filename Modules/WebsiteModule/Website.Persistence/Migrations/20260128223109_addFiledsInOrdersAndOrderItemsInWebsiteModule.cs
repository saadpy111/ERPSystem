using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Website.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class addFiledsInOrdersAndOrderItemsInWebsiteModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "DiscountTotal",
                schema: "Website",
                table: "Orders",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "SubTotal",
                schema: "Website",
                table: "Orders",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "AppliedOfferName",
                schema: "Website",
                table: "OrderItems",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "DiscountAmount",
                schema: "Website",
                table: "OrderItems",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "FinalPrice",
                schema: "Website",
                table: "OrderItems",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DiscountTotal",
                schema: "Website",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "SubTotal",
                schema: "Website",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "AppliedOfferName",
                schema: "Website",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "DiscountAmount",
                schema: "Website",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "FinalPrice",
                schema: "Website",
                table: "OrderItems");
        }
    }
}
