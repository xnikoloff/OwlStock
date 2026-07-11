using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OwlStock.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddStatusToGiftCard : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsUsed",
                table: "GiftCards");

            migrationBuilder.DropColumn(
                name: "IsValidated",
                table: "GiftCards");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "GiftCards",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "GiftCards");

            migrationBuilder.AddColumn<bool>(
                name: "IsUsed",
                table: "GiftCards",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsValidated",
                table: "GiftCards",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
