using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inventory.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class addtenantinInventory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TenantId",
                schema: "Inventory",
                table: "Warehouses",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TenantId",
                schema: "Inventory",
                table: "StockQuants",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TenantId",
                schema: "Inventory",
                table: "StockMoves",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TenantId",
                schema: "Inventory",
                table: "StockAdjustments",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TenantId",
                schema: "Inventory",
                table: "SerialOrBatchNumbers",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TenantId",
                schema: "Inventory",
                table: "Products",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TenantId",
                schema: "Inventory",
                table: "ProductImages",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TenantId",
                schema: "Inventory",
                table: "ProductCostHistories",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TenantId",
                schema: "Inventory",
                table: "ProductCategories",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TenantId",
                schema: "Inventory",
                table: "ProductBarcodes",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TenantId",
                schema: "Inventory",
                table: "ProductAttributeValues",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TenantId",
                schema: "Inventory",
                table: "ProductAttributes",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TenantId",
                schema: "Inventory",
                table: "Locations",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TenantId",
                schema: "Inventory",
                table: "InventoryQuarantines",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TenantId",
                schema: "Inventory",
                table: "Attachments",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Warehouses_TenantId",
                schema: "Inventory",
                table: "Warehouses",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_StockQuants_TenantId",
                schema: "Inventory",
                table: "StockQuants",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_StockMoves_TenantId",
                schema: "Inventory",
                table: "StockMoves",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_StockAdjustments_TenantId",
                schema: "Inventory",
                table: "StockAdjustments",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_SerialOrBatchNumbers_TenantId",
                schema: "Inventory",
                table: "SerialOrBatchNumbers",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_TenantId",
                schema: "Inventory",
                table: "Products",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductImages_TenantId",
                schema: "Inventory",
                table: "ProductImages",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductCostHistories_TenantId",
                schema: "Inventory",
                table: "ProductCostHistories",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductCategories_TenantId",
                schema: "Inventory",
                table: "ProductCategories",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductBarcodes_TenantId",
                schema: "Inventory",
                table: "ProductBarcodes",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductAttributeValues_TenantId",
                schema: "Inventory",
                table: "ProductAttributeValues",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductAttributes_TenantId",
                schema: "Inventory",
                table: "ProductAttributes",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_TenantId",
                schema: "Inventory",
                table: "Locations",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryQuarantines_TenantId",
                schema: "Inventory",
                table: "InventoryQuarantines",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Attachments_TenantId",
                schema: "Inventory",
                table: "Attachments",
                column: "TenantId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Warehouses_TenantId",
                schema: "Inventory",
                table: "Warehouses");

            migrationBuilder.DropIndex(
                name: "IX_StockQuants_TenantId",
                schema: "Inventory",
                table: "StockQuants");

            migrationBuilder.DropIndex(
                name: "IX_StockMoves_TenantId",
                schema: "Inventory",
                table: "StockMoves");

            migrationBuilder.DropIndex(
                name: "IX_StockAdjustments_TenantId",
                schema: "Inventory",
                table: "StockAdjustments");

            migrationBuilder.DropIndex(
                name: "IX_SerialOrBatchNumbers_TenantId",
                schema: "Inventory",
                table: "SerialOrBatchNumbers");

            migrationBuilder.DropIndex(
                name: "IX_Products_TenantId",
                schema: "Inventory",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_ProductImages_TenantId",
                schema: "Inventory",
                table: "ProductImages");

            migrationBuilder.DropIndex(
                name: "IX_ProductCostHistories_TenantId",
                schema: "Inventory",
                table: "ProductCostHistories");

            migrationBuilder.DropIndex(
                name: "IX_ProductCategories_TenantId",
                schema: "Inventory",
                table: "ProductCategories");

            migrationBuilder.DropIndex(
                name: "IX_ProductBarcodes_TenantId",
                schema: "Inventory",
                table: "ProductBarcodes");

            migrationBuilder.DropIndex(
                name: "IX_ProductAttributeValues_TenantId",
                schema: "Inventory",
                table: "ProductAttributeValues");

            migrationBuilder.DropIndex(
                name: "IX_ProductAttributes_TenantId",
                schema: "Inventory",
                table: "ProductAttributes");

            migrationBuilder.DropIndex(
                name: "IX_Locations_TenantId",
                schema: "Inventory",
                table: "Locations");

            migrationBuilder.DropIndex(
                name: "IX_InventoryQuarantines_TenantId",
                schema: "Inventory",
                table: "InventoryQuarantines");

            migrationBuilder.DropIndex(
                name: "IX_Attachments_TenantId",
                schema: "Inventory",
                table: "Attachments");

            migrationBuilder.DropColumn(
                name: "TenantId",
                schema: "Inventory",
                table: "Warehouses");

            migrationBuilder.DropColumn(
                name: "TenantId",
                schema: "Inventory",
                table: "StockQuants");

            migrationBuilder.DropColumn(
                name: "TenantId",
                schema: "Inventory",
                table: "StockMoves");

            migrationBuilder.DropColumn(
                name: "TenantId",
                schema: "Inventory",
                table: "StockAdjustments");

            migrationBuilder.DropColumn(
                name: "TenantId",
                schema: "Inventory",
                table: "SerialOrBatchNumbers");

            migrationBuilder.DropColumn(
                name: "TenantId",
                schema: "Inventory",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "TenantId",
                schema: "Inventory",
                table: "ProductImages");

            migrationBuilder.DropColumn(
                name: "TenantId",
                schema: "Inventory",
                table: "ProductCostHistories");

            migrationBuilder.DropColumn(
                name: "TenantId",
                schema: "Inventory",
                table: "ProductCategories");

            migrationBuilder.DropColumn(
                name: "TenantId",
                schema: "Inventory",
                table: "ProductBarcodes");

            migrationBuilder.DropColumn(
                name: "TenantId",
                schema: "Inventory",
                table: "ProductAttributeValues");

            migrationBuilder.DropColumn(
                name: "TenantId",
                schema: "Inventory",
                table: "ProductAttributes");

            migrationBuilder.DropColumn(
                name: "TenantId",
                schema: "Inventory",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "TenantId",
                schema: "Inventory",
                table: "InventoryQuarantines");

            migrationBuilder.DropColumn(
                name: "TenantId",
                schema: "Inventory",
                table: "Attachments");
        }
    }
}
