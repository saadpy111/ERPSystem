using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hr.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class addtestmig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attachments_Employees_EmployeeId",
                schema: "Hr",
                table: "Attachments");

            migrationBuilder.DropIndex(
                name: "IX_Attachments_EmployeeId",
                schema: "Hr",
                table: "Attachments");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                schema: "Hr",
                table: "Attachments");

            migrationBuilder.AddColumn<int>(
                name: "ProbationPeriod",
                schema: "Hr",
                table: "EmployeeContracts",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProbationPeriod",
                schema: "Hr",
                table: "EmployeeContracts");

            migrationBuilder.AddColumn<int>(
                name: "EmployeeId",
                schema: "Hr",
                table: "Attachments",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Attachments_EmployeeId",
                schema: "Hr",
                table: "Attachments",
                column: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Attachments_Employees_EmployeeId",
                schema: "Hr",
                table: "Attachments",
                column: "EmployeeId",
                principalSchema: "Hr",
                principalTable: "Employees",
                principalColumn: "EmployeeId");
        }
    }
}
