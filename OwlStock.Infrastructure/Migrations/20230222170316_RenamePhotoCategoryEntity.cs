using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OwlStock.Infrastructure.Migrations
{
    public partial class RenamePhotoCategoryEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PhotoCategory_Photos_PhotoId",
                table: "PhotoCategory");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PhotoCategory",
                table: "PhotoCategory");

            migrationBuilder.RenameTable(
                name: "PhotoCategory",
                newName: "PhotosCategories");

            migrationBuilder.RenameIndex(
                name: "IX_PhotoCategory_PhotoId",
                table: "PhotosCategories",
                newName: "IX_PhotosCategories_PhotoId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PhotosCategories",
                table: "PhotosCategories",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PhotosCategories_Photos_PhotoId",
                table: "PhotosCategories",
                column: "PhotoId",
                principalTable: "Photos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PhotosCategories_Photos_PhotoId",
                table: "PhotosCategories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PhotosCategories",
                table: "PhotosCategories");

            migrationBuilder.RenameTable(
                name: "PhotosCategories",
                newName: "PhotoCategory");

            migrationBuilder.RenameIndex(
                name: "IX_PhotosCategories_PhotoId",
                table: "PhotoCategory",
                newName: "IX_PhotoCategory_PhotoId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PhotoCategory",
                table: "PhotoCategory",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PhotoCategory_Photos_PhotoId",
                table: "PhotoCategory",
                column: "PhotoId",
                principalTable: "Photos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
