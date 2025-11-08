using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hr.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class addmanagertoDepartment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Departments_DepartmentId",
                schema: "Hr",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Employees_DepartmentId",
                schema: "Hr",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "DepartmentId",
                schema: "Hr",
                table: "Employees");

            migrationBuilder.AddColumn<int>(
                name: "ManagerId",
                schema: "Hr",
                table: "Departments",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Departments_ManagerId",
                schema: "Hr",
                table: "Departments",
                column: "ManagerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Departments_Employees_ManagerId",
                schema: "Hr",
                table: "Departments",
                column: "ManagerId",
                principalSchema: "Hr",
                principalTable: "Employees",
                principalColumn: "EmployeeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Departments_Employees_ManagerId",
                schema: "Hr",
                table: "Departments");

            migrationBuilder.DropIndex(
                name: "IX_Departments_ManagerId",
                schema: "Hr",
                table: "Departments");

            migrationBuilder.DropColumn(
                name: "ManagerId",
                schema: "Hr",
                table: "Departments");

            migrationBuilder.AddColumn<int>(
                name: "DepartmentId",
                schema: "Hr",
                table: "Employees",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employees_DepartmentId",
                schema: "Hr",
                table: "Employees",
                column: "DepartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Departments_DepartmentId",
                schema: "Hr",
                table: "Employees",
                column: "DepartmentId",
                principalSchema: "Hr",
                principalTable: "Departments",
                principalColumn: "DepartmentId");
        }
    }
}
