using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OpenTracker.Migrations
{
    /// <inheritdoc />
    public partial class AddSavedSessionSet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SavedSessionSet",
                table: "Sessions",
                type: "TEXT",
                maxLength: 3,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SavedSessionSet",
                table: "Sessions");
        }
    }
}
