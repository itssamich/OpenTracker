using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OpenTracker.Migrations
{
    /// <inheritdoc />
    public partial class removeDuplicateJoinTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CardSession");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CardSession",
                columns: table => new
                {
                    CardsCardId = table.Column<int>(type: "INTEGER", nullable: false),
                    SessionsSessionId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardSession", x => new { x.CardsCardId, x.SessionsSessionId });
                    table.ForeignKey(
                        name: "FK_CardSession_Cards_CardsCardId",
                        column: x => x.CardsCardId,
                        principalTable: "Cards",
                        principalColumn: "CardId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CardSession_Sessions_SessionsSessionId",
                        column: x => x.SessionsSessionId,
                        principalTable: "Sessions",
                        principalColumn: "SessionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CardSession_SessionsSessionId",
                table: "CardSession",
                column: "SessionsSessionId");
        }
    }
}
