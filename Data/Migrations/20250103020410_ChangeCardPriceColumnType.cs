using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OpenTracker.Migrations
{
    /// <inheritdoc />
    public partial class ChangeCardPriceColumnType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Cards",
                type: "TEXT",
                precision: 8,
                scale: 2,
                nullable: true,
                oldClrType: typeof(double),
                oldType: "REAL",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "Price",
                table: "Cards",
                type: "REAL",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "TEXT",
                oldPrecision: 8,
                oldScale: 2,
                oldNullable: true);
        }
    }
}
