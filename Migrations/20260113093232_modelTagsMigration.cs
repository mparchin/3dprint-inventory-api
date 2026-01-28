using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _3dprint_inventory_api.Migrations
{
    /// <inheritdoc />
    public partial class modelTagsMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ModelTag");

            migrationBuilder.CreateTable(
                name: "ModelTags",
                columns: table => new
                {
                    ModelId = table.Column<int>(type: "INTEGER", nullable: false),
                    TagId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModelTags", x => new { x.ModelId, x.TagId });
                    table.ForeignKey(
                        name: "FK_ModelTags_Models_ModelId",
                        column: x => x.ModelId,
                        principalTable: "Models",
                        principalColumn: "ModelId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ModelTags_Tags_TagId",
                        column: x => x.TagId,
                        principalTable: "Tags",
                        principalColumn: "TagId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ModelTags_ModelId_TagId",
                table: "ModelTags",
                columns: new[] { "ModelId", "TagId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ModelTags_TagId",
                table: "ModelTags",
                column: "TagId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ModelTags");

            migrationBuilder.CreateTable(
                name: "ModelTag",
                columns: table => new
                {
                    ModelsModelId = table.Column<int>(type: "INTEGER", nullable: false),
                    TagsTagId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModelTag", x => new { x.ModelsModelId, x.TagsTagId });
                    table.ForeignKey(
                        name: "FK_ModelTag_Models_ModelsModelId",
                        column: x => x.ModelsModelId,
                        principalTable: "Models",
                        principalColumn: "ModelId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ModelTag_Tags_TagsTagId",
                        column: x => x.TagsTagId,
                        principalTable: "Tags",
                        principalColumn: "TagId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ModelTag_TagsTagId",
                table: "ModelTag",
                column: "TagsTagId");
        }
    }
}
