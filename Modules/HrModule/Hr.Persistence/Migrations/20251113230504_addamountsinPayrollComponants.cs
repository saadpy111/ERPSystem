using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hr.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class addamountsinPayrollComponants : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeContracts_SalaryStructures_SalaryStructureId",
                schema: "Hr",
                table: "EmployeeContracts");

            migrationBuilder.RenameColumn(
                name: "Amount",
                schema: "Hr",
                table: "PayrollComponents",
                newName: "FixedAmount");

            migrationBuilder.AddColumn<decimal>(
                name: "Percentage",
                schema: "Hr",
                table: "PayrollComponents",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeContracts_SalaryStructures_SalaryStructureId",
                schema: "Hr",
                table: "EmployeeContracts",
                column: "SalaryStructureId",
                principalSchema: "Hr",
                principalTable: "SalaryStructures",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeContracts_SalaryStructures_SalaryStructureId",
                schema: "Hr",
                table: "EmployeeContracts");

            migrationBuilder.DropColumn(
                name: "Percentage",
                schema: "Hr",
                table: "PayrollComponents");

            migrationBuilder.RenameColumn(
                name: "FixedAmount",
                schema: "Hr",
                table: "PayrollComponents",
                newName: "Amount");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeContracts_SalaryStructures_SalaryStructureId",
                schema: "Hr",
                table: "EmployeeContracts",
                column: "SalaryStructureId",
                principalSchema: "Hr",
                principalTable: "SalaryStructures",
                principalColumn: "Id");
        }
    }
}
