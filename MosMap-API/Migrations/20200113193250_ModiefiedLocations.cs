using Microsoft.EntityFrameworkCore.Migrations;

namespace MosMap_API.Migrations
{
    public partial class ModiefiedLocations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShowUserSuggestedLocation",
                table: "Locations");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Locations",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "LocationChecked",
                table: "Locations",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ShowLocation",
                table: "Locations",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "LocationChecked",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "ShowLocation",
                table: "Locations");

            migrationBuilder.AddColumn<bool>(
                name: "ShowUserSuggestedLocation",
                table: "Locations",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }
    }
}
