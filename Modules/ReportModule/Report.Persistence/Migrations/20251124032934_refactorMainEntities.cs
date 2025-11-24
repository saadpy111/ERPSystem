using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Report.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class refactorMainEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReportDataSources_Reports_ReportId",
                schema: "Report",
                table: "ReportDataSources");

            migrationBuilder.DropIndex(
                name: "IX_ReportDataSources_ReportId",
                schema: "Report",
                table: "ReportDataSources");

            migrationBuilder.DropColumn(
                name: "FieldName",
                schema: "Report",
                table: "ReportSortings");

            migrationBuilder.DropColumn(
                name: "FieldName",
                schema: "Report",
                table: "ReportGroups");

            migrationBuilder.DropColumn(
                name: "Value",
                schema: "Report",
                table: "ReportFilters");

            migrationBuilder.DropColumn(
                name: "ReportId",
                schema: "Report",
                table: "ReportDataSources");

            migrationBuilder.RenameColumn(
                name: "SortingId",
                schema: "Report",
                table: "ReportSortings",
                newName: "ReportSortingId");

            migrationBuilder.RenameColumn(
                name: "ParameterId",
                schema: "Report",
                table: "ReportParameters",
                newName: "ReportParameterId");

            migrationBuilder.RenameColumn(
                name: "GroupId",
                schema: "Report",
                table: "ReportGroups",
                newName: "ReportGroupId");

            migrationBuilder.RenameColumn(
                name: "FilterId",
                schema: "Report",
                table: "ReportFilters",
                newName: "ReportFilterId");

            migrationBuilder.RenameColumn(
                name: "FieldId",
                schema: "Report",
                table: "ReportFields",
                newName: "ReportFieldId");

            migrationBuilder.RenameColumn(
                name: "DataSourceId",
                schema: "Report",
                table: "ReportDataSources",
                newName: "ReportDataSourceId");

            migrationBuilder.AlterColumn<int>(
                name: "Direction",
                schema: "Report",
                table: "ReportSortings",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "Expression",
                schema: "Report",
                table: "ReportSortings",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                schema: "Report",
                table: "Reports",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<string>(
                name: "DefaultValue",
                schema: "Report",
                table: "ReportParameters",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Expression",
                schema: "Report",
                table: "ReportGroups",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ParameterName",
                schema: "Report",
                table: "ReportFilters",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Expression",
                schema: "Report",
                table: "ReportFields",
                type: "nvarchar(max)",
                maxLength: 5000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SqlTemplate",
                schema: "Report",
                table: "ReportDataSources",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Expression",
                schema: "Report",
                table: "ReportSortings");

            migrationBuilder.DropColumn(
                name: "Expression",
                schema: "Report",
                table: "ReportGroups");

            migrationBuilder.DropColumn(
                name: "ParameterName",
                schema: "Report",
                table: "ReportFilters");

            migrationBuilder.DropColumn(
                name: "Expression",
                schema: "Report",
                table: "ReportFields");

            migrationBuilder.DropColumn(
                name: "SqlTemplate",
                schema: "Report",
                table: "ReportDataSources");

            migrationBuilder.RenameColumn(
                name: "ReportSortingId",
                schema: "Report",
                table: "ReportSortings",
                newName: "SortingId");

            migrationBuilder.RenameColumn(
                name: "ReportParameterId",
                schema: "Report",
                table: "ReportParameters",
                newName: "ParameterId");

            migrationBuilder.RenameColumn(
                name: "ReportGroupId",
                schema: "Report",
                table: "ReportGroups",
                newName: "GroupId");

            migrationBuilder.RenameColumn(
                name: "ReportFilterId",
                schema: "Report",
                table: "ReportFilters",
                newName: "FilterId");

            migrationBuilder.RenameColumn(
                name: "ReportFieldId",
                schema: "Report",
                table: "ReportFields",
                newName: "FieldId");

            migrationBuilder.RenameColumn(
                name: "ReportDataSourceId",
                schema: "Report",
                table: "ReportDataSources",
                newName: "DataSourceId");

            migrationBuilder.AlterColumn<int>(
                name: "Direction",
                schema: "Report",
                table: "ReportSortings",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "FieldName",
                schema: "Report",
                table: "ReportSortings",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                schema: "Report",
                table: "Reports",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<string>(
                name: "DefaultValue",
                schema: "Report",
                table: "ReportParameters",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FieldName",
                schema: "Report",
                table: "ReportGroups",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Value",
                schema: "Report",
                table: "ReportFilters",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ReportId",
                schema: "Report",
                table: "ReportDataSources",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ReportDataSources_ReportId",
                schema: "Report",
                table: "ReportDataSources",
                column: "ReportId");

            migrationBuilder.AddForeignKey(
                name: "FK_ReportDataSources_Reports_ReportId",
                schema: "Report",
                table: "ReportDataSources",
                column: "ReportId",
                principalSchema: "Report",
                principalTable: "Reports",
                principalColumn: "ReportId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
