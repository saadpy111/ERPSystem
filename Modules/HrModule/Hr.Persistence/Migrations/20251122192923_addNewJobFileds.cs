using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hr.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class addNewJobFileds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RequiredExperience",
                schema: "Hr",
                table: "Jobs",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RequiredQualification",
                schema: "Hr",
                table: "Jobs",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RequiredSkills",
                schema: "Hr",
                table: "Jobs",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Responsibilities",
                schema: "Hr",
                table: "Jobs",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RequiredExperience",
                schema: "Hr",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "RequiredQualification",
                schema: "Hr",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "RequiredSkills",
                schema: "Hr",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "Responsibilities",
                schema: "Hr",
                table: "Jobs");
        }
    }
}
