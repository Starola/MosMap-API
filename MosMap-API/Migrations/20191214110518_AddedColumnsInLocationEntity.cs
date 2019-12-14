using Microsoft.EntityFrameworkCore.Migrations;

namespace MosMap_API.Migrations
{
    public partial class AddedColumnsInLocationEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ShowUserSuggestedLocation",
                table: "Locations",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "UserSuggestedLocation",
                table: "Locations",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShowUserSuggestedLocation",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "UserSuggestedLocation",
                table: "Locations");
        }
    }
}
