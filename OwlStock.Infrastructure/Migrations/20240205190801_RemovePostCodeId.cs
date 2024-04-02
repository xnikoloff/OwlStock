using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OwlStock.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemovePostCodeId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cities_PostCodes_PostCodeId",
                table: "Cities");

            migrationBuilder.DropIndex(
                name: "IX_Cities_PostCodeId",
                table: "Cities");

            migrationBuilder.DropColumn(
                name: "PostCodeId",
                table: "Cities");

            migrationBuilder.AddColumn<string>(
                name: "PostCode",
                table: "Cities",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PostCode",
                table: "Cities");

            migrationBuilder.AddColumn<int>(
                name: "PostCodeId",
                table: "Cities",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cities_PostCodeId",
                table: "Cities",
                column: "PostCodeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cities_PostCodes_PostCodeId",
                table: "Cities",
                column: "PostCodeId",
                principalTable: "PostCodes",
                principalColumn: "Id");
        }
    }
}
