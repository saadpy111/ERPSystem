using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hr.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class addnewApplicantFileds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                schema: "Hr",
                table: "Applicants",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfBirth",
                schema: "Hr",
                table: "Applicants",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                schema: "Hr",
                table: "Applicants",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Gender",
                schema: "Hr",
                table: "Applicants",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                schema: "Hr",
                table: "Applicants",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                schema: "Hr",
                table: "Applicants");

            migrationBuilder.DropColumn(
                name: "DateOfBirth",
                schema: "Hr",
                table: "Applicants");

            migrationBuilder.DropColumn(
                name: "Email",
                schema: "Hr",
                table: "Applicants");

            migrationBuilder.DropColumn(
                name: "Gender",
                schema: "Hr",
                table: "Applicants");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                schema: "Hr",
                table: "Applicants");
        }
    }
}
