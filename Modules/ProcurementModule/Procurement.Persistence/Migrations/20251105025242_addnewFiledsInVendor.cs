using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Procurement.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class addnewFiledsInVendor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "City",
                schema: "Procurement",
                table: "Vendors",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CommercialRegistrationNumber",
                schema: "Procurement",
                table: "Vendors",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Currency",
                schema: "Procurement",
                table: "Vendors",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Govrenment",
                schema: "Procurement",
                table: "Vendors",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                schema: "Procurement",
                table: "Vendors",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PaymentMethod",
                schema: "Procurement",
                table: "Vendors",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProductVendorName",
                schema: "Procurement",
                table: "Vendors",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Rate",
                schema: "Procurement",
                table: "Vendors",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "SupplierCreditLimit",
                schema: "Procurement",
                table: "Vendors",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VendorCode",
                schema: "Procurement",
                table: "Vendors",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Webpage",
                schema: "Procurement",
                table: "Vendors",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "City",
                schema: "Procurement",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "CommercialRegistrationNumber",
                schema: "Procurement",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "Currency",
                schema: "Procurement",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "Govrenment",
                schema: "Procurement",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "Notes",
                schema: "Procurement",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "PaymentMethod",
                schema: "Procurement",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "ProductVendorName",
                schema: "Procurement",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "Rate",
                schema: "Procurement",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "SupplierCreditLimit",
                schema: "Procurement",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "VendorCode",
                schema: "Procurement",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "Webpage",
                schema: "Procurement",
                table: "Vendors");
        }
    }
}
