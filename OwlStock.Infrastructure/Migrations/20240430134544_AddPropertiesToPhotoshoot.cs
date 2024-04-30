using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OwlStock.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPropertiesToPhotoshoot : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GoogleMapsLink",
                table: "PhotoShoots",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDecidedByUs",
                table: "PhotoShoots",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "PhotoShoots",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "UserPlace",
                table: "PhotoShoots",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GoogleMapsLink",
                table: "PhotoShoots");

            migrationBuilder.DropColumn(
                name: "IsDecidedByUs",
                table: "PhotoShoots");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "PhotoShoots");

            migrationBuilder.DropColumn(
                name: "UserPlace",
                table: "PhotoShoots");
        }
    }
}
