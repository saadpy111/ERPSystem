using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hr.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class addLeaveTypeEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LeaveType",
                schema: "Hr",
                table: "LeaveRequests");

            migrationBuilder.AddColumn<int>(
                name: "LeaveTypeId",
                schema: "Hr",
                table: "LeaveRequests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "LeaveTypes",
                schema: "Hr",
                columns: table => new
                {
                    LeaveTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LeaveTypeName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DurationDays = table.Column<int>(type: "int", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeaveTypes", x => x.LeaveTypeId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LeaveRequests_LeaveTypeId",
                schema: "Hr",
                table: "LeaveRequests",
                column: "LeaveTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_LeaveRequests_LeaveTypes_LeaveTypeId",
                schema: "Hr",
                table: "LeaveRequests",
                column: "LeaveTypeId",
                principalSchema: "Hr",
                principalTable: "LeaveTypes",
                principalColumn: "LeaveTypeId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LeaveRequests_LeaveTypes_LeaveTypeId",
                schema: "Hr",
                table: "LeaveRequests");

            migrationBuilder.DropTable(
                name: "LeaveTypes",
                schema: "Hr");

            migrationBuilder.DropIndex(
                name: "IX_LeaveRequests_LeaveTypeId",
                schema: "Hr",
                table: "LeaveRequests");

            migrationBuilder.DropColumn(
                name: "LeaveTypeId",
                schema: "Hr",
                table: "LeaveRequests");

            migrationBuilder.AddColumn<string>(
                name: "LeaveType",
                schema: "Hr",
                table: "LeaveRequests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
