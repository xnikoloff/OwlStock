using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OwlStock.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ReplaceGearStringPropWithGearEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Gear",
                table: "GalleryPhotos");

            migrationBuilder.AddColumn<Guid>(
                name: "GearId",
                table: "GalleryPhotos",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Gear",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CameraBrand = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CameraModel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CameraLens = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AdditionalInformation = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gear", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GalleryPhotos_GearId",
                table: "GalleryPhotos",
                column: "GearId");

            migrationBuilder.AddForeignKey(
                name: "FK_GalleryPhotos_Gear_GearId",
                table: "GalleryPhotos",
                column: "GearId",
                principalTable: "Gear",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GalleryPhotos_Gear_GearId",
                table: "GalleryPhotos");

            migrationBuilder.DropTable(
                name: "Gear");

            migrationBuilder.DropIndex(
                name: "IX_GalleryPhotos_GearId",
                table: "GalleryPhotos");

            migrationBuilder.DropColumn(
                name: "GearId",
                table: "GalleryPhotos");

            migrationBuilder.AddColumn<string>(
                name: "Gear",
                table: "GalleryPhotos",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);
        }
    }
}
