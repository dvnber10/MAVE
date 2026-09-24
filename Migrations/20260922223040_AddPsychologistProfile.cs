using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MAVE.Migrations
{
    /// <inheritdoc />
    public partial class AddPsychologistProfile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PSYCHOLOGIST_PROFILE",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CredentialUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Verified = table.Column<bool>(type: "bit", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PSYCHOLOGIST_PROFILE", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_PSYPROFILE_USER",
                        column: x => x.UserId,
                        principalTable: "USER",
                        principalColumn: "UserId");
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PSYCHOLOGIST_PROFILE");
        }
    }
}
