using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inventory.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class addmainsupplier : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MainSupplierId",
                schema: "Inventory",
                table: "Products");

            migrationBuilder.AddColumn<string>(
                name: "MainSupplierName",
                schema: "Inventory",
                table: "Products",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MainSupplierName",
                schema: "Inventory",
                table: "Products");

            migrationBuilder.AddColumn<Guid>(
                name: "MainSupplierId",
                schema: "Inventory",
                table: "Products",
                type: "uniqueidentifier",
                nullable: true);
        }
    }
}
