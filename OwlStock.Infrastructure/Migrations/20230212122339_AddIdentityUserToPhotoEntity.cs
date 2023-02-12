using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OwlStock.Infrastructure.Migrations
{
    public partial class AddIdentityUserToPhotoEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IdentityUserId",
                table: "Photos",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Photos_IdentityUserId",
                table: "Photos",
                column: "IdentityUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Photos_AspNetUsers_IdentityUserId",
                table: "Photos",
                column: "IdentityUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Photos_AspNetUsers_IdentityUserId",
                table: "Photos");

            migrationBuilder.DropIndex(
                name: "IX_Photos_IdentityUserId",
                table: "Photos");

            migrationBuilder.DropColumn(
                name: "IdentityUserId",
                table: "Photos");
        }
    }
}
