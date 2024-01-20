using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OwlStock.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddIsDonwloadablePropToGalleryPhoto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDownloadable",
                table: "PhotosBase");

            migrationBuilder.AddColumn<bool>(
                name: "IsDownloadable",
                table: "GalleryPhotos",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDownloadable",
                table: "GalleryPhotos");

            migrationBuilder.AddColumn<bool>(
                name: "IsDownloadable",
                table: "PhotosBase",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
