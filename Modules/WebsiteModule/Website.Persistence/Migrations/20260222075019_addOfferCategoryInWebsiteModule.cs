using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Website.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class addOfferCategoryInWebsiteModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Priority",
                schema: "Website",
                table: "Offers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "OfferCategories",
                schema: "Website",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OfferId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OfferCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OfferCategories_Offers_OfferId",
                        column: x => x.OfferId,
                        principalSchema: "Website",
                        principalTable: "Offers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OfferCategories_WebsiteCategories_CategoryId",
                        column: x => x.CategoryId,
                        principalSchema: "Website",
                        principalTable: "WebsiteCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OfferCategories_CategoryId",
                schema: "Website",
                table: "OfferCategories",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_OfferCategories_OfferId",
                schema: "Website",
                table: "OfferCategories",
                column: "OfferId");

            migrationBuilder.CreateIndex(
                name: "IX_OfferCategories_TenantId",
                schema: "Website",
                table: "OfferCategories",
                column: "TenantId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OfferCategories",
                schema: "Website");

            migrationBuilder.DropColumn(
                name: "Priority",
                schema: "Website",
                table: "Offers");
        }
    }
}
