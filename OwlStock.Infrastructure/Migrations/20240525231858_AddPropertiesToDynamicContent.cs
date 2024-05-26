using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OwlStock.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPropertiesToDynamicContent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedById",
                table: "DynamicContents",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "DynamicContents",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "DeletedById",
                table: "DynamicContents",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "DynamicContents",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EditedById",
                table: "DynamicContents",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EditedOn",
                table: "DynamicContents",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ReadingTime",
                table: "DynamicContents",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_DynamicContents_CreatedById",
                table: "DynamicContents",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_DynamicContents_DeletedById",
                table: "DynamicContents",
                column: "DeletedById");

            migrationBuilder.CreateIndex(
                name: "IX_DynamicContents_EditedById",
                table: "DynamicContents",
                column: "EditedById");

            migrationBuilder.AddForeignKey(
                name: "FK_DynamicContents_AspNetUsers_CreatedById",
                table: "DynamicContents",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DynamicContents_AspNetUsers_DeletedById",
                table: "DynamicContents",
                column: "DeletedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DynamicContents_AspNetUsers_EditedById",
                table: "DynamicContents",
                column: "EditedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DynamicContents_AspNetUsers_CreatedById",
                table: "DynamicContents");

            migrationBuilder.DropForeignKey(
                name: "FK_DynamicContents_AspNetUsers_DeletedById",
                table: "DynamicContents");

            migrationBuilder.DropForeignKey(
                name: "FK_DynamicContents_AspNetUsers_EditedById",
                table: "DynamicContents");

            migrationBuilder.DropIndex(
                name: "IX_DynamicContents_CreatedById",
                table: "DynamicContents");

            migrationBuilder.DropIndex(
                name: "IX_DynamicContents_DeletedById",
                table: "DynamicContents");

            migrationBuilder.DropIndex(
                name: "IX_DynamicContents_EditedById",
                table: "DynamicContents");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "DynamicContents");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "DynamicContents");

            migrationBuilder.DropColumn(
                name: "DeletedById",
                table: "DynamicContents");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "DynamicContents");

            migrationBuilder.DropColumn(
                name: "EditedById",
                table: "DynamicContents");

            migrationBuilder.DropColumn(
                name: "EditedOn",
                table: "DynamicContents");

            migrationBuilder.DropColumn(
                name: "ReadingTime",
                table: "DynamicContents");
        }
    }
}
