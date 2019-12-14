using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MosMap_API.Migrations
{
    public partial class AddedAuthorizationEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AuthorizationId",
                table: "Users",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Authorizations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Admin = table.Column<bool>(nullable: false),
                    Council = table.Column<bool>(nullable: false),
                    User = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Authorizations", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_AuthorizationId",
                table: "Users",
                column: "AuthorizationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Authorizations_AuthorizationId",
                table: "Users",
                column: "AuthorizationId",
                principalTable: "Authorizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Authorizations_AuthorizationId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "Authorizations");

            migrationBuilder.DropIndex(
                name: "IX_Users_AuthorizationId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "AuthorizationId",
                table: "Users");
        }
    }
}
