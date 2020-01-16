using Microsoft.EntityFrameworkCore.Migrations;

namespace MosMap_API.Migrations
{
    public partial class fixedUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Locations_Users_UserId",
                table: "Locations");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Authorizations_AuthorizationId",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "ApplicationUser");

            migrationBuilder.RenameIndex(
                name: "IX_Users_AuthorizationId",
                table: "ApplicationUser",
                newName: "IX_ApplicationUser_AuthorizationId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ApplicationUser",
                table: "ApplicationUser",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUser_Authorizations_AuthorizationId",
                table: "ApplicationUser",
                column: "AuthorizationId",
                principalTable: "Authorizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Locations_ApplicationUser_UserId",
                table: "Locations",
                column: "UserId",
                principalTable: "ApplicationUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUser_Authorizations_AuthorizationId",
                table: "ApplicationUser");

            migrationBuilder.DropForeignKey(
                name: "FK_Locations_ApplicationUser_UserId",
                table: "Locations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ApplicationUser",
                table: "ApplicationUser");

            migrationBuilder.RenameTable(
                name: "ApplicationUser",
                newName: "Users");

            migrationBuilder.RenameIndex(
                name: "IX_ApplicationUser_AuthorizationId",
                table: "Users",
                newName: "IX_Users_AuthorizationId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Locations_Users_UserId",
                table: "Locations",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Authorizations_AuthorizationId",
                table: "Users",
                column: "AuthorizationId",
                principalTable: "Authorizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
