using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Report.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class initReport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Report");

            migrationBuilder.CreateTable(
                name: "EmployeeReports",
                schema: "Report",
                columns: table => new
                {
                    EmployeeReportId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DepartmentName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    JobTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Salary = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ContractType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ManagerName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HireDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeReports", x => x.EmployeeReportId);
                });

            migrationBuilder.CreateTable(
                name: "ReportDataSources",
                schema: "Report",
                columns: table => new
                {
                    ReportDataSourceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    ConnectionString = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    SqlTemplate = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportDataSources", x => x.ReportDataSourceId);
                });

            migrationBuilder.CreateTable(
                name: "Reports",
                schema: "Report",
                columns: table => new
                {
                    ReportId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Query = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    ReportDataSourceId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reports", x => x.ReportId);
                    table.ForeignKey(
                        name: "FK_Reports_ReportDataSources_ReportDataSourceId",
                        column: x => x.ReportDataSourceId,
                        principalSchema: "Report",
                        principalTable: "ReportDataSources",
                        principalColumn: "ReportDataSourceId");
                });

            migrationBuilder.CreateTable(
                name: "ReportFields",
                schema: "Report",
                columns: table => new
                {
                    ReportFieldId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReportId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Expression = table.Column<string>(type: "nvarchar(max)", maxLength: 5000, nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Width = table.Column<int>(type: "int", nullable: true),
                    IsVisible = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportFields", x => x.ReportFieldId);
                    table.ForeignKey(
                        name: "FK_ReportFields_Reports_ReportId",
                        column: x => x.ReportId,
                        principalSchema: "Report",
                        principalTable: "Reports",
                        principalColumn: "ReportId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReportFilters",
                schema: "Report",
                columns: table => new
                {
                    ReportFilterId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReportId = table.Column<int>(type: "int", nullable: false),
                    FieldName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Operator = table.Column<int>(type: "int", nullable: false),
                    ParameterName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportFilters", x => x.ReportFilterId);
                    table.ForeignKey(
                        name: "FK_ReportFilters_Reports_ReportId",
                        column: x => x.ReportId,
                        principalSchema: "Report",
                        principalTable: "Reports",
                        principalColumn: "ReportId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReportGroups",
                schema: "Report",
                columns: table => new
                {
                    ReportGroupId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReportId = table.Column<int>(type: "int", nullable: false),
                    Expression = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportGroups", x => x.ReportGroupId);
                    table.ForeignKey(
                        name: "FK_ReportGroups_Reports_ReportId",
                        column: x => x.ReportId,
                        principalSchema: "Report",
                        principalTable: "Reports",
                        principalColumn: "ReportId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReportParameters",
                schema: "Report",
                columns: table => new
                {
                    ReportParameterId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReportId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    DataType = table.Column<int>(type: "int", nullable: false),
                    IsRequired = table.Column<bool>(type: "bit", nullable: false),
                    DefaultValue = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportParameters", x => x.ReportParameterId);
                    table.ForeignKey(
                        name: "FK_ReportParameters_Reports_ReportId",
                        column: x => x.ReportId,
                        principalSchema: "Report",
                        principalTable: "Reports",
                        principalColumn: "ReportId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReportSortings",
                schema: "Report",
                columns: table => new
                {
                    ReportSortingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReportId = table.Column<int>(type: "int", nullable: false),
                    Expression = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Direction = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportSortings", x => x.ReportSortingId);
                    table.ForeignKey(
                        name: "FK_ReportSortings_Reports_ReportId",
                        column: x => x.ReportId,
                        principalSchema: "Report",
                        principalTable: "Reports",
                        principalColumn: "ReportId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReportFields_ReportId",
                schema: "Report",
                table: "ReportFields",
                column: "ReportId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportFilters_ReportId",
                schema: "Report",
                table: "ReportFilters",
                column: "ReportId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportGroups_ReportId",
                schema: "Report",
                table: "ReportGroups",
                column: "ReportId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportParameters_ReportId",
                schema: "Report",
                table: "ReportParameters",
                column: "ReportId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_ReportDataSourceId",
                schema: "Report",
                table: "Reports",
                column: "ReportDataSourceId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportSortings_ReportId",
                schema: "Report",
                table: "ReportSortings",
                column: "ReportId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployeeReports",
                schema: "Report");

            migrationBuilder.DropTable(
                name: "ReportFields",
                schema: "Report");

            migrationBuilder.DropTable(
                name: "ReportFilters",
                schema: "Report");

            migrationBuilder.DropTable(
                name: "ReportGroups",
                schema: "Report");

            migrationBuilder.DropTable(
                name: "ReportParameters",
                schema: "Report");

            migrationBuilder.DropTable(
                name: "ReportSortings",
                schema: "Report");

            migrationBuilder.DropTable(
                name: "Reports",
                schema: "Report");

            migrationBuilder.DropTable(
                name: "ReportDataSources",
                schema: "Report");
        }
    }
}
