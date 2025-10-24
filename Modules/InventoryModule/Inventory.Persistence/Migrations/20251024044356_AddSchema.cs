using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inventory.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Inventory");

            migrationBuilder.RenameTable(
                name: "Warehouses",
                newName: "Warehouses",
                newSchema: "Inventory");

            migrationBuilder.RenameTable(
                name: "StockQuants",
                newName: "StockQuants",
                newSchema: "Inventory");

            migrationBuilder.RenameTable(
                name: "StockMoves",
                newName: "StockMoves",
                newSchema: "Inventory");

            migrationBuilder.RenameTable(
                name: "StockAdjustments",
                newName: "StockAdjustments",
                newSchema: "Inventory");

            migrationBuilder.RenameTable(
                name: "SerialOrBatchNumbers",
                newName: "SerialOrBatchNumbers",
                newSchema: "Inventory");

            migrationBuilder.RenameTable(
                name: "Products",
                newName: "Products",
                newSchema: "Inventory");

            migrationBuilder.RenameTable(
                name: "ProductImages",
                newName: "ProductImages",
                newSchema: "Inventory");

            migrationBuilder.RenameTable(
                name: "ProductCostHistories",
                newName: "ProductCostHistories",
                newSchema: "Inventory");

            migrationBuilder.RenameTable(
                name: "ProductCategories",
                newName: "ProductCategories",
                newSchema: "Inventory");

            migrationBuilder.RenameTable(
                name: "ProductBarcodes",
                newName: "ProductBarcodes",
                newSchema: "Inventory");

            migrationBuilder.RenameTable(
                name: "ProductAttributeValues",
                newName: "ProductAttributeValues",
                newSchema: "Inventory");

            migrationBuilder.RenameTable(
                name: "ProductAttributes",
                newName: "ProductAttributes",
                newSchema: "Inventory");

            migrationBuilder.RenameTable(
                name: "Locations",
                newName: "Locations",
                newSchema: "Inventory");

            migrationBuilder.RenameTable(
                name: "InventoryQuarantines",
                newName: "InventoryQuarantines",
                newSchema: "Inventory");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "Warehouses",
                schema: "Inventory",
                newName: "Warehouses");

            migrationBuilder.RenameTable(
                name: "StockQuants",
                schema: "Inventory",
                newName: "StockQuants");

            migrationBuilder.RenameTable(
                name: "StockMoves",
                schema: "Inventory",
                newName: "StockMoves");

            migrationBuilder.RenameTable(
                name: "StockAdjustments",
                schema: "Inventory",
                newName: "StockAdjustments");

            migrationBuilder.RenameTable(
                name: "SerialOrBatchNumbers",
                schema: "Inventory",
                newName: "SerialOrBatchNumbers");

            migrationBuilder.RenameTable(
                name: "Products",
                schema: "Inventory",
                newName: "Products");

            migrationBuilder.RenameTable(
                name: "ProductImages",
                schema: "Inventory",
                newName: "ProductImages");

            migrationBuilder.RenameTable(
                name: "ProductCostHistories",
                schema: "Inventory",
                newName: "ProductCostHistories");

            migrationBuilder.RenameTable(
                name: "ProductCategories",
                schema: "Inventory",
                newName: "ProductCategories");

            migrationBuilder.RenameTable(
                name: "ProductBarcodes",
                schema: "Inventory",
                newName: "ProductBarcodes");

            migrationBuilder.RenameTable(
                name: "ProductAttributeValues",
                schema: "Inventory",
                newName: "ProductAttributeValues");

            migrationBuilder.RenameTable(
                name: "ProductAttributes",
                schema: "Inventory",
                newName: "ProductAttributes");

            migrationBuilder.RenameTable(
                name: "Locations",
                schema: "Inventory",
                newName: "Locations");

            migrationBuilder.RenameTable(
                name: "InventoryQuarantines",
                schema: "Inventory",
                newName: "InventoryQuarantines");
        }
    }
}
