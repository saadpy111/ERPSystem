using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Website.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class addImagesFroProductAndCategoryInWebsiteModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrlSnapshot",
                schema: "Website",
                table: "WebsiteProducts");

            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                schema: "Website",
                table: "WebsiteCategories",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "WebsiteProductImages",
                schema: "Website",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    AltText = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    IsPrimary = table.Column<bool>(type: "bit", nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    WebsiteProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WebsiteProductImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WebsiteProductImages_WebsiteProducts_WebsiteProductId",
                        column: x => x.WebsiteProductId,
                        principalSchema: "Website",
                        principalTable: "WebsiteProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WebsiteProductImages_TenantId",
                schema: "Website",
                table: "WebsiteProductImages",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_WebsiteProductImages_WebsiteProductId",
                schema: "Website",
                table: "WebsiteProductImages",
                column: "WebsiteProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WebsiteProductImages",
                schema: "Website");

            migrationBuilder.DropColumn(
                name: "ImagePath",
                schema: "Website",
                table: "WebsiteCategories");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrlSnapshot",
                schema: "Website",
                table: "WebsiteProducts",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
