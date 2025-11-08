using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hr.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class addmig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Attachments_EntityType_EntityId",
                schema: "Hr",
                table: "Attachments",
                columns: new[] { "EntityType", "EntityId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Attachments_EntityType_EntityId",
                schema: "Hr",
                table: "Attachments");
        }
    }
}
