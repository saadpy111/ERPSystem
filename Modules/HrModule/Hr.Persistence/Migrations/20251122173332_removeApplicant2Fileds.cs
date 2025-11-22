using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hr.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class removeApplicant2Fileds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EducationalQualifications",
                schema: "Hr",
                table: "Applicants");

            migrationBuilder.DropColumn(
                name: "ExperienceDetails",
                schema: "Hr",
                table: "Applicants");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EducationalQualifications",
                schema: "Hr",
                table: "Applicants",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExperienceDetails",
                schema: "Hr",
                table: "Applicants",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
