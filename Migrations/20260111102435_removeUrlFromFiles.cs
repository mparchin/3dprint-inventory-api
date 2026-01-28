using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _3dprint_inventory_api.Migrations
{
    /// <inheritdoc />
    public partial class removeUrlFromFiles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Url",
                table: "Files");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Url",
                table: "Files",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }
    }
}
