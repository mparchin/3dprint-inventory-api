using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _3dprint_inventory_api.Migrations
{
    /// <inheritdoc />
    public partial class SpoolsMultiColorMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ColorHex",
                table: "Spools",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AddColumn<string>(
                name: "MultiColorHexes",
                table: "Spools",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MultiColorHexes",
                table: "Spools");

            migrationBuilder.AlterColumn<string>(
                name: "ColorHex",
                table: "Spools",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);
        }
    }
}
