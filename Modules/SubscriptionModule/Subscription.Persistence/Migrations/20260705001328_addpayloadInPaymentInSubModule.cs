using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Subscription.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class addpayloadInPaymentInSubModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "TenantId",
                schema: "Subscription",
                table: "Payments",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AddColumn<string>(
                name: "Payload",
                schema: "Subscription",
                table: "Payments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                schema: "Subscription",
                table: "Payments",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_UserId_Purpose_TargetId_Status",
                schema: "Subscription",
                table: "Payments",
                columns: new[] { "UserId", "Purpose", "TargetId", "Status" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Payments_UserId_Purpose_TargetId_Status",
                schema: "Subscription",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "Payload",
                schema: "Subscription",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "UserId",
                schema: "Subscription",
                table: "Payments");

            migrationBuilder.AlterColumn<string>(
                name: "TenantId",
                schema: "Subscription",
                table: "Payments",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);
        }
    }
}
