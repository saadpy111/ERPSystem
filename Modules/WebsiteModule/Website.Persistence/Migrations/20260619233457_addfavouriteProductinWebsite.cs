using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Website.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class addfavouriteProductinWebsite : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FavoriteProducts",
                schema: "Website",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FavoriteProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FavoriteProducts_WebsiteProducts_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "Website",
                        principalTable: "WebsiteProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteProducts_ProductId",
                schema: "Website",
                table: "FavoriteProducts",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteProducts_TenantId",
                schema: "Website",
                table: "FavoriteProducts",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteProducts_TenantId_UserId",
                schema: "Website",
                table: "FavoriteProducts",
                columns: new[] { "TenantId", "UserId" });

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteProducts_UserId_ProductId_TenantId",
                schema: "Website",
                table: "FavoriteProducts",
                columns: new[] { "UserId", "ProductId", "TenantId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FavoriteProducts",
                schema: "Website");
        }
    }
}
