using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MAVE.Migrations
{
    /// <inheritdoc />
    public partial class AddChatMessage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CHAT_MESSAGE",
                columns: table => new
                {
                    MessageId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SenderId = table.Column<int>(type: "int", nullable: false),
                    ReceiverId = table.Column<int>(type: "int", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Date = table.Column<DateTime>(type: "datetime", nullable: false),
                    Read = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CHAT_MESSAGE", x => x.MessageId);
                    table.ForeignKey(
                        name: "FK_CHAT_RECEIVER",
                        column: x => x.ReceiverId,
                        principalTable: "USER",
                        principalColumn: "UserId");
                    table.ForeignKey(
                        name: "FK_CHAT_SENDER",
                        column: x => x.SenderId,
                        principalTable: "USER",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CHAT_MESSAGE_ReceiverId",
                table: "CHAT_MESSAGE",
                column: "ReceiverId");

            migrationBuilder.CreateIndex(
                name: "IX_CHAT_MESSAGE_SenderId",
                table: "CHAT_MESSAGE",
                column: "SenderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CHAT_MESSAGE");
        }
    }
}
