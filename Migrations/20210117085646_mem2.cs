using Microsoft.EntityFrameworkCore.Migrations;

namespace figma.Migrations
{
    public partial class mem2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ConfirmEmail",
                table: "Members",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "token",
                table: "Members",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConfirmEmail",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "token",
                table: "Members");
        }
    }
}
