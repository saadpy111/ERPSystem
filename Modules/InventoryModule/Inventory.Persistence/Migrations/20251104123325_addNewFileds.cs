using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inventory.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class addNewFileds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "MainSupplierId",
                schema: "Inventory",
                table: "Products",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OrderLimit",
                schema: "Inventory",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProductBarcode",
                schema: "Inventory",
                table: "Products",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Tax",
                schema: "Inventory",
                table: "Products",
                type: "decimal(18,2)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MainSupplierId",
                schema: "Inventory",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "OrderLimit",
                schema: "Inventory",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ProductBarcode",
                schema: "Inventory",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Tax",
                schema: "Inventory",
                table: "Products");
        }
    }
}
