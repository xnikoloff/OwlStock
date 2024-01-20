using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OwlStock.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPropertiesToPhotoBaseAndPhotoShootPhoto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileData",
                table: "PhotosBase");

            migrationBuilder.AddColumn<bool>(
                name: "IsPublic",
                table: "PhotoShootPhotos",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDownloadable",
                table: "PhotosBase",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPublic",
                table: "PhotoShootPhotos");

            migrationBuilder.DropColumn(
                name: "IsDownloadable",
                table: "PhotosBase");

            migrationBuilder.AddColumn<byte[]>(
                name: "FileData",
                table: "PhotosBase",
                type: "varbinary(max)",
                nullable: true);
        }
    }
}
