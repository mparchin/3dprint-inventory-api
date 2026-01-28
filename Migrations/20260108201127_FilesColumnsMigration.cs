using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _3dprint_inventory_api.Migrations
{
    /// <inheritdoc />
    public partial class FilesColumnsMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Mass",
                table: "Files");

            migrationBuilder.RenameColumn(
                name: "RelativePath",
                table: "Files",
                newName: "Url");

            migrationBuilder.AddColumn<double>(
                name: "AdditionalCostsg",
                table: "Models",
                type: "REAL",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PrintSpecifications",
                table: "Models",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "ValueToCostRatio",
                table: "Models",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "ElectricityCostg",
                table: "Files",
                type: "REAL",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Path",
                table: "Files",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "PrintTime",
                table: "Files",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Repeatations",
                table: "Files",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Weight",
                table: "Files",
                type: "REAL",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdditionalCostsg",
                table: "Models");

            migrationBuilder.DropColumn(
                name: "PrintSpecifications",
                table: "Models");

            migrationBuilder.DropColumn(
                name: "ValueToCostRatio",
                table: "Models");

            migrationBuilder.DropColumn(
                name: "ElectricityCostg",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "Path",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "PrintTime",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "Repeatations",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "Weight",
                table: "Files");

            migrationBuilder.RenameColumn(
                name: "Url",
                table: "Files",
                newName: "RelativePath");

            migrationBuilder.AddColumn<double>(
                name: "Mass",
                table: "Files",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
