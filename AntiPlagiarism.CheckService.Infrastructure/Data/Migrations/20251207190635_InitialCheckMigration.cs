using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AntiPlagiarism.CheckService.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCheckMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Submissions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    StudentId = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    AssignmentId = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    FileId = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    SubmittedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Submissions", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Submissions");
        }
    }
}
