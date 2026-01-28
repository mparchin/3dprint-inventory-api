using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _3dprint_inventory_api.Migrations
{
    /// <inheritdoc />
    public partial class SpoolsWeightMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "FilamentWeight",
                table: "Spools",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FilamentWeight",
                table: "Spools");
        }
    }
}
