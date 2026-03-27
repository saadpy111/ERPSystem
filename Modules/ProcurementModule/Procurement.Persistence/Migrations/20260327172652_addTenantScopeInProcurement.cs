using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Procurement.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class addTenantScopeInProcurement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TenantId",
                schema: "Procurement",
                table: "Vendors",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TenantId",
                schema: "Procurement",
                table: "PurchaseRequisitions",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TenantId",
                schema: "Procurement",
                table: "PurchaseOrders",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TenantId",
                schema: "Procurement",
                table: "PurchaseOrderItems",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TenantId",
                schema: "Procurement",
                table: "PurchaseInvoices",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TenantId",
                schema: "Procurement",
                table: "ProcurementAttachments",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TenantId",
                schema: "Procurement",
                table: "GoodsReceipts",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TenantId",
                schema: "Procurement",
                table: "GoodsReceiptItems",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Vendors_TenantId",
                schema: "Procurement",
                table: "Vendors",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Vendors_TenantId_Id",
                schema: "Procurement",
                table: "Vendors",
                columns: new[] { "TenantId", "Id" });

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseRequisitions_TenantId",
                schema: "Procurement",
                table: "PurchaseRequisitions",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseRequisitions_TenantId_Id",
                schema: "Procurement",
                table: "PurchaseRequisitions",
                columns: new[] { "TenantId", "Id" });

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrders_TenantId",
                schema: "Procurement",
                table: "PurchaseOrders",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrders_TenantId_Id",
                schema: "Procurement",
                table: "PurchaseOrders",
                columns: new[] { "TenantId", "Id" });

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderItems_TenantId",
                schema: "Procurement",
                table: "PurchaseOrderItems",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderItems_TenantId_Id",
                schema: "Procurement",
                table: "PurchaseOrderItems",
                columns: new[] { "TenantId", "Id" });

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseInvoices_TenantId",
                schema: "Procurement",
                table: "PurchaseInvoices",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseInvoices_TenantId_Id",
                schema: "Procurement",
                table: "PurchaseInvoices",
                columns: new[] { "TenantId", "Id" });

            migrationBuilder.CreateIndex(
                name: "IX_ProcurementAttachments_TenantId",
                schema: "Procurement",
                table: "ProcurementAttachments",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcurementAttachments_TenantId_Id",
                schema: "Procurement",
                table: "ProcurementAttachments",
                columns: new[] { "TenantId", "Id" });

            migrationBuilder.CreateIndex(
                name: "IX_GoodsReceipts_TenantId",
                schema: "Procurement",
                table: "GoodsReceipts",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_GoodsReceipts_TenantId_Id",
                schema: "Procurement",
                table: "GoodsReceipts",
                columns: new[] { "TenantId", "Id" });

            migrationBuilder.CreateIndex(
                name: "IX_GoodsReceiptItems_TenantId",
                schema: "Procurement",
                table: "GoodsReceiptItems",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_GoodsReceiptItems_TenantId_Id",
                schema: "Procurement",
                table: "GoodsReceiptItems",
                columns: new[] { "TenantId", "Id" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Vendors_TenantId",
                schema: "Procurement",
                table: "Vendors");

            migrationBuilder.DropIndex(
                name: "IX_Vendors_TenantId_Id",
                schema: "Procurement",
                table: "Vendors");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseRequisitions_TenantId",
                schema: "Procurement",
                table: "PurchaseRequisitions");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseRequisitions_TenantId_Id",
                schema: "Procurement",
                table: "PurchaseRequisitions");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseOrders_TenantId",
                schema: "Procurement",
                table: "PurchaseOrders");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseOrders_TenantId_Id",
                schema: "Procurement",
                table: "PurchaseOrders");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseOrderItems_TenantId",
                schema: "Procurement",
                table: "PurchaseOrderItems");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseOrderItems_TenantId_Id",
                schema: "Procurement",
                table: "PurchaseOrderItems");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseInvoices_TenantId",
                schema: "Procurement",
                table: "PurchaseInvoices");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseInvoices_TenantId_Id",
                schema: "Procurement",
                table: "PurchaseInvoices");

            migrationBuilder.DropIndex(
                name: "IX_ProcurementAttachments_TenantId",
                schema: "Procurement",
                table: "ProcurementAttachments");

            migrationBuilder.DropIndex(
                name: "IX_ProcurementAttachments_TenantId_Id",
                schema: "Procurement",
                table: "ProcurementAttachments");

            migrationBuilder.DropIndex(
                name: "IX_GoodsReceipts_TenantId",
                schema: "Procurement",
                table: "GoodsReceipts");

            migrationBuilder.DropIndex(
                name: "IX_GoodsReceipts_TenantId_Id",
                schema: "Procurement",
                table: "GoodsReceipts");

            migrationBuilder.DropIndex(
                name: "IX_GoodsReceiptItems_TenantId",
                schema: "Procurement",
                table: "GoodsReceiptItems");

            migrationBuilder.DropIndex(
                name: "IX_GoodsReceiptItems_TenantId_Id",
                schema: "Procurement",
                table: "GoodsReceiptItems");

            migrationBuilder.DropColumn(
                name: "TenantId",
                schema: "Procurement",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "TenantId",
                schema: "Procurement",
                table: "PurchaseRequisitions");

            migrationBuilder.DropColumn(
                name: "TenantId",
                schema: "Procurement",
                table: "PurchaseOrders");

            migrationBuilder.DropColumn(
                name: "TenantId",
                schema: "Procurement",
                table: "PurchaseOrderItems");

            migrationBuilder.DropColumn(
                name: "TenantId",
                schema: "Procurement",
                table: "PurchaseInvoices");

            migrationBuilder.DropColumn(
                name: "TenantId",
                schema: "Procurement",
                table: "ProcurementAttachments");

            migrationBuilder.DropColumn(
                name: "TenantId",
                schema: "Procurement",
                table: "GoodsReceipts");

            migrationBuilder.DropColumn(
                name: "TenantId",
                schema: "Procurement",
                table: "GoodsReceiptItems");
        }
    }
}
