using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OwlStock.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeImagePropName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BannerUrl",
                table: "DynamicContents");

            migrationBuilder.RenameColumn(
                name: "ThumbnailUrl",
                table: "DynamicContents",
                newName: "ImageName");

            migrationBuilder.AddColumn<bool>(
                name: "ShowInTopPosition",
                table: "DynamicContents",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShowInTopPosition",
                table: "DynamicContents");

            migrationBuilder.RenameColumn(
                name: "ImageName",
                table: "DynamicContents",
                newName: "ThumbnailUrl");

            migrationBuilder.AddColumn<int>(
                name: "BannerUrl",
                table: "DynamicContents",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
