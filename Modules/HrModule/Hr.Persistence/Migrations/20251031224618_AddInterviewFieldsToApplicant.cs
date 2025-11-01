using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hr.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddInterviewFieldsToApplicant : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                schema: "Hr",
                table: "RecruitmentStages",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "InterviewDate",
                schema: "Hr",
                table: "Applicants",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InterviewNotes",
                schema: "Hr",
                table: "Applicants",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                schema: "Hr",
                table: "RecruitmentStages");

            migrationBuilder.DropColumn(
                name: "InterviewDate",
                schema: "Hr",
                table: "Applicants");

            migrationBuilder.DropColumn(
                name: "InterviewNotes",
                schema: "Hr",
                table: "Applicants");
        }
    }
}
