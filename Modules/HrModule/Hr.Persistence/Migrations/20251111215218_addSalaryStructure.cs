using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hr.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class addSalaryStructure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SalaryStructureId",
                schema: "Hr",
                table: "EmployeeContracts",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "SalaryStructures",
                schema: "Hr",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    EffectiveDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalaryStructures", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SalaryStructureComponents",
                schema: "Hr",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SalaryStructureId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FixedAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Percentage = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalaryStructureComponents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SalaryStructureComponents_SalaryStructures_SalaryStructureId",
                        column: x => x.SalaryStructureId,
                        principalSchema: "Hr",
                        principalTable: "SalaryStructures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeContracts_SalaryStructureId",
                schema: "Hr",
                table: "EmployeeContracts",
                column: "SalaryStructureId");

            migrationBuilder.CreateIndex(
                name: "IX_SalaryStructureComponents_SalaryStructureId",
                schema: "Hr",
                table: "SalaryStructureComponents",
                column: "SalaryStructureId");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeContracts_SalaryStructures_SalaryStructureId",
                schema: "Hr",
                table: "EmployeeContracts",
                column: "SalaryStructureId",
                principalSchema: "Hr",
                principalTable: "SalaryStructures",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeContracts_SalaryStructures_SalaryStructureId",
                schema: "Hr",
                table: "EmployeeContracts");

            migrationBuilder.DropTable(
                name: "SalaryStructureComponents",
                schema: "Hr");

            migrationBuilder.DropTable(
                name: "SalaryStructures",
                schema: "Hr");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeContracts_SalaryStructureId",
                schema: "Hr",
                table: "EmployeeContracts");

            migrationBuilder.DropColumn(
                name: "SalaryStructureId",
                schema: "Hr",
                table: "EmployeeContracts");
        }
    }
}
