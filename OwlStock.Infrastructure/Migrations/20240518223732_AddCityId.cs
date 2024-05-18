using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OwlStock.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCityId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CityId",
                table: "PhotoShoots",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PhotoShoots_CityId",
                table: "PhotoShoots",
                column: "CityId");

            migrationBuilder.AddForeignKey(
                name: "FK_PhotoShoots_Cities_CityId",
                table: "PhotoShoots",
                column: "CityId",
                principalTable: "Cities",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PhotoShoots_Cities_CityId",
                table: "PhotoShoots");

            migrationBuilder.DropIndex(
                name: "IX_PhotoShoots_CityId",
                table: "PhotoShoots");

            migrationBuilder.DropColumn(
                name: "CityId",
                table: "PhotoShoots");
        }
    }
}
