using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OwlStock.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPlace : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PhotoShoots_Cities_CityId",
                table: "PhotoShoots");

            migrationBuilder.DropIndex(
                name: "IX_PhotoShoots_CityId",
                table: "PhotoShoots");

            migrationBuilder.DropColumn(
                name: "GoogleMapsLink",
                table: "PhotoShoots");

            migrationBuilder.DropColumn(
                name: "UserPlace",
                table: "PhotoShoots");

            migrationBuilder.AddColumn<Guid>(
                name: "PlaceId",
                table: "PhotoShoots",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Places",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GoogleMapsURL = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImagePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsPopular = table.Column<bool>(type: "bit", nullable: false),
                    CityId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Places", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Places_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PhotoShoots_PlaceId",
                table: "PhotoShoots",
                column: "PlaceId");

            migrationBuilder.CreateIndex(
                name: "IX_Places_CityId",
                table: "Places",
                column: "CityId");

            migrationBuilder.AddForeignKey(
                name: "FK_PhotoShoots_Places_PlaceId",
                table: "PhotoShoots",
                column: "PlaceId",
                principalTable: "Places",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PhotoShoots_Places_PlaceId",
                table: "PhotoShoots");

            migrationBuilder.DropTable(
                name: "Places");

            migrationBuilder.DropIndex(
                name: "IX_PhotoShoots_PlaceId",
                table: "PhotoShoots");

            migrationBuilder.DropColumn(
                name: "PlaceId",
                table: "PhotoShoots");

            migrationBuilder.AddColumn<string>(
                name: "GoogleMapsLink",
                table: "PhotoShoots",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserPlace",
                table: "PhotoShoots",
                type: "nvarchar(max)",
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
    }
}
