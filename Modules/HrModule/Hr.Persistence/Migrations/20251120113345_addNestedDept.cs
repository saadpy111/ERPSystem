using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hr.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class addNestedDept : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ParentDepartmentId",
                schema: "Hr",
                table: "Departments",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Departments_ParentDepartmentId",
                schema: "Hr",
                table: "Departments",
                column: "ParentDepartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Departments_Departments_ParentDepartmentId",
                schema: "Hr",
                table: "Departments",
                column: "ParentDepartmentId",
                principalSchema: "Hr",
                principalTable: "Departments",
                principalColumn: "DepartmentId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Departments_Departments_ParentDepartmentId",
                schema: "Hr",
                table: "Departments");

            migrationBuilder.DropIndex(
                name: "IX_Departments_ParentDepartmentId",
                schema: "Hr",
                table: "Departments");

            migrationBuilder.DropColumn(
                name: "ParentDepartmentId",
                schema: "Hr",
                table: "Departments");
        }
    }
}
