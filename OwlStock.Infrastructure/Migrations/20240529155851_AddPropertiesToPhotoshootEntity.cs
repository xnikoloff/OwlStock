using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OwlStock.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPropertiesToPhotoshootEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "DoNotUploadPhotos",
                table: "PhotoShoots",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "PhotoDeliveryMethod",
                table: "PhotoShoots",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DoNotUploadPhotos",
                table: "PhotoShoots");

            migrationBuilder.DropColumn(
                name: "PhotoDeliveryMethod",
                table: "PhotoShoots");
        }
    }
}
