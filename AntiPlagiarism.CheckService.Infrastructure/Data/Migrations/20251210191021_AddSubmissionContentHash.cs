using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AntiPlagiarism.CheckService.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddSubmissionContentHash : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ContentHash",
                table: "Submissions",
                type: "TEXT",
                maxLength: 128,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContentHash",
                table: "Submissions");
        }
    }
}
