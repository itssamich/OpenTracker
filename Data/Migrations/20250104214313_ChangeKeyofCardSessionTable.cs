using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OpenTracker.Migrations
{
    /// <inheritdoc />
    public partial class ChangeKeyofCardSessionTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_CardSessions",
                table: "CardSessions");

            migrationBuilder.AddColumn<int>(
                name: "CardSessionId",
                table: "CardSessions",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0)
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_CardSessions",
                table: "CardSessions",
                column: "CardSessionId");

            migrationBuilder.CreateIndex(
                name: "IX_CardSessions_CardId",
                table: "CardSessions",
                column: "CardId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_CardSessions",
                table: "CardSessions");

            migrationBuilder.DropIndex(
                name: "IX_CardSessions_CardId",
                table: "CardSessions");

            migrationBuilder.DropColumn(
                name: "CardSessionId",
                table: "CardSessions");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CardSessions",
                table: "CardSessions",
                columns: new[] { "CardId", "SessionId" });
        }
    }
}
