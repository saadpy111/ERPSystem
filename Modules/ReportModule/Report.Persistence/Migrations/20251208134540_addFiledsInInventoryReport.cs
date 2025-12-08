using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Report.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class addFiledsInInventoryReport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductId",
                schema: "Report",
                table: "InventoryReports");

            migrationBuilder.DropColumn(
                name: "WarehouseId",
                schema: "Report",
                table: "InventoryReports");

            migrationBuilder.RenameColumn(
                name: "ProductCode",
                schema: "Report",
                table: "InventoryReports",
                newName: "Sku");

            migrationBuilder.AlterColumn<decimal>(
                name: "UnitCost",
                schema: "Report",
                table: "InventoryReports",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalQuantity",
                schema: "Report",
                table: "InventoryReports",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalCost",
                schema: "Report",
                table: "InventoryReports",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "ReservedQuantity",
                schema: "Report",
                table: "InventoryReports",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "QuarantineQuantity",
                schema: "Report",
                table: "InventoryReports",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "AvailableQuantity",
                schema: "Report",
                table: "InventoryReports",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddColumn<decimal>(
                name: "CostPrice",
                schema: "Report",
                table: "InventoryReports",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                schema: "Report",
                table: "InventoryReports",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                schema: "Report",
                table: "InventoryReports",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "MainSupplierName",
                schema: "Report",
                table: "InventoryReports",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OrderLimit",
                schema: "Report",
                table: "InventoryReports",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "SalePrice",
                schema: "Report",
                table: "InventoryReports",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Tax",
                schema: "Report",
                table: "InventoryReports",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UnitOfMeasure",
                schema: "Report",
                table: "InventoryReports",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CostPrice",
                schema: "Report",
                table: "InventoryReports");

            migrationBuilder.DropColumn(
                name: "Description",
                schema: "Report",
                table: "InventoryReports");

            migrationBuilder.DropColumn(
                name: "IsActive",
                schema: "Report",
                table: "InventoryReports");

            migrationBuilder.DropColumn(
                name: "MainSupplierName",
                schema: "Report",
                table: "InventoryReports");

            migrationBuilder.DropColumn(
                name: "OrderLimit",
                schema: "Report",
                table: "InventoryReports");

            migrationBuilder.DropColumn(
                name: "SalePrice",
                schema: "Report",
                table: "InventoryReports");

            migrationBuilder.DropColumn(
                name: "Tax",
                schema: "Report",
                table: "InventoryReports");

            migrationBuilder.DropColumn(
                name: "UnitOfMeasure",
                schema: "Report",
                table: "InventoryReports");

            migrationBuilder.RenameColumn(
                name: "Sku",
                schema: "Report",
                table: "InventoryReports",
                newName: "ProductCode");

            migrationBuilder.AlterColumn<decimal>(
                name: "UnitCost",
                schema: "Report",
                table: "InventoryReports",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalQuantity",
                schema: "Report",
                table: "InventoryReports",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalCost",
                schema: "Report",
                table: "InventoryReports",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "ReservedQuantity",
                schema: "Report",
                table: "InventoryReports",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "QuarantineQuantity",
                schema: "Report",
                table: "InventoryReports",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "AvailableQuantity",
                schema: "Report",
                table: "InventoryReports",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                schema: "Report",
                table: "InventoryReports",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WarehouseId",
                schema: "Report",
                table: "InventoryReports",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
