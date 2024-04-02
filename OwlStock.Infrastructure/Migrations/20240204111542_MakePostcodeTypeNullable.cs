using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OwlStock.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MakePostcodeTypeNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cities_PostCodes_PostCodeId",
                table: "Cities");

            migrationBuilder.AlterColumn<int>(
                name: "PostCodeId",
                table: "Cities",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Cities_PostCodes_PostCodeId",
                table: "Cities",
                column: "PostCodeId",
                principalTable: "PostCodes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cities_PostCodes_PostCodeId",
                table: "Cities");

            migrationBuilder.AlterColumn<int>(
                name: "PostCodeId",
                table: "Cities",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Cities_PostCodes_PostCodeId",
                table: "Cities",
                column: "PostCodeId",
                principalTable: "PostCodes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
