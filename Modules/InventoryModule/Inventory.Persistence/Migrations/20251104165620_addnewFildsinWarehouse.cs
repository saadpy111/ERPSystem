using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inventory.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class addnewFildsinWarehouse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ContactNumber",
                schema: "Inventory",
                table: "Warehouses",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FinancialAccountCode",
                schema: "Inventory",
                table: "Warehouses",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Government",
                schema: "Inventory",
                table: "Warehouses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InventoryPolicy",
                schema: "Inventory",
                table: "Warehouses",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                schema: "Inventory",
                table: "Warehouses",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "PercentageUtilized",
                schema: "Inventory",
                table: "Warehouses",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ResponsibleEmployee",
                schema: "Inventory",
                table: "Warehouses",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TotalStorageCapacity",
                schema: "Inventory",
                table: "Warehouses",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "UtilizationPercentage",
                schema: "Inventory",
                table: "Warehouses",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WarehouseCode",
                schema: "Inventory",
                table: "Warehouses",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WarehouseType",
                schema: "Inventory",
                table: "Warehouses",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContactNumber",
                schema: "Inventory",
                table: "Warehouses");

            migrationBuilder.DropColumn(
                name: "FinancialAccountCode",
                schema: "Inventory",
                table: "Warehouses");

            migrationBuilder.DropColumn(
                name: "Government",
                schema: "Inventory",
                table: "Warehouses");

            migrationBuilder.DropColumn(
                name: "InventoryPolicy",
                schema: "Inventory",
                table: "Warehouses");

            migrationBuilder.DropColumn(
                name: "IsActive",
                schema: "Inventory",
                table: "Warehouses");

            migrationBuilder.DropColumn(
                name: "PercentageUtilized",
                schema: "Inventory",
                table: "Warehouses");

            migrationBuilder.DropColumn(
                name: "ResponsibleEmployee",
                schema: "Inventory",
                table: "Warehouses");

            migrationBuilder.DropColumn(
                name: "TotalStorageCapacity",
                schema: "Inventory",
                table: "Warehouses");

            migrationBuilder.DropColumn(
                name: "UtilizationPercentage",
                schema: "Inventory",
                table: "Warehouses");

            migrationBuilder.DropColumn(
                name: "WarehouseCode",
                schema: "Inventory",
                table: "Warehouses");

            migrationBuilder.DropColumn(
                name: "WarehouseType",
                schema: "Inventory",
                table: "Warehouses");
        }
    }
}
